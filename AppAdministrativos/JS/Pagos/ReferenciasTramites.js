$(document).ready(function () {
    var AlumnoId;
    var PeriodoId;
    var Anio;
    var tblReferencias;
    var PeriodoAlcorriente = null;
    var Periodo = null;
    var Tipo;

    $('#btnBuscar').click(function () {
        var lbl = $('#lblNombre');
        lbl[0].innerHTML = "";
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0) { return false; }
        if (tblReferencias != undefined) {
            tblReferencias.fnClearTable();
        }
        $('#Load').modal('show');
        BuscarAlumno(AlumnoId);

    });
    function BuscarAlumno(idAlumno) {
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultarAlumno",
            data: "{AlumnoId:'" + idAlumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    $('#Load').modal('hide');
                    return false;
                } 

                    var lbl = $('#lblNombre');
                    lbl[0].innerHTML = data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno;
                    lbl[0].innerHTML += data.d.AlumnoInscrito.EsEmpresa == true ? data.d.AlumnoInscrito.EsEspecial == true ? " - Alumno Especial  " : " - Grupo  Empresarial" : ""; // +data.d.Grupo.Descripcion : ""; 
                    CargarPagos();
            }
        });
    }
    function CargarPagos() {
        var BECA;

        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultaPagosTramites",
            data: "{AlumnoId:'" + AlumnoId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (res) {
                var data = res.d.item1;
                var dk = res.d.item2;
                if (data === null) {
                    $('#Load').modal('hide');
                    return false;
                }

                    var Especial = res.d.item1[0].EsEspecial;


                    tblReferencias = $('#tblReferencias3').dataTable({
                        "aaData": data,
                        "bSort": false,
                        "aoColumns": [
                            { "mDataProp": "Concepto" },
                            { "mDataProp": "ReferenciaId" },
                            { "mDataProp": "Periodo" },
                            { "mDataProp": "CargoFechaLimite" },
                            { "mDataProp": "TotalMDescuentoMBecas" },
                            { "mDataProp": "OtroDescuento" },
                            { "mDataProp": "Pagado" },
                            { "mDataProp": "SaldoPagado" }
                        ],
                        "columnDefs": [
                          {
                              "targets": [5],
                              "visible": dk,
                              "searchable": false
                          },
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": true,
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
                            row.childNodes[0].style.textAlign = 'left';
                            row.childNodes[1].style.textAlign = 'left';
                            row.childNodes[2].style.textAlign = 'center';
                            row.childNodes[3].style.textAlign = 'center';
                            row.childNodes[4].style.textAlign = 'right';
                            row.childNodes[5].style.textAlign = 'right';
                            row.childNodes[6].style.textAlign = 'right';
                            if (dk) {
                                row.childNodes[7].style.textAlign = 'right';
                            }
                            if (data.Pagoid == 0) {
                                row.childNodes[0].style.fontWeight = 'bold';
                                row.childNodes[0].style.fontSize = '12px';
                            } if (data.Adeudo == true) {
                                row.style.color = "#FFFFFF";
                                row.style.backgroundColor = '#e35b5a';
                            }
                        }
                    });

                    var tr
                    if (dk) {
                        tr = '<tr id = "tr1">' +
                         '<th></th>' +
                         '<th></th>' +
                         '<th></th>' +
                         '<th></th>' +
                         '<th></th>' +
                         '<th></th>' +
                         '<th></th>' +
                         '<th style="text-align:right">' + data[0].TotalPagado + '</th></tr>';
                    } else {
                        tr = '<tr id = "tr1">' +
                            '<th></th>' +
                            '<th></th>' +
                            '<th></th>' +
                            '<th></th>' +
                            '<th></th>' +
                            '<th></th>' +
                            '<th style="text-align:right">' + data[0].TotalPagado + '</th></tr>';
                    }
                    //var tabla = document.getElementById("tblReferencias3");
                    document.getElementById("tblReferencias3").insertRow(-1).innerHTML = tr;
                $('#Load').modal('hide');
            }
        });
    }



    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });



});