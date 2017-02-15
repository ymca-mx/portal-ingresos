var Genero = function () {
    var cargarGenero = function () {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarGenero",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.GeneroId);

                    $("#slcSexo").append(option);
                });
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
            }
        });
    }
        
    return {
        //main function to initiate the module
        init: function () {

            cargarGenero();
        }

    };
    }();