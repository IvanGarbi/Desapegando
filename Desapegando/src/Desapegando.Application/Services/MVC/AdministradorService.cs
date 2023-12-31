using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Microsoft.Extensions.Options;

namespace Desapegando.Application.Services.MVC
{
    public interface IAdministradorService
    {
        Task<ResponseResult> AtivarCondomino(Guid id);
        Task<ResponseResult> ExcluirCondomino(Guid id);
    }

    public class AdministradorService : Service, IAdministradorService
    {
        public HttpClient _httpClient { get; private set; }

        public AdministradorService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        }

        public async Task<ResponseResult> AtivarCondomino(Guid id)
        {
            var ativarCondominoContent = GetContent(id);

            var response = await _httpClient.PostAsync("Administrador/AtivarCondomino/", ativarCondominoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> ExcluirCondomino(Guid id)
        {
            var excluirCondominoContent = GetContent(id);

            var response = await _httpClient.PostAsync("Administrador/ExcluirCondomino/", excluirCondominoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
