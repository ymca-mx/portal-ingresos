var Login = function () {

    function Validar() {
        var credenciales = {
            username: $('#username').val(),
            password: $('#password').val()
        };

        $.ajax({
            url: '/Api/Login/Valida',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(credenciales),
            dataType: 'json',
            success: function (Resultado) {
                Datos = Resultado.d;
                if (Datos === null)
                    alert('Favor de verificar las credenciales');
                else {
                    $.cookie('userAdminE', credenciales['username'], { expires: 1 });
                    $('#username').append($.cookie('userAdminE'));
                    $(location).attr('href', 'index.html');
                }
            },
            error: function (Resultado) {
                alert('Se presento un error en la validación de las credenciales');
            }
        });
    }

    function RecoveryPassword() {
      

        $.ajax({
            url: 'WS/Usuario.asmx/RecuperaPassword',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: "{'email': '" + $('#email').val() + "'}",
            dataType: 'json',
            success: function (Resultado) {
                //$(location).attr('href', 'login.html');
                $('.content').css('width', '550px');
                $('#div-recovery').html(

                    "<p style='text-align: right;'><a href='http://108.163.172.122/portaladministrativo/login.html'>Log In</a> | Universidad YMCA" +
                    "</p>" +
                    "<br />" +
                    "<p><h3>Recuperación de la contraseña.</h3></p>" +
                    "<p>Se ha enviado un email a la cuenta de correo electrónico asociado," +
                    " si no puedes visualizarlo en tu bandeja de entrada en los proximos 15 minutos; " +
                    " buscalo en tu carpeta de elementos no deseados.</p>" +
                    "<p>Si lo encuentras ahi, por favor marcalo como 'No Spam'.</p>");

                jQuery('.login-form').hide();
                jQuery('.forget-form').hide();
            },
            error: function (Resultado) {
                alert('Se presento un error en la validación de las credenciales');
            }
        });
    }

    var handleLogin = function () {
        $('.login-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                username: {
                    required: true
                },
                password: {
                    required: true
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   
                $('.alert-danger', $('.login-form')).show();
            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        $('#btnLoginU').click(function () {
            if ($('#form_login').valid())
                Validar();
        });


        $('.login-form input').keypress(function (e) {
            if (e.which === 13) {
                if ($('#form_login').valid())
                    Validar();
            }
        });
    }

    var handleForgetPassword = function () {
        $('.forget-form').validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",
            rules: {
                email: {
                    required: true,
                    email: true
                }
            },

            invalidHandler: function (event, validator) { //display error alert on form submit   

            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function (label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function (error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function (form) {
                form.submit();
            }
        });

        /*
        $('.forget-form input').keypress(function (e) {
            if (e.which == 13) {
                if ($('.forget-form').validate().form()) {
                    //$('.forget-form').submit();

                }
                return false;
            }
        });
        */

        $('.forget-form input').keypress(function (e) {
            if (e.which === 13) {
                if ($('.forget-form').valid())
                    Validar();
            }
        });

        $('#btnLogin').click(function () {
            RecoveryPassword();
        });


        jQuery('#forget-password').click(function () {
            jQuery('.login-form').hide();
            jQuery('.forget-form').show();
        });

        jQuery('#back-btn').click(function () {
            jQuery('.login-form').show();
            jQuery('.forget-form').hide();
        });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleLogin();
            handleForgetPassword();
        }
    };

}();