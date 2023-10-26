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
        public IActionResult GetAll() {
            return Ok(repo.GetAll());
        }

        [HttpGet("paged/{page}")]
        public IActionResult GetPaged(int page) {
            return Ok(repo.GetPaged(page));
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            return Ok(repo.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] FlightInfoModel model)
        {
            string generatedId = repo.Add(model);
            return CreatedAtAction(nameof(Get), new { id = generatedId }, model);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            bool success = repo.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update([FromBody] FlightInfoModel model) {
            string updatedId = repo.Update(model);
            return CreatedAtAction(nameof(Get), new { id = updatedId }, model);
        }
    }

}