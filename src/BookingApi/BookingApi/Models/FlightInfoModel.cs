namespace BookingApi.Models {
    public class FlightInfoModel {

        public string PlaneId { get; set; } = String.Empty;
        public string FlightId { get; set; } = String.Empty;
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Origin { get; set; } = String.Empty;
        public string Destination { get; set; } = String.Empty;
        public int PassengersAvailableTotal { get; set; }
        public int BaggageWeightAvailableTotal { get; set; }
    }
}