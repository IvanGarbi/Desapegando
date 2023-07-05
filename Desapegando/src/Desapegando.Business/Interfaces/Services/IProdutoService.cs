using Desapegando.Business.Models;

namespace Desapegando.Business.Interfaces.Services;

public interface IProdutoService : IService
{
    Task Create(Produto produto);
    Task Update(Produto produto);
    Task Delete(Guid id);
    Task Curtir(Guid id);
    Task Descurtir(Guid id);
}