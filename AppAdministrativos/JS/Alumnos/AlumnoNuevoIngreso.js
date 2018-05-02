$(function() {
    var tblAlumnos;

    var NuevoIngreFn = {
        init() {
            $('#tblAlumnos').on('click', 'a', this.tblAlumnosClickA);
            $('#btnCancelar').on('click', this.btnCancelarClick);
            $('#btnGuardarDatos').on('click', this.btnGuardarDatosClick);
            
            GlobalFn.init();
            GlobalFn.GetGenero();
            this.TraerAlumnos();
        },
        objGuardar: { AlumnoId: -1 },
        TraerAlumnos() {
            $('#Load').modal('show');
            IndexFn.Api('Alumno/ConsultarAlumnosNuevos/', "GET", "")
                .done(function (Respuesta) {
                    tblAlumnos = $('#tblAlumnos').dataTable({
                        "aaData": Respuesta,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId" },
                            { "mDataProp": "Nombre" },
                            { "mDataProp": "FechaRegistro" },
                            { "mDataProp": "Descripcion" },
                            { "mDataProp": "Usuario" },
                            {
                                "mDataProp": function (data) {
                                    return "<a name='Personales' href=''onclick='return false;' class='btn btn-success'> Editar </a> ";
                                }
                            },
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
                })
                .fail(function (Respuesta) {
                    alertify.alert('Error al cargar datos');
                });
        },
        Guardar(Datos) {
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
                            NuevoIngreFn.TraerAlumnos();
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
        },
        tblAlumnosClickA() {
            $('#Load').modal('show');
            var fid = tblAlumnos.fnGetData(this.parentNode.parentNode, 0);
            
                NuevoIngreFn.TraerDatosPersonales(fid);
        },
        TraerDatosPersonales(AlumnoId) {
            IndexFn.Api('Alumno/ConsultarAlumno/' + AlumnoId, "GET", "")
                .done(function (data) {
                    if (data !== null) {
                        NuevoIngreFn.objGuardar.AlumnoId = data.AlumnoId;
                        $('#ModalDatos').modal('show');
                        $('#txtNombre').val('');
                        $('#txtNombre').val(data.Nombre);
                        $('#txtPaterno').val('');
                        $('#txtPaterno').val(data.Paterno);
                        $('#txtMaterno').val('');
                        $('#txtMaterno').val(data.Materno);
                        $('#txtFecha').val('');
                        $('#txtFecha').val(data.DTOAlumnoDetalle.FechaNacimientoC);
                        $('#slcSexo').val(data.DTOAlumnoDetalle.GeneroId);
                        $('#txtCURP').val('');
                        $('#txtCURP').val(data.DTOAlumnoDetalle.CURP);

                    }
                    $('#Load').modal('hide');
                });
        },        
        btnCancelarClick() {
            $('#ModalDatos').modal('hide');
        },
        btnGuardarDatosClick() {
            var $frm = $('#frmDatos');
            if ($frm[0].checkValidity()) {
                $('#Load').modal('show');
                var objGuardar = {
                    AlumnoId: NuevoIngreFn.objGuardar.AlumnoId,
                    Nombre: $('#txtNombre').val(),
                    Paterno: $('#txtPaterno').val(),
                    Materno: $('#txtMaterno').val(),
                    Nacimiento: $('#txtFecha').val(),
                    GeneroId: $('#slcSexo').val(),
                    CURP: $('#txtCURP').val(),
                    UsuarioId: $.cookie('userAdmin'),
                };
                NuevoIngreFn.Guardar(JSON.stringify(objGuardar));
            }
        },
       
    };

    NuevoIngreFn.init();
});