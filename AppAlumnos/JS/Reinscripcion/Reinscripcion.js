$(function init() {
    // $.cookie('user', 7493, { expires: 1 });
    var lstCuotas;
    var AlumnoId;
    var OfertaEducativa;
    var Flujo;
    var Tipo;
    var Bandera = 0;
    $('#btnGenerar').prop("disabled", true);
    DatosAlumno();
    function DatosAlumno() {
        $('#Load').modal('show');
        AlumnoId = $.cookie('user');
        //var AlumnoId = '9579';
        $.ajax({
            url: 'Services/Alumno.asmx/ConsultarAlumnoReinscripcion',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == null) {
                    $('#Load').modal('hide');
                    return null;
                }
                $('#lblAlumno').text(data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno);
                $(data.d.lstAlumnoInscrito).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.OfertaEducativa.Descripcion);
                    option.val(this.OfertaEducativa.OfertaEducativaId);
                    option.attr("data-Tipo", this.OfertaEducativa.OfertaEducativaTipoId);
                    $('#slcOfertaEducativa').append(option);
                });
                
                if (data.d.lstAlumnoInscrito.length ==1) {
                    $('#slcOfertaEducativa').val(data.d.lstAlumnoInscrito[0].OfertaEducativaId);
                    $('#slcOfertaEducativa').change();
                    $('#slcOfertaEducativa').prop("disabled", true);
                } else {
                    $('#Load').modal('hide');
                }
            }
        });
    }
    $('#slcOfertaEducativa').change(function () {
        if ($('#slcOfertaEducativa').val() == -1) { return false; } else {
            $('#Load').modal('show');
            Adeudos();
        }
    });
    function Pagar(Descripcion) {
         OfertaEducativa = $('#slcOfertaEducativa').val();
         $.ajax({
             type: "POST",
             url: "Services/Reinscripcion.asmx/GenerarInscrCole",
             data: "{AlumnoId:" + AlumnoId + ",OfertaEducativaId:" + OfertaEducativa + ",PeriodoD:'" + Descripcion + "'}", // the data in form-encoded format, ie as it would appear on a querystring
             //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
             contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
             success: function (data) {
                 if (data.d == "Guardado") {
                     alertify.alert("Tus Cargos se han generado correctamente.");
                     Bandera = 0;
                 } else {
                     alertify.alert("Se a producido un error intente de nuevo mas tarde.");
                     Bandera = 0;
                     $('#btnGenerar').prop("disabled", false);;
                     console.log(data.d);
                 }
                 $('#Load').modal('hide');
             }
         });
    }
    $('#btnGenerar').on('click', function () {
        $('#Load').modal('show');
        $('#btnGenerar').prop("disabled", true);
        OfertaEducativa = $('#slcOfertaEducativa').val();
        var TipoOferta = $(Tipo).data("tipo");
        if (TipoOferta == 4) {
            $.ajax({
                type: "POST",
                url: "Services/WS/Reinscripcion.asmx/ConsultarPagodeMes",
                data: "{AlumnoId:" + AlumnoId + ",OfertaEducativaId:" + OfertaEducativa + "}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d == null) {
                        $('#btnGenerar').prop("disabled", false);
                        $('#Load').modal('hide');
                        return false;
                    }
                    if (data.d.length > 1) {
                        $('#btnActual').text(data.d[0].Descripcion);
                        $('#btnActual').attr("data-MesId", data.d[0].MesId);
                        $('#btnSiguiente').text(data.d[1].Descripcion);
                        $('#btnSiguiente').attr("data-MesId", data.d[1].MesId);
                    } else {
                        $('#btnActual').css("visibility", "hidden");
                        $('#btnSiguiente').text(data.d[0].Descripcion);
                        $('#btnSiguiente').attr("data-MesId", data.d[0].MesId);
                    }
                    $('#btnGenerar').prop("disabled", false);
                    $('#small').modal('show');
                }
            });
        }
        else {
            $.ajax({
                type: "POST",
                url: "Services/Reinscripcion.asmx/ConsultarPagosPeriodo",
                data: "{AlumnoId:" + AlumnoId + ",OfertaEducativaId:" + OfertaEducativa + "}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    $('#Load').modal('hide');
                    if (data.d[0] == "Generar") {
                        var Descripcion = data.d[1];
                        alertify.confirm("<p>¿ Desea continuar generando las referencias de reinscripción para el periodo " + Descripcion + " ?<br><br><hr>", function (e) {
                            if (e) {
                                $('#Load').modal('show');
                                if (Bandera == 0) {
                                    Bandera = 1;
                                    Pagar(Descripcion);                                    
                                }
                                
                            } else { $('#btnGenerar').prop("disabled", false); }
                        });
                    } else if (data.d[0] == "Completo") {
                        alertify.alert("El Alumno ya tiene generados todos sus cargos del cuatrimestre");
                        $('#btnGenerar').prop("disabled", true);
                        return false;
                    } else if (data.d[0] == "No es Idioma") {
                        alertify.alert("El Alumno ya tiene sus cuotas generadas");
                        $('#btnGenerar').prop("disabled", true);
                        return false;
                    }
                    //else {
                    //    alertify.confirm("El alumno ya tiene generado cargos para el cuatrimestre," +
                    //        "¿Quiere generar la colegiatura para el siguiente mes?", function (e) {
                    //            if (e) {
                    //                Pagar();
                    //            } 
                    //        });

                    //}
                }
            });
        }

    });
    $('#btnSiguiente').on('click', function () {
        var mesid = $('#btnSiguiente').data("mesid");
        GenerarIngles(mesid);
    });
    $('#btnActual').on('click', function () {
        var mesid = $('#btnActual').data("mesid");
        GenerarIngles(mesid);
    });
    function GenerarIngles(MesId) {
        $.ajax({
            type: "POST",
            url: "Services/Reinscripcion.asmx/GenerarColegiaturaIngles",
            data: "{AlumnoId:" + AlumnoId + ",OfertaEducativaId:" + OfertaEducativa + ",MesId:" + MesId + "}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                $('#small').modal('hide');
                $('#btnSiguiente').removeData('mesid');
                $('#btnActual').removeData('mesid');
                if (data.d == "Guardado") {
                    alertify.alert("Tus Cargos se han generado correctamente.");
                    //CargarTabla();
                } else {
                    alertify.alert("Se a producido un error intente de nuevo mas tarde.");
                    console.log(data.d);
                }
            }
        });
    }
    function formato_numero(numero, decimales, separador_decimal, separador_miles) { // v2007-08-06
        numero = parseFloat(numero);
        if (isNaN(numero)) {
            return "";
        }

        if (decimales !== undefined) {
            // Redondeamos
            numero = numero.toFixed(decimales);
        }

        // Convertimos el punto en separador_decimal
        numero = numero.toString().replace(".", separador_decimal !== undefined ? separador_decimal : ",");

        if (separador_miles) {
            // Añadimos los separadores de miles
            var miles = new RegExp("(-?[0-9]+)([0-9]{3})");
            while (miles.test(numero)) {
                numero = numero.replace(miles, "$1" + separador_miles + "$2");
            }
        }

        return numero;
    }
    function Pendiente() {
        OfertaEducativa = $('#slcOfertaEducativa').val();
        $.ajax({
            type: "POST",
            url: "Services/Reinscripcion.asmx/Pendiente",
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#Load').modal('hide');
                //console.log(data);
                if (data.d.length > 1) {
                    alertify.confirm("El alumno esta Inscrito pero no ha generado cargos" +
                        "¿Desea generar sus cargos del " + data.d[1] + "?", function (e) {
                            if (e) {
                                $('#Load').modal('show');
                                $.ajax({
                                    type: "POST",
                                    url: "Services/Reinscripcion.asmx/InscribirGenerar",
                                    data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        if (data.d == "Guardado") {
                                            alertify.alert("Tus Cargos se han generado correctamente.");
                                        } else {
                                            alertify.alert("Se a producido un error intente de nuevo mas tarde.");
                                        }
                                        $('#Load').modal('hide');
                                    }
                                });
                            }
                        });
                }
            }
        });
    }
    function Adeudos()
    {
        $.ajax({
            type: "POST",
            url: "Services/Descuentos.asmx/ConsultarAdeudo",
            data: '{AlumnoId:' + AlumnoId + '}',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == "Debe") {
                    alertify.alert('Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                    $('#slcOfertaEducativa').val(-1);
                    $('#btnGenerar').prop("disabled", true);
                    $('#Load').modal('hide');
                } else {
                    $('#btnGenerar').prop("disabled", false);
                    Tipo = $('#slcOfertaEducativa').find('option:selected');
                    //Pendiente();
                    $('#Load').modal('hide');
                }
            }
        });
    }
});