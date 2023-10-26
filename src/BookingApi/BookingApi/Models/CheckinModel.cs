namespace BookingApi.Models;

public class CheckinModel {
    public string CheckinId { get; set; } = Guid.NewGuid().ToString();
    public string Email { get; set; } = String.Empty;
    public string BookingId { get; set; } = String.Empty;
}