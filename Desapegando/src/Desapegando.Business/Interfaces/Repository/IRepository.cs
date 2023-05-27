using Desapegando.Business.Models;
using System.Linq.Expressions;

namespace Desapegando.Business.Interfaces.Repository;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task Create(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(Guid id);
    Task<TEntity> ReadById(Guid id);
    Task<IEnumerable<TEntity>> Read();
    Task<IEnumerable<TEntity>> ReadExpression(Expression<Func<TEntity, bool>> predicateExpression);
    Task<int> SaveChanges();
}