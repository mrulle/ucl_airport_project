using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevBookingRepository : IBookingRepository
{
    private readonly List<BookingModel> _bookingModels;

    public DevBookingRepository()
    {
        _bookingModels = new();
    }

    public string Add(BookingModel item)
    {
        if (String.IsNullOrEmpty(item.InputBookingId))
            item.InputBookingId = Guid.NewGuid().ToString();

        _bookingModels.Add(item);
        return item.InputBookingId;
    }

    public bool Delete(string id)
    {
        var item = _bookingModels.Where(x => x.InputBookingId == id) 
            ?? throw new NullReferenceException($"No flight info found with the id: {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one flight info was found with the id: {id}");

        return _bookingModels.Remove(item.ElementAt(0));
    }

    public List<BookingModel> GetAll()
    {
        return _bookingModels;
    }

    public BookingModel GetById(string id)
    {
        var item = _bookingModels.Where(x => x.InputBookingId == id) 
            ?? throw new KeyNotFoundException($"item not found {id}");

        if (!item.Any())
            throw new Exception($"No item was found with the id: {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one booking was found with the id: {id}");
        
        return item.ElementAt(0);
    }

    public string Update(BookingModel item)
    {
        var itemToUpdate = _bookingModels.Where(x => x.InputBookingId == item.InputBookingId) 
            ?? throw new KeyNotFoundException($"item not found {item.InputBookingId}");

        if (!itemToUpdate.Any())
            throw new Exception($"No item was found with the id: {item.InputBookingId}");

        _bookingModels.Remove(itemToUpdate.ElementAt(0));
        _bookingModels.Add(item);
        return item.InputBookingId;
    }
}