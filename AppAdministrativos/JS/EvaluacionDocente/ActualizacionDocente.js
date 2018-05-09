$(function() {
    var tblDocentes;
    var Funciones = {
        init() {

            $('#Load').modal('show');
            this.DocumentoTipo();
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
        DocenteEstudio: {},
        ListDocentes: [],
        TraerDocentes() {
            $.ajax({
                type: "GET",
                url: "Api/Docentes/TraerDocentes",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data !== null) {
                        if (tblDocentes !== undefined) { $('#tblDocentes').empty(); }
                        Funciones.TraerPeriodos();
                        Funciones.ListDocentes = data;
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
                type: "GET",
                url: "Api/Docentes/TraerPeriodos",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.length > 0) {

                        $(data).each(function () {
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

                        $('#slcPeriodoGrl').val(data[1].Anio + '' + data[1].PeriodoId);
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
                                var bot = '<button name="OFertaTipo" class="btn bg-blue">Agregar Formación</button>';
                                $(d.ListaEstudios).each(function () {
                                    if (this.Anio === Anio && this.PeriodoId === PeriodoId) {
                                        var col = 'bg-success';
                                        bot = '<a name="OFertaTipoVer" class="' + col + '">' + this.Anio + "-" + this.PeriodoId + "  " + this.EstudioDocente.Carrera + ' </a>'
                                    }
                                });
                                return bot;
                            } else {
                                var bot1;
                                bot1 = '<button name="OFertaTipo" class="btn bg-blue">Agregar Formación</button>';
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
           

            $('#frmFormacion')[0].reset();

            $('#txtComprobante').text('');
            var file = $('#FileComprobante');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComprobante span span').text('Seleccionar Archivo...');
            $('#linkDoc').attr("href", "#");          
            $('#linkDoc').text("");

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodo').val(parseInt(idselc));

            var anio_s = $("#slcPeriodoGrl :selected").data("anio"),
                periodo_s = $("#slcPeriodoGrl :selected").data("periodoid");

            $(DTODocente.ListaEstudios).each(function () {
                if (this.Anio === anio_s && this.PeriodoId === periodo_s) {
                    Funciones.DocenteEstudio = {
                        DocenteId: DTODocente.DocenteId,
                        EstudioId: this.EstudioId,
                        DocenteEstudioPeriodoId: this.DocenteEstudioPeriodoId
                    };

                    $('#slcOFertaTipo').val(this.EstudioDocente.OfertaEducativaTipoId);
                    $('#txtCarrera').val(this.EstudioDocente.Carrera);
                    $('#slcDocumentoTipo').val(this.EstudioDocente.Documento.DocumentoTipoId);
                    $('#txtInstitucion').val(this.EstudioDocente.Institucion);

                    if (this.EstudioDocente.Documento.DocumentoUrl !== null) {
                        $('#FileComprobante span span').text('Cambiar');                        
                        $('#linkDoc').attr("href", this.EstudioDocente.Documento.DocumentoUrl);
                        $('#linkDoc').text("Abrir Archivo");
                    }
                }
            });
            
            $('#ModalFormacion').modal('show');

        },
        MostrarCusos(DTODocente, esYmca) {
            $('#frmCurso')[0].reset();
            
            $('#slcPeriodoCurso').attr('disabled', true);
            $('#tiutuloCurso')[0].innerHTML = esYmca ? "Curso YMCA" : "Curso Externo";

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodoCurso').val(parseInt(idselc));

            var anio_s = $("#slcPeriodoGrl :selected").data("anio"),
                periodo_s = $("#slcPeriodoGrl :selected").data("periodoid");

            $('#txtCursoNombreI').addClass('edited');
            $('#txtTituloCurso').addClass('edited');
            $('#slcPeriodoCurso').addClass('edited');
            $('#slcDuracion').addClass('edited');
            $('#txtFechas').addClass('edited');

            $(DTODocente.CursosDocente).each(function () {
                if (this.EsCursoYMCA === esYmca && this.Anio === anio_s && this.PeriodoId === periodo_s) {
                    $('#txtCursoNombreI').val(this.Institucion);
                    $('#txtTituloCurso').val(this.Descripcion);
                    $('#slcDuracion').val(this.Duracion);
                    $('#txtFechas').val(this.FechaInicial + ' - ' + this.FechaFinal);
                    Funciones.DocenteEstudio = {
                        DocenteId: DTODocente.DocenteId,
                        DocenteCursoId: this.DocenteCursoId
                    };
                }
            });

            $('#ModalCurso').modal('show');

        },
        PopFormacionAcademica(DTODocente) {
            Funciones.DocenteEstudio = {
                DocenteId: DTODocente.DocenteId,
                DocenteCursoId: -1
            };
            $('#frmFormacion')[0].reset();
            $('#slcDocumentoTipo').attr('disabled', false);
            $('#slcOFertaTipo').attr('disabled', false);
            $('#slcPeriodo').attr('disabled', true);
            $('#frmFormacion input').removeAttr('readonly');

            var idselc = $('#slcPeriodoGrl').val();
            $('#slcPeriodo').val(parseInt(idselc));

            $("#FileComprobante").show();

            $('#txtComprobante').text('');
            var file = $('#FileComprobante');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComprobante span span').text('Seleccionar Archivo...');

            
            $('#ModalFormacion').modal('show');
        },
        PopCursoExterno(DTODocente) {
            Funciones.DocenteEstudio = {
                DocenteId: DTODocente.DocenteId,
                DocenteCursoId:-1
            };
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
            Funciones.DocenteEstudio = {
                DocenteId: DTODocente.DocenteId,
                DocenteCursoId: -1
            };
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
            $('#linkDoc').attr("href", "#");
            $('#linkDoc').text("");

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
            $('#linkDoc').attr("href", "#");
            $('#linkDoc').text("");
        },
        btnGuardarFormacionClick() {
            var $frm = $('#frmFormacion');
            if ($frm[0].checkValidity()) {
                if ($('#slcOFertaTipo').val() === "-1") {
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
                            $('#ModalFormacion').modal('hide');
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
                DocenteCursoId: Funciones.DocenteEstudio.DocenteCursoId,
                Institucion: $('#txtCursoNombreI').val(),
                Descripcion: $('#txtTituloCurso').val(),
                Anio: $("#slcPeriodoCurso :selected").data("anio"),
                PeriodoId: $("#slcPeriodoCurso :selected").data("periodoid"),
                Duracion: $('#slcDuracion').val(),
                FechaInicial: '',
                FechaFinal: '',
                EsCursoYMCA: Funciones.EsCursoYMCA,
                DocenteId: Funciones.DocenteEstudio.DocenteId,
                UsuarioId: $.cookie('userAdmin'),
            }
        },
        GuardarFormacionAcademica() {

            var objFomacion = {
                DocenteId: Funciones.DocenteEstudio.DocenteId,
                Anio: $("#slcPeriodo :selected").data("anio"),
                PeriodoId: $("#slcPeriodo :selected").data("periodoid"),
                EstudioId: Funciones.DocenteEstudio.EstudioId,
                DocenteEstudioPeriodoId: Funciones.DocenteEstudio.DocenteEstudioPeriodoId,
                EstudioDocente: {
                    Institucion: $('#txtInstitucion').val(),
                    OfertaEducativaTipoId: $('#slcOFertaTipo').val(),
                    Carrera: $('#txtCarrera').val(),
                    Documento: {
                        DocumentoTipoId: $('#slcDocumentoTipo').val()
                    },
                    UsuarioId: $.cookie('userAdmin'),
                }
            };

            objFomacion = JSON.stringify(objFomacion);

            var data = new FormData();
            var fileComprobante = $('#ArchivoComprobante'); // FileList object
            fileComprobante = fileComprobante[0].files[0];

            data.append("objDocente", objFomacion);

            if (fileComprobante !== undefined) {
                data.append("DocumentoComprobante", fileComprobante);
            }

            $.ajax({
                type: "POST",
                url: "Api/Docentes/GuardarFormacion",
                data: data,
                contentType: false,
                processData: false,
            })
                .done(function (data1) {

                    if (data1.EstudioId !== -1) {
                        alertify.alert("Guardado Correctamente.", function () {
                            Funciones.TraerDocentes();
                        });
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Fallo la subida del Archivo, intente nuevamente.", function () { $('#ModalFormacion').modal('show'); });
                    }
                })
                .fail(function () {
                    $('#Load').modal('hide');
                    alertify.alert("Fallo la subida del Archivo, intente nuevamente.", function () { $('#ModalFormacion').modal('show'); });
                });
        },
        DocumentoTipo() {
            $('#slcDocumentoTipo').empty();
            $.get("Api/Docentes/TipoDocumentos")
                .done(function (data) {
                    $(data).each(function () {
                        var opt = $(document.createElement('option'));

                        opt.val(this.DocumentoTipoId);
                        opt.text(this.Descripcion);

                        $('#slcDocumentoTipo').append(opt);
                    });
                })
                .fail(function (data) {
                    alertify.alert("Fallo");
                });
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