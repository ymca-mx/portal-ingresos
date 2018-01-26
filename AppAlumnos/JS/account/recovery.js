$(document).ready(function () {

    //Obtener TokenId
    var url = document.URL;
    //var url = $('#divDinamico').data('url');
    var restante = url.split('?')[1];
    var GET = restante.split('&');
    var parametros = {};
    for (var i = 0; i < GET.length; i++) {
        var tmp = GET[i].split('=');

        //Guardar los parametros : Si se quieren mostrar
        parametros[tmp[0]] = unescape(decodeURI(tmp[1]));
    }

    urlParametros = {
        token: parametros["recoveryToken"]
    };

    var datos;
    //alert("El parametro es: " + urlParametros["token"]);



    function RPass() {
        $.ajax({
            url: '../Services/Alumno.asmx/ActualizaPassword',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: datos,
            dataType: 'json'
        }).done(function (resultado) {
            alert('La contraseña de tu cuenta ha sido actualizada correctamente.');
            $(location).attr('href', 'http://108.163.172.122/portalalumno/login.html');
        }).fail(function (jqXHR, textStatus) {
            alert('Ocurrio un error...' + textStatus);
        });
    }


    $(function () {
        $('#form_recovery').submit(function () {
            if ($(this).valid()) {

                datos = "{'password':'" + $('#txtpassword').val() +
                "', 'token':'" + urlParametros['token'] + "'}";
                
                RPass();
            }
        });
    });
});


