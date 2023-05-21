using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;

namespace Desapegando.Business.Services
{
    public class ProdutoImagemService : IProdutoImagemService, IDisposable
    {
        private readonly IProdutoImagemRepository _repository;

        public ProdutoImagemService(IProdutoImagemRepository repository)
        {
            _repository = repository;
        }

        public async Task Create(ProdutoImagem produtoImagem)
        {
            await _repository.Create(produtoImagem);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task Update(ProdutoImagem produtoImagem)
        {
            await _repository.Update(produtoImagem);
        }

        public async void Dispose()
        {
            _repository?.Dispose();
        }
    }
}
