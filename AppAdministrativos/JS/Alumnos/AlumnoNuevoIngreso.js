$(function init() {
    console.log("Estoy dentro del JS ALumnos Nuevos....");
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
                    "order": [[0, "desc"]]
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
    var objGuardar = {
        AlumnoId:0,
        Nombre:'',
        Paterno:'',
        Materno:'',
        Nacimiento:'',
        GeneroId:0,
        CURP:'',
        UsuarioId:0
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
                    objGuardar.AlumnoId = data.d.AlumnoId;
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
        var $frm = $('#frmDatos');
        if ($frm[0].checkValidity()) {
            /* submit the form */
            $('#Load').modal('show');
            objGuardar.Nombre = $('#txtNombre').val();
            objGuardar.Paterno = $('#txtPaterno').val();
            objGuardar.Materno = $('#txtMaterno').val();
            objGuardar.Nacimiento = $('#txtFecha').val();
            objGuardar.GeneroId = $('#slcSexo').val();
            objGuardar.CURP = $('#txtCURP').val();
            objGuardar.UsuarioId = $.cookie('userAdmin');
            Guardar(JSON.stringify(objGuardar));
        }
    });

    function Guardar(Datos) {
        $.ajax({
            url: 'WS/Alumno.asmx/AtualizarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: Datos,
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $('#ModalDatos').modal('hide');
                    $('#Load').modal('hide');
                    alertify.alert("Se guardo correctamente", function () {
                        TraerAlumnos();
                    });                    
                } else {
                    alertify.alert("Ocurrio un error al guardar, intente nuevamente.");
                    $('#Load').modal('hide');
                }
            },
            error: function () {
                alertify.alert("Ocurrio un error al guardar, intente nuevamente.");
                $('#Load').modal('hide');
            }
        });
    }
});