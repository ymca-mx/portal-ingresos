﻿var Mostrar = {
    show: function () {
        console.log('Soy modal');
        $('#popDatos').modal('show');
    }
};

$(document).ready(function init() {
    var AlumnoNum;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    AlumnoNum = $.cookie('user');
    getUrlVars();
    Load();

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    }
    
    function Load() {
        
        LimpiarCampos();
        $('#Load').modal('show');
        EsNumero(AlumnoNum);
    }


    function LimpiarCampos() {
        $("#submit_form").trigger('reset');
        error.hide();
        success.hide();
        $('#submit_form  div').removeClass('has-error');
        $('#submit_form  div').removeClass('has-success');
        $('#submit_form  i').removeClass('fa-warning');
        $('#submit_form  i').removeClass('fa-check');
    }

    function EsNumero(Alumno) {
        $.ajax({
            type: "POST",
            url: "Services/Alumno.asmx/ObenerDatosAlumnoActualiza",
            data: "{AlumnoId:'" + Alumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    $('#slcNacionalidad').val(data.d.DTOAlumnoDetalle.PaisId == 146 ? 1 : 2);
                    if (data.d.DTOAlumnoDetalle.PaisId == 146) {
                        CargarEstados($('#slcLugarN'), data.d.DTOAlumnoDetalle.EntidadNacimientoId);
                    } else {
                        CargarPaises($('#slcLugarN'), data.d.DTOAlumnoDetalle.PaisId);
                    }
                    ///Personales 
                    $('#txtnombre').val(data.d.Nombre);
                    $('#txtApPaterno').val(data.d.Paterno);
                    $('#txtApMaterno').val(data.d.Materno);
                    $('#txtCelular').val(data.d.DTOAlumnoDetalle.Celular);
                    $('#txtFNacimiento').val(data.d.DTOAlumnoDetalle.FechaNacimientoC);
                    $('#txtCURP').val(data.d.DTOAlumnoDetalle.CURP);
                    $('#txtEmail').val(data.d.DTOAlumnoDetalle.Email);
                    $('#txtTelefonoCasa').val(data.d.DTOAlumnoDetalle.TelefonoCasa);

                    $('#txtCalle').val(data.d.DTOAlumnoDetalle.Calle);
                    $('#txtNumeroE').val(data.d.DTOAlumnoDetalle.NoExterior);
                    $('#txtNumeroI').val(data.d.DTOAlumnoDetalle.NoInterior);
                    $('#txtCP').val(data.d.DTOAlumnoDetalle.Cp);
                    $('#txtColonia').val(data.d.DTOAlumnoDetalle.Colonia);

                    $('#slcEstadoCivil').val(data.d.DTOAlumnoDetalle.EstadoCivilId);
                    $('#slcSexo').val(data.d.DTOAlumnoDetalle.GeneroId);
                    $('#slcEstado').val(data.d.DTOAlumnoDetalle.EntidadFederativaId);
                    CargarEstados1(data.d.DTOAlumnoDetalle.EntidadFederativaId, data.d.DTOAlumnoDetalle.MunicipioId);

                    $('#Load').modal('hide');
                }
                else {
                    $('#popDatos').modal('hide');
                    $('#Load').modal('hide');
                    alertify.alert("Error, El Alumno no Existe.");

                }

            }
        });
    }

    function CargarEstados1(EstadoId, MunicipioId) {
        $('#slcEstado').empty();
        $.ajax({
            type: "POST",
            url: "Services/General.asmx/ConsultarEntidadFederativa",
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
                    url: "Services/General.asmx/ConsultarMunicipios",
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

    $('#Guardar').on('click',function () {
        if (form.valid() == false) { return false; }
        var nombre = $('#hCarga');
        nombre[0].innerText = "Guardando";
        $('#Load').modal('show');
        GuardarTodo();
    });

    function GuardarTodo() {


        var obj = {
            'AlumnoDatos': {
                "AlumnoId" : AlumnoNum ,
                "Celular": $('#txtCelular').val(),
                "Email" : $('#txtEmail').val() ,
                "TelefonoCasa": $('#txtTelefonoCasa').val(),
                "Calle":  $('#txtCalle').val() ,
                "NoExterior": $('#txtNumeroE').val(),
                "NoInterior": $('#txtNumeroI').val(),
                "Cp": $('#txtCP').val(),
                "Colonia": $('#txtColonia').val(),
                "EstadoCivilId": $('#slcEstadoCivil').val(),
                "EntidadFederativaId": $('#slcEstado').val(),
                "MunicipioId": $('#slcMunicipio').val()
            }
        };
        obj = JSON.stringify(obj);
        $.ajax({
            type: "POST",
            url: "Services/Alumno.asmx/UpdateAlumnoDatos",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $('#Load').modal('hide');
                    $('#popDatos').modal('hide');
                    alertify.alert("Datos del Alumno Modificados",function(){
                        //VerificarEncuesta();
                    });
                } else {
                    $('#Load').modal('hide');
                    $('#popDatos').modal('hide');
                    alertify.alert("Error, Revisar datos capturados.", function ()
                    {
                        $('#popDatos').modal('hide');
                        $('#popDatos').empty();
                    });
                }
            }
        });


    }

    function VerificarEncuesta() {
        $.ajax({
            type: "POST",
            url: "Services/Alumno.asmx/VerificaAlumnoEncuesta",
            data: "{AlumnoId:'" + AlumnoNum + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $('#popDatos').empty();
                    $('#popDatos').load('Views/Alumno/EncuestaPortal.html');
                }
            }
        });
    }

    function CargarPaises(combo, PaisId) {
        combo.empty();
        $.ajax({
            type: "POST",
            url: "Services/General.asmx/ConsultarPaises",
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
            url: "Services/General.asmx/ConsultarEntidadFederativa",
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

    $('#slcLugarUni').change(function () {
        $("#slcPaisUni").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcPaisUni").append(optionP);

        var tipo = $("#slcLugarUni");
        tipo = tipo[0].value;
        if (tipo == 2) {
            $('#lblLugarUni').html('Pais');
            CargarPaises($("#slcPaisUni"), -1);
        }
        else if (tipo == 1) {
            $('#lblLugarUni').html('Estado');
            CargarEstados($("#slcPaisUni"), -1);
        }
        else {
            $('#lblLugarUni').html(' ');
            $("#slcPaisUni").append(optionP);
        }
    });

    $('#slcNacionalidadPrep').change(function () {
        $("#slcEstadoPais").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcEstadoPais").append(optionP);

        var tipo = $("#slcNacionalidadPrep");
        tipo = tipo[0].value;
        if (tipo == 2) {
            $('#lblPN').html('País');
            CargarPaises($("#slcEstadoPais"), -1);
        }
        else if (tipo == 1) {
            $('#lblPN').html('Estado');
            CargarEstados($("#slcEstadoPais"), -1);
        }
        else {
            $('#lblPN').html('País | Estado');
            $("#slcEstadoPais").append(optionP);
        }
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




});
