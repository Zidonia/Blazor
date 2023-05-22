using System.Text.Json;
using System.Text;

namespace Eko.InvoiceSearch.Client.Repositorios
{
    public class Repositorio : IRepositorios
    {
        private readonly HttpClient httpClient;
        public Repositorio(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        private JsonSerializerOptions OpcionesPorDefectoJSON => new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        public async Task<RespuestaDTO<object>> Post<T>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            return new RespuestaDTO<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<RespuestaDTO<TResponse>> Post<T, TResponse>(string url, T enviar)
        {
            var enviarJSON = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJSON, Encoding.UTF8, "application/json");
            var responseHttp = await httpClient.PostAsync(url, enviarContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<TResponse>(responseHttp, OpcionesPorDefectoJSON);
                return new RespuestaDTO<TResponse>(response, false, responseHttp);
            }
            else
            {
                return new RespuestaDTO<TResponse>(default, true, responseHttp);
            }
        }
        private async Task<T> DeserializarRespuesta<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions);
        }

        public async Task<RespuestaDTO<T>> Get<T>(string url)
        {
            var responseHTTP = await httpClient.GetAsync(url);
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await DeserializarRespuesta<T>(responseHTTP, OpcionesPorDefectoJSON);
                return new RespuestaDTO<T>(response, false, responseHTTP);
            }
            else
            {
                return new RespuestaDTO<T>(default, true, responseHTTP);
            }
        }
    }
}
