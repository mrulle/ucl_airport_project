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
        if (String.IsNullOrEmpty(item.BookingId))
            item.BookingId = Guid.NewGuid().ToString();

        _bookingModels.Add(item);
        return item.BookingId;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BookingModel> GetAll()
    {
        return _bookingModels;
    }

    public BookingModel GetById(string id)
    {
        var item = _bookingModels.Where(x => x.BookingId == id) 
            ?? throw new KeyNotFoundException($"item not found {id}");

        if (item.Count() > 1) 
            throw new Exception($"More than one booking was found with the id: {id}");
        
        return item.ElementAt(0);
    }

    public string Update(BookingModel item)
    {
        throw new NotImplementedException();
    }
}