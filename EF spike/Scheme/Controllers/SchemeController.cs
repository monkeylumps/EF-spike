using Microsoft.AspNetCore.Mvc;
using spike.Scheme.Models;

namespace EF_spike.Controllers
{
    [Produces("application/json")]
    [Route("api/Scheme")]
    public class SchemeController : Controller
    {
        [HttpGet]
        public Scheme Get()
        {
            return new Scheme();
        }
    }
}