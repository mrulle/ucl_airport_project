using BookingApi.Models;
using BookingApi.RabbitMQ;
using Microsoft.Extensions.ObjectPool;
using Npgsql;

namespace BookingApi.Persistance;

public class ProdBaggageRepository : IBaggageRepository
{
    private RabbitMQChannel _channel;
    public ProdBaggageRepository(RabbitMQChannel channel)
    {
        _channel = channel;
    }

    public static BaggageModel NotifyBaggage(string id)
    {
        Console.WriteLine("Hellos");
        return null;
    }
    public string Add(BaggageModel item)
    {
        throw new NotImplementedException();
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BaggageModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public BaggageModel GetById(string id)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
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
        if (baggages.Count == 1)
        {
            UpdateBaggage(baggages[0]);
            return baggages[0];
        }
        throw new Exception("multiple rows with same id");
    }
    private void UpdateBaggage(BaggageModel baggageModel){
         var exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_BAGAGE_EXCHANGE");

         _channel.PublishMessagesToExchange(string.Empty, baggageModel, routingKey: exchangeName);
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

    public string Update(BaggageModel item)
    {
        throw new NotImplementedException();
    }
    
}
