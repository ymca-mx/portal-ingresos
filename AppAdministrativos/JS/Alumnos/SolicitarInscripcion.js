$(document).ready(function init() {
    var AlumnoId;
    var tblAlumnos;

    var Funciones = {
        AlumnoDB: undefined,
        ClickBuscar: function () {
            AlumnoId = $('#txtAlumno').val();
            if (!isNaN(AlumnoId)) { Funciones.CargarAlumno(); }
            else { Funciones.BuscarNombre(); }
        },
        CargarAlumno: function () {
            IndexFn.Block(true);
            alertify.alert().destroy();
            Funciones.AlumnoDB = undefined;
            $('#slcOFertaEducativa').empty();
            $('#slcPeriodo').empty();
            $('#txtDetalle').val('');

            $.ajax({
                url: 'WS/Reinscripcion.asmx/ObtenerAlumno',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{AlumnoId:"' + AlumnoId + '"}',
                dataType: 'json',
                success: function (data) {
                    IndexFn.Block(false);
                    if (data.d !== null) {
                        Funciones.AlumnoDB = data.d;
                        $('#lblNombre').text(data.d.Nombre);
                        Funciones.CargarOfertas();
                    } else {
                        alertify.error("No se regreso nada, favor de hablar a sistemas.");
                    }
                }
            });
        },
        CargarOfertas: function () {
            var option = $(document.createElement('option'));
            option.text('--Selecionar--');
            option.val(-1);
            $('#slcOFertaEducativa').append(option);

            $(Funciones.AlumnoDB.ListaOfertas).each(function () {
                var option1 = $(document.createElement('option'));
                option1.text(this.Descripcion);
                option1.val(this.OfertaEducativaId);
                $('#slcOFertaEducativa').append(option1);
            });
        },
        SeleccionarAlumno: function () {
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
            $('#txtAlumno').val(rowadd.AlumnoId);
            $('#frmVarios').hide();
            Funciones.ClickBuscar();
        },
        BuscarNombre: function () {
            IndexFn.Block(true);
            $.ajax({
                url: 'WS/Alumno.asmx/BuscarAlumnoString',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{Filtro:"' + AlumnoId + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data !== null) {
                        $('#frmVarios').show();
                        tblAlumnos = $('#tblAlumnos').dataTable({
                            "aaData": data.d,
                            "aoColumns": [
                                { "mDataProp": "AlumnoId" },
                                { "mDataProp": "Nombre" },
                                { "mDataProp": "FechaRegistro" },
                                { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                //{ "mDataProp": "FechaSeguimiento" },
                                {
                                    "mDataProp": function (data) {
                                        return "<a class='btn green'>Seleccionar</a>";
                                    }
                                }
                            ],
                            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                            "searching": false,
                            "ordering": false,
                            "async": true,
                            "bDestroy": true,
                            "bPaginate": true,
                            "bLengthChange": false,
                            "bFilter": false,
                            "bInfo": false,
                            "pageLength": 5,
                            "bAutoWidth": false,
                            "asStripClasses": null,
                            "language": {
                                "lengthMenu": "_MENU_  Registros",
                                "paginate": {
                                    "previous": "<",
                                    "next": ">"
                                },
                                "search": "Buscar Alumno "
                            },
                            "order": [[2, "desc"]]
                        });
                    }
                    IndexFn.Block(false);

                }
            });
        },
        DetectarEnter: function (e) {
            if (e.which === 13) {
                Funciones.ClickBuscar();
            }
        },
        OfertaChange: function () {
            var oferta = $('#slcOFertaEducativa').val();
            $('#slcPeriodo').empty();
            if (oferta !== "-1") {
                oferta = parseInt(oferta);

                $(Funciones.AlumnoDB.ListaOfertas).each(function () {
                    if (parseInt(this.OfertaEducativaId) === oferta) {
                        var option = $(document.createElement('option'));
                        option.text('--Selecionar--');
                        option.val(-1);
                        $('#slcPeriodo').append(option);
                        $(this.ListaPeriodos).each(function () {
                            //if (this.Inscripcion !== null || this.VistoBueno !== null) {
                                var option1 = $(document.createElement('option'));
                                option1.text(this.descripcion);
                                option1.val(this.anio + '' + this.periodoId);
                                option1.attr("data-Anio", this.anio);
                                option1.attr("data-Periodoid", this.periodoId);
                                $('#slcPeriodo').append(option1);
                            //}
                        });
                    }
                });
            } else {                
                $('#txtDetalle').val('');
            }
        },
        PeriodoChange: function () {
            if ($(this).val() !== "-1") {
                var combo = $(this).find(':selected');
                var anio = $(combo[0]).data('anio');
                var periodo = $(combo[0]).data('periodoid');

                var oferta = $('#slcOFertaEducativa').val();
                oferta = parseInt(oferta);

                $(Funciones.AlumnoDB.ListaOfertas).each(function () {
                    if (this.OfertaEducativaId === oferta) {
                        $(this.ListaPeriodos).each(function () {
                            if (this.periodoId === periodo && this.anio === anio) {
                                $('#txtDetalle').prop('readonly', (this.Inscripcion === null && this.Solicitud === null ? false : true));

                                $('#txtDetalle').val((this.Inscripcion === null && this.Solicitud === null ? ''
                                    : this.Inscripcion === null && this.Solicitud !== null ? this.Solicitud.Observaciones
                                        : this.Inscripcion !== null && this.Solicitud !== null ? (this.Solicitud.Observaciones + '\n' + 'El alumno ya fue inscrito el día ' + this.Inscripcion.Fecha + ' por   ' + this.Inscripcion.Usuario) 
                                            : ('El alumno ya fue inscrito el día ' + this.Inscripcion.Fecha + ' por   ' + this.Inscripcion.Usuario)));
                                $('#btnGuardar').prop('disabled', (this.Inscripcion === null && this.Solicitud === null ? '' : 'disabled'));
                            }
                        });
                    }
                });
            } else {
                $('#txtDetalle').val('');
            }
        },
        Guardar: function () {
            var combo = $('#slcPeriodo').find(':selected');
            var anio = $(combo[0]).data('anio');
            var periodo = $(combo[0]).data('periodoid');
            var oferta = $('#slcOFertaEducativa').val();
            oferta = parseInt(oferta);

            var objAlumno = {
                AlumnoId: AlumnoId,
                OfertaEducativaId: oferta,
                Anio: anio,
                PeriodoId: periodo,
                Comentario: $('#txtDetalle').val(),
                UsuarioId:  localStorage.getItem('userAdmin')
            };

            objAlumno = JSON.stringify(objAlumno);

            IndexFn.Block(true);
            $.ajax({
                url: 'WS/Reinscripcion.asmx/SolicitarInscripcion',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: objAlumno,
                dataType: 'json',
                success: function (data) {
                    if (data !== null) {
                        if (data.d) {
                            IndexFn.Block(false);
                            alertify.alert("Se guardo Correctamente", function () { Funciones.ClickBuscar(); });
                        } else {
                            IndexFn.Block(false);
                            alertify.alert("Ocurrio un error al solicitar la inscripción, favor de llamar a Sistemas.");
                        }
                    } else {
                        IndexFn.Block(false);
                        alertify.alert("Ocurrio un error al solicitar la inscripción, favor de llamar a Sistemas.");                     
                    }
                },
                error: function () {
                    alertify.alert("Ocurrio un error al solicitar la inscripción, favor de llamar a Sistemas.");
                    IndexFn.Block(false);
                }
            });
        },
        init: function () {
            $('#btnBuscarAlumno').on('click', Funciones.ClickBuscar);
            $('#tblAlumnos').on('click', 'a', Funciones.SeleccionarAlumno);
            $('#txtAlumno').on('keydown', Funciones.DetectarEnter);
            $('#slcOFertaEducativa').on('change', Funciones.OfertaChange);
            $('#slcPeriodo').on('change', Funciones.PeriodoChange);
            $('#btnGuardar').on('click', Funciones.Guardar);
        }       
    };

    Funciones.init();
});