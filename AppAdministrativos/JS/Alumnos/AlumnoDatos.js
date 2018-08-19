$(function () {
    var Now = new Date();
    var años = Now.getFullYear() - 18;
    var mes = Now.getMonth() + 1;
    var Fecha = Now.getDate() + '-' + mes + '-' + años;

    var tblBecas, AlumnoNum, tblAlumnos, Antecendentes;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);

    var OfertaEducativaTipoId = -1;
    var AlumnoOrigin = undefined;
    /*Region */
    
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
                maxlength: 12
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
                maxlength: 12
            },
            txtTelefonoPAT: {
                digits: true,
                minlength: 8,
                maxlength: 12
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
                maxlength: 12
            },
            txtTelefonoPAT2: {
                digits: true,
                minlength: 8,
                maxlength: 12
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
                maxlength: 4
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

    var AlumnoDatosFn = {
        init() {
            GlobalFn.GetMedios();
            GlobalFn.GetGenero();
            GlobalFn.GetPeriodo_N_I();
            GlobalFn.GetEstadoCivil();
            GlobalFn.GetAreas("slcArea");
            GlobalFn.GetParentesco("slcParentesco");
            GlobalFn.GetParentesco("slcParentesco2");

            this.BloquearTodo();
            $('#btnBuscarAlumno').click(this.btnBuscarAlumnoClick);
            $('#tblAlumnos').on('click', 'a', this.tblAlumnosClickA);
            $('#btnEditar').click(this.btnEditarClick);
            $('#Guardar').click(this.BtnGuardarClick);
            $('#slcOfertaEducativa').change(this.slcOfertaEducativaChange);
            $('#chkUni').click(this.chkUniClick);
            $('#txtAlumno').on('keydown', this.txtAlumnoKeydown);
            $('#txtFNacimiento').change(this.txtFNacimientoChange);
            $('li a').click(this.TabsAClick);
            $('#slcNacionalidad').change(this.slcNacionalidadChange);
            $('#slcLugarUni').change(this.slcLugarUniChnage);
            $('#slcNacionalidadPrep').change(this.slcNacionalidadPrepChange);
            $("#slcOfertasAlumno1").change(this.slcOfertasAlumno1Change);
        },
        LimpiarCampos() {
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
        },
        EsNumero(Alumno) {


            IndexFn.Api("Alumno/ObtenerDatosAlumno/" + Alumno, "GET", "")
                .done(function (data) {
                    if (data != null) {
                        AlumnoOrigin = {
                            alumnoId: data.AlumnoId,
                            nombre: data.Nombre,
                            paterno: data.Paterno,
                            materno: data.Materno,
                            curp: data.DTOAlumnoDetalle.CURP
                        };

                        Antecendentes = data.Antecendentes;
                        AlumnoDatosFn.OfertasAlumnoIni(data.lstOfertas);
                        $('#divEditar').show();
                        $('#MenuTab').show();
                        $('#slcNacionalidad').val(data.DTOAlumnoDetalle.PaisId == 146 ? 1 : 2);
                        if (data.DTOAlumnoDetalle.PaisId == 146) {
                            GlobalFn.GetEstado("slcLugarN", data.DTOAlumnoDetalle.EntidadNacimientoId);
                        } else {
                            GlobalFn.GetPais("slcLugarN", data.DTOAlumnoDetalle.PaisId);
                        }
                        ///Personales 
                        document.getElementById("fotoAlumno").src = "data:image/png;base64," + data.DTOAlumnoDetalle.fotoBase64;
                        $('#txtMatricula').val(data.Matricula);
                        $('#txtnombre').val(data.Nombre);
                        $('#txtApPaterno').val(data.Paterno);
                        $('#txtApMaterno').val(data.Materno);
                        $('#txtCelular').val(data.DTOAlumnoDetalle.Celular);
                        $('#txtFNacimiento').val(data.DTOAlumnoDetalle.FechaNacimientoC);
                        $('#txtFNacimiento').change();
                        $('#txtCURP').val(data.DTOAlumnoDetalle.CURP);
                        $('#txtEmail').val(data.DTOAlumnoDetalle.Email);
                        $('#txtTelefonoCasa').val(data.DTOAlumnoDetalle.TelefonoCasa);
                        $('#txtObservaciones').val(data.DTOAlumnoDetalle.Observaciones);

                        $('#txtCalle').val(data.DTOAlumnoDetalle.Calle);
                        $('#txtNumeroE').val(data.DTOAlumnoDetalle.NoExterior);
                        $('#txtNumeroI').val(data.DTOAlumnoDetalle.NoInterior);
                        $('#txtCP').val(data.DTOAlumnoDetalle.Cp);
                        $('#txtColonia').val(data.DTOAlumnoDetalle.Colonia);

                        $('#slcEstadoCivil').val(data.DTOAlumnoDetalle.EstadoCivilId);
                        $('#slcSexo').val(data.DTOAlumnoDetalle.GeneroId);
                        //$('#slcEstado').val(data.DTOAlumnoDetalle.EntidadFederativaId);
                        //CargarEstados1($('#slcMunicipio'), data.DTOAlumnoDetalle.MunicipioId);
                        AlumnoDatosFn.CargarEstados1(data.DTOAlumnoDetalle.EntidadFederativaId, data.DTOAlumnoDetalle.MunicipioId);

                        if (data.DTOPersonaAutorizada.length > 0) {
                            $('#txtPAutorizada').val(data.DTOPersonaAutorizada[0].Nombre);
                            $('#txtAPPaterno').val(data.DTOPersonaAutorizada[0].Paterno);
                            $('#txtAPMaterno').val(data.DTOPersonaAutorizada[0].Materno);
                            $('#slcParentesco').val(data.DTOPersonaAutorizada[0].ParentescoId);
                            $('#txtPEmail').val(data.DTOPersonaAutorizada[0].Email);
                            $('#txtTelefonoPA').val(data.DTOPersonaAutorizada[0].Celular == null ? "" : data.DTOPersonaAutorizada[0].Celular);
                            $('#txtTelefonoPAT').val(data.DTOPersonaAutorizada[0].Telefono);

                            if (data.DTOPersonaAutorizada[0].Autoriza == true) {
                                $('#chkAuotiza1').prop("checked", true);
                                var spq = $('#chkAuotiza1')[0].parentElement;
                                $(spq).addClass('checked');
                            }
                            if (data.DTOPersonaAutorizada.length > 1) {
                                $('#txtPAutorizada2').val(data.DTOPersonaAutorizada[1].Nombre);
                                $('#txtAPPaterno2').val(data.DTOPersonaAutorizada[1].Paterno);
                                $('#txtAPMaterno2').val(data.DTOPersonaAutorizada[1].Materno);
                                $('#slcParentesco2').val(data.DTOPersonaAutorizada[1].ParentescoId);
                                $('#txtPEmail2').val(data.DTOPersonaAutorizada[1].Email);
                                $('#txtTelefonoPA2').val(data.DTOPersonaAutorizada[1].Celular);
                                $('#txtTelefonoPAT2').val(data.DTOPersonaAutorizada[1].Telefono);

                                if (data.DTOPersonaAutorizada[1].Autoriza == true) {
                                    $('#chkAuotiza2').prop("checked", true);
                                    var spam = $('#chkAuotiza2')[0].parentElement;
                                    $(spam).addClass('checked');
                                }
                            }

                        }

                        //$('#').val(data.d);
                        //OfertasAlumno(Alumno);
                        IndexFn.Block(false);
                        $('#tab1btn').click();
                    }
                    else {
                        document.getElementById("fotoAlumno").src = "";
                        IndexFn.Block(false);
                        alertify.alert("Error, El Alumno no Existe.");

                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Error, El Alumno no Existe.");
                });
            
        },
        EsString(Alumno) {
            $('#tab1').hide();

            IndexFn.Api("Alumno/BuscarAlumnoString/" + Alumno, "GET", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    if (data != null) {
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
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    alertify.alert("Error, No se han encontrado resultados.");
                });
        },
        BloquearTodo() {
            $('#MenuTab').hide();
            $('#submit_form  input').prop("disabled", true);
            $('#submit_form  select').prop("disabled", true);
            $('#submit_form  button').prop("disabled", true);
            $('#submit_form  textarea').prop("disabled", true);
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
            
        },
        ValidarTodos() {
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
            if (count == 4) { return true; } else { return false; }
        },
        UpdateComplemento() {
            if (AlumnoOrigin.nombre !== $('#txtnombre').val()
                || AlumnoOrigin.paterno !== $('#txtApPaterno').val()
                || AlumnoOrigin.materno !== $('#txtApMaterno').val()
                || AlumnoOrigin.curp !== $('#txtCURP').val()) {

                AlumnoOrigin.nombre = $('#txtnombre').val().trim() + " " + $('#txtApPaterno').val().trim() + " " + $('#txtApMaterno').val().trim();
                AlumnoOrigin.curp = $('#txtCURP').val();
                AlumnoOrigin.curp = AlumnoOrigin.curp.replace(/\s/g, "");
                if (AlumnoOrigin.curp.length === 18) {
                    $.ajax({
                        url: 'http://108.163.172.122/YPlatform/ResourceComprobante/api/Universal/EditComplementoEducativo',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(AlumnoOrigin),
                        dataType: 'json',
                    }).done(function (data) {
                        console.log(data);
                    }).fail(function (data) {
                        console.log(data.responseJSON);
                    }).always(function (data) {
                        console.log(data.responseJSON);
                    });
                }
            }
        },
        GuardarTodo() {

            AlumnoDatosFn.UpdateComplemento();

            var objAlumno = {
                Nombre: $('#txtnombre').val(),
                Paterno: $('#txtApPaterno').val(),
                Materno: $('#txtApMaterno').val(),
                AlumnoId: AlumnoNum,
                UsuarioId:  localStorage.getItem('userAdmin'),
                DTOAlumnoDetalle: {
                    AlumnoId: AlumnoNum,
                    ProspectoId: AlumnoNum,
                    Celular: $('#txtCelular').val(),
                    FechaNacimientoC: $('#txtFNacimiento').val(),

                    CURP: $('#txtCURP').val(),
                    Email: $('#txtEmail').val(),
                    TelefonoCasa: $('#txtTelefonoCasa').val(),
                    TelefonoOficina: "",

                    Calle: $('#txtCalle').val(),
                    NoExterior: $('#txtNumeroE').val(),
                    NoInterior: $('#txtNumeroI').val(),
                    Cp: $('#txtCP').val(),
                    Colonia: $('#txtColonia').val(),

                    EstadoCivilId: $('#slcEstadoCivil').val(),
                    GeneroId: $('#slcSexo').val(),
                    EntidadFederativaId: $('#slcEstado').val(),
                    MunicipioId: $('#slcMunicipio').val(),
                    PaisId: parseInt($('#slcNacionalidad').val()) == 1 ? 146 : $('#slcLugarN').val(),
                    EntidadNacimientoId: parseInt($('#slcNacionalidad').val()) == 1 ? $('#slcLugarN').val() : null,
                    Observaciones: $('#txtObservaciones').val()
                },
                DTOPersonaAutorizada: [{
                    AlumnoId: AlumnoNum,
                    Nombre: $('#txtPAutorizada').val(),
                    Paterno: $('#txtAPPaterno').val(),
                    Materno: $('#txtAPMaterno').val(),
                    ParentescoId: $('#slcParentesco').val(),
                    Email: $('#txtPEmail').val(),
                    Celular: $('#txtTelefonoPA').val(),
                    Telefono: $('#txtTelefonoPAT').val(),
                    Autoriza: ($('#chkAuotiza1').prop("checked") ? 'true' : 'false'),
                }]
            };

            if ($('#txtPAutorizada2').val().length > 0) {
                objAlumno.DTOPersonaAutorizada.push({
                    AlumnoId: AlumnoNum,
                    Nombre: $('#txtPAutorizada2').val(),
                    Paterno: $('#txtAPPaterno2').val(),
                    Materno: $('#txtAPMaterno2').val(),
                    ParentescoId: $('#slcParentesco2').val(),
                    Email: $('#txtPEmail2').val(),
                    Celular: $('#txtTelefonoPA2').val(),
                    Telefono: $('#txtTelefonoPAT2').val(),
                    Autoriza: ($('#chkAuotiza2').prop("checked") ? 'true' : 'false')
                });
            }

            AlumnoDatosFn.GuardarAntecedentesTmp();
            ///////////////////////////
            IndexFn.Api("Alumno/UpdateAlumno", "POST", JSON.stringify(objAlumno))
                .done(function (data) {
                    try {
                        if (data) {
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

                                alertify.alert("Datos del Alumno Modificados");
                                $('#divGuardar').hide();
                                $('#divEditar').show();
                                var load = $('#Load').modal();
                                $('#txtAlumno').val(AlumnoNum);
                                $('#btnBuscarAlumno').click();
                            } else {
                                IndexFn.Block(false);

                                alertify.alert("Error, Revisar datos capturados.");
                            }

                        } else {
                            IndexFn.Block(false);
                            alertify.alert("Error, Revisar datos capturados.");
                        }
                    }
                    catch (error) {
                        console.log(error.message);
                        IndexFn.Block(false);

                        alertify.alert("Error, Revisar datos capturados.");
                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);

                    alertify.alert("Error, Revisar datos capturados.");
                });
        },
        CargarDescuentos(AlumnoId, OfertaEducativa) {
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
                    IndexFn.Block(false);
                }
            });
        },
        calcular_edad(fecha) {
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
        },
        CargarEstados1(EstadoId, MunicipioId) {
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

        },
        OfertasAlumnoIni(data) {
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

        },
        GuardarAntecedentesTmp() {
            if (OfertaEducativaTipoId === -1) { return false; }
            var usuario =  localStorage.getItem('userAdmin');
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
                            $('#slcNacionalidadPrep').val() == 1 ? $('#slcEstadoPais').val() : -1,
                        EscuelaEquivalencia: $('#txtUni').val(),
                        EsTitulado: ($('#chkUniSi').prop("checked") ? true : false),
                        FechaRegistro: "",
                        EsEquivalencia: ($('#chkUni').prop("checked") ? true : false),
                        MedioDifusionId: $('#slcMedio').val(),
                        MesId: $("#txtMesT").val(),
                        PaisId: $('#slcNacionalidadPrep').val() == -1 ? -1 :
                            $('#slcNacionalidadPrep').val() == 1 ? 146 : $('#slcEstadoPais').val(),
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
                } else if ($('#slcNacionalidadPrep').val() === 2) {
                    Antecendentes[posobj].PaisId = $('#slcEstadoPais').val();
                }
                Antecendentes[posobj].EsEquivalencia = ($('#chkUni').prop("checked") ? true : false);
                Antecendentes[posobj].MedioDifusionId = $('#slcMedio').val();
                Antecendentes[posobj].TitulacionMedio = $('#txtUniMotivo').val();
            }
        },
        btnBuscarAlumnoClick() {
            AlumnoOrigin = undefined;
            $('#frmVarios').hide();
            if (tblAlumnos != undefined) {
                tblAlumnos.fnClearTable();
            }
            if ($('#txtAlumno').val().length == 0) { return false; }
            AlumnoDatosFn.LimpiarCampos();
            $("#slcOfertaEducativa").empty();
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcOfertaEducativa").append(optionP);
            AlumnoDatosFn.BloquearTodo();


            IndexFn.Block(true);
            AlumnoNum = $('#txtAlumno').val();
            if (tblBecas != null) {
                tblBecas.fnClearTable();
            }

            if (!isNaN(AlumnoNum)) {
                AlumnoDatosFn.EsNumero(AlumnoNum);
            } else {
                AlumnoDatosFn.EsString(AlumnoNum);
            }
        },
        tblAlumnosClickA() {
            $('#frmVarios').hide();
            IndexFn.Block(true);
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
            AlumnoNum = rowadd.AlumnoId;
            AlumnoDatosFn.EsNumero(rowadd.AlumnoId);
        }, 
        btnEditarClick() {
            $('#divEditar').hide();
            $('#submit_form  input').prop("disabled", false);
            $('#submit_form  select').prop("disabled", false);
            $('#submit_form  button').prop("disabled", false);
            $('#submit_form  textarea').prop("disabled", false);
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

        },
        BtnGuardarClick() {

            IndexFn.Block(true);

            var estatus = AlumnoDatosFn.ValidarTodos();
            if (estatus) {
                AlumnoDatosFn.GuardarTodo();
            }
            else { IndexFn.Block(false); }
        },
        slcOfertaEducativaChange() {
            IndexFn.Block(true);
            AlumnoDatosFn.CargarDescuentos(AlumnoNum, $('#slcOfertaEducativa').val());
        },
        chkUniClick() {
            var chk = this;
            chk = chk.checked;
            if (chk) {
                $('#txtUni').prop("disabled", false);
            } else {
                $('#txtUni').prop("disabled", true);

            }
        },
        txtAlumnoKeydown(e) {
            if (e.which == 13) {
                $('#btnBuscarAlumno').click();
            }
        },
        txtFNacimientoChange() {
            var cumple = $('#txtFNacimiento').val();
            var a = AlumnoDatosFn.calcular_edad(cumple);
            $('#spnA').text(a);
        },
        TabsAClick() {
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
        },
        slcNacionalidadChange() {
            $("#slcLugarN").empty();
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcLugarN").append(optionP);

            var tipo = $("#slcNacionalidad");
            tipo = tipo[0].value;
            if (tipo == 2) {
                GlobalFn.GetPais("slcLugarN", 146);
            }
            else if (tipo == 1) {
                GlobalFn.GetEstado("slcLugarN", 9);
            }
            else { $("#slcLugarN").append(optionP); }
        },
        slcLugarUniChnage() {
            $("#slcPaisUni").empty();
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcPaisUni").append(optionP);

            var tipo = $("#slcLugarUni");
            tipo = tipo[0].value;
            if (tipo == 2) {
                $('#lblLugarUni').html('Pais');
                GlobalFn.GetPais("slcPaisUni", 146);
            }
            else if (tipo == 1) {
                $('#lblLugarUni').html('Estado');
                GlobalFn.GetEstado("slcPaisUni", 9);
            }
            else {
                $('#lblLugarUni').html(' ');
                $("#slcPaisUni").append(optionP);
            }
        },
        slcNacionalidadPrepChange() {

            var tipo = $("#slcNacionalidadPrep");
            tipo = tipo[0].value;
            if (tipo == 2) {
                $('#lblPN').html('País');
                GlobalFn.GetPais($("#slcEstadoPais"), 146);
            }
            else if (tipo == 1) {
                $('#lblPN').html('Estado');
                GlobalFn.GetEstado($("#slcEstadoPais"), 9);
            }
            else {
                $('#lblPN').html('País | Estado');
                $("#slcEstadoPais").empty();
            }
        },
        slcOfertasAlumno1Change() {
            $('#submit_form  div').removeClass('has-error');
            $('#submit_form  div').removeClass('has-success');
            $('#submit_form  i').removeClass('fa-warning');
            $('#submit_form  i').removeClass('fa-check');
            AlumnoDatosFn.GuardarAntecedentesTmp();
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
                            GlobalFn.GetEstado("slcEstadoPais", this.EntidadFederativaId);
                        } else if (this.PaisId !== -1) {
                            $("#slcNacionalidadPrep").val(2);
                            GlobalFn.GetPais("slcEstadoPais", this.PaisId);
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
        }
    };

    if (jQuery().datepicker) {
        $('.date-picker').datepicker({
            rtl: Metronic.isRTL(),
            orientation: "left",
            autoclose: true,
            language: 'es'
        });
        $(".date-picker").datepicker("setDate", Fecha);
        $('#spnA').text(AlumnoDatosFn.calcular_edad(Fecha));
    }

    AlumnoDatosFn.init();

});
