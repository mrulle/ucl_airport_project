using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevFlightInfoRepository: IFlightInfoRepository {

    private readonly List<FlightInfoModel> _flightInfoList;

    public DevFlightInfoRepository()
    {
        _flightInfoList = new();
    }

    public string Add(FlightInfoModel item)
    {
        if (String.IsNullOrEmpty(item.FlightId))
            item.FlightId = Guid.NewGuid().ToString();

        _flightInfoList.Add(item);
        return item.FlightId;
    }

    public bool Delete(string id)
    {
        var item = _flightInfoList.Where(x => x.FlightId == id) 
            ?? throw new NullReferenceException($"No flight info found with the id: {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one flight info was found with the id: {id}");

        return _flightInfoList.Remove(item.ElementAt(0));
    }

    public List<FlightInfoModel> GetAll()
    {
        return _flightInfoList;
    }

    public FlightInfoModel GetById(string id)
    {
        var item = _flightInfoList.Where(x => x.FlightId == id) 
            ?? throw new KeyNotFoundException($"item not found {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one flight info was found with the id: {id}");

        return item.ElementAt(0);
    }

    public string Update(FlightInfoModel item)
    {
        var itemToUpdate = _flightInfoList.Where(x => x.FlightId == item.FlightId) 
            ?? throw new KeyNotFoundException($"item not found {item.FlightId}");

        if (!itemToUpdate.Any())
            throw new Exception($"No item was found with the id: {item.FlightId}");

        _flightInfoList.Remove(itemToUpdate.ElementAt(0));
        _flightInfoList.Add(item);
        return item.FlightId;
    }
}

