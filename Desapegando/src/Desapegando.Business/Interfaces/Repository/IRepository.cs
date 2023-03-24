using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Repository;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Create(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(Guid id);
    Task<TEntity> ReadById(Guid id);
    Task<IEnumerable<TEntity>> Read();
    Task<int> SaveChanges();
}