$(function init() {
    var tblReferencias;
    var Alumnoid = 0;
    var GenerarSemestrales = {
        Funciones: {            
            Combo2: function (Mes) {
                $('#sclMesFinal').val(Mes);                
            },
            CalculaAnioFinal: function (Mes) {
                $('#slcAnioFinal').empty();
                var AnioActual = $('#slcAnioInicial').val();
                AnioActual = parseInt(AnioActual);
                var AnioSig = Mes == -1 ? AnioActual :
                    (Mes >= 8 ? AnioActual + 1 : AnioActual);

                var option = $(document.createElement('option'));
                option.text(AnioSig);
                option.val(AnioSig);

                $('#slcAnioFinal').append(option);
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
                        } else { IndexFn.Block(false); }
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

                IndexFn.Block(false);
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
                        IndexFn.Block(false);
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
            },
            CargarAnios: function () {
                var Fecha = new Date();

                Fecha.getFullYear();

                var Anios = [
                    { Id: Fecha.getFullYear() - 1, Text: Fecha.getFullYear() - 1 },
                    { Id: Fecha.getFullYear(), Text: Fecha.getFullYear() }
                ];

                $(Anios).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Id);
                    option.val(this.Text);
                    $("#slcAnioInicial").append(option);
                });
                $("#slcAnioInicial").change();

            }
        },
        Eventos: {
            init: function () {
                $('#btnBuscar').on('click', this.btnBuscarClick);

                $('#txtClave').on('keydown', this.txtClavekeydown);

                $('#sclMesinicial').on('change', this.sclMesinicialchange);

                $("#slcOfertaEducativa").on('change', this.slcOfertaEducativachange);

                $('#ModalMontos').on('click', this.ModalMontosclick);

                $('#btnGuardar').on('click', this.btnGuardarclick);

                $('#slcAnioInicial').on('change', this.slcAnioInicialChange);

                GenerarSemestrales.Funciones.CargarAnios();
            },
            btnBuscarClick: function () {
                $('#txtMontoInscripcion').val("");
                $('#txtMontoColegiatura').val("");
                Alumnoid = 0;
                var lbl = $('#lblNombre');
                lbl[0].innerHTML = "";
                AlumnoId = $('#txtClave').val();
                if (AlumnoId.length == 0) { return false; }
                IndexFn.Block(true);
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
                GenerarSemestrales.Funciones.CalculaAnioFinal(parseInt(mes));
            },
            slcOfertaEducativachange: function () {
                if ($(this).val() !== "-1") {
                    IndexFn.Block(true);
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
                    

                    var json = JSON.stringify({
                        AlumnoId: Alumnoid,
                        OfertaEducativaId: $("#slcOfertaEducativa").val(),
                        AnioInicial: $("#slcAnioInicial").val(),
                        MesFinal: $('#sclMesFinal').val(),
                        MesInicial: $('#sclMesinicial').val(),
                        AnioFinal: $("#slcAnioFinal").val(),
                        UsuarioId:  localStorage.getItem('userAdmin'),
                        Inscripcion: $('#txtMontoInscripcion').val(),
                        Colegiatura: $('#txtMontoColegiatura').val()
                    });                    

                    IndexFn.Block(true);

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
                                    GenerarSemestrales.Funciones.TraerReferencias($("#slcOfertaEducativa").val())
                                });
                            } else {
                                IndexFn.Block(false);
                                alertify.alert("No se pudieron generar las Referencias, intente más tarde.");
                            }
                        },
                        error: function () {
                            alertify.alert("Error, hay problemas de conexión o comunicación.")
                            IndexFn.Block(false);
                        }
                    });
                }
                else {
                    $('#Montos').modal('hide');
                    alertify.alert('Datos invalidos, favor de verificar.', function () { $('#Montos').modal('show'); });
                }
            },
            slcAnioInicialChange: function () {
                $('#sclMesinicial').change();
            },
        }
    };

    GenerarSemestrales.Eventos.init();
});