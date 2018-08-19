$(document).ready(function () {
    var tblReporte, anio, periodo,descripcion;
    CargarCuatrimestre();

    $("#slcPeriodo").change(function () {

        anio = $('#slcPeriodo').find(':selected').data("anio");
        periodo = $('#slcPeriodo').find(':selected').data("periodoid");
        descripcion = $('#slcPeriodo option:selected').text();
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

    }

    $('#divContenido').submit(function () {
        //do your stuff
        return false;
    });

    function CargarReporteReferencias(anio, periodo) {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteAlumnoReferencia",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    IndexFn.Block(false);
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
                    }

                });

                var fil = $('#dtReferencias_filter label input');
                fil.removeClass('input-small').addClass('input-large');


                IndexFn.Block(false);
            },//success
        });// end $.ajax


    }



    ///exportar
    function exportarexcel(Tabla,nombre) {

        var table1 = $('#' + Tabla).dataTable().api();
        var data1 = table1.rows({ filter: 'applied' }).data();
        var data2 = [];
        var hd;

        $(data1).each(function () {
            var ojb2 = {
                "Alumno id": this.alumnoId,
                "Nombre Alumno": this.nombreAlumno,
                "Oferta Educativa": this.especialidad,
                "Inscripcion": this.inscripcion,
                "Colegiatura": this.colegiatura,
                "MateriaSuelta": this.materiaSuelta,
                "Asesoria Especial": this.asesoriaEspecial,
                "# Materias": this.noMaterias,
                "Materias": this.calificacionMaterias,
                "# bajas": this.noBaja,
                "Tipo": this.tipo
            };
            data2.push(ojb2);
        });
        hd = ["Alumno id", "Nombre Alumno", "Oferta Educativa", "Inscripcion", "Colegiatura", "MateriaSuelta", "Asesoria Especial", "# Materias", "Materias", "# bajas", "Bajas", "Tipo"];


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

        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), nombre +" " +descripcion+".xlsx");
    }


    $('#btnReferencias').on('click', function () {
        exportarexcel('dtReferencias',"Referencias");
    });



});


