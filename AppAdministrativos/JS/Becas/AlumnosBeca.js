$(document).ready(function init() {
    var AlumnoId;
    var tblAlumnosPActual = null, tblPagos = null, tblBecas = null, tblAlumnos = null;
    var Estado = "";
    var UsurioTipo = 0, NuevoIngreso = 0;
    var TieneSEP = false, TieneComite = false, EsEspecial = false, esEmpresa, EsCompleto = true;
    var objAlumno = undefined;
    $("#chkSEP").bootstrapSwitch({
        size:"small"
    });

    var Funciones = {
        CrearEnlace: function () {
            $('#chkSEP').on('switchChange.bootstrapSwitch', Funciones.chkSEPonchange);
            $('#btnCargosBeca').on('click', Funciones.btnCargosBecaonClick);
            $('#btnRegresar').on('click', Funciones.btnRegresaronClcik);
            $("#tblBecas").on('click', 'a', Funciones.tblBecasonClicka);
            $('#tbAlumnos').on('click', 'a', Funciones.tbAlumnosonClicka);
            $('#btnBuscarAlumno').on('click', Funciones.btnBuscarAlumnoonClick);
            $('#btnClosedivOfertas').on('click', Funciones.btnClosedivOfertasonClick);
            $('#divOfertaAlumno').on('click', 'a', Funciones.divOfertaAlumnoonClicka);
            $("#txtBecaMonto").focus(Funciones.txtBecaMontoFocus);
            $('#txtBecaMonto').keyup(Funciones.txtBecaMontoKeyUp);
            $('#btnGenerarCargos').on('click', Funciones.btnGenerarCargosonClick);
            $('#CartaArchivo').bind('change', Funciones.CartaArchivoBindChange);
            $('#FileCarta a').on('click', Funciones.FileCartaAOnClick);
            $('#txtAlumno').on('keydown', Funciones.txtAlumnoonKeyDown);
            $('#tblAlumnos').on('click', 'a', Funciones.TablaAlumnoClick);
            $('#slcDescripcionBeca').on('change', Funciones.slcDescripcionBecaChange);
        },
        GetUsuario: function () {
            var usuario = $.cookie('userAdmin');
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ObtenerUsuario",
                data: "{UsuarioId:'" + usuario + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d !== null) {
                        UsurioTipo = data.d.UsuarioTipoId;
                    }
                }
            });
        },
        CargarPeriodo: function (data) {                    
            if (data.length > 0) {
                var Desactiva = false;

                $(data).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.Anio + '' + this.PeriodoId);
                    option.attr("data-Anio", this.Anio);
                    option.attr("data-PeriodoId", this.PeriodoId);
                    if (this.SolicitudInscripcion !== null) {
                        Desactiva = Desactiva == true ? true : true;
                        option.attr("data-Observacion",
                            this.SolicitudInscripcion.SolicitudUsuarioId + "|" +
                            this.SolicitudInscripcion.SolicitudNombreUsuario + ": " +
                            this.SolicitudInscripcion.Observaciones);
                    }

                    $('#slcDescripcionBeca').append(option);

                  
                });

                if (Desactiva) {
                    $('#slcDescripcionBeca').removeAttr("disabled");
                }

                $('#slcDescripcionBeca').val(data[0].Anio + '' + data[0].PeriodoId);
                Funciones.slcDescripcionBecaChange();
            }
        },
        slcDescripcionBecaChange: function () {
            $('#lblPeriodoBeca').text('');
            $('#divObservaciones').hide();

            var textbutton = $('#btnGenerarCargos').data('texto');
            if (textbutton !== undefined) {
                $('#btnGenerarCargos').text(textbutton);
                var estadoDisabled = $('#btnGenerarCargos').data('disabled');
                if (estadoDisabled !== undefined || estadoDisabled === true) {
                    $('#btnGenerarCargos').attr('disabled',"disaled");
                }
            }
            
            var Anio = $('#slcDescripcionBeca').find(':selected').data("anio");
            var PeriodoId = $('#slcDescripcionBeca').find(':selected').data("periodoid");
            var Observaciones = $('#slcDescripcionBeca').find(':selected').data("observacion");

            $('#lblPeriodoBeca').text(Anio + ' ' + PeriodoId);
            $('#txtPeriodo').val(Anio + ' ' + PeriodoId);

            if (Observaciones !== undefined) {
                $('#divObservaciones').show();
                $('#txtObservacioes').val(Observaciones);

                $('#btnGenerarCargos').data('texto', $('#btnGenerarCargos').text());
                $('#btnGenerarCargos').text("Reinscribir");

                var estado = $('#btnGenerarCargos').attr('disabled');
                $('#btnGenerarCargos').data('disabled', (estado === undefined ? false : true));
                $("#btnGenerarCargos").removeAttr("disabled");
            }
        },
        CargarDescuentos: function () {
            var OfertaEducativa = $('#txtOfertaEducativa').data("ofertaid");
            $.ajax({
                url: 'WS/Beca.asmx/DescuentosAnteriores',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data.d.length > 0) {
                        if (tblBecas !== null) {
                            $('#txtBecaMonto').val("0");
                            tblBecas.fnClearTable();
                        }

                        var objUltimo = data.d[data.d.length - 1];
                        if (objUltimo !== undefined && objUltimo.AnioPeriodoId === $('#lblPeriodoBeca').text()) {
                            objAlumno = objUltimo;
                        }
                        var monto = EsEspecial ? data.d[data.d.length - 1].OtrosDescuentos : data.d[data.d.length - 1].SMonto;
                        var smonto = 0;
                        if (monto[monto.length - 1] === '%') {
                            smonto = monto.substring(0, monto.length - 1);
                            $('#txtBecaMonto').val(smonto);
                        } else {
                            $('#txtBecaMonto').val(monto);
                        }
                        $('#txtBecaMonto').keyup();
                        $("#txtBecaMonto").focus();

                        tblBecas = $("#tblBecas").dataTable({
                            "aaData": data.d,
                            "aoColumns": [
                                { "mDataProp": "AnioPeriodoId" },
                                { "mDataProp": "DescripcionPeriodo" },
                                { "mDataProp": "SMonto" },
                                { "mDataProp": "BecaDeportiva" },
                                { "mDataProp": "OtrosDescuentos" },
                                { "mDataProp": "BecaSEP" },
                                { "mDataProp": "BecaComite" },
                                { "mDataProp": "Usuario.Nombre" },
                                { "mDataProp": "FechaAplicacionS" }
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": false,
                            "ordering": false,
                            "async": true,
                            "bDestroy": true,
                            "bPaginate": false,
                            "bLengthChange": false,
                            "bFilter": false,
                            "bInfo": false,
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
                            "order": [[2, "desc"]],
                            "createdRow": function (row, data, dataIndex) {
                                row.childNodes[1].style.textAlign = 'center';
                                row.childNodes[2].style.textAlign = 'center';
                                row.childNodes[3].style.textAlign = 'center';
                                row.childNodes[4].style.textAlign = 'center';

                                if (data.DocComiteRutaId.length > 0) {
                                    if (data.DocComiteRutaId !== "-1") {
                                        var newLinkS = $(document.createElement("a"));
                                        newLinkS[0].innerText = "Archivo...";
                                        newLinkS.attr('data-file', data.DocComiteRutaId);

                                        var colComite = row.childNodes[6];
                                        colComite.innerHTML += "<br />" + newLinkS[0].outerHTML;
                                    }
                                }
                                if (data.DocAcademicaId.length > 0) {
                                    if (data.DocAcademicaId !== "-1") {
                                        var newLinkA = $(document.createElement("a"));
                                        newLinkA[0].innerText = "Archivo...";
                                        newLinkA.attr('data-file', data.DocAcademicaId);

                                        var colComite = row.childNodes[5];
                                        colComite.innerHTML += "<br />" + newLinkA[0].outerHTML;
                                    }
                                }
                            }
                        });
                        if (TieneSEP) {
                            Funciones.SetearCombo(true);
                        }
                        $('#Load').modal('hide');
                    } else {
                        $('#Load').modal('hide');
                    }
                }
            });
        },
        CrearPDF: function (DocumentoId) {
            $.ajax({
                url: 'WS/Beca.asmx/GenerarPDF',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{DocumentoId:"' + DocumentoId + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data !== null) {
                        $('#Load').modal('hide');
                        window.open(data.d, "ArchivoPDF");
                    } else {
                        $('#Load').modal('hide');
                    }
                }
            });
        },
        AbrirArchivo: function (DocumentoId) {
            var url = "../../WS/Beca.asmx/GenerarPDF2?DocumentoId=" + DocumentoId;
            var archiv = window;
            archiv.open("Views/Archivos/Archivo.html", "PDF");
            archiv.Ruta = url;
        },
        BuscarNombre: function (Nombre) {
            $.ajax({
                url: 'WS/Alumno.asmx/BuscarAlumnoString',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{Filtro:"' + Nombre + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
                        $('#frmVarios').show();
                        tblAlumnos = $('#tblAlumnos').dataTable({
                            "aaData": data.d,
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
                    $('#Load').modal('hide');

                }
            });
        },
        Limpiar: function () {
            if ($('#txtAlumno').val().length === 0) { return false; }
            var nombre = $('#lblNombre');
            nombre[0].innerText = "";
            $('#txtOfertaEducativa').val("");
            $('#txtPeriodo').val("");
            $('#txtBecaMonto').val("0");
            //  $('#chkSEP').prop('checked', false);
            var chk = $('#chkSEP');
            var va = chk[0].checked;

            if (va === true) {
                Funciones.SetearCombo(false);               

                if (UsurioTipo === 3) {
                    Funciones.SEPReadOnly();
                }
                va = false;
            }
            else {
                if (UsurioTipo === 3) {
                    Funciones.SEPReadOnly();
                }
            }

            $('#divComite1').hide();
            //$('#chkSEP').removeAttr("checked");
            $('#divComite2').hide();
            $('#FileCarta a').click();
            $('#FileComite a').click();
            $('#divInscrito').hide();
            $('#divInscrito2').hide();
            $('#divInscrito3').hide();
            if (tblPagos !== null && tblPagos !== undefined) {
                tblPagos.fnClearTable();
            }
            if (tblBecas !== null && tblBecas !== undefined)
            { tblBecas.fnClearTable(); }
        },
        SEPReadOnly: function () {
            $("#chkSEP").bootstrapSwitch('readonly', true);
            $("#chkSEP").bootstrapSwitch('disabled', true);
        },
        SEPReadOnlyOff: function () {
            $("#chkSEP").bootstrapSwitch('readonly', false);
            $("#chkSEP").bootstrapSwitch('disabled', false);
        },
        OFertasPeriodo: function () {
            $.ajax({
                url: 'WS/Beca.asmx/OfertasAlumno',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data.d.length === 0) {
                        alertify.alert("El Alumno no Existe.");
                        $("#btnGenerarCargos").attr("disabled", "disabled");
                        Funciones.Limpiar();
                        $('#Load').modal('hide');
                        return false;
                    }
                    else if (data.d.length === 1) {
                        ///////////////Inicio
                        Funciones.TraerOfertaAlumno(AlumnoId, data.d[0].OfertaEducativaId);
                    }
                    else if (data.d.length > 1) {
                        $('#divOfertaAlumno tbody').remove();
                        $('#Load').modal('hide');
                        $('#divOfertas').modal('show');

                        $(data.d).each(function (s, d) {
                            var dv = "<tr><th>" + d.OfertaEducativaId + "</th><th><a class='btn blue'>" + d.Descripcion + "</a></th> </tr>";
                            $('#divOfertaAlumno').append(dv);
                        });
                    }
                    //////////Fin 
                }
            });
        },
        TraerOfertaAlumno: function (AlumnoId1, Oferta2) {
            NuevoIngreso = 0;
            $.ajax({
                url: 'WS/Beca.asmx/BuscarAlumno',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId1 + '",OfertaEducativaId:"' + Oferta2 + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data.d === null) {
                        $("#btnGenerarCargos").attr("disabled", "disabled");
                        Funciones.Limpiar();
                        $('#Load').modal('hide');
                        return false;
                    }

                    Funciones.CargarPeriodo(data.d.ListPeriodos);

                    var lblEmpresa = $('#divInscrito3');
                    lblEmpresa = $(lblEmpresa)[0].children[0].children[0].innerText;
                    lblEmpresa = data.d.EsEmpresa === true ? data.d.EsEspecial === true ? "Alumno Especial" : "Grupo Empresarial" : "";
                    EsEspecial = data.d.EsEspecial;
                    esEmpresa = data.d.EsEmpresa;
                    EsCompleto = data.d.Completa;
                    //lblEmpresa = lblEmpresa + " " + data.d.Grupo;
                    var objEmr = $('#divInscrito3');
                    $(objEmr)[0].children[0].children[0].innerText = lblEmpresa;


                    ///Asesorias & Materias Sueltas 
                    $('#txtMateria').val(data.d.Materias);
                    $('#txtAsesoria').val(data.d.Asesorias);
                    /////////// Alumno en Grupo Especial
                    /////////// Tiene Pagos
                    /////////// Inscrito?
                    if (data.d.AlumnoId === "-1") {
                        $('#divInscrito3').show();
                        //$('#tblBeca').hide();
                        $('#divComite').hide();
                        $("#txtBecaMonto").attr("disabled", "disabled");
                        if (data.d.Inscrito === true) {
                            $('#divInscrito').show();
                            var labelIns = $('#lblInscito');
                            labelIns[0].innerText += " Alumno Inscrito";

                            $("#btnGenerarCargos").attr("disabled", "disabled");
                            $('#btnGenerarCargos').text("Actualizar");
                        }
                        if (data.d.EsEspecial) {
                            $("#btnGenerarCargos").removeAttr("disabled");
                            $("#txtBecaMonto").removeAttr("disabled");
                        }
                        if (data.d.LstPagos.length > 0) {
                            if (data.d.LstPagos[0].BecaSEPD > 0) {
                                TieneSEP = true;
                            }
                        }
                    }
                    /////////// Alumno en Gruopo Especial
                    /////////// No tiene Pagos Generados
                    /////////// No Esta Inscrito
                    else if (data.d.AlumnoId === "-21") {
                        Estado = "-21";
                        $('#divInscrito2').show();
                        $('#divInscrito3').show();
                        $("#txtBecaMonto").attr("disabled", "disabled");
                        //$('#tblBeca').hide();
                        //$("#btnGenerarCargos").attr("disabled", "disabled");
                        //$('#btnGenerarCargos').text("Actualizar");
                        if (data.d.Inscrito === true) {
                            var lab = $('#lblPagos');
                            lab[0].innerText = "Reinscrito, sin Cargos Generados";
                            $('#divInscrito').show();
                            var labelIns = $('#lblInscito');
                            labelIns[0].innerText += " Alumno Inscrito";
                        }
                        if (data.d.EsEspecial) {
                            $("#btnGenerarCargos").removeAttr("disabled");
                            $("#txtBecaMonto").removeAttr("disabled");
                        }
                    }
                    /////////// Alumno Normal
                    /////////// No Tiene Pagos
                    else if (data.d.AlumnoId === "-2") {
                        Estado = "-2";
                        //alertify.alert("El Alumno no genero sus referencias de reinscripción.");
                        $('#divInscrito2').show();
                        /////////// Si Esta Inscrito Hace lo siguiente
                        if (data.d.Inscrito === true) {
                            $('#btnGenerarCargos').text("Actualizar");
                            var lab = $('#lblPagos');
                            lab[0].innerText = "Reinscrito, sin Cargos Generados";
                        }
                    }
                    else if (data.d.AlumnoId === "-4") {
                        alertify.alert("El alumno no inicio su proceso de reinscripción");
                        NuevoIngreso = -4;
                    }
                    else if (data.d.AlumnoId === "-5") {
                        $('#divInscrito').show();
                        var lab1 = $('#lblInscito');
                        lab1[0].innerText = "Sin Inscribir";
                        $('#btnGenerarCargos').text("Inscribir");

                        if (data.d.Inscrito) {
                            $('#btnGenerarCargos').text("Actualizar");
                            lab1[0].innerText = "Alumno Inscrito";
                            //if (EsCompleto) { $("#btnGenerarCargos").removeAttr("disabled"); } <<<<<<<<<------------ Codigo pendiente por Confirmar con Alejandra
                            //else { $("#btnGenerarCargos").attr("disabled", "disabled"); }
                        }
                        if (esEmpresa) {
                            $('#divInscrito3').show();
                            $("#txtBecaMonto").attr("disabled", "disabled");
                            if (data.d.EsEspecial) {
                                $("#btnGenerarCargos").removeAttr("disabled");
                                $("#txtBecaMonto").removeAttr("disabled");
                            }
                        }

                    }
                    /////////// Alumno Inscrito
                    /////////// No tiene ninguna condicion negativa
                    if (data.d.Inscrito === true && data.d.AlumnoId > 0 && data.d.NuevoIngreso === false) {
                        $('#divInscrito').show();
                        $('#btnGenerarCargos').text("Actualizar");
                        var lab = $('#lblInscito');
                        lab[0].innerText = "Inscrito";
                        /////////// Ya tiene Beca-SEP
                        if (data.d.LstPagos.length > 0) {
                            if (data.d.LstPagos[0].BecaSEPD > 0) {
                                TieneSEP = true;
                            }
                        }
                    }
                    if (data.d.NuevoIngreso) {
                        NuevoIngreso = 1000;
                        $('#divInscrito').show();
                        var labelIns = $('#lblInscito');
                        labelIns[0].innerText = "Alumno Inscrito - Nuevo Ingreso";
                        //SEPReadOnly();
                        if (!data.d.EsEspecial && data.d.EsEmpresa && data.d.SEP) {
                            $("#txtBecaMonto").removeAttr("disabled");
                            $("#txtBecaMonto").val("");

                            $("#btnGenerarCargos").removeAttr("disabled");
                            $('#btnGenerarCargos').text("Actualizar");
                            $('#divComite2').hide();

                            TieneSEP = true;
                        }
                        else if (!data.d.EsEspecial && data.d.EsEmpresa && !data.d.SEP) {
                            $("#txtBecaMonto").attr("disabled", "disabled");
                            $("#txtBecaMonto").val("");

                            $("#btnGenerarCargos").attr("disabled", "disabled");
                            $('#btnGenerarCargos').text("Actualizar");

                            $('#divComite2').hide();

                            TieneSEP = false;
                        } else if (data.d.SEP) {
                            $("#txtBecaMonto").removeAttr("disabled");
                            $("#txtBecaMonto").val("");

                            $("#btnGenerarCargos").removeAttr("disabled");
                            $('#btnGenerarCargos').text("Actualizar");
                            $('#divComite2').hide();


                            TieneSEP = true;
                        }
                        $('#btnGenerarCargos').text("Actualizar");
                    }

                    if (!data.d.Revision && data.d.ListPeriodos[0].PeriodoD !== "2017 1") {
                        $('#divInscrito').show();
                        var labelIns = $('#lblInscito');
                        labelIns[0].innerText += " Coordinador no ha dado su VistoBueno";
                    }
                    $('#txtOfertaEducativa').val(data.d.OfertasEducativas[0].Descripcion);
                    $('#txtOfertaEducativa').attr("data-Ofertaid", data.d.OfertasEducativas[0].OfertaEducativaId);
                    $('#txtPeriodo').val(data.d.ListPeriodos[0].PeriodoD);
                    $('#txtPeriodo').attr("data-Anio", data.d.Anio);
                    $('#txtPeriodo').attr("data-PeriodoId", data.d.PeriodoId);
                    var nombre = $('#lblNombre');
                    nombre[0].innerText = data.d.Nombre;

                    if (tblPagos !== null) {
                        tblPagos.fnClearTable();
                    }

                    Funciones.CargarDescuentos();
                    $("#txtBecaMonto").focus();
                    $('#txtBecaMonto').keyup();
                }
            });
        },
        Guardar: function () {
            var ObjBecaAlumno = Funciones.AlumnoBeca();

            $.ajax({
                url: 'WS/Beca.asmx/InsertarBeca',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(ObjBecaAlumno),
                dataType: 'json',
                success: function (data) {
                    if (data.d === "Guardado" || data.d === "Insertado") {
                        objAlumno = undefined;
                        Funciones.GuardarDocumentos();
                    }
                    else if (data.d === "Fallo") {
                        $('#Load').modal('hide');
                        alertify.alert("Se ha producido un error al inscribir.");
                    }
                }
            });
        },
        RecortarNombre: function (name) {
            var cadena;
            if (name.length > 15) {
                cadena = name.substring(0, 8);
                cadena += name.substring(name.length - 4, name.length);
                return cadena;
            } else {
                return name;
            }
        },
        GuardarDocumentos: function (Alumnoid, OfertaEducativaId, Anio, Periodo, EsComite, Usuario) {
            var data = new FormData();

            var filBeca = $('#CartaArchivo'); // FileList object
            filBeca = filBeca[0].files[0];
            data.append("DocumentoBeca", filBeca);

            data.append("AlumnoId", Alumnoid);
            data.append("OfertaEducativaId", Funciones.AlumnoBeca.OfertaEducativaId);
            data.append("Anio", Funciones.AlumnoBeca.Anio);
            data.append("Periodo", Funciones.AlumnoBeca.Periodo);
            data.append("EsComite", Funciones.AlumnoBeca.EsComite);
            data.append("UsuarioId", Funciones.AlumnoBeca.Usuario);

            var request = new XMLHttpRequest();
            request.open("POST", 'WS/Beca.asmx/GuardarDocumentos', true);
            request.send(data);

            request.onreadystatechange = function () {
                if (this.readyState === 2) {
                    $('#btnBuscarAlumno').click();
                }
            };
        },
        chkSEPonchange: function (event) {
            if (!isNaN(AlumnoId) && AlumnoId.length>0) {
                var val = this;
                if (val.checked === true) {
                    (function () {
                        if (!esEmpresa || EsEspecial) {
                            $("#txtBecaMonto").removeAttr("disabled");
                        }
                        $("#btnGenerarCargos").removeAttr("disabled");
                        if (NuevoIngreso === 0)
                            $('#btnGenerarCargos').text("Actualizar");

                        var lstDescuentos = [];

                        $(tblBecas).DataTable()
                            .column(2)
                            .data()
                            .each(function (value, index) {
                                if (value.length > 0) {
                                    lstDescuentos.push({
                                        Indice: index,
                                        Valor: value.replace("%", ""),
                                        TipoB: "Academica - SEP"
                                    });
                                }
                            });
                        $(tblBecas).DataTable()
                            .column(4)
                            .data()
                            .each(function (value, index) {
                                if (value.length > 0) {
                                    lstDescuentos.push({
                                        Indice: index,
                                        Valor: value.replace("%", ""),
                                        TipoB: "Otros"
                                    });
                                }
                            });
                        var indice = -1, monto = 0;

                        $(lstDescuentos).each(function () {
                            if (this.Indice !== indice) {
                                if (this.Indice > indice) {
                                    indice = this.Indice;
                                    monto = this.Valor;
                                }
                            }
                        });
                        $("#txtBecaMonto").val(monto);
                    })();
                } else {
                    if (esEmpresa && !EsEspecial) {
                        $("#txtBecaMonto").attr("disabled", "disabled");
                        $("#txtBecaMonto").val("0");
                    }
                    if (NuevoIngreso === 0)
                        $("#btnGenerarCargos").attr("disabled", "disabled");
                }
            } else {
                return false;
            }
        },
        btnCargosBecaonClick: function () {
            $('#dvTabla').hide();
            $('#dvCargos').show();
        },
        btnRegresaronClcik: function () {
            if (tblPagos !== undefined) {
                tblPagos.fnClearTable();
            }
            if (tblPagos !== tblBecas) {
                tblBecas.fnClearTable();
            }
            $("#txtAlumno").val("");
            Funciones.Limpiar();
            $('#dvTabla').show();
            $('#dvCargos').hide();
        },
        tblBecasonClicka: function () {
            var a = this;
            a = $(a).data("file");
            Funciones.CrearPDF(a);

        },
        tbAlumnosonClicka: function () {
            var idAlumno = tblAlumnosPActual.fnGetData(this.parentNode.parentNode, 0);
            $('#dvTabla').hide();
            $('#dvCargos').show();
            $('#txtAlumno').val(idAlumno);
            $('#btnBuscarAlumno').click();
        },
        TablaAlumnoClick: function () {
            $('#frmVarios').hide();
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
            $('#txtAlumno').val(rowadd.AlumnoId);
            Funciones.btnBuscarAlumnoonClick();
        },
        btnBuscarAlumnoonClick: function () {
            AlumnoId = $('#txtAlumno').val();
            objAlumno = undefined;
            $('#divObservaciones').hide();
            $('#slcDescripcionBeca').attr('disabled', 'disabled');
            $('#slcDescripcionBeca').empty();
            if (AlumnoId.length > 0) {
                var labelIns = $('#lblInscito');
                labelIns[0].innerText = "";

                TieneComite = false;
                TieneSEP = false;
                if (tblBecas !== null && tblBecas !== undefined)
                { tblBecas.fnClearTable(); }
                Funciones.Limpiar();
                var nombre = $('#hCarga');
                nombre[0].innerText = "Cargando";
                var labelIns = $('#lblInscito');
                labelIns.innerText = "Alumno Inscrito";

                $("#btnGenerarCargos").removeAttr("disabled");
                var load = $('#Load').modal();
                $('#divInscrito').hide();
                $('#divInscrito2').hide();
                $('#divInscrito3').hide();
                $('#txtOfertaEducativa').removeData("ofertaid");
                $('#txtPeriodo').removeData("anio");
                $('#txtPeriodo').removeData("periodoid");
                $('#txtBecaMonto').removeAttr("disabled");
                $('#btnGenerarCargos').text("Reinscribir");
                $('#tblBeca').show();
                Estado = "";
                var lab = $('#lblPagos');
                lab[0].innerText = "Pagos sin Generar";

                if (!isNaN(AlumnoId)) { Funciones.OFertasPeriodo(); }
                else { Funciones.BuscarNombre(AlumnoId); }
            }
            else { return false; }
        },
        btnClosedivOfertasonClick: function () {
            $("#txtAlumno").val("");
            Funciones.Limpiar();
        },
        divOfertaAlumnoonClicka: function (a) {
            var th = a.currentTarget.parentElement.parentElement.childNodes[0].innerText;
            $('#divOfertas').modal('hide');
            $('#Load').modal('show');
            Funciones.TraerOfertaAlumno(AlumnoId, th);
        },
        txtBecaMontoFocus: function () {
            this.select();
            var coords = $(this).offset();
            $(document).scrollLeft(coords.left);
            $(document).scrollTop(coords.top);
        },
        SetearCombo: function (estatus) {
            var read = $("#chkSEP").bootstrapSwitch('readonly');
            var disab = $("#chkSEP").bootstrapSwitch('disabled');

            $("#chkSEP").bootstrapSwitch('readonly', false);
            $("#chkSEP").bootstrapSwitch('disabled', false);

            $("#chkSEP").bootstrapSwitch('state', estatus);

            $("#chkSEP").bootstrapSwitch('readonly', read);
            $("#chkSEP").bootstrapSwitch('disabled', disab);
        },
        txtBecaMontoKeyUp: function (e) {
            var ch2 = $("#chkSEP")[0];

            var val = e.target.value;
            if (val < 0) {
                $('#txtBecaMonto').val(0);
            } else if (val > 100) {
                $('#txtBecaMonto').val(100);
            }

            if (NuevoIngreso === 0) {

                if (val === 0) {
                    $('#divComite2').hide();



                    if ($("#chkSEP").bootstrapSwitch('readonly')) {
                        if (!TieneSEP) {
                            Funciones.SetearCombo(false);
                        }
                        Funciones.SEPReadOnly();
                    }
                } else if (val > 0) {
                    if (UsurioTipo !== 3) { $('#divComite2').show(); }
                    UsurioTipo = UsurioTipo;

                    if ($("#chkSEP").bootstrapSwitch('readonly') && UsurioTipo !== 3) {
                        Funciones.SEPReadOnlyOff();
                    }
                }
            }
        },
        AlumnoBeca: function () {
            var Monto = $('#txtBecaMonto').val();
            Monto = Monto.length === 0 ? 0 : Monto;
            var SEP = $('#chkSEP');
            SEP = SEP[0].checked;

            return {
                AlumnoId: AlumnoId,
                OfertaEducativaId: $('#txtOfertaEducativa').data("ofertaid"),
                Monto: Monto,
                SEP: SEP,
                Anio: $('#slcDescripcionBeca').find(':selected').data("anio"),
                PeriodoId: $('#slcDescripcionBeca').find(':selected').data("periodoid"),
                Usuario: $.cookie('userAdmin'),
                EsComite: objAlumno !== undefined ? objAlumno.BecaComite === "Si" ? true : false : false,
                EsEmpresa: esEmpresa,
                Materias: $('#txtMateria').val(),
                Asesorias: $('#txtAsesoria').val()
            };
        },
        btnGenerarCargosonClick: function () {
            var nombre = $('#hCarga');
            nombre[0].innerText = "Guardando";
            $('#Load').modal('show');

            Funciones.Guardar();
        },
        CartaArchivoBindChange: function () {
            var file = $('#FileCarta');
            var tex = $('#txtCartaArchivo').html();
            if (this.files.length > 0) {
                $('#txtCartaArchivo').text(Funciones.RecortarNombre(this.files[0].name));
                file.addClass('fileinput-exists').removeClass('fileinput-new');
                $('#FileCarta span span').text('Cambiar');
            }
            else {
                $('#txtCartaArchivo').text('');
                file.removeClass('fileinput-exists').addClass('fileinput-new');
                $('#FileCarta span span').text('Seleccionar Archivo...');
            }
        },
        FileCartaAOnClick: function () {
            var file = $('#FileCarta');
            $('#txtCartaArchivo').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#CartaArchivo')[0].value = null;
            $('#FileCarta span span').text('Seleccionar Archivo...');
        },
        txtAlumnoonKeyDown: function (e) {
            if (e.which === 13) {
                Funciones.btnBuscarAlumnoonClick();
            }
        },        
        init: function () {

            $('#dvTabla').hide();
            $('#dvCargos').show();
            $('#divComite2').hide();
            $("#btnGenerarCargos").attr("disabled", "disabled");
            jQuery('#pulsate-regular2').pulsate({
                color: "#bf1c56"
            });
            $('#btnGenerarCargos').removeData('texto');
            $('#btnGenerarCargos').removeData('disabled');

            Funciones.CrearEnlace();

            Funciones.GetUsuario();

        },
    };

    Funciones.init();
});