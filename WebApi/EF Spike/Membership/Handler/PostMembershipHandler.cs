using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            var membership = context.TblMembership.Where(x => x.Psrnumber == request.Membership.Psrnumber && x.EndDate == null && x.EndEventReference == null);

            using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var eventId = await mediator.Send(new CreateEvent { Event = request.Event }, cancellationToken);

                    int replaceMembershipReference = 0;
                    var endEventReference = 0;
                    DateTime existingEffectiveDate;
                    DateTime? existingNotificationDate = null;
                    int existingMembershipReference;
                    short lessThan2Type = 0;

                    if (request.Membership.MembershipReference == 0)
                    {
                        var lessThan2MembersQuery = context.TblMembership.Where(x =>
                                x.Psrnumber == request.Membership.Psrnumber &&
                                x.SectionNumber == request.Membership.SectionNumber &&
                                x.EndEventReference == null &&
                                x.EndDate == null)
                            .Join(context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                e => e.EventReference,
                                (tblMembership, e) => new {e.NotificationDate, Membership = tblMembership});

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

                            existingNotificationDate = lessThan2Members.NotificationDate;
                            existingMembershipReference = lessThan2Members.Membership.MembershipReference;
                            existingEffectiveDate = lessThan2Members.Membership.EffectiveDate;


                        }
                        else
                        {
                            lessThan2Members = await lessThan2MembersQuery.FirstOrDefaultAsync(
                                x => x.Membership.LevyTagTypeReference == null &&
                                     x.Membership.EffectiveDate == request.Membership.EffectiveDate, cancellationToken);

                            existingNotificationDate = lessThan2Members.NotificationDate;
                            existingMembershipReference = lessThan2Members.Membership.MembershipReference;
                            existingEffectiveDate = lessThan2Members.Membership.EffectiveDate;
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
                            var member = await context.TblMembership.FirstAsync(
                                x => x.MembershipReference == replaceMembershipReference, cancellationToken);

                            context.TblMembership.Update(member);

                            await context.SaveChangesAsync(cancellationToken);
                        }

                        transaction.Commit();

                        return AutoMapper.Mapper.Map<TblMembership, Model.Membership>(memberToAdd);
                    }
                    else
                    {
                        var lessThan2Members = await context.TblMembership
                            .Where(x => x.MembershipReference == request.Membership.MembershipReference).Join(
                                context.TblEvent, tblMembership => tblMembership.StartEventReference,
                                e => e.EventReference,
                                (tblMembership, e) => new {e.NotificationDate, Membership = tblMembership}).FirstOrDefaultAsync(cancellationToken);

                        existingNotificationDate = lessThan2Members.NotificationDate;
                        existingEffectiveDate = lessThan2Members.Membership.EffectiveDate;

                        if (existingNotificationDate > request.Event.NotificationDate)
                        {
                            endEventReference = eventId;
                        }

                        request.Membership.EndEventReference = endEventReference;

                        var memberToAdd = AutoMapper.Mapper.Map<Model.Membership, TblMembership>(request.Membership);
                        await context.AddAsync(memberToAdd, cancellationToken);

                        if (request.Membership.LevyTagTypeReference == null ||
                            request.Membership.LevyTagTypeReference < lessThan2Type)
                        {

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