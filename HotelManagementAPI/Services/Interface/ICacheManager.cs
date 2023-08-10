namespace HotelManagementAPI.Services.Interface;

public interface ICacheManager
{
    Task<T> GetOrAdd<T>(string key,Func<Task<T>> func) where T : class;
    Task AddAsync(string key, object data);
    void Remove(string key);
}