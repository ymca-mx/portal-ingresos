$(function () {
    var tblSaldosPeriodos,
        tblSaldos,
        tblDetalle,
        periodos,
        total,
        ColumnasDetalle;

    var ReporteFn = {
        init() {
            this.CagarReporteSaldos();
            $('#divContenido').submit(function () {
                //do your stuff
                return false;
            });

            ////exportar//////
            $('#btnExportar').on('click', function () {
                ReporteFn.exportarexcel("Antiguedad de saldos ");
            });

            $('#slcTipoOferta').on('change', this.TipoOfertaChange);
            $('#slcOfertaEducativa').on('change', this.OfertaChange);
        },
        OfertaChange() {
            var TipoO = parseInt($('#slcTipoOferta').val());
            var Oferta = parseInt($('#slcOfertaEducativa').val());
            
            if (TipoO !== -1 && Oferta !== -1) {
                var lstAlumnosOFerta = [];

                ReporteFn.Todos.find(x => x.Ofer);


            } else if (TipoO === -1 && Oferta === -1){
                ReporteFn.GenerarAlumnoDetalle(ReporteFn.Todos);
            }
        },
        TipoOfertaChange() {
            $("#slcOfertaEducativa").empty();
            var TipoOf = parseInt($('#slcTipoOferta').val());
            
            var Ofertas = ReporteFn.TipoOfertas.find(function (TipoOferta) {
                return TipoOferta.OfertaEducativaTipoId === TipoOf;
            });

            $(Ofertas.OfertasEducativas).each(function () {
                var option = $(document.createElement('option'));

                option.text(this.Descripcion);
                option.val(this.OfertaEducativaId);

                $("#slcOfertaEducativa").append(option);
            });

            $("#slcOfertaEducativa").change();
        },
        GetDetalleAlumno(OfertaEducativaId) {
            IndexFn.Block(true);
            IndexFn.Api("Reporte/ReporteSaldosAlumnos/" + OfertaEducativaId, "GET", "")
                .done(function (data) {
                    ReporteFn.GenerarAlumnoDetalle(data.Alumnos);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                });
        },
        CargarTipoOfertas() {
            if (ReporteFn.TipoOfertas.length > 0) {
                $(ReporteFn.TipoOfertas).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.OfertaEducativaTipoId);

                    $("#slcTipoOferta").append(option);
                });
                $("#slcTipoOferta").change();
            }
        },
        TipoOfertas: {},
        Todos: {},
        CagarReporteSaldos() {
            IndexFn.Block(true);
            IndexFn.Api("Reporte/CargarReporteSaldos", "GET", "")
                .done(function (data) {
                    if (data === null) {
                        IndexFn.Block(false);
                        return false;
                    }

                    periodos = data.Periodos;

                    ReporteFn.TipoOfertas = [];
                    ReporteFn.TipoOfertas = data.TipoOfertas;
                    
                    ReporteFn.Todos = [];
                    ReporteFn.Todos = data.Alumnos;

                    var columns1 = [{ "mDataProp": "Descripcion" }];

                    var columns2 = [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                    ];

                    $("#theadPeridos").remove();
                    $("#theadAlumnos").remove();

                    var html1 = '<thead id="theadPeridos"><tr role="row"><th style="text-align:center"> Carrera </th>';

                    var html2 = '<thead id="theadAlumnos"><tr role="row"><th style="text-align:center"> Alumno </th><th style="text-align:center"> Nombre </th>';


                    $(periodos).each(function (i, d) {
                        html1 = html1 + " <th style='text- align:center'>" + d.DescripcionCorta + " </th>";
                        html2 = html2 + " <th style='text- align:center'>" + d.DescripcionCorta + " </th>";

                        columns1.push({
                            "mDataProp": "Periodos",
                            "mRender": function (data) {
                                var valor = "";

                                $(data).each(function (i1, d1) {
                                    if (d1.Periodo == d.Descripcion) valor = d1.Saldo
                                });
                                return valor;
                            }
                        });

                        columns2.push({
                            "mDataProp": "Detalle",
                            "mRender": function (data) {
                                var valor = "";

                                $(data).each(function (i1, d1) {
                                    if (d1.Periodo == d.Descripcion) valor = d1.Saldo
                                });
                                return valor;
                            }
                        });

                    });

                    html1 += '<th style="text-align:center"> Saldo Total </th></tr></thead>';
                    html2 += '<th style="text-align:center"> Saldo Total </th></tr></thead>';

                    columns1.push({ "mDataProp": "SaldoTotal" });
                    columns2.push({ "mDataProp": "SaldoTotal" });

                    $("#AntiguedadSaldosPeriodos").append(html1);
                    $("#AntiguedadSaldos").append(html2);

                    var ofertas = data.Ofertas;
                    total = ofertas[ofertas.length - 1];
                    ofertas.splice((ofertas.length - 1), 1);

                    tblSaldosPeriodos = $("#AntiguedadSaldosPeriodos").DataTable({
                        "aaData": ofertas,
                        "aoColumns": columns1,
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": true,
                        "ordering": false,
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
                        "fnDrawCallback": function (oSettings) {
                            var registros = oSettings.aiDisplay.length;
                            $('#lbSaldosPeriodos').text(registros);
                        }

                    });

                    var fil = $('#AntiguedadSaldosPeriodos_filter label input');
                    fil.removeClass('input-small').addClass('input-large');

                    $("#tfootPeridos").remove();
                    var footer = '<tfoot id="tfootPeridos" style="color:white;background-color:#3598dc;"><tr><th> ' + total.Descripcion + ' </th>';

                    $(periodos).each(function (i, d) {
                        $(total.Periodos).each(function () {
                            if (this.Periodo == d.Descripcion) footer = footer + "<th> " + this.Saldo + " </th>";
                        });
                    });

                    footer = footer + "<th> " + total.SaldoTotal + " </th></tr></tfoot>";

                    $("#AntiguedadSaldosPeriodos").append(footer);

                    var alumnos = data.Alumnos;
                    ColumnasDetalle = columns2;
                    

                    $("#btnExportar").show();

                    ReporteFn.CargarTipoOfertas();

                    IndexFn.Block(false);

                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    return false;
                });
        },
        exportarexcel(nombre) {

            var table1 = $('#AntiguedadSaldosPeriodos').dataTable().api();
            var data01 = table1.rows({ filter: 'applied' }).data();
            data01.push(total);
            var data1 = [];
            var hd1 = ["Carrera"];
            var table2 = $('#AntiguedadSaldos').dataTable().api();
            var data02 = table2.data();
            var data2 = [];
            var hd2 = ["Alumno", "Nombre"];

            $(data01).each(function (i, d) {

                var obj = { "Carrera": d.Descripcion };

                $(periodos).each(function () {
                    var header = this.Descripcion;

                    $(d.Periodos).each(function () {

                        if (this.Periodo == header) { obj[header] = this.Saldo; };
                    });
                });

                obj["Saldo Total"] = this.SaldoTotal;

                data1.push(obj);
            });

            $(data02).each(function (i, d) {

                var obj = { "Alumno": d.AlumnoId, "Nombre": d.Nombre };

                $(periodos).each(function () {
                    var header = this.Descripcion;

                    $(d.Detalle).each(function () {

                        if (this.Periodo == header) { obj[header] = this.Saldo; };
                    });
                });

                obj["Saldo Total"] = this.SaldoTotal;

                data2.push(obj);
            });

            $(periodos).each(function () {
                hd1.push(this.Descripcion);
                hd2.push(this.Descripcion);
            });
            hd1.push("Saldo Total");
            hd2.push("Saldo Total");


            var ws = XLSX.utils.json_to_sheet(data1, { header: hd1 });
            var ws2 = XLSX.utils.json_to_sheet(data2, { header: hd2 });

            var ws_name = "Detalle Periodos";
            var ws_name2 = "Detalle Alumno";

            function Workbook() {
                if (!(this instanceof Workbook)) return new Workbook();
                this.SheetNames = [];
                this.Sheets = {};
            }

            var wb = new Workbook();

            /* add worksheet to workbook */
            wb.SheetNames.push(ws_name);
            wb.SheetNames.push(ws_name2);

            wb.Sheets[ws_name] = ws;
            wb.Sheets[ws_name2] = ws2;

            var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });


            function s2ab(s) {
                var buf = new ArrayBuffer(s.length);
                var view = new Uint8Array(buf);
                for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                return buf;
            }

            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), nombre + ".xlsx");
        },
        GenerarAlumnoDetalle(Alumnos) {
            tblSaldos = $("#AntiguedadSaldos").DataTable({
                "aaData": Alumnos,
                "aoColumns": ColumnasDetalle,
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
                "fnDrawCallback": function (oSettings) {
                    var registros = oSettings.aiDisplay.length;
                    $('#lbAntiguedadSaldos').text(registros);
                }

            });

            var fil2 = $('#AntiguedadSaldos_filter label input');
            fil2.removeClass('input-small').addClass('input-large');

            IndexFn.Block(false);
        }
    }; 

    ReporteFn.init();
});


