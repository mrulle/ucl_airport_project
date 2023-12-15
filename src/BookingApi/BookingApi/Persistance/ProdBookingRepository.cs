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
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=postgres";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var getPassengerSql = $"select passenger_id from bookings;";
        using var getPassengerCmd = new NpgsqlCommand(getPassengerSql, con);
        var passenger_id = getPassengerCmd.ExecuteScalar();
        Console.WriteLine($"found passenger_id: {passenger_id}");
        
        // Delete baggage
        try {
            var deleteBaggageSql = $"delete from baggage where booking_id = '{id}';";
            using var deleteBaggageCmd = new NpgsqlCommand(deleteBaggageSql, con);
            var baggageDeleted = deleteBaggageCmd.ExecuteNonQuery();
            Console.WriteLine($"baggage deleted: {baggageDeleted}");
        }
        catch (Exception e) {
            Console.WriteLine($"could not delete baggage with booking_id: {id}");
            Console.WriteLine(e.Message);
        }
        // Delete Checkin (if exists)  NOT FOR PRODUCTION REAL LIFE
        try {
            var deleteCheckinSql = $"delete from checkins where booking_id = '{id}';";
            using var deleteCheckinCmd = new NpgsqlCommand(deleteCheckinSql, con);
            var checkinDeleted = deleteCheckinCmd.ExecuteNonQuery();
            Console.WriteLine($"checkin deleted: {checkinDeleted}");
        }
        catch (Exception e) {
            Console.WriteLine($"could not delete checkin with booking_id: {id}");
            Console.WriteLine(e.Message);
        }
        // Delete booking
        try {
            var deleteBookingSql = $"delete from bookings where id = '{id}';";
            using var deleteBookingCmd = new NpgsqlCommand(deleteBookingSql, con);
            var bookingDeleted = deleteBookingCmd.ExecuteNonQuery();
            Console.WriteLine($"booking deleted: {bookingDeleted}");
        }
        catch (Exception e) {
            Console.WriteLine($"could not delete booking with id: {id}");
            Console.WriteLine(e.Message);
        }
        // Delete passenger
        try {
            var deletePassengerSql = $"delete from passengers where id = '{passenger_id}'::UUID;";
            using var deletePassengerCmd = new NpgsqlCommand(deletePassengerSql, con);
            var monkey = deletePassengerCmd.ExecuteNonQuery();
            Console.WriteLine($"passenger {passenger_id} deleted: {monkey}");
        }
        catch (Exception e) {
            Console.WriteLine($"could not delete passenger with id: {passenger_id}");
            Console.WriteLine(e.Message);
        }
        con.Close();
        return true;
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