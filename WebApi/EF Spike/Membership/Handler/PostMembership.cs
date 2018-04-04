using MediatR;

namespace EF_Spike.Membership.Handler
{
    public class PostMembership : IRequest<Model.Membership>
    {
        public Model.Membership Membership { get; set; }
    }
}