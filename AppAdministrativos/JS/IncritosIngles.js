﻿$(function init() {
    //$.cookie('userAdmin', 6883, { expires: 1 });
    $('#Contenedor').hide();
    var EsLenguas = false;
    var Usuario;
    var MesP = [];
    var MItable;
    var fid;
    var Mas;
    var form = $('#formPopup');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    form.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block help-block-error', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        rules: {
            txtNombrePrepa: {
                required: true,
                maxlength: 100,
                minlength: 4
            },
            txtPromedio: {
                required: true,
                number: true,
                minlength: 1,
                max: 10
            },
            txtAñoT: {
                required: true,
                minlength: 4,
                maxlength: 4
            },
            txtMesT: {
                required: true,
                number: true,
                minlength: 1,
                maxlength: 2,
                min: 1,
                max: 12

            },
            slcArea: {
                min: 1
            },
            txtUni: {
                maxlength: 100,
                minlength: 2
            },
            slcNacionalidadPrep: {
                required: true,
                min: 1
            },
            slcEstadoPais: {
                required: true,
                min: 1
            },
            slcMedio: {
                required: true,
                min: 1
            },
        },
        invalidHandler: function (event, validator) { //display error alert on form submit              
            success.hide();
            error.show();
        },
        errorPlacement: function (error, element) { // render error placement for each input type
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass('fa-check').addClass("fa-warning");
            icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
        },
        highlight: function (element) { // hightlight error inputs
            $(element)
                .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group   
        },

        unhighlight: function (element) { // revert the change done by hightlight
        },

        success: function (label, element) {
            var icon = $(element).parent('.input-icon').children('i');
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
            icon.removeClass("fa-warning").addClass("fa-check");
        },

        submitHandler: function (form) {
            success.show();
            error.hide();
        }
    });
    Cargar();


    $("#tbAlumnos").on("click", "a", function () {
        Mas = undefined;
        fid = MItable.fnGetData(this.parentNode.parentNode, 0);
        var Fecha;
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + fid + '"}',
            dataType: 'json',
            success: function (data) {
                EsLenguas = false;

                if (data.d.lstAlumnoInscrito.length == 0) {
                    if (data.d.lstAlumnoInscrito[0].OfertaEducativa.OfertaEducativaTipoId === 4) {
                        EsLenguas = true;
                    }
                }
                $(data.d.lstAlumnoInscrito).each(function () {
                    if (this.OfertaEducativa.OfertaEducativaTipoId != 4) { Mas = 1; }
                });

                if (Mas != 'undefined') {
                    $('#divExamen').hide();
                    EsLenguas = true;
                }
                $('#txtAlumnoId').val(fid);
                Fecha = new Date(parseInt(data.d.DTOAlumnoDetalle.FechaNacimiento.slice(6)));
                Fecha = new Date(Fecha);
                $('#txtNombre').val(data.d.Nombre);
                $('#txtPaterno').val(data.d.Paterno);
                $('#txtMaterno').val(data.d.Materno);
                $('#txtFechaNacimiento').val(Fecha.getDate() + '/' + parseInt(Fecha.getMonth() + 1) + '/' + Fecha.getFullYear());
                $('#txtOferta').val(data.d.AlumnoInscrito.OfertaEducativa.Descripcion);
                $('#txtFechaRegistro').val(data.d.FechaRegistro);
                $('#Encabezado').hide();
                $('#Contenedor').show();
                $('#DivDescuentos').show();
                Platel();
            }
        });
    });
    $('#slcPlantel').change(function () {
        $("#slcOferta").empty();
        var plantel = $('#slcPlantel').val();
        if (plantel == -1) { $("#slcOfertaEducativa").empty(); $("#slcSistemaPago").empty(); return false; }
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarOfertaEducativaTipo",
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
                $('#DivCol').hide();
                $('#divInscripcion').hide();
                $('#divExamen').hide();
                $('#divCredencial').hide();
                $('#divMaterial').show();
                EsLenguas = false;
            } else {
                if (tipo === "1") {
                    $('#lblLugarP').text("Lugar donde estudio la preparatoria");
                    $('#hOfertaTitulo').text('Preparatoria de Procedencia');
                    $('#lblOfertaTitulo2').text('Preparatoria de Procedencia');
                    $('#txtNombrePrepa').val("");
                } else if (tipo === "2" || tipo === "3") {
                    $('#hOfertaTitulo').text('Universidad de Procedencia');
                    $('#lblOfertaTitulo2').text('Universidad de Procedencia');
                    $('#lblLugarP').text("Lugar donde estudio la Universidad");
                    $('#txtNombrePrepa').val("universidad YMCA");
                }
                $('#txtDescuentoBec').val(50).trigger('change');
                $('#DivCol').show();
                $('#divInscripcion').show();
                $('#divCredencial').show();
                if (Mas != 1) { $('#divExamen').show(); }
                $('#divMaterial').hide();
            }

            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarOfertaEducativa",
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
        if (Sispago.search("4") != -1 || Sispago.search("6") != -1) {
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
    $('#btnCancelar').on('click', function () {
        $('#txtcuotaCol').text('');
        $('#txtPagarCol').text('');
        $('#txtcuotaCred').text('');
        $('#txtPagarCred').text('');
        $('#txtJustificacionBec').val('');
        $('#txtJustificacionCred').val('');
        $('#chkMaterial')[0].checked = false;
        $('#chkEsEmpresa')[0].checked = false;
        $('#Encabezado').show();
        $('#Contenedor').hide();
        //$('#tblDescuentos').hide();
    });
    $('#slcOfertaEducativa').change(function () {
        var Idioma = $('#slcOfertaEducativa').val();
        var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();

        $.ajax({
            type: "POST",
            url: "WS/General.asmx/BuscarLengua",
            data: "{AlumnoId:'" + fid + "',Idioma:'" + Idioma + "',Periodo:'" + Periodo + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var bResp = data.d;
                if (bResp == "0") {
                    if ($('#slcOferta').val() == 4) {
                        //$('#divTablaDescuento').show();
                        //$.ajax({
                        //    type: "POST",
                        //    url: "WS/Descuentos.asmx/TraerDescuentosIdiomas",
                        //    data: "{'Idioma':" + Idioma + ",Periodo:'" + Periodo + "'}",
                        //    contentType: "application/json; charset=utf-8",
                        //    success: function (data) {
                        //        if (data.d.length > 0) {
                        //            var temtxt;
                        //            var MaxDes;
                        //            MaxDes = data.d[0].Descuento.MontoMaximo;
                        //            //$('#txtDescuentoBec').attr("data-val-max", MaxDes);
                        //            temtxt = $('#txtDescuentoBec').data();
                        //            temtxt.valMax = MaxDes;
                        //            $('#txtcuotaCol').text('$' + data.d[0].Monto);
                        //            //$('#txtDescuentoBec').val(MaxDes);
                        //            monto = data.d[0].Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                        //            monto = data.d[0].Monto - monto;
                        //            monto = Math.round(monto);
                        //            $('#txtPagarCol').text('$' + String(monto));
                        //            //$('#txtDescuentoBec').change()

                        //            MaxDes = data.d[1].Descuento.MontoMaximo;
                        //            temtxt = $('#txtDescuentoCred').data();
                        //            temtxt.valMax = MaxDes;
                        //            //$('#txtDescuentoCred').val(MaxDes);
                        //            $('#txtcuotaCred').text('$' + data.d[1].Monto);
                        //            monto = (data.d[1].Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                        //            monto = data.d[1].Monto - monto;
                        //            monto = Math.round(monto);
                        //            $('#txtPagarCred').text('$' + String(monto));
                        //            //$('#txtDescuentoCred').change();
                        //            CrearTabla(Periodo);
                        //        }
                        //        else {
                        //            Limpiar();
                        //        }
                        //    }
                        //});

                    } else {
                        //if ($('#slcOferta').val() == 2 || $('#slcOferta').val() == 3) {
                        //    $.ajax({
                        //        type: "POST",
                        //        url: "WS/Descuentos.asmx/ConsultarAdeudo",
                        //        data: '{AlumnoId:' + fid + '}',
                        //        contentType: "application/json; charset=utf-8",
                        //        success: function (data) {
                        //            if (data.d == "Debe") {
                        //                alertify.alert('El alumno ' + $('#txtNombre').val() + ' tiene adeudo, favor de pasar a Control Administrativo para resolver su situación financiera.');
                        //                $('#slcOfertaEducativa').val(-1);
                        //            } else {
                        //                DescuentosPeriodos(Idioma, Periodo);
                        //            }
                        //        }
                        //    });
                        //} else {
                            DescuentosPeriodos(Idioma, Periodo);
                        //}
                    }
                } else {
                    alertify.alert('El alumno ya tiene registrado la opción seleccionada');
                    $('#slcOfertaEducativa').val("-1");
                    Limpiar();
                }
            }
        });

    });
    function DescuentosPeriodos(Idioma, Periodo) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/TraerDescuentosPeriodo",
            data: "{'OfertaEducativaId':" + Idioma + ",Periodo:'" + Periodo + "'}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var Sispago = $('#slcSistemaPago option:selected').html();
                var monto;
                var temtxt;
                var MaxDes;
                if (Sispago.search("4") != -1 || Sispago.search("6") != -1) {
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
                Limpiar();
            }
        });
    }
    function Limpiar() {
        $('#txtcuotaCol').text('');
        $('#txtPagarCol').text('');
        $('#txtcuotaCred').text('');
        $('#txtPagarCred').text('');
        $('#txtcuotaIn').text('');
        $('#txtPagarIn').text('');
        $('#txtcuotaExa').text('');
        $('#txtPagarExa').text('');
        $('#chkMaterial')[0].checked = false;
        $('#chkEsEmpresa')[0].checked = false;
    }

    $('#chkEsEmpresa').on('click', function () {
        if (this.checked) {
            $('#DivDescuentos').hide();
        } else { $('#DivDescuentos').show();}
    });

    $('#btnGuardar').on('click', function () {
        if ($('#slcOfertaEducativa').val() == '-1') {
            alertify.alert("Seleccione un " + $('#lblOFerta').html() + " para poder continar", function () {
                return true;
            });
            return false;
        }
        if (EsLenguas) {
            $('#Antecedentes').modal('show');
        }
        else {
            $('#Load').modal('show');
            if (!form.valid()) { return false; }
            Usuario = $.cookie('userAdmin');
            if (jQuery.type(Usuario) === "undefined") {
                return false;
            }
            var Campos = {
                'AlumnoId': fid,//0
                'OfertaEducativa': $('#slcOfertaEducativa').val(),//1
                'Turno': $('#slcTurno').val(),//2
                'Periodo': $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html(),//3
                'SistemaPago': $('#slcSistemaPago').val(),//4
                'DescuentoBec': $('#txtDescuentoBec').val(),//5
                'JustificacionBec': $('#txtJustificacionBec').val() == '' ? 'null' : $('#txtJustificacionBec').val(),//6
                'Credencial': $('#txtDescuentoCred').val(),//7
                'JustificacionCred': $('#txtJustificacionCred').val() == '' ? 'null' : $('#txtJustificacionCred').val(),//8
                'Material': $('#chkMaterial')[0].checked,//9
                'EsEmpresa': $('#chkEsEmpresa')[0].checked,//10
                'DescuentoExamen': Mas == 1 ? '-1' : $('#txtDescuentoExa').val(),//11
                'JustificacionExam': $('#txtJustificacionExa').val() == '' ? 'null' : $('#txtJustificacionExa').val(),//12
                'DescuentoIns': $('#txtDescuentoIns').val(),//13
                'JustificacionIns': $('#txtJustificacionIns').val() == '' ? 'null' : $('#txtJustificacionIns').val(),//14
                'Usuario': Usuario//15
            };
            CargadInfoAntecedentes.GuardarDescuentos(Campos);
        }
        //$('#btnGuardarAntecedente').click();
    });
    $('#btnGuardarAntecedente').on('click', function () {
        if ($('#slcOfertaEducativa').val() == '-1') { alertify.alert("Seleccione un " + $('#lblOFerta').html() + " para poder continar"); return false; }
        if (!form.valid()) { return false; }
        Usuario = $.cookie('userAdmin');
        if (jQuery.type(Usuario) === "undefined") {
            return false;
        }
        var Resultado;

        var Campos = {
            'AlumnoId': fid,//0
            'OfertaEducativa': $('#slcOfertaEducativa').val(),//1
            'Turno': $('#slcTurno').val(),//2
            'Periodo': $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html(),//3
            'SistemaPago': $('#slcSistemaPago').val(),//4
            'DescuentoBec': $('#txtDescuentoBec').val(),//5
            'JustificacionBec': $('#txtJustificacionBec').val() == '' ? 'null' : $('#txtJustificacionBec').val(),//6
            'Credencial': $('#txtDescuentoCred').val(),//7
            'JustificacionCred': $('#txtJustificacionCred').val() == '' ? 'null' : $('#txtJustificacionCred').val(),//8
            'Material': $('#chkMaterial')[0].checked,//9
            'EsEmpresa': $('#chkEsEmpresa')[0].checked,//10
            'DescuentoExamen': Mas == 1 ? '-1' : $('#txtDescuentoExa').val(),//11
            'JustificacionExam': $('#txtJustificacionExa').val() == '' ? 'null' : $('#txtJustificacionExa').val(),//12
            'DescuentoIns': $('#txtDescuentoIns').val(),//13
            'JustificacionIns': $('#txtJustificacionIns').val() == '' ? 'null' : $('#txtJustificacionIns').val(),//14
            'Usuario': Usuario//15
        };
        var Antecedentes = {
            AlumnoId: fid,
            AntecedenteTipoId: $('#slcOferta').val(),
            UsuarioId: Usuario,
            Procedencia: $('#txtNombrePrepa').val(),
            MesId: $('#txtMesT').val(),
            Anio: $('#txtAñoT').val(),
            AreaAcademicaId: $('#slcArea').val(),
            Promedio: $('#txtPromedio').val(),
            EsEquivalencia: $('#chkUni')[0].checked,
            EscuelaEquivalencia: $('#txtUni').val(),
            PaisId: $('#slcNacionalidadPrep').val() === "1" ? 146 : $('#slcEstadoPais').val(),
            EntidadFederativaId: $('#slcNacionalidadPrep').val() === "1" ? $('#slcEstadoPais').val() : 33,
            EsTitulado: $('#chkUniSi')[0].checked ? true : false,
            TitulacionMedio: $('#txtUniMotivo').val(),
            MedioDifusionId: $('#slcMedio').val(),
        };
        Antecedentes = JSON.stringify(Antecedentes);
        $('#Antecedentes').modal('hide');
        alertify.confirm("¿Esta seguro que desea guardar los cambios?",
            function (e) {
                $('#Load').modal('show');
                CargadInfoAntecedentes.GuardarAntecedentes(Antecedentes, Campos);
            },
            function () {
                $('#Antecedentes').modal('show');
            });

    });
    function GuardarDocumentoIngles(AlumnoId, OfertaEducativa) {
        var data = new FormData();
        var flIns = $('#BecArchivo');

        flIns = flIns[0].files[0];
        data.append("DocBeca", flIns);
        data.append("AlumnoId", AlumnoId);
        data.append("OfertaEducativaId", OfertaEducativa);
        var request = new XMLHttpRequest();
        request.open("POST", 'WS/Descuentos.asmx/GuardarDocumentosIngles', true);
        request.send(data);

    }
    function GuardarDocumentos(Beca, Insc, Exam) {
        var data = new FormData();
        var flIns = $('#BecArchivo'); // FileList object

        flIns = flIns[0].files[0];
        data.append("DocBeca", flIns);
        data.append("DescuentoIdB", Beca);
        flIns = $('#InsArchivo');
        flIns = flIns[0].files[0];
        data.append("DocInscipcion", flIns);
        data.append("DescuentoIdI", Insc);
        flIns = $('#ExamenArchivo');
        flIns = flIns[0].files[0];
        data.append("DocExamen", flIns);
        data.append("DescuentoExam", Exam);


        var request = new XMLHttpRequest();
        request.open("POST", 'WS/Descuentos.asmx/GuardarDocumentos', true);
        request.send(data);

    }
    $('#slcPeriodo').change(function () {
        $('#slcOfertaEducativa').change();
    });
    $('#txtDescuentoBec').keyup(function () {
        var Maximo = $('#txtDescuentoBec').data();
        if (Maximo.valMax < $('#txtDescuentoBec').val()) { $('#txtDescuentoBec').val(Maximo.valMax); return false; }
        var monto = CalcularDescuento($('#txtcuotaCol').text().replace('$', ''), $('#txtDescuentoBec').val());
        $('#txtPagarCol').text('$' + String(monto));
        RecalculaTabla($('#txtDescuentoBec').val());
    });
    $('#txtDescuentoBec').knob({
        change: function (val) {
            var Maximo = $('#txtDescuentoBec').data();
            if (Maximo.valMax < val) { $('#txtDescuentoBec').val(Maximo.valMax); return false; }
            var monto = CalcularDescuento($('#txtcuotaCol').text().replace('$', ''), val);
            $('#txtPagarCol').text('$' + String(monto));
            RecalculaTabla(val);
        }
    });
    $('#txtDescuentoCred').keyup(function () {
        var Maximo = $('#txtDescuentoCred').data();
        if (Maximo < $('#txtDescuentoCred').val()) { $('#txtDescuentoCred').val(Maximo.valMax); return false; }
        var monto = CalcularDescuento($('#txtcuotaCred').text().replace('$', ''), $('#txtDescuentoCred').val());
        $('#txtPagarCred').text('$' + String(monto));
    });
    $('#txtDescuentoCred').knob({
        change: function (val) {
            var Maximo = $('#txtDescuentoCred').data();
            if (Maximo.valMax < val) { $('#txtDescuentoCred').val(Maximo.valMax); return false; }
            var monto = CalcularDescuento($('#txtcuotaCred').text().replace('$', ''), val);
            $('#txtPagarCred').text('$' + String(monto));
        }
    });
    $('#txtDescuentoIns').keyup(function () {
        var Maximo = $('#txtDescuentoIns').data();
        if (Maximo.valMax < $('#txtDescuentoIns').val()) { $('#txtDescuentoIns').val(Maximo.valMax); return false; }
        $('#txtPagarIn').text('$' +
            String(CalcularDescuento($('#txtcuotaIn').text().replace('$', ''), $('#txtDescuentoIns').val())));
    });
    $('#txtDescuentoIns').knob({
        change: function (val) {
            var Maximo = $('#txtDescuentoIns').data();
            if (Maximo.valMax < val) { $('#txtDescuentoIns').val(Maximo.valMax); return false; }
            $('#txtPagarIn').text('$' +
           String(CalcularDescuento($('#txtcuotaIn').text().replace('$', ''), val)));
        }
    });
    $('#txtDescuentoExa').keyup(function () {
        var Maximo = $('#txtDescuentoExa').data();
        if (Maximo.valMax < $('#txtDescuentoExa').val()) { $('#txtDescuentoExa').val(Maximo.valMax); return false; }
        $('#txtPagarExa').text('$' +
            String(CalcularDescuento($('#txtcuotaExa').text().replace('$', ''), $('#txtDescuentoExa').val())));
    });
    $('#txtDescuentoExa').knob({
        change: function (val) {
            var Maximo = $('#txtDescuentoExa').data();
            if (Maximo.valMax < val) { $('#txtDescuentoExa').val(Maximo.valMax); return false; }
            $('#txtPagarExa').text('$' +
           String(CalcularDescuento($('#txtcuotaExa').text().replace('$', ''), val)));
        }
    });
    function Cargar() {
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumnos',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (Respuesta) {
                MItable = $('#tbAlumnos').dataTable({
                    "aaData": Respuesta.d,
                    "aoColumns": [
                        {
                            "mDataProp": "AlumnoId", "Nombre": "AlumnoId",
                            Stextalign: 'center'
                        },
                        {
                            "mDataProp": "Nombre",
                            "mRender": function (data) {
                                return "<a href=''onclick='return false;'>" + data + " </a> ";
                            },
                            sWidth: '350px' 
                        },
                        { "mDataProp": "FechaRegistro" },
                        { "mDataProp": "Descripcion" },
                        //{ "mDataProp": "FechaSeguimiento" },
                        { "mDataProp": "Usuario" }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "info": false,
                    "async": true,
                    "bDestroy": true,
                    "language": {
                        "lengthMenu": "_MENU_  Registros",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "order": [[2, "desc"]]
                });
                var fil = $('#tbAlumnos_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                $('#Load').modal('hide');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
            }
        });
    }
    //$('#btnNuevo').on('click', function () {
    //    $('#divDinamico').empty();
    //    var url = $(this).attr("href");
    //    $('#divDinamico').load(url);
    //    return false;
    //});
    function Platel() {
        $('#slcPlantel').empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarPlantel",
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
                $('#divTablaDescuento').hide();
                $("#slcPlantel").val('1');
                $('#slcPlantel').change();
            }
        });
    }
    function PlanPago() {
        $("#slcSistemaPago").empty();
        var OFerta = $('#slcOferta').val();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarPagosPlanLenguas",
            data: "{Oferta:'" + OFerta + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.PlanPago);
                    option.val(this.PagoPlanId);

                    $("#slcSistemaPago").append(option);
                });
            }
        });
    }
    function CrearTabla(Periodo) {
        var th;
        var num;
        var fila = '<tr id="tr1">';
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/PeriodosCompletos",
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
    function CalcularDescuento(Monto, Descuento) {
        var Redondeado;
        Monto = parseFloat(Monto);
        Descuento = (Monto * (Descuento / 100))
        Redondeado = Monto - Descuento;
        Redondeado = Math.round(Redondeado);
        return Redondeado;
    }
    function RecalculaTabla(monto) {
        var filx;
        var descu;
        for (i = 0; i < 4; i++) {
            if (MesP[i] != '0.00') {
                filx = $('#mes' + i);
                descu = CalcularDescuento(MesP[i], monto);
                $(filx).text('$' + String(descu));
            }
        }
    }


    var CargadInfoAntecedentes = {
        AreasCursadas: function () {
            $('#slcArea').empty();
            var option1 = $(document.createElement('option'));

            option1.text("--Seleccionar--");
            option1.val(-1);
            $("#slcArea").append(option1);
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ObtenerAreas",
                data: "",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.AreaAcademicaId);

                        $("#slcArea").append(option);
                    });
                }
            });
        },
        Paises: function () {
            $('#slcEstadoPais').empty();
            var option1 = $(document.createElement('option'));

            option1.text("--Seleccionar--");
            option1.val(-1);
            $("#slcEstadoPais").append(option1);
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarPaises",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.Descripcion);
                        option.val(this.PaisId);

                        $("#slcEstadoPais").append(option);
                    });
                }
            });

        },
        Estados: function () {
            $('#slcEstadoPais').empty();
            var option1 = $(document.createElement('option'));

            option1.text("--Seleccionar--");
            option1.val(-1);
            $("#slcEstadoPais").append(option1);
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarEntidadFederativa",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.EntidadFederativaId);

                        $("#slcEstadoPais").append(option);
                    });
                    $("#slcEstadoPais").val(9);

                }
            });
        },
        MedioDifusion: function () {
            $('#slcMedio').empty();
            var option1 = $(document.createElement('option'));

            option1.text("--Seleccionar--");
            option1.val(-1);
            $("#slcMedio").append(option1);
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/TraerListaMedios",
                data: "{}", 
                contentType: "application/json; charset=utf-8", 
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.MedioDifusionId);

                        $("#slcMedio").append(option);
                    });
                }
            });
        },
        GuardarDescuentos: function (Campos) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/GuardarIdioma",
                data: JSON.stringify(Campos), // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                datatype: JSON,
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var Respuesta = data.d[0];

                    if (Respuesta == "Guardado") {
                        GuardarDocumentoIngles(fid, $('#slcOfertaEducativa').val());
                        Resultado = "guardado";
                    } else if (data.d != null) {
                        var res2 = data.d[0];
                        if (res2 !== undefined) {
                            GuardarDocumentos(data.d[1], data.d[0], data.d[2]);
                            Resultado = "guardado";
                        } else { return "Fallo"; }
                    }
                    if (Resultado == "guardado") {
                        alertify.alert("Alumno Guardado", function () {
                            CargadInfoAntecedentes.Email(fid);
                        });

                    } else {
                        alertify.alert("Error no se guardaron los cambios, intente de nuevo", function () {
                            $('#Load').modal('hide');
                            return false;
                        });
                    }
                }
            });
        },
        GuardarAntecedentes: function (Antecedente, Campos) {
            $.ajax({
                type: "POST",
                url: "WS/Alumno.asmx/GuardarAntecedentes",
                data: Antecedente,
                datatype: JSON,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d) {
                        $('#Antecedentes').modal('hide');
                        CargadInfoAntecedentes.GuardarDescuentos(Campos);
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Error no se guardaron los cambios, intente de nuevo", function () {
                            $('#Antecedentes').modal('show');                            
                            return false;
                        });
                    }
                }
            });
        },
        Email: function (alumnoid) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/EnviarMail2",
                data: "{listaID:'" + alumnoid + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d.length > 1) {
                        CargadInfoAntecedentes.Email2(alumnoid);
                    } else {
                        var extramail = "<p>" + "Se ha enviado un mail al Alumno en el cual podra visualizar el reglamento escolar." + "</p>";
                        alertify.alert(extramail, function () {
                            $('#Encabezado').show();
                            $('#Contenedor').hide();
                            Cargar();
                        });
                    }
                }
            });
        },
        Email2: function (Alumnoid) {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/EnviarMail2",
                data: "{listaID:'" + Alumnoid + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d.length > 1) {
                        CargadInfoAntecedentes.Email2(Alumnoid);
                    } else {                            
                            var extramail = "<p>" + "Se ha enviado un mail al Alumno en el cual podra visualizar el reglamento escolar." + "</p>";
                            alertify.alert(extramail, function () {
                                $('#Encabezado').show();
                                $('#Contenedor').hide();
                                Cargar();
                            });
                        
                    }
                }
            });
        }
    }
    
    CargadInfoAntecedentes.AreasCursadas();
    CargadInfoAntecedentes.MedioDifusion();
    $('#slcNacionalidadPrep').on('change', function () {
        $('#Load').modal('show');
        if (this.value === "1") { CargadInfoAntecedentes.Estados(); }
        else if (this.value === "2") { CargadInfoAntecedentes.Paises(); }
        else { $('#slcEstadoPais').empty(); }
        $('#Load').modal('hide');
    });
    $('#chkUniSi').on('click', function () {
        if (this.checked) {
            $('#txtUniMotivo').prop('disabled', false);
        }
    });
    $('#chkUniNo').on('click', function () {
        if (this.checked) {
            $('#txtUniMotivo').prop('disabled', true);
        }
    });
    $('#chkUni').on('click', function () {
        if (this.checked) { $('#txtUni').prop('disabled', false); }
        else { $('#txtUni').prop('disabled', true); }
    });
});
 