$(document).ready(function () {
    var tblBecaDetalle, tblBecaConcentrado, anio, periodo;

    //inicializar
    CargarCuatrimestre();

    $("#slcCuatrimestre").change(function () {

        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
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
                    //$("#slcCuatrimestre").change();
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
                            nCells[d + 4].innerHTML = b.valor2;
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

    function Exportar(NombreTabla) {
        var tablabe = $('#' + NombreTabla)[0];
        var instanse = new TableExport(tablabe, {
            formats: ['xlsx'],
            exportButtons: false
        });
        var ExpTable = instanse.getExportData()[NombreTabla]['xlsx'];
        instanse.export2file(ExpTable.data, ExpTable.mimeType, ExpTable.filename, ExpTable.fileExtension);

    }

    //Botones
    $('#btnBecas').mousedown(function () {
        if (this.which === 1) {
            $('#Load').modal('show', $('#btnBecas').click());
            $('#Load').modal('hide');
        }
    });

    $('#btnBecas').on('click', function () {
        setTimeout(
            Exportar('BecaDetalle'), 1000);
    });

    $('#btnBecas2').mousedown(function () {
        if (this.which === 1) {
            $('#Load').modal('show', $('#btnBecas2').click());
            $('#Load').modal('hide');
        }
    });

    $('#btnBecas2').on('click', function () {
        setTimeout(
            Exportar('BecaDetalle', 'BecaConcentrado'), 1000);
    });





});


