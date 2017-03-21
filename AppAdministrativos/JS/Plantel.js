var Plantel = function () {
    var cargarPlantel = function () {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarPlantel",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.DescripcionId);
                    option.val(this.SucursalId);

                    $("#slcPlantel").append(option);
                });
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                $("#slcPlantel").val('1');
                $('#slcPlantel').change();
            }
        });
    }
    $('#slcPlantel').change(function () {
        $("#slcOferta").empty();
        var plantel = $('#slcPlantel').val();
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativaTipo",
            data: "{Plantel:'"+plantel+"'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.OfertaEducativaTipoId);

                    $("#slcOferta").append(option);
                });
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                $("#slcOferta").change();
            }
        });
    });
    $("#slcOferta").change(function () {
        $("#slcCarrera").empty();
        var plantel = $('#slcPlantel').val();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcCarrera").append(optionP);
        $('#txtUni').val('');
        $('#txtUni').attr("disabled", "disabled");
        $('#chkUni').is(':checked') ? $('#chkUni').click() : 'false'; 
        $('#slcLugarUni').val(-1);
        $('#slcLugarUni').change();
        $('#txtNombreUni').val('');
        $('#chkUniSi').prop("checked", false);
        $('#chkUniNo').prop("checked",false);
        $('txtUniMotivo').val('');
        $('#divPrepa').show();
        var tipo = $("#slcOferta");
        tipo = tipo[0].value;
        if (tipo != -1) {
            $('#lblOFerta').html(tipo == 1 ? 'Licenciatura' : tipo == 2 ? 'Especialidad' : tipo == 3 ? 'Maestría' : tipo == 4 ? 'Idioma' : tipo == 5 ? 'Doctorado' : ' ');
            $.ajax({
                type: "POST",
                url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativa",
                data: "{tipoOferta:'" + tipo + "',Plantel:'"+plantel+"'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                dataType: "json",
                success: function (data) {
                    var datos = data.d;
                    if (datos.length > 0) {
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));

                            option.text(this.Descripcion);
                            option.val(this.OfertaEducativaId);

                            $("#slcCarrera").append(option);
                        });
                    } else {
                        $("#slcCarrera").append(optionP);
                    }
                    if (tipo != 4) {
                        $('#divPrepa').show();
                        $('#divUni').show();
                        $('#lblTituloProcedencia').text(tipo == 1 ? 'Preparatoria de Procedencia' : 'Universidad de Procedencia');
                        $('#lblLugarProcedencia').text(tipo == 1 ? 'Lugar donde estudio la preparatoria' : 'Lugar donde estudio la Universidad');
                        document.getElementById("h3Titulo").innerHTML = tipo == 1 ? 'Preparatoria de Procedencia' : 'Universidad de Procedencia';
                    } else  {
                        $('#divPrepa').hide();
                        $('#divUni').hide();
                        //$('#divUniversidad').hide();
                    }
                    //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                }
            });
        } else {
            $("#slcCarrera").append(optionP);
        }
    });
    $("#slcSistemaPago").change(function () {
        var Sispago = $('#slcSistemaPago option:selected').html();
        var monto, total;
        if (Sispago.search("4") != -1) {
            total = Number($('#txtcuotaCol').text().replace('$', ''));
            $('#txtcuotaCol').text('$' + (total / 4));
            monto = (total / 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
            monto = (total / 4) - monto;
            $('#txtPagarCol').text('$' + String(monto));
            //$('#txtDescuentoBec').change();
        } else {
            total = Number($('#txtcuotaCol').text().replace('$', ''));
            $('#txtcuotaCol').text('$' + (total * 4));
            monto = (total * 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
            monto = (total * 4) - monto;
            $('#txtPagarCol').text('$' + String(monto));
        }
    });
    return {
        //main function to initiate the module
        init: function () {

            cargarPlantel();
        }

    };
}();