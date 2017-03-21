$(document).ready(function () {
    var datos;

    datos = "{'usuarioId':'" + $.cookie('userAdmin') + "'}";
  
    $.ajax({
        url: 'Services/Usuario.asmx/InsertaBitacora',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: datos,
        dataType: 'json'
    }).done(function (resultado) {
        
    }).fail(function (jqXHR, textStatus) {
       
    });
});