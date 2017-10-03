$(function init() {
    TraerAlumno();
    
    function TraerAlumno() {
        $('#PopLoad').modal('show');
        var AlumnoId = $.cookie('user');
        //var AlumnoId = '9579';
        $.ajax({
            url: 'Services/Alumno.asmx/TraerSede',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                OpenFile(data.d);
            }
        });
    }
    function OpenFile(Ruta) {
        $('#filepdf')
            .attr('src', Ruta);
    }

    $('#filepdf').load(function () {
        $('#PopLoad').modal('hide');
    });
});
