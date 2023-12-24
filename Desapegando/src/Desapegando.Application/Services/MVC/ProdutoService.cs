using Desapegando.Application.Extensions;
using Microsoft.Extensions.Options;

namespace Desapegando.Application.Services.MVC
{
    public class ProdutoService
    {
        public HttpClient _httpClient { get; private set; }

        public ProdutoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        }
    }
}
