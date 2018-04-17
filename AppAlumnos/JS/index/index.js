var IndexFn = {
    TiempoRestante: null,
    Continue: false,
    LifeSesion: function () {
        $('#txtTime').val("00:01:00");
        $('#txtTime').data("time", 60);
        $('#modalTime').modal('show');
        IndexFn.TiempoRestante = setInterval(function () {
            if (IndexFn.Continue) {
                IndexFn.Continue = false;
                clearInterval(IndexFn.TiempoRestante);
            }
            else {
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
            }
        }, 1000);
    },
    ContinuarSesion: function () {
        IndexFn.Continue = true;
        IndexFn.SetTime();
        $('#modalTime').modal('hide');
    },
    CerrarSesion: function () {
        localStorage.clear();
        $(location).attr('href', 'login.html');
    },
    SetTime: function () {
        setTimeout(this.LifeSesion, 500000);
    },
    Message:
         '<div class="modal" tabindex="-1" data-backdrop="static" style="background-color:#0000008f;" id="Load" data-keyboard="false">' +
            '<div class="modal-dialog modal-sm" style="height: 180px;">' +
            '<div class="scene">' +
            '<svg version="1.1"' +
            'id="dc-spinner"' +
            'xmlns="http://www.w3.org/2000/svg"' +
            'x="0px" y="0px"' +
            'wwidth="200" height="200"' +
            'viewBox="0 0 38 38"' +
            'preserveAspectRatio="xMinYMin meet">' +
            '<image xlink:href="Imagenes/uniymca.png" height="20" width="20" x="10" y="12"></image>' +
            '<path fill="#373a42" d="M20,35c-8.271,0-15-6.729-15-15S11.729,5,20,5s15,6.729,15,15S28.271,35,20,35z M20,5.203' +
            'C11.841,5.203,5.203,11.841,5.203,20c0,8.159,6.638,14.797,14.797,14.797S34.797,28.159,34.797,20' +
            'C34.797,11.841,28.159,5.203,20,5.203z">' +
            '</path>' +
            '<path fill="#373a42" d="M20,33.125c-7.237,0-13.125-5.888-13.125-13.125S12.763,6.875,20,6.875S33.125,12.763,33.125,20' +
            'S27.237,33.125,20,33.125z M20,7.078C12.875,7.078,7.078,12.875,7.078,20c0,7.125,5.797,12.922,12.922,12.922' +
            'S32.922,27.125,32.922,20C32.922,12.875,27.125,7.078,20,7.078z">' +
            '</path>' +
            '<path fill="#ff0129" stroke="#ff0129" stroke-width="0.70" stroke-miterlimit="10" d="M5.203,20' +
            'c0-8.159,6.638-14.797,14.797-14.797V5C11.729,5,5,11.729,5,20s6.729,15,15,15v-0.203C11.841,34.797,5.203,28.159,5.203,20z">' +
            '<animateTransform attributeName="transform"' +
            'type="rotate"' +
            'from="0 20 20"' +
            'to="360 20 20"' +
            'calcMode="spline"' +
            'keySplines="0.4, 0, 0.2, 1"' +
            'keyTimes="0;1"' +
            'dur="2s"' +
            'repeatCount="indefinite" />' +
            '</path>' +
            '<path fill="#3B83BD" stroke="#3B83BD" stroke-width="0.50" stroke-miterlimit="10" d="M7.078,20' +
            'c0-7.125,5.797-12.922,12.922-12.922V6.875C12.763,6.875,6.875,12.763,6.875,20S12.763,33.125,20,33.125v-0.203' +
            'C12.875,32.922,7.078,27.125,7.078,20z">' +
            '<animateTransform attributeName="transform"' +
            'type="rotate"' +
            'from="0 20 20"' +
            'to="360 20 20"' +
            'dur="1.8s"' +
            'repeatCount="indefinite" />' +
            '</path>' +
            '</svg>' +
            '</div>' +
            '</div>' +
            '</div>'
    
};
$(function () {
    var app_router;
    var bandera = 0;

    IndexFn.init = function () {
        this.Block(true);
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
                IndexFn.Block(false);
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
                IndexFn.Block(false);
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
                IndexFn.SetTime();
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
                    IndexFn.SetTime();
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

        var menuid = (url.toLowerCase().search('perfil') > 0 ? 0 : 1);
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
                IndexFn.SetTime();
                IndexFn.ClearAlert();
                $('#divDinamico').load(url);
                $('#divDinamico').append(IndexFn.ClearAlert());
            }
        }
        return false;
    };
    IndexFn.PopDatosAlumnoClick = function () {
        location.reload();
    };
    IndexFn.ConstruirMenu = function () {
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
        IndexFn.SetTime();
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
    IndexFn.Block = function (option) {
        if (option) {
            var Message = "";
            if ($('#Load').length) { Message = $('#Load'); }
            else {
                var e = document.getElementById('Load');
                if (e != null) { e.removeChild('body'); }

                $('#divDinamico').after(IndexFn.Message);
                
                Message = $('#Load');
            }
            $.blockUI({
                message: Message,
                css: { backgroundColor: '#48525e', color: '#fff', border: 'none' }
            });
        }
        else {
            $.unblockUI({ onUnblock: function () { } });
        }
    };

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
                IndexFn.SetTime();
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
                if (direccion.search("login.html") != -1) {
                    Backbone.history.stop();
                    localStorage.clear();
                    $(location).attr('href', 'login.html');
                    return false;
                } else {
                    IndexFn.SetTime();
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

            IndexFn.SetTime();
            IndexFn.ClearAlert();
            $('#divDinamico').load(url);

            bandera = 1;
        }
    });


    IndexFn.init();
});