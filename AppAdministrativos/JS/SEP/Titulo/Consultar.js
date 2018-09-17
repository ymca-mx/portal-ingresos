$(function () {

    var tblTitulos,
        tblDetalles;
    
    var TituloFn = {
        init() {

            $('#tblTitulos').on('click', 'button', this.NameButton);
            $('#tblTitulos').on('click', 'input', this.NameButton);
            $('#btnGuardar').on('click', this.Enviar);
            $('#slcMedioTitulacion').on('change', this.MedioChange);
            $('#slcOption').on('change', this.OptionChange);

            this.GetAlumnos();
            tblTitulos = $('#tblTitulos').DataTable();
        },
        OptionChange() {

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
            IndexFn.Api("SEP/Alumnos/Firmados", "GET", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    TituloFn.lstTitulos = data;
                    TituloFn.InitAlumnos();
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
                    "aaData": TituloFn.lstTitulos,
                    "bSort": false,
                    "aoColumns": [
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
                                var Nombre = c.AlumnoId + ' | ' + c.Nombre + ' ' + c.Paterno + ' ' + c.Materno;
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
                                var Nombre = "<a href='" + c.Archivo + "'target='_blank' class='btn bg-blue'>Archivo</a>";
                                return Nombre;
                            }
                        },
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
                                var Nombre = "";
                                if (c.EstatusId === 3) {
                                    var input = c.Autorizado ? ('<input type="checkbox" id="' + c.AlumnoId + c.Carrera.OfertaEducativaId + '" class="md-check" checked name="Quitar">') :
                                        ('<input type="checkbox" id="' + c.AlumnoId + c.Carrera.OfertaEducativaId + '" class="md-check" name="Quitar">');

                                    Nombre = '<div class="md-checkbox-list">' +
                                        '<div class="md-checkbox">' +
                                        input +
                                        '<label for="' + c.AlumnoId + c.Carrera.OfertaEducativaId + '" >' +
                                        '<span></span>' +
                                        '<span class="check"></span>' +
                                        '<span class="box"></span>' +
                                        'Enviar' +
                                        '</label>' +
                                        '</div>';
                                } else {
                                    Nombre = "<a href='javascript:;' class='btn bg-green' name='Detalle'><span>" + c.EstatusSEP + "</span></a>";
                                }

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
            TituloFn.RowSelect = {};
            TituloFn.RowSelect = this.name === "Quitar" ?
                this.parentNode.parentNode.parentNode
                : this.parentNode.parentNode;

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
                case 'Quitar':
                    TituloFn.Borrar();
                    break;
                case 'Nombre':
                    TituloFn.ShowDetails();
                    break;
            }
        },
        AlumnoSelect: new ClasesFn.AlumnoTitulo(),
        RowSelect: {},
        ShowTitulo() {
            TituloFn.UpdateSelect({
                Descripcion: TituloFn.AlumnoSelect.Titulo.EntidadFederativa,
                Id: TituloFn.AlumnoSelect.Titulo.EntidadFederativaId
            }, 'slcEntidadFederativa');

            TituloFn.UpdateSelect({
                Descripcion: TituloFn.AlumnoSelect.Titulo.FundamentoLegal,
                Id: TituloFn.AlumnoSelect.Titulo.FundamentoLegalId
            }, 'slcServicio');

            TituloFn.UpdateSelect({
                Descripcion: TituloFn.AlumnoSelect.Titulo.MedioTitulacion,
                Id: TituloFn.AlumnoSelect.Titulo.MedioTitulacionId
            }, 'slcMedioTitulacion');

            $('#slcMedioTitulacion').change();

            $('#txtFechaExencion').val(TituloFn.AlumnoSelect.Titulo.FExencion);
            $('#txtFechaExamen').val(TituloFn.AlumnoSelect.Titulo.FExamenProf);

            $('#modalTitulo').modal('show');
        },
        ShowSedeOferta() {
            $('#modalSede input').val('');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Institucion.InstitucionId,
                Descripcion: TituloFn.AlumnoSelect.Institucion.Nombre
            }, 'slcSede');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Carrera.OfertaEducativaId,
                Descripcion: TituloFn.AlumnoSelect.Carrera.OfertaEducativa
            }, 'slcOferta');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Carrera.AutReconocimientoId,
                Descripcion: TituloFn.AlumnoSelect.Carrera.AutReconocimiento
            }, 'slcAutRec');

            $('#txtSedeClave').val(TituloFn.AlumnoSelect.Institucion.Clave);
            $('#txtClaveOferta').val(TituloFn.AlumnoSelect.Carrera.Clave);
            $('#txtRVOE').val(TituloFn.AlumnoSelect.Carrera.RVOE);

            $('#txtFInicioOferta').val(TituloFn.AlumnoSelect.Carrera.FInicio);
            $('#txtFFinOferta').val(TituloFn.AlumnoSelect.Carrera.FFin);

            $('#modalSede').modal('show');
        },
        ShowAlumno() {
            $('#modalAlumno input').val('');
            $('#modalAlumno select').empty();

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Institucion.InstitucionId,
                Descripcion: TituloFn.AlumnoSelect.Institucion.Nombre
            }, 'slcSedePrev');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Carrera.OfertaEducativaId,
                Descripcion: TituloFn.AlumnoSelect.Carrera.OfertaEducativa
            }, 'slcOfertaPrev');

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

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Antecedente.TipoAntecedenteId,
                Descripcion: TituloFn.AlumnoSelect.Antecedente.TipoAntecedente
            }, 'slcTAntecedente');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Antecedente.EntidadFederativaId,
                Descripcion: TituloFn.AlumnoSelect.Antecedente.EntidadFederativa
            }, 'slcEntidadAntecedente');

            $('#txtProcedencia').val(TituloFn.AlumnoSelect.Antecedente.Institucion);
            $('#txtFechaFin').val(TituloFn.AlumnoSelect.Antecedente.FechaFin);
            $('#txtFechaInicio').val(TituloFn.AlumnoSelect.Antecedente.FechaInicio);

            $('#modalAntecedente').modal('show');
        },
        ShowResponsable() {
            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Responsables[0].CargoId,
                Descripcion: TituloFn.AlumnoSelect.Responsables[0].Cargo
            }, 'slcCargo1');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Responsables[0].UsuarioId,
                Descripcion: TituloFn.AlumnoSelect.Responsables[0].UsuarioId + " - " + TituloFn.AlumnoSelect.Responsables[0].Nombre
            }, 'slcResponsable1');

            $('#txtNombreR1').val(TituloFn.AlumnoSelect.Responsables[0].Nombre);
            $('#txtPaternoR1').val(TituloFn.AlumnoSelect.Responsables[0].Paterno);
            $('#txtMaternoR1').val(TituloFn.AlumnoSelect.Responsables[0].Materno);

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Responsables[1].CargoId,
                Descripcion: TituloFn.AlumnoSelect.Responsables[1].Cargo
            }, 'slcCargo2');

            TituloFn.UpdateSelect({
                Id: TituloFn.AlumnoSelect.Responsables[1].UsuarioId,
                Descripcion: TituloFn.AlumnoSelect.Responsables[1].UsuarioId + " - " + TituloFn.AlumnoSelect.Responsables[1].Nombre
            }, 'slcResponsable2');

            $('#txtNombreR2').val(TituloFn.AlumnoSelect.Responsables[1].Nombre);
            $('#txtPaternoR2').val(TituloFn.AlumnoSelect.Responsables[1].Paterno);
            $('#txtMaternoR2').val(TituloFn.AlumnoSelect.Responsables[1].Materno);

            $('#modalResponsables').modal('show');
        },
        ShowDetails() {

            tblDetalles = $('#tblDetalles')
                .DataTable({
                    "aaData": TituloFn.AlumnoSelect.AccioSEP,
                    "bSort": false,
                    "aoColumns": [
                        {
                            "mDataProp": "",
                            "mRender": function (a, b, c) {
                                var Nombre = c.UsuarioId + ' | ' + c.NombreUsuario;
                                return Nombre;
                            }
                        },
                        { "mDataProp": "NumeroLote" },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "Mensaje" },
                        { "mDataProp": "Fecha" },
                        { "mDataProp": "Hora" },
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": false,
                    "ordering": false,
                    "async": false,
                    "bDestroy": true,
                    "bPaginate": false,
                    "bLengthChange": true,
                    "bFilter": false,
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


            $('#modalDetails').modal('show');
        },
        Borrar() {
            if (!TituloFn.AlumnoSelect.Autorizado) {
                TituloFn.AlumnoSelect.Autorizado = true;

                tblTitulos
                    .row(TituloFn.RowSelect)
                    .data(TituloFn.AlumnoSelect)
                    .draw();
            } else {
                alertify.confirm(("No se enviara el archivo del alumno a la SEP, ¿Desea continuar?" + TituloFn.AlumnoSelect.AlumnoId + "-" + TituloFn.AlumnoSelect.Nombre),
                    function (e) {
                        if (e) {
                            TituloFn.AlumnoSelect.Autorizado = false;

                            tblTitulos
                                .row(TituloFn.RowSelect)
                                .data(TituloFn.AlumnoSelect)
                                .draw();
                        }
                    });
            }
        },
        Enviar() {
            var alumnosadd = [];
            $(tblTitulos.rows().data()).each(function () {
                if (this.EstatusId === 3 && this.Autorizado) {
                    this.UsuarioId = localStorage.getItem('userAdmin');
                    alumnosadd.push(this);
                }
            });

            if (alumnosadd.length === 0) { return false; }
            IndexFn.Block(true);

            IndexFn.Api("SEP/Enviar", 'PUT', JSON.stringify(alumnosadd))
                .done(function (data) {
                    IndexFn.Block(false);
                    TituloFn.lstTitulos = [];

                    if (data.fallidos !== undefined) {
                        if (data.fallidos.length === 0) {
                            alertify.alert("Universidad YMCA", "Se enviaron satisfactoriamente los archivos a la SEP.");
                            tblTitulos
                                .clear()
                                .draw();

                            TituloFn.GetAlumnos();
                        } else {
                            $(data.fallidos).each(function () {
                                TituloFn.lstTitulos.push(this.Alumno);
                            });
                            alertify.alert("Universidad YMCA", "Los siguientes alumnos no se pudieron guardar.");
                        }

                        TituloFn.InitAlumnos();
                    } else {
                        alertify.alert("Universidad YMCA", "Ocurrio un error al enviar los archivos a la SEP.");

                        tblTitulos
                            .clear()
                            .draw();

                        TituloFn.GetAlumnos();
                    }
                })
                .fail(function (data) {
                    alertify.alert("Universidad YMCA", "Fallo al momento de guardar.");
                    var result = JSON.parse(data.responseText).Message + "'";
                    console.log(result);
                    IndexFn.Block(false);
                });
        }
    };

    TituloFn.init();
});