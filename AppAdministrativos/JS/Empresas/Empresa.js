$(function () {
    tblAlumnosCom1
    var Seleccionados = "", MItable, Alumnos, DatosEmpresas, AlumnoId, TipoMovimiento, ModificarGrupo, TM, sucursalid, AlumnosEmpresa;
    var MiGrupo, Fila, objEstados, EmpresaI, GrupoI, OfertaI, InscripcionID, CuotaIdColegiatura, MesP = [], OFertas;
    var tblAlumnosCompletos, AlumnoConfig = { AlumnoId: "", GrupoId: "" };
    var EmpresaAlumno = [], tblAlumnosCompletos1;
    var form = $('#formPopup');
    var GrupoFrm = $('#frmGrupo');
    var error = $('.alert-danger', form);
    var error1 = $('.alert-danger', GrupoFrm);
    var success = $('.alert-success', form);
    var success1 = $('.alert-success', GrupoFrm);
    var formAlumno = $('#frmAlumnoCuota');
    var errorAlumno = $('.alert-danger', formAlumno);
    var successAlumno = $('.alert-success', formAlumno);
    var Now = new Date();
    var años = Now.getFullYear();
    var mes = Now.getMonth() + 1;
    var Fecha = Now.getDate() + '-' + mes + '-' + años;

    var EmpresasFn =
        {
            init() {
                form.validate({
                    errorElement: 'span', //default input error message container
                    errorClass: 'help-block help-block-error', // default input error message class
                    focusInvalid: false, // do not focus the last invalid input
                    rules: {
                        txtDescripcion: {
                            required: true,
                            minlength: 5
                        },
                        txtRFC: {
                            required: true,
                            minlength: 10,
                            maxlength: 13
                        },
                        txtmail: {
                            email: true,
                            required: true
                        },
                        txtCalle: {
                            required: true
                        },
                        txtCP: {
                            required: true,
                            digits: true,
                            minlength: 5
                        },
                        NoExterior: {
                            required: true,
                            digits: true
                        },
                        NoInterior: {
                            required: false
                        },
                        slcPaisUni: {
                            required: true,
                            min: 1
                        },
                        slcEstadoPais: {
                            required: true,
                            min: 1
                        },
                        slcDelegacion: {
                            required: true,
                            min: 1
                        },
                        txtObservacion: {
                            required: false
                        },
                        txtColonia: {
                            required: true
                        },
                        txtNombre: {
                            required: true,
                        },
                        txtPaterno: {
                            required: true
                        },
                        txtMaterno: {
                            required: true
                        },
                        txtEmail: {
                            required: true,
                            email: true
                        },
                        txtTelefono: {
                            required: true,
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        txtCelular: {
                            required: true,
                            digits: true,
                            minlength: 10,
                            maxlength: 10
                        },
                        txtFechaV: {
                            required: true,
                            minlength: 10,
                            maxlength: 10
                        }
                    },
                    invalidHandler: function (event, validator) { //display error alert on form submit              
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
                GrupoFrm.validate({
                    errorElement: 'span', //default input error message container
                    errorClass: 'help-block help-block-error', // default input error message class
                    focusInvalid: false, // do not focus the last invalid input
                    rules: {
                        txtNombreGrupo: {
                            required: true,
                            minlength: 5
                        },
                        slcPlantel: {
                            required: true,
                            min: 1
                        },
                        txtDireccion: {
                            required: true,
                            minlength: 5
                        },
                        slcOfertaEducativa: {
                            required: true,
                            min: 1
                        },
                        txtFechaInicio: {
                            required: true
                        },
                        txtNPagos: {
                            required: true,
                            digits: true
                        },
                        BecArchivo: {
                            extension: 'pdf|jpg|png|jpeg'
                        }
                    },
                    invalidHandler: function (event, validator) { //display error alert on form submit              
                        success1.hide();
                        error1.show();
                        Metronic.scrollTo(error1, -200);
                    },

                    errorPlacement: function (error1, element) { // render error placement for each input type
                        var icon = $(element).parent('.input-icon').children('i');
                        icon.removeClass('fa-check').addClass("fa-warning");
                        icon.attr("data-original-title", error1.text()).tooltip({ 'container': 'body' });
                    },

                    highlight: function (element) { // hightlight error inputs
                        $(element)
                            .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group   
                    },

                    unhighlight: function (element) { // revert the change done by hightlight
                    },

                    success: function (label, element) {
                        var icon = $(element).parent('.input-icon').children('i');
                        $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                        icon.removeClass("fa-warning").addClass("fa-check");
                    },

                    submitHandler: function (GrupoFrm) {
                        success1.show();
                        error1.hide();
                    }
                });
                formAlumno.validate({
                    errorElement: 'span', //default input error message container
                    errorClass: 'help-block help-block-error', // default input error message class
                    focusInvalid: false, // do not focus the last invalid input
                    rules: {
                        slcOferta: {
                            required: true,
                            min: 1
                        },
                        slcOfertaEducativa: {
                            required: true,
                            min: 1
                        },
                        slcSistemaPago: {
                            required: true,
                            min: 1
                        },
                        txtCuotaInscripcion: {
                            required: true
                        },
                        txtCuotaColegiatura: {
                            required: true
                        },
                        slcPeriodo: {
                            required: true
                        },
                    }
                });
                EmpresasFn.Cargar();
                EmpresasFn.CargarSede();
                EmpresasFn.CargarTipo();
                if (jQuery().datepicker) {
                    $('.date-picker').datepicker({
                        rtl: Metronic.isRTL(),
                        orientation: "left",
                        autoclose: true,
                        language: 'es'
                    });
                    $(".date-picker").datepicker("setDate", Fecha);
                }

                $('#btnGuardarGrupo').click(function () {
                    if (GrupoFrm.valid() == false) { return false; }

                    query = "{ EmpresaId:" + EmpresaI + ",GrupoId:" + GrupoI + ",Nombre:'" + $('#txtNombreGrupo').val() + "',Sede:'" + $('#slcPlantel').val() + "',Direccion:'" + $('#txtDireccion').val() + "',FechaIni:'" + $('#txtFechaInicio').val() +
                        "',UsuarioId:'" + $.cookie('userAdmin') + "'}";

                    $.ajax({
                        type: "POST",
                        url: "WS/Empresa.asmx/GuardarGrupo2",
                        data: query, // the data in form-encoded format, ie as it would appear on a querystring
                        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            if (data.d >= 1) {
                                LimpiarGrupo();
                                alertify.alert("Grupo Guardado");
                                EmpresasFn.CargarGrupo(EmpresaI);
                            }
                            else { alertify.alert("Error"); }
                        }

                    });
                });

                $('#chkFiscales').click(function () {
                    if ($(this).is(':checked')) {
                        $('#slcPaisUniFiscal').prop('disabled', 'disabled');
                        $('#slcPaisUniFiscal').val($('#slcPaisUni').val());
                        $('#slcPaisUniFiscal').change();
                        $('#slcEstadoPaisFiscal').prop('disabled', 'disabled');
                        $('#slcEstadoPaisFiscal').val($('#slcEstadoPais').val());
                        $('#slcEstadoPaisFiscal').change();
                        $("#slcDelegacionFiscal").prop('disabled', 'disabled');
                        $('#txtCalleFiscal').prop('readonly', true);
                        $('#txtCalleFiscal').val($('#txtCalle').val());
                        $('#txtCPFiscal').prop('readonly', true);
                        $('#txtCPFiscal').val($('#txtCP').val());
                        $('#NoExteriorFiscal').prop('readonly', true);
                        $('#NoExteriorFiscal').val($('#NoExterior').val());
                        $('#NoInteriorFiscal').prop('readonly', true);
                        $('#NoInteriorFiscal').val($('#NoInterior').val());
                        $('#txtObservacionFiscal').prop('readonly', true);
                        $('#txtObservacionFiscal').val($('#txtObservacion').val());
                        $('#txtColoniaFiscal').prop('readonly', true);
                        $('#txtColoniaFiscal').val($('#txtColonia').val());
                    } else {
                        $('#txtCalleFiscal').removeAttr('readonly');
                        $('#txtCPFiscal').removeAttr('readonly');
                        $('#NoExteriorFiscal').removeAttr('readonly');
                        $('#NoInteriorFiscal').removeAttr('readonly');
                        $('#slcPaisUniFiscal').removeAttr('disabled');
                        $('#slcEstadoPaisFiscal').removeAttr('disabled');
                        $('#slcDelegacionFiscal').removeAttr('disabled');
                        $('#txtObservacionFiscal').removeAttr('readonly');
                        $('#txtColoniaFiscal').removeAttr('readonly');

                        $('#txtCalleFiscal').val('');
                        $('#txtCPFiscal').val('');
                        $('#NoExteriorFiscal').val('');
                        $('#NoInteriorFiscal').val('');
                        $('#slcPaisUniFiscal').val(146);
                        $('#slcEstadoPaisFiscal').val(-1);
                        $('#slcDelegacionFiscal').val(-1);
                        $('#txtObservacionFiscal').val('');
                        $('#txtColoniaFiscal').val('');
                    }
                });

                $('#btnEmpresa').click(function () {
                    $('#NuevaEmpresa').modal('show');
                    EmpresasFn.Paises();
                });

                $("#slcOferta").change(function () {
                    EmpresasFn.CargarOfertasL(-1);
                });

                $('#slcPlantel').change(function () {
                    EmpresasFn.CargarDireccion();
                });

                $("#slcOfertaEducativa").change(function () {
                    if ($("#slcOferta").val() == 4) {
                        return false;
                    } else {
                        EmpresasFn.CargarDescuento($("#slcOfertaEducativa").val());
                    }
                });

                $('#btnCerrar').mousedown(function () {
                    $('#txtDescripcion').val('');
                    $('#txtArea').val('');
                });

                $('#btnSiguiente').click(function () {
                    if (form.valid() == false) { return false; }
                    error.hide();
                    $('#divDatosEmpresa').hide();
                    $('#divFiscales').show();
                    $('#btnAtras').removeAttr('style');
                    $('#btnSiguiente').attr('style', 'display: none');
                    $('#btnGuardar').removeAttr('style');
                });

                $('#btnAtras').click(function () {
                    $('#divFiscales').hide();
                    $('#btnGuardar').attr('style', 'display: none');
                    $('#btnAtras').attr('style', 'display: none');
                    $('#btnSiguiente').removeAttr('style');
                    $('#divDatosEmpresa').show();
                });

                $('#btnGuardar').click(function () {
                    if (form.valid() == false) { return false; }
                    $('#NuevaEmpresa').modal('hide');
                    var query = "{";
                    query += "Razon:'" + $('#txtDescripcion').val() + "',RFC:'" + $('#txtRFC').val() + "',UsuarioId:'" + $.cookie('userAdmin') + "',Email:'" + $('#txtmail').val() + "',Calle:'" + $('#txtCalle').val() + "',CP:'" + $('#txtCP').val() +
                        "',NoExterior:'" + $('#NoExterior').val() + "',NoInterior:'" + $('#NoInterior').val() + "',Pais:'" + $('#slcPaisUni').val() + "',Estado:'" + $('#slcEstadoPais').val() +
                        "',Delegacion:'" + $('#slcDelegacion').val() + "',Observacion:'" + $('#txtObservacion').val() + "',Colonia:'" + $('#txtColonia').val() + "',FechaV:'" + $('#txtFechaV').val() +
                        "',NombreC:'" + $('#txtNombre').val() + "',Paterno:'" + $('#txtPaterno').val() + "',Materno:'" + $('#txtMaterno').val() + "',EmailC:'" + $('#txtEmail').val() + "',Telefono:'" + $('#txtTelefono').val() +
                        "',Celular:'" + $('#txtCelular').val() + "'";

                    if ($('#chkFiscales')[0].checked == false) {
                        query += ",CalleF:'" + $('#txtCalleFiscal').val() + "',CPF:'" + $('#txtCPFiscal').val() + "',NoExteriorF:'" + $('#NoExteriorFiscal').val() + "',NoInteriorF:'" + $('#NoInteriorFiscal').val() +
                            "',PaisF:'" + $('#slcPaisUniFiscal').val() + "',EstadoF:'" + $('#slcEstadoPaisFiscal').val() + "',DelegacionF:'" + $('#slcDelegacionFiscal').val() + "',ObservacionF:'" + $('#txtObservacionFiscal').val() + "',ColoniaF:'" + $('#txtColoniaFiscal').val() + "',Igual:'false'}";;
                    } else {
                        query += ",CalleF:'" + "" + "',CPF:'" + "" + "',NoExteriorF:'" + "" + "',NoInteriorF:'" + "" + "',PaisF:'" + "" +
                            "',EstadoF:'" + "" + "',DelegacionF:'" + "" + "',ObservacionF:'" + "" + "',ColoniaF:'" + "" + "',Igual:'true'}";
                    }

                    $.ajax({
                        type: "POST",
                        url: "WS/Empresa.asmx/GuardarEmpresa",
                        data: query, // the data in form-encoded format, ie as it would appear on a querystring
                        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            $('#NuevaEmpresa').modal('show');
                            if (data.d == "True") {
                                alertify.alert("Empresa Guardada", function () {


                                    $("#formPopup").trigger('reset');
                                    error.hide();
                                    success.hide();
                                    $('#formPopup  div').removeClass('has-error');
                                    $('#formPopup  div').removeClass('has-success');
                                    $('#formPopup  i').removeClass('fa-warning');
                                    $('#formPopup  i').removeClass('fa-check');
                                    var chk = $('#chkFiscales')[0];
                                    var spam = $('#chkFiscales')[0].parentElement;
                                    $(spam).removeClass('checked');
                                    $('#txtCalleFiscal').removeAttr('readonly');
                                    $('#txtCPFiscal').removeAttr('readonly');
                                    $('#NoExteriorFiscal').removeAttr('readonly');
                                    $('#NoInteriorFiscal').removeAttr('readonly');
                                    $('#slcPaisUniFiscal').removeAttr('disabled');
                                    $('#slcEstadoPaisFiscal').removeAttr('disabled');
                                    $('#slcDelegacionFiscal').removeAttr('disabled');
                                    $('#txtObservacionFiscal').removeAttr('readonly');
                                    $('#txtColoniaFiscal').removeAttr('readonly');
                                    $(".date-picker").datepicker("setDate", Fecha);
                                    $('#divFiscales').hide();
                                    $('#divDatosEmpresa').show();
                                    $('#NuevaEmpresa').modal('hide');
                                    $('#btnGuardar').attr('style', 'display: none');
                                    $('#btnAtras').attr('style', 'display: none');
                                    $('#btnSiguiente').removeAttr('style');
                                    btnGuardar
                                    EmpresasFn.Cargar();
                                });
                            }
                            else {
                                alertify.alert("Error, no fue posible guardar la empresa", function () {
                                    return false;
                                });
                            }
                        }
                    });
                });

                $("#slcPaisUni").change(function () {
                    var Pais = $("#slcPaisUni").val();
                    $("#slcEstadoPais").empty();
                    var optionP = $(document.createElement('option'));
                    optionP.text('--Seleccionar--');
                    optionP.val('-1');
                    $("#slcEstadoPais").append(optionP);


                    $(objEstados).each(function () {
                        if (Pais != 146) {

                            if (this.EntidadFederativaId == 33) {
                                var option = $(document.createElement('option'));
                                option.text(this.Descripcion);
                                $("#slcEstadoPais").append(option);
                                option.val(this.EntidadFederativaId);
                                $("#slcEstadoPais").append(option);
                            }//this.EntidadFederativaId == 33
                        }
                        else {
                            if (this.EntidadFederativaId != 33) {
                                var option = $(document.createElement('option'));
                                option.text(this.Descripcion);
                                option.val(this.EntidadFederativaId);
                                $("#slcEstadoPais").append(option);
                            }//this.EntidadFederativaId == 33
                        }//Pais != 146
                    });


                });

                $("#slcPaisUniFiscal").change(function () {
                    var Pais = $("#slcPaisUniFiscal").val();
                    $("#slcEstadoPaisFiscal").empty();
                    var optionP = $(document.createElement('option'));
                    optionP.text('--Seleccionar--');
                    optionP.val('-1');
                    $("#slcEstadoPaisFiscal").append(optionP);


                    $(objEstados).each(function () {
                        if (Pais != 146) {

                            if (this.EntidadFederativaId == 33) {
                                var option = $(document.createElement('option'));
                                option.text(this.Descripcion);
                                $("#slcEstadoPaisFiscal").append(option);
                                option.val(this.EntidadFederativaId);
                                $("#slcEstadoPaisFiscal").append(option);
                            }//this.EntidadFederativaId == 33
                        }
                        else {
                            if (this.EntidadFederativaId != 33) {
                                var option = $(document.createElement('option'));
                                option.text(this.Descripcion);
                                option.val(this.EntidadFederativaId);
                                $("#slcEstadoPaisFiscal").append(option);
                            }//this.EntidadFederativaId == 33
                        }//Pais != 146
                    });


                });

                $("#slcEstadoPais").change(function () {
                    $("#slcDelegacion").empty();
                    var Entidad = $("#slcEstadoPais");
                    var optionP = $(document.createElement('option'));
                    optionP.text('--Seleccionar--');
                    optionP.val('-1');
                    $("#slcDelegacion").append(optionP);

                    Entidad = Entidad[0].value;
                    $.ajax({
                        type: "POST",
                        url: "WS/General.asmx/ConsultarMunicipios",
                        data: "{EntidadFederativaId:'" + Entidad + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            var datos = data.d;
                            $(datos).each(function () {
                                var option = $(document.createElement('option'));

                                option.text(this.Descripcion);
                                option.val(this.EntidadFederativaId);

                                $("#slcDelegacion").append(option);
                            });
                        }
                    });
                });

                $('#slcEstadoPaisFiscal').change(function () {
                    $("#slcDelegacionFiscal").empty();
                    var Entidad = $("#slcEstadoPaisFiscal");
                    var optionP = $(document.createElement('option'));
                    optionP.text('--Seleccionar--');
                    optionP.val('-1');
                    $("#slcDelegacionFiscal").append(optionP);

                    Entidad = Entidad[0].value;
                    $.ajax({
                        type: "POST",
                        url: "WS/General.asmx/ConsultarMunicipios",
                        data: "{EntidadFederativaId:'" + Entidad + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            var datos = data.d;
                            $(datos).each(function () {
                                var option = $(document.createElement('option'));

                                option.text(this.Descripcion);
                                option.val(this.EntidadFederativaId);

                                $("#slcDelegacionFiscal").append(option);
                            });
                            if ($('#chkFiscales')[0].checked == false) {
                                $("#slcDelegacionFiscal").val('-1');
                            } else {
                                $("#slcDelegacionFiscal").val($('#slcDelegacion').val());
                            }
                        }
                    });

                });

                $('#tblEmpresa').on('click', 'a', function () {
                    $('#NombreEmpresa').text(this.innerHTML);
                    EmpresaI = this.parentNode.parentNode;
                    EmpresaI = MItable.fnGetData(EmpresaI, 0);
                    EmpresasFn.CargarGrupo(EmpresaI);
                    $('#EmpresaLista').hide();
                    $('#EmpresaC').show();
                });

                $('#btnCerrarGrupo').click(function () {
                    error1.hide();
                    success1.hide();
                    $('#frmGrupo  div').removeClass('has-error');
                    $('#frmGrupo  div').removeClass('has-success');
                    $('#frmGrupo  i').removeClass('fa-warning');
                    $('#PopGrupo').modal('hide');
                });

                $('#btnGrupo').click(function () {
                    EmpresasFn.LimpiarGrupo();
                    $('#hGrupos').text("Alta de Grupo");
                    GrupoI = 0;
                    $('#PopGrupo').modal('show');
                });

                $('#btnAtrasE').click(function () {
                    $('#EmpresaLista').show();
                    $('#EmpresaC').hide();
                });

                $('#ListaGrupos').on('click', 'a', function () {


                    if (this.innerHTML == "Nuevo Grupo") { return false; }
                    if (this.name == "btnAlumos") {

                        $('#NombreEmpresa1').text($('#NombreEmpresa').text());
                        $('#NombreGrupo1').text(this.innerHTML);
                        var rFila = this.parentNode.parentNode;
                        var row = this.parentNode.parentNode;
                        var rowadd = MiGrupo.fnGetData($(this).closest('tr'));
                        rFila = MiGrupo.fnGetData(rFila, 0);
                        GrupoI = rowadd.GrupoId;
                        $('#Load').modal('show');
                        EmpresasFn.CargarTablaAlumnosGrupo(GrupoI);
                        EmpresasFn.CargarEmpresasLigero();
                        $('#EmpresaC').hide();
                        $('#DivAlumnosGrupo').show();
                    }

                    if (this.name == "btnModificar") {
                        var rFila = this.parentNode.parentNode;
                        var row = this.parentNode.parentNode;
                        var rowadd = MiGrupo.fnGetData($(this).closest('tr'));
                        rFila = MiGrupo.fnGetData(rFila, 0);
                        Fila = rFila;
                        $('#hGrupos').text("Modificación de grupo ");
                        $('#PopGrupo').modal('show');
                        $('#txtNombreGrupo').val(rowadd.Descripcion);
                        $('#slcPlantel').val(rowadd.SucursalId);
                        $('#txtDireccion').val(rowadd.SucursalDireccion);
                        $('#txtFechaInicio').val(rowadd.FechaInicioS);
                        GrupoI = rowadd.GrupoId;
                    }
                });

                $('#btnAtrasG').click(function () {
                    if (Seleccionados.length > 1) {
                        alertify.confirm("<p>¿Tiene cambios pendientes, seguro que desea salir?<br><br><hr>", function (e) {
                            if (e) {
                                $('#EmpresaC').show();
                                $('#DivGrupo').hide();
                                Seleccionados = "";
                            } else {
                                $('#btnGrupoAl').focus();
                            }
                        });
                    } else {
                        $('#EmpresaC').show();
                        $('#DivGrupo').hide();
                        Seleccionados = "";
                    }
                });

                $("#txtNPagos").keypress(function (e) {
                    //if the letter is not digit then display error and don't type anything
                    if (e.which == 13) { $('#btnBuscar').click(); }
                    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                        return false;
                    }
                });

                $('#btnAtrasAlumnos').on('click', function () {
                    var origen = $('#DivAlumnos').data('origen');
                    var grupoid = $('#DivAlumnos').data('grupoid');
                    if (grupoid != undefined) {
                        EmpresasFn.CargarTablaAlumnosGrupo(grupoid);
                    }
                    $('#' + origen).show();
                    $('#DivAlumnos').hide();
                });

                $('#btnAlumnoGrupoG').on('click', function () {
                    if ($('#slcGrupo').val() != -1) {
                        var grupo = $('#slcGrupo').val();
                        var ofertaid = OfertaI
                        if (GrupoI != grupo) {
                            var usuario = $.cookie('userAdmin');
                            $('#Load').modal('show');
                            $.ajax({
                                type: "POST",
                                url: "WS/Empresa.asmx/MovimientosAlumnoGrupo",
                                data: "{GrupoId:'" + grupo + "',AlumnoId:'" + AlumnoId + "',UsuarioId:'" + usuario + "',OfertaId:'" + OfertaI + "',TipoMovimiento:'" + TipoMovimiento + "'}",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "Guardado") {

                                        if (ModificarGrupo == 1) {
                                            EmpresasFn.CargarTabla();
                                        } else {
                                            EmpresasFn.CargarTablaAlumnosGrupo(GrupoI);
                                        }
                                        alertify.alert("Guardado Correctamente");
                                        $('#PopAlumnoGrupo').modal('hide');
                                        $('#Load').modal('hide');

                                    }
                                }
                            });
                        }//end if
                    }//if ($('#slcGrupo').val()!= -1)


                });

                $('#tblAlumnosCom').on('click', 'a', function () {
                    var row = this.parentNode.parentNode;
                    var rowadd = tblAlumnosCompletos.fnGetData($(this).closest('tr'));
                    TipoMovimiento = 0;
                    if (rowadd.AlumnoCuota.GrupoId == 0) {
                        $("#slcEmpresa").prop('disabled', false);
                        $("#slcGrupo").prop('disabled', false);
                        $("#btnAlumnoGrupoG").prop('disabled', false);
                        $("#slcEmpresa").val(-1);
                        $("#slcGrupo").empty();
                        TipoMovimiento = 1;
                    } else {

                        $("#slcEmpresa").val(rowadd.AlumnoCuota.EmpresaId);
                        $("#slcEmpresa").change();
                        $("#slcEmpresa").prop('disabled', true);
                        $("#slcGrupo").val(rowadd.AlumnoCuota.GrupoId);
                        $("#slcGrupo").prop('disabled', true);
                        $("#btnAlumnoGrupoG").prop('disabled', true);
                        TipoMovimiento = 2;
                    }
                    AlumnoId = rowadd.AlumnoId;
                    $('#PopAlumnoGrupo').modal('show');
                    ModificarGrupo = 1;
                    GrupoI = rowadd.AlumnoCuota.GrupoId;
                    OfertaI = rowadd.OfertaEducativaId;
                });

                $('#tblAlumnosCom').on('click', 'button', function () {
                    var row = this.parentNode.parentNode;
                    var rowadd = tblAlumnosCompletos.fnGetData($(this).closest('tr'));

                    if ($(this)[0].name === "VerCredenciales") {
                        var alumnoCred = "AlumnoId=" + rowadd.AlumnoId;
                        alumnoCred = alumnoCred + "&OfertaEducativaid=" + rowadd.OfertaEducativaId;
                        window.open('Views/Alumno/Credenciales.aspx?' + alumnoCred, '_blank');
                    } else {
                        $("#frmAlumnoCuota").trigger('reset');
                        var validator = $("#frmAlumnoCuota").validate();
                        validator.resetForm();
                        AlumnoConfig.AlumnoId = "";
                        AlumnoConfig.GrupoId = "";

                        AlumnoConfig.AlumnoId = rowadd.AlumnoId;
                        AlumnoConfig.GrupoId = rowadd.GrupoId;
                        if (rowadd.AlumnoCuota.EstatusId != 8) {
                            $("#slcSistemaPago").prop('disabled', true);
                            $("#slcOferta").prop('disabled', true);
                            $("#slcOfertaEducativa").prop('disabled', true);
                            $('#txtCuotaInscripcion').val(rowadd.AlumnoCuota.CuotaInscripcion);
                            $("#txtCuotaInscripcion").prop('disabled', true);
                            $('#txtCuotaColegiatura').val(rowadd.AlumnoCuota.CuotaColegiatura);
                            $("#txtCuotaColegiatura").prop('disabled', true);
                            $('#txtNPagos').val(rowadd.AlumnoCuota.NoPagos);
                            $("#txtNPagos").prop('disabled', true);

                            var chk = $('#chkCongelada');
                            $(chk).prop('disabled', true);
                            if (rowadd.AlumnoCuota.CuotaCongelada) {
                                $('#chkCongelada')[0].checked = true;
                            }

                            var chk2 = $('#chkInCongelada');
                            $(chk2).prop('disabled', true);
                            if (rowadd.AlumnoCuota.InscripcionCongelada) {
                                $('#chkInCongelada')[0].checked = true;
                            }

                            var chk1 = $('#chkEspecial');
                            $(chk1).prop('disabled', true);
                            if (rowadd.AlumnoCuota.EsEspecial) {
                                $('#chkEspecial')[0].checked = true;
                            }

                            var chkCre = $('#chkCredenciales');
                            $(chkCre).prop('disabled', true);
                            $('#chkCredenciales')[0].checked = true;


                            var opc = $('#slcPeriodo').find('option[value="1"]').val();
                            if (opc == '1') {
                                $("#slcPeriodo option[value='1']").remove();
                            }
                            var optionP = $(document.createElement('option'));
                            optionP.text(rowadd.AlumnoCuota.DescipcionPeriodo);
                            optionP.val('1');
                            $("#slcPeriodo").append(optionP);
                            $("#slcPeriodo").val('1');

                            $("#slcPeriodo").prop('disabled', true);

                            $("#btnAlumnoConfig").prop('disabled', true);
                        } else {
                            $("#slcSistemaPago").prop('disabled', false);
                            $("#slcOferta").prop('disabled', false);
                            $("#slcOfertaEducativa").prop('disabled', false);
                            $("#txtCuotaInscripcion").prop('disabled', false);
                            $("#txtCuotaColegiatura").prop('disabled', false);
                            $("#btnAlumnoConfig").prop('disabled', false);
                            $('#txtNPagos').val('');
                            $("#txtNPagos").prop('disabled', false);

                            var chk = $('#chkCongelada');
                            $(chk).prop('disabled', false);
                            $('#chkCongelada')[0].checked = false;

                            var chk2 = $('#chkInCongelada');
                            $(chk2).prop('disabled', false);
                            $('#chkInCongelada')[0].checked = false;

                            var chk1 = $('#chkEspecial');
                            $(chk1).prop('disabled', false);
                            $('#chkEspecial')[0].checked = false;

                            var chkCre = $('#chkCredenciales');
                            $(chkCre).prop('disabled', false);
                            $('#chkCredenciales')[0].checked = false;


                            var opc = $('#slcPeriodo').find('option[value="1"]').val();
                            if (opc == '1') {
                                $("#slcPeriodo option[value='1']").remove();
                            }
                            $("#slcPeriodo").val(rowadd.AlumnoCuota.PeriodoIdGrupo + ' ' + rowadd.AlumnoCuota.AnioGrupo);
                            $("#slcPeriodo").prop('disabled', false);
                        }
                        sucursalid = rowadd.AlumnoCuota.SucuralGrupo;
                        $("#slcOferta").empty();
                        var optionP = $(document.createElement('option'));
                        optionP.text('--Seleccionar--');
                        optionP.val('-1');
                        $("#slcOferta").append(optionP);

                        $(OFertas).each(function (i, d) {
                            $(d.Ofertas).each(function (i1, d1) {

                                if (sucursalid == 3) {
                                    if ($("#slcOferta option[value='" + d.OfertaEducativaTipoId + "']").length == 0 && d.OfertaEducativaTipoId != 4) {
                                        var option = $(document.createElement('option'));
                                        option.text(d.Descripcion);
                                        option.val(d.OfertaEducativaTipoId);
                                        $("#slcOferta").append(option);
                                    }

                                } else if (this.sucursalid == sucursalid) {

                                    if ($("#slcOferta option[value='" + d.OfertaEducativaTipoId + "']").length == 0 && d.OfertaEducativaTipoId != 4) {
                                        var option = $(document.createElement('option'));
                                        option.text(d.Descripcion);
                                        option.val(d.OfertaEducativaTipoId);
                                        $("#slcOferta").append(option);
                                    }

                                }
                            });
                        });

                        $(OFertas).each(function (i, d) {
                            $(d.Ofertas).each(function (i1, d1) {
                                if (this.ofertaEducativaId == rowadd.OfertaEducativaId) {
                                    $("#slcOferta").val(d.OfertaEducativaTipoId);
                                    EmpresasFn.CargarOfertasL(rowadd.OfertaEducativaId);
                                }
                            });
                        });


                        GrupoI = rowadd.AlumnoCuota.GrupoId;
                        OfertaI = rowadd.OfertaEducativaId;

                        $('#PopAlumnoConfiguracion').modal('show');
                    }

                });

                $('#btnAlumnos1').on('click', function () {
                    $('#DivAlumnosGrupo').hide();
                    $('#DivAlumnos').show();
                    $('#Load').modal('show');
                    EmpresasFn.CargarTabla();
                    $('#DivAlumnos').removeAttr('data-origen');
                    $('#DivAlumnos').data("origen", 'DivAlumnosGrupo');
                    $('#DivAlumnos').data("grupoid", GrupoI);
                });

                $('#btnAlumnos').on('click', function () {
                    $('#EmpresaLista').hide();
                    $('#DivAlumnos').show();
                    $('#Load').modal('show');
                    EmpresasFn.CargarTabla();
                    $('#DivAlumnos').removeAttr('data-origen');
                    $('#DivAlumnos').data("origen", 'EmpresaLista');
                });

                $('#btnAlumnoConfig').on('click', function () {
                    if (formAlumno.valid()) {
                        $('#Load').modal('show');
                        EmpresasFn.GuardarConfiguracion();
                    }
                });

                $("#slcEmpresa").change(function () {
                    $("#slcGrupo").empty();
                    var EmpresaVal = $("#slcEmpresa").val();

                    var optionP = $(document.createElement('option'));
                    optionP.text('--Seleccionar--');
                    optionP.val('-1');
                    $("#slcGrupo").append(optionP);


                    var GruposAlumno;
                    $(DatosEmpresas).each(function () {

                        if (this.EmpresaId == parseInt(EmpresaVal)) {
                            GruposAlumno = this.Grupo;
                        }
                    });

                    $(GruposAlumno).each(function () {

                        var option = $(document.createElement('option'));
                        option.text(this.Descripcion);
                        option.val(this.GrupoId);
                        $("#slcGrupo").append(option);

                    });

                });

                $('#btnAtrasAlumnosGrupos').on('click', function () {
                    $('#EmpresaC').show();
                    $('#DivAlumnosGrupo').hide();
                });

                $('#tblAlumnosCom1').on('click', 'a', function () {
                    var row = this.parentNode.parentNode;
                    var rowadd = tblAlumnosCompletos1.fnGetData($(this).closest('tr'));
                    TipoMovimiento = 0;
                    $("#slcEmpresa").prop('disabled', false);
                    $("#slcGrupo").prop('disabled', false);
                    $("#btnAlumnoGrupoG").prop('disabled', false);
                    $("#slcEmpresa").val(rowadd.AlumnoCuota.EmpresaId);
                    $("#slcEmpresa").change();
                    $("#slcGrupo").val(rowadd.AlumnoCuota.GrupoId);
                    TipoMovimiento = 2;
                    AlumnoId = rowadd.AlumnoId;
                    $('#PopAlumnoGrupo').modal('show');
                    ModificarGrupo = 2;
                    GrupoI = rowadd.AlumnoCuota.GrupoId;
                    OfertaI = rowadd.OfertaEducativaId;
                });

                $('#tblAlumnosCom1').on('click', 'button', function () {
                    $("#frmAlumnoCuota").trigger('reset');
                    var validator = $("#frmAlumnoCuota").validate();
                    validator.resetForm();
                    AlumnoConfig.AlumnoId = "";
                    AlumnoConfig.GrupoId = "";
                    var row = this.parentNode.parentNode;
                    var rowadd = tblAlumnosCompletos1.fnGetData($(this).closest('tr'));
                    AlumnoConfig.AlumnoId = rowadd.AlumnoId;
                    AlumnoConfig.GrupoId = rowadd.GrupoId;
                    if (rowadd.AlumnoCuota.EstatusId != 8) {
                        $("#slcSistemaPago").prop('disabled', true);
                        $("#slcOferta").prop('disabled', true);
                        $("#slcOfertaEducativa").prop('disabled', true);
                        $('#txtCuotaInscripcion').val(rowadd.AlumnoCuota.CuotaInscripcion);
                        $("#txtCuotaInscripcion").prop('disabled', true);
                        $('#txtCuotaColegiatura').val(rowadd.AlumnoCuota.CuotaColegiatura);
                        $("#txtCuotaColegiatura").prop('disabled', true);
                        $('#txtNPagos').val(rowadd.AlumnoCuota.NoPagos);
                        $("#txtNPagos").prop('disabled', true);

                        var chk = $('#chkCongelada');
                        $(chk).prop('disabled', true);
                        if (rowadd.AlumnoCuota.CuotaCongelada) {
                            $(chk[0].parentElement).addClass('checked');
                        }

                        var chk2 = $('#chkInCongelada');
                        $(chk2).prop('disabled', true);
                        if (rowadd.AlumnoCuota.InscripcionCongelada) {
                            $(chk2[0].parentElement).addClass('checked');
                        }

                        var chk1 = $('#chkEspecial');
                        $(chk1).prop('disabled', true);
                        if (rowadd.AlumnoCuota.EsEspecial) {
                            $(chk1[0].parentElement).addClass('checked');
                        }

                        var chkCre = $('#chkCredenciales');
                        $(chkCre).prop('disabled', true);
                        $(chkCre[0].parentElement).addClass('checked');


                        var opc = $('#slcPeriodo').find('option[value="1"]').val();
                        if (opc == '1') {
                            $("#slcPeriodo option[value='1']").remove();
                        }
                        var optionP = $(document.createElement('option'));
                        optionP.text(rowadd.AlumnoCuota.DescipcionPeriodo);
                        optionP.val('1');
                        $("#slcPeriodo").append(optionP);
                        $("#slcPeriodo").val('1');

                        $("#slcPeriodo").prop('disabled', true);

                        $("#btnAlumnoConfig").prop('disabled', true);
                    } else {
                        $("#slcSistemaPago").prop('disabled', false);
                        $("#slcOferta").prop('disabled', false);
                        $("#slcOfertaEducativa").prop('disabled', false);
                        $("#txtCuotaInscripcion").prop('disabled', false);
                        $("#txtCuotaColegiatura").prop('disabled', false);
                        $("#btnAlumnoConfig").prop('disabled', false);
                        $('#txtNPagos').val('');
                        $("#txtNPagos").prop('disabled', false);

                        var chk = $('#chkCongelada');
                        $(chk).prop('disabled', false);
                        $(chk[0].parentElement).removeClass('checked');

                        var chk2 = $('#chkInCongelada');
                        $(chk2).prop('disabled', false);
                        $(chk2[0].parentElement).removeClass('checked');

                        var chk1 = $('#chkEspecial');
                        $(chk1).prop('disabled', false);
                        $(chk1[0].parentElement).removeClass('checked');

                        var chkCre = $('#chkCredenciales');
                        $(chkCre).prop('disabled', false);
                        $(chkCre[0].parentElement).removeClass('checked');


                        var opc = $('#slcPeriodo').find('option[value="1"]').val();
                        if (opc == '1') {
                            $("#slcPeriodo option[value='1']").remove();
                        }
                        $("#slcPeriodo").val(rowadd.AlumnoCuota.PeriodoIdGrupo + ' ' + rowadd.AlumnoCuota.AnioGrupo);
                        $("#slcPeriodo").prop('disabled', false);
                    }
                    sucursalid = rowadd.AlumnoCuota.SucuralGrupo;
                    $("#slcOferta").empty();
                    var optionP = $(document.createElement('option'));
                    optionP.text('--Seleccionar--');
                    optionP.val('-1');
                    $("#slcOferta").append(optionP);

                    $(OFertas).each(function (i, d) {
                        $(d.Ofertas).each(function (i1, d1) {

                            if (sucursalid == 3) {
                                if ($("#slcOferta option[value='" + d.OfertaEducativaTipoId + "']").length == 0 && d.OfertaEducativaTipoId != 4) {
                                    var option = $(document.createElement('option'));
                                    option.text(d.Descripcion);
                                    option.val(d.OfertaEducativaTipoId);
                                    $("#slcOferta").append(option);
                                }

                            } else if (this.sucursalid == sucursalid) {

                                if ($("#slcOferta option[value='" + d.OfertaEducativaTipoId + "']").length == 0 && d.OfertaEducativaTipoId != 4) {
                                    var option = $(document.createElement('option'));
                                    option.text(d.Descripcion);
                                    option.val(d.OfertaEducativaTipoId);
                                    $("#slcOferta").append(option);
                                }

                                //CargarOfertasL(rowadd.OfertaEducativaId);
                            }
                        });
                    });

                    $(OFertas).each(function (i, d) {
                        $(d.Ofertas).each(function (i1, d1) {
                            if (this.ofertaEducativaId == rowadd.OfertaEducativaId) {
                                $("#slcOferta").val(d.OfertaEducativaTipoId);
                                EmpresasFn.CargarOfertasL(rowadd.OfertaEducativaId);
                            }
                        });
                    });


                    GrupoI = rowadd.AlumnoCuota.GrupoId;
                    OfertaI = rowadd.OfertaEducativaId;

                    //console.log(rowadd);
                    $('#PopAlumnoConfiguracion').modal('show');
                });

            },
            CargarSede() {
                $("#slcPlantel").empty();
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarPlantelEmpresas",
                    data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        var datos = data.d;
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));

                            option.text(this.DescripcionId);
                            option.val(this.SucursalId);

                            $("#slcPlantel").append(option);
                        });
                        $("#slcPlantel").val('1');
                        $('#slcPlantel').change();
                    }
                });
            },
            Cargar() {
                $('#Load').modal('show');
                $.ajax({
                    url: 'WS/Empresa.asmx/ListarEmpresas',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: '{}',
                    dataType: 'json',
                    success: function (Respuesta) {
                        MItable = $('#tblEmpresa').dataTable({
                            "aaData": Respuesta.d,
                            "aoColumns": [
                                { "mDataProp": "EmpresaId", "RazonSocial": "EmpresaId", "sWidth": "10%" },
                                { "mDataProp": "RFC", "sWidth": "30%" },
                                {
                                    "mDataProp": "RazonSocial", "sWidth": "30%",
                                    "mRender": function (data) {
                                        return "<a href=''onclick='return false;'>" + data + " </a> ";
                                    }
                                },
                                { "mDataProp": "FechaAltaS", "sWidth": "30%" },
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "async": true,
                            "bDestroy": true,
                            "language": {
                                "lengthMenu": "_MENU_  Registros",
                                "paginate": {
                                    "previous": "<",
                                    "next": ">"
                                },
                                "search": "Buscar Empresa "
                            },
                            "order": [[2, "desc"]]
                            , "fnDrawCallback": function (oSettings) {
                                EmpresasFn.cambiarNumero(this[0].id);
                            }
                        });
                        EmpresasFn.Paises();
                    },
                    error: function (Respuesta) {
                        alertify.alert('Error al cargar datos', function () {
                            return false;
                        });
                    }
                });
            },
            cambiarNumero(tutabla) {

                var Descrip = $('#' + tutabla);

                Descrip = Descrip[0].parentElement.parentElement.childNodes[2].childNodes[0];
                Descrip = Descrip.childNodes[0];
                var Texto = Descrip.innerHTML;
                Texto = Texto.split(" ");
                Texto = Texto[5];

                Texto = "Numero total de registros: " + Texto;
                Descrip.innerHTML = Texto;

            },
            Estados() {

                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarEntidadFederativa",
                    data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        objEstados = data.d;
                        $('#slcPaisUni').change();
                        $('#slcPaisUniFiscal').change();
                        $('#Load').modal('hide');
                    }
                });
            },
            Paises() {
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarPaisesT",
                    data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        var datos = data.d;
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));
                            var option1 = $(document.createElement('option'));
                            option.text(this.Descripcion);
                            option.val(this.PaisId);
                            option1.text(this.Descripcion);
                            option1.val(this.PaisId);

                            $('#slcPaisUni').append(option);
                            $('#slcPaisUniFiscal').append(option1);
                        });
                        $('#slcPaisUni').val('146');
                        $('#slcPaisUniFiscal').val('146');
                        EmpresasFn.Estados();


                    }
                });
            },
            CargarDireccion() {
                var id = $("#slcPlantel").val();
                $.ajax({
                    type: "POST",
                    url: "WS/Empresa.asmx/DireccionEmpresa",
                    data: "{SucursalId:'" + id + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        var datos = data.d;
                        $('#txtDireccion').val(datos);
                    }
                });
            },
            CargarTipo() {
                $('#slcOferta').empty();
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/OfertaEducativaTipo",
                    data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        OFertas = data.d;
                    }
                });
            },
            CargarDescuento(Oferta) {
                var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();
                $.ajax({
                    type: "POST",
                    url: "WS/Descuentos.asmx/TraerDescuentosPeriodo",
                    data: "{'OfertaEducativaId':" + Oferta + ",Periodo:'" + Periodo + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        var monto;

                        $('#txtcuotaIn').val('$' + data.d[0].Monto);
                        monto = (data.d[0].Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                        monto = data.d[0].Monto - monto;
                        $('#txtPagarIn').text('$' + String(monto));

                        CuotaIdColegiatura = data.d[1].CuotaId;
                        $('#txtcuotaCol').val('$' + data.d[1].Monto);
                        monto = (data.d[1].Monto * (parseFloat($('#txtDescuentoBec2').val()) / 100));
                        monto = data.d[1].Monto - monto;
                        $('#txtPagarCol').text('$' + String(monto));


                    }
                });
            },
            CargarGrupo(EmpresaId) {
                $.ajax({
                    url: 'WS/Empresa.asmx/ListarGrupos',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: "{EmpresaId:'" + EmpresaId + "'}",
                    dataType: 'json',
                    success: function (Respuesta) {
                        MiGrupo = $('#tblGrupo').dataTable({
                            "aaData": Respuesta.d,
                            "aoColumns": [
                                { "mDataProp": "GrupoId", "Descripcion": "GrupoId" },
                                {
                                    "mDataProp": "Descripcion",
                                    "mRender": function (data) {
                                        return "<a name='btnAlumos'href=''onclick='return false;'>" + data + " </a> ";
                                    }
                                },
                                { "mDataProp": "FechaInicioS" },
                                {
                                    "mDataProp": function (data) {
                                        return "<a class='btn yellow' name ='btnModificar'>Modificar</a>";
                                    }
                                }
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": false,
                            "ordering": true,
                            "info": true,
                            "async": true,
                            "bDestroy": true,
                            "language": {
                                "lengthMenu": "_MENU_  Registros",
                                "paginate": {
                                    "previous": "<",
                                    "next": ">"
                                },
                                "search": "Buscar Empresa "
                            },
                            "order": [[2, "desc"]]
                            , "fnDrawCallback": function (oSettings) {
                                EmpresasFn.cambiarNumero(this[0].id);
                            }
                        });

                        $('#PopGrupo').modal('hide');
                    },
                    error: function (Respuesta) {
                        alertify.alert('Error al cargar datos', function () {
                            return false;
                        });
                    }
                });
            },
            CargarAlumnoDeEmpresa(grupoId) {
                $.ajax({
                    url: 'WS/Alumno.asmx/ConsultarAlumnosDeEmpresa',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: "{grupoId:'" + grupoId + "'}",
                    dataType: 'json',
                    success: function (Respuesta) {
                        AlumnosEmpresa = $('#tblAlumnosN').dataTable({
                            "aaData": Respuesta.d,
                            "aoColumns": [
                                { "mDataProp": "AlumnoId", "Nombre": "AlumnoId" },
                                { "mDataProp": "Nombre" },
                                { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                { "mDataProp": "FechaRegistro" },
                                { "mDataProp": "Usuario.Nombre" },
                                {
                                    "mDataProp": "AlumnoId", "Nombre": "AlumnoId",
                                    "mRender": function (data) {
                                        return '<button type="button" class="btn blue" ><i class="fa fa-plus-square"></i> Seleccionar </button>';
                                    }
                                },
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": true,
                            "ordering": true,
                            "info": true,
                            "async": true,
                            "bDestroy": true,
                            "language": {
                                "lengthMenu": "_MENU_  Registros",
                                "paginate": {
                                    "previous": "<",
                                    "next": ">"
                                },
                                "search": "Buscar Alumno "
                            },
                            "order": [[2, "desc"]],
                            "createdRow": function (row, data, dataIndex) {
                                if (Seleccionados.search(data.AlumnoId) > -1) {
                                    var StileRow = row.childNodes[5];
                                    StileRow = StileRow.childNodes[0];
                                    StileRow.disabled = false;
                                } else {
                                    var StileRow = row.childNodes[5];
                                    StileRow = StileRow.childNodes[0];
                                    StileRow.disabled = true;
                                }
                            }
                            , "fnDrawCallback": function (oSettings) {
                                EmpresasFn.cambiarNumero(this[0].id);
                            }
                        });
                    },
                    error: function (Respuesta) {
                        alertify.alert('Error al cargar datos');
                    }
                });
            },
            LimpiarGrupo() {
                $("#frmGrupo").trigger('reset');
                error1.hide();
                success1.hide();
                $('#frmGrupo  div').removeClass('has-error');
                $('#frmGrupo  div').removeClass('has-success');
                $('#frmGrupo  i').removeClass('fa-warning');
                $('#PopGrupo').modal('hide');
                EmpresasFn.CargarSede();
            },
            PlanPago() {
                $("#slcSistemaPago").empty();
                var OFerta = $('#slcOferta').val();
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarPagosPlanLenguas",
                    data: "{Oferta:'" + OFerta + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        var datos = data.d;
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));
                            option.text(this.PlanPago);
                            option.val(this.PagoPlanId);

                            $("#slcSistemaPago").append(option);
                        });
                    }
                });
            },
            CargarTabla() {
                TM = 1;
                $.ajax({
                    type: "POST",
                    url: "WS/Empresa.asmx/ListarAlumnos",
                    data: "{GrupoId:'" + 0 + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d != null) {
                            tblAlumnosCompletos = $('#tblAlumnosCom').dataTable({
                                "aaData": data.d,
                                "aoColumns": [
                                    { "mDataProp": "AlumnoId" },
                                    { "mDataProp": "Nombre" },
                                    { "mDataProp": "OfertaEducativaS" },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            if (d.AlumnoCuota != null) {
                                                link = "$ " + d.AlumnoCuota.CuotaInscripcion;
                                            }
                                            else { link = "No" }

                                            return link;
                                        }
                                    },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            if (d.AlumnoCuota != null) {
                                                link = "$ " + d.AlumnoCuota.CuotaColegiatura;
                                            }
                                            else { link = "No" }

                                            return link;
                                        }
                                    },

                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            if (d.AlumnoCuota != null) {
                                                if (d.AlumnoCuota.GrupoId == 0) {
                                                    link = "<a href='' class='btn blue' onclick='return false;'>" + "Agregar Grupo" + " </a> ";
                                                }
                                                else { link = "<a href='' class='btn red' onclick='return false;'>" + "Ver Grupo" + " </a> "; }
                                            } else { link = "No" }
                                            return link;
                                        }
                                    },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            if (d.AlumnoCuota != null) {

                                                if (d.AlumnoCuota.EstatusId == 8) {
                                                    if (d.AlumnoCuota.GrupoId == 0) {
                                                        link = "<button href='' class='btn blue' onclick='return false;' disabled>" + "Agregar Configuracion" + " </button> ";
                                                    } else { link = "<button href='' class='btn blue' onclick='return false;'>" + "Agregar Configuracion" + " </button> "; }
                                                }
                                                else { link = "<button href='' class='btn red' onclick='return false;'>" + "Ver Configuracion" + " </button> "; }
                                            }
                                            else { link = "No" }

                                            return link;
                                        }
                                    },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {

                                            var link = "- -";
                                            if (d.AlumnoCuota != null) {
                                                if (d.AlumnoCuota.EstatusId !== 8) {
                                                    link = "<button href='' class='btn blue' name='VerCredenciales' onclick='return false;'>" + "Ver" + " </button> ";
                                                }
                                            }
                                            else { link = "No" }
                                            return link;
                                        }
                                    }
                                ],
                                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                                "searching": true,
                                "ordering": true,
                                "info": true,
                                "async": true,
                                "bSort": false,
                                "bDestroy": true,
                                "language": {
                                    "lengthMenu": "_MENU_  Registros",
                                    "paginate": {
                                        "previous": "<",
                                        "next": ">"
                                    },
                                    "search": "Buscar Alumno "
                                },
                                "aaSorting": []
                                , "fnDrawCallback": function (oSettings) {
                                    EmpresasFn.cambiarNumero(this[0].id);
                                }
                            });
                            EmpresasFn.CargarEmpresasLigero();
                        }
                        else { alertify.alert("Error"); }
                    }
                });
            },
            CargarEmpresaGrupos() {
                $.ajax({
                    type: "POST",
                    url: "WS/Empresa.asmx/--",
                    data: query,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        EmpresaAlumno = data.d;
                    }
                });
            },
            GuardarConfiguracion() {
                var usuario = $.cookie('userAdmin');
                var periodo = $('#slcPeriodo').val();
                var periodoId = periodo.substring(0, 1);
                var anio = periodo.substring(2, 6);
                var obj = {
                    'AlumnoConfig': {
                        'AlumnoId': AlumnoConfig.AlumnoId,
                        'OfertaEducativaId': $('#slcOfertaEducativa').val(),
                        'OfertaEducativaIdAnterior': OfertaI,
                        'PagoPlanId': $('#slcSistemaPago').val(),
                        'CuotaInscripcion': $('#txtCuotaInscripcion').val(),
                        'CuotaColegiatura': $('#txtCuotaColegiatura').val(),
                        'EsCuotaCongelada': $('#chkCongelada')[0].checked,
                        'EsInscripcionCongelada': $('#chkInCongelada')[0].checked,
                        'EsEspecial': $('#chkEspecial')[0].checked,
                        'NoPagos': $('#txtNPagos').val(),
                        'UsuarioId': usuario,
                        'GrupoId': GrupoI,
                        'Anio': anio,
                        'PeriodoId': periodoId,
                        'Credenciales': $('#chkCredenciales')[0].checked
                    }
                };
                obj = JSON.stringify(obj);
                $.ajax({
                    type: "POST",
                    url: "WS/Empresa.asmx/GuardarConfiguracion",
                    data: obj,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d) {
                            alertify.alert("Configuración Guardada");
                            $('#PopAlumnoConfiguracion').modal('hide');
                            if (TM == 1) { EmpresasFn.CargarTabla(); } else { EmpresasFn.CargarTablaAlumnosGrupo(GrupoI) }
                        } else {
                            $('#Load').modal('hide');
                            alertify.alert("Error al guardar, intente nuevamente");
                        }
                    }
                });
            },
            CargarOfertasL(OFertaEducativaId) {
                $("#slcOfertaEducativa").empty();
                var optionP = $(document.createElement('option'));
                optionP.text('--Seleccionar--');
                optionP.val('-1');
                $("#slcOfertaEducativa").append(optionP);
                var tipo = $("#slcOferta");
                tipo = tipo[0].value;

                if (OFertas.length > 0) {

                    $(OFertas).each(function (i, d) {
                        if (tipo == String(this.OfertaEducativaTipoId)) {
                            $(d.Ofertas).each(function (i1, d1) {

                                if (sucursalid == 3) {
                                    var option = $(document.createElement('option'));
                                    option.text(this.descripcion);
                                    option.val(this.ofertaEducativaId);

                                    $("#slcOfertaEducativa").append(option);
                                } else if (sucursalid == this.sucursalid) {
                                    var option = $(document.createElement('option'));
                                    option.text(this.descripcion);
                                    option.val(this.ofertaEducativaId);

                                    $("#slcOfertaEducativa").append(option);
                                }

                            });
                        }
                    });
                } else {
                    $("#slcOfertaEducativa").append(optionP);
                }
                $("#slcOfertaEducativa").val(OFertaEducativaId);
                EmpresasFn.PlanPago();
            },
            CargarEmpresasLigero() {
                $.ajax({
                    type: "POST",
                    url: "WS/Empresa.asmx/ListarEmpresaLigera",
                    data: "{}",
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {

                        DatosEmpresas = data.d;

                        $("#slcEmpresa").empty();
                        var option0 = $(document.createElement('option'));
                        option0.text("--Seleccionar--");
                        option0.val(-1);
                        $("#slcEmpresa").append(option0);

                        $(DatosEmpresas).each(function () {

                            var option = $(document.createElement('option'));
                            option.text(this.RazonSocial);
                            option.val(this.EmpresaId);

                            $("#slcEmpresa").append(option);
                        });

                        $('#Load').modal('hide');
                    }
                });
            },
            CargarTablaAlumnosGrupo(GrupoId) {
                TM = 2;
                $.ajax({
                    type: "POST",
                    url: "WS/Empresa.asmx/ListarAlumnos",
                    data: "{GrupoId:'" + GrupoId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d != null) {
                            tblAlumnosCompletos1 = $('#tblAlumnosCom1').dataTable({
                                "aaData": data.d,
                                "aoColumns": [
                                    { "mDataProp": "AlumnoId" },
                                    { "mDataProp": "Nombre" },
                                    { "mDataProp": "OfertaEducativaS" },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            link = "$ " + d.AlumnoCuota.CuotaInscripcion;
                                            return link;
                                        }
                                    },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            link = "$ " + d.AlumnoCuota.CuotaColegiatura;
                                            return link;
                                        }
                                    },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {
                                            var link;

                                            if (d.AlumnoCuota.EstatusId == 2) {
                                                link = "<a href='' class='btn red' onclick='return false;' disabled>" + "Modificar Grupo" + " </a> ";
                                            }
                                            else { link = "<a href='' class='btn red' onclick='return false;'>" + "Modificar Grupo" + " </a> "; }
                                            return link;
                                        }
                                    },
                                    {
                                        "mDataProp": "AlumnoCuota",
                                        "mRender": function (data, f, d) {

                                            var link;

                                            if (d.AlumnoCuota.EstatusId == 8) {
                                                link = "<button href='' class='btn blue' onclick='return false;'>" + "Agregar Configuracion" + " </button> ";
                                            }
                                            else { link = "<button href='' class='btn red' onclick='return false;'>" + "Ver Configuracion" + " </button> "; }
                                            return link;
                                        }
                                    },
                                ],
                                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                                "searching": true,
                                "ordering": true,
                                "info": true,
                                "async": true,
                                "bSort": false,
                                "bDestroy": true,
                                "language": {
                                    "lengthMenu": "_MENU_  Registros",
                                    "paginate": {
                                        "previous": "<",
                                        "next": ">"
                                    },
                                    "search": "Buscar Alumno "
                                },
                                "aaSorting": []
                                , "fnDrawCallback": function (oSettings) {
                                    //oSettings.aiDisplay.length;
                                    EmpresasFn.cambiarNumero(this[0].id);
                                }
                            });

                            $('#Load').modal('hide');
                        }
                        else { alertify.alert("Error"); }
                    }
                });
            }
        };

    EmpresasFn.init();

});