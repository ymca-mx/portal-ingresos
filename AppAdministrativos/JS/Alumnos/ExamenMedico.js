$(document).ready(function () {
    var tblAlumnos, AlumnoId = -1;
    $('#btnBuscarAlumno').click(function () {
        $('#frmVarios').hide();
        if (tblAlumnos != undefined) {
            tblAlumnos.fnClearTable();
        }
        if ($('#txtAlumno').val().length == 0) { return false; }
        
        IndexFn.Block(true);
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
                    IndexFn.Block(false);
                } else {
                    alertify.alert("Error al traer los datos del Alumno");
                    IndexFn.Block(false);
                }
            }
        });
    }
    $('#btnGuardaExamen').on('click', function () {
        
        var usuario =  localStorage.getItem('userAdmin');
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
                IndexFn.Block(true);
                $.ajax({
                    url: 'WS/ExamenMedico.asmx/GuardarExamen',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: objAl,
                    dataType: 'json',
                    success: function (data) {
                        if (data.d) {
                            IndexFn.Block(false);
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
                            IndexFn.Block(false);
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
                    IndexFn.Block(false);
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
        IndexFn.Block(true);
        var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
        AlumnoNum = rowadd.AlumnoId;
        EsNumero(rowadd.AlumnoId);
    });
});