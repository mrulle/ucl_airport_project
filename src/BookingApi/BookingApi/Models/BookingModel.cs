using System.Text.Json.Serialization;

namespace BookingApi.Models;

public class BookingModel
{
    public BookingModel(string flightId) {
        FlightId = flightId;
    }
    public string Email { get; set; } = String.Empty;
    public int PassportNumber { get; set; }
    public int AddedLuggage { get; set; }
    [JsonIgnore]
    public string BookingNumber { get; set; } = Guid.NewGuid().ToString();
    [JsonIgnore]
    public string BagageId { get; set; } = Guid.NewGuid().ToString();
    public string FlightId { get; set; } = String.Empty;
    [JsonIgnore]
    public string PassengerId { get; set; } = Guid.NewGuid().ToString();
    [JsonIgnore]
    public string InputBookingId {get; set; } = Guid.NewGuid().ToString();
}