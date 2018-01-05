$(document).ready(function () {
    var datos;

    datos = "{'alumnoId':'" + $.cookie('user') + "'}";

    $.ajax({
        url: 'Services/Alumno.asmx/InsertaBitacora',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: datos,
        dataType: 'json'
    }).done(function (resultado) {
        
    }).fail(function (jqXHR, textStatus) {
        $(location).attr('href', 'login.html');
    });
});