using HotelManagementAPI.DataModels;
using HotelManagementAPI.Repositories.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Repositories.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly HotelManagementContext _context;
    private DbSet<T> _entities;

    public BaseRepository(HotelManagementContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int Id, CancellationToken ct = default)
    {
        return await _entities.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefaultAsync(ct);
    }

    public async Task<T?> GetByIdNoTrackingAsync(int Id, CancellationToken ct = default)
    {
        var entity = await _entities.AsNoTracking().Where(x => x.IsDeleted == false && x.Id == Id)
            .FirstOrDefaultAsync(ct);
        return entity;
    }

    public async Task<List<T>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var skip = (pageNumber - 1) * pageSize;
        return await _entities.Skip(skip).Take(pageSize).Where(x => x.IsDeleted == false)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _entities.Where(x => x.IsDeleted == false).ToListAsync(ct);
    }

    public async Task<T> Insert(T entity, CancellationToken ct = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _entities.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity;
    }

    public async Task Update(T entity, CancellationToken ct = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _entities.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Delete(T entity, CancellationToken ct = default)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _entities.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<T>> InsertMultiple(IEnumerable<T> entities, CancellationToken ct = default)
    {
        if (entities == null || !entities.Any())
        {
            throw new ArgumentNullException();
        }

        await _entities.AddRangeAsync(entities, ct);
        await _context.SaveChangesAsync(ct);

        return entities;
    }
}