$(function () {
    var AlumnoNum, tblAlumnos, tblDatos;
    UsuarioId =  localStorage.getItem('userAdmin');

    var fnAlumnoDatos =
        {
            init() {
                GlobalFn.init()
                GlobalFn.GetGenero();
                $("#btnBuscarAlumno").click(fnAlumnoDatos.buscarAlumno);

                $('#tblAlumnos').on('click', 'a', fnAlumnoDatos.seleccionarAlumno);
                
                $('#txtAlumno').on('keydown', function (e) {
                    if (e.which == 13) {
                        $('#btnBuscarAlumno').click();
                    }
                });
            },
            buscarAlumno() {
                $("#divGuardar").hide();
                $('#frmVarios').hide();
                if (tblAlumnos != undefined) {
                    tblAlumnos.fnClearTable();
                }
                if ($('#txtAlumno').val().length == 0) { return false; }
                $("#submit_form").trigger('reset');
                IndexFn.Block(true);
                AlumnoNum = $('#txtAlumno').val();

                if (!isNaN(AlumnoNum)) {
                    fnAlumnoDatos.esNumero(AlumnoNum);
                } else {
                    fnAlumnoDatos.esString(AlumnoNum);
                }
            },
            seleccionarAlumno() {
                $('#frmVarios').hide();
                $('#frmTabs').show();
                IndexFn.Block(true);
                var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
                AlumnoNum = rowadd.AlumnoId;
                fnAlumnoDatos.esNumero(AlumnoNum);
            },
            esNumero(Alumno) {
                IndexFn.Api('Alumno/ObenerDatosAlumnoTodos/' + Alumno, "GET", "")
                    .done(function (data) {
                        if (data != null) {
                           
                            ///Personales
                            document.getElementById("fotoAlumno").src = "data:image/png;base64," + data.fotoBase64;
                            $('#txtnombre').val(data.Nombre + " " + data.Paterno + " " + data.Materno);
                            $('#txtFNacimiento').val(data.FechaNacimientoC);
                            $('#txtCURP').val(data.CURP);
                            $('#slcSexo').val(data.GeneroId);
                            tblDatos = $('#tblDatos').dataTable({
                                "aaData": data.DatosContacto,
                                "aoColumns": [
                                    { "mDataProp": "Dato" },
                                    { "mDataProp": "ServiciosEscolares" }
                                ],
                                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                                "searching": false,
                                "ordering": false,
                                "async": true,
                                "bDestroy": true,
                                "bPaginate": false,
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
                                "order": [[1, "desc"]],
                                "createdRow": function (row, data, dataIndex) {
                                    row.childNodes[0].style.borderTopStyle = 'Solid';
                                    row.childNodes[0].style.borderTopColor = '#3598dc';
                                    row.childNodes[0].style.backgroundColor = "#666666";
                                    row.childNodes[0].style.color = "#fff";
                                }
                            });
                            $('#frmTabs').show();
                            IndexFn.Block(false);
                        }
                        else {
                            $('#frmTabs').hide();
                            IndexFn.Block(false);
                            alertify.alert("Error, El Alumno no Existe.");
                        }
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                    });
                
            },
            esString(Alumno) {
                $('#frmTabs').hide();
                IndexFn.Api("Alumno/BuscarAlumnoString/" + Alumno, "GET", "")
                    .done(function (data) {
                        if (data != null) {
                            $('#frmVarios').show();
                            tblAlumnos = $('#tblAlumnos').dataTable({
                                "aaData": data,
                                "aoColumns": [
                                    { "mDataProp": "AlumnoId" },
                                    { "mDataProp": "Nombre" },
                                    { "mDataProp": "FechaRegistro" },
                                    { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
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
                        IndexFn.Block(false);
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                    });
            }
        };
    fnAlumnoDatos.init();
});