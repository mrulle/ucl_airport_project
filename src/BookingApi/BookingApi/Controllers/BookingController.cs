using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using BookingApi.Models;


namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository repo;

        public BookingController(IBookingRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult GetAll() {
            return Ok(repo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            return Ok(repo.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] BookingModel model)
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
        public IActionResult Update([FromBody] BookingModel model) {
            string updatedId = repo.Update(model);
            return CreatedAtAction(nameof(Get), new { id = updatedId }, model);
        }
    }

}