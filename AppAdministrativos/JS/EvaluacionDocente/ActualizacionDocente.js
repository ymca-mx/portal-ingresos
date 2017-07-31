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
            Funciones.TraerTipoDocumento();
            Funciones.TraerDocentes();
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
        TraerTipoDocumento: function () {
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
                            if (d.Actualizaciones.length > 0) {
                                return "tiene";
                            } else {
                                var bot;
                                bot = '<button name="OFertaTipo" class="btn bg-blue">Agregar Formación</button>'
                                return bot;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            if (d.Actualizaciones.length > 0) {
                                return "tiene";
                            } else {
                                var bot;
                                bot = '<button name="CursoExterno" class="btn bg-blue">Agregar Curso</button>'
                                return bot;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            if (d.Actualizaciones.length > 0) {
                                return "tiene";
                            } else {
                                var bot;
                                bot = '<button name="CursoYMCA" class="btn bg-blue">Agregar Curso</button>'
                                return bot;
                            }
                        }
                    },
                    {
                        "mDataProp": function (d) {
                            if (d.Actualizaciones.length > 0) {
                                return "tiene";
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
        },
        PopFormacionAcademica: function (DTODocente) {
            Funciones.DocenteSeleccionado = DTODocente.DocenteId;
            $('#frmFormacion')[0].reset();
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-grey',
                radioClass: 'iradio_square-grey',
                increaseArea: '20%' // optional
            });
            $('#txtComprobante').text('');
            var file = $('#FileComprobante');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComprobante span span').text('Seleccionar Archivo...');

            $('#ModalFormacion').modal('show');
        },
        PopCursoExterno: function (DTODocente) {
            $('#frmCurso')[0].reset();

            $('#ModalCurso').modal('show');
            $('#tiutuloCurso')[0].innerHTML = "Curso Externo";
        },
        PopCursoYMCA: function (DTODocente) {
            $('#frmCurso')[0].reset();

            $('#ModalCurso').modal('show');
            $('#tiutuloCurso')[0].innerHTML = "Curso YMCA";
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
        GuardarFormacionAcademica: function () {
            var objFomacion = {
                DocenteId: Funciones.DocenteSeleccionado,
                Institucion: $('#txtInstitucion').val(),
                OFertaTipo: $('#slcOFertaTipo').val(),
                Carrera: $('#txtCarrera').val(),
                Cedula: $('#chkCedula')[0].checked,
                Titulo: $('#chkTitulo')[0].checked,
                UsuarioId: $.cookie('userAdmin')
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
            data.append("DocumentoComprobante", fileComprobante);
            data.append("EstudioId", EstudioId);
            data.append("TipoDocumento", Tipo);


            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/GuardarFormacionDocumento",
                data: data,
                contentType: false,
                processData: false,
                success: function (data1) {
                    $('#Load').modal('hide');
                    var $xml = $(data1);
                    var $bool = $xml.find("boolean");

                    if ($bool[0].textContent === 'true') {
                        alertify.alert("Guardado Correctamente.", function () {
                            Funciones.TraerDocentes();
                        });
                    } else {                        
                        alertify.alert("Fallo la subida del Archivo, intente nuevamente.", function () { $('#ModalFormacion').modal('show'); });
                    }
                }
            });
        },
        DocenteSeleccionado: 0,
    };

    Funciones.init();
    $('#tblDocentes').on('click', 'button', Funciones.IdentificarBoton);
    $('#btnCancelarFormacion').on('click', Funciones.CerrarPopFormacion);
    $('#btnCancelarCurso').on('click', Funciones.CerrarPopCurso);
    $('#ArchivoComprobante').bind('change', Funciones.CambiarArchivo);
    $('#FileComprobante a').click(Funciones.ClickArchivo);
    $('#btnGuardarFormacion').on('click', Funciones.btnGuardarFormacionClick);
});