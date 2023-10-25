using BookingApi.Models;
using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;


namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightInfoController: ControllerBase {
        private readonly IRepository<FlightInfoModel> repo;

        public FlightInfoController(IRepository<FlightInfoModel> repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            // return all available flights here
            return Ok();
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            
            return Ok();
        }
    }

}