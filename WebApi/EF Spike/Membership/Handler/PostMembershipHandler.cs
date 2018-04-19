using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using EF_Spike.Shared.Handler;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF_Spike.Membership.Handler
{
    public class PostMembershipHandler : IRequestHandler<PostMembership, Model.Membership>
    {
        private readonly RegistryContext context;
        private readonly IMediator mediator;

        public PostMembershipHandler(RegistryContext context, IMediator mediator)
        {
            this.context = context;
            this.mediator = mediator;
        }

        public async Task<Model.Membership> Handle(PostMembership request, CancellationToken cancellationToken)
        {
            using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var eventId = await mediator.Send(new CreateEvent { Event = request.Event }, cancellationToken);

                    var replaceMembershipReference = 0;
                    int? endEventReference = null;
                    var existingEffectiveDate = new DateTime();
                    DateTime? existingNotificationDate = null;
                    var existingMembershipReference = 0;
                    short lessThan2Type = 0;

                    request.Membership.StartEventReference = eventId;

                    if (request.Membership.MembershipReference == 0)
                    {
                        var lessThan2MembersQuery = context.TblMembership.Where(x =>
                                x.Psrnumber == request.Membership.Psrnumber &&
                                x.SectionNumber == request.Membership.SectionNumber &&
                                x.EndEventReference == null &&
                                x.EndDate == null)
                            .Join(context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                e => e.EventReference,
                                (tblMembership, e) => new { e.NotificationDate, Membership = tblMembership });

                        var lessThan2Members = await lessThan2MembersQuery.FirstOrDefaultAsync(cancellationToken);

                        if (request.Membership.LevyTagTypeReference != null)
                        {
                            lessThan2Type = await context.TblLevyTagType
                                .Where(x => x.LevyTagDescription == "Less than 2")
                                .Select(x => x.LevyTagTypeReference).FirstOrDefaultAsync(cancellationToken);

                            lessThan2Members = await lessThan2MembersQuery.FirstOrDefaultAsync(
                                x => x.Membership.LevyTagTypeReference == request.Membership.LevyTagTypeReference,
                                cancellationToken);

                            if (request.Membership.LevyTagTypeReference == lessThan2Type)
                            {
                                lessThan2Members = await lessThan2MembersQuery.FirstOrDefaultAsync(
                                    x => x.Membership.EffectiveDate == request.Membership.EffectiveDate,
                                    cancellationToken);
                            }

                            if (lessThan2Members != null)
                            {
                                existingNotificationDate = lessThan2Members.NotificationDate;
                                existingMembershipReference = lessThan2Members.Membership.MembershipReference;
                                existingEffectiveDate = lessThan2Members.Membership.EffectiveDate;
                            }
                        }
                        else
                        {
                            lessThan2Members = await lessThan2MembersQuery.FirstOrDefaultAsync(
                                x => x.Membership.LevyTagTypeReference == null &&
                                     x.Membership.EffectiveDate == request.Membership.EffectiveDate, cancellationToken);

                            if (lessThan2Members != null)
                            {
                                existingNotificationDate = lessThan2Members.NotificationDate;
                                existingMembershipReference = lessThan2Members.Membership.MembershipReference;
                                existingEffectiveDate = lessThan2Members.Membership.EffectiveDate;
                            }
                        }

                        if (existingNotificationDate <= request.Event.NotificationDate)
                        {
                            replaceMembershipReference = existingMembershipReference;
                        }

                        if (existingNotificationDate > request.Event.NotificationDate)
                        {
                            endEventReference = eventId;
                        }

                        request.Membership.EndEventReference = endEventReference;

                        var memberToAdd = AutoMapper.Mapper.Map<Model.Membership, TblMembership>(request.Membership);
                        await context.AddAsync(memberToAdd, cancellationToken);

                        await context.SaveChangesAsync(cancellationToken);

                        if (request.Membership.LevyTagTypeReference != lessThan2Type ||
                            request.Membership.LevyTagTypeReference == lessThan2Type &&
                            request.Membership.EffectiveDate == existingEffectiveDate)
                        {
                            var member = await context.TblMembership.FirstOrDefaultAsync(
                                x => x.MembershipReference == replaceMembershipReference, cancellationToken);

                            if (member != null)
                            {
                                member.EndEventReference = eventId;
                                context.TblMembership.Update(member);

                                await context.SaveChangesAsync(cancellationToken);
                            }
                        }

                        transaction.Commit();

                        return AutoMapper.Mapper.Map<TblMembership, Model.Membership>(memberToAdd);
                    }
                    else
                    {
                        var lessThan2Members = await context.TblMembership.Where(x =>
                            x.MembershipReference == request.Membership.MembershipReference).Join(
                                context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                e => e.EventReference,
                                (tblMembership, e) => new { e.NotificationDate, Membership = tblMembership }).FirstOrDefaultAsync(cancellationToken);

                        if (lessThan2Members != null)
                        {
                            existingNotificationDate = lessThan2Members.NotificationDate;
                            existingEffectiveDate = lessThan2Members.Membership.EffectiveDate;
                        }

                        if (existingNotificationDate > request.Event.NotificationDate)
                        {
                            endEventReference = eventId;
                        }

                        request.Membership.EndEventReference = endEventReference;

                        var memberToAdd = AutoMapper.Mapper.Map<Model.Membership, TblMembership>(request.Membership);
                        await context.AddAsync(memberToAdd, cancellationToken);

                        await context.SaveChangesAsync(cancellationToken);

                        if (request.Membership.LevyTagTypeReference == null ||
                            request.Membership.LevyTagTypeReference != lessThan2Type)
                        {
                            var lessThan2MembersQuery = context.TblMembership.Where(x =>
                                    x.MembershipReference != memberToAdd.MembershipReference &&
                                    x.Psrnumber == request.Membership.Psrnumber &&
                                    x.SectionNumber == request.Membership.SectionNumber &&
                                    x.LevyTagTypeReference == request.Membership.LevyTagTypeReference &&
                                    x.EndEventReference == null)
                                .Join(context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                    e => e.EventReference,
                                    (tblMembership, e) => new { e.NotificationDate, Membership = tblMembership }).Where(x => x.NotificationDate <= request.Event.NotificationDate);

                            foreach (var item in lessThan2MembersQuery)
                            {
                                item.Membership.EndEventReference = endEventReference;
                            }

                            context.UpdateRange(lessThan2MembersQuery.Select(x => x.Membership));
                            await context.SaveChangesAsync(cancellationToken);

                            lessThan2MembersQuery = context.TblMembership.Where(x =>
                                    x.MembershipReference == memberToAdd.MembershipReference &&
                                    x.Psrnumber == request.Membership.Psrnumber &&
                                    x.SectionNumber == request.Membership.SectionNumber &&
                                    x.LevyTagTypeReference == null &&
                                    x.EndEventReference == null)
                                .Join(context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                    e => e.EventReference,
                                    (tblMembership, e) => new { e.NotificationDate, Membership = tblMembership }).Where(x => x.NotificationDate <= request.Event.NotificationDate);

                            foreach (var item in lessThan2MembersQuery)
                            {
                                item.Membership.EndEventReference = endEventReference;
                            }

                            context.UpdateRange(lessThan2MembersQuery.Select(x => x.Membership));
                            await context.SaveChangesAsync(cancellationToken);
                        }
                        else if (request.Membership.LevyTagTypeReference == lessThan2Type &&
                                 request.Membership.EffectiveDate == existingEffectiveDate)
                        {
                            var lessThan2MembersQuery = context.TblMembership.Where(x =>
                                    x.MembershipReference != memberToAdd.MembershipReference &&
                                    x.Psrnumber == request.Membership.Psrnumber &&
                                    x.SectionNumber == request.Membership.SectionNumber &&
                                    x.LevyTagTypeReference == request.Membership.LevyTagTypeReference &&
                                    x.EndEventReference == null &&
                                    x.EffectiveDate == request.Membership.EffectiveDate)
                                .Join(context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                    e => e.EventReference,
                                    (tblMembership, e) => new { e.NotificationDate, Membership = tblMembership }).Where(x => x.NotificationDate <= request.Event.NotificationDate);

                            foreach (var item in lessThan2MembersQuery)
                            {
                                item.Membership.EndEventReference = endEventReference;
                            }

                            context.UpdateRange(lessThan2MembersQuery.Select(x => x.Membership));
                            await context.SaveChangesAsync(cancellationToken);

                            lessThan2MembersQuery = context.TblMembership.Where(x =>
                                    x.MembershipReference == memberToAdd.MembershipReference &&
                                    x.Psrnumber == request.Membership.Psrnumber &&
                                    x.SectionNumber == request.Membership.SectionNumber &&
                                    x.LevyTagTypeReference == null &&
                                    x.EndEventReference == null &&
                                    x.EffectiveDate == request.Membership.EffectiveDate)
                                .Join(context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                    e => e.EventReference,
                                    (tblMembership, e) => new { e.NotificationDate, Membership = tblMembership }).Where(x => x.NotificationDate <= request.Event.NotificationDate);

                            foreach (var item in lessThan2MembersQuery)
                            {
                                item.Membership.EndEventReference = endEventReference;
                            }

                            context.UpdateRange(lessThan2MembersQuery.Select(x => x.Membership));
                            await context.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    //log this error and do stuff
                }
            }

            return null;
        }
    }
}