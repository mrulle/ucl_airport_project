using System.Runtime.InteropServices;
using BookingApi.Models;
using BookingApi.Persistance.DAO;
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
        // var sql = $"call sp_insert_flight_data(plane_id, plane_max_passengers, plane_max_baggage_total, plane_max_baggage_weight, plane_max_baggage_dimension, flight_id, flight_arrival_time, flight_departure, flight_origin, flight_destination) values ('{item.PlaneId}', {item.PassengersAvailableTotal}, {item.BaggageWeightAvailableTotal}, 4000000, 150, '{item.FlightId}', '{item.Arrival}', '{item.Departure}', '{item.Origin}', '{item.Destination}');";
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

