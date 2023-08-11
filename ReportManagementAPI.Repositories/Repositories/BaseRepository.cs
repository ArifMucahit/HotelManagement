using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ReportManagementAPI.Repositories.DataModels;
using ReportManagementAPI.Repositories.Repositories.Interface;

namespace ReportManagementAPI.Repositories.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly ReportManagementContext _context;
    private DbSet<T> _entities;

    public BaseRepository(ReportManagementContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid Id, CancellationToken ct = default)
    {
        return await _entities.Where(x => x.IsDeleted == false && x.UUID == Id).FirstOrDefaultAsync(ct);
    }

    public async Task<List<T?>> GetByFilter(Expression<Func<T, bool>> filter)
    {
        return await _entities.Where(filter).ToListAsync();
    }

    public async Task<T?> GetByIdNoTrackingAsync(Guid Id, CancellationToken ct = default)
    {
        var entity = await _entities.AsNoTracking().Where(x => x.IsDeleted == false && x.UUID == Id)
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

        entity.IsDeleted = true;
        _entities.Update(entity);
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