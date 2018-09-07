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
            $('#btnExportar').on('click', ReporteFn.exportarexcel);

            $('#slcTipoOferta').on('change', this.TipoOfertaChange);
            $('#slcOfertaEducativa').on('change', this.OfertaChange);
            $('#slcTipo').on('change', this.OfertaChange);
        },
        OfertaChange() {
            var Oferta = parseInt($('#slcOfertaEducativa').val()),
                empresa = parseInt($('#slcTipo').val());

            if (Oferta === -1 && empresa === -1) {
                ReporteFn.GenerarAlumnoDetalle(ReporteFn.Todos);
            }else {
                var lstAlumnosOferta = [];
                var EsEmpresa = empresa === 2 ? true : false;
                
                $(ReporteFn.Todos).each(function () {
                    var alumno = {
                        AlumnoId: this.AlumnoId,
                        Nombre: this.Nombre,
                        SaldoPeriodo:[]
                    };

                    $(this.SaldoPeriodo).each(function () {
                        var OfertasAlumno = [];

                        $(this.Ofertas).each(function () {
                            if (Oferta === -1) {
                                if (empresa < 0) {
                                    OfertasAlumno.push({
                                        EsEmpresa: this.EsEmpresa,
                                        OfertaEducativaId: this.OfertaEducativaId,
                                        Saldo: this.Saldo
                                    });
                                } else if (this.EsEmpresa === EsEmpresa) {
                                    OfertasAlumno.push({
                                        EsEmpresa: this.EsEmpresa,
                                        OfertaEducativaId: this.OfertaEducativaId,
                                        Saldo: this.Saldo
                                    });
                                }
                            } else if (this.OfertaEducativaId === Oferta) {
                                if (empresa < 0) {
                                    OfertasAlumno.push({
                                        EsEmpresa: this.EsEmpresa,
                                        OfertaEducativaId: this.OfertaEducativaId,
                                        Saldo: this.Saldo
                                    });
                                } else if (this.EsEmpresa === EsEmpresa) {
                                    OfertasAlumno.push({
                                        EsEmpresa: this.EsEmpresa,
                                        OfertaEducativaId: this.OfertaEducativaId,
                                        Saldo: this.Saldo
                                    });
                                }
                            }
                        });
                        
                        if (OfertasAlumno.length > 0) {
                            alumno.SaldoPeriodo.push({
                                Anio: this.Anio,
                                PeriodoId: this.PeriodoId,
                                Descripcion: this.Descripcion,
                                Ofertas: OfertasAlumno
                            });
                        }
                    });

                    if (alumno.SaldoPeriodo.length > 0) {
                        lstAlumnosOferta.push(alumno);
                    }
                });
                ReporteFn.GenerarAlumnoDetalle(lstAlumnosOferta);
            } 
        },
        SaldoTotal:0,
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
                    ReporteFn.CreateTable(data);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    return false;
                });
        },
        exportarexcel(e) {

            e.preventDefault();

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

                        if (this.Periodo === header) { obj[header] = this.Saldo; }
                    });
                });

                obj["Saldo Total"] = this.SaldoTotal;

                data1.push(obj);
            });

            $(data02).each(function (i, d) {

                var obj = { "Alumno": d.AlumnoId, "Nombre": d.Nombre };
                var TotalAlumno = 0;

                $(periodos).each(function () {
                    var header = this.Descripcion;

                    $(d.SaldoPeriodo).each(function () {
                        if (this.Descripcion === header) {
                            var SaldoPeriodo = 0;
                            $(this.Ofertas).each(function () {
                                SaldoPeriodo += this.Saldo;
                            });

                            obj[header] = SaldoPeriodo;
                            TotalAlumno += SaldoPeriodo;
                        }
                    });
                });

                obj["Saldo Total"] = TotalAlumno;

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
                for (var i = 0; i !== s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                return buf;
            }

            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), "Antiguedad de saldos " + ".xlsx");
        },
        GenerarAlumnoDetalle(Alumnos) {
            tblSaldos = $("#AntiguedadSaldos").DataTable({
                "aaData": Alumnos,
                "aoColumns": ColumnasDetalle,
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": true,
                "ordering": true,
                "async": true,
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
                    "search": "Buscar "
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
        },
        CreateTable(data) {

            periodos = data.Periodos;

            ReporteFn.TipoOfertas = [];
            ReporteFn.TipoOfertas = data.TipoOfertas;

            ReporteFn.Todos = [];
            ReporteFn.Todos = data.Alumnos;

            ReporteFn.CrearAlumnoDetalle();

            var columns1 = [{ "mDataProp": "Descripcion" }];
            $("#theadPeridos").remove();

            var html1 = '<thead id="theadPeridos"><tr role="row"><th style="text-align:center"> Carrera </th>';


            $(periodos).each(function (i, d) {
                html1 = html1 + " <th style='text-align:center'>" + d.DescripcionCorta + " </th>";                

                columns1.push({
                    "mDataProp": "Periodos",
                    "mRender": function (data) {
                        var valor = "";

                        $(data).each(function (i1, d1) {
                            if (d1.Periodo === d.Descripcion) valor = d1.Saldo;
                        });
                        return valor;
                    }
                });

            });

            html1 += '<th style="text-align:center"> Saldo Total </th></tr></thead>';
            

            columns1.push({ "mDataProp": "SaldoTotal" });
            

            $("#AntiguedadSaldosPeriodos").append(html1);
            

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
                    if (this.Periodo === d.Descripcion) footer = footer + "<th> " + this.Saldo + " </th>";
                });
            });

            footer = footer + "<th> " + total.SaldoTotal + " </th></tr></tfoot>";

            $("#AntiguedadSaldosPeriodos").append(footer);

            $("#btnExportar").show();

            ReporteFn.CargarTipoOfertas();            

            IndexFn.Block(false);
        },
        CrearAlumnoDetalle() {
            var columns2 = [
                { "mDataProp": "AlumnoId" },
                { "mDataProp": "Nombre" }
            ];
            $("#theadAlumnos").remove();

            var html2 = '<thead id="theadAlumnos"><tr role="row"><th style="text-align:center"> Alumno </th><th style="text-align:center"> Nombre </th>';
            $(periodos).each(function (i, d) {
                html2 = html2 + " <th style='text-align:center'>" + d.DescripcionCorta + " </th>";
                columns2.push({
                    "mDataProp": "SaldoPeriodo",
                    "mRender": function (data) {
                        var valor = 0;

                        $(data).each(function (i1, d1) {

                            if (d1.Descripcion === d.Descripcion) {
                                $(d1.Ofertas).each(function () {
                                    valor += this.Saldo;
                                });
                            }
                        });
                        ReporteFn.SaldoTotal += valor;

                        return valor > 0 ? (valor).toLocaleString('es-mx', {
                            style: 'currency',
                            currency: 'MXN',
                            minimumFractionDigits: 2
                        }) : "";
                    }
                });
            });
            html2 += '<th style="text-align:center"> Saldo Total </th></tr></thead>';
            columns2.push({
                "mDataProp": "SaldoPeriodo",
                "mRender": function (data) {
                    var Saldo = 0;
                    $(data).each(function () {
                        $(this.Ofertas).each(function () {
                            Saldo += this.Saldo;
                        });
                    });

                    return (Saldo).toLocaleString('es-mx', {
                        style: 'currency',
                        currency: 'MXN',
                        minimumFractionDigits: 2
                    });
                }
            });
            $("#AntiguedadSaldos").append(html2);

            ColumnasDetalle = columns2;
        }
    }; 

    ReporteFn.init();
});


