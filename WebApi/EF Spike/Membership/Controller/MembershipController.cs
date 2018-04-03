using EF_Spike.Membership.Handler;
using Microsoft.AspNetCore.Mvc;

namespace EF_Spike.Membership.Controller
{
    [Produces("application/json")]
    [Route("api/Membership")]
    public class MembershipController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IGetMembership getMembership;

        public MembershipController(IGetMembership getMembership)
        {
            this.getMembership = getMembership;
        }

        [HttpGet]
        public IActionResult Get(int Psr)
        {
            var result = getMembership.Handle(Psr);
            return Ok(result);
        }
    }
}