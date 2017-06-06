$(function init() {
    var tblReferencias;
    var Alumnoid = 0;

    $('#btnBuscar').on('click', function () {
        Alumnoid = 0;
        var lbl = $('#lblNombre');
        lbl[0].innerHTML = "";
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0) { return false; }        
        $('#Load').modal('show');
        BuscarAlumno(AlumnoId);
    });

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });

    $('#sclMesinicial').on('change', function () {
        var mes = $(this).val();
        Combo2(mes === '-1' ? Combo2(mes) : mes < 8 ? (parseInt(mes) + 5) : (parseInt(mes) - 7));
    });

    function Combo2(Mes) {
        $('#sclMesFinal').val(Mes);
    }

    function BuscarAlumno(idAlumno) {
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
                    CargarOferta(data.d.lstAlumnoInscrito);
                } else { $('#Load').modal('hide');}
            }
        });
    }
    function CargarOferta(OFertas) {
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
    }
    $("#slcOfertaEducativa").on('change', function () {
        $('#Load').modal('show');
        var oferta = $("#slcOfertaEducativa").val();
        TraerReferencias(oferta);
    });

    $('#ModalMontos').on('click', function () {
        if (Alumnoid !== 0 && $("#slcOfertaEducativa").val() !== -1 && $('#sclMesinicial').val() !== "-1") {
            $('#Montos').modal('show');
        } else {
            alertify.alert("Favor de insertar todos los datos.")
        }
    });

    $('#btnGuardar').on('click', function () {
        $('#Load').modal('show');
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
    
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/GenerarSemestre",
            data: json,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {                
                if (data.d) {
                    $('#Montos').modal('hide'); 
                    alertify.alert("Se generaron las referencias");
                    TraerReferencias(OFerta);
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
    });

    function TraerReferencias(OFerta) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/ReferenciasSemestrales",
            data: JSON.stringify({ AlumnoId: Alumnoid, OfertaEducativaId: OFerta }),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                tblReferencias = $('#tblReferencias').dataTable({
                    "aaData": data.d,
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

                $('#Load').modal('hide');
            }
        });
    }
});