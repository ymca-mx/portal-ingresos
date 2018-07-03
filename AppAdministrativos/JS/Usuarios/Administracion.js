$(function () {
    var UsuariosFn = {
        init() {
            $('#tblUsuarios').on('click', 'a', this.ShowPop);
            this.GetUsuarios();
            $('#btnGuardar').on('click', this.SaveChanges);
        },
        GetUsuarios() {
            $('#Load').modal('show');
            IndexFn.Api("Administracion/Usuarios/" + $.cookie('userAdmin'),"GET","")
                .done(function (data) {
                    UsuariosFn.PintarTabla(data.Usuarios);
                })
                .fail(function (data) {
                    console.log("Fallo");
                    console.log(data);
                });
        },
        PintarTabla(lstUsuarios) {
            UsuariosFn.TblUsuarios =
                $('#tblUsuarios').dataTable({
                    "aaData": lstUsuarios,
                    "aoColumns": [
                        {
                            "mDataProp": "UsuarioId",
                            Stextalign: 'center'
                        },
                        {
                            "mDataProp": function (data) {
                                return (data.Nombre + " " + data.Paterno + " " + data.Materno);
                            },
                            sWidth: '350px'
                        },
                        { "mDataProp": "Descripcion" },
                        { "mDataProp": "Password" },
                        { "mDataProp": "Estatus" },
                        {
                            "mDataProp": function (data) {
                                var link = "";
                                link = "<a href=''onclick='return false;' name='edit' class='btn btn-success'> Editar </a> ";
                                
                                return link;
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
                        "search": "Buscar Usuario ",
                    },
                    "order": [[2, "desc"]]
                });
            var fil = $('#tblUsuarios_filter label input');
            fil.removeClass('input-small').addClass('input-large');
            $('#Load').modal('hide');
        },
        TblUsuarios: $('#tblUsuarios').dataTable(),
        ShowPop() {
            var row = this.parentNode.parentNode;
            var Data = UsuariosFn.TblUsuarios.fnGetData($(this).closest('tr'));

            $('#txtUsuarioId').val(Data.UsuarioId);
            $('#txtNombre').val(Data.Nombre);
            $('#txtPaterno').val(Data.Paterno);
            $('#txtMaterno').val(Data.Materno);
            $('#txtPassword').val(Data.Password);
            $('#txtPasswordN').val(Data.Password);
            $('#chkEstatus').prop("checked", (Data.EstatusId === 1 ? true : false));

            $('#PopDatos').modal('show');
        },
        SaveChanges() {
            if ($('#txtPasswordN').val().length > 3) {
                var objDocente = {
                    UsuarioId: $('#txtUsuarioId').val(),
                    Nombre: $('#txtNombre').val(),
                    Paterno: $('#txtPaterno').val(),
                    Materno: $('#txtMaterno').val(),
                    Password: $('#txtPasswordN').val(),
                    EstatusId: ($('#chkEstatus')[0].checked ? 1 : 2)
                };

                $('#Load').modal('show');
                IndexFn.Api("Administracion/Usuario", "POST", JSON.stringify(objDocente))
                    .done(function (data) {
                        $('#Load').modal('hide');
                        $('#PopDatos').modal('hide');
                        alertify.alert("Universidad YMCA", 'Se guardo correctamente', function () {
                            UsuariosFn.GetUsuarios();
                        });
                    })
                    .fail(function (data) {
                        $('#Load').modal('hide');
                        alertify.alert("Universidad YMCA", 'Error al guardar los datos.', function () {
                            console.log(data);
                        });
                    });
            } else {
                alertify.alert("Universidad YMCA", 'Favor de ingresar una contraseña mayor a 3 caracteres.');
            }
        }
    };

    UsuariosFn.init();
});