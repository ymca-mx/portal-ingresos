$(function Init() {
    var AlumnoId;
    var tblAlumnosPActual, tblPagos, tblBecas;
    var Estado = "";
    var UsurioTipo = 0,NuevoIngreso = 0;
    var TieneSEP = false, TieneComite = false, EsEspecial = false, esEmpresa, EsCompleto = true;
    var objAlumno = undefined;
    GetUsuario();
    CargarPeriodo();
    
    $("#chkSEP").bootstrapSwitch();
    $('#chkSEP').change(function (event) {
        var val = this;
        if (val.checked == true) {
            (function () {
                if (!esEmpresa || EsEspecial) {
                    $("#txtBecaMonto").removeAttr("disabled");
                }
                $("#btnGenerarCargos").removeAttr("disabled");
                if (NuevoIngreso == 0)
                    $('#btnGenerarCargos').text("Actualizar");

                var lstDescuentos = [];

                $(tblBecas).DataTable()
                    .column(2)
                    .data()
                    .each(function (value, index) {
                        if (value.length > 0) {
                            lstDescuentos.push({
                                Indice: index,
                                Valor: value,
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
                                Valor: value,
                                TipoB: "Otros"
                            });
                        }
                    });
                var indice = -1, monto = 0;

                $(lstDescuentos).each(function () {
                    if (this.Indice != indice) {
                        if (this.Indice > indice) {
                            indice = this.Indice;
                            monto = this.Valor;
                        }
                    }
                });
                $("#txtBecaMonto").val(monto);
            })();

        } else {
            if (esEmpresa  && !EsEspecial) {
                $("#txtBecaMonto").attr("disabled", "disabled");
                $("#txtBecaMonto").val("0");
            }            
            if(NuevoIngreso==0)
                $("#btnGenerarCargos").attr("disabled", "disabled");
        }
    });

    $('#dvTabla').hide();
    $('#dvCargos').show();
    $('#divComite2').hide();

    $("#btnGenerarCargos").attr("disabled", "disabled");
    function GetUsuario() {
        var usuario = $.cookie('userAdmin');
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ObtenerUsuario",
            data: "{UsuarioId:'" + usuario + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    UsurioTipo = data.d.UsuarioTipoId;
                }
            }
        });
    }
    jQuery('#pulsate-regular2').pulsate({
        color: "#bf1c56"
    });
    //Cargar();
    function CargarPeriodo() {
        $.ajax({
            url: 'WS/General.asmx/GetPeriodoActual',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{}',
            dataType: 'json',
            success: function (data) {
                $('#lblPeriodoBeca').text(data.d.Anio.toString() + ' ' + data.d.PeriodoId.toString());
                $('#lblDescripcionBeca').text(data.d.Descripcion);
            }
        });
    }

    $('#btnCargosBeca').click(function () {
        $('#dvTabla').hide();
        $('#dvCargos').show();
    });

    $('#btnRegresar').click(function () {
        if (tblPagos != undefined) {
            tblPagos.fnClearTable();
        }
        if (tblPagos != tblBecas) {
            tblBecas.fnClearTable();
        }
        $("#txtAlumno").val("");
        Limpiar();
        $('#dvTabla').show();
        $('#dvCargos').hide();
    });

    function CargarDescuentos() {
        var OfertaEducativa = $('#txtOfertaEducativa').data("ofertaid");
        $.ajax({
            url: 'WS/Beca.asmx/DescuentosAnteriores',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d.length > 0) {
                    if (tblBecas != null) {
                        $('#txtBecaMonto').val("0");
                        tblBecas.fnClearTable();
                    }

                    var objUltimo = data.d[data.d.length - 1];
                    if (objUltimo != undefined && objUltimo.AnioPeriodoId == $('#lblPeriodoBeca').text()) {
                        objAlumno = objUltimo;
                    }
                    var monto = EsEspecial ? data.d[data.d.length - 1].OtrosDescuentos : data.d[data.d.length - 1].SMonto;
                    var smonto = 0;
                    if (monto[monto.length - 1] == '%') {
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
                            "search": "Buscar Alumno ",
                        },
                        "order": [[2, "desc"]],
                        "createdRow": function (row, data, dataIndex) {
                            row.childNodes[1].style.textAlign = 'center';
                            row.childNodes[2].style.textAlign = 'center';
                            row.childNodes[3].style.textAlign = 'center';
                            row.childNodes[4].style.textAlign = 'center';

                            if (data.DocComiteRutaId.length > 0) {
                                if (data.DocComiteRutaId != "-1") {
                                    var newLinkS = $(document.createElement("a"));
                                    newLinkS[0].innerText = "Archivo...";
                                    newLinkS.attr('data-file', data.DocComiteRutaId)

                                    var colComite = row.childNodes[6];
                                    colComite.innerHTML += "<br />" + newLinkS[0].outerHTML;
                                }
                            }
                            if (data.DocAcademicaId.length > 0) {
                                if (data.DocAcademicaId != "-1") {
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
                        var chk1 = $('#chkSEP');
                        chk1[0] = true;
                        $('input[name="chkSEP"]').bootstrapSwitch('setState', true);
                    }
                    $('#Load').modal('hide');
                } else {
                    $('#Load').modal('hide');
                }
            }
        });
    }

    function CrearPDF(DocumentoId) {
        $.ajax({
            url: 'WS/Beca.asmx/GenerarPDF',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{DocumentoId:"' + DocumentoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data != null) {
                    $('#Load').modal('hide');
                    window.open(data.d, "ArchivoPDF");
                } else {
                    $('#Load').modal('hide');
                }
            }
        });
    }
    function AbrirArchivo(DocumentoId) {
        var url = "WS/Beca.asmx/GenerarPDF2?DocumentoId=" + DocumentoId;
        var archiv = window;
        archiv.open("../Inscritos/Archivos/Archivo.html", "PDF");
        archiv.Ruta = url;
    }
    $("#tblBecas").on('click', 'a', function () {
        var a = this;
        a = $(a).data("file");
        AbrirArchivo(a);

    });
    function Limpiar() {
        if ($('#txtAlumno').val().length == 0) { return false; }
        var nombre = $('#lblNombre');
        nombre[0].innerText = "";
        $('#txtOfertaEducativa').val("");
        $('#txtPeriodo').val("");
        $('#txtBecaMonto').val("0");
        //  $('#chkSEP').prop('checked', false);
        var chk = $('#chkSEP');
        var va = chk[0].checked;

        if (va == true) {

            $('input[name="chkSEP"]').bootstrapSwitch('setState', false);

            if (UsurioTipo == 3) {
                SEPReadOnly();
            }
            va = false;
        }
        else {
            if (UsurioTipo == 3) {
                SEPReadOnly();
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
        if (tblPagos != null) {
            tblPagos.fnClearTable();
        }
        if (tblBecas != null)
        { tblBecas.fnClearTable(); }
    }

    function SEPReadOnly() {
        var sep = $('#chkSEP');
        sep = sep[0];
        if (!$('input[name="chkSEP"]').bootstrapSwitch('isReadOnly')) {
            $('input[name="chkSEP"]').bootstrapSwitch('setReadOnly', true);
            $('input[name="chkSEP"]').bootstrapSwitch('setDisabled', true);
        }

    }

    function SEPReadOnlyOff() {
        var sep = $('#chkSEP');
        sep = sep[0];
        //$(sep.parentNode.parentNode.parentNode).removeAttr('readonly');
        if ($('input[name="chkSEP"]').bootstrapSwitch('isReadOnly')) {
            $('input[name="chkSEP"]').bootstrapSwitch('setReadOnly', false);
            $('input[name="chkSEP"]').bootstrapSwitch('setDisabled', false);
        }

    }


    $('#tbAlumnos').on('click', 'a', function () {
        var idAlumno = tblAlumnosPActual.fnGetData(this.parentNode.parentNode, 0);
        $('#dvTabla').hide();
        $('#dvCargos').show();
        $('#txtAlumno').val(idAlumno);
        $('#btnBuscarAlumno').click();
    });

    $('#btnBuscarAlumno').click(function () {
        AlumnoId = $('#txtAlumno').val();
        objAlumno = undefined;
        if (AlumnoId.length > 0) {
            var labelIns = $('#lblInscito');
            labelIns[0].innerText = "";

            TieneComite = false;
            TieneSEP = false;
            if (tblBecas != null)
            { tblBecas.fnClearTable(); }
            Limpiar();
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
            OFertasPeriodo();
        } else { return false; }
    });
    function OFertasPeriodo() {
        $.ajax({
            url: 'WS/Beca.asmx/OfertasAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d.length == 0) {
                    alertify.alert("El Alumno no Existe.");
                    $("#btnGenerarCargos").attr("disabled", "disabled");
                    Limpiar();
                    $('#Load').modal('hide');
                    return false;
                }
                else if (data.d.length == 1) {
                    ///////////////Inicio
                    TraerOfertaAlumno(AlumnoId, data.d[0].OfertaEducativaId);
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
    }
    $('#btnClosedivOfertas').click(function () {
        $("#txtAlumno").val("");
        Limpiar();
    });
    $('#divOfertaAlumno').on('click', 'a', function (a) {
        var th = a.currentTarget.parentElement.parentElement.childNodes[0].innerText;
        $('#divOfertas').modal('hide');
        $('#Load').modal('show');
        TraerOfertaAlumno(AlumnoId, th);
    });
    function TraerOfertaAlumno(AlumnoId1, Oferta2) {
        NuevoIngreso = 0;
        $.ajax({
            url: 'WS/Beca.asmx/BuscarAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId1 + '",OfertaEducativaId:"' + Oferta2 + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == null) {
                    $("#btnGenerarCargos").attr("disabled", "disabled");
                    Limpiar();
                    $('#Load').modal('hide');
                    return false;
                }


                var lblEmpresa = $('#divInscrito3');
                lblEmpresa = $(lblEmpresa)[0].children[0].children[0].innerText;
                lblEmpresa = data.d.EsEmpresa == true ? data.d.EsEspecial == true ? "Alumno Especial" : "Grupo Empresarial" : "";
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
                if (data.d.AlumnoId == "-1") {
                    $('#divInscrito3').show();
                    //$('#tblBeca').hide();
                    $('#divComite').hide();
                    $("#txtBecaMonto").attr("disabled", "disabled");
                    if (data.d.Inscrito == true) {
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
                    if (data.d.lstPagos.length > 0) {
                        if (data.d.lstPagos[0].BecaSEPD > 0) {
                            TieneSEP = true;
                        }
                    }
                }
                    /////////// Alumno en Gruopo Especial
                    /////////// No tiene Pagos Generados
                    /////////// No Esta Inscrito
                else if (data.d.AlumnoId == "-21") {
                    Estado = "-21";
                    $('#divInscrito2').show();
                    $('#divInscrito3').show();
                    $("#txtBecaMonto").attr("disabled", "disabled");
                    //$('#tblBeca').hide();
                    //$("#btnGenerarCargos").attr("disabled", "disabled");
                    //$('#btnGenerarCargos').text("Actualizar");
                    if (data.d.Inscrito == true) {
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
                else if (data.d.AlumnoId == "-2") {
                    Estado = "-2";
                    //alertify.alert("El Alumno no genero sus referencias de reinscripción.");
                    $('#divInscrito2').show();
                    /////////// Si Esta Inscrito Hace lo siguiente
                    if (data.d.Inscrito == true) {
                        $('#btnGenerarCargos').text("Actualizar");
                        var lab = $('#lblPagos');
                        lab[0].innerText = "Reinscrito, sin Cargos Generados";
                    }
                }
                else if (data.d.AlumnoId == "-4") {
                    alertify.alert("El alumno no inicio su proceso de reinscripción");
                    NuevoIngreso = -4;
                }
                else if (data.d.AlumnoId == "-5") {
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
                if (data.d.Inscrito == true && data.d.AlumnoId > 0 && data.d.NuevoIngreso == false) {
                    $('#divInscrito').show();
                    $('#btnGenerarCargos').text("Actualizar");
                    var lab = $('#lblInscito');
                    lab[0].innerText = "Inscrito";
                    /////////// Ya tiene Beca-SEP
                    if (data.d.lstPagos.length > 0) {
                        if (data.d.lstPagos[0].BecaSEPD > 0) {
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
                    if ((!data.d.EsEspecial && data.d.EsEmpresa) && data.d.SEP) {
                        $("#txtBecaMonto").removeAttr("disabled");
                        $("#txtBecaMonto").val("");

                        $("#btnGenerarCargos").removeAttr("disabled");
                        $('#btnGenerarCargos').text("Actualizar");
                        $('#divComite2').hide();

                        TieneSEP = true;
                    }
                    else if ((!data.d.EsEspecial && data.d.EsEmpresa) && !data.d.SEP) {
                        $("#txtBecaMonto").attr("disabled", "disabled");
                        $("#txtBecaMonto").val("");

                        $("#btnGenerarCargos").attr("disabled", "disabled");
                        $('#btnGenerarCargos').text("Actualizar");

                        $('#divComite2').hide();

                        TieneSEP = false;
                    } else if(data.d.SEP) {
                        $("#txtBecaMonto").removeAttr("disabled");
                        $("#txtBecaMonto").val("");

                        $("#btnGenerarCargos").removeAttr("disabled");
                        $('#btnGenerarCargos').text("Actualizar");
                        $('#divComite2').hide();

                        
                        TieneSEP = true;
                    }
                    $('#btnGenerarCargos').text("Actualizar");
                }

                if (!data.d.Revision && data.d.PeriodoD != "2017 1") {
                    $('#divInscrito').show();
                    var labelIns = $('#lblInscito');
                    labelIns[0].innerText += " Coordinador no ha dado su VistoBueno";
                }
                $('#txtOfertaEducativa').val(data.d.OfertasEducativas[0].Descripcion);
                $('#txtOfertaEducativa').attr("data-Ofertaid", data.d.OfertasEducativas[0].OfertaEducativaId);
                $('#txtPeriodo').val(data.d.PeriodoD);
                $('#txtPeriodo').attr("data-Anio", data.d.Anio);
                $('#txtPeriodo').attr("data-PeriodoId", data.d.PeriodoId);
                var nombre = $('#lblNombre');
                nombre[0].innerText = data.d.Nombre;

                if (tblPagos != null) {
                    tblPagos.fnClearTable();
                }

                CargarDescuentos();
                $("#txtBecaMonto").focus();
                $('#txtBecaMonto').keyup();
            }
        });
    }

    $("#txtBecaMonto").focus(function () {
        this.select();
        var coords = $(this).offset();
        $(document).scrollLeft(coords.left);
        $(document).scrollTop(coords.top);
    });

    $('#txtBecaMonto').keyup(function (e) {
        var ch2 = $('input[name="chkSEP"]')[0];

        var val = e.target.value;
        if (val < 0) {
            $('#txtBecaMonto').val(0);
        } else if (val > 100) {
            $('#txtBecaMonto').val(100);
        }

        if (NuevoIngreso == 0) {

            if (val == 0) {
                $('#divComite2').hide();



                if ($('input[name="chkSEP"]').bootstrapSwitch('isReadOnly')) {
                    if (!TieneSEP) { $('input[name="chkSEP"]').bootstrapSwitch('setState', false); }
                    SEPReadOnly();
                }
            } else if (val > 0) {
                if (UsurioTipo != 3) { $('#divComite2').show(); }
                UsurioTipo = UsurioTipo;

                if ($('input[name="chkSEP"]').bootstrapSwitch('isReadOnly') && UsurioTipo != 3) {
                    SEPReadOnlyOff();
                }
            }
        }
    });
    $('#btnGenerarCargos').click(function () {
        var Monto = $('#txtBecaMonto').val();
        //alertify.confirm("<p>¿Esta seguro que desea continuan?<br><br><hr>", function (e) {
        //    if (e) {            
        Monto = Monto.length == 0 ? 0 : Monto;
        var nombre = $('#hCarga');
        nombre[0].innerText = "Guardando";
        $('#Load').modal('show');
        var OfertaEducativa = $('#txtOfertaEducativa').data("ofertaid");
        var SEP = $('#chkSEP');
        SEP = SEP[0].checked;


        var Anio = $('#txtPeriodo').data("anio");
        var Periodo = $('#txtPeriodo').data("periodoid");
        var Empresa = esEmpresa;
        var Comite = objAlumno != undefined ? objAlumno.BecaComite == "Si" ? true : false : false;
        var Materias = $('#txtMateria').val();
        var Asesorias = $('#txtAsesoria').val();
        Guardar(Monto, OfertaEducativa, SEP, Anio, Periodo, Comite, Empresa, Materias, Asesorias);
        //    }
        //});
    });

    function Guardar(Monto, OfertaEducativaId, SEP, Anio, Periodo, Comite, esEmpresa, Materias, Asesorias) {
        SEP = SEP == true ? "true" : "false";
        var usuario = $.cookie('userAdmin');
        $.ajax({
            url: 'WS/Beca.asmx/InsertarBeca',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativaId + '",Monto:"' + Monto + '",SEP:"' +
                SEP + '",Anio:"' + Anio + '",PeriodoId:"' + Periodo + '",Usuario:"' + usuario + '",EsComite:"' + Comite +
                '",EsEmpresa:"' + esEmpresa + '",Materias:"' + Materias + '",Asesorias:"' + Asesorias + '"}',
            dataType: 'json',
            success: function (data) {
                if (data.d == "Guardado" || data.d == "Insertado") {
                    objAlumno = undefined;
                    GuardarDocumentos(AlumnoId, OfertaEducativaId, Anio, Periodo, Comite, usuario);
                }
                else if (data.d == "Fallo") {
                    $('#Load').modal('hide');
                    alertify.alert("Se ha producido un error al inscribir.");
                }
            }
        });
    }

    $('#CartaArchivo').bind('change', function () {
        var file = $('#FileCarta');
        var tex = $('#txtCartaArchivo').html();
        if (this.files.length > 0) {
            $('#txtCartaArchivo').text(RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileCarta span span').text('Cambiar');
        }
        else {
            $('#txtCartaArchivo').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileCarta span span').text('Seleccionar Archivo...');
        }
    });
    $('#FileCarta a').click(function () {
        var file = $('#FileCarta');
        $('#txtCartaArchivo').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileCarta span span').text('Seleccionar Archivo...');
    });
    function RecortarNombre(name) {
        var cadena;
        if (name.length > 15) {
            cadena = name.substring(0, 8);
            cadena += name.substring(name.length - 4, name.length);
            return cadena;
        } else {
            return name;
        }
    }
    function GuardarDocumentos(Alumnoid, OfertaEducativaId, Anio, Periodo, EsComite, Usuario) {
        var data = new FormData();

        var filBeca = $('#CartaArchivo'); // FileList object
        filBeca = filBeca[0].files[0];
        data.append("DocumentoBeca", filBeca);

        data.append("AlumnoId", Alumnoid);
        data.append("OfertaEducativaId", OfertaEducativaId);
        data.append("Anio", Anio);
        data.append("Periodo", Periodo);
        data.append("EsComite", EsComite);
        data.append("UsuarioId", Usuario);

        var request = new XMLHttpRequest();
        request.open("POST", 'WS/Beca.asmx/GuardarDocumentos', true);
        request.send(data);

        request.onreadystatechange = function () {
            if (this.readyState == 2) {
                //$("#txtAlumno").val("");
                //$('#Load').modal('hide');
                //Limpiar();
                $('#btnBuscarAlumno').click();
                //$('#dvTabla').show();
                //$('#dvCargos').hide();
                //Cargar2();
            }
        }
    }
    $('#txtAlumno').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscarAlumno').click();
        }
    });
});