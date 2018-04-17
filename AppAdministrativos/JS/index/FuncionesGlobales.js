var GlobalFn = {
    init() {
        $("#slcPlantel").change(this.PlantelChange);
        $("#slcTipoOferta").change(this.TipoOfertaChange);
    },
    DatosOferta: {
        PlantelId: -1,
        OfertaEducativaTipoId: -1,
        OFertaEducativa:-1
    },
    PlantelChange() {
        GlobalFn.GetTipoOferta($("#slcPlantel").val());
    },
    TipoOfertaChange() {
        GlobalFn.GetOfertaEducativa($("#slcPlantel").val(),$("#slcTipoOferta").val());
    },
    GetGenero() {
        $("#slcSexo").empty();
        IndexFn.Api("General/ConsultarGenero", "GET", "")
            .done(function (datos) {
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.GeneroId);

                    $("#slcSexo").append(option);
                });
            })
            .fail(function (data) {
                console.log("Fallo la carga de GetGenero");
            });
    },
    GetTurno() {
        $("#slcTurno").empty();
        IndexFn.Api("General/ConsultarTurnos", "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.TurnoId);

                    $("#slcTurno").append(option);
                });                
            })
            .fail(function (data) {
                console.log("Fallo la carga de GetTurno");
            });
    },
    GetPeriodo_N_I() {
        $("#slcPeriodo").empty();
        IndexFn.Api("General/ConsultarPeriodos", "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.PeriodoId + " " + this.Anio);
                    option.data("anio", this.Anio);
                    option.data("periodoid", this.PeriodoId);

                    $("#slcPeriodo").append(option);
                });
            })
            .fail(function (data) {
                console.log("Fallo la carga de GetPeriodo_N_I");
            });
    },
    GetPlantel() {
        $("#slcPlantel").empty();
        $("#slcTipoOferta").empty();
        $("#slcOFertaEducativa").empty();
        IndexFn.Api("General/Plantel", "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.SucursalId);

                    $("#slcPlantel").append(option);
                });
                $("#slcPlantel").change();
            })
            .fail(function (data) {
                console.log("Fallo la carga de GetPlantel");
            });
    },
    GetTipoOferta(PlantelId) {
        $("#slcTipoOferta").empty();
        $("#slcOFertaEducativa").empty();
        IndexFn.Api("General/OFertaEducativaTipo/" + PlantelId, "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.OfertaEducativaTipoId);

                    $("#slcTipoOferta").append(option);
                });

                if (GlobalFn.DatosOferta.OfertaEducativaTipoId !== -1) {
                    $("#slcTipoOferta").val(GlobalFn.DatosOferta.OfertaEducativaTipoId);
                }
                
                $("#slcTipoOferta").change();
            })
            .fail(function (data) {
                console.log(data);
            });
    },
    GetOfertaEducativa(PlantelId, TipoOFertaId) {
        $("#slcOFertaEducativa").empty();
        IndexFn.Api("General/OFertaEducativa/" + PlantelId + "/" + TipoOFertaId, "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.OfertaEducativaId);

                    $("#slcOFertaEducativa").append(option);
                });

                if (GlobalFn.DatosOferta.OFertaEducativa !== -1) {
                    $("#slcOFertaEducativa").val(GlobalFn.DatosOferta.OFertaEducativa);
                }

                $("#slcOFertaEducativa").change();
            })
            .fail(function (data) {
                console.log(data);
            });
    }
};
