var Genero = function () {
    var cargarGenero = function () {
        $.ajax({
            type: "Get",
            url: "Api/General/ConsultarGenero",
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (datos) {
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