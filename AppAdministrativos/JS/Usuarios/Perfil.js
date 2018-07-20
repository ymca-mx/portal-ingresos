$(function () {

    var PerfilFn = {
        init() {
            $('#frmPerfil').on('submit', this.Validar);
            $('#txtFecha').datepicker({
                rtl: false,
                orientation: "left",
                autoclose: true,
                language: 'es',
                format:'dd/mm/yyyy'
            });
            this.GetGenero();
        },
        Validar(e) {
            e.preventDefault();
        },
        Guardar() {

        },
        GetUser() {
            IndexFn.Api("Administracion/Usuario/" + $.cookie('userAdmin'), "GET", "")
                .done(function (data) {
                    $('#txtNombre').val(data.Nombre);
                    $('#txtPaterno').val(data.Paterno);
                    $('#txtMaterno').val(data.Materno);
                    $('#slcSexo').val(data.GeneroId);
                    $('#txtFecha').datepicker('update', data.FechaNacimiento);
                    $('#txtEmail').val(data.Email);
                    $('#txtTelefono').val(data.Telefono);
                    $('#txtTipoUsuario').val(data.UsuarioTipo);
                })
                .fail(function (data) {
                    alertify.alert("Universidad YMCA", "Ocurrio un error al consultar la información.");
                    console.log(data);
                });
            
        },
        GetGenero() {
            $("#slcSexo").empty();
            IndexFn.Api("General/ConsultarGenero", "GET", "")
                .done(function (datos) {
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));

                        option.text(this.Descripcion);
                        option.val(this.GeneroId);

                        $("#slcSexo").append(option);
                    });
                    PerfilFn.GetUser();
                })
                .fail(function (data) {
                    console.log("Fallo la carga de GetGenero");
                });
        }
    };

    PerfilFn.init();
});