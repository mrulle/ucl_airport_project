using BookingApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using BookingApi.Models;


namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckinController : ControllerBase
    {
        private readonly ICheckinRepository _checkinRepo;
        private readonly IBoardingPassRepository _boardingPassRepo;

        public CheckinController(ICheckinRepository repo, IBoardingPassRepository boardingPassRepo)
        {
            this._checkinRepo = repo;
            _boardingPassRepo = boardingPassRepo;
        }

        [HttpGet]
        public IActionResult GetAll() {
            return Ok(_checkinRepo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            return Ok(_checkinRepo.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CheckinModel model)
        {
            string generatedId = _checkinRepo.Add(model);

            var boardingPass = _boardingPassRepo.GetById(generatedId);
            
            // It should actually return this (and remove the boarding pass repo) to accommodate rest structure
            // But had trouble fetching the boarding pass on the controller endpoint immediately after this endpoint returned
            return CreatedAtAction(nameof(Get), new { id = generatedId }, boardingPass);

            // return Ok(boardingPass);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id) {
            bool success = _checkinRepo.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update([FromBody] CheckinModel model) {
            string updatedId = _checkinRepo.Update(model);
            return CreatedAtAction(nameof(Get), new { id = updatedId }, model);
        }
    }

}