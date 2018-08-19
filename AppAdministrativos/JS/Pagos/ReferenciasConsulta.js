$(document).ready(function () {
    var buscar;
    var PeriodoId;
    var Anio;
    var tblReferencias, TipoBusqueda = 1;
    var PeriodoAlcorriente = null;
    var Periodo = null;

    $("#rdAlumno").click(function () {
        $("#lbBuscar").text("Clave Alumno");
        TipoBusqueda = 1;
        $("#dpbuscar").hide();
        $("#divClave").show();
    });
    $("#rdReferencias").click(function () {
        $("#lbBuscar").text("Referencia");
        TipoBusqueda = 2;
        $("#dpbuscar").hide();
        $("#divClave").show();
    });
    $("#rdImporte").click(function () {
        $("#lbBuscar").text("Importe");
        TipoBusqueda = 3;
        $("#dpbuscar").hide();
        $("#divClave").show();
    });
    $("#rdFecha").click(function () {
        $("#lbBuscar").text("Fecha de Pago");
        TipoBusqueda = 4;
        $("#dpbuscar").show();
        $("#divClave").hide();
    });

    if (jQuery().datepicker) {
        $('.date-picker').datepicker({
            rtl: Metronic.isRTL(),
            orientation: "left",
            autoclose: true,
            language: 'es'
        });
        //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
    }

    $("#btnBuscar").click(function () {
        if (TipoBusqueda != 4) {
            buscar = $('#txtClave').val();
        } else { buscar = $('#txtFechaInicio').val(); }

        if (buscar.length == 0) { return false; }
        if (tblReferencias != undefined) {
            tblReferencias.fnClearTable();
        }
       
        IndexFn.Block(true);
        BuscarReferencia(buscar);

    });

    function BuscarReferencia(buscar) {
        var BECA;

        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ReferenciasConsulta",
            data: "{Dato:'" + buscar + "',TipoBusqueda:'"+TipoBusqueda+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (res) {
                var data = res.d;
                if (data === null) {
                    IndexFn.Block(false);
                    return false;
                }
                tblReferencias = $('#tblReferencias3').dataTable({
                    "aaData": data,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                        { "mDataProp": "ReferenciaId" },
                        { "mDataProp": "FechaPago" },
                        { "mDataProp": "MontoReferencia" },
                        { "mDataProp": "MontoPagado" },
                        { "mDataProp": "Saldo" }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
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
                        row.childNodes[0].style.textAlign = 'left';
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'right';
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'right';
                        row.childNodes[5].style.textAlign = 'right';
                        row.childNodes[6].style.textAlign = 'right';
                       
                    }
                });

                IndexFn.Block(false);
            }
        });
    }




    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });



});