using EF_Spike.Shared.Model;
using MediatR;

namespace EF_Spike.Membership.Handler
{
    public class PostMembershipSproc : IRequest<Model.Membership>
    {
        public Model.Membership Membership { get; set; }
        public Event Event { get; set; }
    }
}