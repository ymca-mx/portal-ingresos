var Listado = function () {
    var CargarListado = function () {
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumnos',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (Respuesta) {
                MItable = $('#tbProspectos').dataTable({
                    "aaData": Respuesta.d,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId", "Nombre" : "AlumnoId", visible: false },
                        {
                            "mDataProp": "Nombre",
                            "mRender": function (data) {
                                return "<a href=''>" + data + " </a> ";
                            }
                        },
                        { "mDataProp": "FechaRegistro" },
                        { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                        //{ "mDataProp": "FechaSeguimiento" },
                        { "mDataProp": "Usuario.Nombre" }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "info": false,
                    "language": {
                        "lengthMenu": "_MENU_  Registros",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno "
                    },
                    "order": [[2, "desc"]]
                });
            },
            error: function (Respuesta) {
                alert('Error al cargar datos');
            }
        });
    }
    return {
        init: function () {
            CargarListado();
        }
    };
}();