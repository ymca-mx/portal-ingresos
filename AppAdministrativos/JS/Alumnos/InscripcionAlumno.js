$(function () {
    
    var Usuario,
        esEmpresa,
        b_Valid = 0,
        AlumnoPromocion,
        tblAlumno2,
        dataAlumno = [],
        hayPromocion = false,
        MesP = [];

    var FormWizard = function () {
        $('#divMaterial').hide();
        var form = $('#submit_form');
        var error = $('.alert-danger', form);
        var success = $('.alert-success', form);
        var keydown;
        var text;
        var Respuesta;
        form.validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block help-block-error', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                txtnombre: {
                    required: true,
                    minlength: 3,
                    maxlength: 50,
                },
                txtemail: {
                    required: true,
                    email: true,
                    minlength: 4,
                    maxlength: 100
                },
                txtApPaterno: {
                    required: true,
                    digits: false,
                    minlength: 2,
                    maxlength: 50
                },
                txtApMaterno: {
                    required: true,
                    digits: false,
                    minlength: 2,
                    maxlength: 50
                },
                slcEstadoCivil: {
                    required: true,
                    min: 1
                },
                txtCelular: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 13
                },
                txtFNacimiento: {
                    required: true,
                    digits: false,
                },
                slcSexo: {
                    required: true,
                    min: 1
                },
                txtCURP: {
                    required: true,
                    minlength: 18,
                    maxlength: 18
                },
                txtTelefonoCasa: {
                    digits: true,
                    required: true,
                    minlength: 8,
                    maxlength: 12
                },
                txtCalle: {
                    required: true,
                    maxlength: 100,
                    minlength: 3
                },
                txtNumeroE: {
                    required: true,
                    maxlength: 30,
                    minlength: 1
                },
                txtNumeroI: {
                    maxlength: 30,
                    minlength: 1
                },
                txtCP: {
                    required: true,
                    digits: true,
                    minlength: 5
                },
                txtColonia: {
                    required: true,
                    maxlength: 100,
                    minlength: 1
                },
                slcMunicipio: {
                    min: 1,
                    required: true,
                },
                slcEstado: {
                    required: true,
                    min: 1
                },
                slcLugarN: {
                    required: true,
                    min: 1
                },
                //Persona Autorizada
                txtPAutorizada: {
                    required: true,
                    maxlength: 50,
                    minlength: 1

                },
                txtAPPaterno: {
                    required: true,
                    maxlength: 50,
                    minlength: 1
                },
                txtAPMaterno: {
                    required: true,
                    maxlength: 50,
                    minlength: 1
                },
                slcParentesco: {
                    required: true,
                    min: 1
                },
                txtPEmail: {
                    maxlength: 100,
                    minlength: 5
                },
                txtTelefonoPA: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 13
                },
                txtTelefonoPAT: {
                    digits: true,
                    minlength: 8,
                    maxlength: 12
                },
                //Autoriza 2
                txtPAutorizada2: {
                    maxlength: 50,
                    minlength: 1
                },
                txtAPPaterno2: {
                    maxlength: 50,
                    minlength: 1
                },
                txtAPMaterno2: {
                    maxlength: 50,
                    minlength: 1
                },
                slcParentesco2: {
                    min: -1
                },
                txtPEmail2: {
                    maxlength: 100,
                    minlength: 5
                },
                txtTelefonoPA2: {
                    digits: true,
                    minlength: 8,
                    maxlength: 12
                },
                txtTelefonoPAT2: {
                    digits: true,
                    minlength: 8,
                    maxlength: 12
                },
                //Oferta Educativa
                slcPlantel: {
                    required: true,
                    min: 1
                },
                slcTipoOferta: {
                    required: true,
                    min: 1
                },
                slcOfertaEducativa: {
                    required: true,
                    min: 1
                },
                slcTurno: {
                    required: true,
                    min: 1
                },
                slcPeriodo: {
                },
                txtNombrePrepa: {
                    required: true,
                    maxlength: 100,
                    minlength: 5
                },
                txtPromedio: {
                    required: true,
                    number: true,
                    minlength: 1,
                },
                txtAñoT: {
                    required: true,
                    number: true,
                    minlength: 4,
                    maxlength: 4,
                },
                txtMesT: {
                    required: true,
                    number: true,
                    minlength: 1,
                    maxlength: 2,
                    min: 1,
                    max: 12

                },
                slcArea: {
                    min: 1
                },
                txtUni: {
                    maxlength: 100,
                    minlength: 2
                },
                InsArchivo: {
                    extension: 'pdf'
                },
                BecArchivo: {
                    extension: 'pdf'
                },
                slcNacionalidad: {
                    required: true,
                    min: 1
                },
                slcNacionalidadPrep: {
                    required: true,
                    min: 1
                },
                slcEstadoPais: {
                    required: true,
                    min: 1
                },
                slcMedio: {
                    required: true,
                    min: 1
                }
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

        var handleTitle = function (tab, navigation, index) {
            var total = navigation.find('li').length;
            var current = index + 1;
            // set wizard title
            $('.step-title', $('#form_wizard_1')).text('Ficha ' + (index + 1) + ' de ' + total);
            // set done steps
            jQuery('li', $('#form_wizard_1')).removeClass("done");
            var li_list = navigation.find('li');
            for (var i = 0; i < index; i++) {
                jQuery(li_list[i]).addClass("done");
            }

            if (current == 1) {
                $('#form_wizard_1').find('.button-previous').hide();
                $('#btnListado').show();
                Metronic.scrollTo($('.page-title'));

            } else {
                $('#form_wizard_1').find('.button-previous').show();
                Metronic.scrollTo($('.page-title'));
                $('#btnListado').hide();
            }

            if (current == 3) {
                //$('#form_wizard_1').find('.button-next').removeClass('btn blue button-next');
                var link = $('#form_wizard_1').find('.button-next');
                link[0].innerText = 'Continuar ';
                link.removeClass('btn blue button-next');
                link.addClass('btn green button-submit');
                Metronic.scrollTo($('.page-title'));
                //$('#form_wizard_1').find('.button-next').hide();
                //$('#form_wizard_1').find('.button-guardar').hide();
                //$('#form_wizard_1').find('.button-submit').show();
            }
            else if (current == 2) {
                try {
                    var link = $('#form_wizard_1').find('.button-submit');
                    link[0].innerText = 'Continuar';
                    link.removeClass('btn green button-submit');
                    link.addClass('btn blue button-next');
                    Metronic.scrollTo($('.page-title'));
                }
                catch (err) {
                    console.log(err);
                    $('#form_wizard_1').find('.button-next').show();
                    $('#form_wizard_1').find('.button-submit').hide();
                    Metronic.scrollTo($('.page-title'));
                }
            }
            else if (current >= total) {
                $('#full-width').addClass('in').attr('display', ': block; margin-top: -121px');
                $('#form_wizard_1').find('.button-previous').hide();
                $('#form_wizard_1').find('.button-submit').hide();
                $('#form_wizard_1').find('.button-guardar').show();
            } else {
                $('#form_wizard_1').find('.button-next').show();
                $('#form_wizard_1').find('.button-submit').hide();
                Metronic.scrollTo($('.page-title'));
            }

        };

        // default form wizard
        $('#form_wizard_1').bootstrapWizard({
            'nextSelector': '.button-next',
            'previousSelector': '.button-previous',
            onTabClick: function (tab, navigation, index, clickedIndex) {
                return false;
                /*
                success.hide();
                error.hide();
                if (form.valid() == false) {
                    return false;
                }
                handleTitle(tab, navigation, clickedIndex);
                */
            },
            onNext: function (tab, navigation, index) {
                if (b_Valid == 1) { b_Valid = 0; return false; } else {
                    success.hide();
                    error.hide();
                    handleTitle(tab, navigation, index);
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#form_wizard_1').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
                //|| form.valid() == false
            },
            onPrevious: function (tab, navigation, index) {
                if (b_Valid == 1) { b_Valid = 0; return false; } else {
                    success.hide();
                    error.hide();
                    handleTitle(tab, navigation, index);
                }
            },
            onTabShow: function (tab, navigation, index) {
                if (b_Valid == 1) { b_Valid = 0; return false; } else {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#form_wizard_1').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }

            }
        });
        $('#form_wizard_1').find('.button-guardar').click(function () {
            alertify.confirm("<p>¿Desea continuar guardando los descuentos?<br><br><hr>", function (e) {
                if (e) {
                    $('#hCarga').text("Guardando...");
                    $('#Load').modal('show');
                    InscripcionFn.GuardarDescuentos();
                } else { return false; }
            });
        }).hide();
        $('#form_wizard_1').find('.button-previous').hide();

        $("#txtCP").keypress(function (e) {
            //if the letter is not digit then display error and don't type anything
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });
        //$("#btnListado").on('click', function () {
        //    $('#divDinamico').empty();
        //    var url = $(this).attr("href");
        //    $('#divDinamico').load(url);
        //    return false;
        //});
        $('#Guardar').keydown(function () {
            keydown = 1;
            $('#Guardar').mousedown();
            return false;
        });
        $('#Guardar').mousedown(function (event) {

            var b_Valid2 = b_Valid;
            if (event.which == 1 || keydown == 1) {
                keydown = 0;
                b_Valid = 0;
                success.hide();
                error.hide();
                if (form.valid() == false) {
                    return false;
                } else {
                    var link = $('#form_wizard_1').find('.button-submit');
                    text = link.length == 0 ? 'NP' : link[0].innerText;
                    if (link.length > 0) {
                        var next = false;
                        if ($('#chkEsEmpresa')[0].checked == false) {
                            event.preventDefault();
                            $("#btnSi").on('click', InscripcionFn.btnSiClick);
                            $("#btnNo").on('click', InscripcionFn.btnNoClick);
                            $("#ModalEsEmpresa").modal("show");                            
                        } else {
                            alertify.confirm("<p>¿Esta seguro que desea guardar los cambios?<br><br><hr>", function (e) {
                                if (e) {
                                    InscripcionFn.Invocar();
                                    //$('#form_wizard_1').find('.button-submit').click();
                                }
                            });
                        }

                    } else {
                        $('#form_wizard_1').bootstrapWizard('next');
                        b_Valid = 1;
                    }
                }
            }
            return false;
        });


    };

    var InscripcionFn = {
        init() {
            $('#slcNacionalidad').on('change', this.NacionalidadChange);
            $('#slcNacionalidadPrep').on('change', this.NacionalidadPrepChange);
            $('#slcLugarN').on('chango', this.setPaisEstado);//<<<--- falta 



            $('#txtDescuentoBec').data('labelp', 'txtPagarCol');
            $('#txtDescuentoBec').data('label', 'txtcuotaCol');
            $('#txtDescuentoBec').on('keyup', this.GetMontos);
            $('#txtDescuentoBec').knob({
                'change': function (val) {
                    InscripcionFn.GetMontosKnob(val, 'txtDescuentoBec');
                }
            });

            $('#txtDescuentoIns').data('labelp', 'txtPagarIn');
            $('#txtDescuentoIns').data('label', 'txtcuotaIn');
            $('#txtDescuentoIns').on('keyup', this.GetMontos);
            $('#txtDescuentoIns').knob({
                'change': function (val) {
                    InscripcionFn.GetMontosKnob(val, 'txtDescuentoIns');
                }
            });

            $('#txtDescuentoExa').data('labelp', 'txtPagarExa');
            $('#txtDescuentoExa').data('label', 'txtcuotaExa');
            $('#txtDescuentoExa').on('keyup', this.GetMontos);
            $('#txtDescuentoExa').knob({
                'change': function (val) {
                    InscripcionFn.GetMontosKnob(val, 'txtDescuentoExa');
                }
            });

            $('#txtDescuentoCred').data('labelp', 'txtPagarCred');
            $('#txtDescuentoCred').data('label', 'txtcuotaCred');
            $('#txtDescuentoCred').on('keyup', this.GetMontos);
            $('#txtDescuentoCred').knob({
                'change': function (val) {
                    InscripcionFn.GetMontosKnob(val, 'txtDescuentoCred');
                }
            });

            this.SetFecha();
            GlobalFn.GetGenero();
            GlobalFn.GetMedios();
            GlobalFn.GetPlantel();
            GlobalFn.GetTurno();
            GlobalFn.GetAreas("slcArea");
            GlobalFn.GetEstadoCivil();
            GlobalFn.GetEstado('slcEstado', 9);
            GlobalFn.GetParentesco("slcParentesco");
            GlobalFn.GetParentesco("slcParentesco2");
            GlobalFn.GetPeriodo_N_I();
            $('#slcNacionalidad').val('1').change();

            //$('#slcTipoOferta').on('change', this.ChangeLabels);

            $("#slcPlantel").change(GlobalFn.PlantelChange);
            $("#slcTipoOferta").change(this.ChangeLabels);

            $("#slcEstado").change(GlobalFn.EstadoChange);

            FormWizard();

            $('#slcPeriodo').on('change', this.slcPeriodoChange);
            $('#chkYo').on('click', this.chkYoClick);
            $("#slcTurno").on('change', this.slcTurnoChange);

            $('#chkEsEmpresa').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });
            $("#btnSi").on('click', this.btnSiClick);
            $("#btnNo").on('click', this.btnNoClick);
            $("#btnPromocion").on('click', this.btnPromocionClick);
            $("#dtAlumno1").on("click", "a", this.dtAlumno1ClickA);
            $("#btnClosePromo").on('click', this.btnClosePromoClick);
            $('#txtClave1').on('keydown', this.txtClave1KeyDown);
            $("#btnBuscar1").on('click', this.btnBuscar1Click);
            $('#txtApPaterno').focusout(this.Buscar);
            $("#txtApMaterno").focusout(this.Buscar);
            $('#chkUniSi').on('change', this.ActiveObse);
            $('#chkUniNo').on('change', this.ActiveObse);

            this.Beca();
            this.Inscripcion();
        },
        ActiveObse() {
            if ($('#chkUniSi')[0].checked) {
                $('#txtUniMotivo').attr("disabled", "disabled");
            } else if ($('#chkUniNo')[0].checked) {
                $('#txtUniMotivo').removeAttr("disabled");
            }
        },
        ChangeLabels() {
            $('#txtUni').val('');
            $('#txtUni').attr("disabled", "disabled");
            $('#chkUni').is(':checked') ? $('#chkUni').click() : 'false';
            $('#slcLugarUni').val(-1);
            $('#slcLugarUni').change();
            $('#txtNombreUni').val('');
            $('#chkUniSi').prop("checked", false);
            $('#chkUniNo').prop("checked", false);
            $('txtUniMotivo').val('');
            $('#divPrepa').show();

            var TipoOferta = parseInt($('#slcTipoOferta').val());
            $('#lblOFerta').html(TipoOferta == 1 ? 'Licenciatura' :
                TipoOferta == 2 ? 'Especialidad' :
                    TipoOferta == 3 ? 'Maestría' :
                        TipoOferta == 4 ? 'Idioma' :
                            TipoOferta == 5 ? 'Doctorado' : ' ');

            if (TipoOferta != 4) {
                $('#divPrepa').show();
                $('#divUni').show();
                $('#lblTituloProcedencia').text(TipoOferta == 1 ? 'Preparatoria de Procedencia' : 'Universidad de Procedencia');
                $('#lblLugarProcedencia').text(TipoOferta == 1 ? 'Lugar donde estudio la preparatoria' : 'Lugar donde estudio la Universidad');
                document.getElementById("h3Titulo").innerHTML = TipoOferta == 1 ? 'Preparatoria de Procedencia' : 'Universidad de Procedencia';
            } else {
                $('#divPrepa').hide();
                $('#divUni').hide();
            }

            GlobalFn.GetOfertaEducativa($("#slcPlantel").val(), $("#slcTipoOferta").val());
        },
        NacionalidadChange() {
            if (parseInt($('#slcNacionalidad').val()) === 1) {
                GlobalFn.GetEstado('slcLugarN', 9);
            } else {
                GlobalFn.GetPais('slcLugarN', 1);
            }
        },
        NacionalidadPrepChange() {
            if (parseInt($('#slcNacionalidadPrep').val()) === 1) {
                GlobalFn.GetEstado('slcEstadoPais', 9);
            } else {
                GlobalFn.GetPais('slcEstadoPais', 1);
            }
        },
        Invocar() {
            Usuario = $.cookie('userAdmin');
            if (jQuery.type(Usuario) === "undefined") {
                return false;
            }

            /// Insertar Block al momento de Guardar
            $('#hCarga').text("Guardando...");
            $('#Load').modal('show');
            
            var DTOalumno = {
                Nombre: $('#txtnombre').val(),//0
                Paterno: $('#txtApPaterno').val(),//1
                Materno: $('#txtApMaterno').val(),//2
                Anio: $('#slcPeriodo :selected').data("anio") ,
                PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                UsuarioId: $.cookie('userAdmin'),
                DTOAlumnoDetalle: {
                    GeneroId: $('#slcSexo').val(),
                    EstadoCivilId: $('#slcEstadoCivil').val(),
                    FechaNacimientoC: $('#txtFNacimiento').val(),
                    CURP: $('#txtCURP').val(),
                    PaisId: parseInt($('#slcNacionalidad').val()) === 1 ? 146 : $('#slcLugarN').val(),
                    EntidadFederativaId: $('#slcEstado').val(),
                    EntidadNacimientoId: parseInt($('#slcNacionalidad').val()) === 1 ? $('#slcLugarN').val() : null,
                    MunicipioId: $('#slcMunicipio').val(),
                    Cp: $('#txtCP').val(),
                    Colonia: $('#txtColonia').val(),
                    Calle: $('#txtCalle').val(),
                    NoExterior: $('#txtNumeroE').val(),
                    NoInterior: $('#txtNumeroI').val() == '' ? 'null' : $('#txtNumeroI').val(),
                    TelefonoCasa: $('#txtTelefonoCasa').val(),
                    TelefonoOficina: "",
                    Celular: $('#txtCelular').val(),
                    Email: $('#txtEmail').val(),
                    Observaciones: $('#txtObservaciones').val(),
                    ProspectoO: {
                        PrepaProcedencia : $('#txtNombrePrepa').val() == "" || $('#txtNombrePrepa').val() == " " ? 'null' : $('#txtNombrePrepa').val(),
                        PrepaArea : $('#slcArea').val(),
                        PrepaAnio : $('#txtAñoT').val(),
                        PrepaPromedio : $('#txtPromedio').val() == "" || $('#txtPromedio').val() == " " ? 'null' : $('#txtPromedio').val(),
                        EsEquivalencia : $('#chkUni')[0].checked,
                        UniversidadProcedencia : $('#txtUni').val(),
                        SucursalId : $('#slcPlantel').val(),
                        PrepaMes : ($('#txtMesT').val().length == 1 ? '0' + $('#txtMesT').val() : $('#txtMesT').val()),
                        PrepaPaisId : parseInt($('#slcNacionalidadPrep').val()) == 1 ? 146 : $('#slcEstadoPais').val(),
                        PrepaEntidadId :  parseInt($('#slcNacionalidadPrep').val()) == 1 ? $('#slcEstadoPais').val() : null,
                        UniversidadPaisId :null,
                        UniversidadEntidadId : null,
                        EsTitulado : $('#chkUniSi')[0].checked,
                        UniversidadMotivo : $('#txtUniMotivo').val(),
                        MedioDifusionId : $("#slcMedio").val(),
                    }
                },
                DTOPersonaAutorizada: [{
                    Nombre: $('#txtPAutorizada').val(),//18
                    Paterno: $('#txtAPPaterno').val(),//19
                    Materno: $('#txtAPMaterno').val(),//20
                    Celular: $('#txtTelefonoPA').val(),//21
                    Email: $('#txtPEmail').val(),//22
                    ParentescoId: $('#slcParentesco').val(),//23
                    Autoriza: $('#chkAuotiza1')[0].checked,//46
                    Telefono: $('#txtTelefonoPAT').val() == '' ? 'null' : $('#txtTelefonoPAT').val(),//48
                }],
                AlumnoInscrito: {
                    OfertaEducativaId : $('#slcOfertaEducativa').val(),
                    TurnoId : $('#slcTurno').val(),
                    Anio: $('#slcPeriodo :selected').data("anio"),
                    PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                    EsEmpresa : $('#chkEsEmpresa')[0].checked,//50
                    UsuarioId : $.cookie('userAdmin'),
                }
            }

            if ($('#txtPAutorizada2').val().length > 0) {
                DTOalumno.DTOPersonaAutorizada.push({
                    Autoriza: $('#chkAuotiza2')[0].checked,
                    Celular: $('#txtTelefonoPA2').val(),
                    Email: $('#txtPEmail2').val(),
                    Materno: $('#txtAPMaterno2').val(),
                    Nombre: $('#txtPAutorizada2').val(),
                    ParentescoId: $('#slcParentesco2').val(),
                    Paterno: $('#txtAPPaterno2').val(),
                    Telefono: $('#txtTelefonoPAT2').val()
                });
            }

            IndexFn.Api("Alumno/Nuevo", "Put", JSON.stringify(DTOalumno))
                .done(function (data) {
                    if (!isNaN(data)) {
                        var Alumno = parseInt(data);
                        if (Alumno > 0) {
                            esEmpresa = $('#chkEsEmpresa')[0].checked
                            if (esEmpresa === true) {

                                if (hayPromocion) { InscripcionFn.GuardarPromocion(Alumno); }

                                IndexFn.clearAlert();
                                alertify.alert("El numero del Alumno Inscrito es: " + Alumno, function () {
                                    //$("#btnListado").click();
                                    InscripcionFn.MandarMail(Alumno);
                                });

                            }
                            else {
                                if (parseInt($('#slcTipoOferta').val()) === 4) {
                                    $('#tab5').hide();
                                    InscripcionFn.MandarMail(Alumno);
                                } else {
                                    GlobalFn.GetSistemaPagoAlumno('slcSistemaPago', Alumno)
                                        .done(function (data) {
                                            InscripcionFn.CargarDescuentos(Alumno);
                                            $('#form_wizard_1').bootstrapWizard('next');
                                        });                                    
                                    if (hayPromocion) { InscripcionFn.GuardarPromocion(Alumno); } else { $('#Load').modal('hide'); }
                                }
                            }
                        }
                        else { $('#Load').modal('hide'); return false; }
                    }
                    else {
                        $('#Load').modal('hide');
                        console.log(data.d);
                    }
                })
                .fail(function (data) {
                    IndexFn.clearAlert();
                    $('#Load').modal('hide');
                    alertify.alert("Error al guardar el alumno, intente nuevamente.");                    
                });
        },
        CargarDescuentos(AlumnoId) {
            $('#txtFolio').val(AlumnoId);

            IndexFn.Api("Descuentos/TraerDescuentos/" + AlumnoId, "Get", "")
                .done(function (data) {
                    if (data.length > 0) {
                        var Sispago = $('#slcSistemaPago option:selected').html();
                        var monto;
                        var MaxDes;

                        var Colegiatura = data.find(function (cuota) {
                            return cuota.PagoConceptoId === 800;
                        });
                        var Inscripcion = data.find(function (cuota) {
                            return cuota.PagoConceptoId === 802;
                        });
                        var Examen = data.find(function (cuota) {
                            return cuota.PagoConceptoId === 1;
                        });
                        var Credencial= data.find(function (cuota) {
                            return cuota.PagoConceptoId === 1000;
                        });

                        if (Sispago.search("4") != -1 || Sispago.search("6") != -1) {
                            MaxDes = Colegiatura.Descuento.MontoMaximo;
                            $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                            //$('#txtDescuentoBec').val(MaxDes);
                            $('#txtcuotaCol').text('$' + Colegiatura.Monto);
                            monto = Colegiatura.Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                            monto = Colegiatura.Monto - monto;
                            $('#txtPagarCol').text('$' + String(Math.round(monto)));
                            //$('#txtDescuentoBec').change();
                        } else {
                            MaxDes = Colegiatura.Descuento.MontoMaximo;
                            $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                            //$('#txtDescuentoBec').val(MaxDes);
                            $('#txtcuotaCol').text('$' + (Colegiatura.Monto * 4));
                            monto = (Colegiatura.Monto * 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
                            monto = (Colegiatura.Monto * 4) - monto;
                            $('#txtPagarCol').text('$' + String(Math.round(monto)));
                            //$('#txtDescuentoBec').change();
                        }

                        MaxDes = Inscripcion.Descuento.MontoMaximo;
                        $('#txtDescuentoIns').attr("data-val-max", MaxDes);
                        //$('#txtDescuentoIns').val(MaxDes);
                        $('#txtcuotaIn').text('$' + Inscripcion.Monto);
                        monto = (Inscripcion.Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                        monto = Inscripcion.Monto - monto;
                        $('#txtPagarIn').text('$' + String(Math.round(monto)));
                        //$('#txtDescuentoIns').change();

                        MaxDes = Examen.Descuento.MontoMaximo;
                        $('#txtDescuentoExa').attr("data-val-max", MaxDes);
                        //$('#txtDescuentoExa').val(MaxDes);
                        $('#txtcuotaExa').text('$' + Examen.Monto);
                        monto = (Examen.Monto * (parseFloat($('#txtDescuentoExa').val()) / 100));
                        monto = Examen.Monto - monto;
                        $('#txtPagarExa').text('$' + String(Math.round(monto)));
                        //$('#txtDescuentoExa').change();

                        MaxDes = Credencial.Descuento.MontoMaximo;
                        $('#txtDescuentoCred').attr("data-val-max", MaxDes);
                        //$('#txtDescuentoCred').val(MaxDes);
                        $('#txtcuotaCred').text('$' + Credencial.Monto);
                        monto = (Credencial.Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                        monto = Credencial.Monto - monto;
                        $('#txtPagarCred').text('$' + String(Math.round(monto)));
                        //$('#txtDescuentoCred').change();
                    } else {
                        $('#form_wizard_1').find('.button-guardar').hide();
                        alertify.alert("Error al traer los descuentos");
                    }
                })
                .fail(function (data) {
                    $('#form_wizard_1').find('.button-guardar').hide();
                    alertify.alert("Error al traer los descuentos");
                });
        },
        GuardarDescuentos() {
            if (jQuery.type($.cookie('userAdmin')) === "undefined") {
                return false;
            }

            var ObjAlumno = {
                AlumnoId: $('#txtFolio').val(),
                SistemaPagoId: $('#slcSistemaPago').val(),
                Anio: $('#slcPeriodo :selected').data("anio"),
                PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                OfertaEducativaId: $('#slcOfertaEducativa').val(),
                Observaciones: $('#txtObservacion').val(),
                UsuarioId: $.cookie('userAdmin'),
                Descuentos: [{
                    PagoConceptoId: 1,
                    TotalPagar: Number($('#txtPagarExa').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionExa').val()
                },
                {
                    PagoConceptoId: 800,
                    TotalPagar: Number($('#txtPagarCol').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionBec').val()
                },
                {
                    PagoConceptoId: 802,
                    TotalPagar: Number($('#txtPagarIn').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionIns').val()
                },
                {
                    PagoConceptoId: 1000,
                    TotalPagar: Number($('#txtPagarCred').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionCred').val()
                }]
            };

            var data = new FormData();

            var flIns = $('#BecArchivo'); // FileList object
            flIns = flIns[0].files[0];
            data.append("DocBeca", flIns);

            flIns = $('#InsArchivo');
            flIns = flIns[0].files[0];
            data.append("DocInscipcion", flIns);

            flIns = $('#ExamenArchivo');
            flIns = flIns[0].files[0];
            data.append("DocExamen", flIns);

            data.append("ObjAlumno", JSON.stringify(ObjAlumno));
            IndexFn.clearAlert();

            IndexFn.ApiFile("Descuentos/GuardarDescuentos", data)
                .done(function (data) {
                    if (data != null) {
                        var Alumnos = [$('#txtFolio').val()];
                        IndexFn.Api("General/EnviarMail2", "Post", JSON.stringify(Alumnos))
                            .done(function (data) {
                                $('#Load').modal('hide');
                                jQuery('li', $('#form_wizard_1')).removeClass("done");
                                jQuery('li', $('#form_wizard_1')).addClass("done");

                                var extramail = "<p>" + "Se ha enviado un mail a " + $('#txtEmail').val() + " con el usuario y password del alumno."
                                    + "Si no puede visualizarlo en su bandeja de entrada en los próximos 15 minutos;  que lo busque en su carpeta de elementos no deseados." +
                                    "Si lo encuentra ahí, por favor que lo marque como 'No Spam'." + "</p>";

                                alertify.alert("Alumno Guardado </br> " + extramail, function () {
                                    var url = "Views/Alumno/Credenciales.aspx?AlumnoId=" + $('#txtFolio').val() + "&OfertaEducativaId=" + $('#slcOfertaEducativa').val();
                                    window.open(url, "Credenciales");
                                    $(location).attr('href', '#Views/1');
                                });
                            })
                            .fail(function (data) {
                                console.log(data);
                                InscripcionFn.MandarMail($('#txtFolio').val());
                            });

                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("No se guardaron los cambios, intente de nuevo");
                    }
                })
                .fail(function (data) {
                    $('#Load').modal('hide');
                    alertify.alert("No se guardaron los cambios, intente de nuevo");
                    console.log(data);
                });

        },
        MandarMail(AlumnoId) {
            var Alumnos = [AlumnoId];
            IndexFn.Api("General/EnviarMail2", "Post", JSON.stringify(Alumnos))
                .done(function (data) {
                    $('#Load').modal('hide');
                    jQuery('li', $('#form_wizard_1')).removeClass("done");
                    jQuery('li', $('#form_wizard_1')).addClass("done");
                    var extramail = "<p>" + "Se ha enviado un mail a " + $('#txtEmail').val() + " con el usuario y password del alumno."
                        + "Si no puede visualizarlo en su bandeja de entrada en los próximos 15 minutos;  que lo busque en su carpeta de elementos no deseados." +
                        "Si lo encuentra ahí, por favor que lo marque como 'No Spam'." + "</p>";
                    IndexFn.clearAlert();
                    alertify.alert(extramail, function () {
                        $(location).attr('href', '#Views/1');
                    });
                })
                .fail(function (data) {
                    console.log(data);
                    InscripcionFn.MandarMail(AlumnoId);
                });
        },
        GuardarPromocion(AlumnoProspecto) {

            dataAlumno[0].AlumnoIdProspecto = AlumnoProspecto;
            dataAlumno[0].Anio = $('#slcPeriodo').val().substring(2);
            dataAlumno[0].PeriodoId = $('#slcPeriodo').val().substring(0, 1);
            dataAlumno[0].UsuarioId = $.cookie('userAdmin');

            var obj = {
                "Promocion": dataAlumno[0]
            };
            obj = JSON.stringify(obj);

            IndexFn.Api("Alumno/GuardarPromocionCasa", "post", obj)
                .done(function (data) {
                    console.log("Se guardo la promoción en casa.");
                })
                .fail(function (data) {
                    console.log("Fallo.")
                });

        },
        slcOfertaEducativaChange() {
            $('#slcPeriodo').change();
        },
        slcPeriodoChange() {
            if (($('#slcPeriodo').val() == "" || $('#slcOfertaEducativa').val() == "")
            || ($('#slcPeriodo').val() == undefined || $('#slcOfertaEducativa').val() == undefined)){ return false; }
            var param = $('#slcOfertaEducativa').val() + "/" +
                $('#slcPeriodo :selected').data("anio") + "/" +
                $('#slcPeriodo :selected').data("periodoid");

            IndexFn.Api("Cuota/TraerCuotaPeriodo/" + param, "GET", "")
                .done(function (result) {
                    if (result.length === 0) {
                        alertify.alert("Error: El periodo seleccionado aun no tiene cuotas generadas, favor de comunicarse al área de Sistemas");
                        $('#slcPeriodo').val($("#slcPeriodo option:first").val());
                    }
                })
                .fail(function (data) {
                    alertify.alert("Error: El periodo seleccionado aun no tiene cuotas generadas, favor de comunicarse al área de Sistemas");
                    $('#slcPeriodo').val($("#slcPeriodo option:first").val());
                });
        },
        chkYoClick() {

            if ($(this).is(':checked')) {
                $('#txtPAutorizada2').val($('#txtnombre').val());
                $('#txtAPPaterno2').val($('#txtApPaterno').val());
                $('#txtAPMaterno2').val($('#txtApMaterno').val());
                $('#txtTelefonoPA2').val($('#txtCelular').val());
                $('#txtPEmail2').val($('#txtEmail').val());
                $('#txtTelefonoPAT2').val($('#txtTelefonoCasa').val());
                $('#slcParentesco2').val('7');
                $('#txtPAutorizada2').attr('readonly', true);
                $('#txtAPPaterno2').attr('readonly', true);
                $('#txtAPMaterno2').attr('readonly', true);
                $('#txtTelefonoPA2').attr('readonly', true);
                $('#txtPEmail2').attr('readonly', true);
                $('#txtTelefonoPAT2').attr('readonly', true);
                $('#slcParentesco2').attr('disabled', true);
            } else {
                $('#txtPAutorizada2').val('');
                $('#txtAPPaterno2').val('');
                $('#txtAPMaterno2').val('');
                $('#txtTelefonoPA2').val('');
                $('#txtPEmail2').val('');
                $('#txtTelefonoPAT2').val('');
                $('#slcParentesco2').val('-1');
                $('#txtPAutorizada2').attr('readonly', false);
                $('#txtAPPaterno2').attr('readonly', false);
                $('#txtAPMaterno2').attr('readonly', false);
                $('#txtTelefonoPA2').attr('readonly', false);
                $('#txtPEmail2').attr('readonly', false);
                $('#txtTelefonoPAT2').attr('readonly', false);
                $('#slcParentesco2').attr('disabled', false);
            }
        },
        slcTurnoChange() {
            if ($(this).val() == 5)
                $('#chkEsEmpresa').iCheck('check');
        },
        btnSiClick() {
            $("#ModalEsEmpresa").modal("hide");
            alertify.confirm("<p>¿Esta seguro que desea guardar los cambios?<br><br><hr>", function (e) {
                if (e) {
                    InscripcionFn.Invocar();
                }
            });
        },
        btnNoClick() {
            $("#chkEsEmpresa").focus();
        },
        btnPromocionClick() {
            $("#PopAlumnoPromocion").modal('show');
        },
        dtAlumno1ClickA() {
            $("#btnPromocion").text("Modificar");
            $("#lbAlumnoPromocion").text(dataAlumno[0].AlumnoId + " | " + dataAlumno[0].NombreC);
            $("#divAlumnoPormocion").show();
            hayPromocion = true;
            $("#PopAlumnoPromocion").modal('hide');
        },
        btnClosePromoClick() {
            $("#txtClave1").val("");
            if (tblAlumno2 != undefined) {
                tblAlumno2.fnClearTable();
            }
            $("#btnPromocion").text("Agregar");
            hayPromocion = false;
            $("#divAlumnoPormocion").hide();
        },
        txtClave1KeyDown(e) {
            if (e.which == 13) {
                $('#btnBuscar1').click();
            }
        },
        btnBuscar1Click() {

            AlumnoPromocion = $('#txtClave1').val();
            if (AlumnoPromocion.length == 0) { return false; }
            if (tblAlumno2 != undefined) {
                tblAlumno2.fnClearTable();
            }
            $('#hCarga').text("Cargando...");
            $('#Load').modal('show');

            IndexFn.Api("Alumno/ConsultarAlumnoPromocionCasa2/" + $('#txtClave1').val(), "GET", "")
                .done(function (data) {
                    if (data === null) {
                        $('#Load').modal('hide');
                        return false;
                    }
                    dataAlumno.length = 0;
                    dataAlumno.push(data);

                    tblAlumno2 = $("#dtAlumno1").dataTable({
                        "aaData": dataAlumno,
                        "aoColumns": [
                            {
                                "mDataProp": "AlumnoId",
                                "mRender": function (data, f, d) {
                                    var link;
                                    link = d.AlumnoId + " | " + d.NombreC;

                                    return link;
                                }
                            },
                            { "mDataProp": "OfertaEducativaActual" },
                            {
                                "mDataProp": function (data) {

                                    return "<a class='btn blue' name ='btnAgregar'>Agregar</a>";

                                }
                            }
                        ],
                        "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
                        "searching": false,
                        "ordering": false,
                        "async": true,
                        "bDestroy": true,
                        "bPaginate": false,
                        "bLengthChange": false,
                        "bFilter": false,
                        "bInfo": false,
                        "bAutoWidth": false,
                        "asStripClasses": null,
                        "colReorder": true,
                        "language": {
                            "lengthMenu": "_MENU_ Registro",
                            "paginate": {
                                "previos": "<",
                                "next": ">"
                            },
                            "search": "Buscar Alumno ",
                        },
                        "order": [[1, "desc"]],
                        "createdRow": function (row, data, dataIndex) {
                            row.childNodes[0].style.textAlign = 'center';
                            row.childNodes[1].style.textAlign = 'center';
                            row.childNodes[2].style.textAlign = 'center';
                        }
                    });//$('#dtbecas').DataTable

                    $('#Load').modal('hide');
                })
                .fail(function (data) {
                    $('#Load').modal('hide');
                    return false;
                });
        },
        Inscripcion() {
            $('#DescuentoI').noUiSlider({
                start: 50,
                step: 5,
                range: {
                    min: 0,
                    max: 100
                }
            });

            $('#DescuentoI').noUiSlider_pips({
                mode: 'values',
                values: [20, 80],
                density: 5
            });

            $('#DescuentoI').on('set', function (event, value) {
                if (value < 20) {
                    $(this).val(20);
                } else if (value > 80) {
                    $(this).val(80);
                }
                $('#valIn').text($(this).val() + '%');
            });

        },
        Beca() {
            $('#DescuentoB').noUiSlider({
                start: 50,
                step: 5,
                range: {
                    min: 0,
                    max: 100
                }
            });

            $('#DescuentoB').noUiSlider_pips({
                mode: 'values',
                values: [20, 80],
                density: 5
            });

            $('#DescuentoB').on('set', function (event, value) {
                if (value < 20) {
                    $(this).val(20);
                } else if (value > 80) {
                    $(this).val(80);
                }
                $('#valBe').text($(this).val() + '%');

            });
            jQuery('#pulsate-regular').pulsate({
                color: "#bf1c56"
            });
            jQuery("#dvCargosEf").pulsate({
                color: "#bf1c56"
            });
        },
        SetFecha() {
            var Now = new Date();
            var años = Now.getFullYear() - 18;
            var mes = Now.getMonth() + 1;
            var Fecha = Now.getDate() + '-' + mes + '-' + años;

            if (jQuery().datepicker) {
                $('.date-picker').datepicker({
                    rtl: Metronic.isRTL(),
                    orientation: "left",
                    autoclose: true,
                    language: 'es'
                });
                $(".date-picker").datepicker("setDate", Fecha);
                $('#spnA').text(InscripcionFn.calcular_edad(Fecha));

            }
            $('#txtFNacimiento').change(function () {
                var cumple = $('#txtFNacimiento').val();
                var a = InscripcionFn.calcular_edad(cumple);
                $('#spnA').text(a);
            });


            /* Workaround to restrict daterange past date select: http://stackoverflow.com/questions/11933173/how-to-restrict-the-selectable-date-ranges-in-bootstrap-datepicker */
        },
        calcular_edad(fecha) {
            var fechaActual = new Date()
            var diaActual = fechaActual.getDate();
            var mmActual = fechaActual.getMonth() + 1;
            var yyyyActual = fechaActual.getFullYear();
            FechaNac = fecha.split("-");
            var diaCumple = FechaNac[0];
            var mmCumple = FechaNac[1];
            var yyyyCumple = FechaNac[2];

            var edad = yyyyActual - yyyyCumple;
            var meses = mmActual - mmCumple;

            if (meses < 0) {
                edad = edad - 1;
                meses = 12 + meses;
            }

            return edad + ' Años, ' + meses + ' meses';
        },
        Buscar() {
            var alumno = {
                Nombre: $('#txtnombre').val(),
                Paterno: $('#txtApPaterno').val(),
                Materno: $('#txtApMaterno').val(),
            };

            if ((alumno.Paterno == "" || alumno.Paterno == " ") && (alumno.Materno == "" || alumno.Materno == " ")) {
                $('#pulsate-regular').attr("hidden", "hidden");
                return false;
            }

            IndexFn.Api("Alumno/BuscarAlumno", "Post", JSON.stringify(alumno))
                .done(function (Respuesta) {
                    if (Respuesta.length > 0) {
                        MItable = $('#Alumnos').dataTable({
                            "aaData": Respuesta,
                            "aoColumns": [
                                { "mDataProp": "AlumnoId", "Nombre": "AlumnoId", visible: false },
                                {
                                    "mDataProp": "Nombre",
                                    "mRender": function (data) {
                                        return "<a href=''onclick='return false;'>" + data + " </a> ";
                                    }
                                },
                                { "mDataProp": "DTOAlumnoDetalle.FechaNacimientoC" },
                                { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                //{ "mDataProp": "FechaSeguimiento" },
                                { "mDataProp": "Usuario.Nombre" }
                            ],
                            "searching": false,
                            "ordering": false,
                            "info": false,
                            "async": true,
                            "bDestroy": true

                        });
                        $('#pulsate-regular').removeAttr("hidden", "hidden");
                    }
                    else { $('#pulsate-regular').attr("hidden", "hidden"); }
                })
                .fail(function (data) {
                    alertify.alert('Error al cargar datos');
                });
        },
        CalcularDescuento(Monto, Descuento) {
            var Redondeado;
            Monto = parseFloat(Monto);
            Descuento = (Monto * (Descuento / 100))
            Redondeado = Monto - Descuento;
            Redondeado = Math.round(Redondeado);
            return Redondeado;
        },
        GetMontos() {
            var idlabel = $('#' + this.id).data('label');
            var idlabelP = $('#' + this.id).data('labelp');

            var Maximo = $('#' + this.id).data();
            if (Maximo.valMax < $('#' + this.id).val()) { $('#' + this.id).val(Maximo.valMax); return false; }
            var monto = InscripcionFn.CalcularDescuento($('#' + idlabel).text().replace('$', ''), $('#' + this.id).val());
            $('#' + idlabelP).text('$' + String(monto));
        },
        GetMontosKnob(val, idl) {
            var idlabel = $('#' + idl).data('label');
            var idlabelP = $('#' + idl).data('labelp');

            var Maximo = $('#' + idl).data();
            if (Maximo.valMax < val) { $('#' + idl).val(Maximo.valMax); return false; }
            var monto = InscripcionFn.CalcularDescuento($('#' + idlabel).text().replace('$', ''), val);
            $('#' + idlabelP).text('$' + String(monto));
        },
    };

    InscripcionFn.init();

});
