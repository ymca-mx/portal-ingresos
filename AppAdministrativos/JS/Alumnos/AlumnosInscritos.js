$(function () {
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

    var InscritosFn = {
        init() {
            $("#tbAlumnos").on("click", "a", this.tbAlumnosClikA);
            $('#btnCancelar').on('click', this.btnCancelar);
            $('#btnGuardar').on('click', this.btnGuardar);
            $('#btnGuardarAntecedente').on('click', this.btnGuardarAntecedente);

            $('#slcTipoOferta').on('change', this.CambiarNombre);
            $('#slcPeriodo').on('change', this.slcPeriodoChange);
            $('#slcOfertaEducativa').on('change', this.slcPeriodoChange);

            this.Cargar();
            GlobalFn.init();
            GlobalFn.GetPlantel();
            GlobalFn.GetTurno();
            GlobalFn.GetPeriodo_N_I();

            $('#txtDescuentoBec').data('labelp', 'txtPagarCol');
            $('#txtDescuentoBec').data('label', 'txtcuotaCol');
            $('#txtDescuentoBec').on('keyup', this.GetMontos);
            $('#txtDescuentoBec').knob({
                'change': function (val) {
                    InscritosFn.GetMontosKnob(val, 'txtDescuentoBec');
                }
            });

            $('#txtDescuentoIns').data('labelp', 'txtPagarIn');
            $('#txtDescuentoIns').data('label', 'txtcuotaIn');
            $('#txtDescuentoIns').on('keyup', this.GetMontos);
            $('#txtDescuentoIns').knob({
                'change': function (val) {
                    InscritosFn.GetMontosKnob(val, 'txtDescuentoIns');
                }
            });

            $('#txtDescuentoExa').data('labelp', 'txtPagarExa');
            $('#txtDescuentoExa').data('label', 'txtcuotaExa');
            $('#txtDescuentoExa').on('keyup', this.GetMontos);
            $('#txtDescuentoExa').knob({
                'change': function (val) {
                    InscritosFn.GetMontosKnob(val, 'txtDescuentoExa');
                }
            });

            $('#txtDescuentoCred').data('labelp', 'txtPagarCred');
            $('#txtDescuentoCred').data('label', 'txtcuotaCred');
            $('#txtDescuentoCred').on('keyup', this.GetMontos);
            $('#txtDescuentoCred').knob({
                'change': function (val) {
                    InscritosFn.GetMontosKnob(val, 'txtDescuentoCred');
                }
            });

            GlobalFn.GetMedios();
            GlobalFn.GetAreas("slcArea");

            this.Beca();
            this.Inscripcion();

            $('#chkEsEmpresa').on('click', this.chkEmpresa);
            $('#slcNacionalidadPrep').on('change', this.PaisPrepa);
            $('#chkUniSi').on('click', this.chkUniSi);
            $('#chkUniNo').on('click', this.chkUniNo);
            $('#chkUni').on('click', this.chkUni);
            $('#slcEstadoPais').on('change', this.slcEstadoPaisChange);

        },
        Inscripcion() {
            $('#DescuentoI').noUiSlider({
                start: 50,
                step: 5,
                range: {
                    min: 0,
                    max: 100
                }
            });

            $('#DescuentoI').noUiSlider_pips({
                mode: 'values',
                values: [20, 80],
                density: 5
            });

            $('#DescuentoI').on('set', function (event, value) {
                if (value < 20) {
                    $(this).val(20);
                } else if (value > 80) {
                    $(this).val(80);
                }
                $('#valIn').text($(this).val() + '%');
            });

        },
        Beca() {
            $('#DescuentoB').noUiSlider({
                start: 50,
                step: 5,
                range: {
                    min: 0,
                    max: 100
                }
            });

            $('#DescuentoB').noUiSlider_pips({
                mode: 'values',
                values: [20, 80],
                density: 5
            });

            $('#DescuentoB').on('set', function (event, value) {
                if (value < 20) {
                    $(this).val(20);
                } else if (value > 80) {
                    $(this).val(80);
                }
                $('#valBe').text($(this).val() + '%');

            });
            jQuery('#pulsate-regular').pulsate({
                color: "#bf1c56"
            });
            jQuery("#dvCargosEf").pulsate({
                color: "#bf1c56"
            });
        },
        CambiarNombre() {
            var tipo = parseInt($('#slcTipoOferta').val());
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
                    if (tipo === 1) {
                        $('#lblLugarP').text("Lugar donde estudio la preparatoria");
                        $('#hOfertaTitulo').text('Preparatoria de Procedencia');
                        $('#lblOfertaTitulo2').text('Preparatoria de Procedencia');
                        $('#txtNombrePrepa').val("");
                    } else if (tipo === 2 || tipo === 3) {
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
            }
        },
        slcPeriodoChange() {

            if (($('#slcPeriodo').val() == "" || $('#slcOfertaEducativa').val() == "" ||
                parseInt($('#slcTipoOferta').val()) ===4)
                || ($('#slcPeriodo').val() == undefined || $('#slcOfertaEducativa').val() == undefined)) { return false; }
            var param = $('#slcOfertaEducativa').val() + "/" +
                $('#slcPeriodo :selected').data("anio") + "/" +
                $('#slcPeriodo :selected').data("periodoid");

            IndexFn.Api("Cuota/TraerCuotaPeriodo/" + param, "GET", "")
                .done(function (data) {
                    if (data.length > 0) {
                        InscritosFn.SetCuotas(data);
                    } else {
                        alertify.alert("Error: El periodo seleccionado aun no tiene cuotas generadas, favor de comunicarse al área de Sistemas");
                    }
                })
                .fail(function (data) {
                    alertify.alert("Error: El periodo seleccionado aun no tiene cuotas generadas, favor de comunicarse al área de Sistemas");
                    $('#slcPeriodo').val($("#slcPeriodo option:first").val());
                });
        },
        SetCuotas(data) {
            var Sispago = $('#slcSistemaPago option:selected').html();
            if (Sispago === undefined) {
                setTimeout(function () { InscritosFn.SetCuotas(data); }, 200);
            } else {
                var monto;
                var MaxDes;

                var Colegiatura = data.find(function (cuota) {
                    return cuota.PagoConceptoId === 800;
                });
                var Inscripcion = data.find(function (cuota) {
                    return cuota.PagoConceptoId === 802;
                });
                var Examen = data.find(function (cuota) {
                    return cuota.PagoConceptoId === 1;
                });
                var Credencial = data.find(function (cuota) {
                    return cuota.PagoConceptoId === 1000;
                });

                if (Sispago.search("4") != -1 || Sispago.search("6") != -1) {
                    MaxDes = Colegiatura.Descuento.MontoMaximo;
                    $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                    //$('#txtDescuentoBec').val(MaxDes);
                    $('#txtcuotaCol').text('$' + Colegiatura.Monto);
                    monto = Colegiatura.Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                    monto = Colegiatura.Monto - monto;
                    $('#txtPagarCol').text('$' + String(Math.round(monto)));
                    //$('#txtDescuentoBec').change();
                } else {
                    MaxDes = Colegiatura.Descuento.MontoMaximo;
                    $('#txtDescuentoBec').attr("data-val-max", MaxDes);
                    //$('#txtDescuentoBec').val(MaxDes);
                    $('#txtcuotaCol').text('$' + (Colegiatura.Monto * 4));
                    monto = (Colegiatura.Monto * 4) * (parseFloat($('#txtDescuentoBec').val()) / 100);
                    monto = (Colegiatura.Monto * 4) - monto;
                    $('#txtPagarCol').text('$' + String(Math.round(monto)));
                    //$('#txtDescuentoBec').change();
                }

                MaxDes = Inscripcion.Descuento.MontoMaximo;
                $('#txtDescuentoIns').attr("data-val-max", MaxDes);
                //$('#txtDescuentoIns').val(MaxDes);
                $('#txtcuotaIn').text('$' + Inscripcion.Monto);
                monto = (Inscripcion.Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                monto = Inscripcion.Monto - monto;
                $('#txtPagarIn').text('$' + String(Math.round(monto)));
                //$('#txtDescuentoIns').change();

                MaxDes = Examen.Descuento.MontoMaximo;
                $('#txtDescuentoExa').attr("data-val-max", MaxDes);
                //$('#txtDescuentoExa').val(MaxDes);
                $('#txtcuotaExa').text('$' + Examen.Monto);
                monto = (Examen.Monto * (parseFloat($('#txtDescuentoExa').val()) / 100));
                monto = Examen.Monto - monto;
                $('#txtPagarExa').text('$' + String(Math.round(monto)));
                //$('#txtDescuentoExa').change();

                MaxDes = Credencial.Descuento.MontoMaximo;
                $('#txtDescuentoCred').attr("data-val-max", MaxDes);
                //$('#txtDescuentoCred').val(MaxDes);
                $('#txtcuotaCred').text('$' + Credencial.Monto);
                monto = (Credencial.Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                monto = Credencial.Monto - monto;
                $('#txtPagarCred').text('$' + String(Math.round(monto)));
                //$('#txtDescuentoCred').change();
            }
        },
        Limpiar() {
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
        },
        GuardarOFerta(Campos) {

            IndexFn.Api("Alumno/NuevaOferta", "put", JSON.stringify(Campos))
                .done(function (data) {
                    if (Campos.AlumnoInscrito.EsEmpresa || parseInt($('#slcTipoOferta').val()) === 4) {
                        alertify.alert("Alumno Guardado", function () {
                            IndexFn.Block(false);
                            InscritosFn.Email(data.AlumnoId, Campos.AlumnoInscrito.OfertaEducativaId);
                        });
                    } else {
                        InscritosFn.GuardarDescuentos(data.AlumnoId);
                    }
                })
                .fail(function (data) {
                    alertify.alert("Error no se guardaron los cambios, intente de nuevo", function () {
                        IndexFn.Block(false);
                    });
                });
            
        },
        GuardarDescuentos(AlumnoId) {
            if (jQuery.type( localStorage.getItem('userAdmin')) === "undefined") {
                return false;
            }

            var ObjAlumno = {
                AlumnoId: AlumnoId,
                SistemaPagoId: $('#slcSistemaPago').val(),
                Anio: $('#slcPeriodo :selected').data("anio"),
                PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                OfertaEducativaId: $('#slcOfertaEducativa').val(),
                UsuarioId:  localStorage.getItem('userAdmin'),
                Descuentos: [
                {
                    PagoConceptoId: 800,
                    TotalPagar: Number($('#txtPagarCol').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionBec').val()
                },
                {
                    PagoConceptoId: 802,
                    TotalPagar: Number($('#txtPagarIn').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionIns').val()
                },
                {
                    PagoConceptoId: 1000,
                    TotalPagar: Number($('#txtPagarCred').text().replace(/[^0-9\.-]+/g, "")),
                    Justificacion: $('#txtJustificacionCred').val()
                }]
            };

            var data = new FormData();

            var flIns = $('#BecArchivo'); // FileList object
            flIns = flIns[0].files[0];
            data.append("DocBeca", flIns);

            flIns = $('#InsArchivo');
            flIns = flIns[0].files[0];
            data.append("DocInscipcion", flIns);

            flIns = $('#ExamenArchivo');
            flIns = flIns[0].files[0];
            data.append("DocExamen", flIns);

            data.append("ObjAlumno", JSON.stringify(ObjAlumno));
            IndexFn.clearAlert();

            IndexFn.ApiFile("Descuentos/GuardarDescuentos", data)
                .done(function (data) {
                    if (data != null) {
                        IndexFn.Block(false);
                        var Alumnos = [ObjAlumno.AlumnoId];
                        IndexFn.Api("General/EnviarMail2", "Post", JSON.stringify(Alumnos))
                            .done(function (data) {

                                alertify.alert("Alumno Guardado </br> ", function () {
                                    $('#Encabezado').show();
                                    $('#Contenedor').hide();
                                    var url = "Views/Alumno/Credenciales.aspx?AlumnoId=" + ObjAlumno.AlumnoId + "&OfertaEducativaId=" + ObjAlumno.OfertaEducativaId;
                                    window.open(url, "Credenciales");
                                    InscritosFn.Cargar();
                                });
                            })
                            .fail(function (data) {
                                console.log(data);
                                InscritosFn.Email(ObjAlumno.AlumnoId, ObjAlumno.OfertaEducativaId);
                            });

                    } else {
                        IndexFn.Block(false);
                        alertify.alert("No se guardaron los cambios, intente de nuevo");
                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("No se guardaron los cambios, intente de nuevo");
                    console.log(data);
                });
        },
        GuardarAntecedentes(objAlumno) {
            IndexFn.Api("Alumno/GuardarAntecedentes", "Put", JSON.stringify(objAlumno.Antecendentes[0]))
                .done(function (data) {
                    $('#Antecedentes').modal('hide');
                    InscritosFn.GuardarOFerta(objAlumno);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Error no se guardaron los cambios, intente de nuevo", function () {
                        $('#Antecedentes').modal('show');
                    });
                });
        },
        Email(AlumnoId, OfertaId) {
            var Alumnos = [AlumnoId];
            IndexFn.Api("General/EnviarMail2", "Post", JSON.stringify(Alumnos))
                .done(function (data) {
                    var extramail = "<p>" + "Se ha enviado un mail al Alumno en el cual podra visualizar el reglamento escolar." + "</p>";
                    alertify.alert(extramail, function () {
                        $('#Encabezado').show();
                        $('#Contenedor').hide();

                        var url = "Views/Alumno/Credenciales.aspx?AlumnoId=" + AlumnoId + "&OfertaEducativaId=" + OfertaId;
                        window.open(url, "Credenciales");

                        InscritosFn.Cargar();
                    });
                })
                .fail(function (data) {
                    console.log(data);
                    InscritosFn.Email(AlumnoId, OfertaId);
                });
        },
        Cargar() {
            IndexFn.Api("Alumno/ConsultarAlumnos", "GET", "")
                .done(function (Respuesta) {
                    MItable = $('#tbAlumnos').dataTable({
                        "aaData": Respuesta,
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
                    $('#slcTipoOferta').change();
                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    alertify.alert('Error al cargar datos');
                });
        },
        CalcularDescuento(Monto, Descuento) {
            var Redondeado;
            Monto = parseFloat(Monto);
            Descuento = (Monto * (Descuento / 100))
            Redondeado = Monto - Descuento;
            Redondeado = Math.round(Redondeado);
            return Redondeado;
        },
        GetMontos() {
            var idlabel = $('#' + this.id).data('label');
            var idlabelP = $('#' + this.id).data('labelp');

            var Maximo = $('#' + this.id).data();
            if (Maximo.valMax < $('#' + this.id).val()) { $('#' + this.id).val(Maximo.valMax); return false; }
            var monto = InscritosFn.CalcularDescuento($('#' + idlabel).text().replace('$', ''), $('#' + this.id).val());
            $('#' + idlabelP).text('$' + String(monto));
        },
        GetMontosKnob(val, idl) {
            var idlabel = $('#' + idl).data('label');
            var idlabelP = $('#' + idl).data('labelp');

            var Maximo = $('#' + idl).data();
            if (Maximo.valMax < val) { $('#' + idl).val(Maximo.valMax); return false; }
            var monto = InscritosFn.CalcularDescuento($('#' + idlabel).text().replace('$', ''), val);
            $('#' + idlabelP).text('$' + String(monto));
        },
        tbAlumnosClikA() {
            IndexFn.Block(true);
            Mas = undefined;
            fid = MItable.fnGetData(this.parentNode.parentNode, 0);
            var Fecha;

            IndexFn.Api("Alumno/ConsultarAlumno/" + fid, "Get", "")
                .done(function (data) {
                    EsLenguas = false;                    
                    if (data.lstAlumnoInscrito.length == 0) {
                        if (data.lstAlumnoInscrito[0].OfertaEducativa.OfertaEducativaTipoId === 4) {
                            EsLenguas = true;
                        }
                    }
                    $(data.lstAlumnoInscrito).each(function () {
                        if (this.OfertaEducativa.OfertaEducativaTipoId != 4) { Mas = 1; }
                    });

                    if (Mas != 'undefined') {
                        $('#divExamen').hide();
                        EsLenguas = true;
                    }
                    $('#txtAlumnoId').val(fid);
                    Fecha = new Date(parseInt(data.DTOAlumnoDetalle.FechaNacimiento.slice(6)));
                    Fecha = new Date(Fecha);
                    $('#txtNombre').val(data.Nombre);
                    $('#txtPaterno').val(data.Paterno);
                    $('#txtMaterno').val(data.Materno);
                    $('#txtFechaNacimiento').val(Fecha.getDate() + '/' + parseInt(Fecha.getMonth() + 1) + '/' + Fecha.getFullYear());
                    $('#txtOferta').val(data.AlumnoInscrito.OfertaEducativa.Descripcion);
                    $('#txtFechaRegistro').val(data.FechaRegistro);
                    $('#Encabezado').hide();
                    $('#Contenedor').show();
                    $('#DivDescuentos').show();

                    $("#slcSistemaPago").change();

                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Fallo la carga de los datos del alumno.");
                    console.log(data);
                });
        },
        btnCancelar() {
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
        },
        btnGuardar() {
            if ($('#slcOfertaEducativa').val() === '-1') {
                alertify.alert("Seleccione un " + $('#lblOFerta').html() + " para poder continar", function () {
                    return true;
                });
                return false;
            }
            if (EsLenguas) {
                $('#Antecedentes').modal('show');
            }
            else {                
                if (!form.valid()) { return false; }

                Usuario =  localStorage.getItem('userAdmin');
                if (jQuery.type(Usuario) === "undefined") {
                    return false;
                }
                var objAlumno = {
                    AlumnoId: fid,
                    UsuarioId: Usuario,
                    Anio: $('#slcPeriodo :selected').data("anio"),
                    PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                    AlumnoInscrito: {
                        OfertaEducativaId: $('#slcOfertaEducativa').val(),
                        EsEmpresa: $('#chkEsEmpresa')[0].checked,
                        PagoPlanId: $('#slcSistemaPago').val(),
                        TurnoId: $('#slcTurno').val(),
                        Material: $('#chkMaterial')[0].checked
                    },
                };
                IndexFn.Block(true);
                InscritosFn.GuardarOFerta(objAlumno);
            }
            //$('#btnGuardarAntecedente').click();
        },
        btnGuardarAntecedente() {
            if ($('#slcOfertaEducativa').val() == '-1') { alertify.alert("Seleccione un " + $('#lblOFerta').html() + " para poder continar"); return false; }
            if (!form.valid()) { return false; }
            Usuario =  localStorage.getItem('userAdmin');
            if (jQuery.type(Usuario) === "undefined") {
                return false;
            }
            var Resultado;

            var objAlumno = {
                AlumnoId: fid,
                UsuarioId: Usuario,
                Anio: $('#slcPeriodo :selected').data("anio"),
                PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                AlumnoInscrito: {
                    OfertaEducativaId: $('#slcOfertaEducativa').val(),
                    EsEmpresa: $('#chkEsEmpresa')[0].checked,
                    PagoPlanId: $('#slcSistemaPago').val(),
                    TurnoId: $('#slcTurno').val(),
                    Material: $('#chkMaterial')[0].checked
                },
                Antecendentes: [{
                    AlumnoId: fid,
                    UsuarioId: Usuario,
                    AntecedenteTipoId: $('#slcTipoOferta').val(),
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
                }]
            };
            
            $('#Antecedentes').modal('hide');
            alertify.confirm("¿Esta seguro que desea guardar los cambios?",
                function (e) {
                    IndexFn.Block(true);
                    InscritosFn.GuardarAntecedentes(objAlumno);
                },
                function () {
                    $('#Antecedentes').modal('show');
                });

        },
        chkEmpresa() {
            if (this.checked) {
                $('#DivDescuentos').hide();
            } else { $('#DivDescuentos').show(); }
        },
        PaisPrepa() {
            if (parseInt($('#slcNacionalidadPrep').val()) === 1) {
                GlobalFn.GetEstado('slcEstadoPais', 9);
            } else {
                GlobalFn.GetPais('slcEstadoPais', 1);
            }
        },
        chkUniSi() {
            if (this.checked) {
                $('#txtUniMotivo').prop('disabled', false);
            }
        },
        chkUniNo() {
            if (this.checked) {
                $('#txtUniMotivo').prop('disabled', true);
            }
        },
        chkUni() {
            if (this.checked) { $('#txtUni').prop('disabled', false); }
            else { $('#txtUni').prop('disabled', true); }
        },
        slcEstadoPaisChange() {
            if (parseInt($('#slcNacionalidadPrep').val()) == 1) {
                if (parseInt($('#slcEstado').val()) == 146) {
                    $('#slcNacionalidadPrep').val(2);
                }
            }
        }
    };


    InscritosFn.init();
    
});
 