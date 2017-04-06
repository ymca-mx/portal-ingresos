$(document).ready(function () {
    var AlumnoId;
    var lstop = [];
    
    $('#btnBuscar').click(function () {
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0 || parseInt(AlumnoId) < 1) { return false; }
        
        $('#Load').modal('show');

        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultaCambioCarrera",
            data: "{AlumnoId:'" + AlumnoId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    alertify.alert("Este alumno no esta inscrito al periodo actual");
                    $('#Load').modal('hide');
                    return false;
                }
                lstop.length = 0;

                lstop.push(data.d);
                $('#lblNombre').text(lstop[0].NombreC);
                $("#slcOfertaEducativa").empty();
                var optionS = $(document.createElement('option'));
                optionS.text(lstop[0].OfertaEducativaActual);
                optionS.val(lstop[0].OfertaEducativaIdActual);
                $("#slcOfertaEducativa").append(optionS);
                //$("#slcOfertaEducativa").prop("disabled", true);

                $("#slcOfertaEducativa2").empty();
                var optionS2 = $(document.createElement('option'));
                optionS2.text("--Seleccionar--");
                optionS2.val(-1);
                $("#slcOfertaEducativa2").append(optionS2);

                $(lstop[0].OfertaEducativa).each(function (i, d)
                {
                    var option = $(document.createElement('option'));
                    option.text(this.descripcion);
                    option.val(this.ofertaEducativaId);
                    $("#slcOfertaEducativa2").append(option);
                });
                $("#slcPeriodo").empty();
                var optionS3 = $(document.createElement('option'));
                optionS3.text(lstop[0].Anio + " - " + lstop[0].PeriodoId + "        " + lstop[0].DescripcionPeriodo);
                optionS3.val(1);
                $("#slcPeriodo").append(optionS3);
                $("#slcOfertaEducativa2").val(-1);

                $("#btnCambio").removeAttr("disabled");
                $('#Load').modal('hide');
            }
        });


    });
    
    
    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });

    $("#btnCambio").click(function () {
        if ($("#slcOfertaEducativa2").val() == -1)
        {
            alertify.alert("Debe seleccionar una  nueva oferta.");
            return false;
        }
        $("#PopComentario").modal('show');
    });

    $("#btnGuardar").click(function () {

        $('#Load').modal('show');
        
        lstop[0].OfertaEducativaIdNueva = $('#slcOfertaEducativa2').val();
        lstop[0].Observaciones = $("#txtComentario").val();
        lstop[0].UsuarioId = $.cookie('userAdmin');

        var obj = {
            "Cambio": lstop[0]
        };
        obj = JSON.stringify(obj);


        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/AplicarCambioCarrera",
            data: obj,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d == true) {
                    $("").empty();
                    $("#PopComentario").modal("hide");
                    alertify.alert("El  cambio se realizó correctamente.");
                    $('#btnBuscar').click();
                    $("#btnCambio").prop('disabled', true);
                } else {
                    $("").empty();
                    $("#PopComentario").modal("hide");
                    alertify.alert("Error al  realizar cambio.");
                }
                $('#Load').modal('hide');
            }
        });

    });

});