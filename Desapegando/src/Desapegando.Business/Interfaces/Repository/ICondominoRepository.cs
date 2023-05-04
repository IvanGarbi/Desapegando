using Desapegando.Business.Models;
using System.Linq.Expressions;

namespace Desapegando.Business.Interfaces.Repository;

public interface ICondominoRepository : IRepository<Condomino>
{
    Task<Condomino> ReadWithExpression(Expression<Func<Condomino, bool>> predicateExpression);
    Task<IEnumerable<Condomino>> ReadWithExpressionList(Expression<Func<Condomino, bool>> predicateExpression);
}