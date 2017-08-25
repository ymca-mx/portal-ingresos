$(function init() {
    $('#divContenido').submit(function () {
        //do your stuff
        return false;
    });

    var Funciones = {
        init: function () {
            Funciones.TraerPeriodos();
            Funciones.ArmarFechas();
            $('#xportxlsx').hide();
        },
        tblDatos: null,
        tblDatos2: null,
        ArmarFechas: function () {
            var fecha = new Date();
            var fechaLim = new Date();
            fechaLim.setDate(fecha.getDate() - 1);

            var mes = fecha.getMonth();
            var mes2 = fechaLim.getMonth();
            
            var dia = fecha.getDate();

            mes = mes < 9 ? (mes + 1) : mes + 1;
            mes = mes < 10 ? '0' + mes : mes;

            mes2 = mes2 < 9 ? (mes2 + 1) : mes2 + 1;
            mes2 = mes2 < 10 ? '0' + mes2 : mes2;

            dia = dia < 10 ? '0' + dia : dia;

            var fini = '01/01/' + fecha.getFullYear() ;
            var ffin = dia + '/' + mes + '/' + fecha.getFullYear();

            var ffinEnd = fechaLim.getDate() + '/' + mes2 + '/' + fechaLim.getFullYear();

            console.log(ffinEnd);

            $('#calInicial').val(fini);
            $('#calFinal').val(ffin);

            $('.date-picker').each(function () {
                if (this.id === 'calInicial') {
                    $(this).datepicker({
                        rtl: Metronic.isRTL(),
                        orientation: "left",
                        language: 'es',
                        format: 'dd/mm/yyyy',
                        autoclose: true,                                                
                        endDate: ffinEnd
                    });
                } else {
                    $(this).datepicker({
                        rtl: Metronic.isRTL(),
                        orientation: "left",
                        language: 'es',
                        format: 'dd/mm/yyyy',
                        autoclose: true,    
                    });
                }
            });

            //$('#calInicial .date-picker').datepicker({
            //    rtl: Metronic.isRTL(),
            //    orientation: "left",
            //    language: 'es',
            //    format: "dd/mm/yyyy",
            //    autoclose: true,
            //    endDate: ffinEnd
            //});

            //$('#calFinal .date-picker').datepicker({
            //    rtl: Metronic.isRTL(),
            //    orientation: "left",
            //    language: 'es',
            //    format: "dd/mm/yyyy",
            //    autoclose: true,
            //});
            
        },
        TraerPeriodos: function () {
            $('#Load').modal('show');
            $.ajax({
                url: 'WS/Reporte.asmx/CargarCuatrimestreHistorico',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{}',
                dataType: 'json',
                success: function (Respuesta) {
                    var option = $(document.createElement('option'));
                    option.text('--Seleccionar--');
                    $(Respuesta.d.periodos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.descripcion);
                        option.attr("data-Anio", this.anio);
                        option.attr("data-PeriodoId", this.periodoId);
                        option.val(this.anio + '' + this.PeriodoId);

                        $("#slcPeriodos").append(option);
                    });
                    $('#Load').modal('hide');
                }
            });
        },
        TraerDatos: function () {
            $('#Load').modal('show');
            var datos = {
                Anio: $('#slcPeriodos option:selected').data("anio"),
                PeriodoId: $('#slcPeriodos option:selected').data("periodoid"),
                FechaInicial: $('#calInicial').val(),
                FechaFinal: $('#calFinal').val()
            };
            datos.FechaFinal = datos.FechaFinal.replace('-', '/').replace('-', '/');
            datos.FechaInicial = datos.FechaInicial.replace('-', '/').replace('-', '/');

            datos = JSON.stringify(datos);
            $.ajax({
                url: 'WS/Reporte.asmx/CarteraVencida',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: datos,
                dataType: 'json',
                success: function (Respuesta) {
                    if (Respuesta.d !== null) {
                        if (Respuesta.d.length > 0) {
                            $('#xportxlsx').show()
                        } else { $('#xportxlsx').hide()}
                            Funciones.PintarTabla(Respuesta.d);
                        
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Intente nuevamente mas tarde");
                    }
                }
            });
        },
        PintarTabla: function (tabla) {

            Funciones.tblDatos = $('#tblDatos').dataTable({
                "aaData": tabla,
                "aoColumns": [
                    { "mDataProp": "Alumno" },
                    { "mDataProp": "FechaPagoS" },
                    { "mDataProp": "Tipo_de_pago" },
                    { "mDataProp": "Concepto" },
                    { "mDataProp": "Pago" },
                    { "mDataProp": "Restante" },
                ],
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": true,
                "ordering": true,
                "async": true,
                "bDestroy": true,
                "bPaginate": true,
                "bLengthChange": true,
                "bFilter": true,
                "bInfo": true,
                "pageLength": 20,
                "bAutoWidth": false,
                "asStripClasses": null,
                "language": {
                    "lengthMenu": "_MENU_  Registros",
                    "paginate": {
                        "previous": "<",
                        "next": ">"
                    },
                    "search": "Buscar Alumno "
                },
                "order": [[1, "desc"]]
            });

            if ($('#tblDatos').find('tfoot').length > 0) {
                $('#tblDatos').find('tfoot')[0].remove();
            }

            if (tabla.length > 0) {
                var tabla2 = $('#tblDatos').DataTable();

                var total1 = tabla2.column(4)
                    .data()
                    .reduce(function (a, b) {
                        return a + b
                    }, 0);
                var total2 = tabla2.column(5)
                    .data()
                    .reduce(function (a, b) {
                        return a + b;
                    }, 0);
                total1 = parseFloat(total1).toFixed(4);
                total2 = parseFloat(total2).toFixed(4);

                var tfoot = '<tfoot><tr><th></th><th></th><th></th><th>Totales: </th><th>' + total1 + ' </th><th>' + total2 + ' </th> </tr></tfoot>';
                              
                $('#tblDatos').append(tfoot);
            }

            var fil = $('#tblDatos_filter label input');
            fil.removeClass('input-small').addClass('input-large');

            $('#Load').modal('hide');
        },
        Exportar: function () {

            //var data = Funciones.tblDatos.data();
            var table1 = $('#tblDatos').dataTable().api();
            var data1 = table1.data();
            var data2 = [];
            //var data3 = [];
            var pagototal=0;
            var restantetotal=0;
            $(data1).each(function () {
                var ojb2 = {
                    "Alumno": this.Alumno,
                    "Fecha de Pago": this.FechaPagoS,
                    "Tipo dePago": this.Tipo_de_pago,
                    "Concepto": this.Concepto,
                    "Pagado": this.Pago,
                    "Por Pagar": this.Restante
                };
                pagototal += this.Pago;
                restantetotal += this.Restante;                

                data2.push(ojb2);
            });

            pagototal = parseFloat(pagototal).toFixed(4);
            restantetotal = parseFloat(restantetotal).toFixed(4);

            var ojb3 = {
                "Alumno": "",
                "Fecha de Pago": "",
                "Tipo dePago": "",
                "Concepto": "Total",
                "Pagado": pagototal,
                "Por Pagar": restantetotal
            };

            data2.push(ojb3);
            //$(data1).each(function () {
            //    var ojb2 = {
            //        "Alumno": this.Alumno,
            //        "Fecha de Pago": this.FechaPagoS,
            //        "Tipo dePago": this.Tipo_de_pago,
            //        "Concepto": this.Concepto,
            //        "Pagado": this.Pago,
            //        "Por Pagar": this.Restante
            //    };
            //    data3.push(ojb2);
            //});

            var ws = XLSX.utils.json_to_sheet(data2, { header: ["Alumno", "Fecha de Pago", "Tipo dePago", "Concepto", "Pagado", "Por Pagar"] });
            //var ws1 = XLSX.utils.json_to_sheet(data3, { header: ["Alumno", "Fecha de Pago", "Tipo dePago", "Concepto", "Pagado", "Por Pagar"] });

            var ws_name = $("#slcPeriodos option:selected").text();
            //var ws_name1 = "Septiembre - Diciembre 2017"

            function Workbook() {
                if (!(this instanceof Workbook)) return new Workbook();
                this.SheetNames = [];
                this.Sheets = {};
            }

            var wb = new Workbook();

            /* add worksheet to workbook */
            wb.SheetNames.push(ws_name);
            //wb.SheetNames.push(ws_name1);

            wb.Sheets[ws_name] = ws;
            //wb.Sheets[ws_name1] = ws1;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });
            

            function s2ab(s) {
                var buf = new ArrayBuffer(s.length);
                var view = new Uint8Array(buf);
                for (var i = 0; i !== s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                return buf;
            }

            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), "CarteraVencida.xlsx");
        },
        BtnExportar: function () {
            Funciones.Exportar();
        }
    };

    Funciones.init();
    $('#btnBuscar').on('click', Funciones.TraerDatos);
    $('#btnExportar').on('click', Funciones.BtnExportar);
});