$(function init() {
    var tblDocentes;
    var Funciones = {
        init: function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });
            $('#Load').modal('show');
            Funciones.TraerTiposOfertas();
            Funciones.TraerDocentes();
            Funciones.TraerPeriodos();
            Funciones.starDate();
        },
        EsCursoYMCA: false,
        PeriodoActual:'',
        TraerDocentes: function () {
            $.ajax({
                type: "POST",
                url: "WS/Docentes.asmx/TraerDocentes",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== null) {
                        if (tblDocentes !== undefined)
                        { $('#tblDocentes').empty(); }
                        Funciones.PintarTabla(data.d);
                    } else { $('#Load').modal('hide'); }
                },
                error: function () {
                    $('#Load').modal('hide');
                }
            });
        },
        TraerTiposOfertas: function () {
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
        TraerPeriodos: function () {
            $.ajax({
                type: "POST",
                url: "WS/Docentes.asmx/TraerPeriodos",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d.length > 0) {

                        $(data.d).each(function () {
                            var opt = $(document.createElement('option'));
                            var opt2 = $(document.createElement('option'));

                            //console.log(this.Anio + '' + this.PeriodoId);

                            opt.text(this.Descripcion);
                            opt.val(this.Anio + '' + this.PeriodoId);
                            opt.data('anio', this.Anio);
                            opt.data('periodoid', this.PeriodoId);

                            opt2.text(this.Descripcion);
                            opt2.val(this.Anio + '' + this.PeriodoId);
                            opt2.data('anio', this.Anio);
                            opt2.data('periodoid', this.PeriodoId);

                            $("#slcPeriodo").append(opt);
                            $('#slcPeriodoCurso').append(opt2);
                        });                    
                        Funciones.PeriodoActual = data.d[1].Anio + '' + data.d[1].PeriodoId;
                    }
                    
                    
                }
            });
        },
        PintarTabla: function (tabla) {
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
                                    var col = this.TieneVbo ? 'bg-success' : 'bg-red';
                                    var a = '<a name="OFertaTipoVer" class="' + col + '">' + this.Anio + "-" + this.PeriodoId + "  " + this.EstudioDocente.Carrera + ' </a>'
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
                                    if (this.EsCursoYMCA === false) {
                                        anio = this.Anio;
                                        periodo = this.PeriodoId;
                                        descripcion = this.Descripcion;
                                        tiene = true;
                                        col = this.VoBo ? 'bg-success' : 'bg-red';
                                    }
                                });
                                bot = tiene ? '<a name="CursoExternoVer" class="' + col + '">' + anio + "-" + periodo + "  " + descripcion + ' </a>'
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
                                    if (this.EsCursoYMCA === true) {
                                        anio = this.Anio;
                                        periodo = this.PeriodoId;
                                        descripcion = this.Descripcion;
                                        tiene = true;
                                        col = this.VoBo ? 'bg-success' : 'bg-red';
                                    }
                                });
                                bot = tiene ? '<a name="CursoYMCAVer" class="' + col + '">'+ anio + "-" + periodo + "  " + descripcion + ' </a>'
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
                                            var puntos2 = (this.Duracion === 20 ? 1.5 : (this.Duracion === 40 ? 3 : 0));
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
        IdentificarBoton: function () {            
            var row = this.parentNode.parentNode;
            var DTODocente = tblDocentes.fnGetData($(this).closest('tr'));
            if ($(this)[0].name === "OFertaTipo") { Funciones.PopFormacionAcademica(DTODocente); }
            else if ($(this)[0].name === "CursoExterno") { Funciones.PopCursoExterno(DTODocente) }
            else if ($(this)[0].name === "CursoYMCA") { Funciones.PopCursoYMCA(DTODocente); }
            else if ($(this)[0].name === "OFertaTipoVer") { Funciones.MostrarFormacionAcademica(DTODocente); }
            else if ($(this)[0].name === "CursoYMCAVer") { Funciones.MostrarCusos(DTODocente, true); }
            else if ($(this)[0].name === "CursoExternoVer") { Funciones.MostrarCusos(DTODocente, false); }
        },
        starDate: function () {
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
        MostrarFormacionAcademica: function (DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;

            $('#frmFormacion')[0].reset();

            $('#txtComprobante').text('');
            var file = $('#FileComprobante');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComprobante span span').text('Seleccionar Archivo...');

            $('#frmFormacion input').attr('readonly', 'readonly');
            $('#slcOFertaTipo').attr('disabled', true);

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
        MostrarCusos: function (DTODocente, esYmca) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            $('#frmCurso')[0].reset();
            $('#frmCurso input').attr('readonly', 'readonly');
            $('#frmCurso input').attr('disabled', true);
            
            $('#slcDuracion').attr('disabled', true);
            $('#tiutuloCurso')[0].innerHTML = esYmca ? "Curso YMCA" : "Curso Externo";

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
        PopFormacionAcademica: function (DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            $('#frmFormacion')[0].reset();
            $('#chkTitulo').attr('disabled', false);
            $('#chkCedula').attr('disabled', false);
            $('#slcOFertaTipo').attr('disabled', false);
            $('#frmFormacion input').removeAttr('readonly');
            $('#slcPeriodo').val(Funciones.PeriodoActual);
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
        PopCursoExterno: function (DTODocente) {
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
            $('#slcPeriodoCurso').addClass('edited');
            
            $('#slcPeriodoCurso').addClass('edited');
            $('#slcPeriodoCurso').val(Funciones.PeriodoActual);

            $('#txtFechas').addClass('edited');

            $('#btnGuardarCurso').prop("disabled", false);
            $('#ModalCurso').modal('show');
        },
        PopCursoYMCA: function (DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            Funciones.EsCursoYMCA = true;
            $('#frmCurso')[0].reset();
            $('#frmCurso input').removeAttr('readonly');
            $('#frmCurso input').attr('disabled', false);
            $('#slcDuracion').attr('disabled', false);
            
            $('#tiutuloCurso')[0].innerHTML = "Curso YMCA";
            $('#txtCursoNombreI').val("Unidad Ejercito");
            $('#txtCursoNombreI').addClass('edited');
            $('#slcPeriodoCurso').addClass('edited');
            $('#slcPeriodoCurso').val(Funciones.PeriodoActual);
            
            $('#txtFechas').addClass('edited');

            $('#btnGuardarCurso').prop("disabled", false);
            $('#ModalCurso').modal('show');
        },
        CerrarPopFormacion: function () {
            $('#ModalFormacion').modal('hide');
        },
        CerrarPopCurso: function () {
            $('#ModalCurso').modal('hide');
        },
        CambiarArchivo: function () {
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
        ClickArchivo: function () {
            var file = $('#FileComprobante');
            $('#txtComprobante').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#ArchivoComprobante')[0].value = null;
            $('#FileComprobante span span').text('Seleccionar Archivo...');
        },
        btnGuardarFormacionClick: function () {
            var $frm = $('#frmFormacion');
            if ($frm[0].checkValidity()) {
                if ($('#slcOFertaTipo').val() === "-1" || ($('#chkCedula')[0].checked === false && $('#chkTitulo')[0].checked === false )) {
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
        btnGuardarCursoExClick: function () {
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
        GuardarCurso: function (objCurso) {
            $.ajax({
                type: "POST",
                url: "WS/Docentes.asmx/GuardarCurso",
                data: objCurso,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== -1) {
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
        DatosCurso: function () {
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
        GuardarFormacionAcademica: function () {
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
        GuardarFormacionAcademicaDocumento: function (EstudioId,Tipo) {
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
    };
    
    Funciones.init();
    $('#tblDocentes').on('click', 'button', Funciones.IdentificarBoton);
    $('#tblDocentes').on('click', 'a', Funciones.IdentificarBoton);
    $('#btnCancelarFormacion').on('click', Funciones.CerrarPopFormacion);
    $('#btnCancelarCurso').on('click', Funciones.CerrarPopCurso);
    $('#ArchivoComprobante').bind('change', Funciones.CambiarArchivo);
    $('#FileComprobante a').click(Funciones.ClickArchivo);
    $('#btnGuardarFormacion').on('click', Funciones.btnGuardarFormacionClick);
    $('#btnGuardarCurso').on('click', Funciones.btnGuardarCursoExClick);
    
});