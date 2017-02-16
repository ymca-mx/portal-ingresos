$(document).ready(function () {
    var tblReporte
    CargarReporteAlumnoOferta();

    function CargarReporteAlumnoOferta() {
        $('#Load').modal('show');
        $.ajax({
            type: 'POST',
            url: "../WebServices/WS/Reporte.asmx/MostraReporteAlumnoOferta",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
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
                    "colReorder": true,
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
                $('#Load').modal('hide');
            }//success
        });//$.ajax
       

    }//CargarAlumnosInscritos()



});