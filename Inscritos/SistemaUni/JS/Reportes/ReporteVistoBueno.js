$(document).ready(function () {
    var tblVoBo, anio, periodo, oferta,vobo, registros, usuarioid;
    //inicializar
    CargarCuatrimestre();

    $("#slcCuatrimestre").change(function () {
        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
        usuarioid = $.cookie('userAdmin');
        CargarVistoBueno(anio, periodo, usuarioid);

    });

    $("#slcOferta").change(function () {
        if ($("#slcOferta").val() != -1) {
            oferta = $("#slcOferta option:selected").html();
        } else
        {
            oferta = "";
        }
        filtroOferta();
    });

    $("#slcVisto").change(function () {
        if ($("#slcVisto").val() != -1) {
            if ($("#slcVisto").val() == 0)
            {
                vobo = "/";
            } else
            {
                vobo = "-";
            }

           
        } else {
            vobo = "";
        }
        filtroEstatus();
    });


    function filtroOferta() {
        tblVoBo.columns(1)
                .search(oferta)
                .draw();
    }

    function filtroEstatus() {
        tblVoBo.columns(4)
                .search(vobo)
                .draw();
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

    }//CargarCatrimestre

    function CargarVistoBueno(anio, periodo, usuarioid) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "../WebServices/WS/Reporte.asmx/ReporteVoBo",
            data: "{anio:" + anio + ",periodoid:" + periodo + ", usuarioid:"+ usuarioid +"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if(data.d != null )
               {
                    var Mostra = data.d.Sw;

                    tblVoBo = $("#dtVoBo").DataTable({
                        "aaData": data.d.lstVoBo,
                        "aoColumns": [
                             {
                                 "mDataProp": "AlumnoId",
                                 "mRender": function (data, f, d) {
                                     var link;
                                     link = d.AlumnoId + " | " + d.Nombre;

                                     return link;
                                 }
                             },
                            { "mDataProp": "OfertaEducativa" },
                             { "mDataProp": "Inscrito" },
                            { "mDataProp": "FechaInscrito" },
                            { "mDataProp": "FechaVoBo" },
                            { "mDataProp": "InscripcionCompleta" },
                            { "mDataProp": "Asesorias" },
                            { "mDataProp": "Materias" },
                            { "mDataProp": "UsuarioVoBo" }

                        ],
                        "columnDefs": [
                          {
                              "targets": [3],
                              "visible": Mostra,
                              "searchable": false
                          },
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
                            row.childNodes[2].style.textAlign = 'center';
                            row.childNodes[3].style.textAlign = 'center';
                            row.childNodes[4].style.textAlign = 'center';
                            row.childNodes[5].style.textAlign = 'center';
                            row.childNodes[6].style.textAlign = 'center';
                            row.childNodes[7].style.textAlign = 'center';
                        }
                        , "fnDrawCallback": function (oSettings) {
                            filtosdatatable();
                            registros = oSettings.aiDisplay.length;
                            $('#lbRegistros').text(registros);
                        }
                    });//$('#dtbecas').DataTable
                    //filtros();


                    $('#Load').modal('hide');
                }//if(data.d != null )

            },//success
        });// end $.ajax


    }//function CargarReporteBecas()


    function filtosdatatable() {

        $("#dtVoBo").tableExport.remove();
        $("#dtVoBo").tableExport({
            formats: ["xlsx"],
        });
    }



});


