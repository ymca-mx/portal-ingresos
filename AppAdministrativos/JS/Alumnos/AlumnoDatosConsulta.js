$(function () {
    var AlumnoNum, tblAlumnos, tblDatos;
    UsuarioId = $.cookie('userAdmin');

    var fnAlumnoDatos =
        {
            init() {
                GlobalFn.init()
                GlobalFn.GetGenero();
                GlobalFn.GetEstado("slcEstado", 9);
                $("#btnBuscarAlumno").click(fnAlumnoDatos.buscarAlumno);

                $('#tblAlumnos').on('click', 'a', fnAlumnoDatos.seleccionarAlumno);

                $('#slcNacionalidad').change(fnAlumnoDatos.nacionalidadChange);

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
                $('#Load').modal('show');
                AlumnoNum = $('#txtAlumno').val();

                if (!isNaN(AlumnoNum)) {
                    fnAlumnoDatos.esNumero(AlumnoNum);
                    $('#frmTabs').show();
                } else {
                    fnAlumnoDatos.esString(AlumnoNum);
                }
            },
            seleccionarAlumno() {
                $('#frmVarios').hide();
                $('#frmTabs').show();
                $('#Load').modal('show');
                var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
                AlumnoNum = rowadd.AlumnoId;
                fnAlumnoDatos.esNumero(AlumnoNum);
            },
            nacionalidadChange() {
                $("#slcLugarN").empty();
                var optionP = $(document.createElement('option'));
                optionP.text('--Seleccionar--');
                optionP.val('-1');
                $("#slcLugarN").append(optionP);

                var tipo = $("#slcNacionalidad");
                tipo = tipo[0].value;
                if (tipo == 2) {
                    GlobalFn.GetPais($("#slcLugarN"), -1);
                }
                else if (tipo == 1) {
                    GlobalFn.GetEstado($("#slcLugarN"), -1);
                }
                else { $("#slcLugarN").append(optionP); }
            },
            esNumero(Alumno) {
                IndexFn.Api('Alumno/ObenerDatosAlumnoTodos/' + Alumno, "GET", "")
                    .done(function (data) {
                        if (data != null) {
                            $('#slcNacionalidad').val(data.PaisId == 146 ? 1 : 2);
                            if (data.PaisId == 146) {
                                GlobalFn.GetEstado('slcLugarN', data.EntidadNacimientoId);
                            } else {
                                GlobalFn.GetPais('slcLugarN', data.PaisId);
                            }
                            ///Personales 

                            var base64_string = data.fotoBase64;
                            document.getElementById("fotoAlumno").src = "data:image/png;base64," + base64_string;

                            $('#txtnombre').val(data.Nombre);
                            $('#txtApPaterno').val(data.Paterno);
                            $('#txtApMaterno').val(data.Materno);
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

                            $('#Load').modal('hide');
                        }
                        else {
                            $('#PopDatosAlumno').modal('hide');
                            $('#Load').modal('hide');
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
                        $('#Load').modal('hide');
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                    });
            }
        };
    fnAlumnoDatos.init();
});