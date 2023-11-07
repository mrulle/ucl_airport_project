using BookingApi.Models;
using Npgsql;
namespace BookingApi.Persistance;


public class ProdFlightInfoRepository : IFlightInfoRepository
{
    public string Add(FlightInfoModel item)
    {
        Console.WriteLine("About to add " + item.ToString());
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        // var sql = $"call sp_insert_flight_data(plane_id, plane_max_passengers, plane_max_baggage_total, plane_max_baggage_weight, plane_max_baggage_dimension, flight_id, flight_arrival_time, flight_departure, flight_origin, flight_destination) values ('{item.PlaneId}', {item.PassengersAvailableTotal}, {item.BaggageWeightAvailableTotal}, 4000000, 150, '{item.FlightId}', '{item.Arrival}', '{item.Departure}', '{item.Origin}', '{item.Destination}');";
        var sql = $"call sp_insert_flight_data('{item.PlaneId}'::VARCHAR(255), {item.PassengersAvailableTotal}, {item.BaggageWeightAvailableTotal}, 400000, 150, '{item.FlightId}'::VARCHAR(255), '{item.Arrival}', '{item.Departure}', '{item.FlightOrigin}', '{item.FlightDestination}');";
        Console.WriteLine($"attempting this statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        var rowsAffected = cmd.ExecuteNonQuery();
        con.Close();
        return $"{item.FlightId}";
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<FlightInfoModel> GetAll()
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var sql = "select * from vw_flight_info;";
        using var cmd = new NpgsqlCommand(sql, con);
        using var reader = cmd.ExecuteReader();
        List<FlightInfoModel> flights = new List<FlightInfoModel>();
        int rowCounter = 0;
        while (reader.Read()){
            rowCounter += 1;
            var flightInfo = Map(reader);
            flights.Add(flightInfo);
        }
        Console.WriteLine($"{rowCounter} results:\n{reader}");
        con.Close();
        return flights;
    }

    public FlightInfoModel GetById(string id)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var sql = $"select * from vw_flight_info where flight_id='{id}';";
        Console.WriteLine($"executing statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        using var reader = cmd.ExecuteReader();
        List<FlightInfoModel> flightList = new List<FlightInfoModel>();
        while (reader.Read()) {
            var flightInfo = Map(reader);
            flightList.Add(flightInfo);
        }
        Console.WriteLine($"found {flightList.Count()} results");
        con.Close();
        if (flightList.Count() == 1){
            return flightList[0];

        }
        throw new Exception("multiple rows with same id");
    }

    public List<FlightInfoModel> GetPaged(int page, int take = 10)
    {
        return GetAll().Skip(page * take).Take(take).ToList();
    }

    public string Update(FlightInfoModel item)
    {
        throw new NotImplementedException();
    }

    private FlightInfoModel Map(NpgsqlDataReader reader) {
        FlightInfoModel model = new FlightInfoModel();
        var arrival_time = reader["arrival_time"].ToString() 
            ?? throw new NullReferenceException("No arrival_time found.");
        model.Arrival = DateTime.Parse(arrival_time);
        var departure = reader["departure"].ToString()
            ?? throw new NullReferenceException("No departure found");
        model.Departure = DateTime.Parse(departure);
        var origin = reader["origin"].ToString()
            ?? throw new NullReferenceException("No origin found.");
        model.FlightOrigin = origin;
        var destination = reader["destination"].ToString()
            ?? throw new NullReferenceException("No destination found.");
        model.FlightDestination = destination;
        var plane_id = reader["plane_id"].ToString()
            ?? throw new NullReferenceException("No plane_id found");
        model.PlaneId = plane_id;
        var flight_id = reader["flight_id"].ToString()
            ?? throw new NullReferenceException("No flight_id found");
        model.FlightId = flight_id;
        var max_passengers = reader["max_passengers"].ToString()
            ?? throw new NullReferenceException("No max_passengers found");
        model.PassengersAvailableTotal = int.Parse(max_passengers);
        var max_baggage_weight = reader["max_baggage_weight"].ToString()
            ?? throw new NullReferenceException("No max_baggage_weight found");
        model.BaggageWeightAvailableTotal = int.Parse(max_baggage_weight);
        return model;
    }
}

