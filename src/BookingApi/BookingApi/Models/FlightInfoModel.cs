namespace BookingApi.Models {
    public class FlightInfoModel {

        public string PlaneId { get; set; }
        public string FlightId { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int PassengersAvaliableTotal { get; set; }
        public int BaggageWeightAvailableTotal { get; set; }
    }
}