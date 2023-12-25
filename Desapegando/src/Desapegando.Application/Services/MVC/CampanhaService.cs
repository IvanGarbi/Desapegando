using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Microsoft.Extensions.Options;

namespace Desapegando.Application.Services.MVC
{
    public interface ICampanhaService
    {
        Task<GetCampanhaResponseId> GetCampanha(Guid id);
        Task<GetAllCampanhaResponse> GetCampanhas();
        Task<GetMinhasCampanhasResponse> GetMinhasCampanhas(Guid id);
        Task<ResponseResult> DeletarCampanha(Guid id);
        Task<ResponseResult> CriarCampanha(PostCampanhaViewModel campanhaViewModel);
        Task<ResponseResult> UpdateCampanha(PatchCampanhaViewModel updateCampanhaViewModel);
    }

    public class CampanhaService : Service, ICampanhaService
    {
        public HttpClient _httpClient { get; private set; }

        public CampanhaService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        }

        public async Task<GetCampanhaResponseId> GetCampanha(Guid id)
        {
            var response = await _httpClient.GetAsync("Campanha/" + id);

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetCampanhaResponseId>(response);
        }

        public async Task<GetAllCampanhaResponse> GetCampanhas()
        {
            var response = await _httpClient.GetAsync("Campanha");

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetAllCampanhaResponse>(response);
        }

        public async Task<GetMinhasCampanhasResponse> GetMinhasCampanhas(Guid id)
        {
            var response = await _httpClient.GetAsync("Campanha/MinhasCampanhas/" + id);

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetMinhasCampanhasResponse>(response);
        }

        public async Task<ResponseResult> DeletarCampanha(Guid id)
        {
            var response = await _httpClient.DeleteAsync("Campanha/" + id);

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<ResponseResult>(response);
        }

        public async Task<ResponseResult> CriarCampanha(PostCampanhaViewModel campanhaViewModel)
        {
            var campanhaContent = GetContent(campanhaViewModel);

            var response = await _httpClient.PostAsync("Campanha/", campanhaContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateCampanha(PatchCampanhaViewModel updateCampanhaViewModel)
        {
            var campanhaContent = GetContent(updateCampanhaViewModel);

            var response = await _httpClient.PatchAsync("Campanha/", campanhaContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }
    }
}
