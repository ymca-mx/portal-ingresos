$(function init() {
    CargarDocumento();
    function CargarDocumento() {
        $('#PopLoad').modal('show');
        var Alumno = $.cookie('user');
        $.ajax({
            url: 'Services/General.asmx/NombreCalendario',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{Alumno:"' + Alumno + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == null) { return null; }
                CargarPDF(data.d);
            }
        });
    }
    function CargarPDF(Nombre) {       
        $('#filepdf')
            .attr('src', Nombre);
    }
    $('#filepdf').load(function () {
        $('#PopLoad').modal('hide');
    });
});