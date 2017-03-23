$(document).ready(function () {
    var tblReferencias;
    var tblAnticipado;
    var tblNormal;
    var tblRetrasado;
    var AlumnoId;
    $('#btnBuscar').on('click', function () {
        AlumnoId = $('#txtClave').val();
        if (AlumnoId == "") { return false; }
        DatosAlumno();
        Cargar();
    });
    $("#txtClave").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which == 13) { $('#btnBuscar').click(); }
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });
    function DatosAlumno() {
        //var AlumnoId = '9579';
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == null) { return null; }
                var Fecha = null;
                var Oferta = null;
                $('#lblAlumno').text(AlumnoId + " " + data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno);
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
        $.ajax({
            url: 'WS//Alumno.asmx/ConsultarReferenciasPagadas',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + AlumnoId + '}',
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
    function ReferenciasTbl(R) {
        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": R.d,
            "bSort": false,
            "aoColumns": [
                { "mDataProp": "EsReferencia" },
                { "mDataProp": "FechaPago" },
                { "mDataProp": "PagoS" },
                { "mDataProp": "Pago1.Referencia", },
                { "mDataProp": "Pago1.DTOCuota.DTOPagoConcepto.Descripcion" },
                { "mDataProp": "Pago1.Referencia" }
            ],

            "lengthMenu": [[10, 20, 40, -1], [10, 20, 40, 'Todos']],
            "searching": false,
            "ordering": false,
            "oder":[[2,"desc"]],
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
                var fecha = new Date(parseInt(data.FechaPago.slice(6)));
                fecha = new Date(fecha);
                var fechas = parseInt(fecha.getDate() + 1) < 10 ? +'0' + String(parseInt(fecha.getDate() + 1)) : String(parseInt(fecha.getDate() + 1));
                fechas += '/';
                fechas += parseInt(fecha.getMonth() + 1) < 10 ? +'0' + String(parseInt(fecha.getMonth() + 1)) : String(parseInt(fecha.getMonth() + 1));

                fechas += '/' + fecha.getFullYear();
                var cols = row.childNodes[1];
                cols.innerText = fechas;
                data.FechaPago = fechas;

                row.childNodes[0].innerText = data.EsReferencia == 1 ? "Scotiabank" : "Caja Universidad";
                row.childNodes[5].innerText = "Aplicada";
                row.childNodes[0].style.textAlign = 'center';
                row.childNodes[1].style.textAlign = 'center';
                row.childNodes[2].style.textAlign = 'right';
                row.childNodes[3].style.textAlign = 'Center';
                row.childNodes[4].style.textAlign = 'center';
                row.childNodes[5].style.textAlign = 'center';
                if (dataIndex / 2 != 0) {
                    $(row).addClass("BackColor");
                }
            }
        });
    }
});