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
        private readonly IBoardingPassRepository repo;

        public BoardingPassController(IBoardingPassRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll() {
            return Ok(repo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            try
            {
                repo.GetById(id);
            }
            catch (System.Exception ex)
            {
                return NotFound("None found: " + ex);
            }
            return Ok(repo.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] BoardingPassModel model)
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
        public IActionResult Update([FromBody] BoardingPassModel model) {
            string updatedId = repo.Update(model);
            return CreatedAtAction(nameof(Get), new { id = updatedId }, model);
        }
    }

}