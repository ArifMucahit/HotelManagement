using HotelManagementAPI.DataModels;

namespace HotelManagementAPI.Repositories.Repository.Interface;

public interface IBaseRepository<T> where T : BaseEntity 
{
    Task<T?> GetByIdAsync(Guid Id, CancellationToken ct = default);
    Task<T?> GetByIdNoTrackingAsync(Guid id, CancellationToken ct = default);
    Task<List<T>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task<T> Insert(T entity, CancellationToken ct = default);
    Task Update(T entity, CancellationToken ct = default);
    Task Delete(T entity, CancellationToken ct = default);
    Task<IEnumerable<T>> InsertMultiple(IEnumerable<T> entities, CancellationToken ct = default);
}