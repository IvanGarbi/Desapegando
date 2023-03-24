using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;

namespace Desapegando.Data.Repository;

public class CondominoRepository : Repository<Condomino>, ICondominoRepository
{
    public CondominoRepository(DesapegandoDbContext context) : base(context)
    {
        
    }
}