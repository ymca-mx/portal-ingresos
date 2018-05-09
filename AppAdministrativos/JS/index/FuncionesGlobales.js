var GlobalFn = {
    init() {
        $("#slcPlantel").change(this.PlantelChange);
        $("#slcTipoOferta").change(this.TipoOfertaChange);
        $("#slcEstado").change(this.EstadoChange);
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
    GetPeriodo_P_C_F(ComboId) {
        $("#" + ComboId).empty();
        IndexFn.Api("General/ConsultarPeriodosPCF", "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.PeriodoId + " " + this.Anio);
                    option.data("anio", this.Anio);
                    option.data("periodoid", this.PeriodoId);

                    $("#" + ComboId).append(option);
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
    GetMedios() {
        $("#slcMedio").empty();
        IndexFn.Api("General/TraerListaMedios", "GET", "")
            .done(function (data) {
                $(data).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.MedioDifusionId);

                    $("#slcMedio").append(option);
                });
            })
            .fail(function (data) {
                console.log("Fallo la carga de Cargar Medios");
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
                } else {
                    $("#slcOFertaEducativa").val(data[0].OfertaEducativaId);
                }

                $("#slcOFertaEducativa").change();
            })
            .fail(function (data) {
                console.log(data);
            });
    },
    GetPais(SelectName, PaisId)
    {
        $("#" + SelectName).empty();
        IndexFn.Api("General/ConsultarPais", "GET", "")
            .done(function (data) {
                var datos = data;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.PaisId);

                    $("#" + SelectName).append(option);
                });
                $("#" + SelectName).val(PaisId);

            })
            .fail(function (data) {
                console.log(data);
            });
    },
    GetEstado(SelectName, EstadoId) {
        $("#" + SelectName).empty();
        IndexFn.Api("General/ConsultarEntidadFederativa", "GET", "")
            .done(function (data) {
                $('#Load').modal('hide');
                var datos = data;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#" + SelectName).append(option);
                });
                $("#" + SelectName).val(EstadoId);
                $('#' + SelectName).change();
            })
            .fail(function (data) {
                console.log(data);
            });

    },
    GetEstadoCivil() {
        $("#slcEstadoCivil").empty();
        IndexFn.Api("General/ConsultarEstadoCivil", "GET", "")
            .done(function (data) {
                var datos = data;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EstadoCivilId);

                    $("#slcEstadoCivil").append(option);
                });
            })
            .fail(function (data) {
                console.log(data);
            });
    },
    GetParentesco(NameSlc) {
        $("#" + NameSlc).empty();

        var DefaultOp = $(document.createElement('option'));
        DefaultOp.text("--Seleccionar--");
        DefaultOp.val("-1");

        $("#" + NameSlc).append(DefaultOp);

        IndexFn.Api("General/ConsultarParentesco", "GET", "")
            .done(function (data) {
                var datos = data;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.ParentescoId);

                    $("#" + NameSlc).append(option);
                });
            })
            .fail(function (data) {
                console.log(data);
            });
    },
    DatosOferta: {
        PlantelId: -1,
        OfertaEducativaTipoId: -1,
        OFertaEducativa: -1
    },
    PlantelChange() {
        GlobalFn.GetTipoOferta($("#slcPlantel").val());
    },
    TipoOfertaChange() {
        GlobalFn.GetOfertaEducativa($("#slcPlantel").val(), $("#slcTipoOferta").val());
    },
    EstadoChange() {
        var deferred = $.Deferred();

        $("#slcMunicipio").empty();
        var Entidad = $("#slcEstado");
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcMunicipio").append(optionP);

        Entidad = Entidad[0].value;

        IndexFn.Api("General/ConsultarMunicipios/" + Entidad, "GET", "")
            .done(function (data) {
                $('#Load').modal('hide');
                var datos = data;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#slcMunicipio").append(option);
                });
                deferred.resolve(data);
            })
            .fail(function (data) {
                console.log(data);
                deferred.reject;
            });
        return deferred.promise();

    },
    GetAreas(NameSlc) {
        $("#" + NameSlc).empty();
        IndexFn.Api("General/ObtenerAreas", "GET", "")
            .done(function (data) {
                var datos = data;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.AreaAcademicaId);

                    $("#" + NameSlc).append(option);
                });
            })
            .fail(function (data) {
                console.log(data);
            });
    }
};
