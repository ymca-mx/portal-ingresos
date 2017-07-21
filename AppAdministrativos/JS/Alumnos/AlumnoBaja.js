$(document).ready(function () {
    var AlumnoId;
    var Alumno;

    var funciones =
        {
            traerCatalogos: function () {
                $('#Load').modal('show');
                $.ajax({
                    type: "POST",
                    url: "WS/Alumno.asmx/ConsultaCatalogosBaja",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d === null) {
                            alertify.alert("Error al cargar datos.");
                            $('#Load').modal('hide');
                            return false;
                        }
                        var datos = data.d

                        var optionS = $(document.createElement('option'));
                        optionS.text("--Seleccionar--");
                        optionS.val("");
                        $("#slcTipo").append(optionS);

                        $(datos.TipoMovimiento).each(function (i, d) {
                            var option = $(document.createElement('option'));
                            option.text(this.Descripcion);
                            option.val(this.TipoMovimientoId);
                            $("#slcTipo").append(option);
                        });

                        var optionS2 = $(document.createElement('option'));
                        optionS2.text("--Seleccionar--");
                        optionS2.val("");
                        $("#slcMotivo").append(optionS2);

                        $(datos.BajaMotivo).each(function (i, d) {
                            var option = $(document.createElement('option'));
                            option.text(this.Descripcion);
                            option.val(this.BajaMotivoId);
                            $("#slcMotivo").append(option);
                        });
                        $('#Load').modal('hide');
                    }
                });
            },
            BuscarAlumno: function () {
                AlumnoId = $('#txtClave').val();
                if (AlumnoId.length == 0 || parseInt(AlumnoId) < 1) { return false; }

                $('#Load').modal('show');
                $.ajax({
                    type: "POST",
                    url: "WS/Alumno.asmx/ConsultaAlumnoBaja",
                    data: "{AlumnoId:'" + AlumnoId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d === null) {
                            alumno = null;

                            alertify.alert("Este alumno no existe o ya esta dado de baja.");
                            $('#Load').modal('hide');
                            return false;
                        }
                        Alumno = data.d;

                        $("#lbNombre").text(Alumno.NombreC);
                        $("#lbOfertaEducativa").text(Alumno.OfertaEducativa);
                        $("#lbPeriodo").text(Alumno.DescripcionPeriodo);

                        $('#Load').modal('hide');
                    }
                });

            },
            RecortarNombre: function (name) {
                var cadena;
                if (name.length > 15) {
                    cadena = name.substring(0, 8);
                    cadena += name.substring(name.length - 4, name.length);
                    return cadena;
                } else {
                    return name;
                }
            },
            GuardarDocumentos: function (AlumnoMovimientoId) {
                var data = new FormData();
                var fileFormato;
                fileFormato = $('#DeportivaArchivo');
                fileFormato = fileFormato[0].files[0];
                data.append("DocumentoDeportiva", fileFormato);
                data.append("AlumnoMovimientoId", AlumnoMovimientoId);
            }
        }

    funciones.traerCatalogos();

    if (jQuery().datepicker) {
        $('.date-picker').datepicker({
            rtl: Metronic.isRTL(),
            orientation: "left",
            autoclose: true,
            language: 'es'
        });
    }

    $('#BajaArchivo').bind('change', function () {
        var file = $('#FileFormato');
        var tex = $('#TxtFolioArchivo').html();
        if (this.files.length > 0) {
            $('#TxtFolioArchivo').text(funciones.RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileFormato span span').text('Cambiar');
        }
        else {
            $('#TxtFolioArchivo').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileFormato span span').text('Seleccionar Archivo...');
        }
    });

    $('#FileFormato a').click(function () {
        var file = $('#FileFormato');
        $('#TxtFolioArchivo').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileFormato span span').text('Seleccionar Archivo...');
    });

    $('#btnBuscar').click(funciones.BuscarAlumno);

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            funciones.BuscarAlumno();
        }
    });

    $("#btnBaja").click(function () {
        var $frm = $('#frmBaja');
        if ($frm[0].checkValidity()) {
            $("#PopComentario").modal('show');
        }
    });

    $('#btnGuardarDatos').on('click', function () {
        var $frm = $('#frmDatos');
        if ($frm[0].checkValidity()) {
            /* submit the form */
            $('#Load').modal('show');
        }
    });


    $("#btnGuardar").click(function () {
        var comentario = $('#txtComentario').val();
        comentario = $.trim(comentario);
        if (comentario.length < 5) {
            alertify.alert("Inserte un comentario.");
            return false;
        }

        $("#PopComentario").modal("hide");
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
                $('#Load').modal('hide');
            }
        });

    });



});