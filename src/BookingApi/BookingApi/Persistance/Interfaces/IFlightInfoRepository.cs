using BookingApi.Models;

namespace BookingApi.Persistance;


public interface IFlightInfoRepository : IRepository<FlightInfoModel>
{
    List<FlightInfoModel> GetPaged(int page, int take = 10);
}