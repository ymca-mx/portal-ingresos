$(document).ready(function () {
    Menu();
    function Menu() {
        var Menu = "";
        $.ajax({
            url: 'Services/Usuario.asmx/ConsultarMenu',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: "{'usuarioId':'" + $.cookie('userAdmin') + "'}",
            dataType: 'json',
            success: function (Resultado) {
                Datos = Resultado.d;
                if (Datos == null)
                    alert('Error en la carga del menu');
                else {
                    $(Resultado.d).each(function () {
                        Menu += '<li class="menu-dropdown classic-menu-dropdown ">' +
                            '<a data-hover="megamenu-dropdown" data-close-others="true" data-toggle="dropdown" href="javascript:;">' +

                            this.Descripcion +

                            ' <i class="fa fa-angle-down"></i>' +
                            '</a>' +
                            '<ul class="dropdown-menu pull-left">';
                        $(this.SubMenu).each(function (ind, ele) {
                            Menu +=
                             '<li >' +
                             '<a href="' + ele.Direccion + '" class="contenido">' +
                             '<i class="fa fa-history"></i>'
                             + ele.Descripcion +
                             '</a>' +
                             '</li>';
                        });
                        Menu += '</ul>' + '</li>';
                    });
                    $('#Menu').append(Menu);
                }
            },
            error: function (Resultado) {
                alert('Se presento un error en la validación de las credenciales');
                $(location).attr('href', 'Login.html');
            }
        });
    }
});