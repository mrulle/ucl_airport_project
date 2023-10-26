using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevBoardingPassRepository : IBoardingPassRepository
{
    private readonly List<BoardingPassModel> _boardingPasses;

    public DevBoardingPassRepository()
    {
        _boardingPasses = new();
    }

    public string Add(BoardingPassModel item)
    {
        _boardingPasses.Add(item);
        return item.CheckinId;
    }

    public bool Delete(string id)
    {
        var item = _boardingPasses.Where(x => x.CheckinId == id) 
            ?? throw new NullReferenceException($"No boarding pass found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one boarding pass was found with the id: {id}");

        return _boardingPasses.Remove(item.ElementAt(0));
    }

    public List<BoardingPassModel> GetAll()
    {
        return _boardingPasses;
    }

    public BoardingPassModel GetById(string id)
    {
        var item = _boardingPasses.Where(x => x.CheckinId == id) 
            ?? throw new KeyNotFoundException($"item not found {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one boarding pass was found with the id: {id}");

        return item.ElementAt(0);
    }

    public string Update(BoardingPassModel item)
    {
        throw new NotImplementedException();
    }
}

