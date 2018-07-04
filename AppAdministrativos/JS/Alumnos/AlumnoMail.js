$(function () {

    var Email, Alumno;

    var AlumnoMail = {
        init() {
            $('#txtClave').on('keydown', this.txtClaveKey);
            $('#btnBuscar').on('click', this.bntBuscar);
            $('#frmCambios').on('submit', this.frmSubmit);
        },
        txtClaveKey(e) {
            if (e.which == 13) {
                AlumnoMail.bntBuscar();
            }
        },
        bntBuscar() {
            $('#Load').modal('show');
            Email = "";
            Alumno = 0;
            var AlumnoId = $('#txtClave').val();

            IndexFn.Api("Alumno/TraerAlumnoDetalle/" + AlumnoId, "GET", "")
                .done(function (data) {
                    $('#Load').modal('hide');
                    $('#lblNombre').text(data.Nombre);
                    $('#txtMail').val(data.Email);
                    Email = data.Email;
                    Alumno = data.AlumnoId;
                })
                .fail(function (data) {
                    if (data == null) {
                        $('#Load').modal('hide');
                        alertify.alert("Universidad YMCA", "Alumno no encotrado..");
                        return false;
                    }
                });
        },
        frmSubmit(e) {
            e.preventDefault();

            $('#Load').modal('show');
            var Usuario = $.cookie('userAdmin');
            var Email1 = $('#txtMail').val();
            if (Email1.length > 0) {
                if (Email1 == Email) {
                    AlumnoMail.EnviarMail(Alumno, Usuario);
                } else {
                    AlumnoMail.UpdateEmail(Alumno, Email1, Usuario);
                }
            }
        },
        UpdateEmail(AlumnoId, Email, Usuario) {
            var objAlumno = {
                AlumnoId: AlumnoId,
                Email: Email,
                UsuarioId: Usuario
            };

            IndexFn.Api("Alumno/UpdateMail", "POST", JSON.stringify(objAlumno))
                .done(function (data) {
                    if (data === null) {
                        alertify.alert("Universidad YMCA", "No se guardaron los cambios, intente de nuevo");
                        return false;
                    }
                    AlumnoMail.EnviarMail(AlumnoId, Usuario);
                })
                .fail(function (data) {
                    if (data === null) {
                        alertify.alert("Universidad YMCA", "No se guardaron los cambios, intente de nuevo");
                        return false;
                    }
                });

        },
        EnviarMail(AlumnoId, Usuario) {
            IndexFn.Api("General/EnviarMailUpdate/" + AlumnoId + "/" + Usuario, "GET", "")
                .done(function (data) {
                    $('#Load').modal('hide');
                    if (data.length > 0) {
                        alertify.alert("Universidad YMCA", "No se pudo enviar el correo al alumno favor de revisar el Correo");
                        return false;
                    }
                    alertify.alert("Universidad YMCA", "Se enviaron las credenciales al alumno");
                    $('#txtMail').val('');
                    $('#lblNombre').text('');
                    Alumno = 0;
                    Email = '';
                })
                .fail(function (data) {
                    console.log(data);
                    alertify.alert("Universidad YMCA", "No se pudo enviar el correo al alumno favor de revisar el Correo");
                });
        }
    };

    AlumnoMail.init();
});