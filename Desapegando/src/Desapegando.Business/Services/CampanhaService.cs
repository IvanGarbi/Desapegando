using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;

namespace Desapegando.Business.Services;

public class CampanhaService : ICampanhaService, IDisposable
{
    private readonly ICampanhaRepository _repository;

    public CampanhaService(ICampanhaRepository repository)
    {
        _repository = repository;
    }

    public async Task Create(Campanha campanha)
    {
        await _repository.Create(campanha);
    }

    public async Task Delete(Guid id)
    {
        await _repository.Delete(id);
    }

    public async Task Update(Campanha campanha)
    {
        await _repository.Update(campanha);
    }

    public async void Dispose()
    {
        _repository?.Dispose();
    }
}
