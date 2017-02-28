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
                 VerificarDatos();
             }
         },
         error: function (Resultado) {
             alert('Se presento un error en la validación de las credenciales');
         }
     });
     function VerificarDatos() {
         var AlumnoNum = $.cookie('user');
         $.ajax({
             type: "POST",
             url: "../WebServices/WS/Alumno.asmx/VerificaAlumnoDatos",
             data: "{AlumnoId:'" + AlumnoNum + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: 'json',
             success: function (data) {
                 if (data.d) {
                     $('#popDatos').load('../inscritos/Alumno/AlumnoActualizaDatos.html');
                 } else
                 {
                     VerificarEncuesta();
                 }

             }
         });
     }

     function VerificarEncuesta() {
         var AlumnoNum = $.cookie('user');
         $.ajax({
             type: "POST",
             url: "../WebServices/WS/Alumno.asmx/VerificaAlumnoEncuesta",
             data: "{AlumnoId:'" + AlumnoNum + "'}",
             contentType: "application/json; charset=utf-8",
             dataType: 'json',
             success: function (data) {
                 if (data.d) {
                     $('#popDatos').load('../inscritos/Alumno/EncuestaPortal.html');
                 }
             }
         });
     }



     $('#PopDatosAlumno').on('hidden.bs.modal', function () {
         location.reload();
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