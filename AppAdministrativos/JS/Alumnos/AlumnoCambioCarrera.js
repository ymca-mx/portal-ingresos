$(document).ready(function () {
    var AlumnoId;
    var lstop = [];
    
    $('#btnBuscar').click(function () {
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0 || parseInt(AlumnoId) < 1) { return false; }
        
        IndexFn.Block(true);
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultaCambioCarrera",
            data: "{AlumnoId:'" + AlumnoId + "', UsuarioId:" +  localStorage.getItem('userAdmin') + "}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                $('#lblNombre').text("");
                $("#slcOfertaEducativa").empty();
                $("#slcOfertaEducativa2").empty();
                $("#slcPeriodo").empty();
                $('#lblInscito').empty();
                if (data.d === null) {
                    alertify.alert("Este alumno no existe.");
                    IndexFn.Block(false);
                    return false;
                }
                lstop.length = 0;
                lstop.push(data.d);
                $('#lblNombre').text(lstop[0].NombreC);

                if (lstop[0].OfertaEducativaIdActual == 0) {

                    if (lstop[0].EstatusId == 7)
                    {
                        $('#lblInscito').text("Ya se aplico cambio de carrera.");
                    } else {
                        $('#lblInscito').text("No ha iniciado proceso de cambio de carrera.");
                    }

                    IndexFn.Block(false);
                    return false;
                }
                if (lstop[0].EstatusId != 4 && lstop[0].EstatusId != 14 && lstop[0].EstatusId != 0) {

                    $('#lblInscito').text("No ha realizado el pago para cambio de carrera.");
                    IndexFn.Block(false);
                    return false;
                }

                var optionS = $(document.createElement('option'));
                optionS.text(lstop[0].OfertaEducativaActual);
                optionS.val(lstop[0].OfertaEducativaIdActual);
                $("#slcOfertaEducativa").append(optionS);
                //$("#slcOfertaEducativa").prop("disabled", true);

                
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
                var optionS3 = $(document.createElement('option'));
                optionS3.text(lstop[0].Anio + " - " + lstop[0].PeriodoId + "        " + lstop[0].DescripcionPeriodo);
                optionS3.val(1);
                $("#slcPeriodo").append(optionS3);
                $("#slcOfertaEducativa2").val(-1);

                $("#btnCambio").removeAttr("disabled");
                IndexFn.Block(false);
            }
        });


    });
    
    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });

    $("#btnCambio").on('click',function () {
        if ($("#slcOfertaEducativa2").val() == -1 || $("#slcOfertaEducativa2").val() == null)
        {
            alertify.alert("Debe seleccionar una  nueva oferta.");
            return false;
        }
        $("#PopComentario").modal('show');
    });

    $("#btnGuardar").on( 'click', function () {
        var comentario = $('#txtComentario').val();
        comentario = $.trim(comentario);
        if (comentario.length < 5)
        {
            alertify.alert("Inserte un comentario.");
            return false;
        } 

        $("#PopComentario").modal("hide");
        IndexFn.Block(true);
        
        lstop[0].OfertaEducativaIdNueva = $('#slcOfertaEducativa2').val();
        lstop[0].Observaciones = $("#txtComentario").val();
        lstop[0].UsuarioId =  localStorage.getItem('userAdmin');

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
                    $("#txtComentario").empty();
                    $("#slcOfertaEducativa").empty();
                    $("#slcPeriodo").empty();
                    $("#slcOfertaEducativa2").empty();
                    $("#btnCambio").attr('disabled', 'disabled');
                    alertify.alert("El  cambio se realizó correctamente.");
                } else {
                    $("#txtComentario").empty();
                    alertify.alert("Error al  realizar cambio.");
                }
                IndexFn.Block(false);
            }
        });

    });

});