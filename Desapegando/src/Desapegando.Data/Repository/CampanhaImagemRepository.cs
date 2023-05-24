using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Models;
using Desapegando.Data.Context;

namespace Desapegando.Data.Repository
{
    public class CampanhaImagemRepository : Repository<CampanhaImagem>, ICampanhaImagemRepository
    {
        public CampanhaImagemRepository(DesapegandoDbContext context) : base(context)
        {
        }
    }
}
