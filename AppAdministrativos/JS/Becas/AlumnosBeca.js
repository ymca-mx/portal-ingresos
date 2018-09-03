$(function () {
    var AlumnoId;
    var tblAlumnosPActual = null, tblPagos = null, tblBecas = null, tblAlumnos = null;
    var Estado = "";
    var UsurioTipo = 0, NuevoIngreso = 0;
    var TieneSEP = false, TieneComite = false, EsEspecial = false, esEmpresa, EsCompleto = true;
    var objAlumno = undefined;

    $("#chkSEP").bootstrapSwitch({
        size: "large",
        onText: "Si",
        offText:"No"
    });

    var ReincripcionFn = {
        init() {
            $('#FileCarta .close').hide();
            $('#dvTabla').hide();
            $('#dvCargos').show();
            $('#divComite2').hide();
            $("#btnGenerarCargos").attr("disabled", "disabled");
            jQuery('#pulsate-regular2').pulsate({
                color: "#bf1c56"
            });
            $('#btnGenerarCargos').removeData('texto');
            $('#btnGenerarCargos').removeData('disabled');

            this.CrearEnlace();

            this.GetUsuario();

        },
        CrearEnlace() {
            $('#chkSEP').on('switchChange.bootstrapSwitch', ReincripcionFn.chkSEPonchange);
            $('#btnCargosBeca').on('click', ReincripcionFn.btnCargosBecaonClick);
            $('#btnRegresar').on('click', ReincripcionFn.btnRegresaronClcik);
            $("#tblBecas").on('click', 'a', ReincripcionFn.tblBecasonClicka);
            $('#tbAlumnos').on('click', 'a', ReincripcionFn.tbAlumnosonClicka);
            $('#btnBuscarAlumno').on('click', ReincripcionFn.btnBuscarAlumnoonClick);
            $('#btnClosedivOfertas').on('click', ReincripcionFn.btnClosedivOfertasonClick);
            $('#divOfertaAlumno').on('click', 'a', ReincripcionFn.divOfertaAlumnoonClicka);
            $("#txtBecaMonto").focus(ReincripcionFn.txtBecaMontoFocus);
            $('#txtBecaMonto').keyup(ReincripcionFn.txtBecaMontoKeyUp);
            $('#btnGenerarCargos').on('click', ReincripcionFn.btnGenerarCargosonClick);
            $('#CartaArchivo').bind('change', ReincripcionFn.CartaArchivoBindChange);
            $('#FileCarta a').on('click', ReincripcionFn.FileCartaAOnClick);
            $('#txtAlumno').on('keydown', ReincripcionFn.txtAlumnoonKeyDown);
            $('#tblAlumnos').on('click', 'a', ReincripcionFn.TablaAlumnoClick);
            $('#slcDescripcionBeca').on('change', ReincripcionFn.slcDescripcionBecaChange);
        },
        GetUsuario() {
            var usuario = localStorage.getItem('userAdmin');
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ObtenerUsuario",
                data: "{UsuarioId:'" + usuario + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success(data) {
                    if (data.d !== null) {
                        UsurioTipo = data.d.UsuarioTipoId;
                    }
                }
            });
        },
        CargarPeriodo(data) {
            if (data.length > 0) {
                var Desactiva = false;

                $(data).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.Anio + '' + this.PeriodoId);
                    option.data("Anio", this.Anio);
                    option.data("PeriodoId", this.PeriodoId);
                    option.data("Descripcion", this.Descripcion);

                    if (this.SolicitudInscripcion !== null) {
                        Desactiva = Desactiva == true ? true : true;
                        option.data("Observacion",
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
                ReincripcionFn.slcDescripcionBecaChange();
            }
        },
        slcDescripcionBecaChange() {
            $('#lblPeriodoBeca').text('');
            $('#divObservaciones').hide();

            var textbutton = $('#btnGenerarCargos').data('texto');
            if (textbutton !== undefined) {
                $('#btnGenerarCargos').text(textbutton);
                var estadoDisabled = $('#btnGenerarCargos').data('disabled');
                if (estadoDisabled !== undefined || estadoDisabled === true) {
                    $('#btnGenerarCargos').attr('disabled', "disaled");
                }
            }

            var Anio = $('#slcDescripcionBeca').find(':selected').data("Anio");
            var PeriodoId = $('#slcDescripcionBeca').find(':selected').data("PeriodoId");
            var Observaciones = $('#slcDescripcionBeca').find(':selected').data("Observacion");

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
        CargarDescuentos() {
            var OfertaEducativa = $('#txtOfertaEducativa').data("OfertaId");
            $.ajax({
                url: 'WS/Beca.asmx/DescuentosAnteriores',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
                dataType: 'json',
                success(data) {
                    if (data.d.length > 0) {
                        if (tblBecas !== null) {
                            $('#txtBecaMonto').val("0");
                            tblBecas
                                .clear()
                                .draw();
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

                        tblBecas = $("#tblBecas").DataTable({
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
                            "createdRow"(row, data, dataIndex) {
                                row.childNodes[1].style.textAlign = 'center';
                                row.childNodes[2].style.textAlign = 'center';
                                row.childNodes[3].style.textAlign = 'center';
                                row.childNodes[4].style.textAlign = 'center';

                                if (data.DocComiteRutaId.length > 0) {
                                    if (data.DocComiteRutaId !== "-1") {
                                        var newLinkS = $(document.createElement("a"));
                                        newLinkS[0].innerText = "Archivo...";

                                        var colComite = row.childNodes[6];
                                        colComite.innerHTML += "<br />" + newLinkS[0].outerHTML;
                                    }
                                }
                                if (data.DocAcademicaId.length > 0) {
                                    if (data.DocAcademicaId !== "-1") {
                                        var newLinkA = $(document.createElement("a"));
                                        newLinkA[0].innerText = "Archivo...";


                                        var colComite = row.childNodes[5];
                                        colComite.innerHTML += "<br />" + newLinkA[0].outerHTML;
                                    }
                                }
                            }
                        });
                        if (TieneSEP) {
                            ReincripcionFn.SetearCombo(true);
                        }
                        IndexFn.Block(false);
                    } else {
                        IndexFn.Block(false);
                    }
                }
            });
        },
        CrearPDF(DocumentoId) {
            $.ajax({
                url: 'WS/Beca.asmx/GenerarPDF',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{DocumentoId:"' + DocumentoId + '"}',
                dataType: 'json',
                success(data) {
                    if (data !== null) {
                        IndexFn.Block(false);
                        window.open(data.d, "ArchivoPDF");
                    } else {
                        IndexFn.Block(false);
                    }
                }
            });
        },
        AbrirArchivo(DocumentoId) {
            var url = "../../WS/Beca.asmx/GenerarPDF2?DocumentoId=" + DocumentoId;
            var archiv = window;
            archiv.open("Views/Archivos/Archivo.html", "PDF");
            archiv.Ruta = url;
        },
        BuscarNombre(Nombre) {
            $.ajax({
                url: 'WS/Alumno.asmx/BuscarAlumnoString',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{Filtro:"' + Nombre + '"}',
                dataType: 'json',
                success(data) {
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
                                    "mDataProp"(data) {
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

                }
            });
        },
        Limpiar() {
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
                ReincripcionFn.SetearCombo(false);

                if (UsurioTipo === 3) {
                    ReincripcionFn.SEPReadOnly();
                }
                va = false;
            }
            else {
                if (UsurioTipo === 3) {
                    ReincripcionFn.SEPReadOnly();
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
            if (tblBecas !== null && tblBecas !== undefined) {
                tblBecas
                    .clear()
                .draw(); }
        },
        SEPReadOnly() {
            $("#chkSEP").bootstrapSwitch('readonly', true);
            $("#chkSEP").bootstrapSwitch('disabled', true);
        },
        SEPReadOnlyOff() {
            $("#chkSEP").bootstrapSwitch('readonly', false);
            $("#chkSEP").bootstrapSwitch('disabled', false);
        },
        OFertasPeriodo() {
            $.ajax({
                url: 'WS/Beca.asmx/OfertasAlumno',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '"}',
                dataType: 'json',
                success(data) {
                    if (data.d.length === 0) {
                        alertify.alert("El Alumno no Existe.");
                        $("#btnGenerarCargos").attr("disabled", "disabled");
                        ReincripcionFn.Limpiar();
                        IndexFn.Block(false);
                    }
                    else if (data.d.length === 1) {
                        ///////////////Inicio
                        ReincripcionFn.TraerOfertaAlumno(AlumnoId, data.d[0].OfertaEducativaId);
                    }
                    else if (data.d.length > 1) {
                        $('#divOfertaAlumno tbody').remove();
                        IndexFn.Block(false);
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
        TraerOfertaAlumno(AlumnoId1, Oferta2) {
            NuevoIngreso = 0;
            $.ajax({
                url: 'WS/Beca.asmx/BuscarAlumno',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId1 + '",OfertaEducativaId:"' + Oferta2 + '"}',
                dataType: 'json',
                success(data) {
                    if (data.d === null) {
                        $("#btnGenerarCargos").attr("disabled", "disabled");
                        ReincripcionFn.Limpiar();
                        IndexFn.Block(false);
                        return false;
                    }

                    ReincripcionFn.CargarPeriodo(data.d.ListPeriodos);

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
                    $('#txtOfertaEducativa').data("OfertaId", data.d.OfertasEducativas[0].OfertaEducativaId);
                    $('#txtPeriodo').val(data.d.ListPeriodos[0].PeriodoD + " " + data.d.ListPeriodos[0].Descripcion);
                    $('#txtPeriodo').data("Anio", data.d.Anio);
                    $('#txtPeriodo').data("PeriodoId", data.d.PeriodoId);
                    var nombre = $('#lblNombre');
                    nombre[0].innerText = data.d.Nombre;

                    if (tblPagos !== null) {
                        tblPagos.fnClearTable();
                    }

                    ReincripcionFn.CargarDescuentos();
                    $("#txtBecaMonto").focus();
                    $('#txtBecaMonto').keyup();
                }
            });
        },
        Guardar() {
            var ObjBecaAlumno = ReincripcionFn.AlumnoBeca();

            $.ajax({
                url: 'WS/Beca.asmx/InsertarBeca',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(ObjBecaAlumno),
                dataType: 'json',
                success(data) {
                    if (data.d === "Guardado" || data.d === "Insertado") {
                        objAlumno = undefined;
                        ReincripcionFn.GuardarDocumentos();
                    }
                    else if (data.d === "Fallo") {
                        IndexFn.Block(false);
                        alertify.alert("Se ha producido un error al inscribir.");
                    }
                }
            });
        },
        RecortarNombre(name) {
            var cadena;
            if (name.length > 15) {
                cadena = name.substring(0, 8);
                cadena += name.substring(name.length - 4, name.length);
                return cadena;
            } else {
                return name;
            }
        },
        GuardarDocumentos(Alumnoid, OfertaEducativaId, Anio, Periodo, EsComite, Usuario) {
            var data = new FormData();

            var filBeca = $('#CartaArchivo'); // FileList object
            filBeca = filBeca[0].files[0];
            data.append("DocumentoBeca", filBeca);

            data.append("AlumnoId", Alumnoid);
            data.append("OfertaEducativaId", ReincripcionFn.AlumnoBeca.OfertaEducativaId);
            data.append("Anio", ReincripcionFn.AlumnoBeca.Anio);
            data.append("Periodo", ReincripcionFn.AlumnoBeca.Periodo);
            data.append("EsComite", ReincripcionFn.AlumnoBeca.EsComite);
            data.append("UsuarioId", ReincripcionFn.AlumnoBeca.Usuario);

            var request = new XMLHttpRequest();
            request.open("POST", 'WS/Beca.asmx/GuardarDocumentos', true);
            request.send(data);

            request.onreadystatechange = function () {
                if (this.readyState === 2) {
                    $('#btnBuscarAlumno').click();
                }
            };
        },
        chkSEPonchange(event) {
            if (!isNaN(AlumnoId) && AlumnoId.length > 0) {
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

                        $($(tblBecas).DataTable()
                            .column(2)
                            .data())
                            .each(function (value, index) {
                                if (value.length > 0) {
                                    lstDescuentos.push({
                                        Indice: index,
                                        Valor: value.replace("%", ""),
                                        TipoB: "Academica - SEP"
                                    });
                                }
                            });
                        $($(tblBecas).DataTable()
                            .column(4)
                            .data())
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
        btnCargosBecaonClick() {
            $('#dvTabla').hide();
            $('#dvCargos').show();
        },
        btnRegresaronClcik() {
            if (tblPagos !== undefined) {
                tblPagos.fnClearTable();
            }
            if (tblPagos !== tblBecas) {
                tblBecas
                    .clear()
                    .draw();
            }
            $("#txtAlumno").val("");
            ReincripcionFn.Limpiar();
            $('#dvTabla').show();
            $('#dvCargos').hide();
        },
        tblBecasonClicka() {
            var RowSelect = this.parentNode.parentNode;
            var Select = tblBecas.row(RowSelect).data();

            ReincripcionFn.CrearPDF(Select.DocComiteRutaId);

        },
        tbAlumnosonClicka() {
            var idAlumno = tblAlumnosPActual.fnGetData(this.parentNode.parentNode, 0);
            $('#dvTabla').hide();
            $('#dvCargos').show();
            $('#txtAlumno').val(idAlumno);
            $('#btnBuscarAlumno').click();
        },
        TablaAlumnoClick() {
            $('#frmVarios').hide();
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
            $('#txtAlumno').val(rowadd.AlumnoId);
            ReincripcionFn.btnBuscarAlumnoonClick();
        },
        btnBuscarAlumnoonClick() {
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
                if (tblBecas !== null && tblBecas !== undefined) {
                    tblBecas
                        .clear()
                    .draw(); }
                ReincripcionFn.Limpiar();

                var labelIns = $('#lblInscito');
                labelIns.innerText = "Alumno Inscrito";

                $("#btnGenerarCargos").removeAttr("disabled");
                IndexFn.Block(true);
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

                if (!isNaN(AlumnoId)) { ReincripcionFn.OFertasPeriodo(); }
                else { ReincripcionFn.BuscarNombre(AlumnoId); }
            }
            else { return false; }
        },
        btnClosedivOfertasonClick() {
            $("#txtAlumno").val("");
            ReincripcionFn.Limpiar();
        },
        divOfertaAlumnoonClicka(a) {
            var th = a.currentTarget.parentElement.parentElement.childNodes[0].innerText;
            $('#divOfertas').modal('hide');
            IndexFn.Block(true);
            ReincripcionFn.TraerOfertaAlumno(AlumnoId, th);
        },
        txtBecaMontoFocus() {
            this.select();
            var coords = $(this).offset();
            $(document).scrollLeft(coords.left);
            $(document).scrollTop(coords.top);
        },
        SetearCombo(estatus) {
            var read = $("#chkSEP").bootstrapSwitch('readonly');
            var disab = $("#chkSEP").bootstrapSwitch('disabled');

            $("#chkSEP").bootstrapSwitch('readonly', false);
            $("#chkSEP").bootstrapSwitch('disabled', false);

            $("#chkSEP").bootstrapSwitch('state', estatus);

            $("#chkSEP").bootstrapSwitch('readonly', read);
            $("#chkSEP").bootstrapSwitch('disabled', disab);
        },
        txtBecaMontoKeyUp(e) {
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
                            ReincripcionFn.SetearCombo(false);
                        }
                        ReincripcionFn.SEPReadOnly();
                    }
                } else if (val > 0) {
                    if (UsurioTipo !== 3) { $('#divComite2').show(); }
                    UsurioTipo = UsurioTipo;

                    if ($("#chkSEP").bootstrapSwitch('readonly') && UsurioTipo !== 3) {
                        ReincripcionFn.SEPReadOnlyOff();
                    }
                }
            }
        },
        AlumnoBeca() {
            var Monto = $('#txtBecaMonto').val();
            Monto = Monto.length === 0 ? 0 : Monto;
            var SEP = $('#chkSEP');
            SEP = SEP[0].checked;

            return {
                AlumnoId: AlumnoId,
                OfertaEducativaId: $('#txtOfertaEducativa').data("OfertaId"),
                Monto: Monto,
                SEP: SEP,
                Anio: $('#slcDescripcionBeca').find(':selected').data("anio"),
                PeriodoId: $('#slcDescripcionBeca').find(':selected').data("periodoid"),
                Usuario: localStorage.getItem('userAdmin'),
                EsComite: objAlumno !== undefined ? objAlumno.BecaComite === "Si" ? true : false : false,
                EsEmpresa: esEmpresa,
                Materias: $('#txtMateria').val(),
                Asesorias: $('#txtAsesoria').val()
            };
        },
        btnGenerarCargosonClick() {
            IndexFn.Block(true);

            ReincripcionFn.Guardar();
        },
        CartaArchivoBindChange() {
            var FileId = "FileCarta";
            var txtId = "txtCartaArchivo";

            var file = $('#' + FileId);


            if (this.files.length > 0) {
                $('#' + txtId).text(ReincripcionFn.RecortarNombre(this.files[0].name));
                file.addClass('fileinput-exists').removeClass('fileinput-new');
                $('#' + FileId + ' span span').text('Cambiar');
                $('#' + FileId + ' .close').show();
            }
            else {
                $('#' + txtId).text('');
                file.removeClass('fileinput-exists').addClass('fileinput-new');
                $('#' + FileId + ' span span').text('Seleccionar Archivo...');
                $('#' + FileId + ' .close').hide();
            }
        },
        FileCartaAOnClick() {
            var FileId = 'FileCarta';
            var txtId = "txtCartaArchivo";

            var file = $('#' + FileId);
            $('#' + txtId).text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            file[0] = null;
            $('#' + FileId + ' span span').text('Seleccionar Archivo...');
            $('#' + FileId + ' .close').hide();
        },
        txtAlumnoonKeyDown(e) {
            if (e.which === 13) {
                ReincripcionFn.btnBuscarAlumnoonClick();
            }
        },
    };

    ReincripcionFn.init();
});