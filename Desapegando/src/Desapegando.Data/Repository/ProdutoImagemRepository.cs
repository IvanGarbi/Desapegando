using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;

namespace Desapegando.Data.Repository
{
    public class ProdutoImagemRepository : Repository<ProdutoImagem>, IProdutoImagemRepository
    {
        public ProdutoImagemRepository(DesapegandoDbContext context) : base(context)
        {
        }
    }
}
