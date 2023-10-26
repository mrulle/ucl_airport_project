using BookingApi.Models;
using Npgsql;

namespace BookingApi.Persistance;


public class ProdBookingRepository : IBookingRepository
{
    public string Add(BookingModel item)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        item.BagageId = Guid.NewGuid().ToString();
        item.PassengerId = Guid.NewGuid().ToString();
        item.InputBookingId = Guid.NewGuid().ToString();
        var sql = $"call sp_insert_booking_data('{item.Email}'::VARCHAR(255), '{item.PassportNumber}'::VARCHAR(255), {item.AddedLuggage}, '{item.BagageId}'::UUID, '{item.FlightId}'::UUID, '{item.PassengerId}'::UUID, '{item.InputBookingId}'::UUID);";
        Console.WriteLine($"attempting this statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        var rowsAffected = cmd.ExecuteNonQuery();
        con.Close();
        return item.InputBookingId;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BookingModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public BookingModel GetById(string id)
    {
        throw new NotImplementedException();
    }

    public string Update(BookingModel item)
    {
        throw new NotImplementedException();
    }
}