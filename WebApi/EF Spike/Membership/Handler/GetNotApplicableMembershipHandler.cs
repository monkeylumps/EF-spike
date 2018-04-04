using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF_Spike.DatabaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EF_Spike.Membership.Handler
{
    public class GetNotApplicableMembershipHandler : IRequestHandler<GetNotApplicableMembership, List<Model.Membership>>
    {
        private readonly RegistryContext context;

        public GetNotApplicableMembershipHandler(RegistryContext context)
        {
            this.context = context;
        }

        public async Task<List<Model.Membership>> Handle(GetNotApplicableMembership request, CancellationToken cancellationToken)
        {
            var membership = await context.TblMembership.Where(x => x.Psrnumber == request.Psr && x.EndDate == null && x.TblMembershipAverageAgeBasis.Any(y => y.MembershipAverageAgeBasis == 3)).ToListAsync(cancellationToken);

            return AutoMapper.Mapper.Map<List<TblMembership>, List<Model.Membership>>(membership);
        }
    }
}