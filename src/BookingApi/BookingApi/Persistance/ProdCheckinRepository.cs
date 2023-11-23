using BookingApi.Models;
using BookingApi.RabbitMQ;
using Npgsql;
using RabbitMQ.Client;

namespace BookingApi.Persistance;


public class ProdCheckinRepository : ICheckinRepository
{
    private readonly RabbitMQChannel _channel;

    public ProdCheckinRepository(RabbitMQChannel _channel)
    {
        this._channel = _channel;
    }
    public string Add(CheckinModel item)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        string checkinId = Guid.NewGuid().ToString();
        con.Open();
        var sql = $"call sp_checkin_passenger('{checkinId}'::UUID, '{item.BookingId}'::UUID);";
        Console.WriteLine($"attempting this statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        var rowsAffected = cmd.ExecuteNonQuery();
        con.Close();
        // Cannot verify that this booking has already been checked in
        Console.WriteLine($"rowsAffected: {rowsAffected}");
        if (rowsAffected == 0)
        {
            throw new Exception("Booking has already been checked in");
        }
        else{
            System.Console.WriteLine();
            // var model = _baggageRepository.GetById(item.BookingId);4
            var model = sendToAssens(item.BookingId, con);
            _channel.PublishMessagesToExchange(string.Empty, model,  "checking.baggage", null, ExchangeType.Direct);
            return checkinId;
        }
        
    }

    private BaggageModel sendToAssens(string id, NpgsqlConnection con){
        con.Open();
        var sql = $"select * from vw_baggage where booking_id='{id}';";
        Console.WriteLine($"executing statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        using var reader = cmd.ExecuteReader();
        List<BaggageModel> baggages = new();
        while (reader.Read()) {
            var mappedBaggage = Map(reader);
            baggages.Add(mappedBaggage);
        }
        Console.WriteLine($"found {baggages.Count()} results");
        con.Close();
        if (baggages.Count != 1)
        {
            // UpdateBaggage(baggages[0]);
            throw new Exception();
        }
        return baggages[0];
    }
    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<CheckinModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public CheckinModel GetById(string id)
    {
        throw new NotImplementedException();
    }

    public string Update(CheckinModel item)
    {
        throw new NotImplementedException();
    }
    private BaggageModel Map(NpgsqlDataReader reader)
    {
        BaggageModel model = new();
        var checkin_id = reader["booking_id"].ToString()
            ?? throw new NullReferenceException("no flight_id found");
        model.CheckInNumber = checkin_id;
        var passenger_id = reader["passenger_id"].ToString()
            ?? throw new NullReferenceException("no passenger_id found");
        model.PassengerId = passenger_id;
        var flight_id = reader["flight_id"].ToString()
            ?? throw new NullReferenceException("no flight_id found");
        model.FlightNumber = flight_id;
        var flight_destination = reader["destination"].ToString()
            ?? throw new NullReferenceException("no destination found");
        model.FlightDestination = flight_destination;
        var bagage_weight = int.Parse(reader["weight"].ToString());
        model.BagageWeight = bagage_weight;
        var bagage_length = 150;
        model.BagageLength = bagage_length;
        Random rnd = new Random();
        bool hidden_threat = (rnd.Next(1,20) == 1) ? true : false;
        model.HiddenThreat = hidden_threat;
        return model;
    }
}

