$(document).ready(function () {
    $('#btnBuscar').click(function () {
        var fid = $('#txtFolio').val();
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + fid + '"}',
            dataType: 'json',
            success: function (data) {
                CargarTiposPagos(fid);
                
            }
        });
    });
    function CargarTiposPagos(AlumnoId) {
        var n4;
        var oferta;
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarPagosPlan",
            data: "{AlumnoId:'" + AlumnoId + "',OfertaEducativaId:'"+oferta+"'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    if (this.Pagos == 4) { n4 = this.PagoPlanId; }
                    option.text(this.PlanPago);
                    option.val(this.PagoPlanId);

                    $("#slcSistemaPago").append(option);
                });
                $("#slcSistemaPago").val(n4);
            }
        });
    }
    $('#slcPlantel').change(function () {
        $("#slcOferta").empty();
        var plantel = $('#slcPlantel').val();
        if (plantel == -1) { $("#slcOfertaEducativa").empty(); $("#slcSistemaPago").empty(); return false; }
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativaTipo",
            data: "{Plantel:'" + plantel + "'}", // the data in form-encoded format, ie as it would appear on a querystring
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
        Limpiar();
        $('#divTablaDescuento').hide();
        $("#slcOfertaEducativa").empty();
        var plantel = $('#slcPlantel').val();
        if (plantel == -1) { return false; }
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcOfertaEducativa").append(optionP);
        var tipo = $("#slcOferta");
        tipo = tipo[0].value;

        if (tipo != -1) {
            $('#lblOFerta').html(tipo == 1 ? 'Licenciatura' : tipo == 2 ? 'Especialidad' : tipo == 3 ? 'Meastría' : tipo == 4 ? 'Idioma' : tipo == 5 ? 'Doctorado' : ' ');
            if (tipo == 4) {
                $('#txtDescuentoBec').val(0).trigger('change');
                $('#divInscripcion').hide();
                $('#divExamen').hide();
                $('#divMaterial').show();
            } else {
                $('#txtDescuentoBec').val(50).trigger('change');
                $('#divInscripcion').show();
                if (Mas != 1) { $('#divExamen').show(); }
                $('#divMaterial').hide();
            }

            $.ajax({
                type: "POST",
                url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativa",
                data: "{tipoOferta:'" + tipo + "',Plantel:'" + plantel + "'}", // the data in form-encoded format, ie as it would appear on a querystring
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

                            $("#slcOfertaEducativa").append(option);
                        });
                    } else {
                        $("#slcOfertaEducativa").append(optionP);
                    }

                }
            });
            PlanPago();
        } else {
            $("#slcOfertaEducativa").append(optionP);
            $("#slcSistemaPago").empty();
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
            monto = Math.round(monto);
            $('#txtPagarCol').text('$' + String(monto));
            //$('#txtDescuentoBec').change();
        } else {
            total = Number($('#txtcuotaCol').text().replace('$', ''));
            $('#txtcuotaCol').text('$' + (total * 4));
            monto = (total * 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
            monto = (total * 4) - monto;
            monto = Math.round(monto);
            $('#txtPagarCol').text('$' + String(monto));
        }
    });
    $('#slcOfertaEducativa').change(function () {
        var Idioma = $('#slcOfertaEducativa').val();
        var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();

        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/BuscarLengua",
            data: "{AlumnoId:'" + fid + "',Idioma:'" + Idioma + "',Periodo:'" + Periodo + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var bResp = data.d;
                if (bResp == "0") {
                    if ($('#slcOferta').val() == 4) {
                        $('#divTablaDescuento').show();
                        $.ajax({
                            type: "POST",
                            url: "../WebServices/WS/Descuentos.asmx/TraerDescuentosIdiomas",
                            data: "{'Idioma':" + Idioma + ",Periodo:'" + Periodo + "'}",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                if (data.d.length > 0) {
                                    var temtxt;
                                    var MaxDes;
                                    MaxDes = data.d[0].Descuento.MontoMaximo;
                                    //$('#txtDescuentoBec').attr("data-val-max", MaxDes);
                                    temtxt = $('#txtDescuentoBec').data();
                                    temtxt.valMax = MaxDes;
                                    $('#txtcuotaCol').text('$' + data.d[0].Monto);
                                    //$('#txtDescuentoBec').val(MaxDes);
                                    monto = data.d[0].Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                                    monto = data.d[0].Monto - monto;
                                    monto = Math.round(monto);
                                    $('#txtPagarCol').text('$' + String(monto));
                                    //$('#txtDescuentoBec').change()

                                    MaxDes = data.d[1].Descuento.MontoMaximo;
                                    temtxt = $('#txtDescuentoCred').data();
                                    temtxt.valMax = MaxDes;
                                    //$('#txtDescuentoCred').val(MaxDes);
                                    $('#txtcuotaCred').text('$' + data.d[1].Monto);
                                    monto = (data.d[1].Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                                    monto = data.d[1].Monto - monto;
                                    monto = Math.round(monto);
                                    $('#txtPagarCred').text('$' + String(monto));
                                    //$('#txtDescuentoCred').change();
                                    CrearTabla(Periodo);
                                }
                                else {
                                    Limpiar();
                                }
                            }
                        });

                    } else {
                        if ($('#slcOferta').val() == 2 || $('#slcOferta').val() == 3) {
                            $.ajax({
                                type: "POST",
                                url: "../WebServices/WS/Descuentos.asmx/ConsultarAdeudo",
                                data: '{AlumnoId:' + fid + '}',
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "Debe") {
                                        alertify.alert('El alumno ' + $('#txtNombre').val() + ' tiene adeudo, favor de pasar a Control Administrativo para resolver su situación financiera.');
                                        $('#slcOfertaEducativa').val(-1);
                                    } else {
                                        DescuentosPeriodos(Idioma, Periodo);
                                    }
                                }
                            });
                        } else {
                            DescuentosPeriodos(Idioma, Periodo);
                        }
                    }
                } else {
                    alertify.alert('El alumno ya tiene registrado la opción seleccionada');
                    $('#slcOfertaEducativa').val("-1");
                }
            }
        });

    });
    function DescuentosPeriodos(Idioma, Periodo) {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Descuentos.asmx/TraerDescuentosPeriodo",
            data: "{'OfertaEducativaId':" + Idioma + ",Periodo:'" + Periodo + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var Sispago = $('#slcSistemaPago option:selected').html();
                var monto;
                var temtxt;
                var MaxDes;
                if (Sispago.search("4") != -1) {
                    MaxDes = data.d[1].Descuento.MontoMaximo;

                    //$('#txtDescuentoBec').attr("data-val-max", MaxDes);
                    temtxt = $('#txtDescuentoBec').data();
                    temtxt.valMax = MaxDes;
                    //$('#txtDescuentoBec').val(MaxDes);
                    $('#txtcuotaCol').text('$' + data.d[1].Monto);
                    monto = data.d[1].Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                    monto = data.d[1].Monto - monto;
                    monto = Math.round(monto);
                    $('#txtPagarCol').text('$' + String(monto));
                    //$('#txtDescuentoBec').change();
                } else {
                    MaxDes = data.d[1].Descuento.MontoMaximo;
                    //$('#txtDescuentoBec').attr("data-val-max", MaxDes);
                    temtxt = $('#txtDescuentoBec').data();
                    temtxt.valMax = MaxDes;
                    //$('#txtDescuentoBec').val(MaxDes);
                    $('#txtcuotaCol').text('$' + (data.d[1].Monto * 4));
                    monto = (data.d[1].Monto * 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
                    monto = (data.d[1].Monto * 4) - monto;
                    monto = Math.round(monto);
                    $('#txtPagarCol').text('$' + String(monto));
                    //$('#txtDescuentoBec').change();
                }
                MaxDes = data.d[0].Descuento.MontoMaximo;
                temtxt = $('#txtDescuentoIns').data();
                temtxt.valMax = MaxDes;
                //$('#txtDescuentoIns').val(MaxDes);
                $('#txtcuotaIn').text('$' + data.d[0].Monto);
                monto = (data.d[0].Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                monto = data.d[0].Monto - monto;
                monto = Math.round(monto);
                $('#txtPagarIn').text('$' + String(monto));
                //$('#txtDescuentoIns').change();

                MaxDes = data.d[2].Descuento.MontoMaximo;
                temtxt = $('#txtDescuentoExa').data();
                temtxt.valMax = MaxDes;
                //$('#txtDescuentoExa').val(MaxDes);
                $('#txtcuotaExa').text('$' + data.d[2].Monto);
                monto = (data.d[2].Monto * (parseFloat($('#txtDescuentoExa').val()) / 100));
                monto = data.d[2].Monto - monto;
                monto = Math.round(monto);
                $('#txtPagarExa').text('$' + String(monto));
                //$('#txtDescuentoExa').change();

                MaxDes = data.d[3].Descuento.MontoMaximo;
                temtxt = $('#txtDescuentoCred').data();
                temtxt.valMax = MaxDes;
                //$('#txtDescuentoCred').val(MaxDes);
                $('#txtcuotaCred').text('$' + data.d[3].Monto);
                monto = (data.d[3].Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                monto = data.d[3].Monto - monto;
                monto = Math.round(monto);
                $('#txtPagarCred').text('$' + String(monto));
                //$('#txtDescuentoCred').change();

            },
            error: function (request, status, error) {
                alertify.alert("No hay cuotas dadas de alta para el periodo seleccionado");
            }
        });
    }
    function CrearTabla(Periodo) {
        var th;
        var num;
        var fila = '<tr id="tr1">';
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/PeriodosCompletos",
            data: "{Periodo:'" + Periodo + "',ofertaId:'" + $("#slcOfertaEducativa").val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                var row = document.getElementById("tr1");
                if (row != null) {
                    padre = row.parentNode;
                    padre.removeChild(row);
                }
                var meses = data.d.lstSubPeriodo.length;
                for (i = 0; i < meses; i++) {
                    num = i + 1;
                    th = '#thm' + num;
                    $(th).html('<i class="fa fa-calendar"></i>&nbsp;' + data.d.lstSubPeriodo[i].Mes.Descripcion);
                    if (data.d.lstSubPeriodo[i].Mes.MontoLengua != null) {
                        MesP[i] = data.d.lstSubPeriodo[i].Mes.MontoLengua.Cuota.Monto;
                        fila += '<td id="' + 'mes' + i + '">' + '$' + data.d.lstSubPeriodo[i].Mes.MontoLengua.Cuota.Monto + '</td>';
                    } else {
                        MesP[i] = 0.00;
                        fila += '<td id="' + 'mes' + i + '">$0.00</td>';
                    }
                }
                fila += '</tr>';
                $('#tblDescuentos').append(fila);
                $('#txtDescuentoBec').keyup();
            }
        });
    }
});