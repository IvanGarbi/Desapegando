using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;

namespace Desapegando.Business.Services;

public class CondominoService : ICondominoService, IDisposable
{
    private readonly ICondominoRepository _repository;

    public async Task Create(Condomino condomino)
    {
        await _repository.Create(condomino);
    }

    public async Task Update(Condomino condomino)
    {
        await _repository.Update(condomino);
    }

    public async Task Delete(Guid id)
    {
        await _repository.Delete(id);
    }

    public async void Dispose()
    {
        _repository?.Dispose();
    }
}