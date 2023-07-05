using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Notifications;
using Desapegando.Business.Validations;

namespace Desapegando.Business.Services;

public class ProdutoService : IProdutoService, IDisposable
{
    private readonly IProdutoRepository _repository;
    private readonly INotificador _notificador;

    public ProdutoService(IProdutoRepository repository, INotificador notificador)
    {
        _repository = repository;
        _notificador = notificador;
    }

    public async Task Create(Produto produto)
    {
        produto.DataPublicacao = DateTime.Now;

        var validator = new ProdutoValidation();
        var resultValidation = validator.Validate(produto);

        if (!resultValidation.IsValid)
        {
            foreach (var error in resultValidation.Errors)
            {
                _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
            }

            return;
        }

        await _repository.Create(produto);
    }

    public async Task Delete(Guid id)
    {
        var produtoDb = await _repository.ReadById(id);

        if (produtoDb == null)
        {
            _notificador.AdicionarNotificacao(new Notificacao("Nenhum produto identificado."));
            return;
        }

        produtoDb.Ativo = false;

        await _repository.Update(produtoDb);

        //await _repository.Delete(id);
    }

    public async Task Update(Produto produto)
    {
        var validator = new ProdutoValidation();
        var resultValidation = validator.Validate(produto);

        if (!resultValidation.IsValid)
        {
            foreach (var error in resultValidation.Errors)
            {
                _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
            }

            return;
        }

        await _repository.Update(produto);
    }

    public async void Dispose()
    {
        _repository?.Dispose();
    }
}