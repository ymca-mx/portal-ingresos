$(document).ready(function () {
    var MItable;
    //$('#txtnombre').focusout(function () {
    //    Buscar();
    //});
    $('#txtApPaterno').focusout(function () {
        Buscar();
    });
    $("#txtApMaterno").focusout(function () {
        Buscar();
    });

function Buscar()
{
    var Nomb=$('#txtnombre').val();
    var aPaterno = $('#txtApPaterno').val();
    var aMaterno = $('#txtApMaterno').val();
    if ((aPaterno == "" || aPaterno == " ") && (aMaterno == "" || aMaterno == " ")) {
        $('#pulsate-regular').attr("hidden", "hidden");
        return false;
    }
    $.ajax({
        type: "POST",
        url: "../WebServices/WS/Alumno.asmx/BuscarAlumno",
        data: "{Nombre:'" + Nomb + "',Paterno:'"+ aPaterno + "',Materno:'" + aMaterno + "'}", // the data in form-encoded format, ie as it would appear on a querystring
        //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
        contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
        dataType: "json",
        success: function (Respuesta) {
            if (Respuesta.d.length > 0) {
                MItable = $('#Alumnos').dataTable({
                    "aaData": Respuesta.d,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId", "Nombre": "AlumnoId", visible: false },
                        {
                            "mDataProp": "Nombre",
                            "mRender": function (data) {
                                return "<a href=''onclick='return false;'>" + data + " </a> ";
                            }
                        },
                        { "mDataProp": "DTOAlumnoDetalle.FechaNacimientoC" },
                        { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                        //{ "mDataProp": "FechaSeguimiento" },
                        { "mDataProp": "Usuario.Nombre" }
                    ],
                    "searching": false,
                    "ordering": false,
                    "info": false,
                    "async": true,
                    "bDestroy": true

                });
                $('#pulsate-regular').removeAttr("hidden", "hidden");
            }
            else { $('#pulsate-regular').attr("hidden", "hidden"); }
            
        },
        error: function (Respuesta) {
            alertify.alert('Error al cargar datos');
        }
    });
}
});