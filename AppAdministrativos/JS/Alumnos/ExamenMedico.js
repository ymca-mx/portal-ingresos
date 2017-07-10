$(document).ready(function () {
    var tblAlumnos, AlumnoId = -1;
    $('#btnBuscarAlumno').click(function () {
        $('#frmVarios').hide();
        if (tblAlumnos != undefined) {
            tblAlumnos.fnClearTable();
        }
        if ($('#txtAlumno').val().length == 0) { return false; }
        
        $('#Load').modal('show');
        AlumnoNum = $('#txtAlumno').val();

        if (!isNaN(AlumnoNum)) {
            EsNumero(AlumnoNum);
        } else {
            EsString(AlumnoNum);
        }
    });

    
    function EsNumero(Alumno) {
        $('#chkExamen')[0].checked = false;
        var spam = $('#chkExamen')[0].parentElement;
        $(spam).removeClass('checked');

        AlumnoId = Alumno;
        $.ajax({
            type: "POST",
            url: "WS/ExamenMedico.asmx/ObtenerAlumno",
            data: "{AlumnoId:'" + Alumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    $('#txtNombre').val(data.d.Nombre);
                    if (data.d.ExamenMedico) {
                        $('#chkExamen')[0].checked = true;
                        var spq = $('#chkExamen')[0].parentElement;
                        $(spq).addClass('checked');
                    }
                    $('#Load').modal('hide');
                } else {
                    alertify.alert("Error al traer los datos del Alumno");
                    $('#Load').modal('hide');
                }
            }
        });
    }
    $('#btnGuardaExamen').on('click', function () {
        
        var usuario = $.cookie('userAdmin');
        if (AlumnoId == -1) { return false; }
        var comentario = $('#txtComentario').val();
        var Examen = $('#chkExamen')[0].checked;
        var objAl = {
            'Alumno': {
                'AlumnoId': AlumnoId,
                'Nombre': '',
                'ExamenMedico': Examen,
                'Comentario': comentario,
                'UsuarioId':usuario
            }
        };
        objAl = JSON.stringify(objAl);
        alertify.confirm("¿Esta seguro que desea guardar el examen medico?", function (e) {
            if (e) {
                $('#Load').modal('show');
                $.ajax({
                    url: 'WS/ExamenMedico.asmx/GuardarExamen',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: objAl,
                    dataType: 'json',
                    success: function (data) {
                        if (data.d) {
                            $('#Load').modal('hide');
                            alertify.alert("Datos guardados", function () {

                                $('#chkExamen')[0].checked = false;
                                var spam = $('#chkExamen')[0].parentElement;
                                $(spam).removeClass('checked');

                                AlumnoId = -1;
                                $('#txtComentario').val('');
                                $('#txtAlumno').val('');
                                $('#txtNombre').val('');                                

                            });
                        } else {
                            $('#Load').modal('hide');
                            alertify.alert("Error al guardar datos, intente de nuevo");
                        }
                    }
                });
            }
        });
    });
    function EsString(Alumno) {
        $.ajax({
            url: 'WS/Alumno.asmx/BuscarAlumnoString',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{Filtro:"' + Alumno + '"}',
            dataType: 'json',
            success: function (data) {
                if (data != null) {
                    tblAlumnos = $('#tblAlumnos').dataTable({
                        "aaData": data.d,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId" },
                            { "mDataProp": "Nombre" },
                            { "mDataProp": "FechaRegistro" },
                            { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                            //{ "mDataProp": "FechaSeguimiento" },
                            {
                                "mDataProp": function (data) {
                                    return "<a class='btn green'>Seleccionar</a>";
                                }
                            }
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": false,
                        "ordering": false,
                        "async": true,
                        "bDestroy": true,
                        "bPaginate": true,
                        "bLengthChange": false,
                        "bFilter": false,
                        "bInfo": false,
                        "pageLength": 5,
                        "bAutoWidth": false,
                        "asStripClasses": null,
                        "language": {
                            "lengthMenu": "_MENU_  Registros",
                            "paginate": {
                                "previous": "<",
                                "next": ">"
                            },
                            "search": "Buscar Alumno "
                        },
                        "order": [[2, "desc"]]
                    });
                    $('#Load').modal('hide');
                    $('#frmVarios').show();
                }
            }
        });
    }
    $('#txtAlumno').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscarAlumno').click();
        }
    });
    $('#tblAlumnos').on('click', 'a', function () {
        $('#frmVarios').hide();
        $('#Load').modal('show');
        var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
        AlumnoNum = rowadd.AlumnoId;
        EsNumero(rowadd.AlumnoId);
    });
});