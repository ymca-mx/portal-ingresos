$(document).ready(function () {
    var AlumnoId;
    var PeriodoId;
    var Anio;
    var tblPermitidos;
    var PeriodoAlcorriente = null;
    var Periodo = null;
    var Tipo;
    $('#btnBuscar').click(function () {
        $("#divBuscar").hide();
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0) { return false; }
        BuscarAlumno();
    });
    function BuscarAlumno() {
        $('#btnLiberar').removeAttr("disabled");
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultarAlumno2",
            data: "{AlumnoId:'" + AlumnoId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d == null) {
                    $("#divBuscar").hide();
                    $('#lblNombre').text('');
                    return false;
                } else if (data.d.AlumnoId == 0) {
                    alertify.alert(data.d.Nombre);
                    CargarTabla(data.d.lstBitacora);
                    $('#btnLiberar').attr("disabled", "disabled");
                    $("#divBuscar").show();
                    return false;
                } else {
                    $('#lblNombre').text(data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno);
                    CargarTabla(data.d.lstBitacora);
                    $("#divBuscar").show();
                }
            }
        });
    }
    function CargarTabla(Lista) {
        tblPermitidos = $('#tblPermitidos').dataTable({
            "aaData": Lista,
            "aoColumns": [
                { "mDataProp": "AlumnoId" },
                { "mDataProp": "Anio" },
                { "mDataProp": "PeriodoId" },
                { "mDataProp": "Descripcion" },
                { "mDataProp": "FechaRegistroS" },
                { "mDataProp": "HoraRegistroS" },
                { "mDataProp": "UsuarioId" }
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
                "search": "Buscar Registro "
            },
            "order": [[2, "desc"]]
        });
    }
    $('#btnLiberar').click(function () {
        var Usuario = $.cookie('userAdmin');
        var Descripcion = $('#txtComentario').val();
        var count = Descripcion.trim();
        if (count.length < 5) {
            alertify.alert("Error: Minimo 5 letras");
            return false;
        }
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/InsertarPermiso",
            data: "{AlumnoId:'" + AlumnoId + "',UsuarioId:'" + Usuario + "',Descripcion:'" + Descripcion + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d == null) {
                    alertify.alert("No se guardaron los cambios, intente de nuevo");
                    return false;
                }
                alertify.alert("Guardado, ahora el Alumno puede generar sus referencias.");
                $('#txtComentario').val('');
                BuscarAlumno();
            }
        });
    });
});