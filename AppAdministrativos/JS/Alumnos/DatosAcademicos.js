$(function () {
    var tblAlumnos;

    var DatosFn = {
        init() {
            $('#tblAlumnos').on('click', 'a', this.tblAlumnosClickA);
            $('#btnGuardarDatosEdu').on('click', this.btnCambioCarreraClick);
            $('#txtColegBeca').on('input', this.txtChange);
            $('#txtInscrBeca').on('input', this.txtChange);
            $('#txtExamenBeca').on('input', this.txtChange);
            $('#txtCredencialBeca').on('input', this.txtChange);
            $('#chkEmpresa').on('change', this.chkChange);
            $('#slcOfertaEducativa').on('change', this.GetCosto);
            $('#slcTurno').on('change', this.chkEmpresaChange);
            $("#slcPeriodo").on('change', this.GetCosto);

            GlobalFn.init();
            GlobalFn.GetTurno();
            GlobalFn.GetPlantel();
            GlobalFn.GetPeriodo_P_C_F("slcPeriodo");
            this.TraerAlumnos();
        },
        chkEmpresaChange() {
            if (parseInt($('#slcTurno').val()) === 3)
                $('#chkEmpresa').prop('checked', true).change();
        },
        GetCosto() {
            if ($('#slcOfertaEducativa').val() != null) {
                var url = $('#slcPeriodo :selected').data("anio") + "/"
                    + $('#slcPeriodo :selected').data("periodoid") + "/"
                    + $('#slcOfertaEducativa').val();

                IndexFn.Api('OfertaEducativa/Costos/' + url, "GET", "")
                    .done(function (Respuesta) {
                        DatosFn.CalcularCosto(Respuesta);
                    })
                    .fail(function (data) {
                        console.log(data);
                    });
            }
        },
        CalcularCosto(lista) {
            var Colegiatura = lista.find(function (cuota) {
                return cuota.PagoConceptoId === 800;
            });
            Colegiatura.Porcentaje = (Colegiatura.Porcentaje != undefined ? Colegiatura.Porcentaje : $('#txtColegBeca').val());

            var Inscripcion = lista.find(function (cuota) {
                return cuota.PagoConceptoId === 802;
            });
            Inscripcion.Porcentaje = (Inscripcion.Porcentaje != undefined ? Inscripcion.Porcentaje : $('#txtInscrBeca').val());

            var Examen = lista.find(function (cuota) {
                return cuota.PagoConceptoId === 1;
            });
            Examen.Porcentaje = (Examen.Porcentaje != undefined ? Examen.Porcentaje : $('#txtExamenBeca').val());

            var Credencial = lista.find(function (cuota) {
                return cuota.PagoConceptoId === 1000;
            });
            Credencial.Porcentaje = (Credencial.Porcentaje != undefined ? Credencial.Porcentaje : $('#txtCredencialBeca').val());


            $('#lblColegiatura').text((Colegiatura.Monto).toLocaleString('es-mx', {
                style: 'currency',
                currency: 'MXN',
                minimumFractionDigits: 2
            }));

            $('#lblInscripcion').text((Inscripcion.Monto).toLocaleString('es-mx', {
                style: 'currency',
                currency: 'MXN',
                minimumFractionDigits: 2
            }));

            $('#lblExamen').text((Examen.Monto).toLocaleString('es-mx', {
                style: 'currency',
                currency: 'MXN',
                minimumFractionDigits: 2
            }));

            $('#lblCredencial').text((Credencial.Monto).toLocaleString('es-mx', {
                style: 'currency',
                currency: 'MXN',
                minimumFractionDigits: 2
            }));

            $('#txtColegBeca').data('cuota', Colegiatura.Monto);
            $('#txtColegBeca').data('txtid', 'txtColegPPagar');
            $('#txtColegBeca').val(Colegiatura.Porcentaje).change();

            $('#txtInscrBeca').data('cuota', Inscripcion.Monto);
            $('#txtInscrBeca').data('txtid', 'txtInscrPPagar');
            $('#txtInscrBeca').val(Inscripcion.Porcentaje).change();

            $('#txtExamenBeca').data('cuota', Examen.Monto);
            $('#txtExamenBeca').data('txtid', 'txtExamPPagar');
            $('#txtExamenBeca').val(Examen.Porcentaje).change();

            $('#txtCredencialBeca').data('cuota', Credencial.Monto);
            $('#txtCredencialBeca').data('txtid', 'txtCredencialPPagar');
            $('#txtCredencialBeca').val(Credencial.Porcentaje).change();

            $('#txtColegPPagar').val(Math.round((Colegiatura.Monto - ((Colegiatura.Porcentaje * Colegiatura.Monto) / 100))));
            $('#txtInscrPPagar').val(Math.round((Inscripcion.Monto - ((Inscripcion.Porcentaje * Inscripcion.Monto) / 100))));
            $('#txtExamPPagar').val(Math.round((Examen.Monto - ((Examen.Porcentaje * Examen.Monto) / 100))));
            $('#txtCredencialPPagar').val(Math.round((Credencial.Monto - ((Credencial.Porcentaje * Credencial.Monto) / 100))));

        },
        TraerAlumnos() {
            IndexFn.Block(true);
            IndexFn.Api('Alumno/ConsultarAlumnosNuevosRP/', "GET", "")
                .done(function (Respuesta) {
                    tblAlumnos = $('#tblAlumnos').dataTable({
                        "aaData": Respuesta,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId" },
                            { "mDataProp": "Nombre" },
                            { "mDataProp": "FechaRegistro" },
                            { "mDataProp": "Descripcion" },
                            { "mDataProp": "Usuario" },
                            {
                                "mDataProp": function (data) {
                                    return "<a href=''onclick='return false;' class='btn btn-success'> Editar </a> ";
                                }
                            },
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
                        "order": [[0, "desc"]]
                    });
                    var fil = $('#tblAlumnos_filter label input');
                    fil.removeClass('input-small').addClass('input-large');
                    IndexFn.Block(false);
                })
                .fail(function (Respuesta) {
                    alertify.alert('Error al cargar datos');
                });
        },
        tblAlumnosClickA() {
            IndexFn.Block(true);
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));

            DatosFn.TraerDatosAcademicos((rowadd.AlumnoId + "/" + rowadd.OfertaEducativaId));
            
        },
        TraerDatosAcademicos(Para) {
            IndexFn.Api("Alumno/Academicos/" + Para, "GET", "")
                .done(function (data) {
                    DatosFn.ChangeCombos(data);
                    DatosFn.objGuardar = {};
                    DatosFn.objGuardar.AlumnoId = data.AlumnoId;
                    DatosFn.objGuardar.OfertaEducativaId = data.OfertaEducativaId;
                    DatosFn.objGuardar.Anio = data.Anio;
                    DatosFn.objGuardar.PeriodoId = data.PeriodoId;

                    DatosFn.CalcularCosto(data.Cuotas);

                    $("#slcPeriodo").val((data.PeriodoId + " " + data.Anio)).change();
                    $("#slcTurno").val(data.TurnoId).change();
                    $('#chkEmpresa').prop('checked', data.EsEmpresa).change();

                    $($("[aria-controls='Academicos']")[0]).click();
                    $('#ModalEducativa').modal('show');
                    IndexFn.Block(false);

                    GlobalFn.DatosOferta.PlantelId = -1;
                    GlobalFn.DatosOferta.OfertaEducativaTipoId = -1;
                    GlobalFn.DatosOferta.OFertaEducativa = -1;
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Error al consultar la información");
                });
        },
        objGuardar: { AlumnoId: -1 },
        ChangeCombos(data) {
            GlobalFn.DatosOferta.PlantelId = -1;
            GlobalFn.DatosOferta.OfertaEducativaTipoId = -1;
            GlobalFn.DatosOferta.OFertaEducativa = -1;

            GlobalFn.DatosOferta.PlantelId = data.SucursalId;
            GlobalFn.DatosOferta.OfertaEducativaTipoId = data.OfertaEducativaTipoId;
            GlobalFn.DatosOferta.OFertaEducativa = data.OfertaEducativaId;

            if (parseInt($("#slcPlantel").val()) !== data.SucursalId) {
                $("#slcPlantel").val(data.SucursalId);
                $("#slcPlantel").change();
            } else if (parseInt($("#slcTipoOferta").val()) !== data.OfertaEducativaTipoId) {
                $("#slcTipoOferta").val(data.OfertaEducativaTipoId);
                $("#slcTipoOferta").change();
            } else if (parseInt($("#slcOfertaEducativa").val()) !== data.OfertaEducativaId) {
                $("#slcOfertaEducativa").val(data.OfertaEducativaId);
                $("#slcOfertaEducativa").change();
            }

        },
        btnCambioCarreraClick() {
            IndexFn.Block(true);
            var objGuardar = {
                AlumnoId: DatosFn.objGuardar.AlumnoId,
                Anio: $('#slcPeriodo :selected').data("anio"),
                PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                OfertaEducativaIdActual: DatosFn.objGuardar.OfertaEducativaId,
                OfertaEducativaIdNueva: $('#slcOfertaEducativa').val(),
                TurnoId: $('#slcTurno').val(),
                EsEmpresa: $('#chkEmpresa').prop('checked'),
                UsuarioId:  localStorage.getItem('userAdmin'),
                MontoColegiatura: $('#txtColegPPagar').val(),
                MontoInscripcion: $('#txtInscrPPagar').val(),
                MontoCredencial: $('#txtCredencialPPagar').val(),
                MontoExamen: $('#txtExamPPagar').val(),
                AnioAnterior: DatosFn.objGuardar.Anio,
                PeriodoIdAnterior: DatosFn.objGuardar.PeriodoId
            };

            IndexFn.Api("Alumno/ChageOffer", "POST", JSON.stringify(objGuardar))
                .done(function (data) {                    
                    IndexFn.Block(false);
                    if (data.Status) {
                        $('#ModalEducativa').modal('hide');
                        alertify.alert("Se guardo correctamente", function () {
                            DatosFn.TraerAlumnos();
                        });
                    } else {
                        alertify.alert("Error al consultar la información");
                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Error al consultar la información");
                });
        },
        txtChange(e) {
            $(this).val(($(this).val() < 0 ? 0 : $(this).val() > 100 ? 100 : $(this).val()));

            var desc = $(this).val();
            desc = $(this).data('cuota') - ($(this).data('cuota') * (desc / 100));

            $('#' + $(this).data('txtid')).val(Math.round(desc));
        },
        chkChange() {

            if (this.checked) {
                $($('#txtColegBeca')[0].parentNode).hide();
                $($('#txtInscrBeca')[0].parentNode).hide();
                $($('#txtCredencialBeca')[0].parentNode).hide();
                $($($($('#txtExamenBeca')[0].parentNode)[0].parentNode).hide()[0].previousElementSibling).hide()

                $('#txtColegPPagar').removeAttr('readonly');
                $('#txtInscrPPagar').removeAttr('readonly');
                $('#txtCredencialPPagar').removeAttr('readonly');
                $('#txtExamenBeca').val(0).change();

            } else {
                if (parseInt($('#slcTurno').val()) !== 3) {
                    $($('#txtColegBeca')[0].parentNode).show();
                    $($('#txtInscrBeca')[0].parentNode).show();
                    $($('#txtCredencialBeca')[0].parentNode).show();
                    $($($($('#txtExamenBeca')[0].parentNode)[0].parentNode).show()[0].previousElementSibling).show()

                    $('#txtColegPPagar').attr('readonly', true);
                    $('#txtInscrPPagar').attr('readonly', true);
                    $('#txtCredencialPPagar').attr('readonly', true);
                } else {
                    this.checked = true;
                }
            }
        }
    };

    DatosFn.init();

});