var IndexFn;

$(function () {
    //AppRouter = 
    var bandera = 0;
    // Initiate the router
    //var app_router = new AppRouter;
    var app_router;
    IndexFn = {
        clearAlert: function () {
            try { alertify.confirm().destroy(); }
            catch (err) { }
            try { alertify.alert().destroy(); }
            catch (arr) { }
            try { alertify.prompt().destroy(); }
            catch (arr3) { }
        },
        Api(url, type, data) {
            var dfd = $.Deferred();

            var Api = $.ajax({
                url: "Api/" + url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: type,
                data: data,
            });

            Api.done(function (data) {
                dfd.resolve(data);
            }).fail(function (data) {
                dfd.reject(data);
            });

            return dfd.promise();
        },
        ApiFile(url, data) {
            var dfd = $.Deferred();

            var Api = $.ajax({
                url: "Api/" + url,
                type: "POST",
                data: data,
                contentType: false,
                processData: false
            });

            Api.done(function (data) {
                dfd.resolve(data);
            }).fail(function (data) {
                dfd.reject(data);
            });

            return dfd.promise();
        },
        btnSalir: function () {
            localStorage.clear();
            Backbone.history.stop();
            var url = "login.html";
            $(location).attr('href', url);
        },        
        CrearMenu: function () {
            var Menu = "";

            IndexFn.Block(true);

            $.ajax({
                url: 'Api/Usuario/ConsultarMenu/' + localStorage.getItem('userAdmin'),
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
            })
                .done(function (Resultado) {
                    Datos = Resultado.Menu;
                    if (Datos == null) {
                        alert('Error en la carga del menu');
                        IndexFn.Block(false);
                    }
                    else {
                        IndexFn.Menu = [];
                        $(Resultado.Menu).each(function () {
                            var objmenu =
                                {
                                    MenuId: this.MenuId,
                                    Descripcion: this.Descripcion,
                                    SubMenu: []
                                };

                            Menu += '<li class="">' +
                                '<a  href="javascript:;">' +
                                '<i class="'+this.Icono+'"></i>' +
                                '<span class="title"> ' + this.Descripcion + '</span>' +
                                '<span class="arrow "></span>' +
                                '</a>' +
                                '<ul class="sub-menu">';
                            $(this.SubMenu).each(function (ind, ele) {
                                var objSubmneu = {
                                    SubMenuId: ele.SubmenuId,
                                    Descripcion: ele.Descripcion,
                                    Direccion: ele.Direccion
                                };
                                Menu +=
                                    '<li class="">' +
                                    '<a name="menu" href="#Views/' + ele.SubmenuId + '" class="contenido">' +
                                    '<i></i>'
                                    + ele.Descripcion +
                                    '</a>' +
                                    '</li>';
                                objmenu.SubMenu.push(objSubmneu);
                            });
                            Menu += '</ul>' + '</li>';
                            IndexFn.Menu.push(objmenu);
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

                        var UsuariosM = {
                            Descripcion: "",
                            MenuId: 0,
                            SubMenu: {
                                SubMenuId: -1,
                                MenuId: 0,
                                Descripcion: "Usuarios",
                                Direccion: "Views/Usuarios/Administracion.html"
                            }
                        };

                        var PerfilM = {
                            Descripcion: "",
                            MenuId: 0,
                            SubMenu: {
                                SubMenuId: -2,
                                MenuId: 0,
                                Descripcion: "Perfil",
                                Direccion: "Views/Usuarios/Perfil.html"
                            }
                        };

                        IndexFn.Menu.push(MenuNuevoIngreso);
                        IndexFn.Menu.push(UsuariosM);
                        IndexFn.Menu.push(PerfilM);

                        $('#Menu').append(Menu);
                        app_router = new AppRouter;
                        Backbone.history.start();
                        $('a[name=menu]').on('click', IndexFn.ClickMenu);
                        IndexFn.Block(false);

                    }
                })
                .fail(function (Resultado) {
                    alert('Se presento un error en la validación de las credenciales');
                    IndexFn.Block(false);
                    $(location).attr('href', 'Login.html');
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

                    $(IndexFn.Menu).each(function () {
                        $(this.SubMenu).each(function () {
                            if (id === this.SubMenuId) {
                                url = this.Direccion;
                            }
                        });
                    });
                }
                $('#divDinamico').empty();

                if (url !== '#') {
                    IndexFn.clearAlert();
                    $('#divDinamico').load(url);
                }
            }
            return false;
        },
        Block : function (option) {
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
        },
        LoadScript : function (url, callback) {
            jQuery.ajax({
                url: url,
                dataType: 'script',
                success: callback,
                async: true
            });
        },
        Message:
            '<div class="modal" tabindex="-1" data-backdrop="static" style="background-color:#0000008f;" id="Load" data-keyboard="false">' +
            '<div class="modal-dialog modal-sm" style="height: 100%;">' +
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
    var AppRouter = Backbone.Router.extend({
        routes: {
            "Views/:SubMenuId": "LoadPage",
            "*actions": "defaultRoute",
        },
        LoadPage: function (SubMenuId) {
            if (typeof  localStorage.getItem('userAdmin') === 'undefined') {
                Backbone.history.stop();
                $(location).attr('href', "login.html");
                return false;
            }
            $('#divDinamico').empty();

            var direccion = "";
            var subInt = parseInt(SubMenuId);
            $(IndexFn.Menu).each(function () {
                $(this.SubMenu).each(function () {
                    if (subInt === this.SubMenuId) {
                        direccion = this.Direccion;
                    }
                });
            });
            if (direccion.length > 0) {
                IndexFn.clearAlert();
                $('#divDinamico').load(direccion);
            }
            bandera = 1;
        },        
        defaultRoute: function (actions) {
            if (typeof  localStorage.getItem('userAdmin') === 'undefined') {
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
            
            IndexFn.clearAlert();
            $('#divDinamico').load(url);

            bandera = 1;
        }
    });   

    IndexFn.CrearMenu();
    
    $('#btnSalir').on('click', IndexFn.btnSalir);

    

});