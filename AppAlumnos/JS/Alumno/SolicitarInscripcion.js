$(document).ready(function init() {
    var AlumnoId = undefined;

    var Funciones = {
        btnBuscarAlumnoonClick: function () {
            AlumnoId = $('#txtAlumno').val();
            if (AlumnoId.length > 0) {
                if (!isNaN(AlumnoId)) { Funciones.TraerAlumno(); }
                else { Funciones.BuscarNombre(AlumnoId); }
            }
            else { return false; }
        },
        BuscarNombre: function (Nombre) {
            $.ajax({
                url: 'WS/Alumno.asmx/BuscarAlumnoString',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{Filtro:"' + Nombre + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
                        $('#frmVarios').show();
                        tblAlumnos = $('#tblAlumnos').dataTable({
                            "aaData": data.d,
                            "aoColumns": [
                                { "mDataProp": "AlumnoId" },
                                { "mDataProp": "Nombre" },
                                { "mDataProp": "FechaRegistro" },
                                { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                //{ "mDataProp": "FechaSeguimiento" },
                                {
                                    "mDataProp": function (data) {
                                        return "<a class='btn green'>Seleccionar</a>";
                                    }
                                }
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": false,
                            "ordering": false,
                            "async": true,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bLengthChange": false,
                            "bFilter": false,
                            "bInfo": false,
                            "pageLength": 5,
                            "bAutoWidth": false,
                            "asStripClasses": null,
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
                    }
                    $('#Load').modal('hide');

                }
            });
        },
        txtAlumnoonKeyDown: function (e) {
            if (e.which === 13) {
                Funciones.btnBuscarAlumnoonClick();
            }
        },
        TraerAlumno: function () {

        },
        init: function () {

        }
    };

    Funciones.init();
});