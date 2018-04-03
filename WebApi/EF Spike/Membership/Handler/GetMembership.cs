using System.Linq;
using EF_Spike.DatabaseContext;

namespace EF_Spike.Membership.Handler
{
    public class GetMembership : IGetMembership
    {
        private readonly RegistryContext context;

        public GetMembership(RegistryContext context)
        {
            this.context = context;
        }

        public Model.Membership Handle(int psr)
        {
            var membership = context.TblMembership.FirstOrDefault(x => x.Psrnumber == psr);

            var returnMembership = new Model.Membership(membership);

            return returnMembership;
        }
    }
}
