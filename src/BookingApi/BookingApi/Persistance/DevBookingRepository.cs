using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevBookingRepository : IBookingRepository
{
    List<BookingModel> bookingModels = new List<BookingModel>();
    public string Add(BookingModel item)
    {
        bookingModels.Add(item);
        return item.BookingId;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BookingModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public BookingModel GetById(string id)
    {
        var item = bookingModels.Where(x => x.BookingId == id);
        if (item is null)
        {
            throw new KeyNotFoundException($"item not found {id}");
        }
        return (BookingModel)item;
    }

    public string Update(BookingModel item)
    {
        throw new NotImplementedException();
    }
}