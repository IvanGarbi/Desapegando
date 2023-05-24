using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;

namespace Desapegando.Business.Services
{
    public class CampanhaImagemService : ICampanhaImagemService, IDisposable
    {
        private readonly ICampanhaImagemRepository _repository;

        public CampanhaImagemService(ICampanhaImagemRepository repository)
        {
            _repository = repository;
        }

        public async Task Create(CampanhaImagem campanhaImagem)
        {
            await _repository.Create(campanhaImagem);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task Update(CampanhaImagem campanhaImagem)
        {
            await _repository.Update(campanhaImagem);
        }

        public async void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
