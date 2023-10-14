using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services
{
    public interface ICompraService : IService
    {
        Task Create(Compra compra);
        Task Update(Compra campanha);
        Task Delete(Guid id);
    }
}
