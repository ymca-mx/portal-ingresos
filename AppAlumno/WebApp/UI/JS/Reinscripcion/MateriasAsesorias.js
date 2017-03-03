$(document).ready(function () {
    var tblAlumnos;
    var AlumnoNum;
    var tblReferencias;
    var objAlumnoC;
    var estado = "";
    $('#btnBuscarAlumno').click(function () {
        $('#btnGuardar').prop("disabled", true);
        objAlumnoC = undefined;
        AlumnoNum = $('#txtAlumno').val();
        $('#frmAlumnos').hide();
        $('#lblEstatus').text('');
        $('#submit_form').trigger('reset');
        if (tblAlumnos != null) {
            tblAlumnos.fnClearTable();
        } if (tblReferencias != null) {
            tblReferencias.fnClearTable();
        }
        
        if (!isNaN(AlumnoNum)) {
            EsNumero(AlumnoNum);
        } else {
            EsString(AlumnoNum);
        }
    });
    $('#txtNAsesoria').on('click', function () {
        this.select();
    });
    $('#txtNMateria').on('click', function () {
        this.select();
    });
    function EsNumero(Alumno) {
        $('#Load').modal('show');
        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Reinscripcion.asmx/AlumnoReinscripcion",
            data: "{AlumnoId:'" + Alumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    $('#lblNombre').text(data.d.Nombre);
                    objAlumnoC = data.d;
                    llenarOfertas();
                } else { $('#Load').modal('hide'); }
            }
        });
    }

    function llenarOfertas() {
        $("#slcOfertas").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcOfertas").append(optionP);
        if (objAlumnoC.lstOfertas.length == 0) { $('#Load').modal('hide'); return false; }
        if (objAlumnoC.lstOfertas.length > 1) {
            $(objAlumnoC.lstOfertas).each(function () {
                var option1 = $(document.createElement('option'));
                option1.text(this.Descripcion);
                option1.val(this.OfertaEducativaId);
                $("#slcOfertas").append(option1);
            });
        } else {
            var option1 = $(document.createElement('option'));
            option1.text(objAlumnoC.lstOfertas[0].Descripcion);
            option1.val(objAlumnoC.lstOfertas[0].OfertaEducativaId);
            $("#slcOfertas").append(option1);
            $("#slcOfertas").val(objAlumnoC.lstOfertas[0].OfertaEducativaId);
        }
        llenarPeriodos();
    }

    function llenarPeriodos() {
        $("#slcPeriodos").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcPeriodos").append(optionP);
        if (objAlumnoC.lstPeriodos.length == 0) { return false; }
        if (objAlumnoC.lstPeriodos.length > 1) {
            $(objAlumnoC.lstPeriodos).each(function () {
                var option1 = $(document.createElement('option'));
                option1.text(this.Descripcion);
                option1.val(this.PeriodoId+" " +this.Anio);
                $("#slcPeriodos").append(option1);
            });
        } else {
            var option1 = $(document.createElement('option'));
            option1.text(objAlumnoC.lstPeriodos[0].Descripcion);
            option1.val(objAlumnoC.lstPeriodos[0].PeriodoId + " " + objAlumnoC.lstPeriodos[0].Anio);
            $("#slcPeriodos").append(option1);
            if ($("#slcOfertas").val() != "-1") { $("#slcPeriodos").val(objAlumnoC.lstPeriodos[0].PeriodoId + " " + objAlumnoC.lstPeriodos[0].Anio); }
        }
        $("#slcPeriodos").change();        
        $(window).scrollTop($('#slcPeriodos').offset().top);
        $('#Load').modal('hide');
        
    }

    $("#slcOfertas").change(function () {
        var op = $("#slcOfertas").val();

        $(objAlumnoC.lstOfertas).each(function () {
            if (String(this.OfertaEducativaId) == op) {
                if (this.OfertaEducativaTipoId != 1) { $('#trAsesoria').hide(); } else { $('#trAsesoria').show(); }
            }
        });
        
        if (op.toString() == "-1") {            
            $('#txtPuAsesoria').val("$0.00");
            $('#txtPuMateria').val("$0.00");
            $('#txtNAsesoria').val(0);
            $('#txtNMateria').val(0);
            $("#slcPeriodos").val("-1");
            var spam = $('#rdbSi')[0].parentElement;
            $(spam).removeClass('checked');
            var spam1 = $('#rdbNo')[0].parentElement;
            $(spam1).removeClass('checked');
            if (tblReferencias != null) {
                tblReferencias.fnClearTable();
            }
            return false;
        }
        llenarPeriodos();
    });

    $("#slcPeriodos").change(function () {
        var op = $("#slcPeriodos").val();
        if (op == -1) {
            $('#txtPuAsesoria').val("$0.00");
            $('#txtPuMateria').val("$0.00");
            $('#txtNAsesoria').val(0);
            $('#txtNMateria').val(0);
            var spam = $('#rdbSi')[0].parentElement;
            $(spam).removeClass('checked');
            var spam1 = $('#rdbNo')[0].parentElement;
            $(spam1).removeClass('checked');
            if (tblReferencias != null) {
                tblReferencias.fnClearTable();
            }
            return false;
        }
        CarcarCuota();
    });

    function CarcarCuota() {
        var op = $("#slcPeriodos").val();
        if (op == -1) { return false; }
        var op1 = $("#slcOfertas").val();
        if (op1 == -1) { return false; }
        op = op.split(" ");
        var anio = op[1];
        var periodo = op[0];

        $(objAlumnoC.Cuotas).each(function () {
            if (this.Anio.toString() == anio.toString()
                && this.PeriodoId.toString() == periodo.toString()
                && this.OfertaEducativaId.toString() == op1.toString()) {
                if (this.PagoConceptoId == 15) {
                    $('#txtPuAsesoria').val("$" + this.Monto);
                } else {
                    $('#txtPuMateria').val("$" + this.Monto);
                }
            }

        });
        CargarEstatus(op1, anio, periodo);
        TablaReferencias(op1, anio, periodo);
    }
    function CargarEstatus(oferta, anio, periodo) {
        estado="";
        $('#lblEstatus').text('');
        $(objAlumnoC.EstatusAl).each(function(){
            if (this.Anio.toString() == anio.toString()
               && this.Periodo.toString() == periodo.toString()
               && this.OfertaEducativaId.toString() == oferta.toString()) {
                estado = this.Estado;
            }
        });
        $('#lblEstatus').text(estado);
    }
    $('#rdbSi').click(function () {
        if (estado.length > 0) { return false;}
        var op = $("#slcPeriodos").val();
        if (op == -1) { return false; }
        var op1 = $("#slcOfertas").val();
        if (op1 == -1) { return false; }
        var chk = this;
        chk = chk.checked;
        if (chk) {
            $('#btnGuardar').prop("disabled", false);
        }
    });
    $('#rdbNo').click(function () {
        if (estado.length > 0) { return false; }
        var op = $("#slcPeriodos").val();
        if (op == -1) {  return false; }
        var op1 = $("#slcOfertas").val();
        if (op1 == -1) { return false; }
        var chk = this;
        chk = chk.checked;
        if (chk) {
            $('#btnGuardar').prop("disabled", false);
        }
    });
    $('#btnGuardar').click(function () {
        Guardar();
    });

    function Guardar() {
        var op = $("#slcPeriodos").val();
        if (op == -1) { return false; }
        var op1 = $("#slcOfertas").val();
        if (op1 == -1) { return false; }
        op = op.split(" ");
        var anio = op[1];
        var periodo = op[0];
        $('#Load').modal('show');
        var usuario = $.cookie('userAdmin');
        var nAse = $('#txtNAsesoria').val();
        var nMat = $('#txtNMateria').val();
        var completa = ($('#rdbSi').attr("checked") ? true : false);
        var comentario = $('#txtComentario').val();
        //AlumnoNum
        var Datos = '{AlumnoId:"' + AlumnoNum + '",anio:"' + anio + '",periodo:"'
            + periodo + '",oferta:"' + op1 + '",NMaterias:"' + nMat + '",NAsesorias:"'
            + nAse + '",Completa:"' + completa + '",usuario:"' + usuario + '",Comentario:"' + comentario + '"}';

        $.ajax({
            url: '../WebServices/WS/Reinscripcion.asmx/Generar',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: Datos,
            dataType: 'json',
            success: function (data) {
                if (data.d == true) {
                    $('#Load').modal('hide');
                    alertify.alert("Datos Generados"); 
                    //Cargar Alumno
                    $.ajax({
                        type: "POST",
                        url: "/../WebServices/WS/Reinscripcion.asmx/AlumnoReinscripcion",
                        data: "{AlumnoId:'" + AlumnoNum + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data) {
                            if (data.d != null) {
                                $('#lblNombre').text(data.d.Nombre);

                                $('#btnGuardar').prop("disabled", true);
                                $('#txtNAsesoria').val(0);
                                $('#txtNMateria').val(0);
                                $('#txtSTAsesoria').val('$0.00');
                                $('#txtSTMateria').val('$0.00');
                                var spam = $('#rdbSi')[0].parentElement;
                                $(spam).removeClass('checked');
                                var spam1 = $('#rdbNo')[0].parentElement;
                                $(spam1).removeClass('checked');                                
                                objAlumnoC = data.d;
                                CargarEstatus(op1, anio, periodo);
                                TablaReferencias(op1, anio, periodo);
                            } else { $('#Load').modal('hide'); }
                        }
                    });
                } else { alertify.alert("Error Al Guardar"); $('#Load').modal('hide'); }
            }
        });
    }
    function EsString(Alumno) {
        $('#Load').modal('show');
        $('#frmAlumnos').show();
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/BuscarAlumnoString',
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
                }
                $('#Load').modal('hide');

            }
        });
    }

    function TablaReferencias(Oferta, Anio, Periodo) {
        $('#txtNoMateria').val("");
        $("#divNoMateria").hide();
        $('#txtNoAsesoria').val("");
        $("#divNoAsesoria").hide();

        var NoAsesorias = 0, NoMaterias = 0;

        var listPa = [];
        $(objAlumnoC.Referencias).each(function () {
            if (this.Anio.toString() == Anio.toString()
               && this.PeriodoId.toString() == Periodo.toString()
               && this.OfertaEducativaId.toString() == Oferta.toString()) {
                listPa.push(this);
            }
        });
        tblReferencias = $('#tblReferencias').dataTable({
            "aaData": listPa,
            "aoColumns": [
                { "mDataProp": "Concepto" },
                { "mDataProp": "Referencia" },
                { "mDataProp": "Fecha" },
                { "mDataProp": "Monto" },
            ],
            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
            "searching": false,
            "ordering": true,
            "info": false,
            "async": true,
            "bDestroy": true,
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
                var concepto = data.Concepto.toLowerCase();
                if (concepto.includes("adelanto")) {
                    NoMaterias += 1;
                } else if (concepto.includes("asesoria")) {
                    NoAsesorias += 1;
                }
                if (dataIndex % 2 == 0) {
                    $(row).addClass('bg-blue');
                }
                else { $(row).addClass('bg-green'); }
            },
        });
        if (NoMaterias > 0) {
            $('#txtNoMateria').val("El alumno tiene: " + NoMaterias + " Materia(s)");
            $("#divNoMateria").show();
        }
        if (NoAsesorias > 0) {
            $('#txtNoAsesoria').val("El alumno tiene: " + NoAsesorias + " Asesoria(s)");
            $("#divNoAsesoria").show();
        }
    }

    $('#tblAlumnos').on('click', 'a', function () {
        $('#Load').modal('show');
        $('#frmAlumnos').hide();
        var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
        AlumnoNum = rowadd.AlumnoId;
        
        EsNumero(rowadd.AlumnoId);
    });

    $('#txtAlumno').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscarAlumno').click();
        }
    });

    $('#txtNAsesoria').on('input', function () {
        if ($('#txtNAsesoria').val() == 0) { $('#txtSTAsesoria').val("$0.00" ); return false; }
        var uni = $('#txtPuAsesoria').val().replace("$", "");
        var nume=$('#txtNAsesoria').val();
        var tot = uni * nume;
        $('#txtSTAsesoria').val("$" + tot);
    });
    $('#txtNMateria').on('input', function () {
        if ($('#txtNMateria').val() == 0) { $('#txtSTMateria').val("$0.00"); return false; }
        var uni = $('#txtPuMateria').val().replace("$", "");
        var nume = $('#txtNMateria').val();
        var tot = uni * nume;
        $('#txtSTMateria').val("$" + tot);
    });
});