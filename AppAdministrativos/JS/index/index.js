﻿$(function init() {

    //$("#Menu").on('click', '.contenido', function () {

    //    $('#divDinamico').empty();
    //    var url = $(this).attr("href");
    //    $('#divDinamico').load(url);
    //    return false;
    //});
    var Funciones ={        
       
        TraerImagen: function () {
            $.ajax({
                url: 'Api/Usuario/Datos/' + localStorage.getItem('userAdmin'),
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
            })
                .done(function (Resultado) {
                    Datos = Resultado;
                    if (Datos == null)
                        alert('Error en la carga de datos generales');
                    else {

                        $('#imgUsuario').attr('src', 'data:image/' + Datos.extensionImagen + ';base64,' + Datos.imagenBase64);
                        $('#lblUsuario span').text(Datos.nombre);

                        if (Datos.extensionImagen == ".png")
                            $('#imgUsuario').attr('src', 'Style/engato/index/imagenes/Guest.png');
                    }
                })
                .fail(function (Resultado) {
                    alert('Se presento un error en la validación de las credenciales');
                });
        }
    }
    Funciones.TraerImagen();

});