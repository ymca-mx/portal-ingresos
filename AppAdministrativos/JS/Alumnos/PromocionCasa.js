$(function () {
    var Periodos,
        Anio,
        PeriodoId,
        tblAlumno1,
        DTOAlumnoPromocionCasa;

    var PromocionFn = {
        init() {
            IndexFn.Block(true);
            $("#slcPeriodo").on('change', this.PeriodoChange);
            $("#dtAlumno").on("click", "a", this.Alumno_a);
            $("#btnPromocion").on('click', this.PromocionClick);
            this.TraerPeriodos();
        },
        TraerPeriodos() {
            IndexFn.Api("Alumno/Periodo/PromocionCasa", "get", "")
                .done(function (data) {
                    if (data.Periodos === null) {
                        IndexFn.Block(false);
                        return false;
                    }
                    Periodos = data.Periodos;
                    $(Periodos).each(function (i, d) {
                        var option = $(document.createElement('option'));
                        option.text(this.Descripcion);
                        option.data("Anio", this.Anio);
                        option.data("PeriodoId", this.PeriodoId);
                        option.val(i);

                        $("#slcPeriodo").append(option);
                    });
                    $("#slcPeriodo").val(0);
                    $("#slcPeriodo").change();

                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    return false;
                });
            

        },
        CargarAlumnos() {
            IndexFn.Api("Alumno/Alumno/PromocionCasa/" + Anio + "/" + PeriodoId, "get", "")
                .done(function (data) {
                    tblAlumno1 = $("#dtAlumno").dataTable({
                        "aaData": data.Alumnos,
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
                                    } else {
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
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    console.log(data);
                });

        },
        PeriodoChange()
        {
            Anio = $('#slcPeriodo').find(':selected').data("Anio");
            PeriodoId = $('#slcPeriodo').find(':selected').data("PeriodoId");
            IndexFn.Block(true);
            $("#slcMes").empty();
            var per = $('#slcPeriodo').val();

            $(Periodos[per].Meses).each(function (i, d) {
                var option = $(document.createElement('option'));
                option.text(this.Descripcion);
                option.val(this.MesId);

                $("#slcMes").append(option);
            });

            PromocionFn.CargarAlumnos();
        },
        Alumno_a() {

            DTOAlumnoPromocionCasa = tblAlumno1.fnGetData($(this).closest('tr'));
            $('#lblNombre').text(DTOAlumnoPromocionCasa.NombreC);
            $("#PopAlumnoProspecto").modal('show');
        },
        PromocionClick() {
            var monto = $("#txtMonto").val();
            if (monto.length == 0 || parseInt(monto) < 1) { return false; }

            $("#PopAlumnoProspecto").modal('hide');
            IndexFn.Block(true);

            DTOAlumnoPromocionCasa.SubPeriodoId = $('#slcMes').val();
            DTOAlumnoPromocionCasa.Monto = $("#txtMonto").val();
            DTOAlumnoPromocionCasa.UsuarioId = localStorage.getItem('userAdmin');
            

            IndexFn.Api("Alumno/PromocionCasa", "Post", JSON.stringify(DTOAlumnoPromocionCasa))
                .done(function (data) {
                    if (data.Message === "Aplicada") {
                        alertify.alert("Promoción Aplicada correctamente.");
                    } else if (data.Message === "Pendiente") {
                        alertify.alert("La promoción se guardó correctamente, se aplicara el descuento en cuanto se genere la referencia para el mes solicitado.");
                    } else if (data.Message === "Ya tiene") {
                        alertify.alert("Este alumno ya tiene promoción en casa para el periodo seleccionado.");
                    } else {
                        alertify.alert("Error al  aplicar promoción.");
                    }

                    $("#slcMes").val(1);
                    $("#txtMonto").val("");
                    
                    $("#PopAlumnoProspecto").modal('hide');

                    PromocionFn.CargarAlumnos();
                })
                .fail(function (data) {                    
                    console.log(data);
                    IndexFn.Block(false);
                    $("#PopAlumnoProspecto").modal('show');
                });
            

        }
    };

    PromocionFn.init();
});