using BookingApi.Models;
using Npgsql;
namespace BookingApi.Persistance;


public class ProdCheckinRepository : ICheckinRepository
{
    public string Add(CheckinModel item)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        string checkinId = Guid.NewGuid().ToString();
        con.Open();
        var sql = $"call sp_checkin_passenger('{item.BookingId}', '{checkinId}';";
        Console.WriteLine($"attempting this statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        var rowsAffected = cmd.ExecuteNonQuery();
        con.Close();
        if (rowsAffected < 1) {
            throw new Exception("ooops.. something went wrong");
        }
        return checkinId;
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

