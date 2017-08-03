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
            Funciones.TraerDocentes();
            Funciones.TraerTiposOfertas();
        },
        EsCursoYMCA: false,
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
                                    var a = '<a name="OFertaTipoVer" class="bg-red">' + this.Anio + "-" + this.PeriodoId + "  " + this.EstudioDocente.Carrera + ' </a>'
                                    bot += a;
                                });
                                return bot;
                            } else {
                                var bot1;
                                bot1 = 'Aun no se captura información.'
                                return bot1;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            var bot = 'Aun no se captura información.'
                            if (d.CursosDocente.length > 0) {
                                var tiene = false;
                                $(d.CursosDocente).each(function () {
                                    if (this.EsCursoYMCA === false) {
                                        anio = this.Anio;
                                        periodo = this.PeriodoId;
                                        descripcion = this.Descripcion;
                                        tiene = true;
                                    }
                                });
                                bot = tiene ? '<a name="CursoExternoVer" class="bg-red">' + anio + "-" + periodo + "  " + descripcion + ' </a>'
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
                            bot = 'Aun no se captura información.'
                            if (d.CursosDocente.length > 0) {
                                var tiene = false;
                                var anio, periodo, descripcion;
                                $(d.CursosDocente).each(function () {
                                    if (this.EsCursoYMCA === true) {
                                        anio = this.Anio;
                                        periodo = this.PeriodoId;
                                        descripcion = this.Descripcion;
                                        tiene = true;
                                    }
                                });
                                bot = tiene ? '<a name="CursoYMCAVer" class="bg-red">' + anio + "-" + periodo + "  " + descripcion + ' </a>'
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
        MostrarFormacionAcademica: function (DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;

            $('#frmFormacion')[0].reset();

            $('#txtInstitucion').val(DTODocente.Nombre + " " + DTODocente.Paterno + " " + DTODocente.Materno);
            $('#slcOFertaTipo').val(DTODocente.ListaEstudios[0].EstudioDocente.OfertaEducativaTipoId);
            $('#txtPeriodo').val(DTODocente.ListaEstudios[0].Periodo.Descripcion);
            $('#txtCarrera').val(DTODocente.ListaEstudios[0].EstudioDocente.Carrera);

            $('#chkCedula')[0].checked = DTODocente.ListaEstudios[0].EstudioDocente.Cedula;
            $('#chkTitulo')[0].checked = DTODocente.ListaEstudios[0].EstudioDocente.Titulo;
            $('#btnVer').attr("href", DTODocente.ListaEstudios[0].EstudioDocente.Documento.DocumentoUrl);

            $('input').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });

            $('#ModalFormacion').modal('show');

        },
        MostrarCusos: function (DTODocente, esYmca) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            $('#frmCurso')[0].reset();

            $('#tiutuloCurso')[0].innerHTML = esYmca ? "Curso YMCA" : "Curso Externo";

            $('#txtCursoNombreI').addClass('edited');
            $('#txtTituloCurso').addClass('edited');
            $('#txtPeriodoCurso').addClass('edited');
            $('#slcDuracion').addClass('edited');
            $('#txtFechas').addClass('edited');

            $(DTODocente.CursosDocente).each(function () {
                if (this.EsCursoYMCA === esYmca) {
                    $('#txtCursoNombreI').val(this.Institucion);
                    $('#txtTituloCurso').val(this.Descripcion);
                    $('#slcDuracion').val(this.Duracion);
                    $('#txtPeriodoCurso').val(this.Periodo.Descripcion);
                    $('#txtFechas').val(this.FechaInicial + ' - ' + this.FechaFinal);
                }
            });

            $('#ModalCurso').modal('show');
        },
        CerrarPopFormacion: function () {
            $('#ModalFormacion').modal('hide');
        },
        CerrarPopCurso: function () {
            $('#ModalCurso').modal('hide');
        },
        DocenteSeleccionado: 0,
    };

    Funciones.init();
    $('#tblDocentes').on('click', 'a', Funciones.IdentificarBoton);
    $('#btnCancelarFormacion').on('click', Funciones.CerrarPopFormacion);
    $('#btnCancelarCurso').on('click', Funciones.CerrarPopCurso);
    $('#btnVistoBuenoFormacion').on('click', Funciones.btnGuardarFormacionClick);
    $('#btnVistoBuenoCurso').on('click', Funciones.btnGuardarCursoExClick);
});