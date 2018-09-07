$(function () {
    
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

        jQuery('#forget-password').click(function () {
            jQuery('.login-form').hide();
            jQuery('.forget-form').show();
        });

        jQuery('#back-btn').click(function () {
            jQuery('.login-form').show();
            jQuery('.forget-form').hide();
        });

    var Login = {
        init() {
            $('.forget-form input').keypress(function (e) {
                if (e.which === 13) {
                    if ($('.forget-form').valid())
                        Login.Validar();
                }
            });

            $('#btnLogin').click(function () {
                Login.RecoveryPassword();
            });

            $('#btnLoginU').click(function () {
                if ($('#form_login').valid())
                    Login.Validar();
            });


            $('.login-form input').keypress(function (e) {
                if (e.which === 13) {
                    if ($('#form_login').valid())
                        Login.Validar();
                }
            });
        },
        Validar() {


            $.blockUI({
                message: $('#Load'),
                css: { backgroundColor: '#48525e', color: '#fff', border: 'none' }
            });

            var credenciales = {
                UsuarioId: $('#username').val(),
                Password: $('#password').val()
            };

            $.ajax({
                url: 'Api/Login/Valida',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(credenciales),
                dataType: 'json'
            })
                .done(function (Resultado) {
                    $.unblockUI({ onUnblock: function () { } });

                    if (Resultado === null)
                        alert('Favor de verificar las credenciales');
                    else {
                        if (!Resultado.Status) {
                            alert("ocurrio un error al consultar la información.");
                            return false;
                        }

                        localStorage.setItem('userAdmin', credenciales.UsuarioId);
                        $(location).attr('href', 'index.html');
                    }
                })
                .fail(function (Resultado) {
                    $.unblockUI({ onUnblock: function () { } });
                    if (Resultado.status===404) {
                        alert("Credenciales incorrectas");
                        return false;
                    }
                    alert('Se presento un error en la validación de las credenciales');
                });
        },
        RecoveryPassword() {
            $.blockUI({
                message: $('#Load'),
                css: { backgroundColor: '#48525e', color: '#fff', border: 'none' }
            });
            
            var obj =
                {
                    Email: $('#email').val()
                };

            $.ajax({
                url: 'Api/Usuario/RecuperaPassword',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(obj),
                dataType: 'json'
            })
                .done(function (Resultado) {
                    $.unblockUI({ onUnblock: function () { } });

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
                })
                .fail(function (Resultado) {
                    $.unblockUI({ onUnblock: function () { } });
                    if (Resultado.status === 404) {
                        alert("El email ingresado no existe.");
                        return false;
                    }
                    alert('Se presento un error en la validación de las credenciales');
                });
        }
    };

    Login.init();
});