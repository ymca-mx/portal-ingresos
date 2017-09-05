$(function init() {
    var AlumnoId;
    var tblReferencias;
    var PagoId;
    var Estatus = "";
    var Eventos = {
        BotonBuscar: function () {
            var clave = $('#txtClave').val();
            if (clave != "") {
                $('#txtBar').text("Buscando Alumno");
                $('#divBar').modal('show');
                $('#slcOfertaEducativa').empty();
                $('#slcConceptos').empty();
                //ConsutlarAdeudos(clave);
                Funciones.DatosAlumno(clave);
            }
        },
        ClickTabla: function () {
            if (this.name == "cancelar") {
                $('#txtComentario').val('');
                PagoId = 0;
                Estatus = "";
                var rowadd = tblReferencias.fnGetData($(this).closest('tr'));
                PagoId = rowadd.PagoId;
                Estatus = rowadd.objNormal.Estatus;
                $('#PopComentario').modal('show');
            } else {
                var rowadd = tblReferencias.fnGetData($(this).closest('tr'));
                alertify.confirm("Esta seguro que quiere activar la referencia:  " + rowadd.Referencia, function (e) {
                    if (e) {
                        $('#divBar').modal('show');
                        Funciones.ActivarReferencia(rowadd.PagoId);
                    }
                });
            }
        },
        TxtClaveKeyDown: function (e) {
            if (e.which == 13) {
                $('#btnBuscar').click();
            }
        },
        BtnGuardarClick: function () {
            var usuario = $.cookie('userAdmin');
            var Texto = $('#txtComentario').val();
            Texto = $.trim(Texto);
            if (Texto.length > 5) {
                $('#PopComentario').modal('hide');
                $('#txtBar').text("Guardando");
                $('#divBar').modal('show');
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/CancelarPago",
                    data: "{PagoId:'" + PagoId + "',Comentario:'" + $('#txtComentario').val() +
                    "',UsuarioId:'" + usuario + "',Estatus:'" + Estatus + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d == "Guardado") {
                            alertify.alert("Cargo correctamente cancelado.").set('onok', function () {
                                $('#btnBuscar').click();
                            });
                        }
                        $('#PopComentario').modal('hide');
                    }
                });
            }
            else {
                alertify.alert("Inserte un comentario.");
            }
        }
    };
    var Funciones = {
        DatosAlumno: function (Alumnoid) {
            AlumnoId = Alumnoid;
            //var AlumnoId = '9579';
            $.ajax({
                url: 'WS/Alumno.asmx/ConsultarAlumnoL',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data.d == null) {
                        $('#divBar').modal('hide');
                        return null;
                    }
                    $('#lblAlumno').text(data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno);
                    Funciones.CargarPagosConceptos(Alumnoid);
                }
            });
        }, 
        CargarPagosConceptos: function (Alumno) {
            $.ajax({
                url: 'WS/Alumno.asmx/ConsultarReferenciasCancelar',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:' + Alumno + '}',
                dataType: 'json',
                success: function (Respuesta) {
                    Funciones.ReferenciasTbl(Respuesta);
                    var fil = $('#tblReferencias label input');
                    fil.removeClass('input-small').addClass('input-large');
                    $('#divBar').modal('hide');
                },
                error: function (Respuesta) {
                    alertify.alert('Error al cargar datos');
                    $('#divBar').modal('hide');
                }
            });
        },
        ReferenciasTbl: function (R) {
            if (tblReferencias != null) {
                tblReferencias.fnClearTable();
            }
            tblReferencias = $('#tblReferencias').dataTable({
                "aaData": R.d,
                "bSort": false,
                "aoColumns": [
                    { "mDataProp": "DTOCuota.DTOPagoConcepto.Descripcion" },
                    { "mDataProp": "Referencia" },
                    { "mDataProp": "FechaGeneracionS" },
                    { "mDataProp": "objNormal.Monto" },
                    { "mDataProp": "objNormal.Restante" },
                    { "mDataProp": "objNormal.FechaLimite" },
                    { "mDataProp": "objNormal.Estatus" },
                    {
                        "mDataProp": function (data) {
                            var link = data.objNormal.Estatus == "Cancelado" ? "<a class='btn blue' name='activar'>Activar</a>"
                                : "<a class='btn red' name='cancelar'>Cancelar</a>";
                            return link;
                        }
                    }
                    //{ "mDataProp": "objRetrasado.Monto" },
                    //{ "mDataProp": "objRetrasado.FechaLimite" },
                ],

                "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'Todos']],
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
                "createdRow": function (row, data, dataIndex) {
                    row.childNodes[0].style.textAlign = 'left';
                    row.childNodes[1].style.textAlign = 'left';
                    row.childNodes[2].style.textAlign = 'left';
                    row.childNodes[3].style.textAlign = 'left';
                    row.childNodes[4].style.textAlign = 'left';
                    if (dataIndex / 2 != 0) {
                        $(row).addClass("BackColor");
                    }

                    if (data.PagoId <= 2588) {
                        $(row).addClass("bold");
                    }
                }
            });
            var fil = $('#tblReferencias_filter label input');
            fil.removeClass('input-small').addClass('input-large');
        },
        ActivarReferencia: function (PagoId) {
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ActivarPago",
                data: "{PagoId:'" + PagoId + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == "Guardado") {
                        $('#divBar').modal('hide');
                        alertify.alert("Referencia Activada correctamente.").set('onok', function () {
                            $('#btnBuscar').click();
                        });
                    } else {
                        alertify.alert("No se pudo activar, favor de llamar al area de sistemas.").set('onok', function () {
                            $('#divBar').modal('hide');
                        });
                    }

                }
            });
        },
    };

    $('#btnBuscar').click(Eventos.BotonBuscar);

    $('#tblReferencias').on('click', 'a', Eventos.ClickTabla);    

    $('#txtClave').on('keydown', Eventos.TxtClaveKeyDown);

    $('#btnGuardar').click(Eventos.BtnGuardarClick);
});
