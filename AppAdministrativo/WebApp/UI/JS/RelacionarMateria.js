$(document).ready(function () {
    CargarOfertas();
    CargarDias();
    CargarDocentes();

    function CargarDias() {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/Dias",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            dataType: "json",
            success: function (data) {
                var datos = data.d;                
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.DiaId);

                    $("#slcDia").append(option);
                });
            }
        });
    }
    function CargarDocentes() {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Docentes.asmx/ListaDocentes",
            data: "{}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            dataType: "json",
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Nombre);
                    option.val(this.DocenteId);

                    $("#slcDocentes").append(option);
                });
            }
        });
    }
    function CargarOfertas() {
        $("#slcOferta").empty();
        var plantel = -1
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativaTipo",
            data: "{Plantel:'" + plantel + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.OfertaEducativaTipoId);

                    $("#slcOferta").append(option);
                });
                //$("#slcSexo").html(data); // show the string that was returned, this will be the data inside the xml wrapper
                $("#slcOferta").change();
            }
        });
    }
    $("#slcOferta").change(function () {
        $("#slcCarrera").empty();
        var tipo = $("#slcOferta");
        tipo = tipo[0].value;
        var plantel = -1;
        if (tipo != -1) {
            $('#lblOFerta').html(tipo == 1 ? 'Licenciatura' : tipo == 2 ? 'Especialidad' : tipo == 3 ? 'Maestría' : tipo == 4 ? 'Idioma' : tipo == 5 ? 'Doctorado' : ' ');
            $.ajax({
                type: "POST",
                url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativa",
                data: "{tipoOferta:'" + tipo + "',Plantel:'" + plantel + "'}", // the data in form-encoded format, ie as it would appear on a querystring
                //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
                contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
                dataType: "json",
                success: function (data) {
                    var datos = data.d;
                    if (datos.length > 0) {
                        $(datos).each(function () {
                            var option = $(document.createElement('option'));
                            option.text(this.Descripcion);
                            option.val(this.OfertaEducativaId);

                            $("#slcCarrera").append(option);
                        });
                    } else {
                        $("#slcCarrera").append(option);
                    }
                    $('#slcCarrera').change();
                }
            });
        } else {
            $("#slcCarrera").append(optionP);
        }
    });
    $('#slcCarrera').change(function () {
        $("#slcMateria").empty();
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Materia.asmx/ListarAsignatura",
            data: "{OfertaEducativaId:'" + $('#slcCarrera') .val()+ "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            dataType: "json",
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));
                    option.text(this.Descripcion);
                    option.val(this.AsignaturaId);

                    $("#slcMateria").append(option);
                });
            }
        });
    });
    $('#btnAgregar').click(function () {
        var OfertaTipo = $('#slcOferta option:selected').html();
        var Oferta = $('#slcCarrera option:selected').html();
        var Materia = $('#slcMateria option:selected').html();
        var Dia = $('#slcDia option:selected').html();
        var Hora = $('#txtHoraI').val() + " " + $('#txtHoraF').html();
        var Docente = $('#slcDocentes option:selected').html();
        var horaid = $('#slcDia').val();
        var td = horaid == 1 ? "<td>" + Hora + "</td><td>" + Materia + "</td>" : horaid == 2 ? "<td>" + Hora + "</td><td></td><td>" + Materia + "</td>" : horaid == 3 ? "<td>" + Hora + "</td><td></td><td></td><td>" + Materia + "</td>" :
            horaid == 4 ? "<td>" + Hora + "</td><td></td><td></td><td></td><td>" + Materia + "</td>" : horaid == 5 ? "<td>" + Hora + "</td><td></td><td></td><td></td><td></td><td>" + Materia + "</td>" : horaid == 6 ?
           "<td>" + Hora + "</td><td></td><td></td><td></td><td></td><td></td><td>" + Materia + "</td>" : "<td>" + Hora + "</td><td></td><td></td><td></td><td></td><td></td><td></td><td>" + Materia + "</td>";

        var tr = "<tr>" + td + "</tr>";
        document.getElementById("tblDias").insertRow(-1).innerHTML = tr;
        //$('#tblDias > tbody').append(tr);
    });
});