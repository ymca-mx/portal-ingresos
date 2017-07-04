var Estado = function () {
    var cargarEstado = function () {
        $('#Load').modal('show');
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarEntidadFederativa",
            data: "{}",
            contentType: "application/json; charset=utf-8", 
            success: function (data) {
                $('#Load').modal('hide');
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#slcEstado").append(option);
                });
                $("#slcEstado").val('9');
                $('#slcEstado').change();
            }
        });
    }

    $("#slcEstado").change(function () {
        $('#Load').modal('show');
        $("#slcMunicipio").empty();
        var Entidad = $("#slcEstado");
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcMunicipio").append(optionP);

        Entidad = Entidad[0].value;
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarMunicipios",
            data: "{EntidadFederativaId:'"+Entidad+"'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                $('#Load').modal('hide');
                var datos = data.d;
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