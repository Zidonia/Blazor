namespace Eko.InvoiceSearch.Client.Repositorios
{
    public interface IRepositorios
    {
        Task<RespuestaDTO<T>> Get<T>(string url);
        Task<RespuestaDTO<object>> Post<T>(string url, T enviar);
        Task<RespuestaDTO<TResponse>> Post<T, TResponse>(string url, T enviar);
    }
}
