$(document).ready(function () {
    Areas();
    function Areas() {
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ObtenerAreas",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.AreaAcademicaId);

                    $("#slcArea").append(option);
                });
            }
        });
    }
});