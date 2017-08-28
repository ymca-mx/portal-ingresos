$(document).ready(function () {
    var tblSaldos, tblDetalle;

    $('#btnBuscar').on('click', function () {

        CargarReporteSaldos();
       
    });

    function CargarReporteSaldos() {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarReporteSaldos",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }
                var saldos = data.d;


                tblSaldos = $("#AntiguedadSaldos").DataTable({
                    "aaData": saldos,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "Saldo" },
                        {
                            "mRender": function (Data) {
                                return "<a class='btn green' name ='btnDetalle'>Detalle</a>";
                            }
                        }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": false,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "colReorder": false,
                    "oSearch": { "bSmart": false },
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar ",
                    },
                    "order": [[0, "asc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[2].style.textAlign = 'center';
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'center';
                    },
                    "fnDrawCallback": function (oSettings) {
                        var registros = oSettings.aiDisplay.length;
                        $('#lbAntiguedadSaldos').text(registros);
                    }

                });

                var fil = $('#AntiguedadSaldos_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                
                $("#btnExportar").show();
                $('#Load').modal('hide');
            }//success

        });//$.ajax
    }


    $('#AntiguedadSaldos').on('click', 'a', function () {

        var rowData = tblSaldos.row($(this).closest('tr')).data();
        var alumnoid = rowData.AlumnoId;
        $('#lblNombre').text(rowData.Nombre);

        detalle(alumnoid);
       
    });

    function detalle(alumnoId)
    {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarReporteSaldosDetalle",
            data: "{alumnoId:" + alumnoId + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }
                var detalle = data.d;

                tblDetalle = $("#AntiguedadSaldosDetalle").DataTable({
                    "aaData": detalle,
                    "aoColumns": [
                        { "mDataProp": "Periodo" },
                        { "mDataProp": "Saldo" }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": false,
                    "ordering": true,
                    "async": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "bLengthChange": true,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "colReorder": false,
                    "oSearch": { "bSmart": false },
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar ",
                    },
                    "order": [[0, "asc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[1].style.textAlign = 'center';
                    }

                });

                $('#PopSaldoDetalle').modal('show');
                $('#Load').modal('hide');
            }//success

        });//$.ajax

       
    }


    $('#divContenido').submit(function () {
        //do your stuff
        return false;
    });


    ////exportar//////

    $('#btnExportar').on('click', function () {
        exportarexcel("AntiguedadSaldos", "Antiguedad de saldos ");
    });

    function exportarexcel(Tabla, nombre) {

        var table1 = $('#' + Tabla).dataTable().api();
        var data1 = table1.data();
        var data2 = [];
        var hd;

        $(data1).each(function () {
            var ojb2 = {
                "Alumno Id": this.AlumnoId,
                "Nombre": this.Nombre,
                "Perido": this.Descripcion,
                "Total": this.Saldo,
            };
            data2.push(ojb2);
        });
        hd = ["Alumno Id", "Nombre", "Perido", "Total"];


        var ws = XLSX.utils.json_to_sheet(data2, {
            header: hd
        });

        var ws_name = nombre;

        function Workbook() {
            if (!(this instanceof Workbook)) return new Workbook();
            this.SheetNames = [];
            this.Sheets = {};
        }

        var wb = new Workbook();

        /* add worksheet to workbook */
        wb.SheetNames.push(ws_name);

        wb.Sheets[ws_name] = ws;

        var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });


        function s2ab(s) {
            var buf = new ArrayBuffer(s.length);
            var view = new Uint8Array(buf);
            for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
            return buf;
        }

        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), nombre + ".xlsx");
    }


});


