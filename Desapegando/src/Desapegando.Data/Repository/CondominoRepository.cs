using System.Linq.Expressions;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Desapegando.Data.Repository;

public class CondominoRepository : Repository<Condomino>, ICondominoRepository
{
    public CondominoRepository(DesapegandoDbContext context) : base(context)
    {
        
    }

    public async Task<Condomino> ReadWithExpression(Expression<Func<Condomino, bool>> predicateExpression)
    {
        return await Db.Condominos.AsNoTracking()
                                  .Where(predicateExpression)
                                  .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Condomino>> ReadWithExpressionList(Expression<Func<Condomino, bool>> predicateExpression)
    {
        return await Db.Condominos.AsNoTracking()
                                  .Where(predicateExpression)
                                  .ToListAsync();
    }
}