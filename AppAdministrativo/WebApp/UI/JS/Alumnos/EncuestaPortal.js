$(document).ready(function () {
    var AlumnoNum, datos, estatusTXA;
    var form = $('#submit_form');
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);
    AlumnoNum = $.cookie('userAdmin');

    form.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block help-block-error', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        rules: {

        },

        invalidHandler: function (event, validator) { //display error alert on form submit       
            //alert('Empeze');
            success.hide();
            error.show();
            Metronic.scrollTo(error, -200);
        },
        errorPlacement: function (error, element) { // render error placement for each input type
            var icon = $(element).parent('.input-icon').children('i');
            icon.removeClass('fa-check').addClass("fa-warning");
            icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
        },
        highlight: function (element) { // hightlight error inputs
            $(element)
                .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group   
            b_Valid = 1;
        },

        unhighlight: function (element) { // revert the change done by hightlight
        },

        success: function (label, element) {
            var icon = $(element).parent('.input-icon').children('i');
            $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
            icon.removeClass("fa-warning").addClass("fa-check");

        },

        submitHandler: function (form) {
            success.show();
            error.hide();
        }
    });

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

    function ratingDisable() {
        $('select').barrating('destroy');
    }

    $('.rating-enable').click(function (event) {
        event.preventDefault();

        ratingEnable();

        $(this).addClass('deactivated');
        $('.rating-disable').removeClass('deactivated');
    });

    $('.rating-disable').click(function (event) {
        event.preventDefault();

        ratingDisable();

        $(this).addClass('deactivated');
        $('.rating-enable').removeClass('deactivated');
    });

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

  
    $("#DivDinamico").on("click", "label", function () {

        var valorid = $(this).data('valorid');
        if ($(this).data('valorid') == 9 || $(this).data('valorid') == 12)
        {
            var textarea = $(this).attr("id").substring(1);
            $("#div" + textarea).show();
            estatusTXA = 1;

        } else if ($(this).data('valorid') == 10 || $(this).data('valorid') == 11)
        {
            var textarea = $(this).attr("id").substring(1);
            $("#div" + textarea).hide();
            estatusTXA = 2;
        }

    });

    function Preguntas() {
        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/PreguntasPortal",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d != null) {
                    datos = data.d;
                    var html = "";
                    $(datos).each(function (i0, d0) {
                        if (this.PreguntaTipoId == 1 )
                        {
                            html += '<div class="col-md-12 ">' +
                                '<h4>' + d0.Descripcion + '</h4>' +
                                '</div>' +
                                '<div class="col-md-12 ">' +
                                '<div class="examples" >' +
                                 '<div class="stars stars-example-css">' +
                                  '<select id="' + d0.PreguntaId + '" data-tipoid = "' + d0.PreguntaTipoId + '" name="rating" autocomplete="off">';
                            objid.push(d0.PreguntaId);
                            $(d0.Opciones).each(function (i, d) {
                                html += '<option value="' + d.PreguntaTipoValoresId + '">' + d.Descripcion + '</option>';
                                   });
                                  html += '</br>' +
                                  '</select>' +
                            '</div>' +
                            '</div>' +
                            '<hr>' +
                                '</div>';

                                  raiting = "#" + d0.PreguntaId.toString();;
                        } else if (d0.PreguntaTipoId == 2)
                        {
                            html += '<div class="col-md-12 ">' +
                                 '<h4>' + d0.Descripcion + '</h4>' +
                            '</div>' +
                                '<div class="col-md-12 ">' +
                                '<div class="clearfix">' +
                             '<div class="btn-group btn-group-circle" data-toggle="buttons">';
                            objid.push("0"+d0.PreguntaId);
                            $(d0.Opciones).each(function (i, d) {
                                html += ' <label class="btn green " data-tipoid = "' + d0.PreguntaTipoId + '" data-valorid = "' + d.PreguntaTipoValoresId + '" id="' + i + d0.PreguntaId + '" >' +
                                  '<input type="radio" class="toggle">' + d.Descripcion +
                                  '</label>';
                              });
                              html += '</div>' +
                             '</div>' +
                             '<hr>' +
                      '</div>';
                        } else if (d0.PreguntaTipoId == 3) {

                            html += '<div class="col-md-12 ">' +
                                '<h4>' + d0.Descripcion + '</h4>' +
                            '</div>' +
                                '<div class="col-md-12 ">' +
                                '<div class="form-group">';
                                  objid.push(d0.PreguntaId);
                                 $(d0.Opciones).each(function (i, d) {
                                     html += ' <textarea class="form-control" rows="3" data-tipoid = "' + d0.PreguntaTipoId + '" data-valorid = "' + d.PreguntaTipoValoresId + '" id="' + d0.PreguntaId + '" ></textarea>';
                                 });
                            html += '  </div>' +
                             '<hr>' +
                          ' </div>';

                        } else if (d0.PreguntaTipoId == 4 || d0.PreguntaTipoId == 5) {

                            html +=
                                '<div class="col-md-12 ">' +
                                '<h4>' + d0.Descripcion + '</h4>' +
                           '</div>' +
                               '<div class="col-md-12 ">' +
                               '<div class="clearfix">' +
                            '<div class="btn-group btn-group-circle" data-toggle="buttons">';
                            objid.push("0"+d0.PreguntaId);
                            $(d0.Opciones).each(function (i, d) {
                                html += ' <label class="btn green " data-tipoid = "' + d0.PreguntaTipoId + '" data-valorid = "' + d.PreguntaTipoValoresId + '" id="' + i + d0.PreguntaId + '" >' +
                                '<input type="radio" class="toggle">' + d.Descripcion +
                                '</label>';
                            });
                            html += '</div>' +
                           '</div>' +
                           '<hr>' +
                           '</div>' +
                           '<div class="col-md-12" style="display:none"  id ="div' + d0.PreguntaId + '">' +
                            '<div class="col-md-12" >' +
                            '<h4>' + d0.SupPregunta + '</h4>' +
                            '</div>' +
                                '<div class="col-md-12 ">' +
                            '<div class="form-group">' +
                            '<div class="col-md-12">';
                            html += ' <textarea class="form-control" rows="3"   id=l"' + d0.PreguntaId + '" ></textarea>' +
                            '  </div>' +
                            '  </div>' +
                             '<hr>' +
                            ' </div>' +
                           
                            ' </div>';
                        }
                      

                    });
                    $("#DivDinamico1").append(html);
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

    function validacion()
    {
       $(objid).each(function () {
           var id = this.toString();
           var tipoid = $(this).data('tipoid');
           if (tipoid == 2 || tipoid == 4 || tipoid == 5)
           {
               var id2 = id.substring(1);
               if ($("#" + id).hasClass("active") || $("#" + id2).hasClass("active")) {
                   alert(objid + " corecto");
               } else { alert(objid + " incorecto"); }

              
               if (tipoid == 4)
               {
                   if ($("#" + id).hasClass("active"))
                   {
                       if ($("#l" + id2).val() != ""  )
                       {
                           alert(objid + " corecto");
                       } else { alert(objid + " incorecto"); }
                   }
               }else if(tipoid==5)
               {
                   if ($("#1" + id2).hasClass("active")) {
                       if ($("#l" + id2).val() != "") {
                           alert(objid + " corecto");
                       } else { alert(objid + " incorecto"); }
                   }
               }

           }
           if(tipo == 3)
           {
               if ($("#" + id).val() != "")
               {
                   alert(objid + " corecto");
               } else { alert(objid + " incorecto"); }
           }


        });
        
    }

    $('#Guardar').on('click', function () {
        validacion();
       
        //var lista = [];
        //var pregunta = 0;
        //var respuesta = 0;
        //var Comentario = "";
        //$(datos).each(function (i0, d0) {

        //    if (d0.PreguntaTipoId == 1)
        //    {
        //        pregunta = d0.PreguntaId;
        //        respuesta = $("#" + d0.PreguntaId).val();
        //        Comentario = "";

        //    } else if (d0.PreguntaTipoId == 2)
        //    {
        //        pregunta = d0.PreguntaId;
        //        if ($("#0" + d0.PreguntaId).hasClass("active")) {
        //            respuesta = $("#0" + d0.PreguntaId).data('valorid');

        //        } else { respuesta = $("#1" + d0.PreguntaId).data('valorid'); }
        //        Comentario = "";

        //    } else if (d0.PreguntaTipoId == 3) {
        //        pregunta = d0.PreguntaId;
        //        respuesta = $("#" + d0.PreguntaId).data('valorid');
        //        Comentario = $("#" + d0.PreguntaId).val();

        //    } else if (d0.PreguntaTipoId == 4 || d0.PreguntaTipoId == 5) {
        //        pregunta = d0.PreguntaId;
        //        if ($("#0" + d0.PreguntaId).hasClass("active")) {
        //            respuesta = $("#0" + d0.PreguntaId).data('valorid');

        //        } else { respuesta = $("#1" + d0.PreguntaId).data('valorid'); }
                
        //        if(estatusTXA ==1)
        //        {
        //            Comentario = $("#l" + d0.PreguntaId).val();
        //        } else if (estatusTXA == 2)
        //        {
        //            Comentario = "";
        //        }
                   
        //    }




        //    var obj = {
        //            "AlumnoId": AlumnoNum,
        //            "Pregunta": pregunta,
        //            "Respuesta": respuesta,
        //            "Comentario": Comentario
                
        //    };

        //    lista.push(obj);
           
        //});

        //var obj2 = {
        //    'RespuesasEncuesta': {
        //        "Respuestas": lista
        //    }
        //};
        //obj2 = JSON.stringify(obj2);

        //GuardarTodo(obj2);



       // if (form.valid() == false) { return false; }
        //var nombre = $('#hCarga');
        //nombre[0].innerText = "Guardando";
        //$('#Load').modal('show');
        //GuardarTodo();
    });



    function GuardarTodo(obj2) {

        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Alumno.asmx/GuardarRespuestas",
            data: obj2,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d) {
                    $('#Load').modal('hide');
                    alertify.alert("Datos del Alumno Modificados");
                    //$('#PopDatosAlumno').modal('hide');
                } else {
                    $('#Load').modal('hide');
                    $('#PopDatosAlumno').modal('hide');
                    alertify.alert("Error, Revisar datos capturados.");
                   // $('#PopDatosAlumno').modal('hide');
                }
            }
        });


    }






});
