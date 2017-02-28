$(document).ready(function () {
    var tblVoBo, anio, periodo;
    //inicializar
    CargarCuatrimestre();

    $("#slcCuatrimestre").change(function () {
        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
               // CargarVistoBueno(anio, periodo);      

    });

    $("#slcOferta").change(function () {
        filtros();
    });

    function filtros() {

        cuatri[reporte] = $("#slcCuatrimestre").val();
        oferta1 = $("#slcOferta option:selected").html();
        oferta[reporte] = $("#slcOferta").val();
        fecha1[reporte] = $("#from").val();
        fecha2[reporte] = $("#to").val();

        switch (reporte) {
            case 0:
                if (oferta1 != "--Todas--") {
                    tblBecas.columns(2)
                                .search(oferta1)
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
                                .search(oferta1)
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
                                .search(oferta1)
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
                                .search(oferta1)
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
            url: "../WebServices/WS/Reporte.asmx/MostrarCuatrimestre",
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

    function CargarVistoBueno(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "../WebServices/WS/Reporte.asmx/MostrarReporteBecaCuatrimestre",
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

    function CargarAlumnosBecaSep(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "../WebServices/WS/Reporte.asmx/MostrarReporteBecaSep",
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
            url: "../WebServices/WS/Reporte.asmx/MostrarReporteIneg",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                tblBecas3 = $("#dtbecas4").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "10%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "ciclo", "sWidth": "10%" },
                        { "mDataProp": "especialidad", "sWidth": "15%" },
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

    function filtosdatatable() {

                $("#dtbecas").tableExport.remove();
                $("#dtbecas").tableExport({
                    formats: ["xlsx"],
                });
            
          
    }



});


