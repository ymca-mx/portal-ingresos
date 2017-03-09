$(document).ready(function () {
    var AlumnoId, AlumnoId1, TA;
    var Periodos;
    var Anio;
    var tblAlumno1, tblAlumno2;
    var lstop = [], lstop1 = [];
    TraerPeriodos();


    function TraerPeriodos()
    {
        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/PeriodosPromocionCasa",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }
                Periodos = data.d;
                $(Periodos).each(function (i, d)
                {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.attr("data-Anio", this.Anio);
                    option.attr("data-PeriodoId", this.PeriodoId);
                    option.val(i);

                    $("#slcPeriodo").append(option);
                });
                $("#slcPeriodo").val(0);
                $("#slcPeriodo").change();
                
                $('#Load').modal('hide');
            }
        });

    }

    $("#slcPeriodo").change(function ()
    {
        $("#slcMes").empty();
        var per = $('#slcPeriodo').val();

        $(Periodos[per].Meses).each(function (i, d) {
            var option = $(document.createElement('option'));
            option.text(this.Descripcion);
            option.val(this.MesId);

            $("#slcMes").append(option);
        });
    });

    $('#btnBuscar').click(function () {
        TA = 1;
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0 || parseInt(AlumnoId) < 1) { return false; }
        if (tblAlumno1 != undefined) {
            tblAlumno1.fnClearTable();
            if (tblAlumno2 != undefined) 
            {
                tblAlumno2.fnClearTable();
                $("#txtClave1").val("");
                $("#divPeriodo").attr("style", "display : none");
            }

        }
        $('#Load').modal('show');
        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/ConsultarAlumnoPromocionCasa",
            data: "{AlumnoId:'" + AlumnoId + "',TA:'" + TA + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    alertify.alert("Este alumno no existe");
                    $('#Load').modal('hide');
                    return false;
                }
                lstop.length = 0;

                lstop.push(data.d);
                $('#lblNombre').text(lstop[0].NombreC);
                $("#slcOfertaEducativa").empty();
                var optionS = $(document.createElement('option'));
                optionS.text("--Seleccionar--");
                optionS.val(-1);
                $("#slcOfertaEducativa").append(optionS);

                if (lstop[0].OfertaEducativa.length == 0) {
                    alertify.alert("Este alumno no está inscrito al periodo actual");
                    $("#slcOfertaEducativa").prop("disabled", true);
                    $('#Load').modal('hide');
                    return false;
                }

                $(lstop[0].OfertaEducativa).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.descripcion);
                    option.val(this.ofertaEducativaId);

                    $("#slcOfertaEducativa").append(option);
                });
                if (lstop[0].OfertaEducativa.length > 1) {
                    $("#slcOfertaEducativa").val(-1);
                    $("#slcOfertaEducativa").prop("disabled", false);
                } else
                {
                    $("#slcOfertaEducativa").val(lstop[0].OfertaEducativa[0].ofertaEducativaId);
                    $("#slcOfertaEducativa").prop("disabled", true);
                    lstop[0].OfertaEducativaIdActual = lstop[0].OfertaEducativa[0].ofertaEducativaId;
                    lstop[0].OfertaEducativaActual = lstop[0].OfertaEducativa[0].descripcion;
                    CargarAlumno(lstop);
                }

                $('#Load').modal('hide');
            }
        });


    });

    $("#slcOfertaEducativa").change(function () {
        if ($("#slcOfertaEducativa").val() != -1) {
            lstop[0].OfertaEducativaIdActual = $("#slcOfertaEducativa").val();
            lstop[0].OfertaEducativaActual = $("#slcOfertaEducativa option:selected").text();
            CargarAlumno(lstop);
        } else
        {
            tblAlumno1.fnClearTable();
        }
    });

    function CargarAlumno() {
        var prospecto = lstop[0].AlumnoProspecto;
        tblAlumno1 = $("#dtAlumno").dataTable({
            "aaData": lstop,
            "aoColumns": [
                 {
                     "mDataProp": "AlumnoId",
                     "mRender": function (data, f, d) {
                         var link;
                         link = d.AlumnoId + " | " + d.NombreC;

                         return link;
                     }
                 },
                { "mDataProp": "OfertaEducativaActual" },
                {
                    "mDataProp": "AlumnoIdProspecto",
                    "mRender": function (data, f, d) {
                        var link = "" ;
                        if (prospecto == true) {
                            link = d.AlumnoIdProspecto + " | " + d.NombreCProspecto;
                        } 
                        return link;
                    }
                },
                { "mDataProp": "OfertaEducativaProspecto" },
                {
                    "mDataProp": function (data) {
                        var link = "";
                        if (prospecto == true) {
                            link = "<a class='btn yellow' name ='btnPromocion'>Cambiar Alumno Prospecto</a>";
                        }
                        else
                        {
                            link = "<a class='btn blue' name ='btnPromocion'>Agregar Alumno Prospecto</a>";
                        }
                        return link;

                    }
                }
            ],
            "columnDefs": [
                      {
                          "targets": [2],
                          "visible": prospecto,
                          "searchable": false
                      },
                      {
                          "targets": [3],
                          "visible": prospecto,
                          "searchable": false
                      },
            ],
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
            "searching": false,
            "ordering": false,
            "async": true,
            "bDestroy": true,
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
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
                row.childNodes[0].style.textAlign = 'center';
                row.childNodes[1].style.textAlign = 'center';
                row.childNodes[2].style.textAlign = 'center';
                if (prospecto == true)
                {
                    row.childNodes[3].style.textAlign = 'center';
                    row.childNodes[4].style.textAlign = 'center';
                }
            }
        });//$('#dtbecas').DataTable
    }

    $("#dtAlumno").on("click", "a", function ()
    {
        $("#PopAlumnoProspecto").modal('show');
    });

    $("#dtAlumno1").on("click", "a", function () {
        lstop[0].AlumnoIdProspecto = lstop1[0].AlumnoIdProspecto;
        lstop[0].NombreCProspecto = lstop1[0].NombreCProspecto;
        lstop[0].OfertaEducativaIdProspecto = lstop1[0].OfertaEducativaIdProspecto;
        lstop[0].OfertaEducativaProspecto = lstop1[0].OfertaEducativaProspecto;
        lstop[0].AlumnoProspecto = true;
        $("#PopAlumnoProspecto").modal('hide');
        $('#Load').modal('show');
        CargarAlumno();
        $("#divPeriodo").attr("style", "display : block");
        $('#Load').modal('hide');
    });

    $("#btnBuscar1").click(function ()
    {
        TA = 2;
        AlumnoId1 = $('#txtClave1').val();
        if (AlumnoId1.length == 0) { return false; }
        if (tblAlumno2 != undefined) {
            tblAlumno2.fnClearTable();
        }
        $('#Load').modal('show');

        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/ConsultarAlumnoPromocionCasa",
            data: "{AlumnoId:'" + AlumnoId1 + "', TA:'" + TA + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                }
                lstop1.length = 0;
                lstop1.push(data.d);

                if (lstop1[0].AlumnoProspecto == false) {
                      alertify.alert("Este alumno presenta adeudo del periodo anterior favor de verificar sus pagos");
                    //$('#Load').modal('hide');
                    //return false;
                } 
               // else{
                    tblAlumno2 = $("#dtAlumno1").dataTable({
                        "aaData": lstop1,
                        "aoColumns": [
                             {
                                 "mDataProp": "AlumnoIdProspecto",
                                 "mRender": function (data, f, d) {
                                     var link;
                                     link = d.AlumnoIdProspecto + " | " + d.NombreCProspecto;

                                     return link;
                                 }
                             },
                            { "mDataProp": "OfertaEducativaProspecto" },
                            {
                                "mDataProp": function (data) {

                                    return "<a class='btn blue' name ='btnAgregar'>Agregar</a>";

                                }
                            }
                        ],
                        "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
                        "searching": false,
                        "ordering": false,
                        "async": true,
                        "bDestroy": true,
                        "bPaginate": false,
                        "bLengthChange": false,
                        "bFilter": false,
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
                            row.childNodes[0].style.textAlign = 'center';
                            row.childNodes[1].style.textAlign = 'center';
                            row.childNodes[2].style.textAlign = 'center';
                        }
                    });//$('#dtbecas').DataTable
                //}
                
                $('#Load').modal('hide');
            }
        });
    });

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });

    $('#txtClave1').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar1').click();
        }
    });

    $("#btnPromocion").click(function ()
    {
        var monto = $("#txtMonto").val();
        if (monto.length == 0 || parseInt(monto) < 1) { return false; }

        $('#Load').modal('show');

        lstop[0].Anio = $('#slcPeriodo').find(':selected').data("anio");
        lstop[0].PeriodoId = $('#slcPeriodo').find(':selected').data("periodoid");
        lstop[0].SubPeriodoId = $('#slcMes').val();
        lstop[0].Monto = $("#txtMonto").val();

        var obj = {
            "Promocion": lstop[0]
        };
        obj = JSON.stringify(obj);


        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/AplicarPromocionCasa",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === "Aplicada") {
                    alertify.alert("Promoción Aplicada correctamente.");
                } else if (data.d === "Pendiente")
                {
                    alertify.alert("La promoción se guardó correctamente, se aplicara el descuento en cuanto se genere la referencia para el mes solicitado.");
                } else if (data.d === "Ya tiene") {
                    alertify.alert("Este alumno ya tiene promoción en casa para el periodo seleccionado.");
                } else
                {
                    alertify.alert("Error al  aplicar promoción.");
                }
                $('#Load').modal('hide');
            }
        });

    });

});