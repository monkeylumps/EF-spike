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
            var membership = context.TblMembership.Where(x => x.Psrnumber == request.Membership.Psrnumber && x.EndDate == null && x.EndEventReference == null);

            using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var eventId = await mediator.Send(new CreateEvent {Event = request.Event}, cancellationToken);

                    if (await membership.AnyAsync(cancellationToken))
                    {
                        foreach (var tblMembership in membership)
                        {
                            tblMembership.EndDate = DateTime.Now;
                            tblMembership.EndEventReference = eventId;
                        }
                    }

                    var memberToAdd = AutoMapper.Mapper.Map<Model.Membership, TblMembership>(request.Membership);
                    await context.AddAsync(memberToAdd, cancellationToken);

                    await context.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return AutoMapper.Mapper.Map<TblMembership, Model.Membership>(memberToAdd);
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