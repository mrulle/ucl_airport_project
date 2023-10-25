using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevRepository: IRepository<FlightInfoModel> {
    static List<FlightInfoModel> flightInfoList = new List<FlightInfoModel>();
    public DevRepository()
    {
        
    }

    public string Add(FlightInfoModel item)
    {
        flightInfoList.Add(item);
        return item.FlightId;
    }

    public bool Delete(string id)
    {
        var item = flightInfoList.Where(x => x.FlightId == id);
        if (item is null){
            return false;
        }
        flightInfoList.Remove((FlightInfoModel) item);
        return true;
    }

    public List<FlightInfoModel> GetAll()
    {
        return flightInfoList;
    }

    public FlightInfoModel GetById(string id)
    {
        var item = flightInfoList.Where(x => x.FlightId == id);
        if (item is null) {
            throw new KeyNotFoundException($"item not found {id}");
        }
        return (FlightInfoModel) item;

    }

    public string Update(FlightInfoModel item)
    {
        var itemToUpdate = flightInfoList.Where(x => x.FlightId == item.FlightId);
        if (itemToUpdate is null)
        {
            throw new KeyNotFoundException($"item not found {item.FlightId}");
        }
        flightInfoList.Remove((FlightInfoModel)itemToUpdate);
        flightInfoList.Add(item);
        return item.FlightId;
    }
}

