using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;

namespace Desapegando.Business.Services;

public class ProdutoService : IProdutoService, IDisposable
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task Create(Produto produto)
    {
        await _repository.Create(produto);
    }

    public async Task Delete(Guid id)
    {
        await _repository.Delete(id);
    }

    public async Task Update(Produto produto)
    {
        await _repository.Update(produto);
    }

    public async void Dispose()
    {
        _repository?.Dispose();
    }
}