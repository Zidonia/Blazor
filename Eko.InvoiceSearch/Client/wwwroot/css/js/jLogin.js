function ValidaUsuarioPass() {
    DotNet.invokeMethodAsync("Eko.InvoiceSearch.Client", "Ingreso").then(resultado => {
        var usuario = $("#usuario").val();
        var pass = $("#password").val();
    })
}