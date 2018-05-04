
$(function () {
    var AlumnoNum;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    AlumnoNum = localStorage.getItem("user");

    var datosFn =
        {
            init() {
                GlobalFn.init()
                GlobalFn.GetGenero();
                GlobalFn.GetEstado("slcEstado", 9);
                GlobalFn.GetEstadoCivil();
                this.Load();
                
                $('#Guardar').on('click', function () {
                    if (form.valid() == false) { return false; }
                    datosFn.GuardarTodo();
                });

                form.validate({
                    errorElement: 'span', //default input error message container
                    errorClass: 'help-block help-block-error', // default input error message class
                    focusInvalid: false, // do not focus the last invalid input
                    rules: {

                        txtemail: {
                            required: true,
                            email: true,
                            minlength: 4,
                            maxlength: 100
                        },
                        txtCelular: {
                            required: true,
                            digits: true,
                            minlength: 10,
                            maxlength: 12
                        },
                        txtTelefonoCasa: {
                            digits: true,
                            //required: true,
                            minlength: 8,
                            maxlength: 12
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
            Load()
            {
                datosFn.LimpiarCampos();
                IndexFn.Block(true);
                datosFn.EsNumero(AlumnoNum);
            },
            LimpiarCampos() {
                $("#submit_form").trigger('reset');
                error.hide();
                success.hide();
                $('#submit_form  div').removeClass('has-error');
                $('#submit_form  div').removeClass('has-success');
                $('#submit_form  i').removeClass('fa-warning');
                $('#submit_form  i').removeClass('fa-check');
            },
            EsNumero(Alumno) {
                $.ajax({
                    type: "Get",
                    url: "Api/Alumno/ObenerDatosAlumnoActualiza/" + Alumno,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data != null) {
                            ///Personales 
                            document.getElementById("fotoAlumno").src = "data:image/png;base64," + data.DTOAlumnoDetalle.fotoBase64;
                            $('#txtnombre').val(data.Nombre + " " + data.Paterno + " " + data.Materno);
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

                            IndexFn.Block(false);
                        }
                        else {
                            $('#popDatos').modal('hide');
                            IndexFn.Block(false);
                            alertify.alert("Universidad YMCA", "Error, El Alumno no Existe.");

                        }

                    }
                });
            },
            GuardarTodo() {
                var obj = {
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
                    "MunicipioId": $('#slcMunicipio').val()
                };
                obj = JSON.stringify(obj);
                IndexFn.Block(true);

                $.ajax({
                    type: "POST",
                    url: "Api/Alumno/UpdateAlumnoDatos",
                    data: obj,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data) {
                            IndexFn.Block(false);
                            $('#popDatos').modal('hide');
                            alertify.alert("Universidad YMCA", "Datos del Alumno Modificados", function () {
                                //VerificarEncuesta();
                            });
                        } else {
                            IndexFn.Block(false);
                            $('#popDatos').modal('hide');
                            alertify.alert("Universidad YMCA", "Error, Revisar datos capturados.", function () {
                                $('#popDatos').modal('show');
                            });
                        }
                    }
                });


            },
            VerificarEncuesta() {
                $.ajax({
                    type: "Get",
                    url: "Api/Alumno/VerificaAlumnoEncuesta/" + AlumnoNum,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data) {
                            $('#popDatos').empty();
                            $('#popDatos').load('Views/Alumno/EncuestaPortal.html');
                        }
                    }
                });
            },
        };
    datosFn.init();

});
