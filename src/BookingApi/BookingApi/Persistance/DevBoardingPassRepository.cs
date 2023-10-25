using BookingApi.Models;

namespace BookingApi.Persistance;


public class DevBoardingPassRepository : IBoardingPassRepository
{
    List<BoardingPassModel> boardingPassModels = new List<BoardingPassModel>();
    public string Add(BoardingPassModel item)
    {
        boardingPassModels.Add(item);
        return item.CheckinId;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public List<BoardingPassModel> GetAll()
    {
        throw new NotImplementedException();
    }

    public BoardingPassModel GetById(string id)
    {
        var item = boardingPassModels.Where(x => x.CheckinId == id);
        if (item is null)
        {
            throw new KeyNotFoundException($"item not found {id}");
        }
        return (BoardingPassModel)item;
    }

    public string Update(BoardingPassModel item)
    {
        throw new NotImplementedException();
    }
}

