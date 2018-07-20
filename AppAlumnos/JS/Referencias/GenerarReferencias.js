$(function () {
    
        tblReferencias;
    
    var ReferenciasFn = {
        init() {
            $('#slcOfertaEducativa').on('change', this.OfertaEducativaChange);
            $('#btnGenerar').on('click', this.BtnGenerarClck);
            this.DatosAlumno();
        },
        DatosAlumno() {
            IndexFn.Block(true);
            IndexFn.Api("Alumno/ConsultarAlumno/" + localStorage.getItem("user"), "GET", "")
                .done(function (data) {
                    if (data == null) {
                        IndexFn.Block(false);
                        return false;
                    }
                    $('#lblAlumno').text(data.Nombre + " " + data.Paterno + " " + data.Materno);
                    $(data.lstAlumnoInscrito).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.OfertaEducativa.Descripcion);
                        option.val(this.OfertaEducativa.OfertaEducativaId);
                        $('#slcOfertaEducativa').append(option);
                    });
                    if (data.lstAlumnoInscrito.length == 1) {
                        $('#slcOfertaEducativa').val(data.lstAlumnoInscrito[0].OfertaEducativaId);
                        $('#slcOfertaEducativa').change();
                    }
                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                });
        },
        ConsutlarAdeudos(ofertaEducativa) {
            var AlumnoId = localStorage.getItem("user");

            IndexFn.Api("Alumno/ConsultarAdeudo/" + AlumnoId + "/" + ofertaEducativa, "GET", "")
                .done(function (data) {
                    if (data === "Debe") {
                        alertify.alert('Universidad YMCA', 'Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                        ReferenciasFn.CargarPagosConceptos(AlumnoId);
                    } else {
                        ReferenciasFn.CargarConceptos(ofertaEducativa);

                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert('Universidad YMCA', "Error al consultar los adeudos.");
                    console.log(data);
                });

        },
        CargarConceptos(OfertaEducativa) {
            $("#slcConceptos").empty();
            IndexFn.Api("General/Conceptos/" +  OfertaEducativa, "GET", "")
                .done(function (data) {
                    var datos = data;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.Descripcion + " | $" + ReferenciasFn.formato_numero(this.Monto, 2, ".", ","));
                        option.val(this.PagoConceptoId);
                        option.data("OfertaEducativaId", this.OfertaEducativaId);
                        option.data("CuotaId", this.CuotaId);
                        option.data("EsMultireferencia", this.EsMultireferencia);
                        option.data("Descripcion", this.Descripcion);

                        $('#slcConceptos').append(option);
                    });
                    ReferenciasFn.CargarPagosConceptos(localStorage.getItem("user"));
                })
                .fail(function (data) {
                    alertify.alert('Universidad YMCA', "Error al consultar los conceptos.");
                    console.log(data);
                });
        },
        CargarPagosConceptos(Alumno) {
            IndexFn.Api('Alumno/ConsultarReferenciasCP/' + Alumno, "GET", "")
                .done(function (Respuesta) {
                    ReferenciasFn.ReferenciasTbl(Respuesta);
                    var fil = $('#tblReferencias label input');
                    fil.removeClass('input-small').addClass('input-large');
                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert('Universidad YMCA', "Error al consultar referencias existentes.");
                    console.log(data);
                });
        },
        ReferenciasTbl(R) {
            tblReferencias = $('#tblReferencias').dataTable({
                "aaData": R,
                "bSort": false,
                "aoColumns": [
                    { "mDataProp": "Descripcion" },
                    { "mDataProp": "ReferenciaId" },
                    { "mDataProp": "Cuota" },
                    { "mDataProp": "FechaLimite" }
                ],

                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": false,
                "ordering": false,
                "async": true,
                "bDestroy": true,
                "bPaginate": false,
                "bLengthChange": true,
                "bFilter": false,
                "bInfo": false,
                "pageLength": 20,
                "bAutoWidth": false,
                "asStripClasses": null,
                "language": {
                    "lengthMenu": "_MENU_  Registros",
                    "paginate": {
                        "previous": "<",
                        "next": ">"
                    },
                    "search": "Buscar Alumno ",
                },
                "createdRow": function (row, data, dataIndex) {
                    row.childNodes[3].style.textAlign = 'center';
                    if (dataIndex / 2 != 0) {
                        $(row).addClass("BackColor");
                    }

                    if (data.PagoId <= 2588) {
                        $(row).addClass("bold");
                    }
                }
            });
        },
        GenerarPago(Cuota) {
            IndexFn.Api("Descuentos/GenerarPago", "POST", JSON.stringify(Cuota))
                .done(function (data) {
                    ReferenciasFn.CargarPagosConceptos(Cuota.AlumnoId);
                    ReferenciasFn.Alerta();
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert('Universidad YMCA', "Error al generar tu referencia. </ br> Por favor actualiza la pagina.");
                    console.log(data);
                });
        },
        Alerta() {

            var ahref = "<p> Los pagos se cancelaran automáticamente después de 15 días.</p><p> Para mas información " +
                "<a class='btn btn-primary' href=javascript:window.open('Views/Pago/ListaConceptos.html'," +
                "'Tramites'" + "," +
                "'width=800,height=550'" + ");>click aqui</a> </p>";
            var delay = alertify.get('notifier', 'delay');
            alertify.set('notifier', 'delay', 100);
            alertify.warning(ahref);
            alertify.set('notifier', 'delay', delay);
        },
        formato_numero(numero, decimales, separador_decimal, separador_miles) { // v2007-08-06
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
        },
        BuscarTabla(Descripcion) {
            var respu = false;
            $('#tblReferencias tbody tr').each(function () {
                var td = this.childNodes[0];
                if (td.innerHTML == Descripcion) {
                    respu = true;
                    return false;
                }
            });
            return respu;
        },
        OfertaEducativaChange() {
            if (parseInt($('#slcOfertaEducativa').val()) != -1) {
                IndexFn.Block(true);
                $("#slcConceptos").empty();
                ReferenciasFn.ConsutlarAdeudos($('#slcOfertaEducativa').val());
            } else {
                $("#slcConceptos").empty();
            }
        },
        BtnGenerarClck() {
            
            var Variables = "";
            var cFech;
            if ($('#slcConceptos').val() === null
                || parseInt($('#slcConceptos').val()) === -1) {
                return false;
            }

            IndexFn.Block(true);

            var ConceptoSelec = $('#slcConceptos').find(":selected");
            Variables = {
                AlumnoId: localStorage.getItem("user"),
                OfertaEducativaId: ConceptoSelec.data("OfertaEducativaId"),
                PagoConceptoId: $('#slcConceptos').val(),
                CuotaId: ConceptoSelec.data("CuotaId")
            };

            if (ReferenciasFn.BuscarTabla(ConceptoSelec.data('Descripcion')) && ConceptoSelec.data('EsMultireferencia')) {
                ReferenciasFn.GenerarPago(Variables);
            }
            else if (!ReferenciasFn.BuscarTabla(ConceptoSelec.data('Descripcion'))) {
                ReferenciasFn.GenerarPago(Variables);
            }
            else {
                alertify.alert("Universidad YMCA", "El concepto que selecciono ya esta Generado");
                IndexFn.Block(false);
            }

        }
    };

    ReferenciasFn.init();
});