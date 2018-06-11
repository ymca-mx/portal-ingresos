$(function init() {
    var AlumnoObject;
    var tblBecas=null;
    var Alumnoid;
    $('#btnGenerarCargos').on('click', function () {
        var txtMonto = $('#txtBecaMonto').val();
        var Perid = parseInt($('#slcPeriodo').val());
        if (Perid === -1) {
            alertify.alert('Periodo Invalido');
            return false;
        }
        if (txtMonto > 0) {

            var nombre = $('#hCarga');
            nombre[0].innerText = "Guardando";
            $('#Load').modal('show');


            var Datos = {
                alumnoId: Alumnoid,
                ofertaEducativaId: $('#slcOfertas').val(),
                anio: $('#slcPeriodo').find(':selected').data("anio"),
                periodoId: $('#slcPeriodo').find(':selected').data("periodoId"),
                porcentajeBeca: txtMonto,
                usuarioId: $.cookie('userAdmin'),
                esComite: true,
                esSEP: false,
                esEmpresa: false,
                fecha:""
            };

            var Sep = "No";
            var Empresa = false;

            $(AlumnoObject.lstDescuentos).each(function () {
                var da = this;
                if (da.OfertaEducativaId === $("#slcOfertas").val()
                    && da.Anio === an && da.PeriodoId === per) {
                    Sep = da.BecaSEP;
                }
            });
            $(AlumnoObject.PeriodosAlumno).each(function () {
                var da = this;
                if (da.OfertaEducativaId === $("#slcOfertas").val()
                    && da.Anio === an && da.PeriodoId === per && da.EsEmpresa === true) {
                    Empresa = true;
                }
            });


            Datos.esSEP = (Sep === "No" ? false : true);
            Datos.esEmpresa = Empresa;

            BecaComite(Datos);
        }
        else {
            alertify.alert('El porcentaje de la Beca no puede ir en 0%');
            return false;
        }
    });

    function BecaComite(Cadena) {
        IndexFn.Api("Beca/BecaComite", "POST", JSON.stringify(Cadena))
            .done(function (data) {
                if (data === 'Guardado') {

                    GuardarDocumentos(Cadena.alumnoId, Cadena.ofertaEducativaId, Cadena.anio, Cadena.periodoId, Cadena.esComite, Cadena.usuarioId);
                }
                else if (data === "No tiene") {
                    alertify.alert('El Alumno debe estar inscrito para poder aplicar una beca comité gracias');
                    $('#Load').modal('hide');
                }
                else {
                    alertify.alert('Error al cargar datos');
                    $('#Load').modal('hide');
                }
            })
            .fail(function (data) {
                alertify.alert('Error al cargar datos');
                $('#Load').modal('hide');
            });
    }

    $('#txtBecaMonto').keyup(function (e) {
        var ch1 = $('input[name="chkComite"]')[0];
        var ch2 = $('input[name="chkSEP"]')[0];

        var val = e.target.value;
        if (val < 0) {
            $('#txtBecaMonto').val(0);
        } else if (val > 100) {
            $('#txtBecaMonto').val(100);
        }
    });

    $('#txtAlumno').on('keydown', function (e) {
        if (e.which === 13) {
            $('#btnBuscarAlumno').click();
        }
    });

    $('#btnBuscarAlumno').click(function () {
        var alumno = $('#txtAlumno').val();
        if (alumno.length > 0) {
            $('#Load').modal('show');
            $('#slcOfertas').empty();
            $('#txtBecaMonto').val(0);
            if (tblBecas !== null) {
                tblBecas.fnClearTable();
                $('#slcPeriodo').empty();
                var optionP = $(document.createElement('option'));
                optionP.text('--Seleccionar--');
                optionP.val('-1');
                $("#slcPeriodo").append(optionP);
            }
            AlumnoObject = null;
            Alumnoid = null;
            BuscarAlumno(alumno);
        }
    });

    function BuscarAlumno(AlumnoId) {
        var nombre = $('#hCarga');
        nombre[0].innerText = "Cargando";

        $("#slcOfertas").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcOfertas").append(optionP);

        IndexFn.Api("Beca/ObtenerAlumno/" + AlumnoId, "GET", "")
            .done(function (data) {
                AlumnoObject = data;
                Alumnoid = AlumnoObject.AlumnoId;
                $('#lblNombre').text(data.Nombre);

                if (data.OfertasAlumnos.length > 1) {
                    $(data.OfertasAlumnos).each(function (s, d) {
                        var option = $(document.createElement('option'));
                        option.text(d.Descripcion);
                        option.val(d.OfertaEducativaId);
                        option.data("esEmpresa", d.EsEmpresa);

                        $("#slcOfertas").append(option);
                    });

                }
                else if (data.OfertasAlumnos.length === 1) {
                    var option = $(document.createElement('option'));
                    option.text(data.OfertasAlumnos[0].Descripcion);
                    option.val(data.OfertasAlumnos[0].OfertaEducativaId);
                    option.data("esEmpresa", data.OfertasAlumnos[0].EsEmpresa);

                    $("#slcOfertas").append(option);
                    $("#slcOfertas").val(data.OfertasAlumnos[0].OfertaEducativaId);
                    $("#slcOfertas").change();
                } else {
                    $('#Load').modal('hide');
                    return false;
                }
                $('#Load').modal('hide');
            })
            .fail(function (data) {
                $('#Load').modal('hide');
                console.log(data);
            });
    }

    $("#slcOfertas").change(function () {
        $("#btnGenerarCargos").attr("disabled", "disabled");

        var Oferta = $("#slcOfertas").val();
        $("#slcPeriodo").empty();
        var optionP = $(document.createElement('option'));
        optionP.text('--Seleccionar--');
        optionP.val('-1');
        $("#slcPeriodo").append(optionP);

        if (Oferta === -1) { return false; }

        if (!$('#slcOfertas').find(':selected').data("esEmpresa")) {
            $(AlumnoObject.PeriodosAlumno).each(function (s, d) {
                if (d.OfertaEducativaId.toString() === Oferta.toString()) {
                    var option = $(document.createElement('option'));
                    option.text(d.Descripcion);
                    option.data("anio", this.Anio);
                    option.data("periodoId", this.PeriodoId);

                    option.val(d.Anio + " " + d.PeriodoId);
                    $("#slcPeriodo").append(option);
                    $("#slcPeriodo").val(d.Anio + " " + d.PeriodoId);
                }
                $('#slcPeriodo').change();
            });
        } else{
            alertify.alert('El alumno en esta oferta educativa es tipo empresa, no es posible aplicar una beca comite.');
        }

        var desc = [];
        $(AlumnoObject.lstDescuentos).each(function () {
            var da = this;
            if (da.OfertaEducativaId.toString() === Oferta.toString()) {
                desc.push(da);
            }
        });
        //console.log(desc);
        if (desc.length > 0) { CargarDescuentos(desc);}
    });

    $('#slcPeriodo').change(function () {
        $("#btnGenerarCargos").attr("disabled");

        var Perid = parseInt($('#slcPeriodo').val());
        if (Perid === -1) { return false; }        

        $("#btnGenerarCargos").removeAttr("disabled");
    });

    function CargarDescuentos(Datos) {
        tblBecas = $("#tblBecas").dataTable({
            "aaData": Datos,
            "aoColumns": [
                { "mDataProp": "AnioPeriodoId" },
                { "mDataProp": "DescripcionPeriodo" },
                { "mDataProp": "SMonto" },
                { "mDataProp": "BecaDeportiva" },
                { "mDataProp": "BecaSEP" },
                { "mDataProp": "BecaComite" },
                { "mDataProp": "Usuario.Nombre" },
                { "mDataProp": "FechaAplicacionS" }
            ],
            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
            "searching": false,
            "ordering": false,
            "async": true,
            "bDestroy": true,
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
            "bInfo": false,
            "bAutoWidth": false,
            "asStripClasses": null,
            "language": {
                "lengthMenu": "_MENU_  Registros",
                "paginate": {
                    "previous": "<",
                    "next": ">"
                },
                "search": "Buscar Alumno ",
            },
            "order": [[2, "desc"]],
            "createdRow": function (row, data, dataIndex) {
                row.childNodes[1].style.textAlign = 'center';
                row.childNodes[2].style.textAlign = 'center';
                row.childNodes[3].style.textAlign = 'center';
                row.childNodes[4].style.textAlign = 'center';
                $('#txtBecaMonto').val(data.SMonto);
                if (data.DocComiteRutaId.length > 0) {
                    var newLinkS = $(document.createElement("a"));
                    newLinkS[0].innerText = "Archivo...";
                    newLinkS.attr('data-file', data.DocComiteRutaId)

                    var colComite = row.childNodes[5];
                    colComite.innerHTML += "<br />" + newLinkS[0].outerHTML;
                }
            }
        });
    }

    $('#ComiteArchivo').bind('change', function () {
        var file = $('#FileComite');
        var tex = $('#TxtComiteArchivo').html();
        if (this.files.length > 0) {
            $('#TxtComiteArchivo').text(RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileComite span span').text('Cambiar');
        }
        else {
            $('#TxtComiteArchivo').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileComite span span').text('Seleccionar Archivo...');
        }
    });

    $('#FileComite a').click(function () {
        var file = $('#FileComite');
        $('#TxtComiteArchivo').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileComite span span').text('Seleccionar Archivo...');
    });

    $("#tblBecas").on('click', 'a', function () {
        $('#Load').modal('show');
        var a = this;
        a = $(a).data("file");
        CrearPDF(a);

    });

    function CrearPDF(DocumentoId) {
        $.ajax({
            url: 'WS/Beca.asmx/GenerarPDF',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{DocumentoId:"' + DocumentoId + '"}',
            dataType: 'json',
            success: function (data) {
                if (data !== null) {
                    window.open(data.d, "ArchivoPDF");
                    $('#Load').modal('hide');
                } else {
                    $('#Load').modal('hide');
                }
            }
        });
    }

    function CrearPDF2(DocumentoId) {
        var url = "WS/Beca.asmx/GenerarPDF2";
        url += '?' + 'DocumentoId=' + DocumentoId;
        window.open(url, "PDF");
    }

    function RecortarNombre(name) {
        var cadena;
        if (name.length > 15) {
            cadena = name.substring(0, 8);
            cadena += name.substring(name.length - 4, name.length);
            return cadena;
        } else {
            return name;
        }
    }

    function GuardarDocumentos(Alumnoid, OfertaEducativaId, Anio, Periodo, EsComite, Usuario) {
        var data = new FormData();
        var filBeca;
        
        filBeca = $('#ComiteArchivo');
        filBeca = filBeca[0].files[0];
        data.append("DocumentoComite", filBeca);

        data.append("AlumnoId", Alumnoid);
        data.append("OfertaEducativaId", OfertaEducativaId);
        data.append("Anio", Anio);
        data.append("Periodo", Periodo);
        data.append("EsComite", EsComite);
        data.append("UsuarioId", Usuario);

        var request = new XMLHttpRequest();
        request.open("POST", 'WS/Beca.asmx/GuardarDocumentos', true);
        request.send(data);

        request.onreadystatechange = function () {
            if (request.readyState === 4) {
                if (request.readyState === XMLHttpRequest.DONE && request.status === 200) {
                    $('#Load').modal('show');
                    $('#slcOfertas').empty();
                    $('#txtBecaMonto').val(0);
                    if (tblBecas !== null) {
                        tblBecas.fnClearTable(); $('#slcPeriodo').empty(); var optionP = $(document.createElement('option'));
                        optionP.text('--Seleccionar--');
                        optionP.val('-1');
                        $("#slcPeriodo").append(optionP);
                    }
                    AlumnoObject = null;
                    alertify.alert('Beca Aplicada');
                    BuscarAlumno(Alumnoid);
                } else {
                    alertify.alert('Error al cargar datos');
                    $('#Load').modal('hide');
                }
            }
        }
    }
});