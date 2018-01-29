var EstadoCivil = function () {
    var cargarEstadoCivil = function () {
        $.ajax({
            type: "get",
            url: "Api/General/ConsultarEstadoCivil",
            contentType: "application/json; charset=utf-8",
            success: function (datos) {
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EstadoCivilId);

                    $("#slcEstadoCivil").append(option);
                });
            }
        });
    }

    return {
        //main function to initiate the module
        init: function () {

            cargarEstadoCivil();
        }

    };
}();