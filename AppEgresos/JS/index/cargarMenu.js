$(document).ready(function () {
    Menu();
    function Menu() {
        var Menu = "";

        $.blockUI({
            message: "<h1>Cargando Menu, por favor espere....</h1>",
            css: { backgroundColor: '#48525e', color: '#fff' }
        });

        $.ajax({
            url: '/Api/Index/ConsultarMenu',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: {usuarioId : $.cookie('userAdminE') },
            dataType: 'json',
            success: function (Resultado) {
                Datos = Resultado;
                if (Datos == null) {
                    alert('Error en la carga del menu');
                    $.unblockUI();
                }
                else {
                    $(Resultado).each(function () {
                        Menu += '<li class="menu-dropdown mega-menu-dropdown ">' +
                            '<a class="dropdown-toggle"  data-hover="megamenu-dropdown" data-close-others="true" data-toggle="dropdown" href="#" aria-haspopup="true" aria-expanded="false">'+

                            this.Descripcion +

                            ' <i class="fa fa-angle-down"></i>' +
                            '</a>' +
                            '<ul class="dropdown-menu pull-left">';
                        $(this.SubMenu).each(function (ind, ele) {
                            Menu +=
                             '<li class="dropdown-submenu">' +
                            '<a href="' + ele.Direccion + '" class="contenido">' +
                             '<i class="fa fa-history"></i>'
                             + ele.Descripcion +
                             '</a>' +
                             '</li>';
                        });
                        Menu += '</ul>' + '</li>';
                    });
                    $('#Menu').append(Menu);
                    $.unblockUI();
                }
            },
            error: function (Resultado) {
                alert('Se presento un error en la validación de las credenciales');
                $.unblockUI();
                $(location).attr('href', 'Login.html');
            }
        });
    }
});