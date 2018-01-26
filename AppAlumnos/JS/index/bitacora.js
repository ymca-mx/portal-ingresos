$(document).ready(function () {

    $.ajax({
        url: 'Api/Alumno/InsertaBitacora/' + localStorage.getItem('user'),
        type: 'Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json'
    }).done(function (resultado) {
        
    }).fail(function (jqXHR, textStatus) {
        $(location).attr('href', 'login.html');
    });
});