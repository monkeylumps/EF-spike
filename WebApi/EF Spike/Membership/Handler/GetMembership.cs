using MediatR;

namespace EF_Spike.Membership.Handler
{
    public class GetMembership : IRequest<Model.Membership>
    {
        public int Psr { get; set; }
    }
}