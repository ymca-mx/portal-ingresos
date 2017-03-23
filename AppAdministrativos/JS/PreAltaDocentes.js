var Validacion = function () {
    var Usuario;
    var b_Valid = 0;
    //$.cookie('userAdmin', 6883, { expires: 1 });
    var FormWizard = function () {
      
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
                    minlength: 4,
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
                    minlength: 4,
                    maxlength: 50
                },
                txtApMaterno: {
                    required: true,
                    digits: false,
                    minlength: 4,
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
                    maxlength: 10
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
                    minlength: 12,
                    maxlength: 13
                },
                txtTelefonoCasa: {
                    digits: true,
                    required: true,
                    minlength: 8,
                    maxlength: 10
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
            if ($('#slcOferta').val() == '4') {
                GuardarIngles();
            } else {
                GuardarDescuentos();
            }
            //AbrirDocumento();
        }).hide();
        $('#form_wizard_1').find('.button-previous').hide();

        $("#txtCP").keypress(function (e) {
            //if the letter is not digit then display error and don't type anything
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });
        $("#btnListado").on('click', function () {
            $('#divDinamico').empty();
            var url = $(this).attr("href");
            $('#divDinamico').load(url);
            return false;
        });
        $('#Guardar').keydown(function () {
            keydown = 1;
            $('#Guardar').mousedown();
            return false;
        });
        $('#Guardar').mousedown(function (event) {

            if (form.valid() == false) {
                return false;
            } else {

                alertify.confirm("<p>¿Esta seguro que desea guardar los cambios?<br><br><hr>", function (e) {
                    if (e) {
                        Guardar();
                        //$('#form_wizard_1').find('.button-submit').click();
                    }
                });
            }
        });
        
    }
    function Guardar() {
        var Usuario = $.cookie('userAdmin');
        if (jQuery.type(Usuario) === "undefined") {
            return false;
        }
        var Civil = $('#slcEstadoCivil').val();
        var Sex = $('#slcSexo').val();
        var datosDoc = '{';
        datosDoc += "Nombre:'" + $('#txtnombre').val() + "',Paterno:'" + $('#txtApPaterno').val();
        datosDoc += "',Materno:'" + $('#txtApMaterno').val() + "',EstadoCivil:'" + Civil;
        datosDoc += "',FechaNacimiento:'" + $('#txtFNacimiento').val() + "',Genero:'" + Sex;
        datosDoc += "',RFC:'" + $('#txtCURP').val() + "',Email:'" + $('#txtEmail').val();
        datosDoc += "',TelCelular:'" + $('#txtCelular').val() + "',TelCasa:'" + $('#txtTelefonoCasa').val() + "',UsuarioId:'" + Usuario;
        datosDoc += "'}";
        $.ajax({
            type: "POST",
            url: "WS/Docentes.asmx/GuardarDocente",
            data: datosDoc, // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            datatype: JSON,
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d != '-1') {
                    alertify.alert("El Docente se guardo correctamente.");

                }
                else {
                    alertify.alert("Error: Intente más tarde.");
                }
            }
        });
    }

    return {
        init: function () {
            FormWizard();
        }
    };
}();