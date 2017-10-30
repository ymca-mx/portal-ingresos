$(document).ready(function init() {
    var Funciones = {
        TraerAlumno: function () {
            $.ajax({
                url: 'Services/Alumno.asmx/Datos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: "{'alumnoId':'" + $.cookie('user') + "'}",
                dataType: 'json',
                success: function (Resultado) {
                    Datos = Resultado.d;
                    if (Datos == null) {
                        alert('Error en la carga de datos generales');
                        $(location).attr('href', 'login.html');
                    }
                    else {

                        $('#imgUsuario').attr('src', 'data:image/' + Datos.extensionImagen + ';base64,' + Datos.imagenBase64);
                        $('#lblUsuario span').text(Datos.nombre);

                        if (Datos.extensionImagen == ".png")
                            $('#imgUsuario').attr('src', 'Style/engato/index/imagenes/Guest.png');
                        //Funciones.RevisaAnticipado();
                        Funciones.ConstruirMenu();                        
                    }
                },
                error: function (Resultado) {
                    alert('Se presento un error en la validación de las credenciales');
                }
            });
        },
        VerificarDatos: function () {
            var AlumnoNum = $.cookie('user');
            $.ajax({
                type: "POST",
                url: "Services/Alumno.asmx/VerificaAlumnoDatos",
                data: "{AlumnoId:'" + AlumnoNum + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d) {
                        $('#popDatos').load('Views/Alumno/AlumnoActualizaDatos.html' , function () {
                            $('#popDatos').modal('show');
                        });
                    }
                    //else
                    //{
                    //    Funciones.VerificarEncuesta();
                    //}

                }
            });
        },
        VerificarEncuesta: function () {
            var AlumnoNum = $.cookie('user');
            $.ajax({
                type: "POST",
                url: "Services/Alumno.asmx/VerificaAlumnoEncuesta",
                data: "{AlumnoId:'" + AlumnoNum + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.d) {
                        $('#divDinamico').load('Views/Alumno/EncuestaPortal.html');
                    }
                }
            });
        },
        RevisaAnticipado: function () {
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
        },
        contenidoClick: function () {

            $('#divDinamico').empty();
            var url = $(this).attr("href");
            $('#divDinamico').load(url);
            return false;
        },
        PopDatosAlumnoClick: function () {
            location.reload();
        },
        ConstruirMenu: function () {
            var ul = "";
            $(Funciones.Menu).each(function () {
                ul += '<li class="menu-dropdown classic-menu-dropdown ">' +
                    '<a data-hover="megamenu-dropdown" data-close-others="true" data-toggle="dropdown" href="javascript:;">' +
                    this.Descripcion + '<i class="' + this.icono + '"></i>' +
                    '</a>' +
                    '<ul class="dropdown-menu pull-left">';
                $(this.SubMenus).each(function () {
                    ul += '<li>' +
                        '<a href="' + this.Direccion + '" class="contenido">' +
                        '<i class="' + this.Icono + '"></i>  ' + this.Descripcion +
                        '</a></li>';
                });
            });
            $('#MenuItems').append(ul);
            $(".contenido").click(Funciones.contenidoClick);
            Funciones.VerificarDatos();
        },
        Menu: {
            MenuId: 1,
            Descripcion: "Pagos",
            icono: "fa fa-angle-down",
            SubMenus: [
                {
                    SubMenuId: 1,
                    Descripcion: 'Consultar Referencias',
                    Direccion: 'Views/Pago/Pagos2.html',
                    Icono: 'fa fa-bank'
                },
                {
                    SubMenuId: 2,
                    Descripcion: 'Estado de Cuenta',
                    Direccion: 'Views/Pago/EstadodeCuenta2.html',
                    Icono: 'fa fa-bar-chart-o'
                },
                {
                    SubMenuId: 3,
                    Descripcion: 'Generar Referencias - Trámites',
                    Direccion: 'Views/Pago/GenerarReferencias.html',
                    Icono: 'fa fa-money'
                },
                {
                    SubMenuId: 4,
                    Descripcion: 'Referencias de Reinscripción',
                    Direccion: 'Views/Pago/ReInscripcion.html',
                    Icono: 'fa fa-book'
                }]
        }
    };    
    $('#popDatos').on('hidden.bs.modal', Funciones.PopDatosAlumnoClick);
    Funciones.TraerAlumno();     
   
});