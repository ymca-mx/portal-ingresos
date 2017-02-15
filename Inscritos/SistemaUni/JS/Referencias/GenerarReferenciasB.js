//$(document).ready(function () {

//});
$(document).ready(function () {
    var lstCuotas;
    var AlumnoId;
    var EsVariable;
    var PagoId = 0;
    var Bandera = 0;
    var tblReferencias;
    function Bloquear(Texto) {
        Bandera = 1;
        //$("#tblContenido *").prop('disabled', true);
        var text = $('#txtBar');
        $('#txtBar').text(Texto);
        $('#divBar').modal('show');    
    }
    function DesBloquear() {
        Bandera = 0;
        //$("#tblContenido *").prop('disabled', false);
        $('#divBar').modal('hide');
    }
    $('#btnBuscar').click(function () {
        if (Bandera == 1) { return false;}
        Bloquear("Buscando Alumno");
        var clave = $('#txtClave').val();
        if (clave != "") {
            $('#slcOfertaEducativa').empty();
            $('#slcConceptos').empty();
            DatosAlumno(clave);
        }
    });
    function ConsutlarAdeudos(AlumnoId, Oferta) {
        $("#slcConceptos").empty();
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarAdeudo',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"'+Oferta +'"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == "Debe") {
                    alertify.alert('Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                    CargarPagosConceptos(AlumnoId, Oferta);
                } else {
                    CargarConceptos($('#slcOfertaEducativa').val());
                }
            }
        });
    }
    function DatosAlumno(Alumnoid) {
        AlumnoId = Alumnoid;
        //var AlumnoId = '9579';
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == null) { return null; }
                $('#lblAlumno').text(data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno);
                $(data.d.lstAlumnoInscrito).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.OfertaEducativa.Descripcion);
                    option.val(this.OfertaEducativa.OfertaEducativaId);
                    $('#slcOfertaEducativa').append(option);
                });
                if (data.d.lstAlumnoInscrito.length == 1) {
                    $('#slcOfertaEducativa').val(data.d.lstAlumnoInscrito[0].OfertaEducativaId);
                    $('#slcOfertaEducativa').change();
                } else {
                    $('#slcOfertaEducativa').change();
                }
            }
        });
    }
    $('#slcOfertaEducativa').change(function () {
        if ($('#slcOfertaEducativa').val() != -1) {
            $('#divBar').modal('show');
            ConsutlarAdeudos(AlumnoId, $('#slcOfertaEducativa').val());
        } else {
            $("#slcConceptos").empty();
        }
    });
    function CargarConceptos(OfertaEducativa) {
        var usuario = $.cookie('userAdmin');
        $("#slcConceptos").empty();
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/Conceptos2",
            data: "{AlumnoId:" + AlumnoId + ",OfertaEducativa:" + OfertaEducativa + ",UsuarioId:" + usuario + "}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                lstCuotas = datos;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.DTOPagoConcepto.Descripcion + " | $" + formato_numero(this.Monto, 2, ".", ","));
                    option.val(this.DTOPagoConcepto.PagoConceptoId);
                    option.attr("data-EsVariable", this.DTOPagoConcepto.EsVairable);
                    $('#slcConceptos').append(option);
                    $('#slcConceptos').change();
                });
                CargarPagosConceptos(AlumnoId, OfertaEducativa);
            }
        });
    }
    function CargarPagosConceptos(Alumno, ofertaEd) {
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarReferenciasCP2',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + Alumno + ',OfertaEducativaId:' + ofertaEd + '}',
            dataType: 'json',
            success: function (Respuesta) {
                ReferenciasTbl(Respuesta);
                var fil = $('#tblReferencias label input');
                fil.removeClass('input-small').addClass('input-large');
                DesBloquear();
            },
            error: function (Respuesta) {
                $('#divBar').modal('hide');
                alertify.alert('Error al cargar datos');
            }
        });
    }
    $('#tblReferencias').on('click', 'a', function () {
        $('#txtComentario').val('');
        PagoId = 0;
        var rowadd = tblReferencias.fnGetData($(this).closest('tr'));
        PagoId = rowadd.PagoId;
        $('#PopComentario').modal('show');
    });
    function ReferenciasTbl(R) {
        if (tblReferencias != null) {
            tblReferencias.fnClearTable();
        }
        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": R.d,
            "bSort": false,
            "aoColumns": [
                { "mDataProp": "DTOCuota.DTOPagoConcepto.Descripcion" },
                { "mDataProp": "Referencia" },
                { "mDataProp": "FechaGeneracionS" },
                { "mDataProp": "objNormal.Monto" },
                { "mDataProp": "objNormal.FechaLimite" },
                {
                    "mDataProp": function (data) {
                        if (data.Cancelable) {
                            return "<a class='btn green'>Cancelar</a>";
                        } else { return "";}
                    }
                }
                //{ "mDataProp": "objRetrasado.Monto" },
                //{ "mDataProp": "objRetrasado.FechaLimite" },
            ],

            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
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
            "language": {
                "lengthMenu": "_MENU_  Registros",
                "paginate": {
                    "previous": "<",
                    "next": ">"
                },
                "search": "Buscar Alumno ",
            },
            "createdRow": function (row, data, dataIndex) {
                if (dataIndex / 2 != 0) {
                    $(row).addClass("BackColor");
                }

                if (data.PagoId <= 2588) {
                    $(row).addClass("bold");
                } 
            }
        });
    }
    $('#slcConceptos').change(function () {
        EsVariable = $('#slcConceptos').find('option:selected');
        var Resp = $(EsVariable[0]).data("esvariable");
        if (Resp == true) {
            $('#btnPopup').click();
        }
    });
    $('#btnMonto').click(function () {
        var Monto = $('#txtMonto').val();
        var idSelec= $('#slcConceptos').val();
        if (Monto.length == 0)
            return false;
        $(lstCuotas).each(function () {
            if (idSelec == this.DTOPagoConcepto.PagoConceptoId) {
                $("#slcConceptos option:selected").text(this.DTOPagoConcepto.Descripcion + " | $" + formato_numero(Monto, 2, ".", ","));
                this.Monto = Monto;
                $('#Monto').modal('hide');
            }
        });
    });
    $('#btnCancelar').click(function () {
        $('#slcConceptos option')[0].selected = true;
    });
    $('#bntClose').click(function () {
        $('#slcConceptos option')[0].selected = true;
    });
    $('#btnGenerar').on('click', function () {

        var Variables;
        var cFech;
        var slcConcepto = $('#slcConceptos');
        slcConcepto = slcConcepto[0].value;
        if (slcConcepto == '-1') { return false; }
        var usuario = $.cookie('userAdmin');
        $(lstCuotas).each(function () {
            var objCuota = this;
            if (objCuota.DTOPagoConcepto.PagoConceptoId == slcConcepto) {
                if (!BuscarTabla(objCuota.DTOPagoConcepto.Descripcion)) {
                    Variables = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId +"',UsuarioId:'"+ usuario+ "'}";
                    alertify.confirm("<p>¿Esta seguro que desea generar la Referecia?<br><br><hr>", function (e) {
                        if (e) {
                            Bloquear("Generando Referencia");

                            var Variables2 = "{OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "'}";
                            $.ajax({
                                type: "POST",
                                url: "../WebServices/WS/General.asmx/ConsultarPagoConcepto2",
                                data: Variables2, // the data in form-encoded format, ie as it would appear on a querystring
                                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                                success: function (data) {
                                    Bloquear("Generando Referencia");
                                    if (objCuota.DTOPagoConcepto.EsVairable) {
                                        var Variables3 = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'"
                                            + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',Monto:'" + objCuota.Monto + "',UsuarioId:'" + usuario + "'}";
                                        GenerarPago2(Variables3);
                                    } else {

                                        GenerarPago(Variables);
                                    }
                                }
                            });


                        }
                    });
                }
                else {
                    Variables = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',UsuarioId:'" + usuario + "'}";
                    var Variables2 = "{OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "'}";


                    $.ajax({
                        type: "POST",
                        url: "../WebServices/WS/General.asmx/ConsultarPagoConcepto2",
                        data: Variables2, // the data in form-encoded format, ie as it would appear on a querystring
                        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            if (data.d.EsMultireferencia == 1) {
                                alertify.confirm("<p>¿Esta seguro que desea generar la Referecia?<br><br><hr>", function (e) {
                                    if (e) {
                                        Bloquear("Generando Referencia");
                                        if (objCuota.DTOPagoConcepto.EsVairable) {
                                            var Variables3 = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'"
                                                + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',Monto:'" + objCuota.Monto + "',UsuarioId:'" + usuario + "'}";
                                            GenerarPago2(Variables3);
                                        } else {

                                            GenerarPago(Variables);
                                        }
                                    }
                                });
                            } else { alertify.alert("El concepto que selecciono ya esta Generado"); }
                        }
                    });

                }
                DesBloquear();
                return false;
            }
        });
    });

    $('#btnGuardar').click(function () {
        var usuario = $.cookie('userAdmin');
        var Texto = $('#txtComentario').val();
        Texto = $.trim(Texto);
        if (Texto.length > 5) {
            $('#PopComentario').modal('hide');
            $('#txtBar').text("Guardando");
            $('#divBar').modal('show');
            $.ajax({
                type: "POST",
                url: "../WebServices/WS/General.asmx/CancelarPago",
                data: "{PagoId:'" + PagoId + "',Comentario:'" + $('#txtComentario').val() + "',UsuarioId:'" + usuario + "'}",
                contentType: "application/json; charset=utf-8", 
                success: function (data) {
                    if (data.d == "Guardado") {
                        $('#btnBuscar').click();
                    }
                    DesBloquear();
                }
            });
        }
        else {
            alertify.alert("Inserte un comentario.");
        }
    });

    //////////////////////////////////////Falta Modificar Codigo (Pasarle al Usuario) 
    function GenerarPago(Cuota) {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Descuentos.asmx/GenerarPagoB",
            data: Cuota, // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d == null) {
                    return false;
                } else {
                    var td = '<tr>';
                    td += '<td>' + data.d.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                    td += '<td>' + data.d.Referencia + '</td>';//Referencia               
                    td += '<td>' + '$' + formato_numero(data.d.Promesa, 2, '.', ',') + '</td>';//Monto
                    td += '<td>' + data.d.objNormal.FechaLimite + '</td>';//Fecha
                    td += '</tr>'
                    
                    $('#txtBar').text("Enviando Correo");
                    MandarMail(data.d.PagoId,td);
                }
            }
        });
    }
    function GenerarPago2(Cuota) {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Descuentos.asmx/GenerarPago2B",
            data: Cuota, // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d == null) {
                    return false;
                } else {
                    var td = '<tr>';
                    td += '<td>' + data.d.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                    td += '<td>' + data.d.Referencia + '</td>';//Referencia               
                    td += '<td>' + '$' + formato_numero(data.d.Promesa, 2, '.', ',') + '</td>';//Monto
                    td += '<td>' + data.d.objNormal.FechaLimite + '</td>';//Fecha
                    td += '</tr>'

                    $('#txtBar').text("Enviando Correo");
                    MandarMail(data.d.PagoId, td);
                }
            }
        });
    }
    function MandarMail(PagoId, td) {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Descuentos.asmx/EnviarMailId",
            data: "{PagoId:'" + PagoId + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                if (data.d == false) {
                    MandarMail(PagoId, td);
                } else {
                    DesBloquear();
                    //$('#tblReferencias').append(td);
                    $('#btnBuscar').click();
                    alertify.alert("Referencia Generada");
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
    function BuscarTabla(Descripcion) {
        var respu = false;
        $('#tblReferencias tbody tr').each(function () {
            var td = this.childNodes[0];
            if (td.innerHTML == Descripcion) {
                respu = true;
                return false;
            }
        });
        return respu;
    }
    $('#btnImprimir').on('click', function () {
        $('#btnImprimir').hide();
        var EscaleElement = $('#divContenido');
        //PrintPDF();
        //var Body = "<canvas>" + EscaleElement[0].innerHTML+ "</Canvas>";

        html2canvas(EscaleElement, {
            onrendered: function (canvasq) {
                //document.body.appendChild(canvasq);
                var img = canvasq.toDataURL("image/png")
                var imagen = new Image;;
                imagen.src = img;
                var myWindow = window.open('');
                var canvas = $('#myCanvas')[0], pic = imagen;
                HDPICanvas.drawImage({
                    canvas: canvas,
                    image: imagen,
                    desx: 10,
                    desy: 10,
                    desw: 300,
                    desh: 90
                });

                myWindow.document.write(imagen.outerHTML);
                myWindow.focus();
                myWindow.print();
                myWindow.close();
                //window.open(img);
                $('#btnImprimir').show();
            }
        });

    });
});