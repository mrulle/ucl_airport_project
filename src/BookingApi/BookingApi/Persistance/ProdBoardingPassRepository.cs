using BookingApi.Models;
using Npgsql;

namespace BookingApi.Persistance;

public class ProdBoardingPassRepository : IBoardingPassRepository
{
    public string Add(BoardingPassModel item)
    {
        throw new NotImplementedException();
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BoardingPassModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public BoardingPassModel GetById(string id)
    {
        var cs = "Host=postgres;Username=postgres;Password=postgres;Database=postgres";
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var sql = $"select * from vw_boarding_pass where checkin_id='{id}';";
        Console.WriteLine($"executing statement:\n{sql}");
        using var cmd = new NpgsqlCommand(sql, con);
        using var reader = cmd.ExecuteReader();
        List<BoardingPassModel> boardingPasses = new();
        while (reader.Read()) {
            var mappedBoardingPass = Map(reader);
            boardingPasses.Add(mappedBoardingPass);
        }
        Console.WriteLine($"found {boardingPasses.Count()} results");
        con.Close();
        if (boardingPasses.Count() == 1){
            return boardingPasses[0];
        }
        throw new Exception("multiple rows with same id");
    }

    private BoardingPassModel Map(NpgsqlDataReader reader)
    {
        BoardingPassModel model = new();
        var checkin_id = reader["checkin_id"].ToString()
            ?? throw new NullReferenceException("no checkin_id found");
        model.CheckinId = checkin_id;
        var passenger_id = reader["passenger_id"].ToString()
            ?? throw new NullReferenceException("no passenger_id found");
        model.PassengerId = passenger_id;
        var flight_id = reader["flight_id"].ToString()
            ?? throw new NullReferenceException("no flight_id found");
        model.FlightId = flight_id;
        return model;
    }

    public string Update(BoardingPassModel item)
    {
        throw new NotImplementedException();
    }
}
