$(document).ready(function () {
    $('.selectpicker').selectpicker();

    $("#btnCrear").click(function () {
        $("#divCompactaciones").hide();
        $("#divApertura").show();
        $("#lbAccion").text("Apertura Compactacion");
        $('#Load').modal('show');
        CargarOfertas();
    });

    $("#btnregresar").click(function () {
        $("#divApertura").hide();
        $("#divCompactaciones").show();
        $("#lbAccion").text("Compactaciones");
    });

    function CargarOfertas() {
        var option1 = $(document.createElement('option'));
        option1.text('--Seleccionar--');
        option1.val('-1');
        $("#slcOferta").append(option1);

        $.ajax({
            url: '/Api/Compactacion/ofertaEducativa',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: '',
            dataType: 'json',
            success: function (data) {
                $(data).each(function () {
                    if (data == null) { return false; }
                    var option2 = $(document.createElement('option'));
                    option2.text(this.ofertaEducativa);
                    option2.val(this.ofertaEducativaId);
                    $("#slcOferta").append(option2);
                });
                $('#Load').modal('hide');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
                $('#Load').modal('hide');
            }
        });
    }

    $("#slcOferta").change(function () {
        var oferta = $("#slcOferta").val();
        $.ajax({
            url: '/Api/Compactacion/catalogos',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: {ofertaEducativaId : oferta},
            dataType: 'json',
            success: function (data) {
                if (data == null) { return false; }
                $('#Load').modal('show');
                CargarCatalogos(data);
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
                $('#Load').modal('hide');
            }
        });
    });

    function CargarCatalogos(data) {
        //materias// 
        $("#slcMateria").empty();
        $("#slcMateria").selectpicker("refresh");
        if (data.materias != null)
        {
            $(data.materias).each(function () {
                var option3 = $(document.createElement('option'));
                option3.text(this.clave + " | " + this.materia + " | "  + this.creditos );
                option3.val(this.materiaId);
                $("#slcMateria").append(option3);
            });
            $("#slcMateria").selectpicker("refresh");
        }
        //Grupos// 
        $("#slcGrupo").empty();
        var option1 = $(document.createElement('option'));
        option1.text('--Seleccionar--');
        option1.val('-1');
        $("#slcGrupo").append(option1);
        if (data.grupos != null)
        {
            $(data.grupos).each(function () {
                var option3 = $(document.createElement('option'));
                option3.text(this.grupo);
                option3.val(this.grupoId);
                $("#slcGrupo").append(option3);
            });
        }
        $('#Load').modal('hide');
    }

    $("#btnAgregar").click(function ()
    {
        var materia = $("#slcMateria").val();
        var grupo = $("#slcGrupo").val();
        if (materia == -1 && grupo == -1) {
            alertify("Debe de seleccionar todas las opciones");
            return false;
        }
    });

    function agregar()
    {
        
    }

});