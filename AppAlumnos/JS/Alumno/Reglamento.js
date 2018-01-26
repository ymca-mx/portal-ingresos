$(function init() {
    TraerAlumno();
    
    function TraerAlumno() {
        $('#PopLoad').modal('show');
        //var AlumnoId = '9579';
        $.ajax({
            url: 'Api/Alumno/TraerSede/' + localStorage.getItem("user"),
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                OpenFile(data);
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
