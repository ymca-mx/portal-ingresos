//JS de Generacion de Referencias Coordinadores
$(function () {
    var lstCuotas,
        AlumnoId,
        EsVariable,
        PagoId = 0,
        Bandera = 0,
        tblReferencias,
        tblAlumnos,
        Estatus;

    var ReferenciasFn = {
        init() {
            $('#fotoAlumno').hide();
            $('#lblNombre').hide();
            $('#btnBuscar').on('click', this.BtnBuscarClick);
            $('#txtClave').on('keydown', this.txtClaveKeydown);
            $('#tblAlumnos').on('click', 'a', this.TablaAlumnoClick);
            $('#slcOfertaEducativa').on('change', this.OfertaChange);
            $('#tblReferencias').on('click', 'a', this.ReferenciasClick);
            $('#slcConceptos').on('change', this.ConceptoChange);
            $('#btnMonto').on('click', this.MontoClick);
            $('#btnCancelar').on('click', this.CancelarClick);
            $('#bntClose').on('click', this.CloseClick);
            $('#btnGenerar').on('click', this.GeneraClick);
            $('#btnGuardar').on('click', this.GuardarClick);
        },
        BtnBuscarClick() {
            document.getElementById("fotoAlumno").src = "";
            $('#frmVarios').hide();
            $('#fotoAlumno').hide();

            $('#lblNombre').hide();

            var lbl = $('#lblNombre');
            lbl[0].innerHTML = "";
            AlumnoId = $('#txtClave').val();

            if (AlumnoId.length == 0) { return false; }
            if (tblReferencias != undefined) { tblReferencias.fnClearTable(); }

            IndexFn.Block(true);
            $('#slcOfertaEducativa').empty();
            $('#slcConceptos').empty();

            if (!isNaN(AlumnoId)) { ReferenciasFn.BuscarAlumno(AlumnoId); }
            else { ReferenciasFn.BuscarNombre(AlumnoId); }
        },
        txtClaveKeydown(e) {
            if (e.which == 13) {
                ReferenciasFn.BtnBuscarClick();
            }
        },
        BuscarAlumno(idAlumno) {

            IndexFn.Api("Alumno/ConsultarAlumno/" + idAlumno, "GET", "")
                .done(function (data) {
                    if (data !== null) {
                        $('#fotoAlumno').show();
                        document.getElementById("fotoAlumno").src = "data:image/png;base64," + data.fotoBase64;
                        $('#lblNombre').show();
                        var lbl = $('#lblNombre');
                        lbl[0].innerHTML = data.Nombre + " " + data.Paterno + " " + data.Materno;
                        lbl[0].innerHTML += data.AlumnoInscrito.EsEmpresa == true && data.AlumnoInscrito.Grupo != null ? (data.AlumnoInscrito.Grupo.EsEspecial == true ? " - Alumno Especial  " : " - Grupo  Empresarial") + " - " + data.AlumnoInscrito.Grupo.Descripcion : "";
                        

                        $(data.lstAlumnoInscrito).each(function () {
                            var option = $(document.createElement('option'));
                            option.text(this.OfertaEducativa.Descripcion);
                            option.val(this.OfertaEducativa.OfertaEducativaId);
                            $('#slcOfertaEducativa').append(option);
                        });

                        if (data.lstAlumnoInscrito.length == 1) {
                            $('#slcOfertaEducativa').val(data.lstAlumnoInscrito[0].OfertaEducativaId);
                            $('#slcOfertaEducativa').change();
                        } else {
                            $('#slcOfertaEducativa').change();
                        }
                    }

                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    console.log(data);
                    IndexFn.Block(false);
                });
        },
        BuscarNombre(Nombre) {

            IndexFn.Api("Alumno/BuscarAlumnoString/" + Nombre, "GET", "")
                .done(function (data) {
                    if (data != null) {
                        $('#frmVarios').show();
                        tblAlumnos = $('#tblAlumnos').dataTable({
                            "aaData": data,
                            "aoColumns": [
                                { "mDataProp": "AlumnoId" },
                                { "mDataProp": "Nombre" },
                                { "mDataProp": "FechaRegistro" },
                                { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                //{ "mDataProp": "FechaSeguimiento" },
                                {
                                    "mDataProp": function (data) {
                                        return "<a class='btn green'>Seleccionar</a>";
                                    }
                                }
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": false,
                            "ordering": false,
                            "async": true,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bLengthChange": false,
                            "bFilter": false,
                            "bInfo": false,
                            "pageLength": 5,
                            "bAutoWidth": false,
                            "asStripClasses": null,
                            "language": {
                                "lengthMenu": "_MENU_  Registros",
                                "paginate": {
                                    "previous": "<",
                                    "next": ">"
                                },
                                "search": "Buscar Alumno "
                            },
                            "order": [[2, "desc"]]
                        });
                    }
                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    console.log(data);
                });
        },
        OfertaChange() {
            if ($('#slcOfertaEducativa').val() != -1) {
                $('#divBar').modal('show');
                ReferenciasFn.GetAdeudo(AlumnoId, $('#slcOfertaEducativa').val());
            } else {
                $("#slcConceptos").empty();
            }
        },
        GetAdeudo(AlumnoId, Oferta) {
            $("#slcConceptos").empty();
            $.ajax({
                url: 'WS/Alumno.asmx/ConsultarAdeudoCoordinador',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + Oferta + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data.d == "Debe") {
                        alertify.alert('Tiene adeudos, favor de pasar a La Corordinación Administrativa para resolver su situación financiera.');
                        ReferenciasFn.CargarPagosConceptos(AlumnoId, Oferta);
                    } else {
                        ReferenciasFn.GetConceptos($('#slcOfertaEducativa').val());
                    }
                }
            });
        },
        GetConceptos(OfertaEducativaId) {
            var usuario = localStorage.getItem('userAdmin');
            $("#slcConceptos").empty();
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/Conceptos2",
                data: "{AlumnoId:" + AlumnoId + ",OfertaEducativa:" + OfertaEducativaId + ",UsuarioId:" + usuario + "}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    lstCuotas = datos;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.DTOPagoConcepto.Descripcion + " | $" + ReferenciasFn.formato_numero(this.Monto, 2, ".", ","));
                        option.val(this.DTOPagoConcepto.PagoConceptoId);
                        option.attr("data-EsVariable", this.DTOPagoConcepto.EsVairable);
                        $('#slcConceptos').append(option);
                        //$('#slcConceptos').change();
                    });
                    ReferenciasFn.CargarPagosConceptos(AlumnoId, OfertaEducativaId);
                }
            });
        },
        CargarPagosConceptos(Alumno, ofertaEd) {
            $.ajax({
                url: 'WS/Alumno.asmx/ConsultarReferenciasCP2',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:' + Alumno + ',OfertaEducativaId:' + ofertaEd + '}',
                dataType: 'json',
                success: function (Respuesta) {
                    ReferenciasFn.CargarReferencias(Respuesta.d);
                    var fil = $('#tblReferencias label input');
                    fil.removeClass('input-small').addClass('input-large');
                    IndexFn.Block(false);
                },
                error: function (Respuesta) {
                    $('#divBar').modal('hide');
                    alertify.alert('Error al cargar datos');
                }
            });
        },
        ReferenciasClick() {
            $('#txtComentario').val('');
            PagoId = 0;
            var rowadd = tblReferencias.fnGetData($(this).closest('tr'));
            PagoId = rowadd.PagoId;
            Estatus = rowadd.objNormal.Estatus;
            $('#PopComentario').modal('show');
        },
        CargarReferencias(Pagos) {
            if (tblReferencias != null) {
                tblReferencias.fnClearTable();
            }
            tblReferencias = $('#tblReferencias').dataTable({
                "aaData": Pagos,
                "bSort": false,
                "aoColumns": [
                    { "mDataProp": "DTOCuota.DTOPagoConcepto.Descripcion" },
                    { "mDataProp": "Referencia" },
                    { "mDataProp": "FechaGeneracionS" },
                    { "mDataProp": "objNormal.Monto" },
                    { "mDataProp": "objNormal.FechaLimite" },
                    {
                        "mDataProp": function (data) {
                            if (data.Cancelable) {
                                return "<a class='btn green'>Cancelar</a>";
                            } else { return ""; }
                        }
                    }
                    //{ "mDataProp": "objRetrasado.Monto" },
                    //{ "mDataProp": "objRetrasado.FechaLimite" },
                ],

                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
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
                    "lengthMenu": "_MENU_  Registros",
                    "paginate": {
                        "previous": "<",
                        "next": ">"
                    },
                    "search": "Buscar Alumno ",
                },
                "createdRow": function (row, data, dataIndex) {
                    if (dataIndex / 2 != 0) {
                        $(row).addClass("BackColor");
                    }

                    if (data.PagoId <= 2588) {
                        $(row).addClass("bold");
                    }
                }
            });
        },
        ConceptoChange() {
            if ($('#slcConceptos').val() == 18) {
                $("#divPeriodo").show();
            } else {
                $("#divPeriodo").hide();
            }

            EsVariable = $('#slcConceptos').find('option:selected');
            var Resp = $(EsVariable[0]).data("esvariable");
            if (Resp == true) {
                $('#btnPopup').click();
            }
        },
        MontoClick() {
            var Monto = $('#txtMonto').val();
            var idSelec = $('#slcConceptos').val();
            if (Monto.length == 0)
                return false;
            $(lstCuotas).each(function () {
                if (idSelec == this.DTOPagoConcepto.PagoConceptoId) {
                    $("#slcConceptos option:selected").text(this.DTOPagoConcepto.Descripcion + " | $" + ReferenciasFn.formato_numero(Monto, 2, ".", ","));
                    this.Monto = Monto;
                    $('#Monto').modal('hide');
                }
            });
        },
        CancelarClick() {
            $('#slcConceptos option')[0].selected = true;
        },
        CloseClick() {
            $('#slcConceptos option')[0].selected = true;
        },
        GeneraClick() {
            var Variables;
            var cFech;
            var Anio, PeriodoId;
            var slcConcepto = $('#slcConceptos');
            slcConcepto = slcConcepto[0].value;
            if (slcConcepto == '-1' || slcConcepto == "") { return false; }
            if (slcConcepto == 18 && $("#slcPeriodo").val() == -1) {
                alertify.alert("Debe seleccionar el periodo.");
                return false;
            }



            var usuario = localStorage.getItem('userAdmin');
            $(lstCuotas).each(function () {
                var objCuota = this;
                if (objCuota.DTOPagoConcepto.PagoConceptoId == slcConcepto) {
                    if (!ReferenciasFn.BuscarTabla(objCuota.DTOPagoConcepto.Descripcion)) {

                        alertify.confirm("<p>¿Esta seguro que desea generar la Referecia?<br><br><hr>", function (e) {
                            if (e) {
                                IndexFn.Block(true); ("Generando Referencia");

                                var Variables2 = "{OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "'}";
                                $.ajax({
                                    type: "POST",
                                    url: "WS/General.asmx/ConsultarPagoConcepto2",
                                    data: Variables2, // the data in form-encoded format, ie as it would appear on a querystring
                                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                                    success: function (data) {
                                        IndexFn.Block(true); ("Generando Referencia");
                                        if (objCuota.DTOPagoConcepto.EsVairable) {
                                            var Variables3 = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'"
                                                + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',Monto:'" + objCuota.Monto + "',UsuarioId:'" + usuario + "'}";
                                            ReferenciasFn.GenerarPago2(Variables3);
                                        } else {

                                            if (slcConcepto == 18) {
                                                Anio = $('#slcPeriodo').find(':selected').data("anio");
                                                PeriodoId = $('#slcPeriodo').find(':selected').data("periodoid");
                                                Variables = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',UsuarioId:'" + usuario + "',Anio:'" + Anio + "',PeriodoId:'" + PeriodoId + "'}";
                                                ReferenciasFn.GenerarPagoC(Variables);
                                            } else {
                                                Variables = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',UsuarioId:'" + usuario + "'}";
                                                ReferenciasFn.GenerarPago(Variables);
                                            }

                                        }
                                    }
                                });


                            }
                        });
                    }
                    else {
                        var Variables2 = "{OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "'}";

                        $.ajax({
                            type: "POST",
                            url: "WS/General.asmx/ConsultarPagoConcepto2",
                            data: Variables2, // the data in form-encoded format, ie as it would appear on a querystring
                            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                            success: function (data) {
                                if (data.d.EsMultireferencia == 1) {
                                    alertify.confirm("<p>¿Esta seguro que desea generar la Referecia?<br><br><hr>", function (e) {
                                        if (e) {
                                            IndexFn.Block(true); ("Generando Referencia");
                                            if (objCuota.DTOPagoConcepto.EsVairable) {
                                                var Variables3 = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'"
                                                    + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',Monto:'" + objCuota.Monto + "',UsuarioId:'" + usuario + "'}";
                                                ReferenciasFn.GenerarPago2(Variables3);
                                            } else {

                                                if (slcConcepto == 18) {
                                                    Anio = $('#slcPeriodo').find(':selected').data("anio");
                                                    PeriodoId = $('#slcPeriodo').find(':selected').data("periodoid");
                                                    Variables = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',UsuarioId:'" + usuario + "',Anio:'" + Anio + "',PeriodoId:'" + PeriodoId + "'}";
                                                    ReferenciasFn.GenerarPagoC(Variables);
                                                } else {
                                                    Variables = "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'" + objCuota.OfertaEducativaId + "',PagoConceptoId:'" + objCuota.PagoConceptoId + "',CuotaId:'" + objCuota.CuotaId + "',UsuarioId:'" + usuario + "'}";
                                                    ReferenciasFn.GenerarPago(Variables);
                                                }

                                            }
                                        }
                                    });
                                } else { alertify.alert("El concepto que selecciono ya esta Generado"); }
                            }
                        });

                    }
                    IndexFn.Block(false);
                    return false;
                }
            });
        },
        GuardarClick() {
            var usuario = localStorage.getItem('userAdmin');
            var Texto = $('#txtComentario').val();
            Texto = $.trim(Texto);
            if (Texto.length > 5) {
                $('#PopComentario').modal('hide');
                $('#txtBar').text("Guardando");
                $('#divBar').modal('show');
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/CancelarPago",
                    data: "{PagoId:'" + PagoId + "',Comentario:'" + $('#txtComentario').val() + "',UsuarioId:'" + usuario + "',Estatus:'" + Estatus + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.d == "Guardado") {
                            Estatus = "";
                            $('#btnBuscar').click();
                        }
                        IndexFn.Block(false);
                    }
                });
            }
            else {
                alertify.alert("Inserte un comentario.");
            }
        },
        GenerarPago(Cuota) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/GenerarPagoB",
                data: Cuota, // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d == null) {
                        return false;
                    } else {
                        var td = '<tr>';
                        td += '<td>' + data.d.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                        td += '<td>' + data.d.Referencia + '</td>';//Referencia               
                        td += '<td>' + '$' + ReferenciasFn.formato_numero(data.d.Promesa, 2, '.', ',') + '</td>';//Monto
                        td += '<td>' + data.d.objNormal.FechaLimite + '</td>';//Fecha
                        td += '</tr>'

                        $('#txtBar').text("Enviando Correo");
                        ReferenciasFn.MandarMail(data.d.PagoId, td);
                    }
                }
            });
        },
        GenerarPagoC(Cuota) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/ReferenciasFn.GenerarPagoC(",
                data: Cuota, // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d == null) {
                        return false;
                    } else {
                        var td = '<tr>';
                        td += '<td>' + data.d.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                        td += '<td>' + data.d.Referencia + '</td>';//Referencia               
                        td += '<td>' + '$' + ReferenciasFn.formato_numero(data.d.Promesa, 2, '.', ',') + '</td>';//Monto
                        td += '<td>' + data.d.objNormal.FechaLimite + '</td>';//Fecha
                        td += '</tr>'

                        $('#txtBar').text("Enviando Correo");
                        ReferenciasFn.MandarMail(data.d.PagoId, td);
                    }
                }
            });
        },
        GenerarPago2(Cuota) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/GenerarPago2B",
                data: Cuota, // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d == null) {
                        return false;
                    } else {
                        var td = '<tr>';
                        td += '<td>' + data.d.DTOCuota.DTOPagoConcepto.Descripcion + '</td>';
                        td += '<td>' + data.d.Referencia + '</td>';//Referencia               
                        td += '<td>' + '$' + ReferenciasFn.formato_numero(data.d.Promesa, 2, '.', ',') + '</td>';//Monto
                        td += '<td>' + data.d.objNormal.FechaLimite + '</td>';//Fecha
                        td += '</tr>'

                        $('#txtBar').text("Enviando Correo");
                        ReferenciasFn.MandarMail(data.d.PagoId, td);
                    }
                }
            });
        },
        MandarMail(PagoId, td) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/EnviarMailId",
                data: "{PagoId:'" + PagoId + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d == false) {
                        ReferenciasFn.MandarMail(PagoId, td);
                    } else {
                        IndexFn.Block(false);
                        //$('#tblReferencias').append(td);                    
                        alertify.alert("Referencia Generada").set('onok', function () {
                            IndexFn.clearAlert();
                            ReferenciasFn.GetAdeudo(AlumnoId, $('#slcOfertaEducativa').val());
                        });
                    }
                }
            });
        },
        formato_numero(numero, decimales, separador_decimal, separador_miles) { // v2007-08-06
            numero = parseFloat(numero);
            if (isNaN(numero)) {
                return "";
            }

            if (decimales !== undefined) {
                // Redondeamos
                numero = numero.toFixed(decimales);
            }

            // Convertimos el punto en separador_decimal
            numero = numero.toString().replace(".", separador_decimal !== undefined ? separador_decimal : ",");

            if (separador_miles) {
                // Añadimos los separadores de miles
                var miles = new RegExp("(-?[0-9]+)([0-9]{3})");
                while (miles.test(numero)) {
                    numero = numero.replace(miles, "$1" + separador_miles + "$2");
                }
            }

            return numero;
        },
        BuscarTabla(Descripcion) {
            var respu = false;
            $('#tblReferencias tbody tr').each(function () {
                var td = this.childNodes[0];
                if (td.innerHTML == Descripcion) {
                    respu = true;
                    return false;
                }
            });
            return respu;
        }
    };

    ReferenciasFn.init();

});