using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevCheckinRepository : ICheckinRepository
{
    private readonly List<CheckinModel> _checkinModels;

    public DevCheckinRepository()
    {
        _checkinModels = new();
    }

    public string Add(CheckinModel item)
    {
        if (String.IsNullOrEmpty(item.CheckinId)) {
            item.CheckinId = Guid.NewGuid().ToString();
        }
        _checkinModels.Add(item);
        return item.CheckinId;
    }

    public bool Delete(string id)
    {
        var item = _checkinModels.Where(x => x.BookingId == id) 
            ?? throw new NullReferenceException($"No flight info found with the id: {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one flight info was found with the id: {id}");

        return _checkinModels.Remove(item.ElementAt(0));
    }

    public List<CheckinModel> GetAll()
    {
        return _checkinModels;
    }

    public CheckinModel GetById(string id)
    {
        var item = _checkinModels.Where(x => x.BookingId == id) 
            ?? throw new KeyNotFoundException($"item not found {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one booking was found with the id: {id}");

        return item.ElementAt(0);
    }

    public string Update(CheckinModel item)
    {
        var itemToUpdate = _checkinModels.Where(x => x.BookingId == item.BookingId) 
            ?? throw new KeyNotFoundException($"item not found {item.BookingId}");

        if (!itemToUpdate.Any())
            throw new Exception($"No item was found with the id: {item.BookingId}");

        _checkinModels.Remove(itemToUpdate.ElementAt(0));
        _checkinModels.Add(item);
        return item.BookingId;
    }
}