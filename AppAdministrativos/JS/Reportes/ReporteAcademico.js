$(document).ready(function () {
    var tblBecas, tblBecas1, tblBecas2, tblBecas3, anio, periodo, reporte, of, oferta1, hoy;
    var cuatri = new Array("0", "0", "0", "0");
    //var fechainicial = new Array(3);
    //var fechafinal = new Array(3);
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
        if ($("#from").val()=== "")
        {
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
        oferta1 =  $("#slcOferta option:selected").html() ;
        oferta[reporte] = $("#slcOferta").val();
        fecha1[reporte] = $("#from").val();
        fecha2[reporte] = $("#to").val();

        switch (reporte) {
            case 0:
                if (oferta1 != "--Todas--") {
                    tblBecas.columns(2)
                                .search("^" + oferta1 + "$", true, false, true)
                                .draw();
                } else {
                    tblBecas.columns(2)
                           .search("")
                   .draw();
                }
                tblBecas.draw();
                break;
            case 1:
                if (oferta1 != "--Todas--") {
                    tblBecas1.columns(2)
                                .search("^" + oferta1 + "$", true, false, true)
                                .draw();
                } else {
                    tblBecas1.columns(2)
                           .search("")
                   .draw();
                }
                tblBecas1.draw();
                break;
            case 2:
                if (oferta1 != "--Todas--") {
                    tblBecas2.columns(2)
                                .search("^" + oferta1 + "$", true, false, true)
                                .draw();
                } else {
                    tblBecas2.columns(2)
                           .search("")
                   .draw();
                }
                break;
            case 3:
                if (oferta1 != "--Todas--") {
                    tblBecas3.columns(3)
                                .search("^" + oferta1 + "$", true, false, true)
                                .draw();
                } else {
                    tblBecas3.columns(3)
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
            url: "WS/Reporte.asmx/MostrarCuatrimestre",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                var datos = data.d.item1;
                var datos2 = data.d.item2;
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
                    $("#slcCuatrimestre").val(0);
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
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/MostrarReporteBecaCuatrimestre",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
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
                    ,"fnDrawCallback": function (oSettings) {
                        filtosdatatable();
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable
                filtros();
                

                $('#Load').modal('hide');
            },//success
        });// end $.ajax
        

    }//function CargarReporteBecas()

    function CargarAlumnosInscritos(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/MostrarReporteInscrito",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
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
                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
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
                    "colReorder": true,
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
                     ,"fnDrawCallback": function (oSettings) {
                         filtosdatatable();
                         registros[reporte] = oSettings.aiDisplay.length;
                         $('#lbBecas').text(registros[reporte]);
                     }
                });//$('#dtbecas').DataTable
                filtros();
                $('#Load').modal('hide');
            }//success

        });//$.ajax

        
    }//CargarAlumnosInscritos()

    function CargarAlumnosBecaSep(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/MostrarReporteBecaSep",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
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
                        filtosdatatable();
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable
                filtros();
                $('#Load').modal('hide');
            }//success

        });//$.ajax

       
    }//function CargarAlumnosBecaSep()

    function CargarBecasInegi(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/MostrarReporteIneg",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                tblBecas3 = $("#dtbecas4").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "15%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "especialidad", "sWidth": "10%" },
                        { "mDataProp": "Cuatrimestre", "sWidth": "10%" },
                        { "mDataProp": "sexo", "sWidth": "10%" },
                        { "mDataProp": "edad", "sWidth": "10%" },
                        { "mDataProp": "fechaNacimiento", "sWidth": "10%" },
                        { "mDataProp": "lugarNacimiento", "sWidth": "15%" },

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
                        filtosdatatable();
                        registros[reporte] = oSettings.aiDisplay.length;
                        $('#lbBecas').text(registros[reporte]);
                    }
                });//$('#dtbecas').DataTable
                filtros();
                $('#Load').modal('hide');
            }//success

        });//$.ajax

       
    }//function CargarBecasInegi()


    $.fn.dataTableExt.afnFiltering.push(
	function (oSettings, aData, iDataIndex) {

	    if (reporte === 3) {
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
            $(this).datepicker({
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

    function filtosdatatable() {

        switch (reporte) {
            case 0:
                $("#dtbecas").tableExport.remove();
                $("#dtbecas").tableExport({
                    formats: ["xlsx"],
                });
                break;
            case 1:
                $("#dtbecas2").tableExport.remove();
                $("#dtbecas2").tableExport({
                    formats: ["xlsx"],
                    escape:'false'
                });
                break;
            case 2:
                $("#dtbecas3").tableExport.remove();
                $("#dtbecas3").tableExport({
                    formats: ["xlsx"],
                });
                break;
            case 3:
                $("#dtbecas4").tableExport.remove();
                $("#dtbecas4").tableExport({
                    formats: ["xlsx"],
                });
                break;
        }
    }

});


