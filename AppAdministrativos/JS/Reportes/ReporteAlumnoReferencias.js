$(document).ready(function () {
    var tblReporte, anio, periodo;
    CargarCuatrimestre();

    $("#slcPeriodo").change(function () {

        anio = $('#slcPeriodo').find(':selected').data("anio");
        periodo = $('#slcPeriodo').find(':selected').data("periodoid");
        CargarReporteReferencias(anio, periodo);
    });

    function CargarCuatrimestre() {
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarCuatrimestre",
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

                        $("#slcPeriodo").append(option);
                        n++;
                    });// $(datos).each(function ()
                    $("#slcPeriodo").val(0);
                    $("#slcPeriodo").change();
                }//if
            }//success
        });// $.ajax

    }//CargarCatrimestre

    function CargarReporteReferencias(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteAlumnoReferencia",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }

                tblReporte = $("#dtReferencias").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "5%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "15%" },
                        { "mDataProp": "especialidad", "sWidth": "15%" },
                        { "mDataProp": "inscripcion", "sWidth": "5%" },
                        { "mDataProp": "colegiatura", "sWidth": "5%" },
                        { "mDataProp": "materiaSuelta", "sWidth": "5%" },
                        { "mDataProp": "asesoriaEspecial", "sWidth": "5%" },
                        { "mDataProp": "noMaterias", "sWidth": "5%" },
                         {
                             "mDataProp": "calificacionMaterias",
                             "mRender": function (data, f, d) {
                                 var link;
                                 if (data != null) { link = data.split("|").join("<br>----------------------------<br>"); }
                                 else { link = ""; }
                                 return link;
                             }
                         },
                        { "mDataProp": "noBaja", "sWidth": "5%" },
                        {
                            "mDataProp": "bajaMaterias",
                            "mRender": function (data, f, d) {
                                var link;
                                if (data != null) { link = data.split("|").join("<br>----------------------------<br>"); }
                                else { link = ""; }
                                return link;
                            }
                        },
                        { "mDataProp": "tipo", "sWidth": "5%" }

                    ],
                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
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
                    "colReorder": true,
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[2].style.textAlign = 'left';
                        row.childNodes[3].style.textAlign = 'left';
                        row.childNodes[4].style.textAlign = 'left';
                        row.childNodes[5].style.textAlign = 'left';
                        row.childNodes[6].style.textAlign = 'left';
                        row.childNodes[8].style.textAlign = 'left';
                        row.childNodes[10].style.textAlign = 'left';
                    }, "fnDrawCallback": function (oSettings) {
                        ExportarExcel();
                    }

                });//$('#dtbecas').DataTable
                $('#Load').modal('hide');
            },//success
        });// end $.ajax
        

    }//function CargarReporteBecas()


    function ExportarExcel()
    {
        $("#dtReferencias").tableExport.remove();
        $("#dtReferencias").tableExport({
            formats: ["xlsx"],
        });
    }
});


