$(document).ready(function () {
    var Email;
    var Alumno;
    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });
    $('#btnBuscar').on('click', function () {
        $('#Load').modal('show');
        Email = "";
        Alumno = 0;
        var AlumnoId = $('#txtClave').val();
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/TraerAlumnoDetalle",
            data: "{AlumnoId:'" + AlumnoId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                $('#Load').modal('hide');
                if (data.d == null) {
                    alertify.alert("Alumno no encotrado..");
                    return false;
                }
                $('#lblNombre').text(data.d.Nombre);
                $('#txtMail').val(data.d.Email);
                Email = data.d.Email;
                Alumno = data.d.AlumnoId;
            }
        });
    });
    $('#btnEnviar').on('click', function () {
        $('#Load').modal('show');
        var Usuario = $.cookie('userAdmin');
        var Email1 = $('#txtMail').val();
        if (Email1.length > 0) {
            if (Email1 == Email) {
                EnviarMail(Alumno, Usuario);
            } else {
                UpdateEmail(Alumno, Email1, Usuario);
            }
        }
    });
    function UpdateEmail(AlumnoId, Email, Usuario) {
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/UpdateMail",
            data: "{AlumnoId:'" + AlumnoId + "',Email:'" + Email + "',UsuarId:'" + Usuario+ "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d == null) {
                    alertify.alert("No se guardaron los cambios, intente de nuevo");
                    return false;
                }
                EnviarMail(AlumnoId, Usuario);
            }
        });
    }
    function EnviarMail(AlumnoId, Usuario) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/EnviarMailUpdate",
            data: "{AlumnoId:'" + AlumnoId + "',UsuarioId:'" + Usuario +"'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                $('#Load').modal('hide');
                if (data.d.length>0) {
                    alertify.alert("No se pudo enviar el correo al alumno favor de revisar el Correo");
                    return false;
                }
                alertify.alert("Se enviaron las credenciales al alumno");
                $('#txtMail').val('');
                $('#lblNombre').text('');
                Alumno = 0;
                Email = '';
            }
        });
    }
});