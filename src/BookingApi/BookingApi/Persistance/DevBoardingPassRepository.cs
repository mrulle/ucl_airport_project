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
        if (String.IsNullOrEmpty(item.CheckinId))
            item.CheckinId = Guid.NewGuid().ToString();

        _boardingPasses.Add(item);
        return item.CheckinId;
    }

    public bool Delete(string id)
    {
        var item = _boardingPasses.Where(x => x.CheckinId == id) 
            ?? throw new NullReferenceException($"No boarding pass found with the id: {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

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

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one boarding pass was found with the id: {id}");

        return item.ElementAt(0);
    }

    public string Update(BoardingPassModel item)
    {
        var itemToUpdate = _boardingPasses.Where(x => x.CheckinId == item.CheckinId) 
            ?? throw new KeyNotFoundException($"item not found {item.CheckinId}");

        if (!itemToUpdate.Any())
            throw new Exception($"No item was found with the id: {item.CheckinId}");

        _boardingPasses.Remove(itemToUpdate.ElementAt(0));
        _boardingPasses.Add(item);
        return item.FlightId;
    }
}

