using BookingApi.Models;
using BookingApi.RabbitMQ;
using Npgsql;
using RabbitMQ.Client;

namespace BookingApi.Persistance;


public class ProdCheckinRepository : ICheckinRepository
{
    private readonly RabbitMQChannel _channel;
    public ProdCheckinRepository(RabbitMQChannel channel)
    {
        this._channel = channel;
    }
    public string Add(CheckinModel item)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=postgres";
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
            _channel.PublishMessagesToExchange(string.Empty, item,  "checkin.baggage", null, ExchangeType.Direct);
            return checkinId;
        }
        
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
}

