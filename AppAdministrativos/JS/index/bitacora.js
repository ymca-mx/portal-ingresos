$(document).ready(function () {
    var datos;
    console.log("soy bitacora");
    datos = { usuarioId: localStorage.getItem('userAdmin') };

    $.ajax({
        url: 'Api/Usuario/InsertaBitacora',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(datos),
        dataType: 'json'
    }).done(function (resultado) {
    }).fail(function (jqXHR, textStatus) {
    });
});