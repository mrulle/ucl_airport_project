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
        if (String.IsNullOrEmpty(item.BookingId))
            item.BookingId = Guid.NewGuid().ToString();

        _checkinModels.Add(item);
        return item.BookingId;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<CheckinModel> GetAll()
    {
        return _checkinModels;
    }

    public CheckinModel GetById(string id)
    {
        var item = _checkinModels.Where(x => x.BookingId == id) 
            ?? throw new KeyNotFoundException($"item not found {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one booking was found with the id: {id}");

        return item.ElementAt(0);
    }

    public string Update(CheckinModel item)
    {
        throw new NotImplementedException();
    }
}