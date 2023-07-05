using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Notifications;
using Desapegando.Business.Validations;

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
        var validator = new CampanhaValidation();
        var resultValidation = validator.Validate(campanha);

        if (!resultValidation.IsValid)
        {
            foreach (var error in resultValidation.Errors)
            {
                _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
            }

            return;
        }

        await _repository.Create(campanha);
    }

    public async Task Delete(Guid id)
    {
        var campanhaDb = await _repository.ReadById(id);

        if (campanhaDb == null)
        {
            _notificador.AdicionarNotificacao(new Notificacao("Nenhuma campanha identificada."));
            return;
        }

        campanhaDb.Ativo = false;

        await _repository.Update(campanhaDb);

        //await _repository.Delete(id);
    }

    public async Task Update(Campanha campanha)
    {
        var validator = new CampanhaValidation();
        var resultValidation = validator.Validate(campanha);

        if (!resultValidation.IsValid)
        {
            foreach (var error in resultValidation.Errors)
            {
                _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
            }

            return;
        }

        await _repository.Update(campanha);
    }

    public async void Dispose()
    {
        _repository?.Dispose();
    }
}
