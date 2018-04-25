﻿$(function () {
    var AlumnoNum, tblAlumnos;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    UsuarioId = $.cookie('userAdmin');

    var fnDatos =
        {
            init: function () {
                ComponentsKnobDials.init();
                ComponentsPickers.init();
                GlobalFn.init()
                GlobalFn.GetGenero();
                GlobalFn.GetEstado("slcEstado", 9);
                GlobalFn.GetEstadoCivil();

                $("#btnBuscarAlumno").on("click", fnDatos.buscarAlumno);
                $('#tblAlumnos').on('click', 'a', fnDatos.seleccionarAlumno);
                $('#Guardar').on('click', fnDatos.guardarTodo);
                $('#slcNacionalidad').on("change", fnDatos.nacionalidadChange);
                $('#txtAlumno').on('keydown', function (e) {
                    if (e.which == 13) {
                        $('#btnBuscarAlumno').click();
                    }
                });
                form.validate({
                    errorElement: 'span', //default input error message container
                    errorClass: 'help-block help-block-error', // default input error message class
                    focusInvalid: false, // do not focus the last invalid input
                    rules: {

                        txtemail: {
                            //required: true,
                            email: true,
                            minlength: 4,
                            maxlength: 100
                        },
                        txtCelular: {
                            //required: true,
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        txtTelefonoCasa: {
                            digits: true,
                            //required: true,
                            minlength: 8,
                            maxlength: 10
                        },
                        txtCalle: {
                            //required: true,
                            maxlength: 100,
                            minlength: 3
                        },
                        txtCP: {
                            //required: true,
                            digits: true,
                            minlength: 5
                        },
                        txtColonia: {
                            //required: true,
                            maxlength: 100,
                            minlength: 1
                        },
                        slcMunicipio: {
                            min: 1,
                            //required: true,
                        },
                        slcEstado: {
                            //required: true,
                            min: 1
                        },
                        slcEstadoCivil: {
                            //required: true,
                            min: 1
                        },
                    },

                    invalidHandler: function (event, validator) { //display error alert on form submit       
                        //alert('Empeze');
                        success.hide();
                        error.show();
                        Metronic.scrollTo(error, -200);
                    },
                    errorPlacement: function (error, element) { // render error placement for each input type
                        var icon = $(element).parent('.input-icon').children('i');
                        icon.removeClass('fa-check').addClass("fa-warning");
                        icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
                    },
                    highlight: function (element) { // hightlight error inputs
                        $(element)
                            .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group   
                        b_Valid = 1;
                    },

                    unhighlight: function (element) { // revert the change done by hightlight
                    },

                    success: function (label, element) {
                        var icon = $(element).parent('.input-icon').children('i');
                        $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                        icon.removeClass("fa-warning").addClass("fa-check");

                    },

                    submitHandler: function (form) {
                        success.show();
                        error.hide();
                    }
                });
            },
            buscarAlumno: function () {
                $("#divGuardar").hide();
                $('#frmVarios').hide();
                if (tblAlumnos != undefined) {
                    tblAlumnos.fnClearTable();
                }
                if ($('#txtAlumno').val().length == 0) { return false; }
                fnDatos.limpiarCampos();
                $('#Load').modal('show');
                AlumnoNum = $('#txtAlumno').val();

                if (!isNaN(AlumnoNum)) {
                    fnDatos.esNumero(AlumnoNum);
                    $('#frmTabs').show();
                } else {
                    fnDatos.esString(AlumnoNum);
                }
            },
            seleccionarAlumno: function () {
                $('#frmVarios').hide();
                $('#frmTabs').show();
                $('#Load').modal('show');
                var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
                AlumnoNum = rowadd.AlumnoId;
                fnDatos.esNumero(AlumnoNum);
            },
            nacionalidadChange: function () {
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
            esNumero: function (Alumno) {
                IndexFn.Api('Alumno/ObenerDatosAlumnoCordinador/' + Alumno, "GET", "")
                    .done(function (data) {
                        if (data != null) {
                            $('#slcNacionalidad').val(data.DTOAlumnoDetalle.PaisId == 146 ? 1 : 2);
                            if (data.DTOAlumnoDetalle.PaisId == 146) {
                                GlobalFn.GetEstado('slcLugarN', data.DTOAlumnoDetalle.EntidadNacimientoId);
                            } else {
                                GlobalFn.GetPais('slcLugarN', data.DTOAlumnoDetalle.PaisId);
                            }
                            ///Personales 
                            $('#txtnombre').val(data.Nombre);
                            $('#txtApPaterno').val(data.Paterno);
                            $('#txtApMaterno').val(data.Materno);
                            $('#txtCelular').val(data.DTOAlumnoDetalle.Celular);
                            $('#txtFNacimiento').val(data.DTOAlumnoDetalle.FechaNacimientoC);
                            $('#txtCURP').val(data.DTOAlumnoDetalle.CURP);
                            $('#txtEmail').val(data.DTOAlumnoDetalle.Email);
                            $('#txtTelefonoCasa').val(data.DTOAlumnoDetalle.TelefonoCasa);

                            $('#txtCalle').val(data.DTOAlumnoDetalle.Calle);
                            $('#txtNumeroE').val(data.DTOAlumnoDetalle.NoExterior);
                            $('#txtNumeroI').val(data.DTOAlumnoDetalle.NoInterior);
                            $('#txtCP').val(data.DTOAlumnoDetalle.Cp);
                            $('#txtColonia').val(data.DTOAlumnoDetalle.Colonia);

                            $('#slcEstadoCivil').val(data.DTOAlumnoDetalle.EstadoCivilId);
                            $('#slcSexo').val(data.DTOAlumnoDetalle.GeneroId);
                            $('#slcEstado').val(data.DTOAlumnoDetalle.EntidadFederativaId);
                            GlobalFn.EstadoChange().done(function () {
                                $("#slcMunicipio").val(data.DTOAlumnoDetalle.MunicipioId);
                            });

                            $("#divGuardar").show();
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
            esString: function (Alumno) {
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
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                    });

            },
            guardarTodo: function () {
                if (form.valid() == false) { return false; }
                var nombre = $('#hCarga');
                nombre[0].innerText = "Guardando";
                $('#Load').modal('show');

                var obj = {
                    'AlumnoDatos': {
                        "AlumnoId": AlumnoNum,
                        "Celular": $('#txtCelular').val(),
                        "Email": $('#txtEmail').val(),
                        "TelefonoCasa": $('#txtTelefonoCasa').val(),
                        "Calle": $('#txtCalle').val(),
                        "NoExterior": $('#txtNumeroE').val(),
                        "NoInterior": $('#txtNumeroI').val(),
                        "Cp": $('#txtCP').val(),
                        "Colonia": $('#txtColonia').val(),
                        "EstadoCivilId": $('#slcEstadoCivil').val(),
                        "EntidadFederativaId": $('#slcEstado').val(),
                        "MunicipioId": $('#slcMunicipio').val(),
                        "UsuarioId": UsuarioId
                    }
                };
                obj = JSON.stringify(obj);
                $.ajax({
                    type: "POST",
                    url: "WS/Alumno.asmx/UpdateAlumnoDatosCoordinador",
                    data: obj,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d) {
                            $('#Load').modal('hide');
                            $('#PopDatosAlumno').modal('hide');
                            alertify.alert("Datos del Alumno Modificados", function () {
                            });
                        } else {
                            $('#Load').modal('hide');
                            $('#PopDatosAlumno').modal('hide');
                            alertify.alert("Error, Revisar datos capturados.", function () {
                                $('#PopDatosAlumno').modal('hide');
                                $('#popDatos').empty();
                            });
                        }
                    }
                });


            },
            limpiarCampos: function () {
                $("#submit_form").trigger('reset');
                error.hide();
                success.hide();
                $('#submit_form  div').removeClass('has-error');
                $('#submit_form  div').removeClass('has-success');
                $('#submit_form  i').removeClass('fa-warning');
                $('#submit_form  i').removeClass('fa-check');
            }
        };

    fnDatos.init();

});