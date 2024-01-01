using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Models;
using Microsoft.Extensions.Options;

namespace Desapegando.Application.Services.MVC
{
    public interface ICompraService
    {
        Task<GetAllCompraResponse> GetCompras();
        Task<GetCompraResponse> GetMinhasCompras(Guid id);
        Task<ResponseResult> CriarCompra(CompraViewModel compraViewModel);
    }

    public class CompraService : Service, ICompraService
    {
        public HttpClient _httpClient { get; private set; }

        public CompraService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        }

        public async Task<GetCompraResponse> GetMinhasCompras(Guid id)
        {
            var response = await _httpClient.GetAsync("Compra/MinhasCompras/" + id);

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetCompraResponse>(response);
        }

        public async Task<ResponseResult> CriarCompra(CompraViewModel compraViewModel)
        {
            var compraContent = GetContent(compraViewModel);

            var response = await _httpClient.PostAsync("Compra/", compraContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<GetAllCompraResponse> GetCompras()
        {
            var response = await _httpClient.GetAsync("Compra");

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetAllCompraResponse>(response);
        }
    }
}
