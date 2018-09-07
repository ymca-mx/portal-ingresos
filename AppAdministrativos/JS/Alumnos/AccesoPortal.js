$(document).ready(function () {
    var AlumnoId, Alumno, tblBAcceso, tblBPassword, cursor = 0;

    var funciones =
        {
            BuscarAlumno: function () {
                AlumnoId = $('#txtClave').val();
                if (AlumnoId.length == 0 || parseInt(AlumnoId) < 1) { return false; }

                IndexFn.Block(true);
                $.ajax({
                    type: "POST",
                    url: "WS/Alumno.asmx/BitacoraAccesoAlumno",
                    data: "{AlumnoId:" + AlumnoId + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d === null) {
                            alumno = null;

                            alertify.alert("Alumno no existe.");
                            IndexFn.Block(false);
                            return false;
                        }
                        Alumno = data.d;

                        $("#lbNombre").text(Alumno.Nombre);
                        $("#myPassword").val(Alumno.Password)

                        var bitacceso = Alumno.BitacoraAcceso;
                        var bitpassword = Alumno.BitacoraPassword;

                        tblBAcceso = $("#tblBAcceso").DataTable({
                            "aaData": bitacceso,
                            "aoColumns": [
                                { "mDataProp": "FechaIngreso" },
                                { "mDataProp": "HoraIngreso" }
                            ],
                            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                            "searching": false,
                            "ordering": false,
                            "async": false,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bLengthChange": true,
                            "bFilter": false,
                            "bInfo": false,
                            "bAutoWidth": false,
                            "asStripClasses": null,
                            "colReorder": false,
                            "oSearch": { "bSmart": false },
                            "dom": '<"row view-filter"<"col-sm-12"<"pull-left"l><"pull-right"f><"clearfix">>>t<"row view-pager"<"col-sm-12"<"text-center"ip>>>',
                            "language": {
                                "lengthMenu": "_MENU_ Registro",
                                "paginate": {
                                    "previos": "<",
                                    "next": ">"
                                },
                                "search": "Buscar ",
                            },
                            "order": [[0, "desc"]]

                        });

                        tblBPassword = $('#tblBPassword').dataTable({
                            "aaData": bitpassword,
                            "aoColumns": [
                                { "mDataProp": "FechaSolicitud" },
                                { "mDataProp": "HoraSolicitud" }
                            ],
                            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                            "searching": false,
                            "ordering": false,
                            "async": true,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bLengthChange": true,
                            "bFilter": false,
                            "bInfo": false,
                            "bAutoWidth": false,
                            "asStripClasses": null,
                            "dom": '<"row view-filter"<"col-sm-12"<"pull-left"l><"pull-right"f><"clearfix">>>t<"row view-pager"<"col-sm-12"<"text-center"ip>>>',
                            "language": {
                                "lengthMenu": "_MENU_  Registros",
                                "paginate": {
                                    "previous": "<",
                                    "next": ">"
                                },
                                "search": "Buscar "
                            },
                            "order": [[0, "desc"]]
                        });

                        IndexFn.Block(false);
                    }
                });

            }
        }
    

    $("#imgVer").mouseover(function () {
        var obj = document.getElementById('myPassword');
        obj.type = "text";
    });

    $("#imgVer").mouseout(function () {
        var obj = document.getElementById('myPassword');
        obj.type = "password";
    });



    $('#btnBuscar').click(funciones.BuscarAlumno);

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            funciones.BuscarAlumno();
        }
    });

});