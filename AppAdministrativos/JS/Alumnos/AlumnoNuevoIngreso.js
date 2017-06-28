$(function init() {
    var tblAlumnos;
    TraerAlumnos();
    function TraerAlumnos() {
        $('#Load').modal('show');
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumnosNuevos',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (Respuesta) {
                tblAlumnos = $('#tblAlumnos').dataTable({
                    "aaData": Respuesta.d,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId"}, 
                        { "mDataProp": "Nombre"},
                        { "mDataProp": "FechaRegistro" },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "Usuario" },
                        { "mDataProp": function (data) {
                                return "<a href=''onclick='return false;' class='btn btn-success'> Editar </a> ";
                            }
                     }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "info": false,
                    "async": true,
                    "bDestroy": true,
                    "language": {
                        "lengthMenu": "_MENU_  Registros",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "order": [[2, "desc"]]
                });
                var fil = $('#tblAlumnos_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                $('#Load').modal('hide');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
            }
        });
    }

    $('#tblAlumnos').on('click', 'a', function () {
        $('#Load').modal('show');
        var fid = tblAlumnos.fnGetData(this.parentNode.parentNode, 0);
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + fid + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    $('#ModalDatos').modal('show');
                    $('#txtNombre').val('');
                    $('#txtNombre').val(data.d.Nombre);
                    $('#txtPaterno').val('');
                    $('#txtPaterno').val(data.d.Paterno);
                    $('#txtMaterno').val('');
                    $('#txtMaterno').val(data.d.Materno);
                    $('#txtFecha').val('');
                    $('#txtFecha').val(data.d.DTOAlumnoDetalle.FechaNacimientoC);                    
                    $('#slcSexo').val(data.d.DTOAlumnoDetalle.GeneroId);
                    $('#txtCURP').val('');
                    $('#txtCURP').val(data.d.DTOAlumnoDetalle.CURP);
                    
                }
                $('#Load').modal('hide');
            }
        });
       
    });
    $('#btnCancelar').on('click', function () {
        $('#ModalDatos').modal('hide');
    });
    $('#btnGuardarDatos').on('click', function () {
        var frm = $('#frmDatos');
        if (frm.valid()) {
            alertify.alert("Valido");
        }
    });
});