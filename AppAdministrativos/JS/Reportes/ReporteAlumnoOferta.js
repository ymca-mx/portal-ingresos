$(document).ready(function () {
    var tblReporte
    CargarReporteAlumnoOferta();

    function CargarReporteAlumnoOferta() {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/ObtenerReporteAlumnoOferta",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }

                tblReporte = $("#dtReporte").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId"},
                        { "mDataProp": "nombreAlumno"},
                        { "mDataProp": "oferta" },
                        { "mDataProp": "ultimoAnio" }
                    ],
                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": false,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "colReorder": false,
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'left';
                        row.childNodes[3].style.textAlign = 'center';
                    }

                });//$('#dtReporte').DataTable
                IndexFn.Block(false);
            }//success
        });//$.ajax
       

    }//CargarAlumnosInscritos()



});