$(document).ready(function () {
    
    var lstCuotas;
    //DatosAlumno();
    DatosAlumno();
    
    function DatosAlumno() {
        $('#PopLoad').modal('show');

        //var AlumnoId = '9579';
        $.ajax({
            url: 'Api/Alumno/ConsultarAlumno/'+localStorage.getItem("user"),
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data == null) { return null; }
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
                $('#PopLoad').modal('hide');
            }
        });
    }
    function ConsutlarAdeudos(ofertaEducativa) {
        var AlumnoId = localStorage.getItem("user");
        $.ajax({
            url: 'Api/Alumno/ConsultarAdeudo/' + localStorage.getItem("user") + "/" + ofertaEducativa,
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (data == "Debe") {
                    alertify.alert('Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                    CargarPagosConceptos(localStorage.getItem("user"));
                } else {
                    CargarConceptos(ofertaEducativa);

                }
            }
        });
    }
    $('#slcOfertaEducativa').change(function () {
        $('#PopLoad').modal('show');
        $("#slcConceptos").empty();
        ConsutlarAdeudos($('#slcOfertaEducativa').val());
    });
    function CargarConceptos(OfertaEducativa) {
        $("#slcConceptos").empty();
        $.ajax({
            type: "Get",
            url: "Api/General/Conceptos/" + localStorage.getItem("user") + "/" + OfertaEducativa,
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data;
                lstCuotas = datos;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.DTOPagoConcepto.Descripcion +" | $"+formato_numero(this.Monto,2,".",","));
                    option.val(this.DTOPagoConcepto.PagoConceptoId);
                    $('#slcConceptos').append(option);
                });
                CargarPagosConceptos(localStorage.getItem("user"));
            }
        });
    }
    function CargarPagosConceptos(Alumno) {
        $.ajax({
            url: 'Api/Alumno/ConsultarReferenciasCP/' + Alumno,
            type: 'Get',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (Respuesta) {
                ReferenciasTbl(Respuesta);
                var fil = $('#tblReferencias label input');
                fil.removeClass('input-small').addClass('input-large');
                $('#PopLoad').modal('hide');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
                $('#PopLoad').modal('hide');
            }
        });
    }
    function ReferenciasTbl(R) {
        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": R,
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
        $('#PopLoad').modal('show');
        var Variables = "";
        var cFech;
        if ($('#slcConceptos').val() == '-1') { return false; }
        $(lstCuotas).each(function () {
            if (this.DTOPagoConcepto.PagoConceptoId == $('#slcConceptos').val()) {
                if (!BuscarTabla(this.DTOPagoConcepto.Descripcion)) {
                    Variables = {
                        AlumnoId: localStorage.getItem("user"),
                        OfertaEducativaId: this.OfertaEducativaId,
                        PagoConceptoId: this.PagoConceptoId,
                        CuotaId: this.CuotaId
                    };
                    GenerarPago(Variables);
                }
                else {
                    Variables = {
                        AlumnoId: localStorage.getItem("user"),
                        OfertaEducativaId: this.OfertaEducativaId,
                        PagoConceptoId: this.PagoConceptoId,
                        CuotaId: this.CuotaId
                    };

                    $.ajax({
                        type: "Get",
                        url: "Api/General/ConsultarPagoConcepto/" + this.OfertaEducativaId + "/"+ PagoConceptoId,
                        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                        success: function (data) {
                            if (data.EsMultireferencia == 1) {
                                GenerarPago(Variables);
                            } else {
                                alertify.alert("El concepto que selecciono ya esta Generado");
                                $('#PopLoad').modal('hide');
                            }
                        }
                    });
                }
                
            }
        });
        if (Variables.length === 0) {
            $('#PopLoad').modal('hide');
        }
    });
    function GenerarPago(Cuota) {
        $.ajax({
            type: "POST",
            url: "Api/Descuentos/GenerarPago",
            data: JSON.stringify(Cuota), 
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var td = '<tr>';
                td += '<td>' + data.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                td += '<td>' + data.Referencia + '</td>';//Referencia               
                td += '<td>' + '$' + formato_numero(data.Promesa, 2, '.', ',') + '</td>';//Monto
                td += '<td>' + data.objNormal.FechaLimite + '</td>';//Fecha
                td += '</tr>'
                $('#tblReferencias').append(td);
                $('#PopLoad').modal('hide');

                Alerta();
            }
        });
    }
    function Alerta() {
        
        var ahref = "Los pagos se cancelaran automáticamente después de 15 días. </hr> Para mas información " +
            "<a class='btn blue' href=javascript:window.open('Views/Pago/ListaConceptos.html'," +
            "'Tramites'" + "," +
            "'width=800,height=450'" + ");>click aqui</a>";
        var delay = alertify.get('notifier', 'delay');
        alertify.set('notifier', 'delay', 100);
        alertify.warning(ahref);
        alertify.set('notifier', 'delay', delay);
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