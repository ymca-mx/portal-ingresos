$(document).ready(function () {

     $(".contenido").click(function () {

         $('#divDinamico').empty();
         var url = $(this).attr("href");
         $('#divDinamico').load(url);
         return false;
     });

     $.ajax({
         url: 'Services/Alumno.asmx/Datos',
         type: 'POST',
         contentType: 'application/json; charset=utf-8',
         data: "{'alumnoId':'" + $.cookie('user') + "'}",
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
                 //RevisaAnticipado();
             }
         },
         error: function (Resultado) {
             alert('Se presento un error en la validación de las credenciales');
         }
     });

     function RevisaAnticipado()
     {
         $.ajax({
             url: 'Services/Alumno.asmx/CalcularAnticipado',
             type: 'POST',
             contentType: 'application/json; charset=utf-8',
             data: "{'alumnoId':'" + $.cookie('user') + "'}",
             dataType: 'json',
             success: function (Resultado) {

                 if (Resultado.d.length > 1) {
                     var text = $('#lblFecha');
                     text[0].innerText = Resultado.d[1];
                     $('#btnPop').click();
                 }
             }
         });
     }
   
});