$(function () {
    var tblTitulos;
    
    var ClasesFn = {
        AlumnoTitulo: class {
            constructor(Alumno, objins, objTit, objCa, objAnt, lstUsuarios, lstSedes) {
                Alumno = Alumno || {};
                objins = objins || {};
                objTit = objTit || {};
                objCa = objCa || {};
                objAnt = objAnt || {};
                lstUsuarios = lstUsuarios || {};
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
                this.UsuarioId = Alumno.UsuarioId;
                this.Comentario = Alumno.Comentario;
                this.Autorizado = Alumno.Autorizado;
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
                this.FundamentoLegal = obj.FundamentoLegal;
                this.EntidadFederativaId = obj.EntidadFederativaId;
                this.EntidadFederativa = obj.EntidadFederativa;
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
                this.AutReconocimiento = obj.AutReconocimiento;
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
                this.EntidadFederativa = obj.EntidadFederativa;
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
                this.Cargo = obj.Cargo;
            }
        }
    };

    var FirmarFn = {
        init() {
            
            $('#tblTitulos').on('click', 'button', this.NameButton);
            $('#tblTitulos').on('click', 'input', this.NameButton);
            $('#btnGuardar').on('click', this.Enviar);
            $('#btnComentario').on('click', this.Comentario);
            $('#btnQuitar').on('click', this.SetComentario);
            $('#slcMedioTitulacion').on('change', this.MedioChange); 

            this.GetAlumnos();
            tblTitulos = $('#tblTitulos').DataTable();
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
        GetAlumnos() {
            IndexFn.Block(true);
            IndexFn.Api("SEP/Alumnos/" + localStorage.getItem('userAdmin'), "GET", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    FirmarFn.lstTitulos = data;
                    FirmarFn.InitAlumnos();
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                });
        },
        UpdateSelect(obj, selectId) {
            $("#" + selectId).empty();

            var option = $(document.createElement('option'));

            option.val(obj.Id);
            option.text(obj.Descripcion);

            $("#" + selectId).append(option);
        },
        lstTitulos: [],
        InitAlumnos() {

            tblTitulos = $('#tblTitulos')
                .DataTable({
                    "aaData": FirmarFn.lstTitulos,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
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
                                if (c.Carrera.OfertaEducativaId > 0 && c.Carrera.AutReconocimientoId !== 0) {
                                    Nombre = c.Institucion.Nombre + "  " + c.Carrera.OfertaEducativa + "  <button type='button' name='Institucion' class='bg-green'>" + "<i class='fa fa-edit'/>" + "</button>";
                                }
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "Titulo.MedioTitulacion",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Titulo' class='btn blue'>Agregar</buttton>";
                                if (c.Titulo.MedioTitulacionId > 0) {
                                    Nombre = c.Titulo.MedioTitulacion + "  <button type='button' name='Titulo' class='bg-green'>" + "<i class='fa fa-edit'/>" + "</button>";
                                }
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "Antecedente.Institucion",
                            "mRender": function (a, b, c) {
                                var Nombre = "<button type='button' name='Antecedente' class='btn blue'>Agregar</buttton>";
                                if (c.Antecedente.TipoAntecedenteId > 0) {
                                    Nombre = c.Antecedente.Institucion + "  <button type='button' name='Antecedente' class='bg-green'>" + "<i class='fa fa-edit'/>" + "</button>";
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
                            "mRender": function (a, b, c) {
                                //var elim = "<a href='javascript:;'><i class='fa fa-edit'/></a> ";
                                //return elim;
                                var input = c.Autorizado ? ('<input type="checkbox" id="' + c.AlumnoId + c.Carrera.OfertaEducativaId + '" class="md-check" checked name="Quitar">') :
                                ('<input type="checkbox" id="' + c.AlumnoId + c.Carrera.OfertaEducativaId + '" class="md-check" name="Quitar">');

                                var Nombre = '<div class="md-checkbox-list">' +
                                                '<div class="md-checkbox">' +
                                                    input +
                                                    '<label for="' + c.AlumnoId + c.Carrera.OfertaEducativaId + '" >' +
                                                        '<span></span>' +
                                                        '<span class="check"></span>' +
                                                        '<span class="box"></span>' +
                                                        'Aturorizado' +
                                                    '</label>' +
                                                '</div>';
                                return Nombre;
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
        NameButton(e) {
            e.preventDefault();
            FirmarFn.RowSelect = {};
            FirmarFn.RowSelect =this.name==="Quitar" ?
                this.parentNode.parentNode.parentNode
                :this.parentNode.parentNode;

            FirmarFn.AlumnoSelect = new ClasesFn.AlumnoTitulo();
            FirmarFn.AlumnoSelect = tblTitulos.row(FirmarFn.RowSelect).data();

            switch (this.name) {
                case 'Institucion':
                    FirmarFn.ShowSedeOferta();
                    break;
                case 'Titulo':
                    FirmarFn.ShowTitulo();
                    break;
                case 'Antecedente':
                    FirmarFn.ShowAntecedente();
                    break;
                case 'Responsable':
                    FirmarFn.ShowResponsable();
                    break;
                case 'Datos':
                    FirmarFn.ShowAlumno();
                    break;
                case 'Quitar':

                    FirmarFn.Borrar();
                    break;
            }
        },
        AlumnoSelect: new ClasesFn.AlumnoTitulo(),
        RowSelect: {},
        SetComentario(){
            if ($('#txtComentario').val().length < 5) {
                alertify.alert("Debe ingresar un comentario.");
                return false;
            }
            FirmarFn.AlumnoSelect.Comentario = $('#txtComentario').val();
            FirmarFn.AlumnoSelect.Autorizado = false;

            tblTitulos
               .row(FirmarFn.RowSelect)
               .data(FirmarFn.AlumnoSelect)
               .draw();

            $('#modalComentario').modal("hide");
        },
        ShowTitulo() {
            FirmarFn.UpdateSelect({
                Descripcion: FirmarFn.AlumnoSelect.Titulo.EntidadFederativa,
                Id: FirmarFn.AlumnoSelect.Titulo.EntidadFederativaId
            }, 'slcEntidadFederativa');

            FirmarFn.UpdateSelect({
                Descripcion: FirmarFn.AlumnoSelect.Titulo.FundamentoLegal,
                Id: FirmarFn.AlumnoSelect.Titulo.FundamentoLegalId
            }, 'slcServicio');

            FirmarFn.UpdateSelect({
                Descripcion: FirmarFn.AlumnoSelect.Titulo.MedioTitulacion,
                Id: FirmarFn.AlumnoSelect.Titulo.MedioTitulacionId
            }, 'slcMedioTitulacion');

            $('#slcMedioTitulacion').change();

            $('#txtFechaExencion').val(FirmarFn.AlumnoSelect.Titulo.FExencion);
            $('#txtFechaExamen').val(FirmarFn.AlumnoSelect.Titulo.FExamenProf);

            $('#modalTitulo').modal('show');
        },
        ShowSedeOferta() {
            $('#modalSede input').val('');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Institucion.InstitucionId,
                Descripcion: FirmarFn.AlumnoSelect.Institucion.Nombre
            }, 'slcSede');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Carrera.OfertaEducativaId,
                Descripcion: FirmarFn.AlumnoSelect.Carrera.OfertaEducativa
            }, 'slcOferta');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Carrera.AutReconocimientoId,
                Descripcion: FirmarFn.AlumnoSelect.Carrera.AutReconocimiento
            }, 'slcAutRec');

            $('#txtSedeClave').val(FirmarFn.AlumnoSelect.Institucion.Clave);
            $('#txtClaveOferta').val(FirmarFn.AlumnoSelect.Carrera.Clave);
            $('#txtRVOE').val(FirmarFn.AlumnoSelect.Carrera.RVOE);

            $('#txtFInicioOferta').val(FirmarFn.AlumnoSelect.Carrera.FInicio);
            $('#txtFFinOferta').val(FirmarFn.AlumnoSelect.Carrera.FFin);

            $('#modalSede').modal('show');
        },
        ShowAlumno() {
            $('#modalAlumno input').val('');
            $('#modalAlumno select').empty();

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Institucion.InstitucionId,
                Descripcion: FirmarFn.AlumnoSelect.Institucion.Nombre
            }, 'slcSedePrev');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Carrera.OfertaEducativaId,
                Descripcion: FirmarFn.AlumnoSelect.Carrera.OfertaEducativa
            }, 'slcOfertaPrev');

            $('#txtAlumnoId').val(FirmarFn.AlumnoSelect.AlumnoId);
            $('#txtNombre').val(FirmarFn.AlumnoSelect.Nombre);
            $('#txtPaterno').val(FirmarFn.AlumnoSelect.Paterno);
            $('#txtMaterno').val(FirmarFn.AlumnoSelect.Materno);
            $('#txtCURP').val(FirmarFn.AlumnoSelect.CURP);
            $('#txtEmail').val(FirmarFn.AlumnoSelect.Email);

            $('#btnAlumnoAdd').hide();
            $('#modalAlumno').modal('show');
        },
        ShowAntecedente() {
            $('#modalAntecedente input').val('');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Antecedente.TipoAntecedenteId,
                Descripcion: FirmarFn.AlumnoSelect.Antecedente.TipoAntecedente
            }, 'slcTAntecedente');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Antecedente.EntidadFederativaId,
                Descripcion: FirmarFn.AlumnoSelect.Antecedente.EntidadFederativa
            }, 'slcEntidadAntecedente');

            $('#txtProcedencia').val(FirmarFn.AlumnoSelect.Antecedente.Institucion);
            $('#txtFechaFin').val(FirmarFn.AlumnoSelect.Antecedente.FechaFin);
            $('#txtFechaInicio').val(FirmarFn.AlumnoSelect.Antecedente.FechaInicio);

            $('#modalAntecedente').modal('show');
        },
        ShowResponsable() {
            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Responsables[0].CargoId,
                Descripcion: FirmarFn.AlumnoSelect.Responsables[0].Cargo
            }, 'slcCargo1');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Responsables[0].UsuarioId,
                Descripcion: FirmarFn.AlumnoSelect.Responsables[0].UsuarioId + " - " + FirmarFn.AlumnoSelect.Responsables[0].Nombre
            }, 'slcResponsable1');

            $('#txtNombreR1').val(FirmarFn.AlumnoSelect.Responsables[0].Nombre);
            $('#txtPaternoR1').val(FirmarFn.AlumnoSelect.Responsables[0].Paterno);
            $('#txtMaternoR1').val(FirmarFn.AlumnoSelect.Responsables[0].Materno);

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Responsables[1].CargoId,
                Descripcion: FirmarFn.AlumnoSelect.Responsables[1].Cargo
            }, 'slcCargo2');

            FirmarFn.UpdateSelect({
                Id: FirmarFn.AlumnoSelect.Responsables[1].UsuarioId,
                Descripcion: FirmarFn.AlumnoSelect.Responsables[1].UsuarioId + " - " + FirmarFn.AlumnoSelect.Responsables[1].Nombre
            }, 'slcResponsable2');

            $('#txtNombreR2').val(FirmarFn.AlumnoSelect.Responsables[1].Nombre);
            $('#txtPaternoR2').val(FirmarFn.AlumnoSelect.Responsables[1].Paterno);
            $('#txtMaternoR2').val(FirmarFn.AlumnoSelect.Responsables[1].Materno);

            $('#modalResponsables').modal('show');
        },
        Borrar() {
            if (!FirmarFn.AlumnoSelect.Autorizado) {
                FirmarFn.AlumnoSelect.Autorizado = true;
                FirmarFn.AlumnoSelect.Comentario = "";

                tblTitulos
               .row(FirmarFn.RowSelect)
               .data(FirmarFn.AlumnoSelect)
               .draw();
            } else {
                alertify.confirm(("El alumno quedara pendiente de su firma." + FirmarFn.AlumnoSelect.AlumnoId + "-" + FirmarFn.AlumnoSelect.Nombre),
                    function (e) {
                        if (e) {
                            $('#txtComentario').val('');
                            $('#modalComentario').modal("show");
                        }
                    });
            }
        },
        Comentario() {
            if ($('#txtComentario').val().length < 10) {
                alertify.alert("Universidad YMCA", "Minimo 10 letras.");
                return false;
            }
            FirmarFn.AlumnoSelect.Comentario = $('#txtComentario').val();

            tblTitulos
                .row(FirmarFn.RowSelect)
                .data(FirmarFn.AlumnoSelect)
                .draw();

            $('#modalComentario').modal("hide");
        },
        Enviar() {
            var alumnosadd = [];
            $(tblTitulos.rows().data()).each(function () {
                this.UsuarioId = localStorage.getItem('userAdmin');
                alumnosadd.push(this);
            });

            if (alumnosadd.length === 0) { return false; }
            IndexFn.Block(true);

            IndexFn.Api("SEP/Firmar", 'PUT', JSON.stringify(alumnosadd))
                .done(function (data) {
                    IndexFn.Block(false);
                    FirmarFn.lstTitulos = [];

                    if (data.fallidos.length === 0) {
                        alertify.alert("Universidad YMCA", "Alumnos enviados a los responsables.");
                    } else {
                        $(data.fallidos).each(function () {
                            FirmarFn.lstTitulos.push(this.Alumno);
                        });
                        alertify.alert("Universidad YMCA", "Los siguientes alumnos no se pudieron guardar.");
                    }

                    FirmarFn.InitAlumnos();
                })
                .fail(function (data) {
                    alertify.alert("Universidad YMCA", "Fallo al momento de guardar.");
                    var result = JSON.parse(data.responseText).Message + "'";
                    console.log(result);
                    IndexFn.Block(false);
                });
        }
    };

    FirmarFn.init();
});