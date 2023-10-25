using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevCheckinRepository : ICheckinRepository
{
    List<CheckinModel> checkinModels = new List<CheckinModel>();
    public string Add(CheckinModel item)
    {
        checkinModels.Add(item);
        return item.BookingId;
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
        var item = checkinModels.Where(x => x.BookingId == id);
        if (item is null)
        {
            throw new KeyNotFoundException($"item not found {id}");
        }
        return (CheckinModel)item;
    }

    public string Update(CheckinModel item)
    {
        throw new NotImplementedException();
    }
}