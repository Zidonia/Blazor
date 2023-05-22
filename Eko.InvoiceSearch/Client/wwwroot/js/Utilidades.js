function pruebaPuntoNetStaic() {
    DotNet.invokeMethodAsync("Eko.InvoiceSearch.Client", "ObtenerCurrentCount")
        .then(resultado => {
            console.log('conteo desde javascript' + resultado);
        })
}

function timerInactivo(dotnetHelper) {
    var timer;

    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;

    function resetTimer() {
        clearTimeout(timer);
        timer = setTimeout(logout, 3*1000) //3 segundos
        console.log(timer);
    }

    function logout() {
        dotnetHelper.invokeMethodAsync("Logout");
    }
}