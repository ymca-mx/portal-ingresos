﻿$(document).ready(function () {
    var tblBecas, tblBecas1, tblBecas2, tblBecas3, anio, periodo,descripcion, reporte, of, oferta1, hoy;
    var cuatri = new Array("1", "1", "1", "1");
    var fecha1 = new Array("", "", "", "");
    var fecha2 = new Array("", "", "", "");
    var oferta = new Array("0", "0", "0", "0");
    var registros = new Array("", "", "", "");
    //inicializar
    $("#rangoFechas").show();
    reporte = 1
    of = 3;
    CargarCuatrimestre();
    $("#liBeca2").addClass('active');

    $("#liBeca1").click(function () {
        $("#divBecas1").show();
        $("#divBecas2").hide();
        $("#divBecas3").hide();
        $("#divBecas4").hide();
        $("#rangoFechas").show();
        reporte = 0;
        of = 6;
        reset();
    });

    $("#liBeca2").click(function () {
        $("#divBecas2").show();
        $("#divBecas1").hide();
        $("#divBecas3").hide();
        $("#divBecas4").hide();
        $("#rangoFechas").show();
        reporte = 1
        of = 3;
        reset();
    });

    $("#liBeca3").click(function () {
        $("#divBecas3").show();
        $("#divBecas1").hide();
        $("#divBecas2").hide();
        $("#divBecas4").hide();
        $("#rangoFechas").show();
        reporte = 2
        of = 5;
        reset();
    });

    $("#liBeca4").click(function () {
        $("#divBecas4").show();
        $("#divBecas1").hide();
        $("#divBecas2").hide();
        $("#divBecas3").hide();
        $("#rangoFechas").hide();
        reporte = 3;
        of = 6;
        reset();
    });

    $("#slcCuatrimestre").change(function () {

        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
        descripcion = $('#slcCuatrimestre option:selected').text();
        switch (reporte) {
            case 0:
                CargarBecasCuatrimestre(anio, periodo);
                break;
            case 1:
                CargarAlumnosInscritos(anio, periodo);

                break;
            case 2:
                CargarAlumnosBecaSep(anio, periodo);
                break;
            case 3:
                CargarBecasInegi(anio, periodo);
                break;
        }

    });

    $("#slcOferta").change(function () {
        filtros();
    });

    $('.date-picker').on("hide", function () {
        if ($("#from").val() === "") {
            $('.input-daterange input').datepicker("setDate", hoy);
        }
        if ($("#to").val() === "") {
            $('.input-daterange input').datepicker("setDate", hoy);
        }
        filtros();
    });

    function reset() {

        $("#slcCuatrimestre").val(cuatri[reporte]);

        if (registros[reporte] === "") {
            $('.input-daterange input').datepicker("setDate", hoy);
            $("#slcCuatrimestre").change();
        }
        $("#slcOferta").val(oferta[reporte]);
        $('#lbBecas').text(registros[reporte]);

        if (fecha1[reporte] === "" && fecha2[reporte] === "") {

        } else {
            $("#from").val(fecha1[reporte]);
            $("#to").val(fecha2[reporte]);
        }

    }

    function filtros() {

        cuatri[reporte] = $("#slcCuatrimestre").val();
        oferta1 = $("#slcOferta option:selected").html();
        oferta[reporte] = $("#slcOferta").val();
        fecha1[reporte] = $("#from").val();
        fecha2[reporte] = $("#to").val();


        switch (reporte) {
            case 0:
                if (oferta1 != "--Todas--") {
                    if (oferta1.includes('Derecho')) {
                        tblBecas.columns(2)
                            .search(oferta1)
                            .draw();
                    } else if (oferta1.includes('Maestría en Educación')) {
                        var selected = [];
                        selected.push("^" + oferta1 + "$");
                        selected.push("^" + oferta1 + " SAT$");
                        var regex = selected.join("|");
                        tblBecas1.columns(2)
                            .search(regex, true, false, true)
                            .draw();
                    } else {
                        tblBecas.columns(2)
                            .search("^" + oferta1 + "$", true, false, true)
                            .draw();
                    }

                } else {
                    tblBecas.columns(2)
                        .search("")
                        .draw();
                }
                tblBecas.draw();

                break;
            case 1:
                if (oferta1 != "--Todas--") {
                    if (oferta1.includes('Derecho')) {
                        tblBecas1.columns(2)
                            .search(oferta1)
                            .draw();
                    } else if (oferta1.includes('Maestría en Educación')) {
                        var selected = [];
                        selected.push("^" + oferta1 + "$");
                        selected.push("^" + oferta1 + " SAT$");
                        var regex = selected.join("|");
                        tblBecas1.columns(2)
                            .search(regex, true, false, true)
                            .draw();
                    } else {
                        tblBecas1.columns(2)
                            .search("^" + oferta1 + "$", true, false, true)
                            .draw();
                    }
                } else {
                    tblBecas1.columns(2)
                        .search("")
                        .draw();
                }
                tblBecas1.draw();
                break;
            case 2:
                if (oferta1 != "--Todas--") {
                    if (oferta1.includes('Derecho')) {
                        tblBecas2.columns(2)
                            .search(oferta1)
                            .draw();
                    } else if (oferta1.includes('Maestría en Educación')) {
                        var selected = [];
                        selected.push("^" + oferta1 + "$");
                        selected.push("^" + oferta1 + " SAT$");
                        var regex = selected.join("|");
                        tblBecas1.columns(2)
                            .search(regex, true, false, true)
                            .draw();
                    } else {
                        tblBecas2.columns(2)
                            .search("^" + oferta1 + "$", true, false, true)
                            .draw();
                    }
                } else {
                    tblBecas2.columns(2)
                        .search("")
                        .draw();
                }
                tblBecas2.draw();
                break;
            case 3:
                if (oferta1 != "--Todas--") {
                    if (oferta1.includes('Derecho')) {
                        tblBecas3.columns(2)
                            .search(oferta1)
                            .draw();
                    } else if (oferta1.includes('Maestría en Educación')) {
                        var selected = [];
                        selected.push("^" + oferta1 + "$");
                        selected.push("^" + oferta1 + " SAT$");
                        var regex = selected.join("|");
                        tblBecas1.columns(2)
                            .search(regex, true, false, true)
                            .draw();
                    } else {
                        tblBecas3.columns(2)
                            .search("^" + oferta1 + "$", true, false, true)
                            .draw();
                    }
                } else {
                    tblBecas3.columns(2)
                        .search("")
                        .draw();
                }
                break;
        }

        //$("#lbBecas").text(registros[reporte]);

    }

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
                var datos2 = data.d.ofertas;
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
                    });// $(datos).each(function ()
                    $("#slcCuatrimestre").val(1);
                    $("#slcCuatrimestre").change();
                }//if
                if (datos2.length > 0) {
                    $(datos2).each(function () {
                        var option1 = $(document.createElement('option'));
                        option1.val(this.ofertaEducativaId);
                        option1.text(this.descripcion);
                        $("#slcOferta").append(option1);
                    });// $(datos).each(function ()
                }//if
            }//success
        });// $.ajax


        // obtener fecha actual
        hoy = new Date();
        var dd = hoy.getDate();
        var mm = hoy.getMonth() + 1; //hoy es 0!
        var yyyy = hoy.getFullYear();
        if (dd < 10) {
            dd = '0' + dd
        }
        if (mm < 10) {
            mm = '0' + mm
        }
        hoy = dd + '/' + mm + '/' + yyyy;


    }//CargarCatrimestre

    function CargarBecasCuatrimestre(anio, periodo) {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteBecaCuatrimestre",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }

                tblBecas = $("#dtbecas").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "7%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "especialidad", "sWidth": "10%" },
                        { "mDataProp": "becaDescuento", "sWidth": "10%" },
                        { "mDataProp": "porcentajeDescuento", "sWidth": "10%" },
                        { "mDataProp": "comentario", "sWidth": "10%" },
                        { "mDataProp": "fechaGeneracion", "sWidth": "10%" },
                        { "mDataProp": "horaGeneracion", "sWidth": "10%" },
                        { "mDataProp": "usuarioAplico", "sWidth": "13%" },

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
                    "oSearch": { "bSmart": false },
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "order": [[1, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[4].style.textAlign = 'right';
                        row.childNodes[6].style.textAlign = 'center';
                        row.childNodes[7].style.textAlign = 'center';
                    }
                    , "fnDrawCallback": function (oSettings) {
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable

                var fil = $('#dtbecas_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                


                filtros();


                IndexFn.Block(false);
            },//success
        });// end $.ajax


    }//function CargarReporteBecas()

    function CargarAlumnosInscritos(anio, periodo) {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteInscrito",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }

                tblBecas1 = $("#dtbecas2").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "10%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "especialidad", "sWidth": "20%" },
                        { "mDataProp": "fechaInscripcion", "sWidth": "10%" },
                        { "mDataProp": "porcentajeDescuento", "sWidth": "10%" },
                        { "mDataProp": "tipoAlumno", "sWidth": "10%" },
                        { "mDataProp": "esEmpresa", "sWidth": "10%" },
                        { "mDataProp": "usuarioAplico", "sWidth": "10%" }
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
                        "search": "Buscar Alumno ",
                    },
                    "order": [[1, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'right';
                    }
                    , "fnDrawCallback": function (oSettings) {
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable

                var fil = $('#dtbecas2_filter label input');
                fil.removeClass('input-small').addClass('input-large');

                
                filtros();
                IndexFn.Block(false);
            }//success

        });//$.ajax


    }//CargarAlumnosInscritos()

    function CargarAlumnosBecaSep(anio, periodo) {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteBecaSep",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }
                tblBecas2 = $("#dtbecas3").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "10%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "especialidad", "sWidth": "15%" },
                        { "mDataProp": "porcentajeDescuento", "sWidth": "10%" },
                        { "mDataProp": "comentario", "sWidth": "10%" },
                        { "mDataProp": "fechaGeneracion", "sWidth": "10%" },
                        { "mDataProp": "horaGeneracion", "sWidth": "10%" },
                        { "mDataProp": "usuarioAplico", "sWidth": "15%" },

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
                    "oSearch": { "bSmart": false },
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "order": [[1, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[3].style.textAlign = 'right';
                        row.childNodes[5].style.textAlign = 'center';
                        row.childNodes[6].style.textAlign = 'center';
                    }
                    , "fnDrawCallback": function (oSettings) {
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable
                var fil = $('#dtbecas3_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                

                filtros();
                IndexFn.Block(false);
            }//success

        });//$.ajax


    }//function CargarAlumnosBecaSep()

    function CargarBecasInegi(anio, periodo) {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteIneg",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }
                tblBecas3 = $("#dtbecas4").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "10%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "especialidad", "sWidth": "10%" },
                        { "mDataProp": "Cuatrimestre", "sWidth": "10%" },
                        { "mDataProp": "sexo", "sWidth": "10%" },
                        { "mDataProp": "edad", "sWidth": "10%" },
                        { "mDataProp": "fechaNacimiento", "sWidth": "10%" },
                        { "mDataProp": "lugarNacimiento", "sWidth": "10%" },
                        { "mDataProp": "lugarEstudio", "sWidth": "10%" },

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
                    "oSearch": { "bSmart": false },
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },

                    "order": [[1, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[2].style.textAlign = 'right';
                        row.childNodes[5].style.textAlign = 'right';
                        row.childNodes[6].style.textAlign = 'center';

                    }
                    , "fnDrawCallback": function (oSettings) {
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable

                var fil = $('#dtbecas4_filter label input');
                fil.removeClass('input-small').addClass('input-large');

                filtros();
                IndexFn.Block(false);
            }//success

        });//$.ajax


    }//function CargarBecasInegi()


    $.fn.dataTableExt.afnFiltering.push(
        function (oSettings, aData, iDataIndex) {

            if (reporte === 3 || !$('#chkYo').prop('checked')) {
                var iFini = "";
                var iFfin = "";
            }
            else {
                var iFini = document.getElementById('from').value;
                var iFfin = document.getElementById('to').value;
            }
            var iStartDateCol = of;

            iFini = iFini.substring(6, 10) + iFini.substring(3, 5) + iFini.substring(0, 2);
            iFfin = iFfin.substring(6, 10) + iFfin.substring(3, 5) + iFfin.substring(0, 2);

            var datofini = aData[iStartDateCol].substring(6, 10) + aData[iStartDateCol].substring(3, 5) + aData[iStartDateCol].substring(0, 2);

            if (iFini === "" && iFfin === "") {
                return true;
            }
            else if (iFini === "" && iFfin >= datofini) {
                return true;
            }
            else if (iFini <= datofini && iFfin === "") {
                return true;
            }
            else if (iFini <= datofini && iFfin >= datofini) {
                return true;
            }
            return false;
        }
    );

    if (jQuery().datepicker) {
        $('.date-picker').each(function () {
            $("#data1").datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                language: 'es',
                dateFormat: 'dd/mm/yyyy',
                firstDay: 1

            });
            $("#data2").datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                language: 'es',
                dateFormat: 'dd/mm/yyyy',
                firstDay: 1

            });
            $('.input-daterange input').datepicker("setDate", hoy);
        });
    }

    $('#divContenido').submit(function () {
        //do your stuff

        return false;
    });

    function exportarexcel(Tabla,nombre) {

        var table1 = $('#' + Tabla).dataTable().api();
        var data1 = table1.rows({ filter: 'applied' }).data();
        var data2 = [];
        var hd;

        if (Tabla == "dtbecas") {
            $(data1).each(function () {
                var ojb2 = {
                    "Alumno id": this.alumnoId,
                    "Nombre Alumno": this.nombreAlumno,
                    "Oferta Educativa": this.especialidad,
                    "Tipo beca | Descuento": this.becaDescuento,
                    "Porcentaje beca | descuento": this.porcentajeDescuento,
                    "Observaciones": this.comentario,
                    "Fecha Generacion": this.fechaGeneracion,
                    "Hora Generacion": this.horaGeneracion,
                    "Usuario Aplico": this.usuarioAplico
                };
                data2.push(ojb2);
            });
            hd = ["Alumno id", "Nombre Alumno", "Oferta Educativa", "Tipo beca | Descuento", "Porcentaje beca | descuento", "Observaciones", "Fecha Generacion", "Hora Generacion", "Usuario Aplico"];
        }
        if (Tabla == "dtbecas2") {
            $(data1).each(function () {
                var ojb2 = {
                    "Alumno Id": this.alumnoId,
                    "Nombre Alumno": this.nombreAlumno,
                    "Oferta Educativa": this.especialidad,
                    "Fecha de Inscripción": this.fechaInscripcion,
                    "Porcentaje beca | descuento": this.porcentajeDescuento,
                    "Tipo de Alumno": this.tipoAlumno,
                    "Es Empresa": this.esEmpresa,
                    "Usuario Aplico": this.usuarioAplico
                };
                data2.push(ojb2);
            });
            hd = ["Alumno Id", "Nombre Alumno", "Oferta Educativa", "Fecha de Inscripción", "Porcentaje beca | descuento", "Tipo de Alumno", "Es Empresa", "Usuario Aplico"];

        }
        if (Tabla == "dtbecas3") {
            $(data1).each(function () {
                var ojb2 = {
                    "Alumno Id": this.alumnoId,
                    "Nombre Alumno": this.nombreAlumno,
                    "Oferta Educativa": this.especialidad,
                    "Porcentaje beca | descuento": this.porcentajeDescuento,
                    "Observaciones": this.comentario,
                    "Fecha Generacion": this.fechaGeneracion,
                    "Hora Generacion": this.horaGeneracion,
                    "Usuario Aplico": this.usuarioAplico
                };
                data2.push(ojb2);
            });
            hd = ["Alumno Id", "Nombre Alumno","Oferta Educativa","Porcentaje beca | descuento","Observaciones","Fecha Generacion","Hora Generacion", "Usuario Aplico"];

        }
        if (Tabla == "dtbecas4") {
            $(data1).each(function () {
                var ojb2 = {
                    "Alumno Id": this.alumnoId,
                    "Nombre Alumno": this.nombreAlumno,
                    "Oferta Educativa": this.especialidad,
                    "Cuatrimestre": this.Cuatrimestre,
                    "Sexo": this.sexo,
                    "Edad": this.edad, 
                    "Fecha de Nacimiento": this.fechaNacimiento,
                    "Lugar de Nacimiento": this.lugarNacimiento,
                    "Lugar de Ultimo Estudio": this.lugarEstudio
                };
                data2.push(ojb2);
            });
            hd = ["Alumno Id","Nombre Alumno","Oferta Educativa","Cuatrimestre","Sexo","Edad",   "Fecha de Nacimiento","Lugar de Nacimiento","Lugar de Ultimo Estudio"];

        }

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

        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), nombre + " " + descripcion +  ".xlsx");
    }

    $('#btndtbecas').on('click', function () {
        exportarexcel('dtbecas', "Becas");
    });
    $('#btndtbecas2').on('click', function () {
        exportarexcel('dtbecas2', "Alumnos Inscritos");
    });
    $('#btndtbecas3').on('click', function () {
        exportarexcel('dtbecas3', "Becas Sep");
    });
    $('#btndtbecas4').on('click', function () {
        exportarexcel('dtbecas4',"INEGI");
    });


    $('#chkYo').change(function () {
        if ($('#chkYo').prop('checked')) {
            $("#from").prop('disabled', false);
            $("#to").prop('disabled', false);
            filtros();
        } else {
            $("#from").prop('disabled', true);
            $("#to").prop('disabled', true);
            filtros();
        }
    });


    $("#btnSolicitar").click(function ()
    {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/EnviarSolicitudSep",
            data: "{FechaInicial:'" + $("#from1").val() + "',FechaFinal:'" + $("#to1").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                IndexFn.Block(false);
                if (data.d) {
                    alertify.alert("Email enviado");
                    
                } else {
                    alertify.alert("Email no pudo ser enviado");
                }

            }//success
        });// $.ajax
    });
});


