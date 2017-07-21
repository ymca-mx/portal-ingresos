$(function init() {
    var Calendarios;
    var EsModificacion = false;
    var CambioArchivo = false;
    var Funciones = {
        TraerCalendarios: function () {
            $('#Load').modal('show');
            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/Listar",
                data:"",
                contentType: "application/json; charset=utf-8", 
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        Funciones.PintarCalendarios(data.d);
                    } else { $('#Load').modal('hide');}
                }
            });
        },
        PintarCalendarios: function (Tabla) {
            if (Calendarios != undefined) {
                Calendarios.fnClearTable();
            }
            Calendarios = $('#tblCalendarios').dataTable({
                "aaData": Tabla,
                "aoColumns": [
                    { "mDataProp": "Nombre" },                    
                    { "mDataProp": "FechaAlta" },                    
                    { "mDataProp": "UsuarioNombre" },
                    {
                        "mDataProp": "EstatusId",
                        "mRender": function (data, f, d) {
                            return "<span>" + (d.EstatusId === 1 ? "Activo" : "Inactivo") + "</span>";
                        }
                    },
                    {
                        "mDataProp": function () {
                            var a = '<a class="btn bg-blue" name="Show">Ver Ofertas</a>'
                            return a;
                        },
                    },
                    {
                        "mDataProp": function () {
                            var a = '<a class="btn bg-blue" name="Edit">Modificar</a>'
                            return a;
                        },
                    },

                ],
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": true,
                "ordering": true,
                "info": false,
                "destroy": true,
                "language": {
                    "lengthMenu": "_MENU_  Registros",
                    "paginate": {
                        "previous": "<",
                        "next": ">"
                    },
                    "search": "Buscar Calendario "
                },
                "order": [[2, "desc"]]
            });
            $('#Load').modal('hide');
        },
        ClickTabla: function (eventObject) {
            if (this.name === "Edit") {
                var row = this.parentNode.parentNode;
                var rowadd = Calendarios.fnGetData($(this).closest('tr'));
                Funciones.CargarDatosCalendario(rowadd);
            } else if (this.name === "Show") {

            }
        },
        CambiarArchivo: function() {
            var file = $('#FileCalendario');
            var tex = $('#txtCalendario').html();
            if (this.files.length > 0) {
                CambioArchivo = true;
                $('#txtCalendario').text(this.files[0].name);
                file.addClass('fileinput-exists').removeClass('fileinput-new');
                $('#FileCalendario span span').text('Cambiar');
            }
            else {
                if (!EsModificacion) {
                    $('#txtCalendario').text('');
                    file.removeClass('fileinput-exists').addClass('fileinput-new');
                    $('#FileCalendario span span').text('Seleccionar Archivo...');
                }
            }
        },
        ClickArchivo: function () {
            if (!EsModificacion) {
                var file = $('#FileCalendario');
                $('#txtCalendario').text('');
                file.removeClass('fileinput-exists').addClass('fileinput-new');
                $('#ArchivoCalendario')[0].value = null;
                $('#FileCalendario span span').text('Seleccionar Archivo...');
            }
        },
        CargarDatosCalendario: function (objCalendario) {
            EsModificacion = true;
            $('#txtNombre').val(objCalendario.Nombre);
            $('#txtNombre').data('calendarioid', objCalendario.CalendarioEscolarId);

            $('#FileCalendario span span').text('Cambiar');

            var nombre = objCalendario.Direccion;
            nombre = nombre.split("/").pop();
            $('#txtCalendario').text(nombre);
            var file = $('#FileCalendario');
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#ArchivoCalendario').removeAttr('required');

            if (objCalendario.EstatusId === 1) {
                $('#rdbActivo')[0].checked = true;
            } else { $('#rdbInactivo')[0].checked = true; }

            $('#ModificarCalendario').modal('show');
        },
        CerrarModificar: function () {
            var file = $('#FileCalendario');
            $('#frmDatos')[0].reset();
            $('#txtNombre').val('');
            $('#txtCalendario').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileCalendario span span').text('Seleccionar Archivo...');
            $('#rdbActivo')[0].checked = false;
            $('#rdbActivo')[0].checked = false;
            EsModificacion = false;
            
            $('#ModificarCalendario').modal('hide');            
        },
        NuevoCalendario: function () {
            $('#ArchivoCalendario').attr('required', true);
            $('#ModificarCalendario').modal('show');
        },
        BotonGuardar: function () {
            var $frm = $('#frmDatos');
            if ($frm[0].checkValidity()) {
                if (EsModificacion) { Funciones.GuardarModificacion(); }
                else { Funciones.GuardarNuevoCalendario(); }
            } 
        },
        GuardarNuevoCalendario: function() {
            $('#Load').modal('show');
            var objGuardar = Funciones.CamposCalendario();
            objGuardar = JSON.stringify(objGuardar);
            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/Insert",
                data: objGuardar,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d > 0) {
                        Funciones.SubirArchivo(data.d, 'Se agrego el nuevo calendario');
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert('Error no se pudo agregar el nuevo calendario, intente más tarde.');
                    }
                }
            });
        },
        SubirArchivo: function (Id,mensaje) {
            var data = new FormData();
            var fileCalendario = $('#ArchivoCalendario'); // FileList object
            fileCalendario = fileCalendario[0].files[0];
            data.append("DocumentoCalendario", fileCalendario);

            data.append("CalendarioEscolarId", Id);


            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/GuardarCalendario",
                data: data,
                contentType: false,
                processData: false,
                success: function (data1) {
                    $('#Load').modal('hide');
                    var $xml = $(data1);
                    var $bool = $xml.find("boolean");
                    
                    if ($bool[0].textContent === 'true') {
                        Funciones.CerrarModificar();
                        alertify.alert(mensaje).set('onok', function (closeEvent) {
                            Funciones.TraerCalendarios()
                        });
                    } else {
                        $('#ModificarCalendario').modal('hide');
                        alertify.alert("Fallo la subida del Archivo, intente nuevamente.", function () { $('#ModificarCalendario').modal('show'); });
                    }
                }
            });            
        },
        GuardarModificacion: function () {
            $('#Load').modal('show');
            var objGuardar = Funciones.CamposCalendario();
            objGuardar = JSON.stringify(objGuardar);
            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/Update",
                data: objGuardar,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d) {
                        if (CambioArchivo) {
                            Funciones.SubirArchivo($('#txtNombre').data('calendarioid'), 'Se guardaron las modificaciones');
                        } else {
                            $('#Load').modal('hide');
                            Funciones.CerrarModificar();
                            alertify.alert('Se guardaron las modificaciones').set('onok', function (CloseEvent) { Funciones.TraerCalendarios();});
                        }
                    } else {
                        $('#Load').modal('hide');
                        $('#ModificarCalendario').modal('hide');
                        alertify.alert('Error no se pudo modificar el calendario, intente más tarde.', function () { $('#ModificarCalendario').modal('show');   });
                    }
                }
            });
        },
        CamposCalendario: function () {
            return {
                Calendario: {
                    CalendarioEscolarId: $('#txtNombre').data('calendarioid'),
                    Nombre: $('#txtNombre').val(),
                    Direccion: $('#txtCalendario').text(),
                    UsuarioId: $.cookie('userAdmin'),
                    EstatusId: ($('#rdbActivo')[0].checked ? 1 : 2)
                }
            };
        }
    }


    Funciones.TraerCalendarios();
    $('#tblCalendarios').on('click', 'a', Funciones.ClickTabla);
    $('#ArchivoCalendario').bind('change', Funciones.CambiarArchivo);
    $('#FileCalendario a').click(Funciones.ClickArchivo);
    $('#btnCancelar').on('click', Funciones.CerrarModificar);
    $('#btnNuevo').on('click', Funciones.NuevoCalendario);
    $('#btnGuardarDatos').on('click', Funciones.BotonGuardar);
});