$(function init() {
    $('#btnBuscar').on('click', function () {
        var lbl = $('#lblNombre');
        lbl[0].innerHTML = "";
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0) { return false; }        
        $('#Load').modal('show');
        BuscarAlumno(AlumnoId);
    });

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            $('#btnBuscar').click();
        }
    });

    $('#sclMesinicial').on('change', function () {
        var mes = $(this).val();
        Combo2(mes === '-1' ? Combo2(mes) : mes < 8 ? (parseInt(mes) + 5) : (parseInt(mes) - 7));
    });

    function Combo2(Mes) {
        $('#sclMesFinal').val(Mes);
    }

    function BuscarAlumno(idAlumno) {
        $.ajax({
            type: "POST",
            url: "WS/Alumno.asmx/ConsultarAlumno",
            data: "{AlumnoId:'" + idAlumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                var lbl = $('#lblNombre');
                lbl[0].innerHTML = data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno;
                lbl[0].innerHTML += data.d.AlumnoInscrito.EsEmpresa == true ? (data.d.AlumnoInscrito.EsEspecial == true ? " - Alumno Especial  " : " - Grupo  Empresarial") + " - " + data.d.Grupo.Descripcion : "";
                TraerReferencias(idAlumno);
            }
        });
    }

    function TraerReferencias(AlumnoId) {
        $.ajax({
            type: "POST",
            url: "WS/Descuentos.asmx/",
            data: JSON.stringify({ id: 1, Pass: Jose1991 }),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                console.log(data);
            }
        });
    }
});