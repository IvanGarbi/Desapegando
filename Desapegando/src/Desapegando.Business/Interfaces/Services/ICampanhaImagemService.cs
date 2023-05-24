
using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services
{
    public interface ICampanhaImagemService : IService
    {
        Task Create(CampanhaImagem campanhaImagem);
        Task Update(CampanhaImagem campanhaImagem);
        Task Delete(Guid id);
    }
}
