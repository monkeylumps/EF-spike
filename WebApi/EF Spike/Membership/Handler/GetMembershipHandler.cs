﻿using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF_Spike.Membership.Handler
{
    public class GetMembershipHandler : IRequestHandler<GetMembership, Model.Membership>
    {
        private readonly RegistryContext context;

        public GetMembershipHandler(RegistryContext context)
        {
            this.context = context;
        }

        public async Task<Model.Membership> Handle(GetMembership query, CancellationToken cancellationToken)
        {
            var membership = await context.TblMembership.FirstOrDefaultAsync(x => x.Psrnumber == query.Psr && x.EndDate == null && x.EndEventReference == null, cancellationToken);

            if (membership == null) return null;

            return AutoMapper.Mapper.Map<TblMembership, Model.Membership>(membership);
        }
    }
}
