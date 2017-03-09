$(document).ready(function () {
    var url;
    var tblReferencias, tblSolicitudes;
    var PagoId, SolicitudId;
    var Estatus = "";
    MostrarSolicitudes();

    function MostrarSolicitudes() {
        var usuario = $.cookie('userAdmin');
        $('#divBar').modal('show');
        $.ajax({
            url: '../WebServices/WS/General.asmx/ConsultarPagoCancelacionSolicitud',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{UsuarioId:"' + usuario + '",Tipo:"' + 2 + '"}',
            dataType: 'json',
            success: function (data) {

                tblSolicitudes = $('#tblSolicitudes').dataTable({
                    "aaData": data.d,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "solicitudId" },
                        {
                            "mDataProp": "pagoId",
                            "mRender": function (data) {
                                return "<a href=''onclick='return false;'>" + data + " </a> ";
                            }
                        },
                        { "mDataProp": "comentario" },
                        { "mDataProp": "fechaSolicitud" },
                        { "mDataProp": "usuarioIdSolicitud" }
                    ],

                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                    "searching": true,
                    "ordering": false,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "language": {
                        "lengthMenu": "_MENU_  Referencias",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Referencia ",
                    },
                    "order": [[2, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'center';
                    }
                });
                var fil = $('#tblSolicitudes_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                $('#divBar').modal('hide');
            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });

    }


    $('#tblSolicitudes').on('click', 'a', function () {
        PagoId = 0;
        var rowadd = tblSolicitudes.fnGetData($(this).closest('tr'));
        SolicitudId = rowadd.solicitudId;
        PagoId = rowadd.pagoId;
        Comentario = rowadd.comentario;
        MostarReferencia(PagoId);
    });

    function MostarReferencia(pagoId) {

        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarAlumnoReferencias',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{alumnoInt:' + 0 + ',pagoid:' + pagoId + '}',
            dataType: 'json',
            success: function (data) {
                var datos = data.d;

                if (data.d == null || data.d.AlumnoDatos == null) {
                    $('#divBar').modal('hide');
                    return null;
                }
                $('#lblAlumno').text(datos.AlumnoDatos.Nombre);

                ReferenciasTbl(datos);
                $('#PopReferencias').modal('show');
                $('#divBar').modal('hide');
            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });
    }


    function ReferenciasTbl(R) {
        if (tblReferencias != null) {
            tblReferencias.fnClearTable();
        }

        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": R.AlumnoReferencias,
            "bSort": false,
            "aoColumns": [
                { "mDataProp": "Concepto" },
                { "mDataProp": "ReferenciaBancaria" },
                { "mDataProp": "FechaGeneracion" },
                { "mDataProp": "UsuarioGenero" },
                { "mDataProp": "Promesa" },
                { "mDataProp": "Pagado" },
                { "mDataProp": "Estatus" }

            ],

            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
            "searching": false,
            "ordering": false,
            "async": true,
            "bDestroy": true,
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
            "bInfo": false,
            "bAutoWidth": false,
            "asStripClasses": null,
            "language": {
                "lengthMenu": "_MENU_  Referencias",
                "paginate": {
                    "previous": "<",
                    "next": ">"
                },
                "search": "Buscar Referencia ",
            },
            "order": [[2, "desc"]],
            "createdRow": function (row, data, dataIndex) {
                row.childNodes[1].style.textAlign = 'center';
                row.childNodes[2].style.textAlign = 'center';
                row.childNodes[4].style.textAlign = 'center';
                row.childNodes[5].style.textAlign = 'center';
                if (dataIndex / 2 != 0) {
                    $(row).addClass("BackColor");
                }

                if (data.ReferenciaBancaria <= 2588) {
                    $(row).addClass("bold");
                }
            }
        });
        var fil = $('#tblReferencias_filter label input');
        fil.removeClass('input-small').addClass('input-large');
    }

    $('#btnNoAplica').click(function () {
        var usuario = $.cookie('userAdmin');
        $('#divBar').modal('show');
        $.ajax({
            url: '../WebServices/WS/General.asmx/CambiarPagoCancelacionSolicitud',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{SolicitudId:' + SolicitudId + ',UsuarioId:' + usuario + ',Tipo: ' + 3 + '}',
            dataType: 'json',
            success: function (data) {
                if (data.d == "Solicitud Rechazada") {
                    MostrarSolicitudes();
                    $('#PopReferencias').modal('hide');
                    alertify.alert("Solicitud Rechazada Exitosamente.");
                    $('#divBar').modal('hide');
                }

            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });
    });

    $('#btnCancelar').click(function () {
        var usuario = $.cookie('userAdmin');
        var rowadd = tblReferencias.fnGetData(0);

        var PagoId2 = String(PagoId);

        PagoId2 = PagoId2.substring(0, PagoId2.length - 1);

        Estatus = rowadd.Estatus;
        $('#divBar').modal('show');

        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/CancelarPago",
            data: "{PagoId:'" + PagoId2 + "',Comentario:'" + Comentario +
                    "',UsuarioId:'" + usuario + "',Estatus:'" + Estatus + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == "Guardado") {

                    $.ajax({
                        url: '../WebServices/WS/General.asmx/CambiarPagoCancelacionSolicitud',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: '{SolicitudId:' + SolicitudId + ',UsuarioId:' + usuario + ',Tipo: ' + 7 + '}',
                        dataType: 'json',
                        success: function (data) {
                            MostrarSolicitudes();
                            $('#PopReferencias').modal('hide');
                            alertify.alert("Cargo correctamente cancelado.");
                            

                        }

                    });


                }
            }
        });

    });


});
