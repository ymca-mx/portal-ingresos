$(function init() {
    var Funciones = {
        tblAlumnos: null,
        PintarTabla: function (datos) {
            Funciones.tblAlumnos = $('#tblAlumnos').dataTable({
                "aaData": datos,
                "aoColumns": [
                    { "mDataProp": "AlumnoId" },
                    { "mDataProp": "Nombre" },
                    { "mDataProp": "_FechaInscripcion" },
                    { "mDataProp": "PeriodoDescripcion" },
                    { "mDataProp": "OfertaEducativa.Descripcion" },
                    { "mDataProp": "UsuarioNombre" },
                    {
                        "mDataProp": function (data) {
                            return "<a href=''onclick='return false;' class='btn btn-success'> Autorizar </a> ";
                        }
                    }
                ],
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                "searching": true,
                "ordering": true,
                "info": false,
                "async": true,
                "bDestroy": true,
                "language": {
                    "lengthMenu": "_MENU_  Registros",
                    "paginate": {
                        "previous": "<",
                        "next": ">"
                    },
                    "search": "Buscar Alumno ",
                },
                "order": [[0, "desc"]]
            });
            var fil = $('#tblAlumnos_filter label input');
            fil.removeClass('input-small').addClass('input-large');

            $('#Load').modal('hide');
        },
        TraerAlumnos: function () {
            $('#Load').modal('show');
            $.ajax({
                url: 'WS/Alumno.asmx/ListaPorAutorizar',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{}',
                dataType: 'json',
                success: function (data) {
                    if (data.d.length > 0) {
                        Funciones.CrearCombo(data.d);
                        
                    } else {
                        alertify.alert("No hay registros que mostrar.")
                        $('#Load').modal('hide');
                    }
                }
            });
        },
        DatosConsulta:[],
        CrearCombo: function (datos) {
            Funciones.DatosConsulta = datos;
            var PeriodosC = [];

            var listperiodos = [
                {
                    PeriodoId: 1,
                    Inicial: 'Septiembre',
                    Final: 'Diciembre'
                },
                {
                    PeriodoId: 2,
                    Inicial: 'Enero',
                    Final: 'Abril'
                },
                {
                    PeriodoId: 3,
                    Inicial: 'Mayo',
                    Final: 'Agosto'
                },
            ];
            

            $(datos).each(function () {
                var periodo = this.Anio + '' + this.PeriodoId;
                if (jQuery.inArray(periodo, PeriodosC) === -1) {
                    PeriodosC.push(periodo);
                }
            });
            
            var option = $(document.createElement('option'));
            option.text("--Todos--");
            option.val(-1);

            $('#slcPeriodos').append(option);

            $(PeriodosC).each(function () {
                var anio = this.substring(0, 4);
                var periodo = this.substring(4, 5);

                console.log(this);

                var option2 = $(document.createElement('option'));

                $(listperiodos).each(function () {
                    if (this.PeriodoId == periodo) {
                        option2.text(this.Inicial + " - " + this.Final + " " + anio);
                    }
                });

                option2.val(this);
                option2.attr("data-Anio", anio);
                option2.attr("data-PeriodoId", periodo);

                $('#slcPeriodos').append(option2);              
            });           

            $('#slcPeriodos').val(-1);
            $('#slcPeriodos').on('change', Funciones.SeleccionarPeriodo);

            Funciones.SeleccionarPeriodo();
            
        },
        SeleccionarPeriodo: function () {
            var datos = [];
            var val = $('#slcPeriodos').val();
            var an = $('#slcPeriodos').find(':selected').data("anio");
            var per = $('#slcPeriodos').find(':selected').data("periodoid");
            if (val !== "-1") {
                $(Funciones.DatosConsulta).each(function () {
                    if (an === this.Anio && per === this.PeriodoId) {
                        datos.push(this);
                    }
                });
                Funciones.PintarTabla(datos);
            } else { Funciones.PintarTabla(Funciones.DatosConsulta);}
        },
        Autorizar: function () {
            var row = this.parentNode.parentNode;
            var rowadd = Funciones.tblAlumnos.fnGetData($(this).closest('tr'));
            alertify.confirm("Se generaran los cargos para: " + rowadd.AlumnoId + " | " + rowadd.Nombre, function () {
                Funciones.GenerarCargos(rowadd);
            });
        },
        GenerarCargos: function (alumno) {
            alumno.UsuarioId = $.cookie('userAdmin');
            alumno.FechaInscripcion = "";
            alumno = { objAlumno: alumno };
            alumno = JSON.stringify(alumno);

            $('#Load').modal('show');
            $.ajax({
                url: 'WS/Alumno.asmx/GenerarCargos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: alumno,
                dataType: 'json',
                success: function (data) {
                    if (data.d === true) {
                        $('#Load').modal('hide');
                        alertify.alert("Se generaron los cargos correctamente, el alumno ya podra visualizarlos en su portal.",
                            function () {
                                Funciones.TraerAlumnos();
                            });
                    } else {
                        $('#Load').modal('hide');
                        alertify.alert("Ocurrio un Error al momento de generar los cargos, favor de llamar a Sistemas.")                        
                    }
                }
            });
        }
    };
    Funciones.TraerAlumnos();
    $('#tblAlumnos').on('click', 'a', Funciones.Autorizar);
});