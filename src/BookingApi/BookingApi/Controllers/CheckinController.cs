using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using BookingApi.Models;


namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckinController : ControllerBase
    {
        private readonly IRepository<CheckinModel> repo;

        public CheckinController(IRepository<CheckinModel> repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CheckinModel model)
        {
            // implement code for checkin here
            return Ok();
        }
    }

}