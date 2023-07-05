using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;

namespace Desapegando.Data.Repository
{
    public class ProdutoCurtidaRepository : Repository<ProdutoCurtida>, IProdutoCurtidaRepository
    {
        public ProdutoCurtidaRepository(DesapegandoDbContext context) : base(context)
        {

        }
    }
}
