$(function init() {
    var tblReferencias;
    var Alumnoid = 0;
    var GenerarSemestrales = {
        Funciones: {
            Combo2: function (Mes) {
                $('#sclMesFinal').val(Mes);
            },
            BuscarAlumno: function (idAlumno) {
                $('#sclMesinicial').val('-1');
                $('#sclMesFinal').val('-1');
                $.ajax({
                    type: "POST",
                    url: "WS/Alumno.asmx/ConsultarAlumno",
                    data: "{AlumnoId:'" + idAlumno + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d !== null) {
                            var lbl = $('#lblNombre');
                            lbl[0].innerHTML = data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno;
                            lbl[0].innerHTML += data.d.AlumnoInscrito.EsEmpresa == true ? (data.d.AlumnoInscrito.EsEspecial == true ? " - Alumno Especial  " : " - Grupo  Empresarial") + " - " + data.d.Grupo.Descripcion : "";
                            Alumnoid = idAlumno;

                            $('#txtMontoInscripcion').val(data.d.Grupo.ConfiguracionAlumno.CuotaInscripcion === -1 ? "" : data.d.Grupo.ConfiguracionAlumno.CuotaInscripcion);
                            $('#txtMontoColegiatura').val(data.d.Grupo.ConfiguracionAlumno.CuotaColegiatura === -1 ? "" : data.d.Grupo.ConfiguracionAlumno.CuotaColegiatura);

                            GenerarSemestrales.Funciones.CargarOferta(data.d.lstAlumnoInscrito);
                        } else { $('#Load').modal('hide'); }
                    }
                });
            },
            CargarOferta: function (OFertas) {
                $("#slcOfertaEducativa").empty();
                var option1 = $(document.createElement('option'));
                var ofer = -1;
                option1.text("--Seleccionar--");
                option1.val(-1);
                $("#slcOfertaEducativa").append(option1);
                $(OFertas).each(function () {
                    if (this.OfertaEducativa.OfertaEducativaTipoId !== 4) {
                        ofer = ofer === -1 ? this.OfertaEducativaId : ofer;
                        var option = $(document.createElement('option'));
                        option.text(this.OfertaEducativa.Descripcion);
                        option.val(this.OfertaEducativaId);
                        option.attr("data-Anio", this.Anio);
                        option.attr("data-PeriodoId", this.PeriodoId);
                        $("#slcOfertaEducativa").append(option);
                    }
                });
                $("#slcOfertaEducativa").val(ofer);
                $("#slcOfertaEducativa").change();

                $('#Load').modal('hide');
            },
            TraerReferencias: function (OFerta) {
                $($('#ModalMontos')[0].parentNode).show();
                $.ajax({
                    type: "GET",
                    url: "api/pago/ReferenciasSemestrales/" + Alumnoid + "/" + OFerta,
                    data: "",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        $('#Load').modal('hide');
                        if (data.Code != undefined) {
                            $($('#ModalMontos')[0].parentNode).hide();
                            alertify.alert("Universidad YMCA", data.Message);
                            return false;
                        }
                        else {
                            tblReferencias = $('#tblReferencias').dataTable({
                                "aaData": data,
                                "bSort": false,
                                "aoColumns": [
                                    { "mDataProp": "DTOCuota.DTOPagoConcepto.Descripcion" },
                                    { "mDataProp": "Referencia" },
                                    { "mDataProp": "objNormal.Monto" },
                                    { "mDataProp": "objNormal.Restante", }
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
                                    "search": "Buscar Referencia "
                                },
                                "createdRow": function (row, data, dataIndex) {
                                    try {
                                        row.childNodes[0].style.textAlign = 'left';
                                        row.childNodes[1].style.textAlign = 'center';
                                        row.childNodes[2].style.textAlign = 'right';
                                        row.childNodes[3].style.textAlign = 'center';
                                    } catch (err) {
                                        console.log(err.message);
                                    }
                                }
                            });
                        }                        
                    }
                });
            }
        },
        Eventos: {
            btnBuscarClick: function () {
                $('#txtMontoInscripcion').val("");
                $('#txtMontoColegiatura').val("");
                Alumnoid = 0;
                var lbl = $('#lblNombre');
                lbl[0].innerHTML = "";
                AlumnoId = $('#txtClave').val();
                if (AlumnoId.length == 0) { return false; }
                $('#Load').modal('show');
                GenerarSemestrales.Funciones.BuscarAlumno(AlumnoId);
            },
            txtClavekeydown: function (e) {
                if (e.which == 13) {
                    $('#btnBuscar').click();
                }
            },
            sclMesinicialchange: function () {
                var mes = $(this).val();
                GenerarSemestrales.Funciones.Combo2(mes === '-1' ? GenerarSemestrales.Funciones.Combo2(mes) : mes < 8 ? (parseInt(mes) + 5) : (parseInt(mes) - 7));
            },
            slcOfertaEducativachange: function () {
                if ($(this).val() !== "-1") {
                    $('#Load').modal('show');
                    var oferta = $("#slcOfertaEducativa").val();
                    GenerarSemestrales.Funciones.TraerReferencias(oferta);
                }
            },
            ModalMontosclick: function () {
                if (Alumnoid !== 0 && $("#slcOfertaEducativa").val() !== "-1" && $('#sclMesinicial').val() !== "-1") {
                    $('#Montos').modal('show');
                } else {
                    alertify.alert("Favor de insertar todos los datos.")
                }
            },
            btnGuardarclick: function () {
                var $frm = $('#frmCuota');
                if ($frm[0].checkValidity()) {
                    
                    var OFerta = $("#slcOfertaEducativa").val();
                    var MesIni = $('#sclMesinicial').val();
                    var MesFin = $('#sclMesFinal').val();
                    var Usuario = $.cookie('userAdmin');
                    var MInscripcion = $('#txtMontoInscripcion').val();
                    var MColegiatura = $('#txtMontoColegiatura').val();

                    var json = JSON.stringify({
                        AlumnoId: Alumnoid,
                        OfertaEducativaId: OFerta,
                        MesFinal: MesFin,
                        MesInicial: MesIni,
                        UsuarioId: Usuario,
                        Inscripcion: MInscripcion,
                        Colegiatura: MColegiatura
                    });                    

                    $('#Load').modal('show');

                    $.ajax({
                        type: "POST",
                        url: "WS/Descuentos.asmx/GenerarSemestre",
                        data: json,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data) {
                            if (data.d) {
                                $('#Montos').modal('hide');
                                alertify.alert("Se generaron las referencias", function () {                                    
                                    GenerarSemestrales.Funciones.TraerReferencias(OFerta)
                                });
                            } else {
                                $('#Load').modal('hide');
                                alertify.alert("No se pudieron generar las Referencias, intente más tarde.");
                            }
                        },
                        error: function () {
                            alertify.alert("Error, hay problemas de conexión o comunicación.")
                            $('#Load').modal('hide');
                        }
                    });
                }
                else {
                    $('#Montos').modal('hide');
                    alertify.alert('Datos invalidos, favor de verificar.', function () { $('#Montos').modal('show'); });
                }
            }
        }
    };
    $('#btnBuscar').on('click', GenerarSemestrales.Eventos.btnBuscarClick);

    $('#txtClave').on('keydown', GenerarSemestrales.Eventos.txtClavekeydown);

    $('#sclMesinicial').on('change', GenerarSemestrales.Eventos.sclMesinicialchange);    

    $("#slcOfertaEducativa").on('change', GenerarSemestrales.Eventos.slcOfertaEducativachange);

    $('#ModalMontos').on('click', GenerarSemestrales.Eventos.ModalMontosclick);

    $('#btnGuardar').on('click', GenerarSemestrales.Eventos.btnGuardarclick);

});