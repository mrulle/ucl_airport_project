using BookingApi.Models;
using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;


namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightInfoController: ControllerBase {
        private readonly IFlightInfoRepository repo;

        public FlightInfoController(IFlightInfoRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var result = repo.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var result = repo.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] FlightInfoModel flightInfoModel) {
            // TODO: return a createdAt timestamp thingy
            var result = repo.Add(flightInfoModel);

            return CreatedAtAction(nameof(GetById), new { id = result }, flightInfoModel);
        }
    }

}