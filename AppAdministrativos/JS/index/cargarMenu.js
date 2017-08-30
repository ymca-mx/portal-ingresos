$(function init() {
    //AppRouter = 

    // Initiate the router
    //var app_router = new AppRouter;

    var Funciones = {
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
                                '<a class="dropdown-toggle"  data-hover="megamenu-dropdown" data-close-others="true" data-toggle="dropdown" href="#" aria-haspopup="true" aria-expanded="false">' +

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
                                    '<li class="dropdown-submenu">' +
                                    '<a href="#/Views/' + ele.SubmenuId + '" class="contenido">' +
                                    '<i class="fa fa-history"></i>'
                                    + ele.Descripcion +
                                    '</a>' +
                                    '</li>';
                                objmenu.SubMenu.push(objSubmneu);
                            });
                            Menu += '</ul>' + '</li>';
                            Funciones.Menu.push(objmenu);
                        });
                        $('#Menu').append(Menu);
                        var app_router = new AppRouter;
                        Backbone.history.start();
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
        Menu: []
    };
    Funciones.CrearMenu();
    
    $('#btnSalir').on('click', Funciones.btnSalir);

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
                }

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
                //console.log('Perras');
                $('#divDinamico').load(url);
            }
    });

});