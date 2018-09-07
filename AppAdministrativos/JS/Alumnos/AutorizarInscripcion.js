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
                            var link = "";
                            if (data.AlumnoAutorizacion === null) {
                                link = "<a href=''onclick='return false;' name='edit' class='btn btn-success'> Autorizar </a> ";
                            } else {
                                link = "<a href=''onclick='return false;' name='show' class='btn btn-info'> Ver detalles </a> ";
                            }
                            return link;
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
                "order": [[2, "desc"]]
            });
            var fil = $('#tblAlumnos_filter label input');
            fil.removeClass('input-small').addClass('input-large');

            IndexFn.Block(false);
        },
        TraerAlumnos: function () {
            IndexFn.Block(true);
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
                        IndexFn.Block(false);
                    }
                }
            });
        },
        DatosConsulta:[],
        CrearCombo: function (datos) {
            $('#slcPeriodos').empty();
            $('#slcEstatus').empty();

            Funciones.DatosConsulta = datos;
            var PeriodosC = [];
            var EstatusArr = [-1];
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
                var Estatus = 1;
                if (this.AlumnoAutorizacion !== null) { Estatus = 2; }
                if (jQuery.inArray(Estatus, EstatusArr) === -1) { EstatusArr.push(Estatus); }
            });
            
            var option = $(document.createElement('option'));
            option.text("--Todos--");
            option.val(-1);

            $('#slcPeriodos').append(option);

            $(PeriodosC).each(function () {
                var anio = this.substring(0, 4);
                var periodo = this.substring(4, 5);
                
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

            
            $(EstatusArr).each(function (ind) {
                var option3 = $(document.createElement('option'));                
                var texto = "";

                switch (EstatusArr[ind]) {
                    case -1: texto = "--Todos--";
                        break;
                    case 1: texto = "Sin Autorizacion";
                        break;
                    case 2: texto = "Con Autorizacion";
                }                

                option3.val(EstatusArr[ind]);
                option3.text(texto)
                $('#slcEstatus').append(option3);      
            });

            $('#slcEstatus').val(-1);
            $('#slcPeriodos').val(-1);
            $('#slcPeriodos').on('change', Funciones.SeleccionarPeriodo);
            $('#slcEstatus').on('change', Funciones.SeleccionarEstatus);

            Funciones.SeleccionarPeriodo();
            
        },
        Condiciones: function () {
            var datos = [];
            var val = $('#slcPeriodos').val();
            var an = $('#slcPeriodos').find(':selected').data("anio");
            var per = $('#slcPeriodos').find(':selected').data("periodoid");
            var estatus = $('#slcEstatus').val();
            

            if (val !== "-1") {
                $(Funciones.DatosConsulta).each(function () {
                    if (an === this.Anio && per === this.PeriodoId) {
                        if (estatus === "-1") {
                            datos.push(this);
                        } else if (estatus === "1") {
                            if (this.AlumnoAutorizacion === null) {
                                datos.push(this);
                            }
                        } else if (estatus === "2") {
                            if (this.AlumnoAutorizacion !== null) {
                                datos.push(this);
                            }
                        }
                    }
                });
                Funciones.PintarTabla(datos);
            } else {
                $(Funciones.DatosConsulta).each(function () {
                    if (estatus === "-1") {
                        datos.push(this);
                    } else if (estatus === "1") {
                        if (this.AlumnoAutorizacion === null) {
                            datos.push(this);
                        }
                    } else if (estatus === "2") {
                        if (this.AlumnoAutorizacion !== null) {
                            datos.push(this);
                        }
                    }
                });
                Funciones.PintarTabla(datos);
            }
        },
        SeleccionarEstatus: function () {
            Funciones.Condiciones();
        },
        SeleccionarPeriodo: function () {
            Funciones.Condiciones();
        },
        Autorizar: function () {

            var row = this.parentNode.parentNode;
            var rowadd = Funciones.tblAlumnos.fnGetData($(this).closest('tr'));

            if (this.name === 'edit') {
                alertify.confirm("Se generaran los cargos para: " + rowadd.AlumnoId + " | " + rowadd.Nombre, function () {
                    Funciones.GenerarCargos(rowadd);
                });
            } else if (this.name === 'show') {
                $('#frmModal')[0].reset();
                $('#txtAlumno').val(rowadd.AlumnoId + " | " + rowadd.Nombre);
                $('#txtUsuario').val(rowadd.AlumnoAutorizacion.UsuarioId + " | " + rowadd.AlumnoAutorizacion.NombreUsuario);
                $('#txtFecha').val(rowadd.AlumnoAutorizacion._Fecha);
                $('#txtHora').val(rowadd.AlumnoAutorizacion._Hora);
                $('#MostrarDetalle').modal('show');
            }
        },
        GenerarCargos: function (alumno) {
            alumno.UsuarioId =  localStorage.getItem('userAdmin');
            alumno.FechaInscripcion = "";
            alumno = { objAlumno: alumno };
            alumno = JSON.stringify(alumno);

            IndexFn.Block(true);
            $.ajax({
                url: 'WS/Alumno.asmx/GenerarCargos',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: alumno,
                dataType: 'json',
                success: function (data) {
                    if (data.d === true) {
                        IndexFn.Block(false);
                        alertify.alert("Se generaron los cargos correctamente, el alumno ya podra visualizarlos en su portal.",
                            function () {
                                Funciones.TraerAlumnos();
                            });
                    } else {
                        IndexFn.Block(false);
                        alertify.alert("Ocurrio un Error al momento de generar los cargos, favor de llamar a Sistemas.")                        
                    }
                }
            });
        }
    };
    Funciones.TraerAlumnos();
    $('#tblAlumnos').on('click', 'a', Funciones.Autorizar);
});