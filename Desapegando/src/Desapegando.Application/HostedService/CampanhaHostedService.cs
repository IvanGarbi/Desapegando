using Desapegando.Business.Interfaces.Repository;

namespace Desapegando.Application.HostedService
{
    public class CampanhaHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer = null;

        public CampanhaHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteProcess, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void ExecuteProcess(object state)
        {
            Console.WriteLine("### Proccess executing ###");
            Console.WriteLine($"{DateTime.Now}");

            await AtualizarCampanhas();
        }


        private async Task AtualizarCampanhas()
        {
            using var scope = _scopeFactory.CreateScope();

            var _campanhaRepository = scope.ServiceProvider.GetService<ICampanhaRepository>();

            var capanhasDb = await _campanhaRepository.ReadExpression(x => x.DataFinal.Date == DateTime.Today && x.Ativo == true);

            foreach (var campanha in capanhasDb)
            {
                campanha.Ativo = false;
                await _campanhaRepository.Update(campanha);
            }

            scope.Dispose();

            return;
        }
    }
}
