using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Notifications;

namespace Desapegando.Business.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _repository;
        private readonly INotificador _notificador;

        public CompraService(ICompraRepository repository, INotificador notificador)
        {
            _repository = repository;
            _notificador = notificador;
        }

        public async Task Create(Compra compra)
        {
            //var validator = new CompraValidation();
            //var resultValidation = validator.Validate(compra);

            //if (!resultValidation.IsValid)
            //{
            //    foreach (var error in resultValidation.Errors)
            //    {
            //        _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
            //    }

            //    return;
            //}

            await _repository.Create(compra);
        }

        public async Task Delete(Guid id)
        {
            var compraDb = await _repository.ReadById(id);

            if (compraDb == null)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Nenhuma compra identificada."));
                return;
            }

            await _repository.Delete(id);
        }

        public async Task Update(Compra compra)
        {
            //var validator = new CompraValidation();
            //var resultValidation = validator.Validate(compra);

            //if (!resultValidation.IsValid)
            //{
            //    foreach (var error in resultValidation.Errors)
            //    {
            //        _notificador.AdicionarNotificacao(new Notificacao(error.ErrorMessage, error.PropertyName));
            //    }

            //    return;
            //}

            await _repository.Update(compra);
        }

        public async void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
