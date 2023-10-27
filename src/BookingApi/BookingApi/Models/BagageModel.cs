namespace BookingApi.Models;

public class BaggageModel
{
    
    public string FlightNumber { get; set; } = String.Empty;
    public string PassengerId { get; set; } = String.Empty;
    public string CheckInNumber { get; set; } = String.Empty;

    public string FlightDestination {get;set;} = string.Empty;

    public int BagageWeight {get;set;}

    public int BagageLength {get;set;}

    public bool HiddenThreat { get; set; }



}