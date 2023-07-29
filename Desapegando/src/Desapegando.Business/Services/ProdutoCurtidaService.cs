using Desapegando.Business.Interfaces.Notifications;
using Desapegando.Business.Interfaces.Repository;
using Desapegando.Business.Interfaces.Services;
using Desapegando.Business.Models;
using Desapegando.Business.Notifications;

namespace Desapegando.Business.Services
{
    public class ProdutoCurtidaService : IProdutoCurtidaService, IDisposable
    {
        private readonly IProdutoCurtidaRepository _produtoCurtidaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly INotificador _notificador;

        public ProdutoCurtidaService(IProdutoCurtidaRepository repository, INotificador notificador, IProdutoRepository produtoRepository)
        {
            _produtoCurtidaRepository = repository;
            _notificador = notificador;
            _produtoRepository = produtoRepository;
        }

        public async Task Create(ProdutoCurtida produtoCurtida)
        {
            await _produtoCurtidaRepository.Create(produtoCurtida);
        }

        public async Task Delete(Guid id)
        {
            await _produtoCurtidaRepository.Delete(id);
        }

        public async Task Curtir(Guid produtoId, Guid condominoId)
        {
            var produtoDb = await _produtoRepository.ReadById(produtoId);

            if (produtoDb == null)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Produto não encontrado"));

                return;
            }

            ProdutoCurtida produtoCurtida = new ProdutoCurtida
            {
                CondominoId = condominoId,
                ProdutoId = produtoId
            };

            await _produtoCurtidaRepository.Create(produtoCurtida);

            produtoDb.Curtida++;

            await _produtoRepository.Update(produtoDb);
        }

        public async Task Descurtir(Guid produtoId, Guid condominoId)
        {
            var produtoDb = await _produtoRepository.ReadById(produtoId);

            if (produtoDb == null)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Produto não encontrado"));

                return;
            }

            var produtoCurtida = await _produtoCurtidaRepository.ReadExpression(x => x.ProdutoId == produtoId && x.CondominoId == condominoId);

            if (produtoCurtida == null || !produtoCurtida.Any())
            {
                _notificador.AdicionarNotificacao(new Notificacao("Erro..."));

                return;
            }

            await _produtoCurtidaRepository.Delete(produtoCurtida.First().Id);

            produtoDb.Curtida--;
            produtoDb.ProdutoCurtidas = null; // colocar nulo para não querer o update do produto no banco. Pois ele atualizaria com o ProdutoCurtidas junto.

            await _produtoRepository.Update(produtoDb);
        }

        public async void Dispose()
        {
            _produtoCurtidaRepository?.Dispose();
        }
    }
}
