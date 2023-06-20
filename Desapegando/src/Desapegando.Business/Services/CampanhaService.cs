using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;

namespace Desapegando.Business.Services;

public class CampanhaService : ICampanhaService, IDisposable
{
    private readonly ICampanhaRepository _repository;
    private readonly INotificador _notificador;

    public CampanhaService(ICampanhaRepository repository, INotificador notificador)
    {
        _repository = repository;
        _notificador = notificador;
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
