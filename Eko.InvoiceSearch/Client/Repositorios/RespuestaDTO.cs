namespace Eko.InvoiceSearch.Client.Repositorios
{
    public class RespuestaDTO<T>
    {
        public bool Error { get; set; }
        public T Response { get; set; }
        public HttpResponseMessage responseMessage { get; set; }

        public RespuestaDTO(T response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            responseMessage = httpResponseMessage;
        }
    }
}
