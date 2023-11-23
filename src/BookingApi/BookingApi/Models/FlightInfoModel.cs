namespace BookingApi.Models {
    public class FlightInfoModel {

        public string PlaneId { get; set; } = String.Empty;
        public string FlightId { get; set; } = String.Empty;
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string FlightOrigin { get; set; } = String.Empty;
        public string FlightDestination { get; set; } = String.Empty;
        public int PassengersAvailableTotal { get; set; }
        public int BaggageWeightAvailableTotal { get; set; }
    }
}