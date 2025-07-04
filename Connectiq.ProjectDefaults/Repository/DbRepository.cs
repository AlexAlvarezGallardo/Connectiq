﻿using AutoMapper;
using Connectiq.ProjectDefaults.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Connectiq.ProjectDefaults;

public class DbRepository<TEntity>(
    DbContext _dbContext,
    IMapper _mapper) : IRepository<TEntity>
    where TEntity : class, ISoftDelete
{
    public async Task<bool> InsertAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        var dbSet = _dbContext.Set<TEntity>();
        var keyValues = GetKeyValues(entity);

        var existingEntity = await dbSet.FindAsync(keyValues);

        if (existingEntity is null)
            return false;

        _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

        if (!_dbContext.ChangeTracker.HasChanges())
            return true;

        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> SoftDeleteAsync(TEntity entity)
    {
        var keyValues = GetKeyValues(entity);

        var dbSet = _dbContext.Set<TEntity>();
        var existingEntity = await dbSet.FindAsync(keyValues);

        if (existingEntity is null)
            return false;

        existingEntity.IsActive = false;

        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<ICollection<TOutput>> GetAllAsync<TOutput>(
        int? page = null,
        int? pageSize = null,
        Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = _dbContext.Set<TEntity>().AsExpandableEFCore();

        if (filter is not null)
            query = query.Where(filter);

        if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

        var entities = await query.ToListAsync();

        return _mapper.Map<ICollection<TOutput>>(entities);
    }

    public async Task<TOutput> GetEntityById<TOutput>(Guid id)
    {
        var entity = await _dbContext.Set<TEntity>().AsExpandableEFCore()
            .SingleOrDefaultAsync(e => e.Id == id);

        return _mapper.Map<TOutput>(entity);
    }

    object[] GetKeyValues(TEntity entity)
    {
        var keyProperties = _dbContext.Model.FindEntityType(typeof(TEntity))!
            .FindPrimaryKey()!
            .Properties;

        var keyValues = keyProperties
            .Select(p => typeof(TEntity)
            .GetProperty(p.Name)!.GetValue(entity)!)
            .ToArray();

        return keyValues;
    }
}
