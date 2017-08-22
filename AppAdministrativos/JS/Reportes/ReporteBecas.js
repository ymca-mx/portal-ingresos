$(document).ready(function () {
    var tblBecaDetalle, tblBecaConcentrado, anio, periodo,descripcion;

    //inicializar
    CargarCuatrimestre();
    
    
    $('#btnBuscar').on('click', function () {
        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
        descripcion = $('#slcCuatrimestre option:selected').text();
        CargarReporteBecas(anio, periodo);
    });

    function CargarCuatrimestre() {
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarCuatrimestreHistorico",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.d === null) {
                    return false;
                }
                var datos = data.d.periodos;
                if (datos.length > 0) {
                    var n = 0;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.descripcion);
                        option.attr("data-Anio", this.anio);
                        option.attr("data-PeriodoId", this.periodoId);
                        option.val(n);

                        $("#slcCuatrimestre").append(option);
                        n++;
                    });
                    $("#slcCuatrimestre").val(n - 2);
                    $("#slcCuatrimestre").change();
                }

            }//success
        });// $.ajax

    }//CargarCatrimestre

    function CargarReporteBecas(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarReporteBecas",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }
                var calculos1 = data.d.Calculos1;
                var calculos2 = data.d.Calculos2;
                var detalle = data.d.Detalle;
                var concentrado = data.d.Concentrado;

                
               
                

                tblBecaDetalle = $("#BecaDetalle").DataTable({
                    "aaData": detalle,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "OfertaEducativaId" },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "CostoIns" },
                        { "mDataProp": "AnticipadoIns" },
                        { "mDataProp": "AnticipadoInsPor"},
                        { "mDataProp": "BecaIns" },
                        { "mDataProp": "BecaInsPor"},
                        { "mDataProp": "DesTotalIns" },
                        { "mDataProp": "DesTotalInsPor"},
                        { "mDataProp": "TotalIns" },
                        { "mDataProp": "CostoCol" },
                        { "mDataProp": "AnticipadoCol" },
                        {"mDataProp": "AnticipadoColPor"},
                        { "mDataProp": "BecaCol" },
                        { "mDataProp": "BecaColPor"},
                        { "mDataProp": "BecaDeportiva" },
                        { "mDataProp": "BecaDeportivaPor"},
                        { "mDataProp": "PromoCasa" },
                        { "mDataProp": "PromoCasaPor"},
                        { "mDataProp": "DesTotalCol" },
                        { "mDataProp": "DesTotalColPor"},
                        { "mDataProp": "TotalCol" },
                        { "mDataProp": "EsEmpresa" }
                    ],
                    "lengthMenu": [[-1, 25, 50, 100], ['Todos',25, 50, 100]],
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
                        "search": "Buscar Alumno ",
                    },
                    "order": [[0, "asc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'right';
                    },
                    "fnFooterCallback": function (tfoot, data, start, end, display)
                    {
                        tfoot.style.backgroundColor = "#3598dc";
                        tfoot.style.color = "white";
                        var nCells = tfoot.getElementsByTagName('th');
                        nCells[2].innerHTML = "Total:";
                        $(calculos1).each(function (d, b) {
                            nCells[d + 3].innerHTML = b.valor;
                        });
                    },
                    "fnDrawCallback": function (oSettings) {
                        var registros = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros);
                    }
                });

                

                var n = 0;

                tblBecaConcentrado = $("#BecaConcentrado").DataTable({
                    "aaData": concentrado,
                    "aoColumns": [
                        {
                            "mRender": function ( Data) {
                                    n = n + 1
                                    return n;
                            }
                        },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "TotalAlumnos" },
                        { "mDataProp": "AlumnosConBeca" },
                        { "mDataProp": "CargosInscripcion" },
                        { "mDataProp": "PromedioAnticipadoPor"},
                        { "mDataProp": "PromedioAnticipado_" },
                        { "mDataProp": "PromedioBecaInscripcionPor"},
                        { "mDataProp": "PromedioBecaInscripcion_" },
                        { "mDataProp": "TotalDescuentoInscripcion" },
                        { "mDataProp": "CargosColegiatura" },
                        { "mDataProp": "PromedioAnticipadoPorColegiatura" },
                        { "mDataProp": "PromedioAnticipado_Colegiatura" },
                        { "mDataProp": "BecaPromedioPor" },
                        { "mDataProp": "BecaPromedio_" },
                        { "mDataProp": "BecaDeportivaPor"},
                        { "mDataProp": "BecaDeportivaPromedio_" },
                        { "mDataProp": "PromoCasaPor"},
                        { "mDataProp": "PromoCasa_" },
                        { "mDataProp": "TotalDescuentoColegiatura" }
                    ],
                    "lengthMenu": [[-1, 25, 50, 100], ['Todos', 25, 50, 100 ]],
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
                        "search": "Buscar Alumno ",
                    },
                    "order": [[1, "asc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'right';
                    },
                    "fnFooterCallback": function (tfoot, data, start, end, display) {
                        tfoot.style.backgroundColor = "#3598dc";
                        tfoot.style.color = "white";
                        var nCells = tfoot.getElementsByTagName('th');

                        $(calculos2).each(function (d, b) {
                            nCells[d + 1].innerHTML = b.valor2;
                        });

                    },
                    "fnDrawCallback": function (oSettings) {
                        var registros = oSettings.aiDisplay.length;
                        $('#lbBecas2').text(registros);
                    }
                });

                $('#Load').modal('hide');
            }//success

        });//$.ajax


    }//CargarAlumnosInscritos()

    $('#divContenido').submit(function () {
        //do your stuff
        return false;
    });

    
////exportar 2//////

    $('#btnBecas3').on('click', function () {
        exportarexcel();
    });

    function exportarexcel()
    {

        if ($("#tab_1").hasClass("active"))
        { $("#tab_2").addClass("active"); }
        else { $("#tab_1").addClass("active");}
        
        var tbl = document.getElementById('BecaDetalle');
        var tbl2 = document.getElementById('BecaConcentrado');
        
        var ws = XLSX.utils.table_to_sheet(tbl);

        var ws1 = XLSX.utils.table_to_sheet(tbl2);

        var ws_name = "Detalle";
        var ws_name1 = "Concentrado";

        function Workbook() {
            if (!(this instanceof Workbook)) return new Workbook();
            this.SheetNames = [];
            this.Sheets = {};
        }

        var wb = new Workbook();

        /* add worksheet to workbook */
        wb.SheetNames.push(ws_name);
        wb.SheetNames.push(ws_name1);

        wb.Sheets[ws_name] = ws;
        wb.Sheets[ws_name1] = ws1;

        var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });


        function s2ab(s) {
            var buf = new ArrayBuffer(s.length);
            var view = new Uint8Array(buf);
            for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
            return buf;
        }

        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), "Reporte Becas " + descripcion + ".xlsx");

        if ($("#tab_1").hasClass("active"))
        { $("#tab_2").removeClass("active");  }
        else { $("#tab_1").removeClass("active"); }
    }


});


