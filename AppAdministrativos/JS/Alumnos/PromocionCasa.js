$(document).ready(function () {
    var Periodos;
    var Anio ,PeriodoId;
    var tblAlumno1;
    var datos, DTOAlumnoPromocionCasa;
    TraerPeriodos();

    function TraerPeriodos() {
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/PeriodosPromocionCasa",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }
                Periodos = data.d;
                $(Periodos).each(function (i, d) {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.attr("data-Anio", this.Anio);
                    option.attr("data-PeriodoId", this.PeriodoId);
                    option.val(i);

                    $("#slcPeriodo").append(option);
                });
                $("#slcPeriodo").val(0);
                $("#slcPeriodo").change();

                IndexFn.Block(false);
            }
        });

    }

    $("#slcPeriodo").change(function () {
        Anio = $('#slcPeriodo').find(':selected').data("anio");
        PeriodoId = $('#slcPeriodo').find(':selected').data("periodoid");
        IndexFn.Block(true);
        $("#slcMes").empty();
        var per = $('#slcPeriodo').val();

        $(Periodos[per].Meses).each(function (i, d) {
            var option = $(document.createElement('option'));
            option.text(this.Descripcion);
            option.val(this.MesId);

            $("#slcMes").append(option);
        });
        CargarAlumnos();
     
        
    });
    
    function CargarAlumnos() {


        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultarAlumnoPromocionCasa",
            data: "{Anio:'" + Anio + "',PeriodoId:'" + PeriodoId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                datos = data.d;

                tblAlumno1 = $("#dtAlumno").dataTable({
                    "aaData": datos,
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
                                var link = "";
                                link = d.AlumnoIdProspecto + " | " + d.NombreCProspecto;
                                return link;
                            }
                        },
                        {
                            "mDataProp": "EstatusId",
                            "mRender": function (data, f, d) {
                                var link = "";
                                if (d.EstatusId == 1) {
                                    link = "<a class='btn green' name ='btnPromocion'>Aplicar promoción</a>";
                                } else if (d.EstatusId == 7) {
                                    link = "Promoción aplicada.";
                                } else
                                {
                                    link = "Pendiente por aplicar.";
                                }
                                return link;
                            }
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
                        row.childNodes[3].style.textAlign = 'center';
                    }
                });//$('#dtbecas').DataTable

                IndexFn.Block(false);
            }
        });

    
    }

    $("#dtAlumno").on("click", "a", function () {
        
        DTOAlumnoPromocionCasa = tblAlumno1.fnGetData($(this).closest('tr'));
        $('#lblNombre').text(DTOAlumnoPromocionCasa.NombreC);
        $("#PopAlumnoProspecto").modal('show');
    });
    

    $("#btnPromocion").click(function () {
        var monto = $("#txtMonto").val();
        if (monto.length == 0 || parseInt(monto) < 1) { return false; }

        IndexFn.Block(true);
        
        DTOAlumnoPromocionCasa.SubPeriodoId = $('#slcMes').val();
        DTOAlumnoPromocionCasa.Monto = $("#txtMonto").val();
        DTOAlumnoPromocionCasa.UsuarioId =  localStorage.getItem('userAdmin');


        var obj = {
            "Promocion": DTOAlumnoPromocionCasa
        };
        obj = JSON.stringify(obj);


        $.ajax({
            type: "POST",
           url: "WS/Alumno.asmx/AplicarPromocionCasa",
           data: obj,
           contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === "Aplicada") {
                    alertify.alert("Promoción Aplicada correctamente.");
                } else if (data.d === "Pendiente") {
                    alertify.alert("La promoción se guardó correctamente, se aplicara el descuento en cuanto se genere la referencia para el mes solicitado.");
                } else if (data.d === "Ya tiene") {
                    alertify.alert("Este alumno ya tiene promoción en casa para el periodo seleccionado.");
                } else {
                    alertify.alert("Error al  aplicar promoción.");
                }
                $("#slcMes").val(1);
                $("#txtMonto").val("");
                CargarAlumnos();
                $("#PopAlumnoProspecto").modal('hide');
                IndexFn.Block(false);
            }
        });

    });

});