using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using BookingApi.Models;


namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardingPassController : ControllerBase
    {
        private readonly IBookingRepository repo;

        public BoardingPassController(IBookingRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public IActionResult Post([FromBody] BookingModel model)
        {
            // implement code for checkin here
            return Ok();
        }
    }

}