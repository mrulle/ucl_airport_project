using System.Runtime.InteropServices;
using BookingApi.Models;
using BookingApi.Persistance.DAO;
using Npgsql;
namespace BookingApi.Persistance;


public class ProdFlightInfoRepository : IFlightInfoRepository
{
    private readonly EF_DataContext context;

    public ProdFlightInfoRepository(EF_DataContext context)
    {
        this.context = context;
    }

    public string Add(FlightInfoModel item)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=production";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        // var sql = $"call sp_insert_flight_data(plane_id, plane_max_passengers, plane_max_baggage_total, plane_max_baggage_weight, plane_max_baggage_dimension, flight_id, flight_arrival_time, flight_departure, flight_origin, flight_destination) values ('{item.PlaneId}', {item.PassengersAvailableTotal}, {item.BaggageWeightAvailableTotal}, 4000000, 150, '{item.FlightId}', '{item.Arrival}', '{item.Departure}', '{item.Origin}', '{item.Destination}');";
        var sql = $"call sp_insert_flight_data('{item.PlaneId}', {item.PassengersAvailableTotal}, {item.BaggageWeightAvailableTotal}, 400000, 150, '{item.FlightId}', '{item.Arrival}', '{item.Departure}', '{item.Origin}', '{item.Destination}');";
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
        model.Arrival = DateTime.Parse(reader["arrival_time"].ToString());
        model.Departure = DateTime.Parse(reader["departure"].ToString());
        model.Origin = reader["origin"].ToString();
        model.Destination = reader["destination"].ToString();
        model.PlaneId = reader["plane_id"].ToString();
        model.FlightId = reader["flight_id"].ToString();
        model.PassengersAvailableTotal = int.Parse(reader["max_passengers"].ToString());
        model.BaggageWeightAvailableTotal = int.Parse(reader["max_baggage_weight"].ToString());
        return model;
    }
}

