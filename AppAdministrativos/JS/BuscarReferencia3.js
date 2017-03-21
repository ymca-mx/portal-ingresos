$(document).ready(function () {
    var tblReferencias;
    var tblPagados;
    var tblAnticipado;
    var tblNormal;
    var tblRetrasado;
    var Total = 0;
    var AlumnoId;

    AlumnoId = $.cookie('user');       
    DatosAlumno();
    Cargar();

    function DatosAlumno() {
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == null) { return null; }
                var Fecha = null;
                var Oferta = null;
                $('#lblAlumno').text(data.d.AlumnoId + " " + data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno);
                if (data.d.lstAlumnoInscrito.length > 1) {
                    $(data.d.lstAlumnoInscrito).each(function () {
                        if (this.OfertaEducativa.OfertaEducativaTipoId != 4) {
                            if (Fecha == null) {
                                Fecha = new Date(parseInt(this.FechaInscripcion.slice(6)));
                                Fecha = new Date(Fecha);
                                Oferta = this.OfertaEducativa.Descripcion;
                            } else {
                                var fecha2 = new Date(parseInt(this.FechaInscripcion.slice(6)));
                                fecha2 = new Date(fecha2);
                                Oferta = fecha2 > Fecha ? this.OfertaEducativa.Descripcion : Oferta;
                                Fecha = fecha2 > Fecha ? fecha2 : Fecha;
                            }
                        }
                    });
                    if (Oferta == null) {
                        $('#lblOferta').text(data.d.lstAlumnoInscrito[0].OfertaEducativa.Descripcion);
                    }
                    else { $('#lblOferta').text(Oferta); }
                } else {
                    $('#lblOferta').text(data.d.lstAlumnoInscrito[0].OfertaEducativa.Descripcion);
                }
            }
        });
    }
    function Cargar() {
        //var AlumnoId = '9579';
        $.ajax({
            url: '../WebServices/WS//Alumno.asmx/ConsultarReferencias2',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + AlumnoId + ',NoPagados:' + "true" + '}',
            dataType: 'json',
            success: function (Respuesta) {
                ReferenciasTbl(Respuesta);
                var fil = $('#tblReferencias label input');
                fil.removeClass('input-small').addClass('input-large');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
            }
        });
    }
    function CargaPagados() {
        $.ajax({
            url: '../WebServices/WS//Alumno.asmx/ConsultarReferenciasPagadasCortas',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + AlumnoId + '}',
            dataType: 'json',
            success: function (Respuesta) {
                ReferenciasTblPagadas(Respuesta);
                $('#lblTotal').text("");
                $('#lblTotal').text(Total);
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
            }
        });
    }
    function ReferenciasTblPagadas(ER) {
        Total = 0;
        tblPagados = $('#tblPagadas').dataTable({
            "aaData": ER.d,
            "bSort": false,
            "aoColumns": [
                { "mDataProp": "Caja" },
                { "mDataProp": "FechaPago" },
                { "mDataProp": "ReferenciaId" },
                { "mDataProp": "MontoPagado" },
                { "mDataProp": "MontoReferencia" },
                { "mDataProp": "Saldo" },

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
                row.childNodes[0].style.textAlign = 'center';
                row.childNodes[1].style.textAlign = 'center';
                row.childNodes[2].style.textAlign = 'center';
                row.childNodes[3].style.textAlign = 'right';
                row.childNodes[4].style.textAlign = 'right';
                row.childNodes[5].style.textAlign = 'right';
                if (dataIndex / 2 != 0) {
                    $(row).addClass("BackColor");
                }

                //var fecha = new Date(parseInt(data.FechaPago.slice(6)));
                //fecha = new Date(fecha);
                //var fechas = parseInt(fecha.getDate() + 1) < 10 ? +'0' + String(parseInt(fecha.getDate() + 1)) : String(parseInt(fecha.getDate() + 1));
                //fechas += '/';
                //fechas += parseInt(fecha.getMonth() + 1) < 10 ? +'0' + String(parseInt(fecha.getMonth() + 1)) : String(parseInt(fecha.getMonth() + 1));

                //fechas += '/' + fecha.getFullYear();
                //var cols = row.childNodes[1];
                //cols.innerText = fechas;
                //data.FechaAlta = fechas;

                //Total += data.SaldoD;
            }
        });
    }
    function ReferenciasTbl(R) {
        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": R.d,
            "bSort": false,
            "aoColumns": [
                { "mDataProp": "DTOCuota.PeridoAnio" },
                { "mDataProp": "DTOCuota.DTOPagoConcepto.Descripcion" },
                { "mDataProp": "Referencia" },
                { "mDataProp": "lstPagoDescuento[0].DTOAlumnDes.SMonto", },
                { "mDataProp": "objAnticipado1.Monto" },
                { "mDataProp": "objAnticipado1.FechaLimite" },
                { "mDataProp": "objAnticipado2.Monto" },
                { "mDataProp": "objAnticipado2.FechaLimite" },
                { "mDataProp": "objNormal.Monto" },
                { "mDataProp": "objNormal.FechaLimite" },
                { "mDataProp": "Restante" }
                // { "mDataProp": "objNormal.Recargo" },
                //{ "mDataProp": "objNormal.Total" }
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
                row.childNodes[0].style.textAlign = 'center';
                row.childNodes[2].style.textAlign = 'center';
                row.childNodes[3].style.textAlign = 'right';
                row.childNodes[4].style.textAlign = 'right';
                row.childNodes[6].style.textAlign = 'right';
                row.childNodes[8].style.textAlign = 'right';
                row.childNodes[5].style.textAlign = 'center';
                row.childNodes[7].style.textAlign = 'center';
                row.childNodes[9].style.textAlign = 'center';
                row.childNodes[10].style.textAlign = 'center';
                //row.childNodes[10].style.textAlign = 'right';
                //row.childNodes[11].style.textAlign = 'right';
                if (dataIndex / 2 != 0) {
                    $(row).addClass("BackColor");
                }

                //if (data.PagoId <= 2588 ) {
                //    row.style.visibility = 'hidden';
                //    $('#txtAdeudo').text('Para todos los adeudos anteriores a 2016 la referencia bancaria que debes usar es "' + data.Referencia + '"');
                //    //$(row).addClass("bold");
                //}

                if (data.EstatusId == 4 || data.EstatusId == 14) {
                    row.style.color = "#FFFFFF";
                    row.style.backgroundColor = '#337ab7';
                }
            }
        });
        //CargaPagados();
    }
    $('#btnImprimir').on('click', function () {
        $('#btnImprimir').hide();
        $('#divBuscar').hide();
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
                myWindow.document.write(imagen.outerHTML);
                myWindow.focus();
                myWindow.print();
                myWindow.close();
                //window.open(img);
                $('#btnImprimir').show();
                $('#divBuscar').show();
            }
        });

    });
    $("#txtClave").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which == 13) { $('#btnBuscar').click(); }
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
});