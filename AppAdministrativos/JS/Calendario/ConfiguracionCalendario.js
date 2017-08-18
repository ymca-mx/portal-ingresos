$(function init() {
    var Calendarios;
    var EsModificacion = false;
    var CambioArchivo = false;
    var Funciones = {
        init: function () {
            Funciones.TraerCalendarios();
            Funciones.TraerPlantel();
        },
        TraerCalendarios: function () {
            $('#Load').modal('show');
            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/Listar",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        Funciones.PintarCalendarios(data.d);
                    } else { $('#Load').modal('hide'); }
                }
            });
        },
        Planteles:
        {
            Plantel: [{
                PlantelId: 0,
                Descripcion: "",
                OfertasTipo: [{
                    OfertaEducativaTipoId: 0,
                    Descripcion: "",
                    OFertasEducativas: [{
                        OfertaEducativaId: 0,
                        Descripcion: "",
                        visible: true
                    }]
                }]
            }],
            PlantelSeleccionado: [{
                PlantelId: 0,
                Descripcion: "",
                OfertasTipo: [{
                    OfertaEducativaTipoId: 0,
                    Descripcion: "",
                    OFertasEducativas: [{
                        OfertaEducativaId: 0,
                        Descripcion: "",
                        visible: true
                    }]
                }]
            }]
        },
        CargarOfertas: function (DTOCalendario) {
            $("#slcSucursal").val(-1);
            $("#slcSucursal").change();
            Funciones.Planteles.PlantelSeleccionado = [];
            Funciones.OfertasExistentes = [];
            console.log(DTOCalendario);

            $(DTOCalendario.Sucursales).each(function (f, obj) {
                var sucursal = {
                    Descripcion: obj.Descripcion,
                    PlantelId: obj.SucursalId,
                    OfertasTipo: []
                };

                $(obj.OFertaEducativaTipo).each(function (a, obj1) {

                    var TipoOFerta = {
                        OfertaEducativaTipoId: obj1.OfertaEducativaTipoId,
                        Descripcion: obj1.Descripcion,
                        OFertasEducativas: []
                    };

                    $(obj1.Ofertas).each(function (c, obj2) {
                        var Oferta = {
                            OfertaEducativaId: obj2.ofertaEducativaId,
                            Descripcion: obj2.descripcion,
                            visible: true
                        };
                        Funciones.OfertasExistentes.push(obj2.ofertaEducativaId);
                        TipoOFerta.OFertasEducativas.push(Oferta);
                    });

                    sucursal.OfertasTipo.push(TipoOFerta);

                });

                Funciones.Planteles.PlantelSeleccionado.push(sucursal);
                
            });

            $('#ModalOfertas').modal('show');
        },
        PintarCalendarios: function (Tabla) {
            if (Calendarios !== undefined) {
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
            var row = this.parentNode.parentNode;
            var rowadd = Calendarios.fnGetData($(this).closest('tr'));
            if (this.name === "Edit") {
                Funciones.CargarDatosCalendario(rowadd);
            } else if (this.name === "Show") {
                Funciones.CargarOfertas(rowadd);
            }
        },
        CambiarArchivo: function () {
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
        GuardarNuevoCalendario: function () {
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
        SubirArchivo: function (Id, mensaje) {
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
                            alertify.alert('Se guardaron las modificaciones').set('onok', function (CloseEvent) { Funciones.TraerCalendarios(); });
                        }
                    } else {
                        $('#Load').modal('hide');
                        $('#ModificarCalendario').modal('hide');
                        alertify.alert('Error no se pudo modificar el calendario, intente más tarde.', function () { $('#ModificarCalendario').modal('show'); });
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
        },
        CerrarPopOfertas: function () {
            $('#frmOfertas')[0].reset();
            $('#ModalOfertas').modal('hide');
        },
        SlcSucursalChange: function () {
            var valSelec = $(this)[0].value;
            valSelec = parseInt(valSelec);
            $('#divChkXAgregar').empty();
            $('#divChkAgregadas').empty();
            if (valSelec !== -1) {
                $("#slcTipoOferta").empty();
                var opt1 = $(document.createElement('option'));
                opt1.text('--Seleccionar--');
                opt1.val(-1);
                $("#slcTipoOferta").append(opt1);

                $(Funciones.Planteles.Plantel).each(function (ind, plan) {
                    if (plan.PlantelId === valSelec) {
                        $(plan.OfertasTipo).each(function (ind2, plan2) {
                            var opt = $(document.createElement('option'));
                            opt.text(plan2.Descripcion);
                            opt.val(plan2.OfertaEducativaTipoId);
                            $("#slcTipoOferta").append(opt);
                        });
                    }
                });
            }
        },
        SlcTipoOfertaChange: function () {
            var valSelec = $("#slcSucursal").val();
            valSelec = parseInt(valSelec);

            var TipoF = $(this)[0].value;
            TipoF = parseInt(TipoF);
            $('#divChkXAgregar').empty();
            $('#divChkAgregadas').empty();
            if (valSelec !== -1) {
                var Ofertas = [];

                $(Funciones.Planteles.PlantelSeleccionado).each(function (ind, plan) {
                    if (plan.PlantelId === valSelec) {
                        $(plan.OfertasTipo).each(function (ind2, plan2) {
                            if (TipoF === plan2.OfertaEducativaTipoId) {
                                $(plan2.OFertasEducativas).each(function (ind3, plan3) {
                                    Ofertas.push(plan3.OfertaEducativaId);
                                    
                                    var id = 'Id=chk' + plan3.OfertaEducativaId;
                                    var check = "<div class='checkbox'><label class='control-label' style='padding-left:20px' >";
                                    check += "<input type='checkbox'" + id + " />" + plan3.Descripcion + "</label></div>";
                                    if (Funciones.OfertasQuitar.findIndex(plan3.OfertaEducativaId) !== -1) {
                                        $('#divChkAgregadas').append(check);
                                    } else {
                                        $('#divChkXAgregar').append(check);
                                    }
                                });
                            }
                        });
                    }
                });

                $(Funciones.Planteles.Plantel).each(function (ind, plan) {
                    if (plan.PlantelId === valSelec) {
                        $(plan.OfertasTipo).each(function (ind2, plan2) {
                            if (TipoF === plan2.OfertaEducativaTipoId) {
                                $(plan2.OFertasEducativas).each(function (ind3, plan3) {
                                    if (jQuery.inArray(plan3.OfertaEducativaId, Ofertas) === -1) {

                                        var id = 'Id=chk' + plan3.OfertaEducativaId;
                                        var check = "<div class='checkbox'><label class='control-label' style='padding-left:20px' >";
                                        check += "<input type='checkbox'" + id + " data-ofertaid='" + plan3.OfertaEducativaId + "'/>" + plan3.Descripcion + "</label></div>";
                                        $('#divChkXAgregar').append(check);

                                        if (jQuery.inArray(plan3.OfertaEducativaId, Funciones.OfertasAgregar) !== -1) {
                                            $('#chk' + plan3.OfertaEducativaId)[0].checked = true;
                                        }
                                    }
                                });
                            }
                        });
                    }
                });
            }
        },
        TraerPlantel: function () {
            $.ajax({
                type: "POST",
                url: "WS/Calendario.asmx/TraerOfertas",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data1) {
                    if (data1.d.length > 0) {

                        var optionP = $(document.createElement('option'));
                        optionP.text('--Seleccionar--');
                        optionP.val(-1);

                        $("#slcSucursal").append(optionP);


                        Funciones.Planteles.Plantel = [];
                        $(data1.d).each(function (f, obj) {
                            var optionP1 = $(document.createElement('option'));
                            optionP1.text(obj.Descripcion);
                            optionP1.val(obj.SucursalId);
                            $("#slcSucursal").append(optionP1);

                            var sucursal = {
                                Descripcion: obj.Descripcion,
                                PlantelId: obj.SucursalId,
                                OfertasTipo: []
                            };

                            $(obj.OFertaEducativaTipo).each(function (a, obj1) {

                                var TipoOFerta = {
                                    OfertaEducativaTipoId: obj1.OfertaEducativaTipoId,
                                    Descripcion: obj1.Descripcion,
                                    OFertasEducativas: []
                                };

                                $(obj1.Ofertas).each(function (c, obj2) {
                                    var Oferta = {
                                        OfertaEducativaId: obj2.ofertaEducativaId,
                                        Descripcion: obj2.descripcion,
                                        visible: true
                                    };
                                    TipoOFerta.OFertasEducativas.push(Oferta);
                                });

                                sucursal.OfertasTipo.push(TipoOFerta);

                            });

                            Funciones.Planteles.Plantel.push(sucursal);
                        });
                    } else {
                        alertify.alert("Fallo la carga de sucursales");
                    }
                },
                error: function () {
                    alertify.alert("Fallo la carga de sucursales");
                }
            });
        },
        ClickCheckBoxXAdd: function () {
            var oid = $(this).data('ofertaid');
            if ($(this)[0].checked) {
                $('#btnPasar').prop("disabled", false);
                Funciones.OfertasAgregar.push(oid);
            }
            else {
                Funciones.OfertasAgregar = jQuery.grep(Funciones.OfertasAgregar, function (value) {
                    return value != oid;
                });
            }
            if (Funciones.OfertasAgregar.length === 0) { $('#btnPasar').prop("disabled", true); }
        },
        ClickCheckBoxXPut: function () {
            var oid = $(this).data('ofertaid');
            if ($(this)[0].checked) {
                $('#btnRegresa').prop("disabled", false);
                Funciones.OfertasQuitar.push(oid);
            } else {
                Funciones.OfertasQuitar = jQuery.grep(Funciones.OfertasQuitar, function (value) {
                    return value != oid;
                });
            }
            if (Funciones.OfertasQuitar.length === 0) { $('#btnRegresa').prop("disabled", true); }
        },
        OfertasExistentes:[],
        OfertasAgregar: [],
        OfertasQuitar: [],
        BtnPasarClick: function () {
            var hijos = $('#divChkXAgregar')[0].childNodes;
            $(hijos).each(function () {
                var idchk = $(this)[0].childNodes[0].childNodes[0].id;
                var ofertaId = $('#' + idchk).data('ofertaid');
                if (jQuery.inArray(ofertaId, Funciones.OfertasAgregar) !== -1) {
                    $('#' + idchk)[0].checked = false;
                    $(this).clone().appendTo('#divChkAgregadas');
                    this.parentNode.removeChild(this);
                    Funciones.OfertasExistentes.push(ofertaId);
                    Funciones.OfertasAgregar = jQuery.grep(Funciones.OfertasAgregar, function (value) {
                        return value != ofertaId;
                    });
                    $('#btnPasar').prop("disabled", true);
                }
            });
            if (Funciones.OfertasQuitar.length === 0) { $('#btnRegresa').prop("disabled", true); }
            Funciones.OrdenarListas('divChkAgregadas');
        },
        BtnRegresarClick: function () {
            var hijos = $('#divChkAgregadas')[0].childNodes;
            $(hijos).each(function () {
                var idchk = $(this)[0].childNodes[0].childNodes[0].id;
                var ofertaId = $('#' + idchk).data('ofertaid');
                if (jQuery.inArray(ofertaId, Funciones.OfertasQuitar) !== -1) {
                    $('#' + idchk)[0].checked = false;
                    $(this).clone().appendTo('#divChkXAgregar');
                    this.parentNode.removeChild(this);

                    Funciones.OfertasQuitar = jQuery.grep(Funciones.OfertasQuitar, function (value) {
                        return value != ofertaId;
                    });

                    Funciones.OfertasExistentes = jQuery.grep(Funciones.OfertasExistentes, function (value) {
                        return value != ofertaId;
                    });
                    $('#btnPasar').prop("disabled", true);
                }
            });
            $('#btnRegresa').prop("disabled", true); 
            Funciones.OrdenarListas('divChkXAgregar');
        },
        OrdenarListas: function (idcombo) {
            var hijos = $('#' + idcombo)[0].childNodes;

            var hijos2 = $(hijos).map(function (_, o) {
                return { objhtml: $(o)[0].innerHTML, ofertaid: $($(o)[0].childNodes[0].childNodes[0]).data("ofertaid") };
            }).get();

            hijos2.sort(function (a, b) {
                return (a.ofertaid > b.ofertaid) ? 1 : ((a.ofertaid < b.ofertaid) ? -1 : 0);
            });

            $(hijos).each(function (i, o) {
                o.innerHTML = hijos2[i].objhtml;
            });            
        }
    };
    Funciones.init();
    $('#tblCalendarios').on('click', 'a', Funciones.ClickTabla);
    $('#ArchivoCalendario').bind('change', Funciones.CambiarArchivo);
    $('#FileCalendario a').click(Funciones.ClickArchivo);
    $('#btnCancelar').on('click', Funciones.CerrarModificar);
    $('#btnCancelarOf').on('click', Funciones.CerrarPopOfertas);
    $('#btnNuevo').on('click', Funciones.NuevoCalendario);
    $('#btnGuardarDatos').on('click', Funciones.BotonGuardar);
    $("#slcSucursal").on('change', Funciones.SlcSucursalChange);
    $("#slcTipoOferta").on('change', Funciones.SlcTipoOfertaChange);
    $("#divChkXAgregar").on('click', 'input', Funciones.ClickCheckBoxXAdd);
    $('#divChkAgregadas').on('click', 'input', Funciones.ClickCheckBoxXPut);
    $('#btnPasar').on('click', Funciones.BtnPasarClick);
    $('#btnRegresa').on('click', Funciones.BtnRegresarClick);
});