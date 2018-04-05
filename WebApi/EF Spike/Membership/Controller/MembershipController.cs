using System;
using System.Threading.Tasks;
using EF_Spike.Membership.Handler;
using EF_Spike.Shared.Event.Enum;
using EF_Spike.Shared.Model;
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
            var result = await mediator.Send(new PostMembership
            {
                Membership = membership,
                Event = new Event
                {
                    EventType = (short)EventType.Amend_Membership,
                    Psrnumber = membership.Psrnumber,
                    SectionNumber = 0,
                    UserId = "user",
                    NotificationDate = DateTime.Now,
                    CreateDateTime = DateTime.Now,
                    EventSourceReference = 1
                }
            });

            return Created(string.Empty, result);
        }

        [HttpGet]
        [Route("api/Membership/NotApplicable")]
        public async Task<IActionResult> GetNotApplicable(int psr)
        {
            var result = await mediator.Send(new GetNotApplicableMembership{Psr = psr});

            return Ok(result);
        }
    }
}