$(function () {
    var Seleccionados = "", MItable, DatosEmpresas, AlumnoId, TipoMovimiento, ModificarGrupo, TM, sucursalid, tblAlumnosCompletos1;
    var MiGrupo, Fila, EmpresaI, GrupoI, OfertaI, tblAlumnosCompletos, AlumnoConfig = { AlumnoId: "", GrupoId: "" };
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
                GlobalFn.init();
                GlobalFn.GetPeriodo_N_I();
                GlobalFn.GetPlantel();
                GlobalFn.GetPais("slcPaisUni", 146).done(function () { $("#slcPaisUni").change(); });
                GlobalFn.GetPais("slcPaisUniFiscal", 146).done(function () { $("#slcPaisUniFiscal").change(); });
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
                        slcEstado: {
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
                        slcTipoOferta: {
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
                if (jQuery().datepicker) {
                    $('.date-picker').datepicker({
                        rtl: Metronic.isRTL(),
                        orientation: "left",
                        autoclose: true,
                        language: 'es'
                    });
                    $(".date-picker").datepicker("setDate", Fecha);
                }
                $('#btnEmpresa').click(function () { $('#NuevaEmpresa').modal('show'); });
                $("#slcPaisUni").change(EmpresasFn.PaisUniChange);
                $("#slcPaisUniFiscal").change(EmpresasFn.PaisUniFiscalChange);
                $('#slcEstadoPaisFiscal').change(EmpresasFn.EstadoPaisFiscalChange);
                $('#btnSiguiente').click(EmpresasFn.BtnSiguienteClick);
                $('#chkFiscales').click(EmpresasFn.ChkFiscalesClick);
                $('#btnGuardar').click(EmpresasFn.BtnGuardarClick);
                $('#tblEmpresa').on('click', 'a', EmpresasFn.TblEmpresaClick);
                $('#btnGrupo').click(EmpresasFn.BtnGrupoClick);
                $('#btnAtrasE').click(function () { $('#EmpresaLista').show(); $('#EmpresaC').hide(); });
                $('#slcPlantel').change(EmpresasFn.CargarDireccion);
                $('#btnGuardarGrupo').click(EmpresasFn.GuardarGrupo);
                $('#btnCerrarGrupo').click(EmpresasFn.btnCerrarGrupoClick);
                $('#ListaGrupos').on('click', 'a', EmpresasFn.ListaGruposClick);
                $('#btnAlumnos').on('click', EmpresasFn.BtnAlumnosClick);
                $('#btnAlumnos1').on('click', EmpresasFn.BtnAlumnos1Click);
                $('#btnAtrasAlumnos').on('click', EmpresasFn.BtnAtrasAlumnosClick);
                $('#tblAlumnosCom').on('click', 'button', EmpresasFn.TblAlumnosComBtnClick);
                $('#tblAlumnosCom').on('click', 'a', EmpresasFn.TblAlumnosComClick);
                $("#slcEmpresa").change(EmpresasFn.SlcEmpresaChange);
                $('#btnAlumnoGrupoG').on('click', EmpresasFn.BtnAlumnoGrupoGClick);
                $('#btnCerrar').mousedown(function () { $('#txtDescripcion').val(''); $('#txtArea').val(''); });
                $('#btnAtras').click(EmpresasFn.BtnAtrasClick);
                $('#btnAtrasG').click(EmpresasFn.BtnAtrasGClick);
                $("#txtNPagos").keypress(EmpresasFn.TxtNPagosKeypress);
                $('#btnAlumnoConfig').on('click', EmpresasFn.GuardarConfiguracion);
                $('#btnAtrasAlumnosGrupos').on('click', function () { $('#EmpresaC').show(); $('#DivAlumnosGrupo').hide(); });
                $('#tblAlumnosCom1').on('click', 'a', EmpresasFn.TblAlumnosCom1AClick);
                $('#tblAlumnosCom1').on('click', 'button', EmpresasFn.TblAlumnosCom1BtnClick);
            },
            PaisUniChange() {
                GlobalFn.GetEstado("slcEstado", -1).done(function () {
                    if ($("#slcPaisUni").val() != 146) {
                        $("#slcEstado > option").each(function () {
                            if (this.value != 33 && this.value != -1)
                                $(this).remove();
                        });
                    }
                });
            },
            PaisUniFiscalChange() {
                GlobalFn.GetEstado("slcEstadoPaisFiscal", -1).done(function () {
                    if ($("#slcPaisUniFiscal").val() != 146) {
                        $("#slcEstadoPaisFiscal > option").each(function () {
                            if (this.value != 33 && this.value != -1)
                                $(this).remove();
                        });
                    }
                    if ($('#chkFiscales')[0].checked == false) {
                        $("#slcEstadoPaisFiscal").val('-1');
                    } else {
                        $("#slcEstadoPaisFiscal").val($('#slcEstado').val());
                        $("#slcEstadoPaisFiscal").change();
                    }
                });
            },
            EstadoPaisFiscalChange() {
                $("#slcDelegacionFiscal").empty();
                var Entidad = $("#slcEstadoPaisFiscal");
                var optionP = $(document.createElement('option'));
                optionP.text('--Seleccionar--');
                optionP.val('-1');
                $("#slcDelegacionFiscal").append(optionP);

                Entidad = Entidad[0].value;
                IndexFn.Api("General/ConsultarMunicipios/" + Entidad, "GET", "")
                    .done(function (data) {
                        var datos = data;
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));

                            option.text(this.Descripcion);
                            option.val(this.EntidadFederativaId);

                            $("#slcDelegacionFiscal").append(option);
                        });
                        if ($('#chkFiscales')[0].checked == false) {
                            $("#slcDelegacionFiscal").val('-1');
                        } else {
                            $("#slcDelegacionFiscal").val($('#slcMunicipio').val());
                        }
                    })
                    .fail(function (data) {
                        console.log(data);
                    });

            },
            BtnSiguienteClick() {
                if (form.valid() == false) { return false; }
                error.hide();
                $('#divDatosEmpresa').hide();
                $('#divFiscales').show();
                $('#btnAtras').removeAttr('style');
                $('#btnSiguiente').attr('style', 'display: none');
                $('#btnGuardar').removeAttr('style');
            },
            ChkFiscalesClick() {
                if ($(this).is(':checked')) {
                    $('#slcPaisUniFiscal').prop('disabled', 'disabled');
                    $('#slcPaisUniFiscal').val($('#slcPaisUni').val());
                    $('#slcPaisUniFiscal').change();
                    $('#slcEstadoPaisFiscal').prop('disabled', 'disabled');
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
            },
            BtnGuardarClick() {
                if (form.valid() == false) { return false; }
                $('#NuevaEmpresa').modal('hide');
                var Empresa =
                    {
                        RFC: $('#txtRFC').val(),
                        RazonSocial: $('#txtDescripcion').val(),
                        FechaV: $('#txtFechaV').val(),
                        Usuarioid:  localStorage.getItem('userAdmin'),
                        EmpresaDetalle:
                        {
                            Nombre: $('#txtNombre').val(),
                            Paterno: $('#txtPaterno').val(),
                            Materno: $('#txtMaterno').val(),
                            EmailContacto: $('#txtEmail').val(),
                            Telefono: $('#txtTelefono').val(),
                            Celular: $('#txtCelular').val(),
                            PaisId: $('#slcPaisUni').val(),
                            EntidadFederativaId: $('#slcEstado').val(),
                            MunicipioId: $('#slcMunicipio').val(),
                            CP: $('#txtCP').val(),
                            Colonia: $('#txtColonia').val(),
                            Calle: $('#txtCalle').val(),
                            NoExterior: $('#NoExterior').val(),
                            NoInterior: $('#NoInterior').val(),
                            Email: $('#txtmail').val(),
                            Observacion: $('#txtObservacion').val(),
                            DatosFiscales:
                            {
                                RFC: $('#txtRFC').val(),
                                PaisId: $('#slcPaisUniFiscal').val(),
                                EntidadFederativaId: $('#slcEstadoPaisFiscal').val(),
                                MunicipioId: $('#slcDelegacionFiscal').val(),
                                CP: $('#txtCPFiscal').val(),
                                Colonia: $('#txtColoniaFiscal').val(),
                                Calle: $('#txtCalleFiscal').val(),
                                NoExterior: $('#NoExteriorFiscal').val(),
                                NoInterior: $('#NoInteriorFiscal').val(),
                                Observacion: $('#txtObservacionFiscal').val()
                            }
                        },
                    };

                IndexFn.Api("Empresas/GuardarEmpresa", "POST", JSON.stringify(Empresa))
                    .done(function (data) {
                        $('#NuevaEmpresa').modal('show');
                        if (data == "True") {
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
                                $('#slcPaisUni').val(146);
                                $('#slcPaisUniFiscal').val(146);
                                EmpresasFn.Cargar();
                            });
                        }
                    })
                    .fail(function (data) {
                        alertify.alert("Error, no fue posible guardar la empresa");
                        console.log(data);
                    });

            },
            TblEmpresaClick() {
                $('#NombreEmpresa').text(this.innerHTML);
                EmpresaI = this.parentNode.parentNode;
                EmpresaI = MItable.fnGetData(EmpresaI, 0);
                EmpresasFn.CargarGrupo(EmpresaI);
                $('#EmpresaLista').hide();
                $('#EmpresaC').show();
            },
            BtnGrupoClick() {
                EmpresasFn.LimpiarGrupo();
                $('#hGrupos').text("Alta de Grupo");
                GrupoI = 0;
                $('#PopGrupo').modal('show');
            },
            btnCerrarGrupoClick() {
                error1.hide();
                success1.hide();
                $('#frmGrupo  div').removeClass('has-error');
                $('#frmGrupo  div').removeClass('has-success');
                $('#frmGrupo  i').removeClass('fa-warning');
                $('#PopGrupo').modal('hide');
            },
            ListaGruposClick() {
                if (this.innerHTML == "Nuevo Grupo") { return false; }
                if (this.name == "btnAlumos") {
                    $('#NombreEmpresa1').text($('#NombreEmpresa').text());
                    $('#NombreGrupo1').text(this.innerHTML);
                    var rFila = this.parentNode.parentNode;
                    var row = this.parentNode.parentNode;
                    var rowadd = MiGrupo.fnGetData($(this).closest('tr'));
                    rFila = MiGrupo.fnGetData(rFila, 0);
                    GrupoI = rowadd.GrupoId;
                    IndexFn.Block(true);
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
            },
            BtnAlumnosClick() {
                $('#EmpresaLista').hide();
                $('#DivAlumnos').show();
                IndexFn.Block(true);
                EmpresasFn.CargarTabla();
                $('#DivAlumnos').removeAttr('data-origen');
                $('#DivAlumnos').data("origen", 'EmpresaLista');
            },
            BtnAlumnos1Click() {
                $('#DivAlumnosGrupo').hide();
                $('#DivAlumnos').show();
                IndexFn.Block(true);
                EmpresasFn.CargarTabla();
                $('#DivAlumnos').removeAttr('data-origen');
                $('#DivAlumnos').data("origen", 'DivAlumnosGrupo');
                $('#DivAlumnos').data("grupoid", GrupoI);
            },
            BtnAtrasAlumnosClick() {
                var origen = $('#DivAlumnos').data('origen');
                var grupoid = $('#DivAlumnos').data('grupoid');
                if (grupoid != undefined) {
                    EmpresasFn.CargarTablaAlumnosGrupo(grupoid);
                }
                $('#' + origen).show();
                $('#DivAlumnos').hide();
            },
            TblAlumnosComBtnClick() {
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
                        $("#slcTipoOferta").prop('disabled', true);
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
                        $("#slcTipoOferta").prop('disabled', false);
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

                    GlobalFn.DatosOferta.PlantelId = rowadd.AlumnoCuota.SucuralGrupo;
                    GlobalFn.DatosOferta.OfertaEducativaTipoId = rowadd.AlumnoCuota.OfertaEducativaTipoId;
                    GlobalFn.DatosOferta.OFertaEducativa = rowadd.AlumnoCuota.OfertaEducativaId;
                    GlobalFn.GetTipoOferta(GlobalFn.DatosOferta.PlantelId);

                    GrupoI = rowadd.AlumnoCuota.GrupoId;
                    OfertaI = rowadd.OfertaEducativaId;
                    $('#PopAlumnoConfiguracion').modal('show');
                }
            },
            TblAlumnosComClick() {
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
            },
            SlcEmpresaChange() {
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
            },
            BtnAlumnoGrupoGClick() {
                if ($('#slcGrupo').val() != -1) {
                    var grupo = $('#slcGrupo').val();
                    var ofertaid = OfertaI
                    if (GrupoI != grupo) {
                        var usuario =  localStorage.getItem('userAdmin');
                        IndexFn.Block(true);

                        var AlumnoGrupo =
                            {
                                GrupoId: grupo,
                                AlumnoId: AlumnoId,
                                UsuarioId: usuario,
                                OfertaId: OfertaI,
                                TipoMovimiento: TipoMovimiento
                            };
                        IndexFn.Api("Empresas/MovimientosAlumnoGrupo", "POST", JSON.stringify(AlumnoGrupo))
                            .done(function (data) {
                                if (data == "Guardado") {
                                    if (ModificarGrupo == 1) {
                                        EmpresasFn.CargarTabla();
                                    } else {
                                        EmpresasFn.CargarTablaAlumnosGrupo(GrupoI);
                                    }
                                    alertify.alert("Guardado Correctamente");
                                    $('#PopAlumnoGrupo').modal('hide');
                                    IndexFn.Block(false);
                                }
                            })
                            .fail(function (data) {
                                console.log(data);
                            });
                    }
                }
            },
            BtnAtrasClick() {
                $('#divFiscales').hide();
                $('#btnGuardar').attr('style', 'display: none');
                $('#btnAtras').attr('style', 'display: none');
                $('#btnSiguiente').removeAttr('style');
                $('#divDatosEmpresa').show();
            },
            BtnAtrasGClick() {
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
            },
            TxtNPagosKeypress(e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            },
            TblAlumnosCom1AClick() {
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
            },
            TblAlumnosCom1BtnClick() {
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
                    $("#slcTipoOferta").prop('disabled', true);
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
                    $("#slcTipoOferta").prop('disabled', false);
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
                GlobalFn.DatosOferta.PlantelId = rowadd.AlumnoCuota.SucuralGrupo;
                GlobalFn.DatosOferta.OfertaEducativaTipoId = rowadd.AlumnoCuota.OfertaEducativaTipoId;
                GlobalFn.DatosOferta.OFertaEducativa = rowadd.AlumnoCuota.OfertaEducativaId;
                GlobalFn.GetTipoOferta(GlobalFn.DatosOferta.PlantelId);

                GrupoI = rowadd.AlumnoCuota.GrupoId;
                OfertaI = rowadd.OfertaEducativaId;

                $('#PopAlumnoConfiguracion').modal('show');
            },
            Cargar() {
                IndexFn.Block(true);
                IndexFn.Api("Empresas/ListarEmpresas", "GET", "")
                    .done(function (data) {
                        MItable = $('#tblEmpresa').dataTable({
                            "aaData": data,
                            "aoColumns": [
                                { "mDataProp": "EmpresaId", "RazonSocial": "EmpresaId", "sWidth": "10%" },
                                { "mDataProp": "RFC", "sWidth": "30%" },
                                {
                                    "mDataProp": "RazonSocial", "sWidth": "30%",
                                    "mRender": function (datos) {
                                        return "<a href=''onclick='return false;'>" + datos + " </a> ";
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
                                EmpresasFn.CambiarNumero(this[0].id);
                            }
                        });
                        IndexFn.Block(false);
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                        console.log(data);
                    });
            },
            CambiarNumero(tutabla) {
                var Descrip = $('#' + tutabla);
                Descrip = Descrip[0].parentElement.parentElement.childNodes[2].childNodes[0];
                Descrip = Descrip.childNodes[0];
                var Texto = Descrip.innerHTML;
                Texto = Texto.split(" ");
                Texto = Texto[5];
                Texto = "Numero total de registros: " + Texto;
                Descrip.innerHTML = Texto;
            },
            CargarGrupo(EmpresaId) {
                IndexFn.Api("Empresas/ListarGrupos/" + EmpresaId, "GET", "")
                    .done(function (datos) {
                        MiGrupo = $('#tblGrupo').dataTable({
                            "aaData": datos,
                            "aoColumns": [
                                { "mDataProp": "GrupoId", "Descripcion": "GrupoId" },
                                {
                                    "mDataProp": "Descripcion",
                                    "mRender": function (data) {
                                        return "<a name='btnAlumos' href=''onclick='return false;'>" + data + " </a> ";
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
                                EmpresasFn.CambiarNumero(this[0].id);
                            }
                        });

                        $('#PopGrupo').modal('hide');
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                        console.log(data);
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
                GlobalFn.GetPlantel();
            },
            CargarDireccion() {
                var id = $("#slcPlantel").val();
                IndexFn.Api("Empresas/DireccionEmpresa/" + id, "GET", "")
                    .done(function (data) {
                        $('#txtDireccion').val(data);
                    })
                    .fail(function (data) {
                        alertify.alert('Error al cargar datos');
                        console.log(data);
                    });
            },
            GuardarGrupo() {
                if (GrupoFrm.valid() == false) { return false; }

                var grupo =
                    {
                        EmpresaId: EmpresaI,
                        GrupoId: GrupoI,
                        Descripcion: $('#txtNombreGrupo').val(),
                        SucursalId: $('#slcPlantel').val(),
                        SucursalDireccion: $('#txtDireccion').val(),
                        FechaInicioS: $('#txtFechaInicio').val(),
                        UsuarioId:  localStorage.getItem('userAdmin')
                    };

                IndexFn.Api("Empresas/GuardarGrupo", "POST", JSON.stringify(grupo))
                    .done(function (data) {
                        if (data >= 1) {
                            EmpresasFn.LimpiarGrupo();
                            alertify.alert("Grupo Guardado");
                            EmpresasFn.CargarGrupo(EmpresaI);
                        }
                        else { alertify.alert("Error"); }
                    })
                    .fail(function (data) {
                        alertify.alert("Error");
                        console.log(data);
                    });
            },
            CargarTablaAlumnosGrupo(GrupoId) {
                TM = 2;
                IndexFn.Api("Empresas/ListarAlumnos/" + GrupoId, "GET", "")
                    .done(function (datos) {
                        if (datos != null) {
                            tblAlumnosCompletos1 = $('#tblAlumnosCom1').dataTable({
                                "aaData": datos,
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
                                    EmpresasFn.CambiarNumero(this[0].id);
                                }
                            });

                            IndexFn.Block(false);
                        }
                        else { alertify.alert("Error"); }
                    })
                    .fail(function (data) {
                        alertify.alert("Error");
                        console.log(data);
                    });
            },
            CargarEmpresasLigero() {
                IndexFn.Api("Empresas/ListarEmpresaLigera", "GET", "")
                    .done(function (data) {
                        DatosEmpresas = data;

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

                        IndexFn.Block(false);
                    })
                    .fail(function (data) {
                        alertify.alert("Error");
                        console.log(data);
                    });
            },
            CargarTabla() {
                TM = 1;
                IndexFn.Api("Empresas/ListarAlumnos/" + 0, "GET", "")
                    .done(function (datos) {
                        if (datos != null) {
                            tblAlumnosCompletos = $('#tblAlumnosCom').dataTable({
                                "aaData": datos,
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
                                    EmpresasFn.CambiarNumero(this[0].id);
                                }
                            });
                            EmpresasFn.CargarEmpresasLigero();
                        }
                        else { alertify.alert("Error"); }
                    })
                    .fail(function (data) {
                        alertify.alert("Error");
                        console.log(data);
                    });

            },
            GuardarConfiguracion() {
                if (formAlumno.valid()) {
                    IndexFn.Block(true);
                    var usuario =  localStorage.getItem('userAdmin');
                    var periodo = $('#slcPeriodo').val();
                    var periodoId = periodo.substring(0, 1);
                    var anio = periodo.substring(2, 6);
                    var obj = {
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
                    };

                    IndexFn.Api("Empresas/GuardarConfiguracion", "POST", JSON.stringify(obj))
                        .done(function (data) {
                            if (data) {
                                alertify.alert("Configuración Guardada");
                                $('#PopAlumnoConfiguracion').modal('hide');
                                if (TM == 1) { EmpresasFn.CargarTabla(); } else { EmpresasFn.CargarTablaAlumnosGrupo(GrupoI) }
                            } else {
                                IndexFn.Block(false);
                                alertify.alert("Error al guardar, intente nuevamente");
                            }
                        })
                        .fail(function (data) {
                            alertify.alert("Error");
                            console.log(data);
                        });

                }
            }
        };

    EmpresasFn.init();

});