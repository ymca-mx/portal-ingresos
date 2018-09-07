$(function () {
    var AlumnoId,
        PeriodoId,
        Anio,
        tblPermitidos,
        PeriodoAlcorriente = null,
        Periodo = null,
        Tipo;

    var AlumnoPerFn = {
        init() {
            $('#btnBuscar').on('click', this.btnBuscar);
            $('#btnLiberar').on('click', this.btnLiberar);
            $('#txtClave').on('keydown', this.txtClaveEnter);
            $('#divBuscar').hide();
        },
        BuscarAlumno() {
            $('#btnLiberar').removeAttr("disabled");
            IndexFn.Block(true);
            IndexFn.Api("Alumno/ConsultarAlumno2/" + AlumnoId, "get", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    if (data === null) {
                        $("#divBuscar").hide();
                        $('#lblNombre').text('');
                    } else if (data.AlumnoId === 0) {
                        alertify.alert(data.Nombre);
                        AlumnoPerFn.CargarTabla(data.lstBitacora);
                        $('#btnLiberar').attr("disabled", "disabled");
                        $("#divBuscar").show();
                    } else {
                        $('#lblNombre').text(data.Nombre + " " + data.Paterno + " " + data.Materno);
                        AlumnoPerFn.CargarTabla(data.lstBitacora);
                        $("#divBuscar").show();
                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Universidad YMCA", "Error al buscar al alumno.");
                    console.log(data);
                });
        },
        CargarTabla(Lista) {
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
        },
        btnBuscar() {
            $("#divBuscar").hide();
            AlumnoId = $('#txtClave').val();
            if (AlumnoId.length == 0) { return false; }
            AlumnoPerFn.BuscarAlumno();
        },
        btnLiberar() {
            var Usuario = localStorage.getItem('userAdmin');
            var Descripcion = $('#txtComentario').val();
            var count = Descripcion.trim();
            if (count.length < 5) {
                alertify.alert("Error: Minimo 5 letras");
                return false;
            }

            var data = {
                AlumnoId: AlumnoId,
                UsuarioId: Usuario,
                Descripcion: Descripcion
            };

            IndexFn.Block(true);

            IndexFn.Api("Alumno/InsertarPermiso", "Put", JSON.stringify(data))
                .done(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Guardado, ahora el Alumno puede generar sus referencias.");
                    $('#txtComentario').val('');
                    AlumnoPerFn.BuscarAlumno();
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("No se guardaron los cambios, intente de nuevo");
                    console.log(data);
                });
        },
        txtClaveEnter(e) {
            if (e.which === 13) {
                AlumnoPerFn.btnBuscar();
            }
        }
    };

    AlumnoPerFn.init();
});