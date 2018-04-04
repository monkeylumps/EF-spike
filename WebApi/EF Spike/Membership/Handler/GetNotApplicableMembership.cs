using System.Collections.Generic;
using MediatR;

namespace EF_Spike.Membership.Handler
{
    public class GetNotApplicableMembership : IRequest<List<Model.Membership>>
    {
        public int Psr { get; set; }
    }
}