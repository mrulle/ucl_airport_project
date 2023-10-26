using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using BookingApi.Models;
using BookingApi.RabbitMQ;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardingPassController : ControllerBase
    {
        private readonly IBookingRepository repo;

        public BoardingPassController(RabbitMQChannel channel   )
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