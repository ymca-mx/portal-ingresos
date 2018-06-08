$(document).ready(function () {
    //alert('Cargada');
    var tbAlumnos;
  
    $.fn.editable.defaults.mode = 'popup';

    $.ajax({
        url: 'Services/Alumno.asmx/Concentrado',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: "{}",

        success: function (Resultado) {
            tbAlumnos = $('#tbAlumnos').dataTable({
                'aaData': Resultado.d,
                'aoColumns': [
                    { 'mDataProp': 'alumnoId', 'width': 'auto', 'sClass': 'centrado' },
                    { 'mDataProp': 'nombre', 'width': 'auto'},
                    {
                        'mDataProp': 'biblioteca', 'mRender': function (data, type, full) {
                            if (data == '1') {
                                return '<input type=\"checkbox\" checked value="' + data + '">';
                            }
                            else {
                                return '<input type=\"checkbox\" value="' + data + '">';
                            }
                        }, 'sClass': 'centrado'
                    },
                    {
                        'mDataProp': 'doctos', 'mRender': function (data, type, full) {
                            if (data == '1') {
                                return '<input type=\"checkbox\" checked value="' + data + '"> <a data-toggle="modal" href="#responsive"> &nbsp; Consultar</a>';
                            }
                            else {
                                return '<input type=\"checkbox\" value="' + data + '"><a data-toggle="modal" href="#responsive"> &nbsp; Consultar</a>';
                            }
                        }, 'sClass': 'centrado'
                    },
                    {
                        'mDataProp': 'adeudo', 'mRender': function (data, type, full) {
                            if (data == '1') {
                                return '<input type=\"checkbox\" checked value="' + data + '">';
                            }
                            else {
                                return '<input type=\"checkbox\" value="' + data + '">';
                            }
                        }, 'sClass': 'centrado'
                        
                    },
                    { 'mDataProp': 'beca'},
                ],
                'lengthMenu': [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                'searching': true,
                'ordering': true,
                'info': false,
                'language': {
                    'lengthMenu': '_MENU_ Registros',
                    'paginate': {
                        'previous': '<',
                        'next': '>'
                    },
                    'search': 'Buscar Alumno'
                },
                "fnCreatedRow": function (row, data, index) {
                    //$('td:eq(5)', row).addClass('second_td');
                    $('td:eq(5)', row).html("<a href='javascript:;' pk-data='' data-placemente='left' data-type='text'>" + $('td:eq(5)', row).text() + "%</a>");
                    
                },
                "fnRowCallback": function (row, data, index) {
                    $('td:eq(5) a', row).editable({
                        type: 'text',
                        name: 'Beca',
                        placement: 'left',
                        mode: 'popup',
                        title: 'Porcentaje de Beca',
                        inputclass: 'entrada'

                        //tpl: "<input type='text' style='width: 50px'>"
                    });
                }
            });
        }
    });
});