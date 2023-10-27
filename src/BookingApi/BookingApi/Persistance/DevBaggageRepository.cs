using BookingApi.Models;

namespace BookingApi.Persistance;

public class DevBaggageRepository : IBaggageRepository
{
    public string Add(BaggageModel item)
    {
        throw new NotImplementedException();
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BaggageModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public BaggageModel GetById(string id)
    {
        return new();
    }

    public string Update(BaggageModel item)
    {
        throw new NotImplementedException();
    }
}
