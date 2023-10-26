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
        var sql = $"call sp_insert_booking_data('{item.Email}', {item.PassportNumber}, {item.AddedLuggage}, '{item.BagageId}', '{item.FlightId}', '{item.PassengerId}', '{item.InputBookingId}');";
        Console.WriteLine($"attempting this statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        var rowsAffected = cmd.ExecuteNonQuery();
        con.Close();
        return $"{item.InputBookingId}";
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