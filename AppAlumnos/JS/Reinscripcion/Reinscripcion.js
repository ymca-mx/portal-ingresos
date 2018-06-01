$(function init() {

    var lstCuotas;
    var AlumnoId;
    var OfertaEducativa;
    var Flujo;
    var Tipo;
    var Bandera = 0;
    $('#btnGenerar').prop("disabled", true);
    DatosAlumno();
    function DatosAlumno() {
        IndexFn.Block(true);
        AlumnoId = localStorage.getItem("user");
        $.ajax({
            url: 'Api/Alumno/ConsultarAlumnoReinscripcion/' + AlumnoId,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        })
            .done(function (data) {
                if (data == null) {
                    IndexFn.Block(false);
                    return null;
                }
                $('#lblAlumno').text(data.Nombre + " " + data.Paterno + " " + data.Materno);
                $(data.lstAlumnoInscrito).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.OfertaEducativa.Descripcion);
                    option.val(this.OfertaEducativa.OfertaEducativaId);
                    option.attr("data-Tipo", this.OfertaEducativa.OfertaEducativaTipoId);
                    $('#slcOfertaEducativa').append(option);
                });

                if (data.lstAlumnoInscrito.length == 1) {
                    $('#slcOfertaEducativa').val(data.lstAlumnoInscrito[0].OfertaEducativaId);
                    $('#slcOfertaEducativa').change();
                    $('#slcOfertaEducativa').prop("disabled", true);
                } else {
                    IndexFn.Block(false);
                }
            })
            .fail(function (data) {
                console.log(data);
                IndexFn.Block(false);
                return null;
            });
    }
    $('#slcOfertaEducativa').change(function () {
        if ($('#slcOfertaEducativa').val() == -1) { return false; } else {
            $('#divpul').hide();
            IndexFn.Block(true);
            Adeudos();
        }
    });
    function Pagar(Descripcion) {
         OfertaEducativa = $('#slcOfertaEducativa').val();
        $.ajax({
            type: "Post",
            url: "Api/Reinscripcion/GenerarInscrCole",
            data: JSON.stringify({ AlumnoId: AlumnoId, OfertaEducativaId: OfertaEducativa, PeriodoD: Descripcion }),
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            if (data == "Guardado") {
                alertify.alert("Universidad YMCA", "Tus Cargos se han generado correctamente.");
                Bandera = 0;
            } else {
                alertify.alert("Universidad YMCA", "Se a producido un error intente de nuevo mas tarde.");
                Bandera = 0;
                $('#btnGenerar').prop("disabled", false);;
                console.log(data);
            }
            IndexFn.Block(false);
        })
            .fail(function (data) {
                IndexFn.Block(false);
                console.log(data);
            });
    }
    $('#btnGenerar').on('click', function () {
        IndexFn.Block(true);
        $('#btnGenerar').prop("disabled", true);
        OfertaEducativa = $('#slcOfertaEducativa').val();
        var TipoOferta = $(Tipo).data("tipo");
        if (TipoOferta == 4) {
            $.ajax({
                type: "Get",
                url: "Api/Reinscripcion/ConsultarPagodeMes/" + AlumnoId + "/" + OfertaEducativa,
                contentType: "application/json; charset=utf-8", 
                success: function (data) {
                    if (data == null) {
                        $('#btnGenerar').prop("disabled", false);
                        IndexFn.Block(false);
                        return false;
                    }
                    if (data.length > 1) {
                        $('#btnActual').text(data[0].Descripcion);
                        $('#btnActual').attr("data-MesId", data[0].MesId);
                        $('#btnSiguiente').text(data[1].Descripcion);
                        $('#btnSiguiente').attr("data-MesId", data[1].MesId);
                    } else {
                        $('#btnActual').css("visibility", "hidden");
                        $('#btnSiguiente').text(data[0].Descripcion);
                        $('#btnSiguiente').attr("data-MesId", data[0].MesId);
                    }
                    $('#btnGenerar').prop("disabled", false);
                    $('#small').modal('show');
                }
            });
        }
        else {
            $.ajax({
                type: "Get",
                url: "Api/Reinscripcion/ConsultarPagosPeriodo/" + AlumnoId + "/"+OfertaEducativa,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    IndexFn.Block(false);
                    if (data[0] == "Generar") {
                        var Descripcion = data[1];
                        alertify.confirm("<p>¿ Desea continuar generando las referencias de reinscripción para el periodo " + Descripcion + " ?<br><br><hr>", function (e) {
                            if (e) {
                                IndexFn.Block(true);
                                if (Bandera == 0) {
                                    Bandera = 1;
                                    Pagar(Descripcion);                                    
                                }
                                
                            } else { $('#btnGenerar').prop("disabled", false); }
                        });
                    } else if (data[0] == "Completo") {
                        alertify.alert("Universidad YMCA","El Alumno ya tiene generados todos sus cargos del cuatrimestre");
                        $('#btnGenerar').prop("disabled", true);
                        return false;
                    } else if (data[0] == "No es Idioma") {
                        alertify.alert("Universidad YMCA","El Alumno ya tiene sus cuotas generadas");
                        $('#btnGenerar').prop("disabled", true);
                        return false;
                    }
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
            url: "Api/Reinscripcion/GenerarColegiaturaIngles",
            data: JSON.stringify({ AlumnoId: AlumnoId, OfertaEducativaId: OfertaEducativa, MesId: MesId }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#small').modal('hide');
                $('#btnSiguiente').removeData('mesid');
                $('#btnActual').removeData('mesid');
                if (data == "Guardado") {
                    alertify.alert("Universidad YMCA", "Tus Cargos se han generado correctamente.");
                    //CargarTabla();
                } else {
                    alertify.alert("Universidad YMCA", "Se a producido un error intente de nuevo mas tarde.");
                    console.log(data);
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
            type: "Get",
            url: "Api/Reinscripcion/Pendiente/" + AlumnoId + "/" + OfertaEducativa,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                IndexFn.Block(false);
                //console.log(data);
                if (data.length > 1) {
                    alertify.confirm("Universidad YMCA","El alumno esta Inscrito pero no ha generado cargos" +
                        "¿Desea generar sus cargos del " + data[1] + "?", function (e) {
                            if (e) {
                                IndexFn.Block(true);
                                $.ajax({
                                    type: "POST",
                                    url: "Api/Reinscripcion/InscribirGenerar",
                                    data: JSON.stringify({ AlumnoId: AlumnoId, OfertaEducativaId: OfertaEducativa }),
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        if (data == "Guardado") {
                                            alertify.alert("Universidad YMCA", "Tus Cargos se han generado correctamente.");
                                        } else {
                                            alertify.alert("Universidad YMCA", "Se a producido un error intente de nuevo mas tarde.");
                                        }
                                        IndexFn.Block(false);
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
            type: "GET",
            url: "Api/Descuentos/ConsultarAdeudo/" + AlumnoId,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Debe") {
                    alertify.alert("Universidad YMCA",'Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                    $('#slcOfertaEducativa').val(-1);
                    $('#btnGenerar').prop("disabled", true);
                    IndexFn.Block(false);
                } else {
                    $('#btnGenerar').prop("disabled", false);
                    Tipo = $('#slcOfertaEducativa').find('option:selected');
                    //Pendiente();
                    var TipoOferta = $(Tipo).data("tipo");
                    if (TipoOferta === 2) {
                        $('#pulsate-regular').pulsate({color:"#de3030"});
                        $('#divpul').show();
                        
                    }
                    IndexFn.Block(false);
                }
            }
        });
    }
});