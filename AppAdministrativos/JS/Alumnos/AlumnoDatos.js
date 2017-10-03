$(function init() {
    var Now = new Date();
    var años = Now.getFullYear() - 18;
    var mes = Now.getMonth() + 1;
    var Fecha = Now.getDate() + '-' + mes + '-' + años;
    var tblBecas;
    var AlumnoNum;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    var tblAlumnos;
    var Antecendentes;
    var OfertaEducativaTipoId = -1;
    BloquearTodo();

    function LimpiarCampos() {
        $("#submit_form").trigger('reset');
        error.hide();
        success.hide();
        $('#submit_form  div').removeClass('has-error');
        $('#submit_form  div').removeClass('has-success');
        $('#submit_form  i').removeClass('fa-warning');
        $('#submit_form  i').removeClass('fa-check');
        $('#divAntecendentes').hide();
        //$('#submit_form  select').prop("disabled", true);
        Antecendentes = null;
        OfertaEducativaTipoId = -1;
    }
    $('#btnBuscarAlumno').click(function () {
        $('#frmVarios').hide();
        if (tblAlumnos != undefined) {
            tblAlumnos.fnClearTable();
        }
        if ($('#txtAlumno').val().length == 0) { return false; }
        LimpiarCampos();
        $("#slcOfertaEducativa").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcOfertaEducativa").append(optionP);
        BloquearTodo();


        $('#Load').modal('show');
        AlumnoNum = $('#txtAlumno').val();
        if (tblBecas != null) {
            tblBecas.fnClearTable();
        }

        if (!isNaN(AlumnoNum)) {
            EsNumero(AlumnoNum);
        } else {
            EsString(AlumnoNum);
        }
    });

    function EsNumero(Alumno) {
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ObenerDatosAlumno",
            data: "{AlumnoId:'" + Alumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    Antecendentes = data.d.Antecendentes;
                    OfertasAlumnoIni(data.d.lstOfertas);
                    $('#divEditar').show();
                    $('#MenuTab').show();
                    $('#slcNacionalidad').val(data.d.DTOAlumnoDetalle.PaisId == 146 ? 1 : 2);
                    if (data.d.DTOAlumnoDetalle.PaisId == 146) {
                        CargarEstados($('#slcLugarN'), data.d.DTOAlumnoDetalle.EntidadNacimientoId);
                    } else {
                        CargarPaises($('#slcLugarN'), data.d.DTOAlumnoDetalle.PaisId);
                    }
                    ///Personales 
                    $('#txtMatricula').val(data.d.Matricula);
                    $('#txtnombre').val(data.d.Nombre);
                    $('#txtApPaterno').val(data.d.Paterno);
                    $('#txtApMaterno').val(data.d.Materno);
                    $('#txtCelular').val(data.d.DTOAlumnoDetalle.Celular);
                    $('#txtFNacimiento').val(data.d.DTOAlumnoDetalle.FechaNacimientoC);
                    $('#txtFNacimiento').change();
                    $('#txtCURP').val(data.d.DTOAlumnoDetalle.CURP);
                    $('#txtEmail').val(data.d.DTOAlumnoDetalle.Email);
                    $('#txtTelefonoCasa').val(data.d.DTOAlumnoDetalle.TelefonoCasa);

                    $('#txtCalle').val(data.d.DTOAlumnoDetalle.Calle);
                    $('#txtNumeroE').val(data.d.DTOAlumnoDetalle.NoExterior);
                    $('#txtNumeroI').val(data.d.DTOAlumnoDetalle.NoInterior);
                    $('#txtCP').val(data.d.DTOAlumnoDetalle.Cp);
                    $('#txtColonia').val(data.d.DTOAlumnoDetalle.Colonia);

                    $('#slcEstadoCivil').val(data.d.DTOAlumnoDetalle.EstadoCivilId);
                    $('#slcSexo').val(data.d.DTOAlumnoDetalle.GeneroId);
                    //$('#slcEstado').val(data.d.DTOAlumnoDetalle.EntidadFederativaId);
                    //CargarEstados1($('#slcMunicipio'), data.d.DTOAlumnoDetalle.MunicipioId);
                    CargarEstados1(data.d.DTOAlumnoDetalle.EntidadFederativaId, data.d.DTOAlumnoDetalle.MunicipioId);

                    if (data.d.DTOPersonaAutorizada.length > 0) {
                        $('#txtPAutorizada').val(data.d.DTOPersonaAutorizada[0].Nombre);
                        $('#txtAPPaterno').val(data.d.DTOPersonaAutorizada[0].Paterno);
                        $('#txtAPMaterno').val(data.d.DTOPersonaAutorizada[0].Materno);
                        $('#slcParentesco').val(data.d.DTOPersonaAutorizada[0].ParentescoId);
                        $('#txtPEmail').val(data.d.DTOPersonaAutorizada[0].Email);
                        $('#txtTelefonoPA').val(data.d.DTOPersonaAutorizada[0].Celular == null ? "" : data.d.DTOPersonaAutorizada[0].Celular);
                        $('#txtTelefonoPAT').val(data.d.DTOPersonaAutorizada[0].Telefono);

                        if (data.d.DTOPersonaAutorizada[0].Autoriza == true) {
                            $('#chkAuotiza1').prop("checked", true);
                            var spq = $('#chkAuotiza1')[0].parentElement;
                            $(spq).addClass('checked');
                        }
                        if (data.d.DTOPersonaAutorizada.length > 1) {
                            $('#txtPAutorizada2').val(data.d.DTOPersonaAutorizada[1].Nombre);
                            $('#txtAPPaterno2').val(data.d.DTOPersonaAutorizada[1].Paterno);
                            $('#txtAPMaterno2').val(data.d.DTOPersonaAutorizada[1].Materno);
                            $('#slcParentesco2').val(data.d.DTOPersonaAutorizada[1].ParentescoId);
                            $('#txtPEmail2').val(data.d.DTOPersonaAutorizada[1].Email);
                            $('#txtTelefonoPA2').val(data.d.DTOPersonaAutorizada[1].Celular);
                            $('#txtTelefonoPAT2').val(data.d.DTOPersonaAutorizada[1].Telefono);

                            if (data.d.DTOPersonaAutorizada[1].Autoriza == true) {
                                $('#chkAuotiza2').prop("checked", true);
                                var spam = $('#chkAuotiza2')[0].parentElement;
                                $(spam).addClass('checked');
                            }
                        }

                    }
                    
                    //$('#').val(data.d);
                    //OfertasAlumno(Alumno);
                    $('#Load').modal('hide');
                    $('#tab1btn').click();
                }
                else {
                    $('#Load').modal('hide');
                    alertify.alert("Error, El Alumno no Existe.");

                }

            }
        });
    }

    function EsString(Alumno) {
        $('#tab1').hide();
        $.ajax({
            url: 'WS/Alumno.asmx/BuscarAlumnoString',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{Filtro:"' + Alumno + '"}',
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
    }

    $('#tblAlumnos').on('click', 'a', function () {
        $('#frmVarios').hide();
        $('#Load').modal('show');
        var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
        AlumnoNum = rowadd.AlumnoId;
        EsNumero(rowadd.AlumnoId);
    });

    function BloquearTodo() {
        $('#MenuTab').hide();
        $('#submit_form  input').prop("disabled", true);
        $('#submit_form  select').prop("disabled", true);
        $('#submit_form  button').prop("disabled", true);
        $('#slcOfertaEducativa').removeAttr("disabled");
        $('#slcOfertasAlumno').removeAttr("disabled");
        $('#slcOfertasAlumno1').removeAttr("disabled");
        
        $('#chkUni').prop("checked", false);
        $('#uniform-chkUniSi').removeClass('disabled');
        $('#divGuardar').hide();
        var chk = $('#chkUni')[0];
        var spam = $('#chkUni')[0].parentElement;
        $(spam).removeClass('checked');
        chk = chk.checked;

        //GetUsuario();
    }

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
                    if (UsurioTipo == 13) {
                        $('#btnEditar').attr("disabled", "disabled");
                    }
                }
            }
        });
    }
    $('#btnEditar').click(function () {
        $('#divEditar').hide();
        $('#submit_form  input').prop("disabled", false);
        $('#submit_form  select').prop("disabled", false);
        $('#submit_form  button').prop("disabled", false);
        $('#uniform-chkAuotiza1').removeClass('disabled');
        $('#uniform-chkAuotiza2').removeClass('disabled');
        $('#uniform-chkUni').removeClass('disabled');
        $('#uniform-chkUniSi').removeClass('disabled');
        $('#uniform-chkUniNo').removeClass('disabled');
        $('#txtMatricula').prop("disabled", true);
        $('#txtUni').prop("disabled", true);
        $('#divGuardar').show();
        if ($('#chkUniNo')[0].checked) {
            $('#txtUniMotivo').prop('disabled', true);
        }

    });

    $('#Guardar').click(function () {
        var nombre = $('#hCarga');
        nombre[0].innerText = "Guardando";
        $('#Load').modal('show');

        var estatus = ValidarTodos();
        if (estatus) {
            GuardarTodo();
        }
        else { $('#Load').modal('hide'); }
    });
    function ValidarTodos() {
        var tabActivo = '#tab';
        var tabActual = '#tab';
        var count = 0;

        for (var i = 1; i < 5; i++) {
            var div = $(tabActivo + '' + i);
            if (div[0].className == "tab-pane active") {
                tabActual += '' + i;
            }
            $(div).show();
            count += form.valid() == true ? 1 : 0;
            $(div).hide();
        }

        $(tabActual).show();
        if (count == 4)
        { return true; } else { return false; }
    }
    function GuardarTodo() {
        var usuario = $.cookie('userAdmin');
        var Variables = "{";
        Variables += "AlumnoId:'" + AlumnoNum + "',";
        Variables += "UsuarioId:'" + usuario + "',";
        Variables += "Nombre:'" + $('#txtnombre').val() + "',";
        Variables += "Paterno:'" + $('#txtApPaterno').val() + "',";
        Variables += "Materno:'" + $('#txtApMaterno').val() + "',";
        Variables += "Celular:'" + $('#txtCelular').val() + "',";
        Variables += "FNacimiento:'" + $('#txtFNacimiento').val() + "',";

        Variables += "CURP:'" + $('#txtCURP').val() + "',";
        Variables += "Email:'" + $('#txtEmail').val() + "',";
        Variables += "TelCasa:'" + $('#txtTelefonoCasa').val() + "',";

        Variables += "Calle:'" + $('#txtCalle').val() + "',";
        Variables += "NumeroE:'" + $('#txtNumeroE').val() + "',";
        Variables += "NumeroI:'" + $('#txtNumeroI').val() + "',";
        Variables += "CP:'" + $('#txtCP').val() + "',";
        Variables += "Colonia:'" + $('#txtColonia').val() + "',";

        Variables += "EstadoCivil:'" + $('#slcEstadoCivil').val() + "',";
        Variables += "Sexo:'" + $('#slcSexo').val() + "',";
        Variables += "Estado:'" + $('#slcEstado').val() + "',";
        Variables += "Municipio:'" + $('#slcMunicipio').val() + "',";
        Variables += "Nacionalidad:'" + $('#slcNacionalidad').val() + "',";

        Variables += "LugarN:'" + $('#slcLugarN').val() + "',";
        ////////////////////
        Variables += "NombrePA1:'" + $('#txtPAutorizada').val() + "',";
        Variables += "PaternoPA1:'" + $('#txtAPPaterno').val() + "',";
        Variables += "MaternoPA1:'" + $('#txtAPMaterno').val() + "',";
        Variables += "PArentescoPA1:'" + $('#slcParentesco').val() + "',";
        Variables += "EmailPA1:'" + $('#txtPEmail').val() + "',";
        Variables += "TelefonoPA1:'" + $('#txtTelefonoPA').val() + "',";
        Variables += "Telefono2PA1:'" + $('#txtTelefonoPAT').val() + "',";
        Variables += "Autoriza1:'" + ($('#chkAuotiza1').prop("checked") ? 'true' : 'false') + "',";

        //////////////////
        Variables += "NombrePA2:'" + $('#txtPAutorizada2').val() + "',";
        Variables += "PaternoPA2:'" + $('#txtAPPaterno2').val() + "',";
        Variables += "MaternoPA2:'" + $('#txtAPMaterno2').val() + "',";
        Variables += "PArentescoPA2:'" + $('#slcParentesco2').val() + "',";
        Variables += "EmailPA2:'" + $('#txtPEmail2').val() + "',";
        Variables += "TelefonoPA2:'" + $('#txtTelefonoPA2').val() + "',";
        Variables += "Telefono2PA2:'" + $('#txtTelefonoPAT2').val() + "',";
        Variables += "Autoriza2:'" + ($('#chkAuotiza2').prop("checked") ? 'true' : 'false') + "'}";

        GuardarAntecedentesTmp();
        ///////////////////////////
        $.ajax({
            url: 'WS/Alumno.asmx/UpdateAlumno',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: Variables,
            dataType: 'json',
            success: function (data) {
                try {

                    if (data.d) {
                        var TRes;
                        $(Antecendentes).each(function (i) {
                            this.AlumnoId = this.AlumnoId.toString();
                            this.AntecedenteTipoId = this.AntecedenteTipoId.toString();
                            this.AreaAcademicaId = this.AreaAcademicaId.toString();
                            this.Procedencia = this.Procedencia.toString();
                            this.Promedio = this.Promedio.toString();
                            this.Anio = this.Anio.toString();
                            this.MesId = this.MesId.toString();
                            this.EsEquivalencia = this.EsEquivalencia.toString();
                            this.EscuelaEquivalencia = this.EscuelaEquivalencia.toString();
                            this.PaisId = this.PaisId.toString();
                            this.EntidadFederativaId = this.EntidadFederativaId.toString();
                            this.EsTitulado = this.EsTitulado.toString();
                            this.TitulacionMedio = this.TitulacionMedio.toString();
                            this.MedioDifusionId = this.MedioDifusionId.toString();
                            this.UsuarioId = this.UsuarioId.toString();

                            $.ajax({
                                url: 'WS/Alumno.asmx/GuardarAntecedentes',
                                type: 'POST',
                                contentType: 'application/json; charset=utf-8',
                                data: JSON.stringify(Antecendentes[i]),
                                dataType: 'json',
                                async: false,
                                success: function (data) { if (data.d) { TRes = true; } else { TRes = false; } }
                            });
                        });
                        if (TRes) {
                            var nombre = $('#hCarga');
                            nombre[0].innerText = "Cargando";
                            alertify.alert("Datos del Alumno Modificados");
                            $('#divGuardar').hide();
                            $('#divEditar').show();
                            var load = $('#Load').modal();
                            $('#txtAlumno').val(AlumnoNum);
                            $('#btnBuscarAlumno').click();
                        } else {
                            $('#Load').modal('hide');
                            var nombre = $('#hCarga');
                            nombre[0].innerText = "Cargando";
                            alertify.alert("Error, Revisar datos capturados.");
                        }

                    } else {
                        $('#Load').modal('hide');
                        var nombre = $('#hCarga');
                        nombre[0].innerText = "Cargando";
                        alertify.alert("Error, Revisar datos capturados.");
                    }
                }
                catch (error) {
                    console.log(error.message);
                    $('#Load').modal('hide');
                    var nombre = $('#hCarga');
                    nombre[0].innerText = "Cargando";
                    alertify.alert("Error, Revisar datos capturados.");
                }
            }
        });
    }

    $('#slcOfertaEducativa').change(function () {
        $('#Load').modal('show');
        CargarDescuentos(AlumnoNum, $('#slcOfertaEducativa').val());
    });
    function CargarDescuentos(AlumnoId, OfertaEducativa) {
        $.ajax({
            url: 'WS/Beca.asmx/DescuentosAnteriores',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:"' + AlumnoId + '",OfertaEducativaId:"' + OfertaEducativa + '"}',
            dataType: 'json',
            success: function (data) {
                if (tblBecas != null) {
                    tblBecas.fnClearTable();
                }

                tblBecas = $("#tblBecas").dataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "AnioPeriodoId" },
                        { "mDataProp": "DescripcionPeriodo" },
                        { "mDataProp": "SMonto" },
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
                        "emptyTable": "No hay registros para mostrar"
                    },
                    "order": [[2, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[1].style.textAlign = 'center';
                        row.childNodes[2].style.textAlign = 'center';
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'center';
                        //7466 557
                    }
                });
                $('#Load').modal('hide');
            }
        });
    }
    $('#chkUni').click(function () {
        var chk = this;
        chk = chk.checked;
        if (chk) {
            $('#txtUni').prop("disabled", false);
        } else {
            $('#txtUni').prop("disabled", true);

        }
    });
    $('#txtAlumno').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscarAlumno').click();
        }
    });
    /*Region */
    if (jQuery().datepicker) {
        $('.date-picker').datepicker({
            rtl: Metronic.isRTL(),
            orientation: "left",
            autoclose: true,
            language: 'es'
        });
        $(".date-picker").datepicker("setDate", Fecha);
        $('#spnA').text(calcular_edad(Fecha));
        //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
    }


    $('#txtFNacimiento').change(function () {
        var cumple = $('#txtFNacimiento').val();
        var a = calcular_edad(cumple);
        $('#spnA').text(a);
    });
    /*----------Funcion para obtener la edad------------*/
    function calcular_edad(fecha) {
        var fechaActual = new Date()
        var diaActual = fechaActual.getDate();
        var mmActual = fechaActual.getMonth() + 1;
        var yyyyActual = fechaActual.getFullYear();
        FechaNac = fecha.split("-");
        var diaCumple = FechaNac[0];
        var mmCumple = FechaNac[1];
        var yyyyCumple = FechaNac[2];

        var edad = yyyyActual - yyyyCumple;
        var meses = mmActual - mmCumple;

        if (meses < 0) {
            edad = edad - 1;
            meses = 12 + meses;
        }

        return edad + ' Años,' + meses + ' meses';
    }
    /*End Region*/
    form.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block help-block-error', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        rules: {
            txtnombre: {
                required: true,
                minlength: 3,
                maxlength: 50,
            },
            txtemail: {
                //required: true,
                email: true,
                minlength: 4,
                maxlength: 100
            },
            txtApPaterno: {
                required: true,
                digits: false,
                minlength: 2,
                maxlength: 50
            },
            txtApMaterno: {
                //required: true,
                digits: false,
                minlength: 2,
                maxlength: 50
            },
            slcEstadoCivil: {
                //required: true,
                min: 1
            },
            txtCelular: {
                //required: true,
                digits: true,
                minlength: 10,
                maxlength: 13
            },
            txtFNacimiento: {
                //required: true,
                digits: false,
            },
            slcSexo: {
                //required: true,
                min: 1
            },
            txtCURP: {
                //required: true,
                minlength: 1,
                maxlength: 18
            },
            txtTelefonoCasa: {
                digits: true,
                //required: true,
                maxlength: 10
            },
            txtCalle: {
                //required: true,
                maxlength: 100,
                minlength: 3
            },
            txtNumeroE: {
                //required: true,
                maxlength: 30,
                minlength: 1
            },
            txtNumeroI: {
                maxlength: 30,
                minlength: 1
            },
            txtCP: {
                //required: true,
                digits: true,
                minlength: 5
            },
            txtColonia: {
                //required: true,
                maxlength: 100,
                minlength: 1
            },
            slcMunicipio: {
                min: 1,
                //required: true,
            },
            slcEstado: {
                //required: true,
                min: 1
            },
            slcLugarN: {
                //required: true,
                min: 1
            },
            //Persona Autorizada
            txtPAutorizada: {
                //required: true,
                maxlength: 50,
                minlength: 1

            },
            txtAPPaterno: {
                //required: true,
                maxlength: 50,
                minlength: 1
            },
            txtAPMaterno: {
                //required: true,
                maxlength: 50,
                minlength: 1
            },
            slcParentesco: {
                //required: true,
                min: -1
            },
            txtPEmail: {
                maxlength: 100,
                minlength: 5
            },
            txtTelefonoPA: {
                digits: true,
                minlength: 8,
                maxlength: 10
            },
            txtTelefonoPAT: {
                digits: true,
                minlength: 8,
                maxlength: 10
            },
            //Autoriza 2
            txtPAutorizada2: {
                maxlength: 50,
                minlength: 1
            },
            txtAPPaterno2: {
                maxlength: 50,
                minlength: 1
            },
            txtAPMaterno2: {
                maxlength: 50,
                minlength: 1
            },
            slcParentesco2: {
                min: -1
            },
            txtPEmail2: {
                maxlength: 100,
                minlength: 5
            },
            txtTelefonoPA2: {
                digits: true,
                minlength: 8,
                maxlength: 10
            },
            txtTelefonoPAT2: {
                digits: true,
                minlength: 8,
                maxlength: 10
            },
            //Oferta Educativa
            slcPlantel: {
                //required: true,
                min: 1
            },
            slcOferta: {
                //required: true,
                min: 1
            },
            slcCarrera: {
                //required: true,
                min: 1
            },
            slcTurno: {
                //required: true,
                min: 1
            },
            slcPeriodo: {
            },
            txtNombrePrepa: {
                //required: true,
                maxlength: 100,
                minlength: 4
            },
            txtPromedio: {
                //required: true,
                number: true,
                minlength: 1,
                max: 10
            },
            txtAñoT: {
                //required: true,
                minlength: 4,
                maxlength:4
            },
            txtMesT: {
                //required: true,
                number: true,
                minlength: 1,
                maxlength: 2,
                min: 1,
                max: 12

            },
            slcArea: {
                min: 1
            },
            txtUni: {
                maxlength: 100,
                minlength: 2
            },
            InsArchivo: {
                extension: 'pdf'
            },
            BecArchivo: {
                extension: 'pdf'
            },
            slcNacionalidad: {
                //required: true,
                min: 1
            },
            slcNacionalidadPrep: {
                //required: true,
                min: 1
            },
            slcEstadoPais: {
                //required: true,
                min: 1
            },
            slcMedio: {
                //required: true,
                min: 1
            },
            slcPaisUni: {
                //required: true,
                min: 1
            }

        },

        invalidHandler: function (event, validator) { //display error alert on form submit       
            //alert('Empeze');
            success.hide();
            error.show();
            Metronic.scrollTo(error, -200);
        },
        errorPlacement: function (error, element) { // render error placement for each input type
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass('fa-check').addClass("fa-warning");
            icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
        },
        highlight: function (element) { // hightlight error inputs
            $(element)
                .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group   
            b_Valid = 1;
        },

        unhighlight: function (element) { // revert the change done by hightlight
        },

        success: function (label, element) {
            var icon = $(element).parent('.input-icon').children('i');
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
            icon.removeClass("fa-warning").addClass("fa-check");

        },

        submitHandler: function (form) {
            success.show();
            error.hide();
        }
    });
    $('a').click(function () {
        var tab = this;

        switch (tab.hash) {
            case '#tab1':
                $('#tab1').show();
                $('#tab2').hide();
                $('#tab3').hide();
                $('#tab4').hide();
                break;
            case '#tab2':
                $('#tab2').show();
                $('#tab1').hide();
                $('#tab3').hide();
                $('#tab4').hide();
                break;
            case '#tab3':
                $('#tab3').show();
                $('#tab2').hide();
                $('#tab1').hide();
                $('#tab4').hide();
                break;
            case '#tab4':
                $('#tab4').show();
                $('#tab2').hide();
                $('#tab3').hide();
                $('#tab1').hide();
                break;
        }
    });
    function CargarPaises(combo, PaisId) {
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarPaises",
            data: "{}", 
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.PaisId);

                    combo.append(option);
                });
                combo.val(PaisId);

            }
        });
    }
    function CargarEstados(combo, EstadoId) {
        combo.empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarEntidadFederativa",
            data: "{}",
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    combo.append(option);
                });
                combo.val(EstadoId);

            }
        });
    }
    function CargarEstados1(EstadoId, MunicipioId) {
        $('#slcEstado').empty();
        $.ajax({
            type: "POST",
            url: "WS/General.asmx/ConsultarEntidadFederativa",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.EntidadFederativaId);

                    $("#slcEstado").append(option);
                });

                $('#slcEstado').val(EstadoId);
                $("#slcMunicipio").empty();

                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarMunicipios",
                    data: "{EntidadFederativaId:'" + EstadoId + "'}",
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        var datos = data.d;
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));

                            option.text(this.Descripcion);
                            option.val(this.EntidadFederativaId);

                            $("#slcMunicipio").append(option);
                        });
                        $("#slcMunicipio").val(MunicipioId);

                    }
                });
            }
        });

    }

    $('#slcNacionalidad').change(function () {
        $("#slcLugarN").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcLugarN").append(optionP);

        var tipo = $("#slcNacionalidad");
        tipo = tipo[0].value;
        if (tipo == 2) {
            CargarPaises($("#slcLugarN"),-1);
        }
        else if (tipo == 1) {
            CargarEstados($("#slcLugarN"),-1);
        }
        else { $("#slcLugarN").append(optionP); }
    });
    $('#slcLugarUni').change(function () {
        $("#slcPaisUni").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcPaisUni").append(optionP);

        var tipo = $("#slcLugarUni");
        tipo = tipo[0].value;
        if (tipo == 2) {
            $('#lblLugarUni').html('Pais');
            CargarPaises($("#slcPaisUni"),-1);
        }
        else if (tipo == 1) {
            $('#lblLugarUni').html('Estado');
            CargarEstados($("#slcPaisUni"),-1);
        }
        else {
            $('#lblLugarUni').html(' ');
            $("#slcPaisUni").append(optionP);
        }
    });
    $('#slcNacionalidadPrep').change(function () {
        $("#slcEstadoPais").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcEstadoPais").append(optionP);

        var tipo = $("#slcNacionalidadPrep");
        tipo = tipo[0].value;
        if (tipo == 2) {
            $('#lblPN').html('País');
            CargarPaises($("#slcEstadoPais"),-1);
        }
        else if (tipo == 1) {
            $('#lblPN').html('Estado');
            CargarEstados($("#slcEstadoPais"),-1);
        }
        else {
            $('#lblPN').html('País | Estado');
            $("#slcEstadoPais").append(optionP);
        }
    });
    function OfertasAlumnoIni(data) {
        $("#slcOfertasAlumno").empty();
        $("#slcOfertasAlumno1").empty();
        var optionSe = $(document.createElement('option'));
        optionSe.text("--Selecciona--");
        optionSe.val("-1");
        $("#slcOfertasAlumno1").append(optionSe);

        var datos = data;
        $(datos).each(function () {
            var option = $(document.createElement('option'));
            var option1 = $(document.createElement('option'));

            option.text(this.Descripcion);
            option.val(this.OfertaEducativaId);
            option.data("tipoid", this.OfertaEducativaTipoId);
            option1.text(this.Descripcion);
            option1.val(this.OfertaEducativaId);
            option1.data("tipoid", this.OfertaEducativaTipoId);

            $("#slcOfertasAlumno").append(option);
            $("#slcOfertasAlumno1").append(option1);
        });
        if (datos.length === 1) {
            $("#slcOfertasAlumno1").val(datos[0].OfertaEducativaId);
            $("#slcOfertasAlumno1").change();
        }

    }
    function GuardarAntecedentesTmp() {
        if (OfertaEducativaTipoId === -1) { return false;}
        var usuario = $.cookie('userAdmin');
        var posobj = -2;
        $(Antecendentes).each(function (i) {
            if (this.AntecedenteTipoId.toString() === OfertaEducativaTipoId.toString()) {
                posobj = i;
                return false;
            } else if (OfertaEducativaTipoId !== -1 && OfertaEducativaTipoId !== 4) {
                posobj = -1;
            }
        });
        if (posobj === -1) {
            var objAntence =
               {
                   AlumnoId: AlumnoNum,
                   Anio: $("#txtAñoT").val(),
                   AntecedenteTipoId: OfertaEducativaTipoId,
                   AreaAcademicaId: $("#slcArea").val(),
                   EntidadFederativaId: $('#slcNacionalidadPrep').val() == -1 ? -1 :
                                $('#slcNacionalidadPrep').val() == 1 ? $('#slcEstadoPais').val():-1,
                   EscuelaEquivalencia: $('#txtUni').val(),
                   EsTitulado: ($('#chkUniSi').prop("checked") ? true : false),
                   FechaRegistro:"",
                   EsEquivalencia: ($('#chkUni').prop("checked") ? true : false),
                   MedioDifusionId: $('#slcMedio').val(),
                   MesId: $("#txtMesT").val(),
                   PaisId: $('#slcNacionalidadPrep').val() == -1 ? -1 :
                                $('#slcNacionalidadPrep').val() ==  1 ? 146 : $('#slcEstadoPais').val(),
                   Procedencia: $("#txtNombrePrepa").val(),
                   Promedio: $("#txtPromedio").val(),
                   TitulacionMedio: $('#txtUniMotivo').val(),
                   UsuarioId: usuario
               };
            Antecendentes.push(objAntence);
        }
        else if (posobj !== -2) {
            Antecendentes[posobj].Procedencia = $("#txtNombrePrepa").val();
            Antecendentes[posobj].Promedio = $("#txtPromedio").val();
            Antecendentes[posobj].Anio = $("#txtAñoT").val();
            Antecendentes[posobj].MesId = $("#txtMesT").val();
            Antecendentes[posobj].AreaAcademicaId = $("#slcArea").val();
            Antecendentes[posobj].EscuelaEquivalencia = $("#txtUni").val();
            Antecendentes[posobj].EsTitulado = ($('#chkUniSi').prop("checked") ? true : false);
            
            if ($('#slcNacionalidadPrep').val() === 1) {
                Antecendentes[posobj].PaisId = 146;
                Antecendentes[posobj].EntidadFederativaId = $('#slcEstadoPais').val();
            } else if($('#slcNacionalidadPrep').val() === 2) {
                Antecendentes[posobj].PaisId = $('#slcEstadoPais').val();
            }
            Antecendentes[posobj].EsEquivalencia = ($('#chkUni').prop("checked") ? true : false);
            Antecendentes[posobj].MedioDifusionId = $('#slcMedio').val();
            Antecendentes[posobj].TitulacionMedio = $('#txtUniMotivo').val();
        }
    }
    $("#slcOfertasAlumno1").change(function () {
        $('#submit_form  div').removeClass('has-error');
        $('#submit_form  div').removeClass('has-success');
        $('#submit_form  i').removeClass('fa-warning');
        $('#submit_form  i').removeClass('fa-check');
        GuardarAntecedentesTmp();
        $('#divAntecendentes  input').val("");
        $('#divAntecendentes select').val('-1');
        $('#txtUniMotivo').prop("disabled", true);
        $('#txtUni').prop("disabled", true);

        $('#chkUniNo').prop("checked", true);
        var spam2U = $('#chkUniNo')[0].parentElement;
        $(spam2U).addClass('checked');
        $('#chkUniSi').prop("checked", false);
        var spamu = $('#chkUniSi')[0].parentElement;
        $(spamu).removeClass('checked');

        $('#chkUni').prop("checked", false);
        var spam1U = $('#chkUni')[0].parentElement;
        $(spam1U).removeClass('checked');

        var vap = $("#slcOfertasAlumno1").val();
        var Tipod = $("#slcOfertasAlumno1 option:selected")[0];
        Tipod = $(Tipod).data("tipoid");
        OfertaEducativaTipoId = Tipod;
        if (vap != -1 && Tipod != 4) {
            $('#hOfertaTitulo').text(Tipod === 1 ? "Preparatoria de Procedencia" : "Universidad de Procedencia");
            $('#lblOfertaTitulo2').text(Tipod === 1 ? "Preparatoria de Procedencia" : "Universidad de Procedencia");
            $('#lblLugarP').text(Tipod === 1 ? "Lugar donde estudio la preparatoria" : "Lugar donde estudio la universidad");

            //Datos

            $(Antecendentes).each(function () {
                if (this.AntecedenteTipoId === Tipod) {
                    $("#txtNombrePrepa").val(this.Procedencia);
                    $("#txtPromedio").val(this.Promedio);
                    $("#txtAñoT").val(this.Anio);
                    $("#txtMesT").val(this.MesId);
                    $("#slcArea").val(this.AreaAcademicaId);
                    $("#txtUni").val(this.EscuelaEquivalencia);
                    
                    if (this.PaisId === 146) {
                        $("#slcNacionalidadPrep").val(1);
                        CargarEstados($('#slcEstadoPais'), this.EntidadFederativaId);
                    } else if(this.PaisId!==-1) {
                        $("#slcNacionalidadPrep").val(2);
                        CargarPaises($('#slcEstadoPais'), this.PaisId)
                    } 
                    $("#slcMedio").val(this.MedioDifusionId);
                    if (this.EsTitulado == true) {
                        $('#chkUniSi').prop("checked", true);
                        var spam2 = $('#chkUniSi')[0].parentElement;
                        $(spam2).addClass('checked');

                        $('#chkUniNo').prop("checked", false);
                        var spam = $('#chkUniNo')[0].parentElement;
                        $(spam).removeClass('checked');
                        $('#txtUniMotivo').val(this.TitulacionMedio);
                        $('#txtUniMotivo').prop("disabled", false);

                    } else {
                        $('#chkUniNo').prop("checked", true);
                        var spam2 = $('#chkUniNo')[0].parentElement;
                        $(spam2).addClass('checked');

                        $('#chkUniSi').prop("checked", false);
                        var spam = $('#chkUniSi')[0].parentElement;
                        $(spam).removeClass('checked');
                    }
                    if (this.EsEquivalencia) {
                        $('#chkUni').prop("checked", true);
                        var spam1 = $('#chkUni')[0].parentElement;
                        $(spam1).addClass('checked');
                        $('#txtUni').prop("disabled", false);
                    }

                    return false;
                }
            });
            $('#divAntecendentes').show();

        } else { $('#divAntecendentes').hide(); }
    });
    
});
