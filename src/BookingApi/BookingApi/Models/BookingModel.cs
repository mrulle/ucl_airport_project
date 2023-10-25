namespace BookingApi.Models;

public class BookingModel
{
    public string Email { get; set; } = String.Empty;
    public string BookingId { get; set; } = String.Empty;
    public int PassportNumber { get; set; }
    public int AddedLuggage { get; set; }
}