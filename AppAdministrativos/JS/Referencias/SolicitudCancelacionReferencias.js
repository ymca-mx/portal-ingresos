$(document).ready(function () {
    var url, refAlumno;
    var tblReferencias,tblReferencias2, tblAlumnos, tblSolicitudes, clave;
    var PagoId;
    var Estatus = "";

    $("#liS1").click(function () {
        $("#divCancelacion").show();
        $("#divSolicitud").hide();
    });

    $("#liS2").click(function () {
        $("#divSolicitud").show();
        $("#divCancelacion").hide();
        MostrarSolicitudes();
    });

    $('#btnBuscar').click(function () {
        clave = $('#txtClave').val();
        if (tblReferencias != null) {
            $('#lblAlumno').text("");
            tblReferencias.fnClearTable();
        }

        if (clave != "") {
            $('#txtBar').text("Buscando Alumno");
            $('#divBar').modal('show');
            $('#slcOfertaEducativa').empty();
            $('#slcConceptos').empty();
            if (!isNaN(clave)) {
                DatosAlumno(clave);
            } else {
                BuscarAlumno(clave);
            }

        }
    });

    function BuscarAlumno(Alumno) {

        $.ajax({
            url: 'WS/Alumno.asmx/BuscarAlumnoString',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{Filtro:"' + Alumno + '"}',
            dataType: 'json',
            success: function (data) {

                if (data.d.length == 0) {
                    $('#divBar').modal('hide');
                    return null;
                } else {
                    $('#divBar').modal('hide');
                    $('#PopAlumnos').modal('show');
                    tblAlumnos = $('#tblAlumnos').dataTable({
                        "aaData": data.d,
                        "bSort": false,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId", "Nombre": "AlumnoId" },
                            {
                                "mDataProp": "Nombre",
                                "mRender": function (data) {
                                    return "<a href=''onclick='return false;'>" + data + " </a> ";
                                }
                            }
                        ],

                        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                        "searching": true,
                        "ordering": false,
                        "async": true,
                        "bDestroy": true,
                        "bPaginate": true,
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
                            "search": "Buscar Alumno ",
                        },
                        "createdRow": function (row, data, dataIndex) {
                            row.childNodes[0].style.textAlign = 'left';
                            row.childNodes[1].style.textAlign = 'left';
                        }
                    });
                    var fil = $('#tblAlumnos_filter label input');
                    fil.removeClass('input-small').addClass('input-large');
                }//if (data.d == null)
            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });

    }

    $("#tblAlumnos").on("click", "a", function () {
        alumno = tblAlumnos.fnGetData(this.parentNode.parentNode, 0);
        $('#PopAlumnos').modal('hide');
        $('#divBar').modal('show');
        DatosAlumno(alumno);
    });

    function DatosAlumno(Alumno) {

        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumnoReferencias',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{alumnoInt:' + Alumno + ',pagoid:' + 0 + '}',
            dataType: 'json',
            success: function (data) {
                var datos = data.d;

                if (data.d == null || data.d.AlumnoDatos == null) {
                    $('#divBar').modal('hide');
                    return null;
                }
                $('#lblAlumno').text(datos.AlumnoDatos.Nombre);

                ReferenciasTbl(datos);

                var fil = $('#tblReferencias label input');
                fil.removeClass('input-small').addClass('input-large');
                $('#divBar').modal('hide');
            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });
    }

    function MostrarSolicitudes() {
        var usuario =  localStorage.getItem('userAdmin');
        $('#divBar').modal('show');
        $.ajax({
            url: 'WS/General.asmx/ConsultarPagoCancelacionSolicitud',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{UsuarioId:"' + usuario + '",Tipo:"' + 1 + '"}',
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
                        { "mDataProp": "fechaAplicacion" },
                        { "mDataProp": "estatusId" }
                    ],

                    "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                    "searching": true,
                    "ordering": true,
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
                    "order": [[1, "desc"]],
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
            url: 'WS/Alumno.asmx/ConsultarAlumnoReferencias',
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
                $('#lblAlumno1').text(datos.AlumnoDatos.Nombre);

                ReferenciasTbl2(datos);
                $('#PopReferencias').modal('show');
                $('#divBar').modal('hide');
            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });
    }

    function ReferenciasTbl2(R) {
        if (tblReferencias2 != null) {
            tblReferencias2.fnClearTable();
        }

        tblReferencias2 = $('#tblReferencias1').dataTable({
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
                { "mDataProp": "Estatus" },
                {
                    "mDataProp": "EnProcesoSolicitud",
                    "mRender": function (data, f, d) {
                        var link;

                        if (d.EnProcesoSolicitud == 1) { link = "<a class='btn yellow'>Solicitar</a>"; }
                        else { link = "En Solicitud"; }
                        return link;
                    }
                }

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

    $('#tblReferencias').on('click', 'a', function () {
        $('#txtComentario').val('');
        PagoId = 0;
        Estatus = "";
        var rowadd = tblReferencias.fnGetData($(this).closest('tr'));
        PagoId = rowadd.ReferenciaBancaria;
        $('#PopComentario').modal('show');
    });

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });

    $('#btnGuardar').click(function () {
        var usuario =  localStorage.getItem('userAdmin');
        var Texto = $('#txtComentario').val();
        Texto = $.trim(Texto);
        if (Texto.length > 5) {
            $('#PopComentario').modal('hide');
            $('#txtBar').text("Guardando");
            $('#divBar').modal('show');
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/PagoCancelacionSolicitud",
                data: "{PagoId:'" + PagoId + "',Comentario:'" + $('#txtComentario').val() +
                        "',UsuarioId:'" + usuario + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == "Solicitud Enviada") {
                        alertify.alert("Solicitud Enviada.");
                        //$('#btnBuscar').click();
                    }

                    DatosAlumno(clave);
                    $('#PopComentario').modal('hide');
                }
            });
        }
        else {
            alertify.alert("Inserte un comentario.");
        }
    });

});
