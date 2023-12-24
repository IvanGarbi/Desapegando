using Desapegando.Application.Extensions;
using Desapegando.Application.ViewModels;
using Desapegando.Business.Models;
using Microsoft.Extensions.Options;

namespace Desapegando.Application.Services.MVC
{
    public interface IProdutoService
    {
        Task<GetProdutoResponseId> GetProduto(Guid id);
        Task<GetAllProdutoResponse> GetProdutos();
        Task<GetMeusProdutoResponse> GetMeusProdutos(Guid id);
        Task<ResponseResult> DeletarProduto(Guid id);
        Task<ResponseResult> CriarProduto(PostProdutoViewModel produtoViewModel);
        Task<ResponseResult> UpdateProduto(PatchProdutoViewModel updateProdutoViewModel);
        Task<ResponseResult> CurtirProduto(CurtidaViewModel curtidaViewModel);
        Task<ResponseResult> DescurtirProduto(DescurtidaViewModel descurtidaViewModel);
        Task<ResponseResult> RemoverProduto(RemoverProdutoViewModel removerProdutoViewModel);
    }

    public class ProdutoService : Service, IProdutoService
    {
        public HttpClient _httpClient { get; private set; }

        public ProdutoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            httpClient.BaseAddress = new Uri(settings.Value.DesapegandoApiUrl);
        }

        public async Task<GetProdutoResponseId> GetProduto(Guid id)
        {
            var response = await _httpClient.GetAsync("Produto/" + id);

            VerifyResponseErros(response);
            
            return await DeserializeObjectResponse<GetProdutoResponseId>(response);
        }

        public async Task<GetAllProdutoResponse> GetProdutos()
        {
            var response = await _httpClient.GetAsync("Produto");

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetAllProdutoResponse>(response);
        }

        public async Task<ResponseResult> DeletarProduto(Guid id)
        {
            var response = await _httpClient.DeleteAsync("Produto/" + id);

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<ResponseResult>(response);
        }

        public async Task<ResponseResult> CriarProduto(PostProdutoViewModel produtoViewModel)
        {
            var produtoContent = GetContent(produtoViewModel);

            var response = await _httpClient.PostAsync("Produto/", produtoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> UpdateProduto(PatchProdutoViewModel updateProdutoViewModel)
        {
            var produtoContent = GetContent(updateProdutoViewModel);

            var response = await _httpClient.PatchAsync("Produto/", produtoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> CurtirProduto(CurtidaViewModel curtidaViewModel)
        {
            var produtoContent = GetContent(curtidaViewModel);

            var response = await _httpClient.PostAsync("Produto/Curtir/", produtoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> DescurtirProduto(DescurtidaViewModel descurtidaViewModel)
        {
            var produtoContent = GetContent(descurtidaViewModel);

            var response = await _httpClient.PostAsync("Produto/Descurtir/", produtoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<ResponseResult> RemoverProduto(RemoverProdutoViewModel removerProdutoViewModel)
        {
            var produtoContent = GetContent(removerProdutoViewModel);

            var response = await _httpClient.PostAsync("Produto/RemoverProduto/", produtoContent);

            if (!VerifyResponseErros(response)) return await DeserializeObjectResponse<ResponseResult>(response);

            return ReturnOk();
        }

        public async Task<GetMeusProdutoResponse> GetMeusProdutos(Guid id)
        {
            var response = await _httpClient.GetAsync("Produto/MeusProdutos/" + id);

            VerifyResponseErros(response);

            return await DeserializeObjectResponse<GetMeusProdutoResponse>(response);
        }
    }
}
