using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Desapegando.Data.Repository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly DesapegandoDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DesapegandoDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task Create(TEntity entity)
    {
        Db.Add(entity);
        await SaveChanges();
    }

    public async Task Update(TEntity entity)
    {
        Db.Update(entity);
        await SaveChanges();
    }

    public async Task Delete(Guid id)
    {
        Db.Remove(new TEntity { Id = id });
        await SaveChanges();
    }

    public async Task<TEntity> ReadById(Guid id)
    {
        return await DbSet.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> Read()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<int> SaveChanges()
    {
        return await Db.SaveChangesAsync();
    }

    public async void Dispose()
    {
        Db?.Dispose();
    }
}