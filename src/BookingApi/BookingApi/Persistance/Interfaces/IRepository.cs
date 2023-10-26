using BookingApi.Models;

namespace BookingApi.Persistance;

public interface IRepository<T> where T: class {
    T GetById(string id);
    List<T> GetAll();
    string Add(T item);
    bool Delete(string id);
    string Update(T item);
}