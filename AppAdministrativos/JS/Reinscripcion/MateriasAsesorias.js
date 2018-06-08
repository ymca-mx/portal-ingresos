$(function init() {
    var tblAlumnos;
    var AlumnoNum;
    var tblReferencias;
    var objAlumnoC;
    var estado = "";
    var hayCuatrimestre = false;
    var Planel;

    var Eventos = {
        btnBuscarAlumnoClick: function () {
            $('#btnBuscarAlumno').blur();
            $('#divCuatri').hide();
            $('#btnGuardar').prop("disabled", true);
            objAlumnoC = undefined;
            AlumnoNum = $('#txtAlumno').val();
            $("#lbCuatrimestre").text("");
            $('#frmAlumnos').hide();
            $('#lblEstatus').text('');
            $('#submit_form').trigger('reset');
            if (tblAlumnos !== undefined) {
                tblAlumnos.fnClearTable();
            } if (tblReferencias !== undefined) {
                tblReferencias.fnClearTable();
            }

            if (!isNaN(AlumnoNum) && AlumnoNum.length > 0) {
                Funciones.EsNumero(AlumnoNum);
            } else if (AlumnoNum.length > 0) {
                Funciones.EsString(AlumnoNum);
            }
        },
        txtNAsesoriaClick: function () {
            this.select();
        },
        txtNMateriaClick: function () {
            this.select();
        },
        btnGuardarMaestriaClick: function () {
            $('#btnGuardarMaestria').blur();
            $('#Load').modal('show');
            var objGM = {
                AlumnoId: objAlumnoC.AlumnoId,
                EspecialidadId: $("#slcOfertas").val(),
                MaestriaId: $("#txtMaestria").data('maestriaid'),
                PeriodoId: $('#slcPeriodos option:selected').data("periodoid"),
                Anio: $('#slcPeriodos option:selected').data("anio"),
                UsuarioId: $.cookie('userAdmin')
            };
            objGM = JSON.stringify(objGM);

            $.ajax({
                type: "POST",
                url: "WS/Reinscripcion.asmx/GenerarMaestria",
                data: objGM,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#Load').modal('hide');
                    if (data.d === "Guardado") {
                        $('#ModalMaestria').modal('hide');
                        alertify.alert("Se guardo correctaente.")
                    } else {
                        alertify.alert("Error, no se pudo inscribir a la Maestria intente nuevamente.");
                    }
                }
            });
        },
        btnMaestriaClick: function () {
            $('#Load').modal('show');
            Funciones.TraerMaestrias();
            $('#ModalMaestria').modal('show');
        },
        btnCancelarClick: function () {
            $('#ModalMaestria').modal('hide');
        },
        slcOfertasChange: function () {
            $('#divMaestria').hide();
            var op = $("#slcOfertas").val();
            $("#lbCuatrimestre").text("");

            if (objAlumnoC.lstOfertas.length !== 0) {
                $(objAlumnoC.lstOfertas).each(function () {
                    if (String(this.OfertaEducativaId) === op) {
                        if (this.OfertaEducativaTipoId !== 1) { $('#trAsesoria').hide(); } else { $('#trAsesoria').show(); }
                        hayCuatrimestre = this.Cuatrimestre !== 0 ? true : false;
                        Funciones.llenarCuatrimestres(this.OfertaEducativaTipoId);
                        var c = this.Cuatrimestre !== 0 ? "Cuatrimestre anterior:  " + this.Cuatrimestre : "";
                        if (this.AplicaMaestria) {
                            Planel = this.SucursalId;
                            $('#divMaestria').show();
                        }
                        $("#lbCuatrimestre").text(c);
                    }
                });

                $('#txtPuAsesoria').val("$0.00");
                $('#txtPuMateria').val("$0.00");

                if (op.toString() === "-1") {

                    $('#txtSTAsesoria').val("$0.00");
                    $('#txtSTMateria').val("$0.00");
                    $('#txtNAsesoria').val(0);
                    $('#txtNMateria').val(0);
                    $("#slcPeriodos").val("-1");
                    var spam = $('#rdbSi')[0].parentElement;
                    $(spam).removeClass('checked');
                    var spam1 = $('#rdbNo')[0].parentElement;
                    $(spam1).removeClass('checked');
                    if (tblReferencias !== undefined) {
                        tblReferencias.fnClearTable();
                    }
                    $('#btnGuardar').prop("disabled", true);
                    $("#divCuatri").hide();
                    $("input:radio")[0].checked = false;
                    return false;
                }

                $("#divCuatri").hide();
                $("input:radio[name=rdbRegular]")[0].checked = false;
                Funciones.llenarPeriodos();
            }
        },
        slcPeriodosChange: function () {
            var op = $("#slcPeriodos").val();
            if (op === "-1") {
                $('#txtPuAsesoria').val("$0.00");
                $('#txtPuMateria').val("$0.00");
                $('#txtSTAsesoria').val("$0.00");
                $('#txtSTMateria').val("$0.00");
                $('#txtNAsesoria').val(0);
                $('#txtNMateria').val(0);
                var spam = $('#rdbSi')[0].parentElement;
                $(spam).removeClass('checked');
                var spam1 = $('#rdbNo')[0].parentElement;
                $(spam1).removeClass('checked');
                $("input:radio[name=rdbInscripcion]")[0].checked = false;
                $('#btnGuardar').prop("disabled", true);
                if (tblReferencias !== undefined) {
                    tblReferencias.fnClearTable();
                }
                return false;
            }
            Funciones.CarcarCuota();
        },
        rdbSiClick: function () {
            if (estado.length > 0) { return false; }
            var op = $("#slcPeriodos").val();
            if (op === "-1" || op === null) { return false; }
            var op1 = $("#slcOfertas").val();
            if (op1 === "-1" || op1 === null) { return false; }
            var chk = this;
            chk = chk.checked;
            if (chk) {
                $('#btnGuardar').prop("disabled", false);
            }
        },
        rdbNoClick: function () {
            if (estado.length > 0) { return false; }
            var op = $("#slcPeriodos").val();
            if (op === "-1" || op === null) { return false; }
            var op1 = $("#slcOfertas").val();
            if (op1 === "-1" || op1 === null) { return false; }
            var chk = this;
            chk = chk.checked;
            if (chk) {
                $('#btnGuardar').prop("disabled", false);
            }
        },
        rdbregularSiClick: function () {
            if (estado.length > 0) { return false; }

            if ($("#slcPeriodos").val() === "-1" || $("#slcPeriodos").val() === null) {
                return false;
            }
            if ($("#slcOfertas").val() === "-1" || $("#slcOfertas").val() === null) {
                return false;
            }

            if (hayCuatrimestre === true) {
                $('#divCuatri').hide();
            } else {
                $('#divCuatri').show();
            }
        },
        rdbregularNoClick: function () {

            if (estado.length > 0) { return false; }

            if ($("#slcPeriodos").val() === "-1" || $("#slcPeriodos").val() === null) {
                return false;
            }
            if ($("#slcOfertas").val() === "-1" || $("#slcOfertas").val() === null) {
                return false;
            }

            $('#divCuatri').show();
        },
        btnGuardarClick: function () {
            $('#btnGuardar').blur();
            var res = $('input:radio[name=rdbRegular]:checked').val();

            if (res === undefined) {
                alertify.alert('Falta indicar si el alumno es regular.');
                return false;
            } else {
                if ($("#divCuatri").is(":visible") && $("#slcCuatrimeste").val() === "-1") {
                    alertify.alert('Falta indicar el cuatrimestre del alumno.');
                    return false;
                }
            }

            $('#Load').modal('show');
            //Adeudos();

            Funciones.Guardar();
        },
        tblAlumnosClicka: function () {
            $('#Load').modal('show');
            $('#frmAlumnos').hide();
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
            AlumnoNum = rowadd.AlumnoId;

            Funciones.EsNumero(rowadd.AlumnoId);
        },
        txtAlumnokeydown: function (e) {
            if (e.which === 13) {
                $('#btnBuscarAlumno').click();
            }
        },
        txtNAsesoriainput: function () {
            if ($('#txtNAsesoria').val() === 0) { $('#txtSTAsesoria').val("$0.00"); return false; }
            var uni = $('#txtPuAsesoria').val().replace("$", "");
            var nume = $('#txtNAsesoria').val();
            var tot = uni * nume;
            $('#txtSTAsesoria').val("$" + tot);
        },
        txtNMateriainput: function () {
            if ($('#txtNMateria').val() === 0) { $('#txtSTMateria').val("$0.00"); return false; }
            var uni = $('#txtPuMateria').val().replace("$", "");
            var nume = $('#txtNMateria').val();
            var tot = uni * nume;
            $('#txtSTMateria').val("$" + tot);
        }
    };

    var Funciones = {
        EsNumero: function (Alumno) {
            $('#Load').modal('show');
            $.ajax({
                type: "POST",
                url: "WS/Reinscripcion.asmx/AlumnoReinscripcion",
                data: "{AlumnoId:'" + Alumno + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== null) {
                        $('#lblNombre').text(data.d.Nombre);
                        objAlumnoC = data.d;
                        Funciones.llenarOfertas();
                    } else { $('#Load').modal('hide'); }
                }
            });
        },
        llenarOfertas: function () {
            $("#slcOfertas").empty();
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcOfertas").append(optionP);
            if (objAlumnoC.lstOfertas.length === 0) { $('#Load').modal('hide'); return false; }
            if (objAlumnoC.lstOfertas.length > 1) {
                $(objAlumnoC.lstOfertas).each(function () {
                    var option1 = $(document.createElement('option'));
                    option1.text(this.Descripcion);
                    option1.val(this.OfertaEducativaId);
                    Planel = this.SucursalId;
                    $("#slcOfertas").append(option1);
                });
            } else {
                var option1 = $(document.createElement('option'));
                option1.text(objAlumnoC.lstOfertas[0].Descripcion);
                option1.val(objAlumnoC.lstOfertas[0].OfertaEducativaId);
                Planel = objAlumnoC.lstOfertas[0].SucursalId;
                $("#slcOfertas").append(option1);
                $("#slcOfertas").val(objAlumnoC.lstOfertas[0].OfertaEducativaId);
                hayCuatrimestre = objAlumnoC.lstOfertas[0].Cuatrimestre !== 0 ? true : false;
            }
            $("#slcOfertas").change();

            Funciones.llenarPeriodos();
        },
        llenarPeriodos: function () {

            $("#slcPeriodos").empty();
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcPeriodos").append(optionP);
            if (objAlumnoC.lstPeriodos.length === 0) { return false; }
            if (objAlumnoC.lstPeriodos.length > 1) {
                $(objAlumnoC.lstPeriodos).each(function () {
                    var option1 = $(document.createElement('option'));
                    option1.text(this.Descripcion);
                    option1.val(this.PeriodoId + " " + this.Anio);
                    option1.attr("data-Anio", this.Anio);
                    option1.attr("data-PeriodoId", this.PeriodoId);
                    $("#slcPeriodos").append(option1);
                });
            } else {
                var option1 = $(document.createElement('option'));
                option1.text(objAlumnoC.lstPeriodos[0].Descripcion);
                option1.val(objAlumnoC.lstPeriodos[0].PeriodoId + " " + objAlumnoC.lstPeriodos[0].Anio);
                option1.attr("data-Anio", objAlumnoC.lstPeriodos[0].Anio);
                option1.attr("data-PeriodoId", objAlumnoC.lstPeriodos[0].PeriodoId);
                $("#slcPeriodos").append(option1);
            }

            $("#slcPeriodos").val(objAlumnoC.lstPeriodos[0].PeriodoId + " " + objAlumnoC.lstPeriodos[0].Anio);

            $("#slcPeriodos").change();
            $(window).scrollTop($('#slcPeriodos').offset().top);
            $('#Load').modal('hide');

        },
        llenarCuatrimestres: function (OfertaEducativaTipoId) {
            $("#slcCuatrimeste").empty();
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcCuatrimeste").append(optionP);
            var i = 1;
            var f = OfertaEducativaTipoId === 1 ? 9 : OfertaEducativaTipoId === 2 ? 3 : OfertaEducativaTipoId === 3 ? 5 : 0;

            while (i <= f) {
                var optionP2 = $(document.createElement('option'));
                optionP2.text(i);
                optionP2.val(i);
                $("#slcCuatrimeste").append(optionP2);
                i++;
            }

        },
        TraerMaestrias: function () {
            var obj = {
                'OfertaEducativaId': $('#slcOfertas').val(),
            };
            obj = JSON.stringify(obj);
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/TraerMaestria",
                data: obj,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#Load').modal('hide');

                    if (data.d === null) { return false; }
                    $("#txtMaestria").val(data.d.Descripcion);
                    $("#txtMaestria").data('maestriaid', data.d.OfertaEducativaId);

                },
                error: function () {
                    $('#Load').modal('hide');
                }
            })
        },
        CarcarCuota: function () {
            var op = $("#slcPeriodos").val();
            if (op === "-1") { return false; }
            var op1 = $("#slcOfertas").val();
            if (op1 === "-1") { return false; }
            op = op.split(" ");
            var anio = op[1];
            var periodo = op[0];

            $(objAlumnoC.Cuotas).each(function () {
                if (this.Anio.toString() === anio.toString()
                    && this.PeriodoId.toString() === periodo.toString()
                    && this.OfertaEducativaId.toString() === op1.toString()) {
                    if (this.PagoConceptoId === 15) {
                        $('#txtPuAsesoria').val("$" + this.Monto);
                    } else {
                        $('#txtPuMateria').val("$" + this.Monto);
                    }
                }

            });
            Funciones.CargarEstatus(op1, anio, periodo);
            Funciones.TablaReferencias(op1, anio, periodo);
        },
        CargarEstatus: function (oferta, anio, periodo) {
            estado = "";
            $('#lblEstatus').text('');
            $(objAlumnoC.EstatusAl).each(function () {
                if (this.Anio.toString() === anio.toString()
                    && this.Periodo.toString() === periodo.toString()
                    && this.OfertaEducativaId.toString() === oferta.toString()) {
                    estado = this.Estado;
                }
            });
            $('#lblEstatus').text(estado);
        },
        Adeudos: function () {
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/ConsultarAdeudo",
                data: '{AlumnoId:' + AlumnoNum + '}',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d === "Debe") {
                        alertify.alert('Tiene adeudos, favor de pasar a La Coordinación Administrativa para resolver su situación financiera.');
                        $('#Load').modal('hide');
                    } else {
                        Funciones.Guardar();
                    }
                }
            });
        },
        Guardar: function () {
            var op = $("#slcPeriodos").val();
            if (op === "-1") { return false; }
            var op1 = $("#slcOfertas").val();
            if (op1 === "-1") { return false; }
            op = op.split(" ");
            var anio = op[1];
            var periodo = op[0];
            var usuario = $.cookie('userAdmin');
            var nAse = $('#txtNAsesoria').val();
            var nMat = $('#txtNMateria').val();
            var completa = ($('#rdbSi')[0].checked);
            var comentario = $('#txtComentario').val();
            var esRegular = ($('#rdbregularSi')[0].checked);
            var Cuatrimestre = ($("#divCuatri").is(":visible") ? $("#slcCuatrimeste").val() : null);

            if (nAse === "0" && nMat === "0" && completa === false) {
                $('#Load').modal('hide');
                alertify.alert("Favor de especificar el numero de adelanto de materia o asesoria especial.")
                return false;
            }
            //AlumnoNum
            var Datos = '{AlumnoId:"' + AlumnoNum + '",anio:"' + anio + '",periodo:"'
                + periodo + '",oferta:"' + op1 + '",NMaterias:"' + nMat + '",NAsesorias:"'
                + nAse + '",Completa:"' + completa + '",usuario:"' + usuario + '",Comentario:"'
                + comentario + '",esRegular:"' + esRegular + '",Cuatrimestre:"' + Cuatrimestre + '"}';

            $.ajax({
                url: 'WS/Reinscripcion.asmx/Generar',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: Datos,
                dataType: 'json',
                success: function (data) {
                    if (data.d === true) {
                        alertify.alert("Datos Generados");
                        $('#txtAlumno').val(AlumnoNum);
                        Eventos.btnBuscarAlumnoClick();
                    } else { alertify.alert("Error Al Guardar"); $('#Load').modal('hide'); }
                }
            });
        },
        EsString: function (Alumno) {
            $('#Load').modal('show');
            $('#frmAlumnos').show();
            $.ajax({
                url: 'WS/Alumno.asmx/BuscarAlumnoString',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{Filtro:"' + Alumno + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data !== null) {
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
        },
        TablaReferencias: function (Oferta, Anio, Periodo) {
            $('#txtNoMateria').val("");
            $("#divNoMateria").hide();
            $('#txtNoAsesoria').val("");
            $("#divNoAsesoria").hide();

            var NoAsesorias = 0, NoMaterias = 0;

            var listPa = [];
            $(objAlumnoC.Referencias).each(function () {
                if (this.Anio.toString() === Anio.toString()
                    && this.PeriodoId.toString() === Periodo.toString()
                    && this.OfertaEducativaId.toString() === Oferta.toString()) {
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
                    if (dataIndex % 2 === 0) {
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
    }

    $('#btnBuscarAlumno').click(Eventos.btnBuscarAlumnoClick);
    $('#txtNAsesoria').on('click', Eventos.txtNAsesoriaClick);
    $('#txtNMateria').on('click', Eventos.txtNMateriaClick);
    $('#btnGuardarMaestria').on('click', Eventos.btnGuardarMaestriaClick);
    $('#btnMaestria').on('click', Eventos.btnMaestriaClick);
    $('#btnCancelar').on('click', Eventos.btnCancelarClick);
    $("#slcOfertas").change(Eventos.slcOfertasChange);
    $("#slcPeriodos").change(Eventos.slcPeriodosChange);
    $('#rdbSi').click(Eventos.rdbSiClick);
    $('#rdbNo').click(Eventos.rdbNoClick);
    $('#rdbregularSi').click(Eventos.rdbregularSiClick);
    $('#rdbregularNo').click(Eventos.rdbregularNoClick);
    $('#btnGuardar').click(Eventos.btnGuardarClick);
    $('#tblAlumnos').on('click', 'a', Eventos.tblAlumnosClicka);
    $('#txtAlumno').on('keydown', Eventos.txtAlumnokeydown);
    $('#txtNAsesoria').on('input', Eventos.txtNAsesoriainput);
    $('#txtNMateria').on('input', Eventos.txtNMateriainput);
});