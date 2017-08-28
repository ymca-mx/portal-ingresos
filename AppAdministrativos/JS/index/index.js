$(function init() {

    //$("#Menu").on('click', '.contenido', function () {

    //    $('#divDinamico').empty();
    //    var url = $(this).attr("href");
    //    $('#divDinamico').load(url);
    //    return false;
    //});
    var Funciones ={        
        btnSalir: function () {
            $.removeCookie('userAdmin', { path: '/' });
            var url = $(this).attr("href");
            $(location).attr('href', url);
        },
        TraerImagen: function () {
            $.ajax({
                url: 'WS/Usuario.asmx/Datos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: "{'usuarioId':'" + $.cookie('userAdmin') + "'}",
                dataType: 'json',
                success: function (Resultado) {
                    Datos = Resultado.d;
                    if (Datos == null)
                        alert('Error en la carga de datos generales');
                    else {

                        $('#imgUsuario').attr('src', 'data:image/' + Datos.extensionImagen + ';base64,' + Datos.imagenBase64);
                        $('#lblUsuario span').text(Datos.nombre);

                        if (Datos.extensionImagen == ".png")
                            $('#imgUsuario').attr('src', 'Style/engato/index/imagenes/Guest.png');
                    }
                },
                error: function (Resultado) {
                    alert('Se presento un error en la validación de las credenciales');
                }
            });
        }
    }
    Funciones.TraerImagen();
    $('#btnSalir').on('click', btnSalir);
});