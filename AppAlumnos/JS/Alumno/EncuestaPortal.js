$(document).ready(function () {
    var AlumnoNum, datos, estatusTXA, valid, html;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    AlumnoNum = $.cookie('user');

    Load();
    var raiting = '';
    var objid = [];

    function Load() {
        $('#PopEncuesta').modal('show');
        LimpiarCampos();
        $('#Load').modal('show');
       Preguntas();
    }

    function ratingEnable() {
        $(raiting).barrating({
            theme: 'css-stars',
            showSelectedRating: true,
        });
    }

    ratingEnable();

    function LimpiarCampos() {
        $("#submit_form").trigger('reset');
        error.hide();
        success.hide();
        $('#submit_form  div').removeClass('has-error');
        $('#submit_form  div').removeClass('has-success');
        $('#submit_form  i').removeClass('fa-warning');
        $('#submit_form  i').removeClass('fa-check');
    }

    $("#DivDinamico1").on("click", "label", function () {

        var estatus = $(this).data('estatus');
        if (estatus == true)
        {
            var objeto = $(this).attr("id").substring(1);
            $("#divH" + objeto).show();
            estatusTXA = 1;

        } else 
        {
            var objeto = $(this).attr("id").substring(1);
            $("#divH" + objeto).hide();
            estatusTXA = 2;
        }
        
    });

    function Preguntas() {
        $.ajax({
            type: "POST",
            url: "Services/Alumno.asmx/PreguntasPortal",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    datos = data.d;
                    html = '';
                    $(datos).each(function (i0, d0) {
                        if (d0.Preguntas[0].PreguntaTipoId == 1 )
                        {
                            Raiting(d0, 0, "","S"); 

                            if (d0.Preguntas[1].PreguntaTipoId != 0) {
                                switch (d0.Preguntas[1].PreguntaTipoId) {
                                    case 1:
                                        Raiting(d0, 1, 'hidden = "hidden"',"H");
                                        break;
                                    case 2:
                                        SiNo(d0, 1, 'hidden = "hidden"', "H");
                                        break;
                                    case 3:
                                        Comentario(d0, 1, 'hidden = "hidden"', "H");
                                        break;
                                    case 4:
                                        Opciones7(d0, 1, 'hidden = "hidden"', "H");
                                        break;
                                }
                            }
                        }
                        else if (d0.Preguntas[0].PreguntaTipoId == 2)
                        {
                            SiNo(d0, 0, "","S");

                            if (d0.Preguntas[1].PreguntaTipoId != 0)
                            {
                                switch (d0.Preguntas[1].PreguntaTipoId) {
                                    case 1:
                                        Raiting(d0, 1, 'hidden = "hidden"',"H"); 
                                        break;
                                    case 2:
                                        SiNo(d0, 1, 'hidden = "hidden"',"H");
                                        break;
                                    case 3:
                                        Comentario(d0, 1, 'hidden = "hidden"', "H");
                                        break;
                                    case 4:
                                        Opciones7(d0, 1, 'hidden = "hidden"', "H");
                                        break;
                                }
                            }
                        }
                        else if (d0.Preguntas[0].PreguntaTipoId == 3)
                        {
                                Comentario(d0, 0, "","S");
                                if (d0.Preguntas[1].PreguntaTipoId != 0) {
                                    switch (d0.Preguntas[1].PreguntaTipoId) {
                                        case 1:
                                            Raiting(d0, 1, 'hidden = "hidden"', "H");
                                            break;
                                        case 2:
                                            SiNo(d0, 1, 'hidden = "hidden"', "H");
                                            break;
                                        case 3:
                                            Comentario(d0, 1, 'hidden = "hidden"', "H");
                                            break;
                                        case 4:
                                            Opciones7(d0, 1, 'hidden = "hidden"', "H");
                                            break;
                                    }
                                }
                        }

                    });
                    $("#DivDinamico1").append(html);

                    $('input[name=radio2]').iCheck({
                        checkboxClass: 'icheckbox_square-red',
                        radioClass: 'iradio_square-red',
                        increaseArea: '20%' // optional
                    });

                    ratingEnable();
                    $('#Load').modal('hide');
                }
                else {
                    $('#PopEncuesta').modal('hide');
                    $('#Load').modal('hide');
                }

            }
        });
    }

    function  validacion()
    {
       valid = 1;
       $(objid).each(function () {
           var id = this.toString();
           var tipoid = $("#" + id).data('tipoid');
           var esVisible = $("#div" + id).is(":visible");
             /*Tipo id
        2 = si / no
        3 = comentario
        4 = 7 opciones
        */
           if (esVisible)
           {
               switch (tipoid) {
                   case 2:
                       if ($("#div" + id + " input[name='radio" + id + "']:radio").is(':checked')) {
                           $("#v" + id).hide();
                       } else { $("#v" + id).show(); valid = 0; }

                       break;
                   case 3:

                       if ($("#" + id).val() != "") {
                           $("#" + id).closest('.form-group').removeClass('has-error').addClass('has-success');
                       } else { $("#" + id).closest('.form-group').removeClass("has-success").addClass('has-error'); valid = 0; }

                       break;
                   case 4:

                       if ($("#div" + id + " input[name='radio2']:radio").is(':checked')) {
                           $("#v" + id).hide();
                       } else { $("#v" + id).show(); valid = 0; }
                       break;
               }
           }

       });

       if (valid == 1)
       {
           success.hide();

       } else
       {
           error.show();
       }
        
    }

    $('#Guardar').on('click', function () {
        validacion();
        if (valid == 1) {
            var nombre = $('#hCarga');
            nombre[0].innerText = "Guardando";
            $('#Load').modal('show');
            var lista = [];


            $(datos).each(function (i0, d0) {
                var pregunta = 0;
                var respuesta1 = 0;
                var respuesta2 = 0;
                var Comentario = "";

                pregunta = d0.PreguntaId;

                switch (d0.Preguntas[0].PreguntaTipoId ) {
                    case 1:
                        
                        respuesta1 = $("#S" + d0.PreguntaId).val();
                        break;
                    case 2:

                        respuesta1 = $('input:radio[name=radioS' + d0.PreguntaId + ']:checked').val();
                        if (d0.Preguntas[1].PreguntaTipoId != 0) {
                            switch (d0.Preguntas[1].PreguntaTipoId) {
                                case 3:

                                    respuesta2 = $("#S" + d0.PreguntaId).data('valorid');
                                    Comentario = $("#S" + d0.PreguntaId).val();
                                    break;
                                case 4:
                                    respuesta2 = $('input:radio[name=radio2]:checked').val();
                                    break;
                            }
                        }
                        break;
                    case 3:
                        
                        respuesta1 = $("#S" + d0.PreguntaId).data('valorid');
                        Comentario = $("#S" + d0.PreguntaId).val();
                       
                        break;
                }

                var obj = {
                        "AlumnoId": AlumnoNum,
                        "Pregunta": pregunta,
                        "Respuesta1": respuesta1,
                        "Respuesta2": respuesta2,
                        "Comentario": Comentario

                };

                lista.push(obj);

            });

            var obj2 = {
                'RespuestasEncuesta': {
                    "Respuestas": lista
                }
            };
            obj2 = JSON.stringify(obj2);

            GuardarTodo(obj2);

        }
        
       
    });

    function GuardarTodo(obj2) {

        $.ajax({
            type: "POST",
            url: "Services/Alumno.asmx/GuardarRespuestas",
            data: obj2,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $('#Load').modal('hide');
                    alertify.alert("Encuesta Guardada", function ()
                    {
                        $('#PopEncuesta').modal('hide');
                        $('#popDatos').empty();
                    });
                  
                } else {
                    $('#Load').modal('hide');
                    $('#PopDatosAlumno').modal('hide');
                    alertify.alert("Error", function ()
                    {
                        $('#PopEncuesta').modal('hide');
                        $('#popDatos').empty();
                    });
                }
            }
        });


    }

    // opciones//
    function Raiting(d0, i, hidden,SH)
    {
        html += '<div class="col-md-12 " id = "div' + SH + d0.PreguntaId + '" ' + hidden + '><div class="col-md-12 ">' +
            '<h4>' + d0.Preguntas[i].Pregunta + '</h4>' +
            '</div>' +
            '<div class="col-md-12 ">' +
            '<div class="examples" >' +
            '<div class="stars stars-example-css">' +
            '<select id="'+ SH + d0.PreguntaId + '" data-tipoid = "' + d0.Preguntas[i].PreguntaTipoId + '" name="rating" autocomplete="off">';
        objid.push(SH + d0.PreguntaId);
        $(d0.Preguntas[i].Opciones).each(function (i, d) {
            html += '<option value="' + d.PreguntaTipoValoresId + '">' + d.Descripcion + '</option>';
        });
        html += '</br>' +
            '</select>' +
            '</div>' +
            '</div>' +
            '<hr>' +
            '</div>'+
            '</div>';

        raiting = "#" + SH + d0.PreguntaId.toString();
    }

    function SiNo(d0, i, hidden, SH)
    {
        html += '<div class="col-md-12 " id = "div' + SH + d0.PreguntaId + '" ' + hidden + '><div class="col-md-12">' +
            '<h4>' + d0.Preguntas[i].Pregunta + '</h4>' +
            '</div>' +
            '<div class="col-md-12 ">' +
            '<div class="clearfix">' +
            '<div class="btn-group btn-group-circle" data-toggle="buttons">';
        objid.push(SH + d0.PreguntaId);
        $(d0.Preguntas[i].Opciones).each(function (i, d) {
            html += ' <label class="btn green " data-estatus = "' + d.Estatus + '" data-tipoid = "' + d.PreguntaTipoId + '" id="' + SH + d0.PreguntaId + '">' +
                '<input type="radio" name="radio' + SH + d0.PreguntaId + '" class="toggle" value = "' + d.PreguntaTipoValoresId + '">' + d.Descripcion +
                '</label>';
        });
        html += '</div>' +
            '</div> <div class="col-md-12 "> &nbsp;</div> ' +
            '<div class="col-md-12 ">' +
            '<span class="alert alert-danger col-md-3" style="display:none; padding: 0px;"   id ="v' + SH + d0.PreguntaId + '" >' +
            'Selecciona una opción ! </span> ' +
            '</div>' +
            '<hr>' +
            '</div>'+
            '</div>';
    }

    function Comentario(d0, i, hidden, SH)
    {

        html += '<div class="col-md-12 "id = "div' + SH + d0.PreguntaId + '" ' + hidden + '><div class="col-md-12">' +
            '<h4>' + d0.Preguntas[i].Pregunta + '</h4>' +
            '</div>' +
            '<div class="col-md-12 ">' +
            '<div class="form-group">';
        objid.push(SH + d0.PreguntaId);
        $(d0.Preguntas[i].Opciones).each(function (i, d) {
            html += ' <textarea class="form-control" rows="3" data-tipoid = "' + d.PreguntaTipoId + '" data-valorid = "' + d.PreguntaTipoValoresId + '" id="' + SH + d0.PreguntaId + '"></textarea>';
        });
        html += '  </div>' +
            '<hr>' +
            ' </div>'+
            ' </div>';
    }

    function Opciones7(d0, i, hidden, SH) {

        html += '<div class="col-md-12 " id = "div' + SH + d0.PreguntaId + '" ' + hidden + '><div class="col-md-12">' +
            '<h4>' + d0.Preguntas[i].Pregunta + '</h4>' +
            '</div>' +
            '<div class="col-md-12 "><div class="form-group"><div class="input-group"><div class="icheck-inline">';
        objid.push(SH + d0.PreguntaId);
        $(d0.Preguntas[i].Opciones).each(function (i, d) {
            html += '<div class="col-md-6 "><label><input type="radio" name="radio2" class="icheck" data-radio="iradio_square-red"  data-tipoid = "' + d.PreguntaTipoId + '" value = "' + d.PreguntaTipoValoresId + '" id="' + SH + d0.PreguntaId  + '" > ' + d.Descripcion + ' </label> </div>';
        });
        html += '</div></div></div><div class="col-md-12 "> &nbsp;</div> ' +
            '<div class="col-md-12 ">' +
            '<span class="alert alert-danger col-md-3" style="display:none; padding: 0px;"   id ="v' + SH + d0.PreguntaId + '" >' +
            'Selecciona una opción ! </span> ' +
            '</div><hr></div></div> ';
    }

    // opciones//

});