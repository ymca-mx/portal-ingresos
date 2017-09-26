$(function init() {
    //AppRouter = 
    var bandera = 0;
    // Initiate the router
    //var app_router = new AppRouter;
    var app_router;
    var Funciones = {
        alertify3: function () {
            return '<script src="Style/Complementos/Alertify/alertify.js"></script>'; 
        },
        btnSalir: function () {
            $.removeCookie('userAdmin', { path: '/' });
            Backbone.history.stop();
            var url = "login.html";
            $(location).attr('href', url);
        },        
        CrearMenu: function () {
            var Menu = "";

            $.blockUI({
                message: "<h1>Cargando Menu, por favor espere....</h1>",
                css: { backgroundColor: '#48525e', color: '#fff' }
            });

            $.ajax({
                url: 'WS/Usuario.asmx/ConsultarMenu',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: "{'usuarioId':'" + $.cookie('userAdmin') + "'}",
                dataType: 'json',
                success: function (Resultado) {
                    Datos = Resultado.d;
                    if (Datos == null) {
                        alert('Error en la carga del menu');
                        $.unblockUI();
                    }
                    else {
                        Funciones.Menu = [];
                        $(Resultado.d).each(function () {
                            var objmenu =
                                {
                                    MenuId: this.MenuId,
                                    Descripcion: this.Descripcion,
                                    SubMenu: []
                                };
                            Menu += '<li class="menu-dropdown mega-menu-dropdown ">' +
                                '<a class="dropdown-toggle"  data-hover="megamenu-dropdown" data-close-others="true" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +

                                this.Descripcion +

                                ' <i class="fa fa-angle-down"></i>' +
                                '</a>' +
                                '<ul class="dropdown-menu pull-left">';
                            $(this.SubMenu).each(function (ind, ele) {
                                var objSubmneu = {
                                    SubMenuId: ele.SubmenuId,
                                    Descripcion: ele.Descripcion,
                                    Direccion: ele.Direccion
                                };
                                Menu +=
                                    '<li class="dropdown">' +
                                    '<a name="menu" href="#Views/' + ele.SubmenuId + '" class="contenido">' +
                                    '<i></i>'
                                    + ele.Descripcion +
                                    '</a>' +
                                    '</li>';
                                objmenu.SubMenu.push(objSubmneu);
                            });
                            Menu += '</ul>' + '</li>';
                            Funciones.Menu.push(objmenu);
                        });

                        var MenuNuevoIngreso = {
                            Descripcion: "",
                            MenuId: 0,
                            SubMenu: {
                                SubMenuId: 0,
                                MenuId: 0,
                                Descripcion: "Inscripcion Nuevo Ingreso",
                                Direccion: "Views/Alumno/InscripcionAlumno.html"
                            }
                        };
                        Funciones.Menu.push(MenuNuevoIngreso);

                        $('#Menu').append(Menu);
                        app_router = new AppRouter;
                        Backbone.history.start();
                        $('a[name=menu]').on('click', Funciones.ClickMenu);
                        $.unblockUI();                        
                    }
                },
                error: function (Resultado) {
                    alert('Se presento un error en la validación de las credenciales');
                    $.unblockUI();
                    $(location).attr('href', 'Login.html');
                }
            });
        },
        Menu: [],
        ClickMenu: function () {
            bandera = 0;
            var url = $(this).attr('href');
            if (url === undefined) { url = '#'; }

           

            app_router.navigate(url, { trigger: true });
            if (bandera === 0) {
                var arrurl = $(this).attr('href');
                if (arrurl !== undefined) {
                    arrurl = arrurl.split('/');
                    var id = arrurl[(arrurl.length - 1)];
                    id = parseInt(id);

                    $(Funciones.Menu).each(function () {
                        $(this.SubMenu).each(function () {
                            if (id === this.SubMenuId) {
                                url = this.Direccion;
                            }
                        });
                    });
                }
                $('#divDinamico').empty();

                if (url !== '#') {
                    $('#divDinamico').load(url);
                    $('#divDinamico').append(Funciones.alertify3());
                }
            }
            return false;
        }
    };
    var AppRouter = Backbone.Router.extend({
        routes: {
            "Views/:SubMenuId": "LoadPage",
            "*actions": "defaultRoute",
        },
        LoadPage: function (SubMenuId) {
            if (typeof $.cookie('userAdmin') === 'undefined') {
                Backbone.history.stop();
                $(location).attr('href', "login.html");
                return false;
            }
            $('#divDinamico').empty();

            var direccion = "";
            var subInt = parseInt(SubMenuId);
            $(Funciones.Menu).each(function () {
                $(this.SubMenu).each(function () {
                    if (subInt === this.SubMenuId) {
                        direccion = this.Direccion;
                    }
                });
            });
            if (direccion.length > 0) {
                $('#divDinamico').load(direccion);
                $('#divDinamico').append(Funciones.alertify3());
            }
            bandera = 1;
        },        
        defaultRoute: function (actions) {
            if (typeof $.cookie('userAdmin') === 'undefined') {
                Backbone.history.stop();
                $(location).attr('href', "login.html");
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

            $('#divDinamico').load(url);
            $('#divDinamico').append(Funciones.alertify3());

            bandera = 1;
        }
    });   

    Funciones.CrearMenu();
    
    $('#btnSalir').on('click', Funciones.btnSalir);

    

});