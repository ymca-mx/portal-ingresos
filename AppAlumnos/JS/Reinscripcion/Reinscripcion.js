$(function() {

    var lstCuotas,
        AlumnoId,
        OfertaEducativa,
        Flujo,
        Tipo,
        Bandera = 0;
    

    var ReinscripcionFn = {
        init() {
            $('#btnGenerar').prop("disabled", true);
            $('#slcOfertaEducativa').on('change', this.OfertaEducativaChange);
            $('#btnGenerar').on('click', this.btnGenerar);
            $('#btnSiguiente').on('click', this.btnSiguiente);
            $('#btnActual').on('click', this.btnActual);
            this.DatosAlumno();
        },
        Pagar(Descripcion) {
            OfertaEducativa = $('#slcOfertaEducativa').val();
            IndexFn.Api("Reinscripcion/GenerarInscrCole", "POST",
                JSON.stringify({ AlumnoId: AlumnoId, OfertaEducativaId: OfertaEducativa, PeriodoD: Descripcion }))
                .done(function (data) {
                    if (data[0] === "Guardado") {
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
                    alertify.alert("Universidad YMCA", "Se a producido un error. </br> Porfavor actualiza la pagina e intente de nuevo.");
                    Bandera = 0;
                    $('#btnGenerar').prop("disabled", false);;
                    console.log(data);
                    IndexFn.Block(false);
                });
        },
        DatosAlumno() {
            IndexFn.Block(true);
            AlumnoId = localStorage.getItem("user");

            IndexFn.Api('Alumno/ConsultarAlumnoReinscripcion/' + AlumnoId, "GET", "")
                .done(function (data) {
                    if (data == null) {
                        IndexFn.Block(false);
                        return null;
                    }
                    $('#lblAlumno').text(data.Nombre + " " + data.Paterno + " " + data.Materno);
                    $(data.Ofertas).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.Descripcion);
                        option.val(this.OfertaEducativaId);
                        $('#slcOfertaEducativa').append(option);
                    });

                    if (data.Ofertas.length == 1) {
                        $('#slcOfertaEducativa').val(data.Ofertas[0].OfertaEducativaId);
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
        },
        Adeudos() {
            IndexFn.Api("Descuentos/ConsultarAdeudo/" + AlumnoId, "GET", "")
                .done(function (data) {
                    if (data == "Debe") {
                        alertify.alert("Universidad YMCA", 'Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                        $('#slcOfertaEducativa').val(-1);
                        $('#btnGenerar').prop("disabled", true);
                        IndexFn.Block(false);
                    } else {
                        $('#btnGenerar').prop("disabled", false);
                        Tipo = $('#slcOfertaEducativa').find('option:selected');
                        var TipoOferta = $(Tipo).data("tipo");
                        if (TipoOferta === 2) {
                            $('#pulsate-regular').pulsate({ color: "#de3030" });
                            $('#divpul').show();

                        }
                        IndexFn.Block(false);
                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Universidad YMCA", 'Ocurrió un error al consultar su estatus financiero.');
                    console.log(data);
                });
            
        },
        OfertaEducativaChange() {
            if (parseInt($('#slcOfertaEducativa').val()) === -1) { return false; } else {
                $('#divpul').hide();
                IndexFn.Block(true);
                ReinscripcionFn.Adeudos();
            }
        },
        btnGenerar() {
            IndexFn.Block(true);
            $('#btnGenerar').prop("disabled", true);
            OfertaEducativa = $('#slcOfertaEducativa').val();

            IndexFn.Api("Reinscripcion/ConsultarPagosPeriodo/" + AlumnoId + "/" + OfertaEducativa, "GET", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    if (data[0] === "Generar") {
                        var Descripcion = data[1];
                        alertify.confirm("Universidad YMCA",
                            "<p>¿ Desea continuar generando las referencias de reinscripción para el periodo " + Descripcion + " ?<br><br><hr>",
                            function () {
                                IndexFn.Block(true);
                                if (Bandera == 0) {
                                    Bandera = 1;
                                    ReinscripcionFn.Pagar(Descripcion);
                                }
                            },
                            function () {
                                $('#btnGenerar').prop("disabled", false);
                            });
                    } else if (data[0] === "Completo") {
                        alertify.alert("Universidad YMCA", "El Alumno ya tiene generados todos sus cargos del cuatrimestre");
                        $('#btnGenerar').prop("disabled", true);
                        return false;
                    } else if (data[0] === "No es Idioma") {
                        alertify.alert("Universidad YMCA", "El Alumno ya tiene sus cuotas generadas");
                        $('#btnGenerar').prop("disabled", true);
                        return false;
                    }
                })
                .fail(function (data) {
                    alertify.alert("Universidad YMCA", "Ocurrio un problema al consultar tu periodo.");
                    console.log(data);
                    return false;
                });
        }
    };

    ReinscripcionFn.init();
});