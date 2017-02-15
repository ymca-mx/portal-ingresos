function CargarPaises(combo) {
    $.ajax({
        type: "POST",
        url: "../WebServices/WS/General.asmx/ConsultarPaises",
        data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
        success: function (data) {
            var datos = data.d;
            $(datos).each(function () {
                var option = $(document.createElement('option'));
                option.text(this.Descripcion);
                option.val(this.PaisId);

                combo.append(option);
            });
            //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
           
        }
    });
}
function CargarEstados(combo) {
    $.ajax({
        type: "POST",
        url: "../WebServices/WS/General.asmx/ConsultarEntidadFederativa",
        data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
        success: function (data) {
            var datos = data.d;
            $(datos).each(function () {
                var option = $(document.createElement('option'));

                option.text(this.Descripcion);
                option.val(this.EntidadFederativaId);

                combo.append(option);
            });
            combo.val('9');
            //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
           
        }
    });
}

    $('#slcNacionalidad').change(function () {
        $("#slcLugarN").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcLugarN").append(optionP);

        var tipo = $("#slcNacionalidad");
        tipo = tipo[0].value;
        if (tipo == 2) {
            CargarPaises($("#slcLugarN"));
        }
        else if (tipo == 1) {
            CargarEstados($("#slcLugarN"));
        }
        else { $("#slcLugarN").append(optionP); }
    });
    $('#slcLugarUni').change(function () {
        $("#slcPaisUni").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcPaisUni").append(optionP);

        var tipo = $("#slcLugarUni");
        tipo = tipo[0].value;
        if (tipo == 2) {
            $('#lblLugarUni').html('Pais');
            CargarPaises($("#slcPaisUni"));
        }
        else if (tipo == 1) {
            $('#lblLugarUni').html('Estado');
            CargarEstados($("#slcPaisUni"));
        }
        else {
            $('#lblLugarUni').html(' ');
            $("#slcPaisUni").append(optionP);
        }
    });
    $('#slcNacionalidadPrep').change(function () {        
        $("#slcEstadoPais").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcEstadoPais").append(optionP);

        var tipo = $("#slcNacionalidadPrep");
        tipo = tipo[0].value;
        if (tipo == 2) {
            $('#lblPN').html('País');
            CargarPaises($("#slcEstadoPais"));
        }
        else if (tipo == 1) {
            $('#lblPN').html('Estado');
            CargarEstados($("#slcEstadoPais"));
        }
        else {
            $('#lblPN').html('País | Estado');
            $("#slcEstadoPais").append(optionP);
        }
    });