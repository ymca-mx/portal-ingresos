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

            GlobalFn.init();
            GlobalFn.GetTurno();
            GlobalFn.GetPlantel();
            GlobalFn.GetPeriodo_P_C_F("slcPeriodo");
            this.TraerAlumnos();
        },
        TraerAlumnos() {
            $('#Load').modal('show');
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
                    $('#Load').modal('hide');
                })
                .fail(function (Respuesta) {
                    alertify.alert('Error al cargar datos');
                });
        },
        tblAlumnosClickA() {
            $('#Load').modal('show');
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

                    var Colegiatura = data.Cuotas.find(function (cuota) {
                        return cuota.PagoConceptoId === 800;
                    });

                    var Inscripcion = data.Cuotas.find(function (cuota) {
                        return cuota.PagoConceptoId === 802;
                    });

                    var Examen = data.Cuotas.find(function (cuota) {
                        return cuota.PagoConceptoId === 1;
                    });

                    var Credencial = data.Cuotas.find(function (cuota) {
                        return cuota.PagoConceptoId === 1000;
                    });

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

                    $('#txtColegBeca').val(Colegiatura.Porcentaje);
                    $('#txtColegBeca').data('cuota', Colegiatura.Monto);
                    $('#txtColegBeca').data('txtid', 'txtColegPPagar');

                    $('#txtInscrBeca').val(Inscripcion.Porcentaje);
                    $('#txtInscrBeca').data('cuota', Inscripcion.Monto);
                    $('#txtInscrBeca').data('txtid', 'txtInscrPPagar');

                    $('#txtExamenBeca').val(Examen.Porcentaje);
                    $('#txtExamenBeca').data('cuota', Examen.Monto);
                    $('#txtExamenBeca').data('txtid', 'txtExamPPagar');

                    $('#txtCredencialBeca').val(Credencial.Porcentaje);
                    $('#txtCredencialBeca').data('cuota', Credencial.Monto);
                    $('#txtCredencialBeca').data('txtid', 'txtCredencialPPagar');

                    $('#txtColegPPagar').val(Math.round((Colegiatura.Monto - ((Colegiatura.Porcentaje * Colegiatura.Monto) / 100))));
                    $('#txtInscrPPagar').val(Math.round((Inscripcion.Monto - ((Inscripcion.Porcentaje * Inscripcion.Monto) / 100))));
                    $('#txtExamPPagar').val(Math.round((Examen.Monto - ((Examen.Porcentaje * Examen.Monto) / 100))));
                    $('#txtCredencialPPagar').val(Math.round((Credencial.Monto - ((Credencial.Porcentaje * Credencial.Monto) / 100))));

                    $("#slcPeriodo").val((data.PeriodoId + " " + data.Anio)).change();
                    $("#slcTurno").val(data.TurnoId).change();
                    $('#chkEmpresa').prop('checked', data.EsEmpresa);
                    $('#chkEmpresa').change();

                    $('#ModalEducativa').modal('show');
                    $('#Load').modal('hide');
                })
                .fail(function (data) {
                    $('#Load').modal('hide');
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
            } else if (parseInt($("#slcOFertaEducativa").val()) !== data.OfertaEducativaId) {
                $("#slcOFertaEducativa").val(data.OfertaEducativaId);
                $("#slcOFertaEducativa").change();
            }
        },
        btnCambioCarreraClick() {
            $('#Load').modal('show');
            var objGuardar = {
                AlumnoId: DatosFn.objGuardar.AlumnoId,
                Anio: $('#slcPeriodo :selected').data("anio"),
                PeriodoId: $('#slcPeriodo :selected').data("periodoid"),
                OfertaEducativaIdActual: DatosFn.objGuardar.OfertaEducativaId,
                OfertaEducativaIdNueva: $('#slcOFertaEducativa').val(),
                TurnoId: $('#slcTurno').val(),
                EsEmpresa: $('#chkEmpresa').prop('checked'),
                UsuarioId: $.cookie('userAdmin'),
                MontoColegiatura: $('#txtColegPPagar').val(),
                MontoInscripcion: $('#txtInscrPPagar').val(),
                MontoCredencial: $('#txtCredencialPPagar').val(),
                MontoExamen: $('#txtExamPPagar').val(),
                AnioAnterior: DatosFn.objGuardar.Anio,
                PeriodoIdAnterior: DatosFn.objGuardar.PeriodoId
            };

            IndexFn.Api("Alumno/ChageOffer", "POST", JSON.stringify(objGuardar))
                .done(function (data) {
                    $('#ModalEducativa').modal('hide');
                    $('#Load').modal('hide');
                    alertify.alert("Se guardo correctamente", function () {
                        DatosFn.TraerAlumnos();
                    });
                })
                .fail(function (data) {
                    $('#Load').modal('hide');
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
                $('#txtExamPPagar').val(0);

            } else {
                $($('#txtColegBeca')[0].parentNode).show();
                $($('#txtInscrBeca')[0].parentNode).show();
                $($('#txtCredencialBeca')[0].parentNode).show();
                $($($($('#txtExamenBeca')[0].parentNode)[0].parentNode).show()[0].previousElementSibling).show()

                $('#txtColegPPagar').attr('readonly', true);
                $('#txtInscrPPagar').attr('readonly', true);
                $('#txtCredencialPPagar').attr('readonly', true);
            }
        }
    };

    DatosFn.init();

});