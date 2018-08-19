$(function () {
    var tblComunicados,
        tblFallidos;

    var Comunicado = {
        ComunicadoId:0,
        init() {
            this.TrerComunicados();
            $('#btnEnviar').on('click', this.validar);
            $('#btnReenviar').on('click', this.ReEnviar);
            $('#fileComunicado').on('change', this.FileChange);
            $('#tblComunicados').on('click', 'a', this.PintarFallidos);
        },
        validar(e) {
            if ($('#frmComunicado')[0].checkValidity()) {
                Comunicado.EnviarComunicado();
            } else {
                alertify.alert("Todos los campos son obligatorios.");
            }
            e.preventDefault();
        },
        EnviarComunicado() {
            var data = new FormData();
            data.append("Documento", $('#fileComunicado')[0].files[0]);
            data.append("UsuarioId",  localStorage.getItem('userAdmin'));
            data.append("Asunto", $('#txtAsunto').val());

            alertify.alert("Se iniciara el proceso de envio, este proceso puede tardar entre 1 a 3 horas,  le mandaremos una copia al terminar.", function () {

                $.ajax({
                    url: "Api/Biblioteca/SendComunicado",
                    type: "POST",
                    data: data,
                    contentType: false,
                    processData: false
                })
                    .done(function (data) {
                        console.log("Termine");
                    })
                    .fail(function (data) {
                        console.log("Falle");
                    });
            });
        },
        FileChange() {
            if (this.files.length > 0) {
                $('#nameFile').text(this.files[0].name);
                $('#statusfile').text("Cambiar");
            } else {
                $('#nameFile').text("No hay archivo seleccionado");
                $('#statusfile').text("Seleccionar Archivo");
            }
        },
        TrerComunicados() {
            console.log("hola");
            //IndexFn.Block(true);
            Comunicado.Api("Biblioteca/Comunicados", "GET", "")
                .done(function (data) {
                    if (data.length > 0) {
                        Comunicado.PintarTabla(data);
                    } else {
                        IndexFn.Block(false);
                    }
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                });
        },
        Api(Url, type, data) {
            var dfd = $.Deferred();

            var Ajax = $.ajax({
                url: "Api/" + Url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: type,
                data: data,
            });

            Ajax.done(function (data) {
                dfd.resolve(data);
            }).fail(function (data) {
                dfd.reject(data);
            });

            return dfd.promise();
        },
        PintarTabla(Lista) {
            tblComunicados = $('#tblComunicados').dataTable({
                "aaData": Lista,
                "aoColumns": [
                    { "mDataProp": "ComunicadoId" },
                    { "mDataProp": "Periodo" },
                    { "mDataProp": "Fecha" },
                    { "mDataProp": "Asunto" },
                    { "mDataProp": "Usuario" },
                    {
                        "mDataProp": function(data) {
                            return "<a class='btn green'>Ver</a>";
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
                    "search": "Buscar Comunicado ",
                },
                "order": [[0, "desc"]]
            });
            var fil = $('#tblComunicados_filter label input');
            fil.removeClass('input-small').addClass('input-large');
            IndexFn.Block(false);
        },
        PintarFallidos() {
            var rowadd = tblComunicados.fnGetData($(this).closest('tr'));

            Comunicado.ComunicadoId = rowadd.ComunicadoId;

            tblFallidos = $('#tblFallidos').dataTable({
                "aaData": rowadd.Fallidos,
                "aoColumns": [
                    { "mDataProp": "UsuarioId" },
                    { "mDataProp": "Nombre" },
                    { "mDataProp": "TipoUsuario" },
                    { "mDataProp": "Mensaje" },
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
                "order": [[0, "desc"]]
            });

            var fil = $('#tblFallidos_filter label input');
            fil.removeClass('input-small').addClass('input-large');

            $('#modalFallidos').modal('show');
        },
        ReEnviar() {
            Comunicado.Api("Biblioteca/Renviar/" + Comunicado.ComunicadoId, "POST", "")
                .done(function (data) {
                    console.log(data);
                })
                .fail(function (data) {
                    console.log(data);
                });
        }
    };

    Comunicado.init();
});