$(function init() {
    CargarDocumento();
    function CargarDocumento() {
        $('#PopLoad').modal('show');
        $.ajax({
            url: 'Api/General/NombreCalendario/' + localStorage.getItem("user"),
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data == null) {
                    $('#PopLoad').modal('hide');
                    return null;
                }
                CargarPDF(data);
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