$(function init() {
    var Funciones = {
        init: function () {
            Funciones.TraerPeriodos();
            Funciones.ArmarFechas();
        },
        tblDatos: null,
        ArmarFechas: function () {
            var fecha = new Date();


            var mes = fecha.getMonth();
            var dia = fecha.getDate();

            mes = mes < 9 ? (mes + 1) : mes + 1;
            mes = mes < 10 ? '0' + mes : mes;

            dia = dia < 10 ? '0' + dia : dia;

            var fini = fecha.getFullYear() + '-01-01';
            var ffin = fecha.getFullYear() + '-' + mes + '-' + dia;


            $('#calInicial').val(fini);
            $('#calFinal').val(ffin);
        },
        TraerPeriodos: function () {
            $('#Load').modal('show');
            $.ajax({
                url: 'WS/Reporte.asmx/CargarCuatrimestreHistorico',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{}',
                dataType: 'json',
                success: function (Respuesta) {
                    var option = $(document.createElement('option'));
                    option.text('--Seleccionar--');
                    $(Respuesta.d.periodos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.descripcion);
                        option.attr("data-Anio", this.anio);
                        option.attr("data-PeriodoId", this.periodoId);
                        option.val(this.anio + '' + this.PeriodoId);

                        $("#slcPeriodos").append(option);
                    });
                    $('#Load').modal('hide');
                }
            });
        },
        TraerDatos: function () {
            $('#Load').modal('show');
            var datos = {
                Anio: $('#slcPeriodos option:selected').data("anio"),
                PeriodoId: $('#slcPeriodos option:selected').data("periodoid"),
                FechaInicial: $('#calInicial').val(),
                FechaFinal: $('#calFinal').val()
            };
            datos.FechaFinal = datos.FechaFinal.replace('-', '/').replace('-', '/');
            datos.FechaInicial = datos.FechaInicial.replace('-', '/').replace('-', '/');

            datos = JSON.stringify(datos);
            $.ajax({
                url: 'WS/Reporte.asmx/CarteraVencida',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: datos,
                dataType: 'json',
                success: function (Respuesta) {
                    if (Respuesta.d !== null) {
                        Funciones.PintarTabla(Respuesta.d);
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Intente nuevamente mas tarde");
                    }
                }
            });
        },
        PintarTabla: function (tabla) {
            Funciones.tblDatos = $('#tblDatos').dataTable({
                "aaData": tabla,
                "aoColumns": [
                    { "mDataProp": "Alumno" },
                    { "mDataProp": "FechaPagoS" },
                    { "mDataProp": "Tipo_de_pago" },
                    { "mDataProp": "Concepto" },
                    { "mDataProp": "Pago" },
                    { "mDataProp": "Restante" },
                ],
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": true,
                "ordering": true,
                "async": true,
                "bDestroy": true,
                "bPaginate": true,
                "bLengthChange": false,
                "bFilter": true,
                "bInfo": true,
                "pageLength": 20,
                "bAutoWidth": true,
                "asStripClasses": null,
                "language": {
                    "lengthMenu": "_MENU_  Registros",
                    "paginate": {
                        "previous": "<",
                        "next": ">"
                    },
                    "search": "Buscar Alumno "
                },
                "order": [[1, "desc"]]
            });
            var fil = $('#tblDatos_filter label input');
            fil.removeClass('input-small').addClass('input-large');
            $('#Load').modal('hide');
        }
    };

    Funciones.init();
    $('#btnBuscar').on('click', Funciones.TraerDatos);
});