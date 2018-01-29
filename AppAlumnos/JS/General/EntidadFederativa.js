var Estado = function () {
    var cargarEstado = function () {
        $.ajax({
            type: "Get",
            url: "Api/General/ConsultarEntidadFederativa",
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (datos) {
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#slcEstado").append(option);
                });
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                $("#slcEstado").val('9');
                $('#slcEstado').change();
            }
        });
    }

    $("#slcEstado").change(function () {
        $("#slcMunicipio").empty();
        var Entidad = $("#slcEstado");
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcMunicipio").append(optionP);

        Entidad = Entidad[0].value;
        $.ajax({
            type: "Get",
            url: "Api/General/ConsultarMunicipios/" + Entidad,
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (datos) {

                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#slcMunicipio").append(option);
                });
            }
        });
    });

    return {
        //main function to initiate the module
        init: function () {

            cargarEstado();
        }

    };
}();