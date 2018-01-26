$(document).ready(function () {

    var tbPagos;
    var tbPagosPendientes;

    $.ajax({
        url: 'Services/Alumno.asmx/PagosAlumno',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "{'alumnoId': '9535', 'ofertaEducativaId': '1' }",

        success: function (Resultado) {
            tbPagos = $('#tbPagos').dataTable({
                'aaData': Resultado.d,
                'aoColumns': [
                    { 'mDataProp': 'fechaPago', 'width': 'auto'},
                    { 'mDataProp': 'conceptoPago' },
                    { 'mDataProp': 'periodo' },
                    { 'mDataProp': 'cuota' },
                    { 'mDataProp': 'descuento' },
                    { 'mDataProp': 'importe' }
                ],
                'lengthMenu': [[10, 20, 50, 100, -1], [10, 20, 50, 100, 'Todos']],
                'searching': true,
                'ordering': true,
                'info': false,
                'language': {
                    'lengthMenu': '_MENU_ Registros',
                    'paginate': {
                        'previous': '<',
                        'next': '>'
                    },
                    'search': 'Buscar Pago'
                }
            });
        }
    });

    $.ajax({
        url: 'Services/Alumno.asmx/PagosAlumnoPendientes',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "{'alumnoId': '9535', 'ofertaEducativaId': '1' }",

        success: function (Resultado) {
            tbPagos = $('#tbPagosPendientes').dataTable({
                'aaData': Resultado.d,
                'aoColumns': [
                    { 'mDataProp': 'conceptoPago', 'width': 'auto' },
                    { 'mDataProp': 'periodo' },
                    { 'mDataProp': 'cuota' },
                    { 'mDataProp': 'descuento' },
                    { 'mDataProp': 'importe' },
                    { 'mDataProp': 'referenciaId' }
                ],
                'lengthMenu': [[10, 20, 50, 100, -1], [10, 20, 50, 100, 'Todos']],
                'searching': true,
                'ordering': true,
                'info': false,
                'language': {
                    'lengthMenu': '_MENU_ Registros',
                    'paginate': {
                        'previous': '<',
                        'next': '>'
                    },
                    'search': 'Buscar Pago Pendiente'
                },
                "createdRow": function (row, data, index) {
                    //$('td', row).eq(5).addClass('Fecha');
                    $('td', row).eq(5).addClass('btn red btn-xs');
                }
            });
        }
    });
});