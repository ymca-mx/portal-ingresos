$(function init() {
    var objAlumno = undefined;
    var tblBecas;
    var AlumnoId;

    $('#btnBuscarAlumno').click(function () {
        AlumnoId = $('#txtAlumno').val();
        objAlumno = undefined;
        if (AlumnoId.length > 0) {          
            $('#Load').modal('show');
            ObtenerAlumno();
        } else { return false; }
    });

    $('#txtAlumno').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscarAlumno').click();
        }
    });

    function ObtenerAlumno() {
        $.ajax({
            url: 'WS/Alumno.asmx/ConsultarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    alertify.alert("El Alumno no Existe.");
                    $('#Load').modal('hide');
                    return false;
                }
                else {
                    var nombre = $('#lblNombre');
                    nombre[0].innerText = data.d.Nombre + ' ' + data.d.Paterno + ' ' + data.d.Materno;
                    llenarComboOfertas(data.d.lstAlumnoInscrito);
                }
            }
        });
    }

    function llenarComboOfertas(ofertas) {
        $("#slcOfertas").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val(-1);
        $("#slcOfertas").append(optionP);

        if (ofertas.length == 0) { $('#Load').modal('hide'); return false; }
        if (ofertas.length > 1) {
            $(ofertas).each(function () {
                var option1 = $(document.createElement('option'));
                option1.text(this.OfertaEducativa.Descripcion);
                option1.val(this.OfertaEducativaId);
                $("#slcOfertas").append(option1);
            });
        } else {
            var option1 = $(document.createElement('option'));
            option1.text(ofertas[0].OfertaEducativa.Descripcion);
            option1.val(ofertas[0].OfertaEducativa.OfertaEducativaId);
            $("#slcOfertas").append(option1);            
        }
        $("#slcOfertas").val(ofertas[0].OfertaEducativaId);

        $("#slcOfertas").change();
        $('#Load').modal('hide');
    }

    $("#slcOfertas").on('change', function () {
        var op = $("#slcOfertas").val();
        if (op !== -1) {
            $('#Load').modal('show');
            CargarDescuentos(op);
        }
    });

    function CargarDescuentos(OfertaEducativa) {
        $.ajax({
            url: 'WS/Beca.asmx/DescuentosAnteriores',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d.length > 0) {
                    if (tblBecas != null) {
                        tblBecas.fnClearTable();
                    }

                    tblBecas = $("#tblBecas").dataTable({
                        "aaData": data.d,
                        "aoColumns": [
                            { "mDataProp": "AnioPeriodoId" },
                            { "mDataProp": "DescripcionPeriodo" },
                            { "mDataProp": "SMonto" },
                            { "mDataProp": "BecaDeportiva" },
                            { "mDataProp": "OtrosDescuentos" },
                            { "mDataProp": "BecaSEP" },
                            { "mDataProp": "BecaComite" },
                            { "mDataProp": "Usuario.Nombre" },
                            { "mDataProp": "FechaAplicacionS" }
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
                            row.childNodes[1].style.textAlign = 'center';
                            row.childNodes[2].style.textAlign = 'center';
                            row.childNodes[3].style.textAlign = 'center';
                            row.childNodes[4].style.textAlign = 'center';
                        }
                    });
                }
                $('#Load').modal('hide');
            }
        });
    }
});