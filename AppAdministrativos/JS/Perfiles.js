$(document).ready(function () {
    ConsultarPerfiles();
    var tblPerfiles;
    var frmPerfiles=$('#frmPerfil');
    var error=$('.alert-danger', frmPerfiles);
    var success = $('.alert-success', frmPerfiles);
    frmPerfiles.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block help-block-error', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        rules: {
            txtNombre: {
                required: true,
                minlength: 4,
                maxlength: 50,
            },
            txtFechaAlta: {
                required: true,
                digits: false,
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
    function ConsultarPerfiles() {
        $.ajax({
            type: "POST",
            url: "WS/OperacionesPerfiles.asmx/ConsultarTodos",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                tblPerfiles = $('#tblPerfiles').dataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        {
                            "mDataProp": "PerfilId",
                            "mRender": function (data) {
                                return "<a href='#ModificarPerfil' data-toggle='modal' onclick='return false;'>" + data + "</a>";
                            }
                        },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "FechaAlta" },
                        {
                            "mDataProp": "PerfilId",
                            "mRender": function (data) {
                                return '<button type="button" class="btn red">Eliminar </button>';
                            }
                        }
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
                        "search": "Buscar Perfil "
                    },
                    "order": [[2, "desc"]],
                    "createdRow": function (row, data, dataIndex) {
                        var fecha = new Date(parseInt(data.FechaAlta.slice(6)));
                        fecha = new Date(fecha);
                        var fechas = parseInt(fecha.getDate() + 1) < 10 ? +'0' + String(parseInt(fecha.getDate() + 1)) : String(parseInt(fecha.getDate() + 1));
                        fechas += '/';
                        fechas += parseInt(fecha.getMonth() + 1) < 10 ? +'0' + String(parseInt(fecha.getMonth() + 1)) : String(parseInt(fecha.getMonth() + 1));

                        fechas += '/' + fecha.getFullYear();
                        var cols = row.childNodes[2];
                        cols.innerText = fechas;
                        data.FechaAlta = fechas;
                    }
                });
            }
        });
    }
    $('#tblPerfiles').on('click', 'a', function () {
        var rowadd = tblPerfiles.fnGetData($(this).closest('tr'));
        $('#txtIDPerfil').val(rowadd.PerfilId);
        $('#txtNombreM').val(rowadd.Descripcion);
        $('#txtFechaAltaM').val(rowadd.FechaAlta);
    });
    $('#tblPerfiles').on('click', 'button', function () {
        var rowadd = tblPerfiles.fnGetData($(this).closest('tr'));
        alertify.confirm("<p>¿Esta seguro que desea eliminar el perfil?<br><br><hr>", function (e) {
            if (e) {
                $.ajax({
                    type: "POST",
                    url: "WS/OperacionesPerfiles.asmx/EliminarPerfil",
                    data: "{PerfilId:'" + rowadd.PerfilId + "'}",
                    contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                    success: function (data) {
                        if (data.d == "Guardado") {
                            alertify.alert("Perfil Eliminado", function () {
                                ConsultarPerfiles();
                            });
                        }
                    }
                });
            }
        });
    });
    $('#btnGuardar').on('click', function () {
        if (frmPerfiles.valid() == false) { return false; }else {
            var Parametros = "{";
            Parametros += "Nombre:'" + $('#txtNombre').val() + "',";
            Parametros += "Fecha:'" + $('#txtFechaAlta').val() + "'}";
            $.ajax({
                type: "POST",
                url: "WS/OperacionesPerfiles.asmx/GuardarPerfil",
                data: Parametros,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d > 0) {
                        var tDate = new Date();
                        $('#txtNombre').val('');
                        $('#txtFechaAlta').val(String(tDate.getDay()) + "/" + String(parseInt(tDate.getMonth() + 1) < 10 ? "0" + String(parseInt(tDate.getMonth() + 1)) : String(parseInt(tDate.getMonth() + 1)))
                            + "/" + String(parseInt(tDate.getYear())));
                        success.hide();
                        error.hide();
                        ConsultarPerfiles();
                    }
                }
            });
        }
    });
    $('#btnModificar').on('click', function () {
        var Parametros = "{";
        Parametros += "PerfilId:'" + $('#txtIDPerfil').val() + "',";
        Parametros += "Nombre:'" + $('#txtNombreM').val() + "',";
        Parametros += "Fecha:'" + $('#txtFechaAltaM').val() + "'}";
        $.ajax({
            type: "POST",
            url: "WS/OperacionesPerfiles.asmx/ModificarPerfil",
            data: Parametros,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d == "Guardado") {
                    ConsultarPerfiles();
                    $('#btnCerrar').click();
                } else {
                    alertify.alert("No se guardaron los cambios.");
                }
            }
        });
    });
});

var ComponentsPickers = function () {

    var handleDatePickers = function () {
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
            $('#spnA').text(calcular_edad(Fecha));
            //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
        }
        $('#txtFNacimiento').change(function () {
            var cumple = $('#txtFNacimiento').val();
            var a = calcular_edad(cumple);
            $('#spnA').text(a);
        });


        /* Workaround to restrict daterange past date select: http://stackoverflow.com/questions/11933173/how-to-restrict-the-selectable-date-ranges-in-bootstrap-datepicker */
    }
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


    return {
        //main function to initiate the module
        init: function () {
            handleDatePickers();
        }
    };

}();