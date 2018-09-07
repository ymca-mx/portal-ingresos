$(function () {
    var tblTitulos,
        tblAlumnos;

    var ClasesFn = {
        AlumnoTitulo: class {
            constructor(Alumno, objins, objTit, objCa, objAnt, lstUsuarios, lstSedes) {
                Alumno = Alumno || {};
                objins = objins || {};
                objTit = objTit || {};
                objCa = objCa || {};
                objAnt = objAnt || {};
                lstUsuarios = lstUsuarios || {};
                lstSedes = lstSedes || {};
                var objRes = [];
                $(lstUsuarios).each(function () { objRes.push(new ClasesFn.Responsable(this)); });

                this.AlumnoId = Alumno.AlumnoId;
                this.Nombre = Alumno.Nombre;
                this.Paterno = Alumno.Paterno;
                this.Materno = Alumno.Materno;
                this.CURP = Alumno.CURP;
                this.Email = Alumno.Email;
                this.Institucion = new ClasesFn.Institucion(objins);
                this.Titulo = new ClasesFn.Titulo(objTit);
                this.Carrera = new ClasesFn.Carrera(objCa);
                this.Antecedente = new ClasesFn.Antecedente(objAnt);
                this.Responsables = objRes;
                this.Sede = lstSedes;
                this.UsuarioId = Alumno.UsuarioId;
                this.EstatusId = Alumno.EstatusId;
            }
        },

        Institucion: class {
            constructor(obj) {
                obj = obj || {};
                this.InstitucionId = obj.InstitucionId;
                this.SedeId = obj.SedeId;
                this.Nombre = obj.Nombre;
                this.Clave = obj.Clave;
            }
        },

        Titulo: class {
            constructor(obj) {
                obj = obj || {};
                this.MedioTitulacionId = obj.MedioTitulacionId;
                this.MedioTitulacion = obj.MedioTitulacion;
                this.FExamenProf = obj.FExamenProf;
                this.FExencion = obj.FExencion;
                this.FundamentoLegalId = obj.FundamentoLegalId;
                this.EntidadFederativaId = obj.EntidadFederativaId;
            }
        },

        Carrera: class {
            constructor(obj) {
                obj = obj || {};
                this.OfertaEducativaId = obj.OfertaEducativaId;
                this.OfertaEducativa = obj.OfertaEducativa;
                this.Clave = obj.Clave;
                this.FInicio = obj.FInicio;
                this.FFin = obj.FFin;
                this.AutReconocimientoId = obj.AutReconocimientoId;
                this.RVOE = obj.RVOE;
            }
        },

        Antecedente: class {
            constructor(obj) {
                obj = obj || {};
                this.Institucion = obj.Institucion;
                this.TipoAntecedenteId = obj.TipoAntecedenteId;
                this.TipoAntecedente = obj.TipoAntecedente;
                this.EntidadFederativaId = obj.EntidadFederativaId;
                this.FechaInicio = obj.FechaInicio;
                this.FechaFin = obj.FechaFin;
            }
        },

        Responsable: class {
            constructor(obj) {
                obj = obj || {};
                this.UsuarioId = obj.UsuarioId;
                this.Nombre = obj.Nombre;
                this.Paterno = obj.Paterno;
                this.Materno = obj.Materno;
                this.CargoId = obj.CargoId;
            }
        }
    };

    var TituloFn = {
        init() {
            
            $('#btnBuscar').on('click', this.BtnBuscarClick);
            $('#txtClave').on('keydown', this.txtClaveKeydown);
            $('#tblAlumnos').on('click', 'a', this.TablaAlumnoClick);
            $('#btnAlumnoAdd').on('click', this.PushAlumno);
            $('#btnTituloAdd').on('click', this.PushTitulo);
            $('#btnSedeAdd').on('click', this.PushSede);
            $('#btnAntecedenteAdd').on('click', this.PushAntecedente);
            $('#btnResponsableAdd').on('click', this.PushResponsable);
            $('#tblTitulos').on('click', 'button', this.NameButton);
            $('#slcOferta').on('change', this.OfertaChange);
            $('#slcSedePrev').on('change', this.SedeChangePrev);
            $('#slcSede').on('change', this.SedeChange);
            $('#slcCargo1').on('change', this.CargoChange);
            $('#slcCargo2').on('change', this.CargoChange);
            $('#slcResponsable1').on('change', this.ResponsableChange);
            $('#slcResponsable2').on('change', this.ResponsableChange);
            $('#btnGuardar').on('click', this.UpdateAlumnos);
            $('#slcMedioTitulacion').on('change', this.MedioChange); 
            $('#tblTitulos').on('click','input', TituloFn.SetEstatus);

            this.TipoEstudio();
            this.AutorizacionReconocimiento();
            this.MedioTitulacion();
            this.ServicioSocial();
            this.Cargo();
            GlobalFn.GetEstado("slcEntidadFederativa", 9);
            GlobalFn.GetEstado("slcEntidadAntecedente", 9);

            this.InitCalendar('txtFInicioOferta');
            this.InitCalendar('txtFFinOferta');
            this.InitCalendar('txtFechaExamen');
            this.InitCalendar('txtFechaExencion');
            this.InitCalendar('txtFechaInicio');
            this.InitCalendar('txtFechaFin');
            tblTitulos = $('#tblTitulos').DataTable();
            $('#modalAlumno').on('hidden.bs.modal', this.RemoveClass);
            $('#modalSede').on('hidden.bs.modal', this.RemoveClass);
            $('#modalTitulo').on('hidden.bs.modal', this.RemoveClass);
            $('#modalAntecedente').on('hidden.bs.modal', this.RemoveClass);
            $('#modalResponsables').on('hidden.bs.modal', this.RemoveClass);

            this.GetSolicitados("");
        },
        RemoveClass() {
            $("#tblTitulos tr").removeClass('bg-purple');
        },
        InitCalendar(inputId) {
            inputId = $('#' + inputId)[0].parentNode;

            $(inputId).datepicker({
                orientation: "left",
                autoclose: true,
                format: "dd/mm/yyyy",
                language: 'es'
            });
        },
        UpdateFecha(inputId, val) {
            inputId = $('#' + inputId)[0].parentNode;

            $(inputId).datepicker("update", val);
        },
        TipoEstudio() {
            IndexFn.Api("SEP/TipoEstudio", "GET", "")
                .done(function (data) {
                    $(data).each(function () {
                        var option = $(document.createElement('option'));

                        option.val(this.TipoEstudioId);
                        option.text(this.TipoEstudio);

                        $("#slcTAntecedente").append(option);
                    });

                    $('#slcTAntecedente').change();
                })
                .fail(function (data) {
                    console.log(data);
                });
        },
        MedioTitulacion() {
            IndexFn.Api("SEP/MedioTitulacion", "GET", "")
                .done(function (data) {
                    $(data).each(function () {
                        var option = $(document.createElement('option'));

                        option.val(this.MedioTitulacionId);
                        option.text(this.Descripcion + " - " + this.TipoModalidad);
                        option.data("Descripcion", this.Descripcion);

                        $("#slcMedioTitulacion").append(option);
                    });

                    $('#slcMedioTitulacion').change();
                })
                .fail(function (data) {
                    console.log(data);
                });
        },
        ServicioSocial() {
            IndexFn.Api("SEP/Servicio", "GET", "")
                .done(function (data) {
                    $(data).each(function () {
                        var option = $(document.createElement('option'));

                        option.val(this.ServicioId);
                        option.text(this.Descripcion);

                        $("#slcServicio").append(option);
                    });
                    $("#slcServicio").val(4);
                    $('#slcServicio').change();
                })
                .fail(function (data) {
                    console.log(data);
                });
        },
        Cargo() {
            IndexFn.Api("SEP/Responsable", "GET", "")
                .done(function (data) {
                    TituloFn.SetCargo(data, 'slcCargo1',6);
                    TituloFn.SetCargo(data, 'slcCargo2',3);
                })
                .fail(function (data) {
                    console.log(data);
                });
        },
        AutorizacionReconocimiento() {
            IndexFn.Api("SEP/AutRec", "GET", "")
                .done(function (data) {
                    $(data).each(function () {
                        var option = $(document.createElement('option'));

                        option.val(this.AutRecId);
                        option.text(this.Descripcion);

                        $("#slcAutRec").append(option);
                    });

                    $('#slcAutRec').change();
                })
                .fail(function (data) {
                    console.log(data);
                });
        },
        BuscarAlumno(idAlumno) {
            $('#modalAlumno input').val('');
            $('#modalAlumno select').empty();
            TituloFn.AlumnoSelect = new ClasesFn.AlumnoTitulo();

            IndexFn.Api("Sep/Alumno/" + idAlumno, "GET", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    if (data.Sede.length === 0) {
                        alertify.alert("Universidad YMCA", "El alumno no contiene una oferta valida o no tiene mas ofertas por titular.");
                        return false;
                    }
                    $('#txtAlumnoId').val(data.AlumnoId);
                    $('#txtNombre').val(data.Nombre);
                    $('#txtPaterno').val(data.Paterno);
                    $('#txtMaterno').val(data.Materno);
                    $('#txtCURP').val(data.CURP);
                    $('#txtEmail').val(data.Email);
                    //TituloFn.SetOfertas(data.OfertaEducativa, "slcOfertaPrev");
                    TituloFn.SetSede(data.Sede, "slcSedePrev");

                    $('#modalAlumno').data("Antecedente", JSON.stringify(data.Antecedente));
                    $('#modalAlumno').data("Sede", JSON.stringify(data.Sede));
                    
                    $('#btnAlumnoAdd').show();
                    $('#modalAlumno').modal('show');
                })
                .fail(function (data) {
                    console.log(data);
                    IndexFn.Block(false);
                });
        },
        BuscarNombre(Nombre) {

            IndexFn.Api("Alumno/BuscarAlumnoString/" + Nombre, "GET", "")
                .done(function (data) {
                    if (data !== null) {
                        $('#frmVarios').show();
                        tblAlumnos = $('#tblAlumnos').dataTable({
                            "aaData": data,
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
                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    console.log(data);
                });
        },
        BtnBuscarClick() {

            var AlumnoId = $('#txtClave').val();

            if (AlumnoId.length === 0) { return false; }

            IndexFn.Block(true);

            if (!isNaN(AlumnoId)) { TituloFn.BuscarAlumno(AlumnoId); }
            else { TituloFn.BuscarNombre(AlumnoId); }
        },
        txtClaveKeydown(e) {
            if (e.which === 13) {
                TituloFn.BtnBuscarClick();
            }
        },
        TablaAlumnoClick() {
            IndexFn.Block(true);
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));

            TituloFn.BuscarAlumno(rowadd.AlumnoId);
        },
        SetEstatus() {
            TituloFn.RowSelect = {};
            TituloFn.RowSelect = $(this).parents()[3];

            TituloFn.AlumnoSelect = new ClasesFn.AlumnoTitulo();
            TituloFn.AlumnoSelect = tblTitulos.row(TituloFn.RowSelect).data();

            TituloFn.AlumnoSelect.EstatusId = $(this).prop('checked') ? 2 : 1;


            tblTitulos
                .row(TituloFn.RowSelect)
                .data(TituloFn.AlumnoSelect)
                .draw();
        },
        MedioChange() {
            if (parseInt($('#slcMedioTitulacion').val()) !== 6) {
                $($('#txtFechaExamen').parents()[2]).show();
                $($('#txtFechaExencion').parents()[2]).hide();
            } else {
                $($('#txtFechaExencion').parents()[2]).show();
                $($('#txtFechaExamen').parents()[2]).hide();
            }
        },
        PushAlumno() {
            var objAlumno = new ClasesFn.AlumnoTitulo();
            var objInstitu = JSON.parse($('#modalAlumno').data("Sede"));
            var objAnte = JSON.parse($('#modalAlumno').data("Antecedente"));

            objAlumno.AlumnoId = $('#txtAlumnoId').val();
            objAlumno.Nombre = $('#txtNombre').val();
            objAlumno.Paterno = $('#txtPaterno').val();
            objAlumno.Materno = $('#txtMaterno').val();
            objAlumno.CURP = $('#txtCURP').val();
            objAlumno.Email = $('#txtEmail').val();
            objAlumno.Sede = objInstitu;
            objAlumno.UsuarioId = localStorage.getItem('userAdmin');

            var objinsti = objInstitu.find(inst => inst.Clave === $('#slcSedePrev').find(':selected').data('Clave'));

            objAlumno.Institucion = new ClasesFn.Institucion({
                InstitucionId: objinsti.InstitucionId,
                SedeId: objinsti.SedeId,
                Nombre: objinsti.Nombre,
                Clave: objinsti.Clave
            });


            objAlumno.Carrera = new ClasesFn.Carrera({
                OfertaEducativaId: $('#slcOfertaPrev').val(),
                OfertaEducativa: $('#slcOfertaPrev').find(':selected').text(),
                Clave: $('#slcOfertaPrev').find(':selected').data('Clave'),
                FInicio: $('#slcOfertaPrev').find(':selected').data('FechaInicio'),
                FFin: $('#slcOfertaPrev').find(':selected').data('FechaFin'),
                AutReconocimientoId: 0,
                RVOE: $('#slcOfertaPrev').find(':selected').data('RVOE'),
            });


            objAlumno.Antecedente = new ClasesFn.Antecedente({
                Institucion: objAnte.Procedencia,
                TipoAntecedenteId: undefined,
                TipoAntecedente: "",
                EntidadFederativaId: objAnte.EntidadFederativaId,
                FechaInicio: objAnte.FechaInicio,
                FechaFin: objAnte.FechaTermino
            });

            objAlumno.Responsables = [
                new ClasesFn.Responsable({}),
                new ClasesFn.Responsable({}),
            ];

            if (objAlumno.Email.length > 0 && objAlumno.CURP.length === 18) {
                var filter = TituloFn.lstTitulos.find(x => x.AlumnoId === objAlumno.AlumnoId
                    && x.Carrera.OfertaEducativaId === objAlumno.Carrera.OfertaEducativaId);
                if (filter === undefined) {
                    TituloFn.Enviar(objAlumno);
                } else {
                    alertify.alert("Ya existe un registro con el mismo Id de alumno.");
                }

            } else {
                alertify.alert("El CURP y Email son obligatorios.");
            }

            $('#txtClave').val('');
            $('#modalAlumno').modal('hide');

        },
        PushTitulo() {

            if (parseInt($('#slcEntidadFederativa').val()) === -1) {
                alertify.alert("Universidad YMCA", "La entidad Federativa no es valida.");
                return false;
            }
            var fecha = parseInt($('#slcMedioTitulacion').val()) === 6
                ? $('#txtFechaExencion').val()
                : $('#txtFechaExamen').val();

            if (fecha === '01/01/1900') {
                alertify.alert("Universidad YMCA", "La fecha no es valida.");
                return false;
            }

            var Titulo = new ClasesFn.Titulo({
                MedioTitulacionId: $('#slcMedioTitulacion').val(),
                MedioTitulacion: $('#slcMedioTitulacion :selected').data("Descripcion"),
                FExamenProf: $('#txtFechaExamen').val(),
                FExencion: $('#txtFechaExencion').val(),
                FundamentoLegalId: $('#slcServicio').val(),
                EntidadFederativaId: $('#slcEntidadFederativa').val()
            });

            TituloFn.AlumnoSelect.Titulo = Titulo;

            tblTitulos
                .row(TituloFn.RowSelect)
                .data(TituloFn.AlumnoSelect)
                .draw();

            $('#modalTitulo').modal('hide');
        },
        PushSede() {
            if ($('#txtFFinOferta').val() === '01/01/1900') {
                alertify.alert("La fecha de termino de los estudios no es valida.");
                return false;
            }
            var Carrera = new ClasesFn.Carrera();
            Carrera.AutReconocimientoId = $('#slcAutRec').val();
            Carrera.Clave = $('#slcSede :selected').data("Clave");
            Carrera.FFin = $('#txtFFinOferta').val();
            Carrera.FInicio = $('#txtFInicioOferta').val();
            Carrera.OfertaEducativa = $('#slcOferta :selected').text();
            Carrera.OfertaEducativaId = $('#slcOferta').val();
            Carrera.RVOE = $('#txtRVOE').val();

            TituloFn.AlumnoSelect.Carrera = Carrera;

            tblTitulos
                .row(TituloFn.RowSelect)
                .data(TituloFn.AlumnoSelect)
                .draw();
            
            $('#modalSede').modal('hide');
        },
        PushAntecedente() {
            if (parseInt($('#slcEntidadAntecedente').val()) === -1 || $('#txtFechaFin').val() === '01/01/1900') {
                alertify.alert("Universidad YMCA", "La endiad federativa o la fecha de termino no son validas.");
                return false;
            }
            var antecedente = new ClasesFn.Antecedente({
                Institucion: $('#txtProcedencia').val(),
                TipoAntecedenteId: $('#slcTAntecedente').val(),
                TipoAntecedente: $('#slcTAntecedente :selected').text(),
                EntidadFederativaId: $('#slcEntidadAntecedente').val(),
                FechaInicio: $('#txtFechaInicio').val(),
                FechaFin: $('#txtFechaFin').val(),
            });

            TituloFn.AlumnoSelect.Antecedente = antecedente;

            tblTitulos
                .row(TituloFn.RowSelect)
                .data(TituloFn.AlumnoSelect)
                .draw();

            $('#modalAntecedente').modal('hide');
        },
        PushResponsable() {
            var Responsables = [
                new ClasesFn.Responsable({
                    UsuarioId: $('#slcResponsable1').val(),
                    Nombre: $('#txtNombreR1').val(),
                    Paterno: $('#txtPaternoR1').val(),
                    Materno: $('#txtMaternoR1').val(),
                    CargoId: $('#slcCargo1').val(),
            }),
                new ClasesFn.Responsable({
                    UsuarioId: $('#slcResponsable2').val(),
                    Nombre: $('#txtNombreR2').val(),
                    Paterno: $('#txtPaternoR2').val(),
                    Materno: $('#txtMaternoR2').val(),
                    CargoId: $('#slcCargo2').val(),
                    })
            ];

            if (Responsables[0].UsuarioId === Responsables[1].UsuarioId) {
                alertify.alert("Universidad YMCA", "No debe ser el mismo usuario. Favor de seleccionar otro.");
                return false;
            }

            TituloFn.AlumnoSelect.Responsables = Responsables;

            tblTitulos
                .row(TituloFn.RowSelect)
                .data(TituloFn.AlumnoSelect)
                .draw();
            
            $('#modalResponsables').modal('hide');
        },
        SedeChange() {
            $('#txtSedeClave').val($('#slcSede :selected').data("Clave"));

            TituloFn.SetOfertas(JSON.parse($('#slcSede :selected').data("Ofertas")), "slcOferta");
        },
        SedeChangePrev() {
            TituloFn.SetOfertas(JSON.parse($('#slcSedePrev :selected').data("Ofertas")), "slcOfertaPrev");
        },
        CargoChange() {
            var id = this.id;
            var lstResponsables = JSON.parse($('#' + id + ' :selected').data("Responsables"));
            var idslcres = "slcResponsable" + ((id).indexOf('1') === -1 ? "2" : "1");
            $('#' + idslcres).empty();
            
            $(lstResponsables).each(function () {
                var option = $(document.createElement('option'));

                option.val(this.UsuarioId);
                option.text(this.UsuarioId + "- " + this.Nombre);
                option.data('Nombre', this.Nombre);
                option.data('Paterno', this.Paterno);
                option.data('Materno', this.Materno);

                $("#" + idslcres).append(option);
            });

            id = ((id).indexOf('1') === -1 ? 1 : 0);

            if (TituloFn.AlumnoSelect.AlumnoId !== undefined) {
                if (TituloFn.AlumnoSelect.Responsables[id].UsuarioId !== undefined) {
                    if ($('#' + idslcres + " option[value='" + TituloFn.AlumnoSelect.Responsables[id].UsuarioId+"']").length !== 0) {
                        $('#' + idslcres).val(TituloFn.AlumnoSelect.Responsables[id].UsuarioId);
                    }                    
                }
            }

            $('#' + idslcres).change();
        },
        ResponsableChange() {
            
            var id = this.id;
            id = ((id).indexOf('1') === -1 ? "2" : "1");

            $('#txtNombreR' + id).val($('#' + this.id + ' :selected').data("Nombre"));
            $('#txtPaternoR' + id).val($('#' + this.id + ' :selected').data("Paterno"));
            $('#txtMaternoR' + id).val($('#' + this.id + ' :selected').data("Materno"));
        },
        SetCargo(lstCargo, idCargo, idset) {
            $('#' + idCargo).empty();

            $(lstCargo).each(function () {
                var option = $(document.createElement('option'));

                option.val(this.CargoId);
                option.text(this.Descripcion);
                option.data('Responsables', JSON.stringify(this.Responsables));

                $("#" + idCargo).val(idset);
                $("#" + idCargo).append(option);
            });

            $('#' + idCargo).change();
        },
        SetSede(lst, slcSede) {
            $("#" + slcSede).empty();

            $(lst).each(function () {
                var option = $(document.createElement('option'));

                option.val(this.SedeId);
                option.text(this.Nombre);
                option.data("Ofertas", JSON.stringify(this.Ofertas));
                option.data("Clave", this.Clave);

                $("#" + slcSede).append(option);
            });

            if (TituloFn.AlumnoSelect.AlumnoId !== undefined) {
                $('#' + slcSede).val(TituloFn.AlumnoSelect.Institucion.SedeId);
            }
            $('#' + slcSede).change();
        },
        SetOfertas(lst, slcoferta) {
            $('#' + slcoferta).empty();

            $(lst).each(function () {
                var option = $(document.createElement('option'));

                option.val(this.OfertaEducativaId);
                option.text(this.Descripcion);

                    option.data("Clave", this.ClaveOfertaEducativa);
                    option.data("RVOE", this.Rvoe);
                    option.data("FechaFin", this.FechaFin);
                option.data("FechaInicio", this.FechaInicio);

                $("#" + slcoferta).append(option);
            });

            if (TituloFn.AlumnoSelect.AlumnoId !== undefined && TituloFn.AlumnoSelect.Carrera.OfertaEducativaId !== undefined) {
                $('#' + slcoferta).val(TituloFn.AlumnoSelect.Carrera.OfertaEducativaId);
            }
            $('#' + slcoferta).change();
        },
        OfertaChange() {
            $('#txtClaveOferta').val($('#slcOferta :selected').data("Clave"));
            $('#txtRVOE').val($('#slcOferta :selected').data("RVOE"));
            TituloFn.UpdateFecha('txtFFinOferta', $('#slcOferta :selected').data("FechaFin"));
            TituloFn.UpdateFecha('txtFInicioOferta', $('#slcOferta :selected').data("FechaInicio"));
            
        },
        lstTitulos: [],       
        InitAlumnos() {

            tblTitulos = $('#tblTitulos')
                .DataTable({
                    "aaData": TituloFn.lstTitulos,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        {
                            "mDataProp": "",
                            "mRender": function (a,b,c) {
                                var Nombre = c.Nombre + ' ' + c.Paterno + ' ' + c.Materno;
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Datos' class='btn green'>Ver</buttton>";
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Institucion' class='btn blue'>Agregar</buttton>";
                                if (c.Carrera.FFin!=="01/01/1900") {
                                    Nombre = c.Institucion.Nombre + "  " + c.Carrera.OfertaEducativa + "  <button type='button' name='Institucion' class='bg-green'>" + "<i class='fa fa-edit'/>" + "</button>";
                                }
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "Titulo.MedioTitulacion",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Titulo' class='btn blue'>Agregar</buttton>";
                                var fecha = c.Titulo.MedioTitulacionId === 6 ? c.Titulo.FExencion : c.Titulo.FExamenProf;

                                if (fecha !== "01/01/1900") {
                                    Nombre = c.Titulo.MedioTitulacion + "  <button type='button' name='Titulo' class='bg-green'>" + "<i class='fa fa-edit'/>" + "</button>";
                                }
                                return Nombre;

                            }
                        },
                        {
                            "mDataProp": "Antecedente.Institucion",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Antecedente' class='btn blue'>Agregar</buttton>";
                                if (c.Antecedente.FechaFin !=="01/01/1900") {
                                    Nombre = c.Antecedente.Institucion+"  <button type='button' name='Antecedente' class='bg-green'>" + "<i class='fa fa-edit'/>" + "</button>";
                                }
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Responsable' class='btn blue'>Agregar</buttton>";
                                if (c.Responsables.length > 1
                                    && c.Responsables[0].UsuarioId !== undefined
                                    && c.Responsables[1].UsuarioId !== undefined) {
                                    Nombre = c.Responsables[0].Nombre + " - " +
                                        c.Responsables[1].Nombre + " <button type='button' class='bg-green' name='Responsable'><i class='fa fa-edit'/></button>";
                                }
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "",
                            "mRender": function (c, b, a) {
                                var estatus = false;

                                estatus = (a.Carrera.FFin === '01/01/1900'
                                    || (a.Titulo.MedioTitulacionId === 6
                                    ? a.Titulo.FExencion === '01/01/1900'
                                    : a.Titulo.FExamenProf === '01/01/1900')
                                    || a.Antecedente.FechaFin === '01/01/1900'
                                    || a.Responsables.length < 2) ? false : true;
                                

                                    var chk =  '<div class="md-checkbox-list">' +
                                        '<div class="md-checkbox">' +
                                        '<input type="checkbox" id="chkAlumno' + a.AlumnoId +
                                    '" class="md-check"' + (a.EstatusId === 2 ? "checked" : "") +
                                        (estatus ? '' :'disabled')+ '>' +
                                        '<label for="chkAlumno' + a.AlumnoId + '">' +
                                        '<span></span>' +
                                        '<span class="check"></span>' +
                                        '<span class="box"></span>' +
                                        'Seleccionar' +
                                        '</label>' +
                                        '</div>' +
                                        '</div>';

                                    return chk;
                            }
                        }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": true,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": true,
                    "bInfo": true,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "language": {
                        "lengthMenu": "_MENU_  Registros",
                        "paginate": {
                            "previous": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno "
                    }
                });
            
        },
        NameButton() {
            TituloFn.RowSelect = {};
            TituloFn.RowSelect = this.parentNode.parentNode;
            $(TituloFn.RowSelect).addClass('bg-purple');

            TituloFn.AlumnoSelect = new ClasesFn.AlumnoTitulo();
            TituloFn.AlumnoSelect = tblTitulos.row(TituloFn.RowSelect).data();

            switch (this.name) {
                case 'Institucion':
                    TituloFn.ShowSedeOferta();
                    break;
                case 'Titulo':
                    TituloFn.ShowTitulo();
                    break;
                case 'Antecedente':
                    TituloFn.ShowAntecedente();
                    break;
                case 'Responsable':
                    TituloFn.ShowResponsable();
                    break;
                case 'Datos':
                    TituloFn.ShowAlumno();
                    break;
            }
        },
        AlumnoSelect: new ClasesFn.AlumnoTitulo(),
        RowSelect: {},
        ShowTitulo() {
            $('#slcEntidadFederativa').val((TituloFn.AlumnoSelect.Titulo.EntidadFederativaId !== undefined ?
                TituloFn.AlumnoSelect.Titulo.EntidadFederativaId : -1));
            
            $('#slcServicio').val((TituloFn.AlumnoSelect.Titulo.FundamentoLegalId !== undefined ?
                TituloFn.AlumnoSelect.Titulo.FundamentoLegalId : 1));

            $('#slcMedioTitulacion').val((TituloFn.AlumnoSelect.Titulo.MedioTitulacionId !== undefined ?
                TituloFn.AlumnoSelect.Titulo.MedioTitulacionId : 1));

            TituloFn.UpdateFecha('txtFechaExencion', TituloFn.AlumnoSelect.Titulo.FExencion);
            TituloFn.UpdateFecha('txtFechaExamen', TituloFn.AlumnoSelect.Titulo.FExamenProf);

            $('#modalTitulo').modal('show');
        },
        ShowSedeOferta() {
            $('#modalSede input').val('');

            TituloFn.SetSede(TituloFn.AlumnoSelect.Sede, "slcSede");

            $('#modalSede').modal('show');
        },
        ShowAlumno() {
            $('#modalAlumno input').val('');
            $('#modalAlumno select').empty();

            TituloFn.SetSede(TituloFn.AlumnoSelect.Sede, "slcSedePrev");

            $('#txtAlumnoId').val(TituloFn.AlumnoSelect.AlumnoId);
            $('#txtNombre').val(TituloFn.AlumnoSelect.Nombre);
            $('#txtPaterno').val(TituloFn.AlumnoSelect.Paterno);
            $('#txtMaterno').val(TituloFn.AlumnoSelect.Materno);
            $('#txtCURP').val(TituloFn.AlumnoSelect.CURP);
            $('#txtEmail').val(TituloFn.AlumnoSelect.Email);

            $('#btnAlumnoAdd').hide();
            $('#modalAlumno').modal('show');            
        },
        ShowAntecedente() {
            $('#modalAntecedente input').val('');

            $('#slcTAntecedente').val((TituloFn.AlumnoSelect.Antecedente.TipoAntecedenteId === undefined ? 1
                : TituloFn.AlumnoSelect.Antecedente.TipoAntecedenteId));

            $('#slcEntidadAntecedente').val((TituloFn.AlumnoSelect.Antecedente.EntidadFederativaId === undefined ? -1
                : TituloFn.AlumnoSelect.Antecedente.EntidadFederativaId));

            $('#txtProcedencia').val(TituloFn.AlumnoSelect.Antecedente.Institucion);
            TituloFn.UpdateFecha('txtFechaFin', TituloFn.AlumnoSelect.Antecedente.FechaFin);
            TituloFn.UpdateFecha('txtFechaInicio', TituloFn.AlumnoSelect.Antecedente.FechaInicio);

            $('#modalAntecedente').modal('show');
        },
        ShowResponsable() {

            if (TituloFn.AlumnoSelect.Responsables.length > 0) {
                if (TituloFn.AlumnoSelect.Responsables[0].CargoId !== undefined) {
                    $('#slcCargo1').val(TituloFn.AlumnoSelect.Responsables[0].CargoId);
                    $('#slcCargo1').change();
                }
            }
            if (TituloFn.AlumnoSelect.Responsables.length > 1) {
                if (TituloFn.AlumnoSelect.Responsables[1].CargoId !== undefined) {
                    $('#slcCargo2').val(TituloFn.AlumnoSelect.Responsables[1].CargoId);
                    $('#slcCargo2').change();
                }
            }
            $('#modalResponsables').modal('show');
        },
        Enviar(Alumno) {

            IndexFn.Block(true);
            IndexFn.Api("SEP/Nuevo", 'PUT', JSON.stringify(Alumno))
                .done(function (data) {                    
                    IndexFn.Block(false);
                    if (!data.Status) {
                        alertify.alert("Universidad YMCA", "No se pudo guardar.");                       
                    } else {
                        alertify.alert("Universidad YMCA", "Alumno Agregado");
                        TituloFn.GetSolicitados(Alumno.AlumnoId);
                    }
                })
                .fail(function (data) {
                    alertify.alert("Universidad YMCA", "Fallo al momento de guardar.");
                    var result = JSON.parse(data.responseText).Message + "'";
                    console.log(result);
                    IndexFn.Block(false);
                });
        },
        UpdateAlumnos() {

            var AlumnosUP = [];

            $(tblTitulos.rows().data()).each(function () {
                AlumnosUP.push(this);
            });
            IndexFn.Block(true);

            IndexFn.Api("SEP/Alumnos/Update", "Post", JSON.stringify(AlumnosUP))
                .done(function (data) {
                    IndexFn.Block(false);
                    if (data.fallidos.length === 0) {
                        alertify.alert("Alumnos Actualizados");
                        TituloFn.GetSolicitados();
                    } else {
                        alertify.alert("Error al actualizar los datos de los alumnos.")
                        TituloFn.lstTitulos = [];

                        $(data.fallidos).each(function () {
                            TituloFn.lstTitulos.push(this.Alumno);
                        });

                        TituloFn.InitAlumnos();
                    }
                })
                .fail(function (data) {
                    alertify.alert("Error al actualizar los datos de los alumnos.")
                    IndexFn.Block(false);
                });
        },
        GetSolicitados(AlumnoId) {    
            IndexFn.Block(true);
            IndexFn.Api("Sep/Alumnos/Espera/" + AlumnoId, "GET", "")
                .done(function (data) {
                    TituloFn.lstTitulos = [];
                    $(tblTitulos.rows().data()).each(function () {
                        TituloFn.lstTitulos.push(this);
                    });

                    $(data).each(function () {
                        var alumnobd = this;
                        this.Institucion.SedeId = this.Institucion.InstitucionId;
                        alumnobd.UsuarioId = localStorage.getItem('userAdmin');
                        alumnobd.Sede=[];
                        alumnobd.Sede.push({
                            SedeId: this.Institucion.SedeId,
                            Nombre: this.Institucion.Nombre,
                            Clave: this.Institucion.Clave,
                            Ofertas: [{
                                OfertaEducativaId: this.Carrera.OfertaEducativaId,
                                ClaveOfertaEducativa: this.Carrera.Clave,
                                Descripcion: this.Carrera.OfertaEducativa,
                                OfertaEducativa: this.Carrera.OfertaEducativa,
                                Rvoe: this.Carrera.RVOE,
                                FechaFin: this.Carrera.FFin,
                                FechaInicio: this.Carrera.FInicio
                            }]
                        });

                        TituloFn.lstTitulos.push(alumnobd);
                    });
                    
                    TituloFn.InitAlumnos();
                    IndexFn.Block(false);
                })
                .fail(function (data) {
                    console.log(data);
                    IndexFn.Block(false);
                });
        }
    };

    TituloFn.init();
});