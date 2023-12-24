using Desapegando.Application.ViewModels;
using System.Text;
using System.Text.Json;

namespace Desapegando.Application.Services.MVC
{
    public abstract class Service
    {
        protected StringContent GetContent(object data)
        {
            return new StringContent(
                        JsonSerializer.Serialize(data),
                        Encoding.UTF8,
                        "application/json");
        }

        protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool VerifyResponseErros(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                // adicionar erros quando necessário
                case 400:
                    return false;
                case 500:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult ReturnOk()
        {
            return new ResponseResult();
        }
    }
}
