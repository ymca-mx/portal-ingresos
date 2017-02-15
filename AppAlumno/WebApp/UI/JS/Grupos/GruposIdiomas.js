$(document).ready(function () {
    var AlumnoId, GrupoId,GrupoId2, TipoDeCurso,OfertaId,TM;
    var tblGruposIdiomas, tblGruposIdiomas1, tblAlumnosIdiomas, tblAlumnosIdiomasGrupo;

    $("#liS1").addClass('active');
    cargarperido();
    cargarGrupos();





    $("#liS2").click(function () {
        $("#divCrear").show();
        $("#divConsultar").hide();
        $("#divEliminar").hide();
        $("#DivAgregarAlumnos").hide();
        $("#DivAlumnosGrupo").hide();
    });

    $("#liS1").click(function () {
        $("#divConsultar").show();
        $("#divCrear").hide();
        $("#divEliminar").hide();
        $("#DivAgregarAlumnos").hide();
        $("#DivAlumnosGrupo").hide();
        cargarGrupos();
       
    });

    $("#liS3").click(function () {
        $("#divEliminar").show();
        $("#divCrear").hide();
        $("#divConsultar").hide();
        $("#DivAgregarAlumnos").hide();
        $("#DivAlumnosGrupo").hide();
        cargarGrupos2();
    });

    $('#btnAlumnosIdiomas').click(function () {
        cargarAlumnos();
        $("#DivAgregarAlumnos").show();
        $("#divConsultar").hide();
    });

    $('#btnAtrasAlumnosGrupos').click(function () {
        $("#DivAlumnosGrupo").hide();
        $("#divConsultar").show();
    });

    $('#btnAtrasAlumnosGrupos1').click(function () {
        $("#DivAgregarAlumnos").hide();
        $("#divConsultar").show();

    });

    $('#dtGruposIdiomas').on('click', 'a', function () {
        var rowadd = tblGruposIdiomas.fnGetData($(this).closest('tr'));
        if (this.name == "btnModificar") {
            $("#txtNombreGrupo1").val(rowadd.Descripcion);
            $("#slcCuatrimestre1").val(rowadd.PeriodoId + " " + rowadd.Anio);
            $('#PopGrupo').modal('show');
            GrupoId = rowadd.GrupoIdiomaId;
        } else
        {
            GrupoId = rowadd.GrupoIdiomaId;
            $("#divConsultar").hide();
            cargarAlumnosGrupo();
            $("#NombreGrupo").text(rowadd.Descripcion);
            GrupoId2 = rowadd.GrupoIdiomaId;

            $("#DivAlumnosGrupo").show();
        }
        
    });

    $('#dtGruposIdiomas1').on('click', 'a', function () {
        if (this.name == "btnEliminar") {
            var rowadd = tblGruposIdiomas1.fnGetData($(this).closest('tr'));
            GrupoId = rowadd.GrupoIdiomaId;
            alertify.confirm("<p>¿ Seguro que deseas eliminar este grupo?<br><br><hr>", function (e) {
                if (e) {
                    $('#txtBar').text("Eliminando");
                    $('#divBar').modal('show');
                    $.ajax({
                        type: "POST",
                        url: "../WebServices/WS/GrupoIdioma.asmx/EliminarGrupoIdioma",
                        data: "{grupoId:'" + GrupoId + "'}",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d == "Eliminado") {

                                alertify.alert("Grupo eliminado correctamente.");
                                cargarGrupos2();

                                $('#divBar').modal('hide');
                            }
                        }
                    });
                } else {
                   
                }
            });
         

        }

    });

    $('#dtAlumnos').on('click', 'a', function () {
        if (this.name == "BtnAgregarAlumno") {
            var rowadd = tblAlumnosIdiomas.fnGetData($(this).closest('tr'));
            AlumnoId = rowadd.AlumnoId;
            TipoDeCurso = rowadd.TipoDeCurso;
            OfertaId = rowadd.OfertaEducativaId;
            TM = 1;
            $('#slcGrupo').val(-1);
            $("#btnAlumnoGrupoAsignar").prop('disabled', false);
            $("#slcGrupo").prop('disabled', false);
            $('#PopAlumnoGrupoAsignar').modal('show');
        } else {
            var rowadd = tblAlumnosIdiomas.fnGetData($(this).closest('tr'));
            AlumnoId = rowadd.AlumnoId;
            TipoDeCurso = rowadd.TipoDeCurso;
            GrupoId = rowadd.GrupoAlumno.GrupoIdiomaId;
            $('#slcGrupo').val(GrupoId);
            $("#btnAlumnoGrupoAsignar").prop('disabled', true);
            $("#slcGrupo").prop('disabled', true);
            $('#PopAlumnoGrupoAsignar').modal('show');
        }

    });

    $('#dtGruposAlumnos').on('click', 'a', function () {
        if (this.name == "BtnModificar") {
            var rowadd = tblAlumnosIdiomasGrupo.fnGetData($(this).closest('tr'));
            AlumnoId = rowadd.AlumnoId;
            TipoDeCurso = rowadd.TipoDeCurso;
            OfertaId = rowadd.OfertaEducativaId;
            TM = 2 ;
            $('#slcGrupo').val(GrupoId);
            $("#btnAlumnoGrupoAsignar").prop('disabled', false);
            $("#slcGrupo").prop('disabled', false);
            $('#PopAlumnoGrupoAsignar').modal('show');
        } else {
            var rowadd = tblAlumnosIdiomasGrupo.fnGetData($(this).closest('tr'));
            AlumnoId = rowadd.AlumnoId;
            TipoDeCurso = rowadd.TipoDeCurso;
            OfertaId = rowadd.OfertaEducativaId;
            alertify.confirm("<p>¿Seguro que deseas eliminar a este alumno del grupo?<br><br><hr>", function (e) {
                if (e) {
                   
                    TM = 3;
                    $('#btnAlumnoGrupoAsignar').click();
                } else {
                }
            });
           

        }

    });

    $('#btnCrearGrupo').click(function () {
        if ($('#txtNombreGrupo').val() == "" )
        {
            alertify.alert("No has indicado el nombre del grupo.");
        } else {
            var usuario = $.cookie('userAdmin');
            var nombre = $('#txtNombreGrupo').val();
            var periodo = $('#slcCuatrimestre').val();
            var periodoId = periodo.substring(0, 1);
            var anio = periodo.substring(2, 6);
            $('#txtBar').text("Guardando");
            $('#divBar').modal('show');
            $.ajax({
                type: "POST",
                url: "../WebServices/WS/GrupoIdioma.asmx/GuardarGrupoIdioma",
                data: "{nombre:'" + nombre + "',anio:'" + anio +
                    "',periodoid:'" + periodoId + "',usuarioid:'" + usuario + "',grupoId:'" + 0 + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == "Guardado") {
                        $("#txtNombreGrupo").val("");
                        alertify.alert("Grupo guardado correctamente.");

                        $('#divBar').modal('hide');
                    }
                }
            });
        }

    });

    $('#btnGrupoG1').click(function () {

        if ($('#txtNombreGrupo1').val() == "") {
            alertify.alert("No has indicado el nombre del grupo.");
        } else {
            var usuario = $.cookie('userAdmin');
            var nombre = $('#txtNombreGrupo1').val();
            var periodo = $('#slcCuatrimestre1').val();
            var periodoId = periodo.substring(0, 1);
            var anio = periodo.substring(2, 6);
            $('#txtBar').text("Guardando");
            $('#divBar').modal('show');
            $.ajax({
                type: "POST",
                url: "../WebServices/WS/GrupoIdioma.asmx/GuardarGrupoIdioma",
                data: "{nombre:'" + nombre + "',anio:'" + anio +
                        "',periodoid:'" + periodoId + "',usuarioid:'" + usuario + "',grupoId:'" + GrupoId + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == "Guardado") {
                        $("#txtNombreGrupo1").empty();
                        alertify.alert("Grupo guardado correctamente.");
                        $('#PopGrupo').modal('hide');
                        cargarGrupos();
                        $('#divBar').modal('hide');
                    }
                }
            });
        }

        
        
    });

    $('#btnCerrar').click(function () {
        $('#PopGrupo').modal('hide');
    });

    $('#btnCerrarGrupo1').click(function () {
        $('#PopAlumnoGrupoAsignar').modal('hide');
    });

    $('#btnAlumnoGrupoAsignar').click(function () {

        if ($('#slcGrupo').val() == -1) {
            alertify.alert("Debes de seleccionar un grupo.");
        } else {
            $('#txtBar').text("Cargando");
            $('#divBar').modal('show');
            var usuario = $.cookie('userAdmin');
            if (TM != 3) {
                GrupoId = $('#slcGrupo').val();
            }
            $.ajax({
                type: "POST",
                url: "../WebServices/WS/GrupoIdioma.asmx/AsignarAlumnosIdiomas",
                data: "{AlumnoId:'" + AlumnoId + "',GrupoId:'" + GrupoId +
                        "',TipoCurso:'" + TipoDeCurso + "',usuarioid:'" + usuario + "',OfertaId:'" + OfertaId + "',TM:'" + TM + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d == "Guardado") {
                        $('#PopAlumnoGrupoAsignar').modal('hide');
                        $('#divBar').modal('hide');

                        if (TM == 1) {
                            alertify.alert("Alumno asignado correctamente.");
                            cargarAlumnos();

                        } else if (TM == 2) {
                            alertify.alert("Alumno asignado correctamente.");
                            GrupoId = GrupoId2;
                            cargarAlumnosGrupo();
                        }
                        else {
                            alertify.alert("Alumno eliminado correctamente.");
                            GrupoId = GrupoId2;
                            cargarAlumnosGrupo();
                        }

                    } else {
                        alertify.alert("Error Al Asignar.");
                        $('#divBar').modal('hide');
                    }
                }
            });
        }

    });

    function CargarPagosConceptos(Alumno) {
        $.ajax({
            url: '../WebServices/WS/GrupoIdioma.asmx/ConsultarGruposIdiomas',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + Alumno + '}',
            dataType: 'json',
            success: function (Respuesta) {
                ReferenciasTbl(Respuesta);
                var fil = $('#tblReferencias label input');
                fil.removeClass('input-small').addClass('input-large');
                $('#divBar').modal('hide');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });
    }

    function cargarGrupos() {
        $('#txtBar').text("Cargando");
        $('#divBar').modal('show');
        $.ajax({
            url: '../WebServices/WS/GrupoIdioma.asmx/ConsultarGruposIdiomas',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (data) {

                $("#slcGrupo").empty();
                var optionP = $(document.createElement('option'));
                optionP.text('--Seleccionar--');
                optionP.val('-1');
                $("#slcGrupo").append(optionP);

                $(data.d).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.GrupoIdiomaId);

                    $("#slcGrupo").append(option);
                });

                tblGruposIdiomas = $('#dtGruposIdiomas').dataTable({
                    "aaData": data.d,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "GrupoIdiomaId", "Descripcion": "GrupoIdiomaId" },
                        {
                            "mDataProp": "Descripcion",
                            "mRender": function (data) {
                                return "<a name='btngrupo' href=''onclick='return false;'>" + data + " </a> ";
                            }
                        },
                        { "mDataProp": "Cuatrimestre" },
                        {
                            "mDataProp": function (data) {
                                return "<a class='btn yellow'  name ='btnModificar'>Modificar Grupo</a>";
                            }
                        }
                    ],

                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, 'Todos']],
                    "searching": true,
                    "ordering": false,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "language": {
                        "lengthMenu": "_MENU_  Grupo",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Referencia ",
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[0].style.textAlign = 'left';
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'left';

                    }
                });
                $('#divBar').modal('hide');


            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });


    }

    function cargarGrupos2() {
        $('#txtBar').text("Cargando");
        $('#divBar').modal('show');
        $.ajax({
            url: '../WebServices/WS/GrupoIdioma.asmx/ConsultarGruposIdiomas',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (data) {
                tblGruposIdiomas1 = $('#dtGruposIdiomas1').dataTable({
                    "aaData": data.d,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "GrupoIdiomaId" },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "Cuatrimestre" },
                        {
                            "mDataProp": function (data) {
                                return "<a class='btn red'  name ='btnEliminar'>Eliminar Grupo</a>";
                            }
                        }
                    ],

                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, 'Todos']],
                    "searching": true,
                    "ordering": false,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "language": {
                        "lengthMenu": "_MENU_  Referencias",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Grupo ",
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[0].style.textAlign = 'left';
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'left';

                    }
                });
                $('#divBar').modal('hide');


            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });


    }

    function cargarAlumnos() {

        $('#divBar').modal('show');
        $.ajax({
            url: '../WebServices/WS/GrupoIdioma.asmx/ConsultarAlumnosIdiomas',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (data) {
                tblAlumnosIdiomas = $('#dtAlumnos').dataTable({
                    "aaData": data.d,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                        { "mDataProp": "OfertaEducativa" },
                        { "mDataProp": "TipoDeCurso" },
                        {
                            "mDataProp": "GrupoAlumno",
                            "mRender": function (data, f, d) {
                                var link;
                                if (d.GrupoAlumno == null) {
                                    link = "<a href='' class='btn blue' onclick='return false;' name='BtnAgregarAlumno'>" + "Agregar Grupo" + " </a> ";
                                }
                                else { link = "<a href='' class='btn red' onclick='return false;'  name='BtnVerAlumno'>" + "Ver Grupo" + " </a> "; }
                                return link;
                            }
                        },

                    ],

                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "language": {
                        "lengthMenu": "_MENU_ Alumno",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[0].style.textAlign = 'left';
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'left';

                    }
                });
                $('#divBar').modal('hide');


            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });


    }

    function cargarAlumnosGrupo() {
        $('#txtBar').text("Cargando");
        $('#divBar').modal('show');
        $.ajax({
            url: '../WebServices/WS/GrupoIdioma.asmx/ConsultarAlumnosIdiomasGrupo',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: "{GrupoId:'" + GrupoId + "'}",
            dataType: 'json',
            success: function (data) {
                tblAlumnosIdiomasGrupo = $('#dtGruposAlumnos').dataTable({
                    "aaData": data.d,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                        { "mDataProp": "OfertaEducativa" },
                        { "mDataProp": "TipoDeCurso" },
                        {
                            "mDataProp": "GrupoAlumno",
                            "mRender": function (data) {
                                return "<a href='' class='btn green' onclick='return false;' name='BtnModificar'>" + "Modificar Grupo" + " </a> ";
                            }
                        },
                        {
                            "mDataProp": "GrupoAlumno",
                            "mRender": function (data) {
                                return "<a href='' class='btn red' onclick='return false;' name='BtnEliminar'>" + "Eliminar del Grupo" + " </a> ";
                            }
                        }

                    ],

                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "language": {
                        "lengthMenu": "_MENU_ Alumno",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[0].style.textAlign = 'left';
                        row.childNodes[1].style.textAlign = 'left';
                        row.childNodes[2].style.textAlign = 'left';

                    }
                });
                $('#divBar').modal('hide');


            },
            error: function (data) {
                alertify.alert('Error al cargar datos');
                $('#divBar').modal('hide');
            }
        });


    }

    function cargarperido() {
        var PeriodoAlcorriente = null;
        var Pariodo = null;
        var Pariodo1 = null;
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarPeriodos",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                var sig = null, act = null, sig2 = null;
                $(datos).each(function () {
                    //primer combo
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.PeriodoId + " " + this.Anio);
                    $("#slcCuatrimestre").append(option);
                    //segundo combo
                    var option1 = $(document.createElement('option'));
                    option1.text(this.Descripcion);
                    option1.val(this.PeriodoId + " " + this.Anio);

                    $("#slcCuatrimestre1").append(option1);
                });
                //primer combo
                Pariodo = datos[0].PeriodoId + " " + datos[0].Anio;
                $("#slcCuatrimestre").val(Pariodo);
                //segundo combo
                Pariodo1 = datos[0].PeriodoId + " " + datos[0].Anio;
                $("#slcCuatrimestre1").val(Pariodo1);
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
            }
        });
    }

});
