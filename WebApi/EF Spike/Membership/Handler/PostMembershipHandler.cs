using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF_Spike.Membership.Handler
{
    public class PostMembershipHandler : IRequestHandler<PostMembership, Model.Membership>
    {
        private readonly RegistryContext context;

        public PostMembershipHandler(RegistryContext context)
        {
            this.context = context;
        }

        public async Task<Model.Membership> Handle(PostMembership request, CancellationToken cancellationToken)
        {
            var membership = context.TblMembership.Where(x => x.Psrnumber == request.Membership.Psrnumber && x.EndDate == null && x.EndEventReference == null);



            if (await membership.AnyAsync(cancellationToken))
            {
                foreach (var tblMembership in membership)
                {
                    tblMembership.EndDate = DateTime.Now;
                }
            }

            try
            {
                var memberToAdd = AutoMapper.Mapper.Map<Model.Membership, TblMembership>(request.Membership);
                await context.AddAsync(memberToAdd, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);

                return AutoMapper.Mapper.Map<TblMembership, Model.Membership>(memberToAdd);
            }
            catch (Exception e)
            {
               //log this error and do stuff
            }

            return null;
        }
    }
}