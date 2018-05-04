var GlobalFn = {
    init() {
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

    }
};
