$(document).ready(function () {
    // $.cookie('user', 7493, { expires: 1 });
    var settings = {
        theme: "ruby col-md-12",
        sticky: false,
        horizontalEdge: "bottom",
        verticalEdge: "right",
        heading: "Alerta",
        life: 30000,
        icon: "cog-gear"
    };
    var lstCuotas;
    //DatosAlumno();
    DatosAlumno();
    
    function DatosAlumno() {
        IndexFn.Block(true);
        var AlumnoId = $.cookie('user');
        //var AlumnoId = '9579';
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumno',
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
                }
                IndexFn.Block(false);
            }
        });
    }
    function ConsutlarAdeudos(ofertaEducativa) {
        var AlumnoId = $.cookie('user');
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAdeudo',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + ofertaEducativa + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == "Debe") {
                    alertify.alert('Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                    CargarPagosConceptos(AlumnoId);
                } else {
                    CargarConceptos(ofertaEducativa );

                }
            }
        });
    }
    $('#slcOfertaEducativa').change(function () {
        IndexFn.Block(true);
        $("#slcConceptos").empty();
        ConsutlarAdeudos($('#slcOfertaEducativa').val());
    });
    function CargarConceptos(OfertaEducativa) {
        var AlumnoId = $.cookie('user');
        $("#slcConceptos").empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/Conceptos",
            data: "{AlumnoId:"+AlumnoId+",OfertaEducativa:"+OfertaEducativa+"}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                lstCuotas = datos;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.DTOPagoConcepto.Descripcion +" | $"+formato_numero(this.Monto,2,".",","));
                    option.val(this.DTOPagoConcepto.PagoConceptoId);
                    $('#slcConceptos').append(option);
                });
                CargarPagosConceptos(AlumnoId);
            }
        });
    }
    function CargarPagosConceptos(Alumno) {
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarReferenciasCP',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + Alumno + '}',
            dataType: 'json',
            success: function (Respuesta) {
                ReferenciasTbl(Respuesta);
                var fil = $('#tblReferencias label input');
                fil.removeClass('input-small').addClass('input-large');
                IndexFn.Block(false);
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
                IndexFn.Block(false);
            }
        });
    }
    function ReferenciasTbl(R) {
        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": R.d,
            "bSort": false,
            "aoColumns": [
                { "mDataProp": "DTOCuota.DTOPagoConcepto.Descripcion" },
                { "mDataProp": "Referencia" },
                { "mDataProp": "objNormal.Monto" },
                { "mDataProp": "objNormal.FechaLimite" }
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
                row.childNodes[3].style.textAlign = 'center';
                if (dataIndex / 2 != 0) {
                    $(row).addClass("BackColor");
                }

                if (data.PagoId <= 2588) {
                    $(row).addClass("bold");
                }
            }
        });
    }
    $('#btnGenerar').on('click', function () {
        IndexFn.Block(true);
        var Variables;
        var cFech;
        if ($('#slcConceptos').val() == '-1') { return false; }
        $(lstCuotas).each(function () {
            if (this.DTOPagoConcepto.PagoConceptoId == $('#slcConceptos').val()) {
                if (!BuscarTabla(this.DTOPagoConcepto.Descripcion)) {
                    Variables = "{AlumnoId:'" + $.cookie('user') + "',OfertaEducativaId:'" + this.OfertaEducativaId + "',PagoConceptoId:'" + this.PagoConceptoId + "',CuotaId:'" + this.CuotaId + "'}";
                    GenerarPago(Variables);
                }
                else {
                    Variables = "{AlumnoId:'" + $.cookie('user') + "',OfertaEducativaId:'" + this.OfertaEducativaId + "',PagoConceptoId:'" + this.PagoConceptoId + "',CuotaId:'" + this.CuotaId + "'}";
                    var Variables2 = "{OfertaEducativaId:'" + this.OfertaEducativaId + "',PagoConceptoId:'" + this.PagoConceptoId + "'}";
                    $.ajax({
                        type: "POST",
                        url: "WS/General.asmx/ConsultarPagoConcepto",
                        data: Variables2, // the data in form-encoded format, ie as it would appear on a querystring
                        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            if (data.d.EsMultireferencia == 1) {
                                GenerarPago(Variables);
                            } else { alertify.alert("El concepto que selecciono ya esta Generado"); }
                        }
                    });
                }
                IndexFn.Block(false);
                return false;
            }
        });
    });

    function GenerarPago(Cuota) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/GenerarPago",
            data: Cuota, // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var td = '<tr>';
                td += '<td>' + data.d.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                td += '<td>' + data.d.Referencia + '</td>';//Referencia               
                td += '<td>' + '$' + formato_numero(data.d.Promesa, 2, '.', ',') + '</td>';//Monto
                td += '<td>' + data.d.objNormal.FechaLimite + '</td>';//Fecha
                td += '</tr>'
                $('#tblReferencias').append(td);
                IndexFn.Block(false);

                Alerta();
            }
        });
    }

    function Alerta() {
        
        var ahref = "<a class='btn blue' href=javascript:window.open('../Inscritos/Pago/ListaConceptos.html'," + "'Tramites'" + "," + "'width=800,height=450'" + ");>click aqui</a>";
        $.notific8('zindex', 11500);
        $.notific8($.trim("Los pagos se cancelaran automáticamente después de 15 días. </hr> Para mas información " + ahref ), settings);

        var not8 = $('.jquery-notific8-container').find('.jquery-notific8-heading');
        not8 = $(not8).parent().parent();
        //not8 = $(not8)[0];
        $(not8).addClass('col-lg-4 col-md-4 col-xs-3');
        //not8.style.
        var $bodnot8 = $(not8[0].childNodes[0]);
        $bodnot8 = $bodnot8[0];
        $bodnot8.style.width = "initial !important";
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
        var respu=false;
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
                var canvas = $('#myCanvas')[0],pic=imagen;
                HDPICanvas.drawImage({
                    canvas: canvas,
                    image: imagen,
                    desx: 10,
                    desy: 10,
                    desw: 300,
                    desh: 90});

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