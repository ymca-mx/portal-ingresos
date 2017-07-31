$(document).ready(function () {
    var tblInscritos, anio, periodo;
    
    //inicializar
    CargarCuatrimestre();
    
    $("#slcCuatrimestre").change(function () {

        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
        CargarAlumnosInscritos(anio, periodo);
    
    });

    $("#slcOferta").change(function () {
        filtro();
    });
    
    function filtro() {
        
          var oferta = $("#slcOferta option:selected").html();
        
                if (oferta != "--Todas--") {
                    if (oferta.includes('Derecho')) {
                        tblInscritos.columns(2)
                            .search(oferta)
                            .draw();
                    } else if (oferta.includes('Maestría en Educación')) {
                        var selected = [];
                        selected.push("^" + oferta + "$");
                        selected.push("^" + oferta + " SAT$");
                        var regex = selected.join("|");
                        tblInscritos.columns(2)
                            .search(regex, true, false, true)
                            .draw();
                    } else {
                        tblInscritos.columns(2)
                            .search("^" + oferta + "$", true, false, true)
                            .draw();
                    }

                } else {
                    tblInscritos.columns(2)
                        .search("")
                        .draw();
                }
                tblInscritos.draw();
    }

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
                    $("#slcCuatrimestre").val(n-1);
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
        
    }//CargarCatrimestre

 

    function CargarAlumnosInscritos(anio, periodo) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteInscrito",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }

                tblInscritos = $("#dtInscritos").DataTable({
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
                        var registros = oSettings.aiDisplay.length;
                        $('#lbInscritos').text(registros);
                    }
                });//$('#dtbecas').DataTable
                filtro();
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
    $('#btnInscritos').mousedown(function () {
        if (this.which === 1) {
            $('#Load').modal('show', $('#btnInscritos').click());
            $('#Load').modal('hide');
        }
    });

    $('#btnInscritos').on('click', function () {
        setTimeout(
            Exportar('dtInscritos'), 1000);
    });



   
});


