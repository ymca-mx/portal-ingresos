var IndexFn = {
    LifeSesion: function () {
        $('#txtTime').val("00:01:00");
        $('#txtTime').data("time", 60);
        $('#modalTime').modal('show');
        setInterval(function () {
            var time = $('#txtTime').data("time");
            time -= 1;
            if (time > 0) {
                $('#txtTime').data("time", time);
                time = (time < 10 ? ("0" + time) : ("" + time));
                $('#txtTime').val("00:00:" + time);
            } else {
                localStorage.clear();
                $(location).attr('href', 'login.html');
            }
        }, 1000);
    },
    ContinuarSesion: function () {
        IndexFn.Config();
        $('#modalTime').modal('hide');
    },
    CerrarSesion: function () {
        localStorage.clear();
        $(location).attr('href', 'login.html');
    },
    SetTime: function () {
        setTimeout(this.LifeSesion, 300000);
    }
};
$(function () {
    var app_router;
    var bandera = 0;


        IndexFn.init = function () {
            $('#btnSesionContinuar').on('click', this.ContinuarSesion);
            $('#btnSesionCerrar').on('click', this.CerrarSesion);
            $('#popDatos').on('hidden.bs.modal', this.PopDatosAlumnoClick);
            this.TraerAlumno();
        };

        IndexFn.TraerAlumno = function () {
            $.ajax({
                url: 'api/Alumno/Datos/' + localStorage.getItem("user"),
                type: 'Get',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (Datos) {
                    if (Datos == null) {
                        alertify.alert("Universidad YMCA", 'Error en la carga de datos generales', function () {
                            localStorage.clear();
                            $(location).attr('href', 'login.html');
                        });
                    }
                    else {

                        $('#imgUsuario').attr('src', 'data:image/' + Datos.extensionImagen + ';base64,' + Datos.imagenBase64);
                        $('#lblUsuario span').text(Datos.nombre);

                        if (Datos.extensionImagen == ".png")
                            $('#imgUsuario').attr('src', 'Style/engato/index/imagenes/Guest.png');                        
                        IndexFn.ConstruirMenu();
                    }
                },
                error: function (Resultado) {
                    alertify.alert("Universidad YMCA", 'Se presento un error en la validación de las credenciales', function () {
                        localStorage.clear();
                        $(location).attr('href', 'login.html');
                    });
                }
            });
        };
        IndexFn.ClearAlert = function () {
            try { alertify.confirm().destroy(); }
            catch (err) { }
            try { alertify.alert().destroy(); }
            catch (arr) { }
            try { alertify.prompt().destroy(); }
            catch (arr3) { }
        };
        IndexFn.VerificarDatos = function () {

            $.ajax({
                type: "get",
                url: "Api/Alumno/VerificaAlumnoDatos/" + localStorage.getItem("user"),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data) {
                        $('#popDatos').load('Views/Alumno/AlumnoActualizaDatos.html', function () {
                            $('#popDatos').modal('show');
                        });
                    }
                    IndexFn.SetTime;
                }
            });
        };
        IndexFn.VerificarEncuesta = function () {
            $.ajax({
                type: "get",
                url: "Api/Alumno/VerificaAlumnoEncuesta/" + localStorage.getItem("user"),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data) {
                        $('#divDinamico').load('Views/Alumno/EncuestaPortal.html');
                    }
                }
            });
        };
        IndexFn.RevisaAnticipado = function () {
            $.ajax({
                url: 'Api/Alumno/CalcularAnticipado/' + localStorage.getItem("user"),
                type: 'get',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (Resultado) {

                    if (Resultado.length > 1) {
                        var text = $('#lblFecha');
                        text[0].innerText = Resultado[1];
                        $('#btnPop').click();
                    }
                }
            });
        };
        IndexFn.contenidoClick = function () {

            bandera = 0;
            var url = $(this).attr('href');
            if (url === undefined) { url = '#'; }

            var menuid = (url.search('perfil') > 0 ? 0 : 1);
            app_router.navigate(url, { trigger: true });

            if (bandera === 0) {
                var arrurl = $(this).attr('href');
                if (arrurl !== undefined) {
                    arrurl = arrurl.split('/');
                    var id = arrurl[(arrurl.length - 1)];
                    id = parseInt(id);

                    $(IndexFn.Menu).each(function () {
                        if (this.MenuId === menuid) {
                            $(this.SubMenus).each(function () {
                                if (id === this.SubMenuId) {
                                    url = this.Direccion;
                                }
                            });
                        }
                    });
                }
                $('#divDinamico').empty();

                if (url !== '#') {
                    IndexFn.ClearAlert();
                    $('#divDinamico').load(url);
                    $('#divDinamico').append(IndexFn.alertify3());
                }
            }
            return false;
        };
        IndexFn.PopDatosAlumnoClick = function () {
            location.reload();
        };
        IndexFn.ConstruirMenu = function () {
            console.log("voy a pintar el menu");
            var ul = "";
            var ulPer = "";
            $(IndexFn.Menu).each(function () {
                if (this.TipoMenuId === 1) {
                    ul += '<li class="menu-dropdown classic-menu-dropdown ">' +
                        '<a data-hover="megamenu-dropdown" data-close-others="true" data-toggle="dropdown" > ' +
                        this.Descripcion + '<i class="' + this.icono + '"></i>' +
                        '</a>';
                    if (this.SubMenus.length > 0) {
                        ul += '<ul class="dropdown-menu pull-left">';
                        $(this.SubMenus).each(function () {
                            ul += '<li>' +
                                '<a href="#Views/' + this.SubMenuId + '" class="contenido">' +
                                '<i class="' + this.Icono + '"></i>  ' + this.Descripcion +
                                '</a></li>';
                        });
                        ul += "</ul>";
                    }
                } else if (this.TipoMenuId === 2) {
                    $(this.SubMenus).each(function () {

                        ulPer += (this.Divider != undefined ? this.Divider : "") + '<li>' +
                            '<a class="contenido" ' +
                            (this.Direccion != undefined ? 'href="#Perfil/' + this.SubMenuId + '"' : '') + '> ' +
                            '<i class="' + this.Icono + '"></i>' + this.Descripcion +
                            '</a></li>';
                    });
                }
            });
            $('#MenuItems').append(ul);
            $('#UlPerfil').append(ulPer);

            app_router = new AppRouter;
            Backbone.history.start();

            $(".contenido").click(IndexFn.contenidoClick);

            IndexFn.VerificarDatos();
        };
        IndexFn.Menu =
            [
                {
                    TipoMenuId: 1,
                    MenuId: 1,
                    Descripcion: "Pagos",
                    icono: "fa fa-angle-down",
                    SubMenus: [
                        {
                            SubMenuId: 1,
                            Descripcion: 'Consultar Referencias',
                            Direccion: 'Views/Pago/Pagos2.html',
                            Icono: 'fa fa-bank'
                        },
                        {
                            SubMenuId: 2,
                            Descripcion: 'Estado de Cuenta',
                            Direccion: 'Views/Pago/EstadodeCuenta2.html',
                            Icono: 'fa fa-bar-chart-o'
                        },
                        {
                            SubMenuId: 3,
                            Descripcion: 'Generar Referencias - Trámites',
                            Direccion: 'Views/Pago/GenerarReferencias.html',
                            Icono: 'fa fa-money'
                        },
                        {
                            SubMenuId: 4,
                            Descripcion: 'Referencias de Reinscripción',
                            Direccion: 'Views/Pago/ReInscripcion.html',
                            Icono: 'fa fa-book'
                        }
                    ],

                },
                {
                    MenuId: 0,
                    TipoMenuId: 2,
                    SubMenus: [
                        {
                            SubMenuId: 1,
                            Descripcion: 'Mi Perfil',
                            Direccion: 'Views/Alumno/AlumnoActualizaDatos.html',
                            Icono: 'icon-user'
                        },
                        {
                            SubMenuId: 2,
                            Descripcion: 'Reglamento Escolar',
                            Direccion: 'Views/Alumno/Reglamento.html',
                            Icono: 'glyphicon glyphicon-book'
                        },
                        {
                            SubMenuId: 3,
                            Descripcion: 'Salir',
                            Direccion: 'login.html',
                            Icono: 'icon-key',
                            Divider: '<li class="divider"></div>'
                        }
                    ]
                }
            ];


        AppRouter = Backbone.Router.extend({
            routes: {
                "Views/:SubMenuId": "LoadPage",
                "Perfil/:SubMenuId": "LoadPerfil",
                "*actions": "defaultRoute",
            },
            LoadPage: function (SubMenuId) {
                if (localStorage.getItem("user") === null) {
                    Backbone.history.stop();
                    localStorage.clear();
                    $(location).attr('href', 'login.html');
                    return false;
                }
                $('#divDinamico').empty();

                var direccion = "";
                var subInt = parseInt(SubMenuId);

                //Sacamos la url Guardada
                $(IndexFn.Menu).each(function () {
                    if (this.MenuId === 1) {
                        $(this.SubMenus).each(function () {
                            if (subInt === this.SubMenuId) {
                                direccion = this.Direccion;
                            }
                        });
                    }
                });
                if (direccion.length > 0) {
                    IndexFn.ClearAlert();
                    $('#divDinamico').load(direccion);
                }
                bandera = 1;
            },
            LoadPerfil: function (SubMenuId) {
                if (localStorage.getItem("user") === null) {
                    Backbone.history.stop();
                    localStorage.clear();
                    $(location).attr('href', 'login.html');
                    return false;
                }
                $('#divDinamico').empty();

                var direccion = "";
                var subInt = parseInt(SubMenuId);

                //Sacamos la url Guardada
                $(IndexFn.Menu).each(function () {
                    if (this.MenuId === 0) {
                        $(this.SubMenus).each(function () {
                            if (subInt === this.SubMenuId) {
                                direccion = this.Direccion;
                            }
                        });
                    }
                });
                if (direccion.length > 0) {
                    if (direccion.search("login.html") > 0) {
                        Backbone.history.stop();
                        localStorage.clear();
                        $(location).attr('href', 'login.html');
                        return false;
                    } else {
                        IndexFn.ClearAlert();
                        $('#divDinamico').load(direccion);
                    }
                }
                bandera = 1;
            },
            defaultRoute: function (actions) {
                if (localStorage.getItem("user") === null) {
                    Backbone.history.stop();
                    localStorage.clear();
                    $(location).attr('href', 'login.html');
                    return false;
                }
                $('#divDinamico').empty();
                if (actions === null) {
                    actions = "#";
                    return false;
                }
                if (actions === '#') { return false; }
                if (actions === 'Views/') { return false; }
                var url = actions;

                IndexFn.ClearAlert();
                $('#divDinamico').load(url);

                bandera = 1;
            }
        });


    IndexFn.init();
});