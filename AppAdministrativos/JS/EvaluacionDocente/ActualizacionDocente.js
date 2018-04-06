$(function() {
    var tblDocentes;
    var Funciones = {
        init() {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });
            $('#Load').modal('show');
            this.TraerTiposOfertas();
            this.TraerDocentes();
            this.starDate();
            $('#slcPeriodoGrl').on('change', this.slcPeriodoGrlChange);
            $('#tblDocentes').on('click', 'button', this.IdentificarBoton);
            $('#tblDocentes').on('click', 'a', this.IdentificarBoton);
            $('#btnCancelarFormacion').on('click', this.CerrarPopFormacion);
            $('#btnCancelarCurso').on('click', this.CerrarPopCurso);
            $('#ArchivoComprobante').bind('change', this.CambiarArchivo);
            $('#FileComprobante a').click(this.ClickArchivo);
            $('#btnGuardarFormacion').on('click', this.btnGuardarFormacionClick);
            $('#btnGuardarCurso').on('click', this.btnGuardarCursoExClick);
        },
        EsCursoYMCA: false,
        ListDocentes: [],
        TraerDocentes() {
            $.ajax({
                type: "GET",
                url: "Api/Docentes/TraerDocentes",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== null) {
                        if (tblDocentes !== undefined) { $('#tblDocentes').empty(); }
                        Funciones.TraerPeriodos();
                        Funciones.ListDocentes = data.d;
                    } else { $('#Load').modal('hide'); }
                },
                error: function () {
                    $('#Load').modal('hide');
                }
            });
        },
        TraerTiposOfertas() {
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/OfertaEducativaTipo",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== null) {
                        var opt1 = $(document.createElement('option'));
                        opt1.text('--Seleccionar--');
                        opt1.val(-1);
                        $("#slcOFertaTipo").append(opt1);

                        $(data.d).each(function () {
                            var opt = $(document.createElement('option'));
                            opt.text(this.Descripcion);
                            opt.val(this.OfertaEducativaTipoId);
                            $("#slcOFertaTipo").append(opt);
                        });
                    }
                }
            });
        },
        TraerPeriodos() {
            $("#slcPeriodo").empty();
            $('#slcPeriodoCurso').empty();
            $('#slcPeriodoGrl').empty();

            $.ajax({
                type: "POST",
                url: "Api/Docentes/TraerPeriodos",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d.length > 0) {

                        $(data.d).each(function () {
                            var opt = $(document.createElement('option'));
                            var opt2 = $(document.createElement('option'));
                            var opt3 = $(document.createElement('option'));

                            //console.log(this.Anio + '' + this.PeriodoId);

                            opt.text(this.Descripcion);
                            opt.val(this.Anio + '' + this.PeriodoId);
                            opt.data('anio', this.Anio);
                            opt.data('periodoid', this.PeriodoId);

                            opt2.text(this.Descripcion);
                            opt2.val(this.Anio + '' + this.PeriodoId);
                            opt2.data('anio', this.Anio);
                            opt2.data('periodoid', this.PeriodoId);

                            opt3.text(this.Descripcion);
                            opt3.val(this.Anio + '' + this.PeriodoId);
                            opt3.data('anio', this.Anio);
                            opt3.data('periodoid', this.PeriodoId);

                            $("#slcPeriodo").append(opt);
                            $('#slcPeriodoCurso').append(opt2);
                            $('#slcPeriodoGrl').append(opt3);

                        });

                        $('#slcPeriodoGrl').val(data.d[1].Anio + '' + data.d[1].PeriodoId);
                        $('#slcPeriodoGrl').change();
                    }


                }
            });
        },
        PintarTabla(tabla, Anio, PeriodoId) {

            tblDocentes = $('#tblDocentes').dataTable({
                "aaData": tabla,
                "aoColumns": [
                    { "mDataProp": "DocenteId" },
                    {
                        "mDataProp": function (columna) {
                            var nombre = columna.Nombre + " " + columna.Paterno + " " + columna.Materno;
                            return nombre;
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            if (d.ListaEstudios.length > 0) {
                                var bot = '';
                                $(d.ListaEstudios).each(function () {
                                    if (this.Anio === Anio && this.PeriodoId === PeriodoId) {
                                        var col = 'bg-success';
                                        var a = '<a name="OFertaTipoVer" class="' + col + '">' + this.Anio + "-" + this.PeriodoId + "  " + this.EstudioDocente.Carrera + ' </a>'
                                    }
                                    bot += a;
                                });
                                return bot;
                            } else {
                                var bot1;
                                bot1 = '<button name="OFertaTipo" class="btn bg-blue">Agregar Formación</button>'
                                return bot1;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            var bot = '<button name="CursoExterno" class="btn bg-blue">Agregar Curso</button>';
                            if (d.CursosDocente.length > 0) {
                                var tiene = false;
                                var col = '';
                                var anio, periodo, descripcion;
                                $(d.CursosDocente).each(function () {
                                    if (this.EsCursoYMCA === false && this.Anio === Anio && this.PeriodoId === PeriodoId) {
                                        anio = this.Anio;
                                        periodo = this.PeriodoId;
                                        descripcion = this.Descripcion;
                                        tiene = true;
                                    }
                                });
                                bot = tiene ? '<a name="CursoExternoVer" class="bg-warning">' + anio + "-" + periodo + "  " + descripcion + ' </a>'
                                    : bot;
                                return bot;
                            } else {
                                return bot;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            var bot;
                            bot = '<button name="CursoYMCA" class="btn bg-blue">Agregar Curso</button>';
                            if (d.CursosDocente.length > 0) {
                                var tiene = false;
                                var col = '';
                                var anio, periodo, descripcion;
                                $(d.CursosDocente).each(function () {
                                    if (this.EsCursoYMCA === true && this.Anio === Anio && this.PeriodoId === PeriodoId) {
                                        anio = this.Anio;
                                        periodo = this.PeriodoId;
                                        descripcion = this.Descripcion;
                                        tiene = true;
                                    }
                                });
                                bot = tiene ? '<a name="CursoYMCAVer" class="bg-warning">' + anio + "-" + periodo + "  " + descripcion + ' </a>'
                                    : bot;
                                return bot;
                            } else {
                                return bot;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            var puntos = 0;
                            if ((d.CursosDocente.length > 0) || (d.ListaEstudios.length > 0)) {
                                if (d.ListaEstudios.length > 0) { puntos = 3; }
                                else {
                                    if (d.CursosDocente.length > 0) {
                                        $(d.CursosDocente).each(function () {
                                            var puntos2 = (this.Anio === Anio && this.PeriodoId === PeriodoId) ? (this.Duracion === 20 ? 1.5 : (this.Duracion === 40 ? 3 : 0)) : 0;
                                            puntos = puntos2 < puntos ? puntos : puntos2;
                                        });
                                    }
                                }
                                return puntos;
                            } else {
                                var bot;
                                bot = '0';
                                return bot;
                            }
                        }
                    }
                ],
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": true,
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
                    "search": "Buscar Docente"
                }
            });
            var fil = $('#tblDocentes_filter label input');
            fil.removeClass('input-small').addClass('input-large');
            $('#Load').modal('hide');
        },
        IdentificarBoton() {

            var row = this.parentNode.parentNode;
            var DTODocente = tblDocentes.fnGetData($(this).closest('tr'));

            if ($(this)[0].name === "OFertaTipo") { Funciones.PopFormacionAcademica(DTODocente); }
            else if ($(this)[0].name === "CursoExterno") { Funciones.PopCursoExterno(DTODocente) }
            else if ($(this)[0].name === "CursoYMCA") { Funciones.PopCursoYMCA(DTODocente); }
            else if ($(this)[0].name === "OFertaTipoVer") { Funciones.MostrarFormacionAcademica(DTODocente); }
            else if ($(this)[0].name === "CursoYMCAVer") { Funciones.MostrarCusos(DTODocente, true); }
            else if ($(this)[0].name === "CursoExternoVer") { Funciones.MostrarCusos(DTODocente, false); }
        },
        starDate() {
            var formon = moment();
            formon.locale('es');
            formon.format('l');

            var fnow = new Date();
            var mes = fnow.getMonth() + 1;
            var year = fnow.getFullYear().toString();
            var fnowS = (fnow.getDate() > 9 ? fnow.getDate().toString() : "0" + fnow.getDate().toString()) + "/" +
                (mes > 9 ? mes.toString() : "0" + mes.toString()) + "/" + fnow.getFullYear().toString();

            fnow.setMonth(fnow.getMonth() - 2);
            mes = fnow.getMonth() + 1;

            var fAntS = '01/01/' + year;
            //(fnow.getDate() > 9 ? fnow.getDate().toString() : "0" + fnow.getDate().toString()) + "/" +
            //  (mes > 9 ? mes.toString() : "0" + mes.toString()) + "/" + fnow.getFullYear().toString();
            $('#txtFechas').val(fAntS + ' - ' + fnowS);
            $('#txtFechas .ui-datepicker-calendar').css("display", "none");
            $('#txtFechas').daterangepicker({
                language: 'es',
                autoApply: true,
                showDropdowns: true,
                alwaysShowCalendars: false,
                linkedCalendars: true,
                locale: {
                    format: "DD/MM/YYYY",
                    separator: " - ",
                    applyLabel: "Aplicar",
                    cancelLabel: "Cancelar",
                    fromLabel: "del",
                    toLabel: "al",
                    "customRangeLabel": "Custom",
                    daysOfWeek: [
                        "Do",
                        "Lu",
                        "Ma",
                        "Mi",
                        "Ju",
                        "Vi",
                        "Sa"
                    ],
                    monthNames: [
                        "Enero",
                        "Febrero",
                        "Marzo",
                        "Abril",
                        "Mayo",
                        "Junio",
                        "Julio",
                        "Agosto",
                        "Septiembre",
                        "Octubre",
                        "Noviembre",
                        "Diciembre"
                    ],
                    "firstDay": 1
                },
                startDate: fAntS,
                endDate: fnowS,
                disableEntry: true
            });

            var inps = $('.input-mini ');
            $(inps).each(function (k, a) {
                $(a).removeClass('input-mini').addClass('input');
                var icon = $(a).parent().find('i');
                $(icon).each(function (i, m) {
                    $(m).hide();
                });
            });
        },
        MostrarFormacionAcademica(DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;

            $('#frmFormacion')[0].reset();

            $('#txtComprobante').text('');
            var file = $('#FileComprobante');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComprobante span span').text('Seleccionar Archivo...');

            $('#frmFormacion input').attr('readonly', 'readonly');
            $('#slcOFertaTipo').attr('disabled', true);
            $('#slcPeriodo').attr('disabled', true);

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodo').val(parseInt(idselc));

            $('#txtInstitucion').val(DTODocente.Nombre + " " + DTODocente.Paterno + " " + DTODocente.Materno);
            $('#slcOFertaTipo').val(DTODocente.ListaEstudios[0].EstudioDocente.OfertaEducativaTipoId);
            $('#txtCarrera').val(DTODocente.ListaEstudios[0].EstudioDocente.Carrera);

            $('#chkCedula')[0].checked = DTODocente.ListaEstudios[0].EstudioDocente.Cedula;
            $('#chkCedula').attr('disabled', true);

            $('#chkTitulo')[0].checked = DTODocente.ListaEstudios[0].EstudioDocente.Titulo;
            $('#chkTitulo').attr('disabled', true);

            $("#FileComprobante").hide();

            $('input').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });
            $('#btnGuardarFormacion').prop("disabled", true);
            $('#ModalFormacion').modal('show');

        },
        MostrarCusos(DTODocente, esYmca) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            $('#frmCurso')[0].reset();
            $('#frmCurso input').attr('readonly', 'readonly');
            $('#frmCurso input').attr('disabled', true);

            $('#slcDuracion').attr('disabled', true);
            $('#slcPeriodoCurso').attr('disabled', true);
            $('#tiutuloCurso')[0].innerHTML = esYmca ? "Curso YMCA" : "Curso Externo";

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodoCurso').val(parseInt(idselc));

            $('#txtCursoNombreI').addClass('edited');
            $('#txtTituloCurso').addClass('edited');
            $('#slcPeriodoCurso').addClass('edited');
            $('#slcDuracion').addClass('edited');
            $('#txtFechas').addClass('edited');

            $(DTODocente.CursosDocente).each(function () {
                if (this.EsCursoYMCA === esYmca) {
                    $('#txtCursoNombreI').val(this.Institucion);
                    $('#txtTituloCurso').val(this.Descripcion);
                    $('#slcDuracion').val(this.Duracion);
                    $('#txtFechas').val(this.FechaInicial + ' - ' + this.FechaFinal);
                }
            });
            $('#btnGuardarCurso').prop("disabled", true);


            $('#ModalCurso').modal('show');

        },
        PopFormacionAcademica(DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            $('#frmFormacion')[0].reset();
            $('#chkTitulo').attr('disabled', false);
            $('#chkCedula').attr('disabled', false);
            $('#slcOFertaTipo').attr('disabled', false);
            $('#slcPeriodo').attr('disabled', true);
            $('#frmFormacion input').removeAttr('readonly');

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodo').val(parseInt(idselc));

            $("#FileComprobante").show();

            $('input').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });
            $('#txtComprobante').text('');
            var file = $('#FileComprobante');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComprobante span span').text('Seleccionar Archivo...');

            $('#btnGuardarFormacion').prop("disabled", false);
            $('#ModalFormacion').modal('show');
        },
        PopCursoExterno(DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            Funciones.EsCursoYMCA = false;
            $('#frmCurso')[0].reset();
            $('#frmCurso input').removeAttr('readonly');
            $('#frmCurso input').attr('disabled', false);
            $('#slcDuracion').attr('disabled', false);

            $('#txtCursoNombreI').removeClass('edited');
            $('#slcDuracion').removeClass('edited');
            $('#txtTituloCurso').removeClass('edited');


            $('#tiutuloCurso')[0].innerHTML = "Curso Externo";

            $('#slcPeriodoCurso').attr('disabled', true);
            $('#slcPeriodoCurso').addClass('edited');

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodoCurso').val(parseInt(idselc));


            $('#txtFechas').addClass('edited');

            $('#btnGuardarCurso').prop("disabled", false);
            $('#ModalCurso').modal('show');
        },
        PopCursoYMCA(DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            Funciones.EsCursoYMCA = true;
            $('#frmCurso')[0].reset();
            $('#frmCurso input').removeAttr('readonly');
            $('#frmCurso input').attr('disabled', false);
            $('#slcPeriodoCurso').attr('disabled', true);

            $('#tiutuloCurso')[0].innerHTML = "Curso YMCA";
            $('#txtCursoNombreI').val("Unidad Ejercito");
            $('#txtCursoNombreI').addClass('edited');

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodoCurso').val(parseInt(idselc));

            $('#txtFechas').addClass('edited');

            $('#btnGuardarCurso').prop("disabled", false);
            $('#ModalCurso').modal('show');
        },
        CerrarPopFormacion() {
            $('#ModalFormacion').modal('hide');
        },
        CerrarPopCurso() {
            $('#ModalCurso').modal('hide');
        },
        CambiarArchivo() {
            var file = $('#FileComprobante');
            var tex = $('#txtComprobante').html();
            if (this.files.length > 0) {
                $('#txtComprobante').text(this.files[0].name);
                file.addClass('fileinput-exists').removeClass('fileinput-new');
                $('#FileComprobante span span').text('Cambiar');
            }
            else {
                $('#txtComprobante').text('');
                file.removeClass('fileinput-exists').addClass('fileinput-new');
                $('#FileComprobante span span').text('Seleccionar Archivo...');
            }
        },
        ClickArchivo() {
            var file = $('#FileComprobante');
            $('#txtComprobante').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#ArchivoComprobante')[0].value = null;
            $('#FileComprobante span span').text('Seleccionar Archivo...');
        },
        btnGuardarFormacionClick() {
            var $frm = $('#frmFormacion');
            if ($frm[0].checkValidity()) {
                if ($('#slcOFertaTipo').val() === "-1" || ($('#chkCedula')[0].checked === false && $('#chkTitulo')[0].checked === false)) {
                    alertify.alert("Favor de Seleccionar una opción.");
                    $('#slcOFertaTipo').focus();
                    $('#slcOFertaTipo').select();
                } else {
                    $('#Load').modal('show');
                    $('#ModalFormacion').modal('hide');
                    Funciones.GuardarFormacionAcademica();
                }
            }
        },
        btnGuardarCursoExClick() {
            var $frm = $('#frmCurso');
            if ($frm[0].checkValidity()) {
                $('#ModalCurso').modal('hide');
                $('#Load').modal('show');
                var Rango = $('#txtFechas').val();
                var objCurso
                objCurso = Funciones.DatosCurso();
                objCurso.FechaInicial = Rango.substring(0, 10);
                objCurso.FechaFinal = Rango.substring(13, 23);

                objCurso = JSON.stringify(objCurso);
                Funciones.GuardarCurso(objCurso);
            }
        },
        GuardarCurso(objCurso) {
            $.ajax({
                type: "POST",
                url: "Api/Docentes/GuardarCurso",
                data: objCurso,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data !== -1) {
                        $('#Load').modal('hide');
                        alertify.alert("Guardado Correctamente.", function () {
                            Funciones.TraerDocentes();
                        });
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Fallo el guardado del docente, Intente nuevamente");
                        $('#ModalCurso').modal('show');
                    }
                }
            });
        },
        DatosCurso() {
            return {
                NombreInstitucion: $('#txtCursoNombreI').val(),
                TituloCurso: $('#txtTituloCurso').val(),
                Anio: $("#slcPeriodoCurso :selected").data("anio"),
                PeriodoId: $("#slcPeriodoCurso :selected").data("periodoid"),
                Duracion: $('#slcDuracion').val(),
                FechaInicial: '',
                FechaFinal: '',
                EsCursoYmca: Funciones.EsCursoYMCA,
                DocenteId: Funciones.DocenteSeleccionado,
                UsuarioId: $.cookie('userAdmin'),
            }
        },
        GuardarFormacionAcademica() {
            var objFomacion = {
                DocenteId: Funciones.DocenteSeleccionado,
                Institucion: $('#txtInstitucion').val(),
                OFertaTipo: $('#slcOFertaTipo').val(),
                Carrera: $('#txtCarrera').val(),
                Cedula: $('#chkCedula')[0].checked,
                Titulo: $('#chkTitulo')[0].checked,
                UsuarioId: $.cookie('userAdmin'),
                Anio: $("#slcPeriodo :selected").data("anio"),
                PeriodoId: $("#slcPeriodo :selected").data("periodoid"),
            };
            objFomacion = JSON.stringify(objFomacion);
            $.ajax({
                type: "POST",
                url: "WS/Docentes.asmx/GuardarFormacion",
                data: objFomacion,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== -1) {
                        Funciones.GuardarFormacionAcademicaDocumento(data.d, $('#chkCedula')[0].checked ? 1 : $('#chkTitulo')[0].checked ? 2 : 0);
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Fallo el guardado del docente, Intente nuevamente");
                        $('#ModalFormacion').modal('show');
                    }
                }
            });
        },
        GuardarFormacionAcademicaDocumento(EstudioId, Tipo) {
            var data = new FormData();
            var fileComprobante = $('#ArchivoComprobante'); // FileList object
            fileComprobante = fileComprobante[0].files[0];
            if (fileComprobante !== undefined) {
                data.append("DocumentoComprobante", fileComprobante);
                data.append("EstudioId", EstudioId);
                data.append("TipoDocumento", Tipo);


                $.ajax({
                    type: "POST",
                    url: "WS/Docentes.asmx/GuardarFormacionDocumento",
                    data: data,
                    contentType: false,
                    processData: false,
                    success: function (data1) {

                        var $xml = $(data1);
                        var $bool = $xml.find("boolean");

                        if ($bool[0].textContent === 'true') {
                            alertify.alert("Guardado Correctamente.", function () {
                                Funciones.TraerDocentes();
                            });
                        } else {
                            $('#Load').modal('hide');
                            alertify.alert("Fallo la subida del Archivo, intente nuevamente.", function () { $('#ModalFormacion').modal('show'); });
                        }
                    }
                });
            } else {
                $('#Load').modal('hide');
                alertify.alert("Guardado Correctamente.", function () {
                    Funciones.TraerDocentes();
                });
            }
        },
        DocenteSeleccionado: 0,
        slcPeriodoGrlChange() {

            //console.log($($(this)[0]).data('anio'));
            //console.log($($(this)[0]).data('periodoid'));
            Funciones.PintarTabla(Funciones.ListDocentes,
                $("#slcPeriodoGrl :selected").data("anio"),
                $("#slcPeriodoGrl :selected").data("periodoid"));
        }
    };
    
    Funciones.init();

    
});