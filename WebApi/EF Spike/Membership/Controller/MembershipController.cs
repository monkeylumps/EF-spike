using System.Threading.Tasks;
using EF_Spike.Membership.Handler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EF_Spike.Membership.Controller
{
    [Produces("application/json")]
    [Route("api/Membership")]
    public class MembershipController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IMediator mediator;

        public MembershipController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int psr)
        {
            var result = await mediator.Send(new GetMembership {Psr = psr});
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Model.Membership membership)
        {
            var result = await mediator.Send(new PostMembership {Membership = membership});
            return Created(string.Empty, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotApplicable(int psr)
        {
            var result = await mediator.Send(new GetNotApplicableMembership{Psr = psr});

            return Ok(result);
        }
    }
}