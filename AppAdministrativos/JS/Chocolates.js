$(document).ready(function () {
    var userAdmin;
    var Alumnos;
    $('#txtFiltro').change(function () {
        buscar();
    });
    $('#txtFiltro').keyup(function (key) {
        if (key.keyCode == 32) {
            buscar();
        }
    });
    $('.switch-radio1').on('switch-change', function () {
        $('.switch-radio1').bootstrapSwitch('toggleRadioState');
    });

    // or
    $('.switch-radio1').on('switch-change', function () {
        $('.switch-radio1').bootstrapSwitch('toggleRadioStateAllowUncheck');
    });

    // or
    $('.switch-radio1').on('switch-change', function () {
        $('.switch-radio1').bootstrapSwitch('toggleRadioStateAllowUncheck', false);
    });
    function buscar() {
        var filtro;
        filtro = $('#txtFiltro').val();
        $.ajax({
            url: 'WS/Alumno.asmx/BuscarAlumnoFiltro',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{filtro:"' + filtro + '"}',
            dataType: 'json',
            success: function (data) {
                Alumnos = $('#tblAlumnos').dataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                        { "mDataProp": "Chocolates" },
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
                        "search": "Buscar Alumno ",
                    },
                    "order": [[2, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        //row.childNodes[1].style.textAlign = 'center';
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'center';
                        row.childNodes[2].childNodes[0].textContent = "";
                        if (data.Chocolates == false) {
                            $(row.childNodes[2]).append('<div class="col-md-9"><input type="checkbox" class="make-switch" data-on-text="Si" data-off-text="No"></div>');
                        } else {
                            $(row.childNodes[2]).append('<div class="col-md-9"><input type="checkbox" class="make-switch" data-on-text="Si" data-off-text="No"></div>');
                        }
                    }
                });
            }
        });
    }
});