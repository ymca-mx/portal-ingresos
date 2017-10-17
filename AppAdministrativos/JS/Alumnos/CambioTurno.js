$(document).ready(function init() {
    var AlumnoId;
    var lstop = [];

    var eventos = {
        buscarAlumno: function () {
            AlumnoId = $('#txtClave').val();
            if (AlumnoId.length == 0 || parseInt(AlumnoId) < 1) { return false; }

            $('#Load').modal('show');
            $.ajax({
                type: "POST",
                url: "WS/Alumno.asmx/ConsultaCambioTurno",
                data: "{AlumnoId:'" + AlumnoId + "', UsuarioId:" + $.cookie('userAdmin') + "}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    $('#lblNombre').text("");
                    $("#txtOfertaEducativa").val("");
                    $("#txtPeriodo").val("");
                    $("#txtTurno").val("");
                    $('#slcTurno').empty();
                    $('#lblInscito').empty();
                    if (data.d === null) {
                        alertify.alert("Este alumno no existe.");
                        $('#Load').modal('hide');
                        return false;
                    }
                    lstop.length = 0;
                    lstop.push(data.d);
                    $('#lblNombre').text(lstop[0].NombreC);

                    if (lstop[0].OfertaEducativaId == 0) {

                        if (lstop[0].EstatusId == 7) {
                            $('#lblInscito').text("Ya se aplico cambio de turno.");
                        } else {
                            $('#lblInscito').text("No ha iniciado proceso de cambio de turno.");
                        }

                        $('#Load').modal('hide');
                        return false;
                    }
                    if (lstop[0].EstatusId != 4 && lstop[0].EstatusId != 14 && lstop[0].EstatusId != 0) {

                        $('#lblInscito').text("No ha realizado el pago para cambio de turno.");
                        $('#Load').modal('hide');
                        return false;
                    }

                    $("#txtOfertaEducativa").val(lstop[0].OfertaEducativa);
                    $("#txtPeriodo").val(lstop[0].DescripcionPeriodo);
                    $("#txtTurno").val(lstop[0].TurnoActual);


                    var optionS = $(document.createElement('option'));
                    optionS.text("--Seleccionar--");
                    optionS.val(-1);
                    $("#slcTurno").append(optionS);

                    $(lstop[0].Turno).each(function (i, d) {
                        var option = $(document.createElement('option'));
                        option.text(this.Descripcion);
                        option.val(this.TurnoId);
                        $("#slcTurno").append(option);
                    });

                    $("#slcTurno").val(-1);

                    $("#btnCambio").removeAttr("disabled");
                    $('#Load').modal('hide');
                }
            });
        },
        validar: function () {
            if ($("#slcTurno").val() == -1 || $("#slcTurno").val() == null) {
                alertify.alert("Debe seleccionar un nuevo turno.");
                return false;
            }
            $("#PopComentario").modal('show');
        },
        guardar: function () {
            var comentario = $('#txtComentario').val();
            comentario = $.trim(comentario);
            if (comentario.length < 5) {
                alertify.alert("Inserte un comentario.");
                return false;
            }

            $("#PopComentario").modal("hide");
            $('#Load').modal('show');

            lstop[0].TurnoIdNueva = $('#slcTurno').val();
            lstop[0].Observaciones = $("#txtComentario").val();
            lstop[0].UsuarioId = $.cookie('userAdmin');

            var obj = {
                "Cambio": lstop[0]
            };
            obj = JSON.stringify(obj);


            $.ajax({
                type: "POST",
                url: "WS/Alumno.asmx/AplicarCambioTurno",
                data: obj,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d == true) {
                        $("#txtComentario").empty();
                        $("#txtOfertaEducativa").val("");
                        $("#txtPeriodo").val("");
                        $("#txtTurno").val("");
                        $('#slcTurno').empty();
                        $("#btnCambio").attr('disabled', 'disabled');
                        $('#lblInscito').text("Ya se aplicó cambio de carrera.");
                        if (lstop[0].TurnoIdNueva == 5) {
                            alertify.alert("El cambio se realizó correctamente, favor de enviar al alumno a relaciones publicas para la configuracion de cuotas correspondientes de turno nocturno.");
                        } else {
                            alertify.alert("El cambio se realizó correctamente.");
                        }
                    } else {
                        $("#txtComentario").empty();
                        alertify.alert("Error al  realizar cambio.");
                    }
                    $('#Load').modal('hide');
                }
            });
        }
    };

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            eventos.buscarAlumno();
        }
    });

    $('#btnBuscar').click(eventos.buscarAlumno);

    $("#btnCambio").click(eventos.validar);

    $("#btnGuardar").click(eventos.guardar);
});