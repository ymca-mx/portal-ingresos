$(document).ready(function () {
    var AlumnoNum, tblAlumnos, tblDatos;
    UsuarioId = $.cookie('userAdmin');

    $("#btnBuscarAlumno").click(function () {
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
            EsNumero(AlumnoNum);
            $('#frmTabs').show();
        } else {
            EsString(AlumnoNum);
        }
    });

    function EsNumero(Alumno) {

        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ObenerDatosAlumnoTodos",
            data: "{AlumnoId:'" + Alumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    $('#slcNacionalidad').val(data.d.PaisId == 146 ? 1 : 2);
                    if (data.d.PaisId == 146) {
                        CargarEstados($('#slcLugarN'), data.d.EntidadNacimientoId);
                    } else {
                        CargarPaises($('#slcLugarN'), data.d.PaisId);
                    }
                    ///Personales 
                    $('#txtnombre').val(data.d.Nombre);
                    $('#txtApPaterno').val(data.d.Paterno);
                    $('#txtApMaterno').val(data.d.Materno);
                    $('#txtFNacimiento').val(data.d.FechaNacimientoC);
                    $('#txtCURP').val(data.d.CURP);
                    $('#slcSexo').val(data.d.GeneroId);
                    CargarEstados1(data.d.EntidadFederativaId, data.d.MunicipioId);

                    
                    tblDatos = $('#tblDatos').dataTable({
                        "aaData": data.d.DatosContacto,
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

            }
        });
    }

    function EsString(Alumno) {
        $('#frmTabs').hide();
        $.ajax({
            url: 'WS/Alumno.asmx/BuscarAlumnoString',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{Filtro:"' + Alumno + '"}',
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
    }

    $('#tblAlumnos').on('click', 'a', function () {
        $('#frmVarios').hide();
        $('#frmTabs').show();
        $('#Load').modal('show');
        var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
        AlumnoNum = rowadd.AlumnoId;
        EsNumero(AlumnoNum);
    });

    function CargarPaises(combo, PaisId) {
        combo.empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarPaises",
            data: "{}",
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.PaisId);

                    combo.append(option);
                });
                combo.val(PaisId);

            }
        });
    }

    function CargarEstados(combo, EstadoId) {

        combo.empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarEntidadFederativa",
            data: "{}",
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    combo.append(option);
                });
                combo.val(EstadoId);

            }
        });
    }

    $('#slcNacionalidad').change(function () {
        $("#slcLugarN").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcLugarN").append(optionP);

        var tipo = $("#slcNacionalidad");
        tipo = tipo[0].value;
        if (tipo == 2) {
            CargarPaises($("#slcLugarN"), -1);
        }
        else if (tipo == 1) {
            CargarEstados($("#slcLugarN"), -1);
        }
        else { $("#slcLugarN").append(optionP); }
    });

    $('#txtAlumno').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscarAlumno').click();
        }
    });

    function CargarEstados1(EstadoId, MunicipioId) {
        $('#slcEstado').empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarEntidadFederativa",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#slcEstado").append(option);
                });

                $('#slcEstado').val(EstadoId);
                $("#slcMunicipio").empty();

                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarMunicipios",
                    data: "{EntidadFederativaId:'" + EstadoId + "'}",
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        var datos = data.d;
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));

                            option.text(this.Descripcion);
                            option.val(this.EntidadFederativaId);

                            $("#slcMunicipio").append(option);
                        });
                        $("#slcMunicipio").val(MunicipioId);

                    }
                });
            }
        });

    }

});
