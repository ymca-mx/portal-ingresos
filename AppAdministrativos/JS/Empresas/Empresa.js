$(document).ready(function () {
    function Anterior() {
        var Seleccionados = "";
        var MItable;
        var Alumnos;
        var AlumnosEmpresa;
        var MiGrupo;
        var Fila;
        var EmpresaI;
        var InscripcionID;
        var MesP = [];
        var form = $('#formPopup');
        var GrupoFrm = $('#frmGrupo');
        var error = $('.alert-danger', form);
        var error1 = $('.alert-danger', GrupoFrm);
        var success = $('.alert-success', form);
        var success1 = $('.alert-success', GrupoFrm);
        Cargar();
        CargarSede();
        var Now = new Date();
        var años = Now.getFullYear();// - 18;
        var mes = Now.getMonth() + 1;
        var Fecha = Now.getDate() + '-' + mes + '-' + años;

        if (jQuery().datepicker) {
            $('.date-picker').datepicker({
                rtl: Metronic.isRTL(),
                orientation: "left",
                autoclose: true,
                language: 'es'
            });
            $(".date-picker").datepicker("setDate", Fecha);
            //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
        }
        form.validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block help-block-error', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                txtDescripcion: {
                    required: true,
                    minlength: 5
                },
                txtRFC: {
                    required: true,
                    minlength: 13,
                    maxlength: 13
                },
                txtmail: {
                    email: true,
                    required: true
                },
                txtCalle: {
                    required: true
                },
                txtCP: {
                    required: true,
                    digits: true,
                    minlength: 5
                },
                NoExterior: {
                    required: true
                },
                NoInterior: {
                    required: false
                },
                slcPaisUni: {
                    required: true,
                    min: 1
                },
                slcEstadoPais: {
                    required: true,
                    min: 1
                },
                slcDelegacion: {
                    required: true,
                    min: 1
                },
                txtObservacion: {
                    required: false
                },
                txtColonia: {
                    required: true
                },
                txtNombre: {
                    required: true,
                },
                txtPaterno: {
                    required: true
                },
                txtMaterno: {
                    required: true
                },
                txtEmail: {
                    required: true,
                    email: true
                },
                txtTelefono: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 10
                },
                txtCelular: {
                    required: true,
                    digits: true,
                    minlength: 10,
                    maxlength: 10
                }
            },
            invalidHandler: function (event, validator) { //display error alert on form submit              
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
        GrupoFrm.validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block help-block-error', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                txtNombreGrupo: {
                    required: true,
                    minlength: 5
                },
                slcPlantel: {
                    required: true,
                    min: 1
                },
                txtDireccion: {
                    required: true,
                    minlength: 5
                },
                slcTipo: {
                    required: true,
                    min: 1
                },
                slcOferta: {
                    required: true,
                    min: 1
                },
                txtFechaInicio: {
                    required: true
                },
                txtNPagos: {
                    required: true,
                    digits: true
                },
                BecArchivo: {
                    extension: 'pdf|jpg|png|jpeg'
                }
            },
            invalidHandler: function (event, validator) { //display error alert on form submit              
                success1.hide();
                error1.show();
                Metronic.scrollTo(error1, -200);
            },
            errorPlacement: function (error1, element) { // render error placement for each input type
                var icon = $(element).parent('.input-icon').children('i');
                icon.removeClass('fa-check').addClass("fa-warning");
                icon.attr("data-original-title", error1.text()).tooltip({ 'container': 'body' });
            },
            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group   
            },

            unhighlight: function (element) { // revert the change done by hightlight
            },

            success: function (label, element) {
                var icon = $(element).parent('.input-icon').children('i');
                $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                icon.removeClass("fa-warning").addClass("fa-check");
            },

            submitHandler: function (GrupoFrm) {
                success1.show();
                error1.hide();
            }
        });
        function CargarSede() {
            $("#slcPlantel").empty();
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarPlantelEmpresas",
                data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.DescripcionId);
                        option.val(this.SucursalId);

                        $("#slcPlantel").append(option);
                    });
                    //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                    $("#slcPlantel").val('1');
                    $('#slcPlantel').change();
                }
            });
        }
        function Cargar() {
            $.ajax({
                url: 'WS/Empresa.asmx/ListarEmpresas',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{}',
                dataType: 'json',
                success: function (Respuesta) {
                    MItable = $('#tblEmpresa').dataTable({
                        "aaData": Respuesta.d,
                        "aoColumns": [
                            { "mDataProp": "EmpresaId", "RazonSocial": "EmpresaId" },
                            {
                                "mDataProp": "RazonSocial",
                                "mRender": function (data) {
                                    return "<a href=''onclick='return false;'>" + data + " </a> ";
                                }
                            },
                            { "mDataProp": "FechaAltaS" },
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": true,
                        "ordering": true,
                        "info": false,
                        "async": true,
                        "bDestroy": true,
                        "language": {
                            "lengthMenu": "_MENU_  Registros",
                            "paginate": {
                                "previous": "<",
                                "next": ">"
                            },
                            "search": "Buscar Empresa "
                        },
                        "order": [[2, "desc"]]
                    });
                    Estados();
                    Paises();
                },
                error: function (Respuesta) {
                    alertify.alert('Error al cargar datos', function () {
                        return false;
                    });
                }
            });
        }
        function Estados() {
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
                        var option1 = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.EntidadFederativaId);
                        option1.text(this.Descripcion);
                        option1.val(this.EntidadFederativaId);

                        $("#slcEstadoPais").append(option);
                        $('#slcEstadoPaisFiscal').append(option1);
                    });
                    //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                    $('#slcEstadoPaisFiscal').val('9');
                    $('#slcEstadoPaisFiscal').change();
                    $("#slcEstadoPais").val('9');
                    $('#slcEstadoPais').change();
                }
            });
        }
        function Paises() {
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarPaisesT",
                data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));
                        var option1 = $(document.createElement('option'));
                        option.text(this.Descripcion);
                        option.val(this.PaisId);
                        option1.text(this.Descripcion);
                        option1.val(this.PaisId);

                        $('#slcPaisUni').append(option);
                        $('#slcPaisUniFiscal').append(option1);
                    });
                    //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper                
                    $('#slcPaisUni').val('146');
                    $('#slcPaisUniFiscal').val('146');
                }
            });
        }
        function CalcularDescuento(Monto, Descuento) {
            Monto = parseFloat(Monto);
            Descuento = (Monto * (Descuento / 100))
            return Monto - Descuento;
        }

        function RecalculaTabla(monto) {
            if (MesP.length == 0) {
                for (i = 0; i < 4; i++) {
                    MesP[i] = $('#mes' + i).text().replace('$', '');
                }
            }
            var filx;
            var descu;
            for (i = 0; i < 4; i++) {
                if (MesP[i] != '0.00') {
                    filx = $('#mes' + i);
                    descu = CalcularDescuento(MesP[i], monto);
                    $(filx).text('$' + String(descu));
                }
            }
        }

        function CargarDireccion() {
            var id = $("#slcPlantel").val();
            $.ajax({
                type: "POST",
                url: "WS/Empresa.asmx/DireccionEmpresa",
                data: "{SucursalId:'" + id + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    $('#txtDireccion').val(datos);
                }
            });
        }

        function CargarTipo() {
            $('#slcTipo').empty();
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarOfertaEducativaTipo",
                data: "{Plantel:'" + $("#slcPlantel").val() + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.OfertaEducativaTipoId);

                        $("#slcTipo").append(option);
                    });
                    //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                    $("#slcTipo").change();
                }
            });
        }

        function CargarDescuento(Oferta) {
            var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/TraerDescuentosPeriodo",
                data: "{'OfertaEducativaId':" + Oferta + ",Periodo:'" + Periodo + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var monto;
                    //$('#txtcuotaCol').val('$' + data.d[1].Monto);
                    //monto = data.d[1].Monto * (parseFloat($('#txtDescuentoBec').val()) / 100);
                    //monto = data.d[1].Monto - monto;
                    //$('#txtPagarCol').text('$' + String(monto));
                    InscripcionID = 800;//data.d[0].CuotaId;
                    $('#txtcuotaIn').val('$' + data.d[0].Monto);
                    monto = (data.d[0].Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                    monto = data.d[0].Monto - monto;
                    $('#txtPagarIn').text('$' + String(monto));

                    //$('#txtcuotaExa').text('$' + data.d[2].Monto);
                    //monto = (data.d[2].Monto * (parseFloat($('#txtDescuentoExa').val()) / 100));
                    //monto = data.d[2].Monto - monto;
                    //$('#txtPagarExa').text('$' + String(monto));

                    //$('#txtcuotaCred').text('$' + data.d[3].Monto);
                    //monto = (data.d[3].Monto * (parseFloat($('#txtDescuentoCred').val()) / 100));
                    //monto = data.d[3].Monto - monto;
                    //$('#txtPagarCred').text('$' + String(monto));

                }
            });
        }

        function CargarDescuentoIdiomas(idioma) {
            var Periodo = $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();
            $.ajax({
                type: "POST",
                url: "WS/Descuentos.asmx/TraerDescuentosIdiomas",
                data: "{'Idioma':" + idioma + ",Periodo:'" + Periodo + "'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d.length > 0) {
                        InscripcionID = 807;//data.d[0].CuotaId;
                        $('#txtcuotaIn').val('$' + data.d[0].Monto);
                        monto = (data.d[0].Monto * (parseFloat($('#txtDescuentoIns').val()) / 100));
                        monto = data.d[0].Monto - monto;
                        $('#txtPagarIn').text('$' + String(monto));
                    }
                    else {
                        $('#txtcuotaCol').text('');
                        $('#txtPagarCol').text('');
                        $('#txtcuotaCred').text('');
                        $('#txtPagarCred').text('');
                    }
                }
            });
        }

        function CargarGrupo(EmpresaId) {
            $.ajax({
                url: 'WS/Empresa.asmx/ListarGrupos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: "{EmpresaId:'" + EmpresaId + "'}",
                dataType: 'json',
                success: function (Respuesta) {
                    MiGrupo = $('#tblGrupo').dataTable({
                        "aaData": Respuesta.d,
                        "aoColumns": [
                            { "mDataProp": "GrupoId", "Descripcion": "GrupoId" },
                            {
                                "mDataProp": "Descripcion",
                                "mRender": function (data) {
                                    return "<a name='btnAlumos'href=''onclick='return false;'>" + data + " </a> ";
                                }
                            },
                            { "mDataProp": "OfertaEducativa.Descripcion" },
                            { "mDataProp": "FechaInicioS" },
                            {
                                "mDataProp": function (data) {
                                    return "<a class='btn yellow' name ='btnModificar'>Modificar</a>";
                                }
                            }
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": false,
                        "ordering": true,
                        "info": false,
                        "async": true,
                        "bDestroy": true,
                        "language": {
                            "lengthMenu": "_MENU_  Registros",
                            "paginate": {
                                "previous": "<",
                                "next": ">"
                            },
                            "search": "Buscar Empresa "
                        },
                        "order": [[2, "desc"]]
                    });
                },
                error: function (Respuesta) {
                    alertify.alert('Error al cargar datos', function () {
                        return false;
                    });
                }
            });
        }

        function CargarAlumnoParaEmpresa(grupoId) {
            $.ajax({
                url: 'WS/Alumno.asmx/ConsultarAlumnosEmpresa',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: "{grupoId:'" + grupoId + "'}",
                dataType: 'json',
                success: function (Respuesta) {
                    Alumnos = $('#tblAlumnos').dataTable({
                        "aaData": Respuesta.d,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId", "Nombre": "AlumnoId", visible: false },
                            { "mDataProp": "Nombre" },
                             { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                            { "mDataProp": "FechaRegistro" },
                            //{ "mDataProp": "FechaSeguimiento" },
                            { "mDataProp": "Usuario.Nombre" },
                            {
                                "mDataProp": "AlumnoId", "Nombre": "AlumnoId",
                                "mRender": function (data) {
                                    return '<button type="button" class="btn blue"><i class="fa fa-plus-square"></i> Seleccionar </button>';
                                }
                            },
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": true,
                        "ordering": true,
                        "info": false,
                        "async": true,
                        "bDestroy": true,
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
                },
                error: function (Respuesta) {
                    alertify.alert('Error al cargar datos');
                }
            });
        }

        function CargarAlumnoDeEmpresa(grupoId) {
            $.ajax({
                url: 'WS/Alumno.asmx/ConsultarAlumnosDeEmpresa',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: "{grupoId:'" + grupoId + "'}",
                dataType: 'json',
                success: function (Respuesta) {
                    AlumnosEmpresa = $('#tblAlumnosN').dataTable({
                        "aaData": Respuesta.d,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId", "Nombre": "AlumnoId", visible: false },
                            { "mDataProp": "Nombre" },
                             { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                            { "mDataProp": "FechaRegistro" },
                            //{ "mDataProp": "FechaSeguimiento" },
                            { "mDataProp": "Usuario.Nombre" },
                            {
                                "mDataProp": "AlumnoId", "Nombre": "AlumnoId",
                                "mRender": function (data) {
                                    return '<button type="button" class="btn blue" disabled="true"><i class="fa fa-plus-square"></i> Seleccionar </button>';
                                }
                            },
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": true,
                        "ordering": true,
                        "info": false,
                        "async": true,
                        "bDestroy": true,
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
                },
                error: function (Respuesta) {
                    alertify.alert('Error al cargar datos');
                }
            });
        }

        function LimpiarGrupo() {
            $('#txtNombreGrupo').val('');
            CargarSede();
            $('#txtNPagos').val('');
            $('#txtcuotaCol').val('');
            $('#txtPagarCol').text('');
            $('#txtPagarIn').text('');
            $('#txtDescuentoBec').val(0);
            $('#txtJustificacionBec').val('');
            $('#txtcuotaIn').val('');
            $('#txtPagarIn').val('');
            $('#txtDescuentoIns').val(0);
            $('#txtJustificacionIns').val('');
            $('#chkDescuento').attr("checked", false).parent().removeClass("checked");
            success1.hide();
        }

        $('#btnGuardarGrupo').click(function () {
            if (GrupoFrm.valid() == false) { return false; }
            var Siempre = $('#chkDescuento').is(':checked') ? 'true' : 'false';
            var query = "{EmpresaId:'" + EmpresaI;
            query += "',Nombre:'" + $('#txtNombreGrupo').val() + "',Sede:'" + $('#slcPlantel').val() + "',Direccion:'" + $('#txtDireccion').val() + "',TipoOferta:'" + $('#slcTipo').val() +
                "',Oferta:'" + $('#slcOferta').val() + "',FechaInicio:'" + $('#txtFechaInicio').val() + "',CuotaColegiatura:'" + $('#txtcuotaCol').val() + "',DescuentoColegiatura:'"
                + $('#txtDescuentoBec').val() + "',CuotaInscripcion:'" + InscripcionID + "',DescuentoInscripcion:'" + $('#txtDescuentoIns').val() + "',Periodo:'"
                + $('#slcPeriodo').val().substring(0, 1) + $('#slcPeriodo option:selected').html();
            query += "',NoPagos:'" + $('#txtNPagos').val() + "',AplicaDescuento:'" + Siempre + "',JustificacionIn:'" + $('#txtJustificacionIns').val()
                + "',JustificacionBec:'" + $('#txtJustificacionBec').val() + "'}";
            $.ajax({
                type: "POST",
                url: "WS/Empresa.asmx/GuardarGrupo",
                data: query, // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d >= 1) {
                        GuardarDocumentos($('#txtJustificacionBec').val(), $('#txtJustificacionIns').val(), InscripcionID, data.d);
                        $('#txtNombreGrupo').val('');
                        $('#txtDireccion').val('');
                        $('#slcPlantel').val('-1');
                        $('#slcTipo').val('-1');
                        $('#slcOferta').val('-1');
                        $('#txtcuotaCol').val('');
                        alertify.alert("Grupo Guardado");
                        $('#ListaGrupos').show();
                        $('#Convenio').hide();
                        LimpiarGrupo();
                        CargarGrupo(EmpresaI);
                    }
                    else { alertify.alert("Error"); }
                }
            });
        });

        function GuardarDocumentos(JustificacionBe, JustificacionIn, PagoConceptoIn, GrupoId) {
            var data = new FormData();
            var flIns = $('#BecArchivo'); // FileList object
            flIns = flIns[0].files[0];
            data.append("DocBeca", flIns);
            data.append("JustificacionBe", JustificacionBe);
            data.append('GrupoId', GrupoId);

            flIns = $('#InsArchivo');
            flIns = flIns[0].files[0];
            data.append("DocInscipcion", flIns);
            data.append("JustificacionIn", JustificacionIn);
            data.append("PagoConcepto", PagoConceptoIn);

            var request = new XMLHttpRequest();
            request.open("POST", 'WS/Empresa.asmx/GuardarDocumentos', true);
            request.send(data);
        }

        $('#chkFiscales').click(function () {
            if ($(this).is(':checked')) {
                $('#slcEstadoPaisFiscal').prop('disabled', 'disabled');
                $('#slcEstadoPaisFiscal').val($('#slcEstadoPais').val());
                $('#slcEstadoPaisFiscal').change();
                $("#slcDelegacionFiscal").prop('disabled', 'disabled');
                $('#txtCalleFiscal').prop('readonly', true);
                $('#txtCalleFiscal').val($('#txtCalle').val());
                $('#txtCPFiscal').prop('readonly', true);
                $('#txtCPFiscal').val($('#txtCP').val());
                $('#NoExteriorFiscal').prop('readonly', true);
                $('#NoExteriorFiscal').val($('#NoExterior').val());
                $('#NoInteriorFiscal').prop('readonly', true);
                $('#NoInteriorFiscal').val($('#NoInterior').val());
                $('#slcPaisUniFiscal').prop('disabled', 'disabled');
                $('#slcPaisUniFiscal').val($('#slcPaisUni').val());
                $('#txtObservacionFiscal').prop('readonly', true);
                $('#txtObservacionFiscal').val($('#txtObservacion').val());
                $('#txtColoniaFiscal').prop('readonly', true);
                $('#txtColoniaFiscal').val($('#txtColonia').val());
            } else {
                $('#txtCalleFiscal').removeAttr('readonly');
                $('#txtCPFiscal').removeAttr('readonly');
                $('#NoExteriorFiscal').removeAttr('readonly');
                $('#NoInteriorFiscal').removeAttr('readonly');
                $('#slcPaisUniFiscal').removeAttr('disabled');
                $('#slcEstadoPaisFiscal').removeAttr('disabled');
                $('#slcDelegacionFiscal').removeAttr('disabled');
                $('#txtObservacionFiscal').removeAttr('readonly');
                $('#txtColoniaFiscal').removeAttr('readonly');

                $('#txtCalleFiscal').val('');
                $('#txtCPFiscal').val('');
                $('#NoExteriorFiscal').val('');
                $('#NoInteriorFiscal').val('');
                $('#slcPaisUniFiscal').val(146);
                $('#slcEstadoPaisFiscal').val(-1);
                $('#slcDelegacionFiscal').val(-1);
                $('#txtObservacionFiscal').val('');
                $('#txtColoniaFiscal').val('');
            }
        });

        $("#slcTipo").change(function () {
            $("#slcOferta").empty();
            var plantel = $('#slcPlantel').val();
            if (plantel == -1) { return false; }
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcOferta").append(optionP);
            var tipo = $("#slcTipo");
            tipo = tipo[0].value;

            if (tipo != -1) {
                $('#lblOFerta').html(tipo == 1 ? 'Licenciatura' : tipo == 2 ? 'Especialidad' : tipo == 3 ? 'Mestría' : tipo == 4 ? 'Idioma' : tipo == 5 ? 'Doctorado' : ' ');
                $.ajax({
                    type: "POST",
                    url: "WS/General.asmx/ConsultarOfertaEducativa",
                    data: "{tipoOferta:'" + tipo + "',Plantel:'" + $("#slcPlantel").val() + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                    //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    dataType: "json",
                    success: function (data) {
                        var datos = data.d;
                        if (datos.length > 0) {
                            $(datos).each(function () {
                                var option = $(document.createElement('option'));

                                option.text(this.Descripcion);
                                option.val(this.OfertaEducativaId);

                                $("#slcOferta").append(option);
                            });
                        } else {
                            $("#slcOferta").append(optionP);
                        }

                    }
                });
                //PlanPago();
            } else {
                $("#slcOferta").append(optionP);
                $("#slcSistemaPago").empty();
            }
        });

        $('#slcPlantel').change(function () {
            CargarDireccion();
            CargarTipo();
        });

        $("#slcOferta").change(function () {
            if ($("#slcTipo").val() == 4) {
                CargarDescuentoIdiomas($("#slcOferta").val());
            } else {
                CargarDescuento($("#slcOferta").val());
            }
        });

        $('#btnCerrar').mousedown(function () {
            $('#txtDescripcion').val('');
            $('#txtArea').val('');
        });

        $('#btnSiguiente').click(function () {
            if (form.valid() == false) { return false; }
            error.hide();
            $('#divDatosEmpresa').hide();
            $('#divFiscales').show();
            $('#btnAtras').removeAttr('style');
            $('#btnSiguiente').attr('style', 'display: none');
            $('#btnGuardar').removeAttr('style');
        });

        $('#btnAtras').click(function () {
            $('#divFiscales').hide();
            $('#btnGuardar').attr('style', 'display: none');
            $('#btnAtras').attr('style', 'display: none');
            $('#btnSiguiente').removeAttr('style');
            $('#divDatosEmpresa').show();
        });

        $('#btnGuardar').click(function () {
            if (form.valid() == false) { return false; }
            var query = "{";
            query += "Nombre:'" + $('#txtDescripcion').val() + "',RFC:'" + $('#txtRFC').val() + "',Email:'" + $('#txtmail').val() + "',Calle:'" + $('#txtCalle').val() + "',CP:'" + $('#txtCP').val() + "',NoExterior:'" + $('#NoExterior').val() +
                "',NoInterior:'" + $('#NoInterior').val() + "',Pais:'" + $('#slcPaisUni').val() + "',Estado:'" + $('#slcEstadoPais').val() + "',Delegacion:'" + $('#slcDelegacion').val() + "',Observacion:'" + $('#txtObservacion').val() +
                "',Colonia:'" + $('#txtColonia').val() + "',FechaV:'" + $('#txtFechaV').val() + "',NombreC:'" + $('#txtNombre').val() + "',Paterno:'" + $('#txtPaterno').val() + "',Materno:'" + $('#txtMaterno').val() + "',EmailC:'" + $('#txtEmail').val() + "',Telefono:'" + $('#txtTelefono').val() +
                "',Celular:'" + $('#txtCelular').val() + "'";
            if ($('#chkFiscales').prop('checked') == false) {
                query += ",CalleF:'" + $('#txtCalleFiscal').val() + "',CPF:'" + $('#txtCPFiscal').val() + "',NoExteriorF:'" + $('#NoExteriorFiscal').val() + "',NoInteriorF:'" + $('#NoInteriorFiscal').val() + "',PaisF:'" + $('#slcPaisUniFiscal').val() +
                 "',EstadoF:'" + $('#slcEstadoPaisFiscal').val() + "',DelegacionF:'" + $('#slcDelegacionFiscal').val() + "',ObservacionF:'" + $('#txtObservacionFiscal').val() + "',ColoniaF:'" + $('#txtColoniaFiscal').val() + "',Igual:'false'}";;
            } else {
                query += ",CalleF:'" + "" + "',CPF:'" + "" + "',NoExteriorF:'" + "" + "',NoInteriorF:'" + "" + "',PaisF:'" + "" +
                 "',EstadoF:'" + "" + "',DelegacionF:'" + "" + "',ObservacionF:'" + "" + "',ColoniaF:'" + "" + "',Igual:'true'}";
            }
            $.ajax({
                type: "POST",
                url: "WS/Empresa.asmx/GuardarEmpresa",
                data: query, // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    if (data.d == "True") {
                        alertify.alert("Empresa Guardada", function () {
                            $('#btnCerrar').click();
                            Cargar();
                        });
                    }
                    else {
                        alertify.alert("Error, no fue posible guardar la empresa", function () {
                            return false;
                        });
                    }
                }
            });
        });

        $("#slcEstadoPais").change(function () {
            $("#slcDelegacion").empty();
            var Entidad = $("#slcEstadoPais");
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcDelegacion").append(optionP);

            Entidad = Entidad[0].value;
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarMunicipios",
                data: "{EntidadFederativaId:'" + Entidad + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.EntidadFederativaId);

                        $("#slcDelegacion").append(option);
                    });
                }
            });
        });

        $('#slcEstadoPaisFiscal').change(function () {
            $("#slcDelegacionFiscal").empty();
            var Entidad = $("#slcEstadoPaisFiscal");
            var optionP = $(document.createElement('option'));
            optionP.text('--Seleccionar--');
            optionP.val('-1');
            $("#slcDelegacionFiscal").append(optionP);

            Entidad = Entidad[0].value;
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/ConsultarMunicipios",
                data: "{EntidadFederativaId:'" + Entidad + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                success: function (data) {
                    var datos = data.d;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.EntidadFederativaId);

                        $("#slcDelegacionFiscal").append(option);
                    });
                    if ($('#chkFiscales').prop('checked') == false) {
                        $("#slcDelegacionFiscal").val('-1');
                    } else {
                        $("#slcDelegacionFiscal").val($('#slcDelegacion').val());
                    }
                }
            });

        });

        $('#txtDescuentoBec').keyup(function () {
            var monto = CalcularDescuento($('#txtcuotaCol').val().replace('$', ''), $('#txtDescuentoBec').val());
            $('#txtPagarCol').text('$' + String(monto));
            RecalculaTabla($('#txtDescuentoBec').val());
        });

        $('#txtDescuentoBec').knob({
            change: function (val) {
                var monto = CalcularDescuento($('#txtcuotaCol').val().replace('$', ''), val);
                $('#txtPagarCol').text('$' + String(monto));
                RecalculaTabla(val);
            }
        });

        $('#txtDescuentoIns').keyup(function () {
            $('#txtPagarIn').text('$' +
                String(CalcularDescuento($('#txtcuotaIn').val().replace('$', ''), $('#txtDescuentoIns').val())));
        });

        $('#txtDescuentoIns').knob({
            change: function (val) {
                $('#txtPagarIn').text('$' +
               String(CalcularDescuento($('#txtcuotaIn').val().replace('$', ''), val)));
            }
        });

        $('#tblAlumnos').on('click', 'button', function () {

            var row = this.parentNode.parentNode;
            row.className = 'active';
            Seleccionados += Alumnos.fnGetData(row, 0) + ",";
            row.closest('tr').remove();
            $('#tblAlumnosN').append(row);

        });

        $('#tblAlumnosN').on('click', 'button', function () {
            var row = this.parentNode.parentNode;
            row.className = 'add';
            Seleccionados = Seleccionados.replace(Alumnos.fnGetData(row, 0) + ",", '');
            row.closest('tr').remove();
            $('#tblAlumnos').append(row);

        });

        $('#tblEmpresa').on('click', 'a', function () {
            $('#NombreEmpresa').text(this.innerHTML);
            EmpresaI = this.parentNode.parentNode;
            EmpresaI = MItable.fnGetData(EmpresaI, 0);
            CargarGrupo(EmpresaI);
            $('#EmpresaLista').hide();
            $('#EmpresaC').show();
        });

        $('#btnCerrarGrupo').click(function () {
            LimpiarGrupo();
            $('#PopGrupo').modal('show');
        });

        $('#btnGrupo').click(function () {
            LimpiarGrupo();
            $('#hGrupos').text("Alta de Grupo");
            $('#PopGrupo').modal('show');
        });

        $('#btnAtrasE').click(function () {
            $('#EmpresaLista').show();
            $('#EmpresaC').hide();
        });

        $('#ListaGrupos').on('click', 'a', function () {

            if (this.innerHTML == "Nuevo Grupo") { return false; }
            if (this.name == "btnAlumos") {
                $('#NombreGrupo').text(this.innerHTML);
                var rFila = this.parentNode.parentNode;
                rFila = MiGrupo.fnGetData(rFila, 0);
                CargarAlumnoParaEmpresa(rFila);
                CargarAlumnoDeEmpresa(rFila);
                $('#EmpresaC').hide();
                $('#DivGrupo').show();
                Fila = rFila;
            }

            if (this.name == "btnModificar") {
                var rFila = this.parentNode.parentNode;
                rFila = MiGrupo.fnGetData(rFila, 0);
                LimpiarGrupo();

                $('#hGrupos').text("Modificación de grupo ");
                $('#PopGrupo').modal('show');
                Fila = rFila;
            }

        });

        $('#btnAtrasG').click(function () {
            $('#EmpresaC').show();
            $('#DivGrupo').hide();
        });

        $('#btnGrupoAl').click(function () {
            if (Seleccionados.length > 1) {
                alertify.confirm("<p>¿Esta seguro que desea guardar los cambios?<br><br><hr>", function (e) {
                    if (e) {

                        $.ajax({
                            type: "POST",
                            url: "WS/Empresa.asmx/GenerarPagos",
                            data: "{Alumnos:'" + Seleccionados + "',Grupo:'" + Fila + "'}",
                            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                            success: function (data) {
                                if (data.d == $('#txtRFC').val()) {
                                    alertify.alert("Alumnos Agregados", function () {
                                        CargarAlumnoParaEmpresa(Fila);
                                        CargarAlumnoDeEmpresa(Fila);
                                        Seleccionados = "";
                                    });
                                }
                                else {
                                    alertify.alert("Error, no fue posible Agregar a los Alumnos", function () {
                                        return false;
                                    });
                                }
                            }
                        });
                    }
                });
            } else {
                return false;
            }
        });

    };
    var tblAlumnosCompletos;
    CargarTabla();
    function CargarTabla(){
        $.ajax({
            type: "POST",
            url: "WS/Empresa.asmx/ListarAlumnos",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    tblAlumnosCompletos= $('#tblAlumnosCom').dataTable({
                        "aaData": data.d,
                        "aoColumns": [
                            { "mDataProp": "AlumnoId" },
                            { "mDataProp": "Nombre" },
                            { "mDataProp": "OfertaEducativaS" },
                            {
                                "mDataProp": "GrupoAlumno",
                                "mRender": function (data, f, d) {
                                    var link;
                                    if (d.GrupoAlumno == null) { link = "<a href='' class='btn blue' onclick='return false;'>" + "Agregar Grupo" + " </a> "; }
                                    else { link = "<a href='' class='btn red' onclick='return false;'>" + "Ver Grupo" + " </a> "; }
                                    return link;
                                }
                            },
                            {
                                 "mDataProp": "GrupoAlumno",
                                 "mRender": function (data, f, d) {
                                     var link;
                                     if (d.AlumnoCuota == null) { link = "<a href='' class='btn blue' onclick='return false;'>" + "Agregar Configuracion" + " </a> "; }
                                     else { link = "<a href='' class='btn red' onclick='return false;'>" + "Ver Configuracion" + " </a> "; }
                                     return link;
                                 }
                             },
                        ],
                        "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                        "searching": true,
                        "ordering": true,
                        "info": false,
                        "async": true,
                        "bDestroy": true,
                        "language": {
                            "lengthMenu": "_MENU_  Registros",
                            "paginate": {
                                "previous": "<",
                                "next": ">"
                            },
                            "search": "Buscar Empresa "
                        },
                        "order": [[2, "desc"]]
                    });
                }
                else { alertify.alert("Error"); }
            }
        });
    }
});