
using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services
{
    public interface IProdutoImagemService : IService
    {
        Task Create(ProdutoImagem produtoImagem);
        Task Update(ProdutoImagem produtoImagem);
        Task Delete(Guid id);
    }
}
