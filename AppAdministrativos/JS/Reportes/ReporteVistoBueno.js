$(document).ready(function () {
    var tblVoBo, anio, periodo, oferta, vobo, registros, usuarioid, alumnoid, Mostra;
    //inicializar
    CargarCuatrimestre();

    $('#divContenido').submit(function () {
        //do your stuff
        return false;
    });

    $("#slcCuatrimestre").change(function () {
        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
        usuarioid = $.cookie('userAdmin');
        CargarVistoBueno(anio, periodo, usuarioid);

    });

    $("#slcOferta").change(function () {
        if ($("#slcOferta").val() != -1) {
            oferta = $("#slcOferta option:selected").html();

            if (oferta.includes('Derecho')) {
                tblVoBo.columns(1)
                    .search(oferta)
                    .draw();
            } else if (oferta.includes('Maestría en Educación')) {
                var selected = [];
                selected.push("^" + oferta + "$");
                selected.push("^" + oferta + " SAT$");
                var regex = selected.join("|");
                tblVoBo.columns(1)
                    .search(regex, true, false, true)
                    .draw();
            } else {
                tblVoBo.columns(1)
                    .search("^" + oferta + "$", true, false, true)
                    .draw();
            }
        } else
        {
            oferta = "";
            tblVoBo.columns(1)
                .search(oferta)
                .draw();
        }
    });


    $("#slcVisto").change(function () {
        var l = 4;
        vobo = ""

        tblVoBo.columns(l)
            .search(vobo)
            .draw();

        tblVoBo.columns(2)
            .search("")
            .draw();

        if ($("#slcVisto").val() != -1) {
            if ($("#slcVisto").val() == 0) {
                vobo = "/";
                l= 4
            } else if ($("#slcVisto").val() == 1) {
                vobo = "-";
                l= 4
            } else
            {
                vobo = "No";
                l = 2
            }
            
        } 

        tblVoBo.columns(l)
            .search(vobo)
            .draw();
            
        
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

    }//CargarCatrimestre

    function CargarVistoBueno(anio, periodo, usuarioid) {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarReporteVoBo",
            data: "{anio:" + anio + ",periodoid:" + periodo + ", usuarioid:"+ usuarioid +"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if(data.d != null )
               {
                    Mostra = data.d.EsEscolares;

                    var Mostra2 = true ;
                    if (Mostra)
                    {
                        Mostra2 = false;
                    }
                    tblVoBo = $("#dtVoBo").DataTable({
                        "aaData": data.d.AlumnoVoBo,
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
                            { "mDataProp": "UsuarioVoBo" },
                            {
                                "mDataProp": "UsuarioVoBo",
                                "mRender": function (data, f, d) {
                                    var link
                                    if (d.Inscrito == "No") {
                                        link = "<a class='btn blue' name ='btnEnviar'>Enviar</a>";
                                    } else { link = ""; }

                                    return link;
                                }
                            }


                        ],
                        "columnDefs": [
                            {
                              "targets": [3],
                              "visible": Mostra,
                              "searchable": false
                            },
                            { 
                                "targets": [9],
                                "visible": Mostra2,
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
                        "colReorder": false,
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
                            //filtosdatatable();
                            registros = oSettings.aiDisplay.length;
                            $('#lbRegistros').text(registros);
                        }
                    });//$('#dtbecas').DataTable
                    //filtros();


                    $('#Load').modal('hide');
                }//if(data.d != null )
                $('#Load').modal('hide');
            },//success
        });// end $.ajax


    }//function CargarReporteBecas()

    $("#dtVoBo").on('click', 'a', function ()
    {
        var rowData = tblVoBo.row($(this).closest('tr')).data();
            alumnoid = rowData.AlumnoId;
            $("#lblNombre").text(rowData.Nombre);
            $("#txtMail").val(rowData.Email);
            $("#PopEnviar").modal("show");
    });
    $('#btndtVoBo').on('mausedown', function () {
        $('#Load').modal('show');
    });
    $('#btndtVoBo').on('click', function () {
        Exportar('dtVoBo');
    });
    function Exportar(NombreTabla) {        
        var tablabe = $('#' + NombreTabla)[0];
        var instanse = new TableExport(tablabe, {
            formats: ['xlsx'],
            exportButtons: false
        });
        var ExpTable = instanse.getExportData()[NombreTabla]['xlsx'];
        instanse.export2file(ExpTable.data, ExpTable.mimeType, ExpTable.filename, ExpTable.fileExtension);
        $('#Load').modal('hide');
    }

    $("#btnEnviar").click(function ()
    {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/ReporteVoBoEnviarEmail",
            data: "{AlumnoId:" + alumnoid + ",EmailAlumno:'" + $("#txtMail").val()+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                $('#Load').modal('hide');
                if (data.d) {
                    alertify.alert("Email enviado");

                    $("#PopEnviar").modal("hide");
                } else
                {
                    alertify.alert("Email no pudo ser enviado");
                }
                
            }//success
        });// $.ajax
    });

});


