using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services
{
    public interface IProdutoCurtidaService : IService
    {
        Task Create(ProdutoCurtida produtoCurtida);
        Task Delete(Guid id);
        Task Curtir(Guid produtoId, Guid condominoId);
        Task Descurtir(Guid produtoId, Guid condominoId);
    }
}
