var EstadoCivil = function () {
    var cargarEstadoCivil = function () {
        $.ajax({
            type: "POST",
            url: "Services/General.asmx/ConsultarEstadoCivil",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var datos = data.d;
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