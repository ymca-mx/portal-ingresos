var Validacion = function () {
    var Usuario, esEmpresa;
    var b_Valid = 0;
    //$('#Load').modal('show');
    //$.cookie('userAdmin', 6883, { expires: 1 });
    var FormWizard = function () {
        $('#slcNacionalidad').val('1');
        $('#slcNacionalidad').change();
        $('#divMaterial').hide();
        $('#divTablaDescuento').hide();
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
                slcOferta: {
                    required: true,
                    min: 1
                },
                slcCarrera: {
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
                $('#btnListado').hide();;
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

        }

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
                    if ($('#slcOferta').val() == '4') {
                        GuardarIngles();
                    } else {
                        GuardarDescuentos();
                    }
                } else { return false; }
            });
            //AbrirDocumento();
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
                            $("#ModalEsEmpresa").modal("show"); return false;
                        } else
                        {
                            alertify.confirm("<p>¿Esta seguro que desea guardar los cambios?<br><br><hr>", function (e) {
                                if (e) {
                                    Invocar();
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

        
    }

    function Invocar() {
        Usuario = $.cookie('userAdmin');
        if (jQuery.type(Usuario) === "undefined") {
            return false;
        }

        /// Insertar Block al momento de Guardar
        $('#hCarga').text("Guardando...");
        $('#Load').modal('show');

        var mesanio = $('#txtAñoT').val();
        mesanio = mesanio + '-' + ($('#txtMesT').val().length == 1 ? '0' + $('#txtMesT').val() : $('#txtMesT').val());
        //var Lista1="{"
        var lista = {
            Nombre: $('#txtnombre').val(),//0
            Paterno: $('#txtApPaterno').val(),//1
            Materno: $('#txtApMaterno').val(),//2

            Sexo: $('#slcSexo').val(),//3
            EstadoCivil: $('#slcEstadoCivil').val(),//4
            FNacimiento: $('#txtFNacimiento').val(),//5
            Curp: $('#txtCURP').val(),//6
            Pais: $('#slcLugarN').val(),//7----->Pais|Estado
            Entidad: $('#slcEstado').val(),//8
            Delegacion: $('#slcMunicipio').val(),//9
            CP: $('#txtCP').val(),//10
            Colonia: $('#txtColonia').val(),//11
            Calle: $('#txtCalle').val(),//12
            NoExterior: $('#txtNumeroE').val(),//13
            NoInterior: $('#txtNumeroI').val() == '' ? 'null' : $('#txtNumeroI').val(),//14
            TCasa: $('#txtTelefonoCasa').val(),//15
            TCelular: $('#txtCelular').val(),//16
            Email: $('#txtEmail').val(),//17

            PANombre: $('#txtPAutorizada').val(),//18
            PAPaterno: $('#txtAPPaterno').val(),//19
            PAMaterno: $('#txtAPMaterno').val(),//20
            PATCelular: $('#txtTelefonoPA').val(),//21
            PAEmail: $('#txtPEmail').val(),//22
            Parentesco: $('#slcParentesco').val(),//23

            OfertaEducativa: $('#slcCarrera').val(),//24
            Turno: $('#slcTurno').val(),//25
            Periodo: $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html(),//26
            Preparatoria: $('#txtNombrePrepa').val() == "" || $('#txtNombrePrepa').val() == " " ? 'null' : $('#txtNombrePrepa').val(),//27
            Area: $('#slcArea').val(),//28
            AñoPrepa: mesanio,//29
            Promedio: $('#txtPromedio').val() == "" || $('#txtPromedio').val() == " " ? 'null' : $('#txtPromedio').val(), //30
            Universidad: $('#chkUni')[0].checked == false ? 'null' : $('#txtUni').val(),//31
            Plantel: $('#slcPlantel').val(),//32            

            //Persona Aurtorizada 2
            PANombre2: $('#txtPAutorizada2').val() == '' ? 'null' : $('#txtPAutorizada2').val(),//33
            PAPaterno2: $('#txtAPPaterno2').val() == '' ? 'null' : $('#txtAPPaterno2').val(),//34
            PAMaterno2: $('#txtAPMaterno2').val() == '' ? 'null' : $('#txtAPMaterno2').val(),//35
            PATCelular2: $('#txtTelefonoPA2').val() == '' ? 'null' : $('#txtTelefonoPA2').val(),//36
            PAEmail2: $('#txtPEmail2').val() == '' ? 'null' : $('#txtPEmail2').val(),//37
            Parentesco2: $('#slcParentesco2').val() == '- 1' ? 'null' : $('#slcParentesco2').val(),//38

            //Nacionalidad Persona
            Nacionalidad: $('#slcNacionalidad').val(), //39 NAcionalidad

            //Pais|Nacionalidad Prepa
            NacionalidadPre: $('#slcNacionalidadPrep').val() == '- 1' ? 'null' : $('#slcNacionalidadPrep').val(),//40 NAcionalidad PRepa
            PaisEstadoPre: $('#slcEstadoPais').val() == '- 1' ? 'null' : $('#slcEstadoPais').val(),//41 PAis Estado Prepa

            //Pais|Nacionalidad Prepa
            NacionalidadUni: 'null',//42NAcionalidad Uni
            PaisEstadoUni: 'null',//43PAis Estado Uni
            Titulado: $('#chkUniSi')[0].checked,//44
            Motivo: $('#chkUniSi')[0].checked == true ? $('#txtUniMotivo').val() : 'null',//45

            //Check de autorizacion
            Autoriza1: $('#chkAuotiza1')[0].checked,//46
            Autoriza2: $('#chkAuotiza2')[0].checked,//47
            TelefonoCasaP: $('#txtTelefonoPAT').val() == '' ? 'null' : $('#txtTelefonoPAT').val(),//48
            TelefonoCasaP2: $('#txtTelefonoPAT2').val() == '' ? 'null' : $('#txtTelefonoPAT2').val(),//49
            EsEmpresa: $('#chkEsEmpresa')[0].checked,//50
            MedioDifusion: $("#slcMedio").val(),//51
            Usuario: Usuario //52
        }

        var objetos = JSON.stringify(lista);

        var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/InsertarAlumno",
            data: objetos, // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            datatype: JSON,
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (!isNaN(data.d)) {
                    var Alumno = parseInt(data.d);
                    if (Alumno > 0) {
                        esEmpresa = $('#chkEsEmpresa')[0].checked
                        if (esEmpresa === true) {

                            if (hayPromocion) { GuardarPromocion(Alumno); }


                            alertify.alert("El numero del Alumno Inscrito es: " + Alumno, function () {
                                //$("#btnListado").click();
                                MandarEMail(Alumno);
                            });

                        }
                        else {
                            if ($('#slcOferta').val() == '4') {
                                $('#tab5').hide();
                                //    GuardarIngles(data.d);
                                MandarEMail(Alumno);
                            } else {
                                $('#divTablaDescuento').hide();
                                CargarTiposPagos(data.d);
                                CargarDescuentos(data.d);
                                $('#form_wizard_1').bootstrapWizard('next');
                                if (hayPromocion) { GuardarPromocion(Alumno); } else { $('#Load').modal('hide'); }
                            }
                        }
                    }
                    else { $('#Load').modal('hide'); return false; }
                }
                else {
                    $('#Load').modal('hide');
                    console.log(data.d);
                }
            }
        });

    }

    function CrearTabla(Periodo) {
        var th;
        var num;
        var fila = '<tr id="tr1">';
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/PeriodosCompletos",
            data: "{Periodo:'" + Periodo + "',ofertaId:'" + $("#slcCarrera").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                var row = document.getElementById("tr1");
                if (row != null) {
                    padre = row.parentNode;
                    padre.removeChild(row);
                }
                var meses = data.d.lstSubPeriodo.length;
                for (i = 0; i < meses; i++) {
                    num = i + 1;
                    th = '#thm' + num;
                    $(th).html('<i class="fa fa-calendar"></i>&nbsp;' + data.d.lstSubPeriodo[i].Mes.Descripcion);
                    if (data.d.lstSubPeriodo[i].Mes.MontoLengua != null) {
                        //MesP[i] = data.d.lstSubPeriodo[i].Mes.MontoLengua.Cuota.Monto;
                        fila += '<td id="' + 'mes' + i + '">' + '$' + data.d.lstSubPeriodo[i].Mes.MontoLengua.Cuota.Monto + '</td>';
                    } else {
                        //MesP[i] = 0.00;
                        fila += '<td id="' + 'mes' + i + '">$0.00</td>';
                    }
                }
                fila += '</tr>';
                $('#tblDescuentos').append(fila);
                $('#divTablaDescuento').show();
                //$('#txtDescuentoBec').keyup();
            }
        });
    }

    function CargarTiposPagos(AlumnoId) {
        var n4;
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarPagosPlan",
            data: "{AlumnoId:'" + AlumnoId + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    if (this.Pagos == 4) { n4 = this.PagoPlanId; }
                    option.text(this.PlanPago);
                    option.val(this.PagoPlanId);

                    $("#slcSistemaPago").append(option);
                });

                $("#slcSistemaPago").val(data.d.length > 1 ? n4 : data.d[0].PagoPlanId);
            }
        });
    }

    $('#slcCarrera').change(function () {
        $('#slcPeriodo').change();
    });

    $('#slcPeriodo').change(function () {
        if ($('#slcPeriodo').val() == -1 || $('#slcCarrera').val() == -1)
        { return false; }
        var Idioma = $('#slcCarrera').val();
        var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();

        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/ConsultarCuotaOfertaEducativaPeriodo",
            data: "{OfertaEducativaId:'" + Idioma + "',Periodo:'" + Periodo + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var bResp = data.d;
                if (bResp.length < 1) {
                    alertify.alert("Error: El periodo seleccionado aun no tiene cuotas generadas, favor de comunicarse al área de Sistemas");
                    $('#slcPeriodo').val($("#slcPeriodo option:first").val());
                }
            }
        });
    })

    function CargarDescuentos(AlumnoId) {
        $('#txtFolio').val(AlumnoId);
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/TraerDescuentos",
            data: "{'AlumnoId':" + AlumnoId + "}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d.length > 0) {
                    var Sispago = $('#slcSistemaPago option:selected').html();
                    var monto;
                    var MaxDes;
                    if (Sispago.search("4") != -1 || Sispago.search("6") != -1) {
                        MaxDes = data.d[1].Descuento.MontoMaximo;
                        $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                        //$('#txtDescuentoBec').val(MaxDes);
                        $('#txtcuotaCol').text('$' + data.d[1].Monto);
                        monto = data.d[1].Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                        monto = data.d[1].Monto - monto;
                        $('#txtPagarCol').text('$' + String(monto));
                        //$('#txtDescuentoBec').change();
                    } else {
                        MaxDes = data.d[1].Descuento.MontoMaximo;
                        $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                        //$('#txtDescuentoBec').val(MaxDes);
                        $('#txtcuotaCol').text('$' + (data.d[1].Monto * 4));
                        monto = (data.d[1].Monto * 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
                        monto = (data.d[1].Monto * 4) - monto;
                        $('#txtPagarCol').text('$' + String(monto));
                        //$('#txtDescuentoBec').change();
                    }

                    MaxDes = data.d[0].Descuento.MontoMaximo;
                    $('#txtDescuentoIns').attr("data-val-max", MaxDes);
                    //$('#txtDescuentoIns').val(MaxDes);
                    $('#txtcuotaIn').text('$' + data.d[0].Monto);
                    monto = (data.d[0].Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                    monto = data.d[0].Monto - monto;
                    $('#txtPagarIn').text('$' + String(monto));
                    //$('#txtDescuentoIns').change();

                    MaxDes = data.d[2].Descuento.MontoMaximo;
                    $('#txtDescuentoExa').attr("data-val-max", MaxDes);
                    //$('#txtDescuentoExa').val(MaxDes);
                    $('#txtcuotaExa').text('$' + data.d[2].Monto);
                    monto = (data.d[2].Monto * (parseFloat($('#txtDescuentoExa').val()) / 100));
                    monto = data.d[2].Monto - monto;
                    $('#txtPagarExa').text('$' + String(monto));
                    //$('#txtDescuentoExa').change();

                    MaxDes = data.d[3].Descuento.MontoMaximo;
                    $('#txtDescuentoCred').attr("data-val-max", MaxDes);
                    //$('#txtDescuentoCred').val(MaxDes);
                    $('#txtcuotaCred').text('$' + data.d[3].Monto);
                    monto = (data.d[3].Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                    monto = data.d[3].Monto - monto;
                    $('#txtPagarCred').text('$' + String(monto));
                    //$('#txtDescuentoCred').change();
                } else {
                    $('#form_wizard_1').find('.button-guardar').hide();
                    alertify.alert("Error al traer los descuentos");
                }
            }
        });
    }

    function GuardarDescuentos() {
        Usuario = $.cookie('userAdmin');
        if (jQuery.type(Usuario) === "undefined") {
            return false;
        }
        var lsDatos = {
            'AlumnoId': $('#txtFolio').val(),//0
            'DescuentoIns': $('#txtDescuentoIns').val(),//1
            'JustificacionIns': $('#txtJustificacionIns').val(),//2        
            'DescuentoBec': $('#txtDescuentoBec').val(),//3
            'JustificacionBec': $('#txtJustificacionBec').val(),//4
            'Observacion': $('#txtObservacion').val(),//5
            'SistemaPago': $('#slcSistemaPago').val(),//6
            'DescuentoExamen': $('#txtDescuentoExa').val(), //Descuento Examen 7
            'JustificacionExam': $('#txtJustificacionExa').val(), //Comentario Examen 8
            'Credencial': $('#txtDescuentoCred').val(),//9
            'JustificacionCred': $('#txtJustificacionCred').val(),//10
            'Usuario': Usuario //11
        };

        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/GuardarDescuentos",
            data: JSON.stringify(lsDatos), // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            datatype: JSON,
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d != null) {
                    GuardarDocumentos(data.d[1], data.d[0], data.d[2]);
                    MandarMail($('#txtFolio').val());
                } else {
                    $('#Load').modal('hide');
                    alertify.alert("No se guardaron los cambios, intente de nuevo");
                }
            }
        });
    }

    function MandarMail2(AlumnoId) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/EnviarMail2",
            data: "{listaID:'" + AlumnoId + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d.length > 1) {
                    MandarMail(Alumnoid);
                } else {
                    GuardarDocumentoIngles($('#txtFolio').val(), $('#slcCarrera').val());
                }
            }
        });
    }

    function MandarEMail(alumnoid) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/EnviarMail2",
            data: "{listaID:'" + alumnoid + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d.length > 1) {
                    MandarMail(alumnoid);
                } else {
                    $('#Load').modal('hide');
                    var extramail = "<p>" + "Se ha enviado un mail a " + $('#txtEmail').val() + " con el usuario y password del alumno."
                        + "Si no puede visualizarlo en su bandeja de entrada en los próximos 15 minutos;  que lo busque en su carpeta de elementos no deseados." +
                        "Si lo encuentra ahí, por favor que lo marque como 'No Spam'." + "</p>";
                    alertify.alert(extramail, function () {
                        $(location).attr('href', '#Views/1');
                    });
                }
            }
        });
    }

    function MandarMail(Alumnoid) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/EnviarMail2",
            data: "{listaID:'" + Alumnoid + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d.length > 1) {
                    MandarMail(Alumnoid);
                } else {
                    jQuery('li', $('#form_wizard_1')).removeClass("done");
                    jQuery('li', $('#form_wizard_1')).addClass("done");
                    if (esEmpresa) {
                        $('#Load').modal('hide');
                        var extramail = "<p>" + "Se ha enviado un mail a " + $('#txtEmail').val() + " con el usuario y password del alumno."
                            + "Si no puede visualizarlo en su bandeja de entrada en los próximos 15 minutos;  que lo busque en su carpeta de elementos no deseados." +
                            "Si lo encuentra ahí, por favor que lo marque como 'No Spam'." + "</p>";
                        alertify.alert(extramail, function () {
                            $(location).attr('href', '#Views/1');
                        });
                    }
                }
            }
        });
    }

    function GuardarDocumentoIngles(AlumnoId, OfertaEducativa) {
        var extramail = "<p>" + "Se ha enviado un mail a " + $('#txtEmail').val() + " con el usuario y password del alumno."
            + "Si no puede visualizarlo en su bandeja de entrada en los próximos 15 minutos;  que lo busque en su carpeta de elementos no deseados." +
            "Si lo encuentra ahí, por favor que lo marque como 'No Spam'." + "</p>";

        var data = new FormData();
        var flIns = $('#BecArchivo');

        flIns = flIns[0].files[0];
        data.append("DocBeca", flIns);
        data.append("AlumnoId", AlumnoId);
        data.append("OfertaEducativaId", OfertaEducativa);
        var request = new XMLHttpRequest();
        request.open("POST", 'WS/Descuentos.asmx/GuardarDocumentosIngles', true);
        request.send(data);
        $('#Load').modal('hide');
        alertify.alert("Alumno Guardado </br> " + extramail, function (Respuesta) {
            var url = "Views/Alumno/Credenciales.aspx?AlumnoId=" + $('#txtFolio').val() + "&OfertaEducativaId=" + $('#slcCarrera').val();
            window.open(url, "Credenciales");
            $(location).attr('href', '#Views/1');
        });
    }

    function GuardarDocumentos(Beca, Insc, Exam) {
        var extramail = "<p>" + "Se ha enviado un mail a " + $('#txtEmail').val() + " con el usuario y password del alumno."
            + "Si no puede visualizarlo en su bandeja de entrada en los próximos 15 minutos;  que lo busque en su carpeta de elementos no deseados." +
            "Si lo encuentra ahí, por favor que lo marque como 'No Spam'." + "</p>";
        var data = new FormData();
        var flIns = $('#BecArchivo'); // FileList object

        flIns = flIns[0].files[0];
        data.append("DocBeca", flIns);
        data.append("DescuentoIdB", Beca);
        flIns = $('#InsArchivo');
        flIns = flIns[0].files[0];
        data.append("DocInscipcion", flIns);
        data.append("DescuentoIdI", Insc);
        flIns = $('#ExamenArchivo');
        flIns = flIns[0].files[0];
        data.append("DocExamen", flIns);
        data.append("DescuentoExam", Exam);


        var request = new XMLHttpRequest();
        request.open("POST", 'WS/Descuentos.asmx/GuardarDocumentos', true);
        request.send(data);
        $('#Load').modal('hide');
        alertify.alert("Alumno Guardado </br> " + extramail, function (Respuesta) {
            var url = "Views/Alumno/Credenciales.aspx?AlumnoId=" + $('#txtFolio').val() + "&OfertaEducativaId=" + $('#slcCarrera').val();
            window.open(url, "Credenciales");
            $(location).attr('href', '#Views/1');
        });
    }

    function GuardarIngles() {
        Usuario = $.cookie('userAdmin');
        if (jQuery.type(Usuario) === "undefined") {
            return false;
        }
        var Campos = {
            'AlumnoId': $('#txtFolio').val(),
            'OfertaEducativa': $('#slcCarrera').val(),
            'Turno': $('#slcTurno').val(),
            'Periodo': $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html(),
            'SistemaPago': $('#slcSistemaPago').val(),
            'DescuentoBec': $('#txtDescuentoBec').val(),
            'JustificacionBec': $('#txtJustificacionBec').val() == '' ? 'null' : $('#txtJustificacionBec').val(),
            'Credencial': $('#txtDescuentoCred').val(),
            'JustificacionCred': $('#txtJustificacionCred').val() == '' ? 'null' : $('#txtJustificacionCred').val(),
            'Material': $('#chkMaterial')[0].checked,
            'EsEmpresa': $('#chkEsEmpresa')[0].checked,
            'DescuentoExamen': null,
            'JustificacionExam': null,
            'DescuentoIns': null,
            'JustificacionIns': null,
            'Usuario': Usuario
        };
        alertify.confirm("¿Esta seguro que desea guardar los cambios?", function (Respuesta) {
            if (Respuesta == true) {
                $.ajax({
                    type: "POST",
                    url: "WS/Descuentos.asmx/GuardarIdioma",
                    data: JSON.stringify(Campos), // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    datatype: JSON,
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        if (data.d[0] == "Guardado") {
                            TemEmail($('#txtFolio').val());
                            //MandarMail2($('#txtFolio').val());
                        }
                        else {
                            alertify.alert("Error no se guardaron los cambios, intente de nuevo", function () {
                                return false;
                            });

                        }
                    }
                });
            }
        });//
    }

    function TemEmail(AlumnoId) {
        $.ajax({
            type: "POST",
            url: "WS/Sexo.asmx/EnviarMail",
            data: "{Alumnos:'" + AlumnoId + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            datatype: JSON,
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                GuardarDocumentoIngles($('#txtFolio').val(), $('#slcCarrera').val());
            }
        });
    }

    function CargarDescuentosIdiomas(Idioma) {
        var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/TraerDescuentosIdiomas",
            data: "{'Idioma':" + Idioma + ",Periodo:'" + Periodo + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var monto;
                var MaxDes;
                MaxDes = data.d[0].Descuento.MontoMaximo;
                $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                $('#txtcuotaCol').text('$' + data.d[0].Monto);
                //$('#txtDescuentoBec').val(MaxDes);
                monto = data.d[0].Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                monto = data.d[0].Monto - monto;
                $('#txtPagarCol').text('$' + String(monto));
                //$('#txtDescuentoBec').change()

                MaxDes = data.d[1].Descuento.MontoMaximo;
                $('#txtDescuentoCred').attr("data-val-max", MaxDes);
                //$('#txtDescuentoCred').val(MaxDes);
                $('#txtcuotaCred').text('$' + data.d[1].Monto);
                monto = (data.d[1].Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                monto = data.d[1].Monto - monto;
                $('#txtPagarCred').text('$' + String(monto));
                //$('#txtDescuentoCred').change();
            }
        });
    }

    function AbrirDocumento() {
        var archivo = new ArrayBuffer();
        $.ajax({
            type: "POST",
            processData: false,
            url: "WS/Descuentos.asmx/TraerDocumento",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            contentType: "application/pdf; charset=utf-8",
            success: function (data) {
                var newdata = "data:" + "application/pdf" + ";base64," + escape(data.documentElement.textContent);
                //To open in new window
                window.open(newdata, "_blank");
            }
        });
    }
    

    $('#chkYo').click(function () {
        
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
    });

    $("#slcTurno").on('change', function () {
        if ($(this).val() == 5)
            $('#chkEsEmpresa').iCheck('check'); 
    });
    
    $('#chkEsEmpresa').iCheck({
        checkboxClass: 'icheckbox_square-grey',
        radioClass: 'iradio_square-grey',
        increaseArea: '20%' // optional
    });

    $("#btnSi").click(function () {
        $("#ModalEsEmpresa").modal("hide");
        alertify.confirm("<p>¿Esta seguro que desea guardar los cambios?<br><br><hr>", function (e) {
            if (e) {
                Invocar();
                //$('#form_wizard_1').find('.button-submit').click();
            }
        });
    });

    $("#btnNo").click(function () { $("#chkEsEmpresa").focus();});
    
    // Promocion en casa//
    var AlumnoPromocion, tblAlumno2, dataAlumno = [], hayPromocion = false;

    $("#btnPromocion").click(function () {
        $("#PopAlumnoPromocion").modal('show');
    });

    $("#dtAlumno1").on("click", "a", function () {
        $("#btnPromocion").text("Modificar");
        $("#lbAlumnoPromocion").text(dataAlumno[0].AlumnoId + " | " + dataAlumno[0].NombreC);
        $("#divAlumnoPormocion").show();
        hayPromocion = true;
        $("#PopAlumnoPromocion").modal('hide');
    });

    $("#btnClosePromo").click(function () {
        $("#txtClave1").val("");
        if (tblAlumno2 != undefined) {
            tblAlumno2.fnClearTable();
        }
        $("#btnPromocion").text("Agregar");
        hayPromocion = false;
        $("#divAlumnoPormocion").hide();
    });

    $('#txtClave1').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar1').click();
        }
    });

    $("#btnBuscar1").click(function () {

        AlumnoPromocion = $('#txtClave1').val();
        if (AlumnoPromocion.length == 0) { return false; }
        if (tblAlumno2 != undefined) {
            tblAlumno2.fnClearTable();
        }
        $('#hCarga').text("Cargando...");
        $('#Load').modal('show');

        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultarAlumnoPromocionCasa2",
            data: "{AlumnoPromocion:'" + AlumnoPromocion + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }
                dataAlumno.length = 0;
                dataAlumno.push(data.d);

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
            }
        });
    });

    function GuardarPromocion(AlumnoProspecto) {

        dataAlumno[0].AlumnoIdProspecto = AlumnoProspecto;
        dataAlumno[0].Anio = $('#slcPeriodo').val().substring(2);
        dataAlumno[0].PeriodoId = $('#slcPeriodo').val().substring(0, 1);
        dataAlumno[0].UsuarioId = $.cookie('userAdmin');

        var obj = {
            "Promocion": dataAlumno[0]
        };
        obj = JSON.stringify(obj);


        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/GuardarPromocionCasa",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d) {

                }
            }
        });

    }

    // Promocion en casa//
    return {
        init: function () {
            FormWizard();
        }
    };
}();