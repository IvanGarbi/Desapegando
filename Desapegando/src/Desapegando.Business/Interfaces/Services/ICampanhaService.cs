using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services
{
    public interface ICampanhaService : IService
    {
        Task Create(Campanha campanha);
        Task Update(Campanha campanha);
        Task Delete(Guid id);
    }
}
