
$(document).ready(function () {
    var AlumnoObject;
    var tblBecas = null;
    var Alumnoid;

    $('#btnGenerarCargos').click(function () {
        var txtMonto = $('#txtBecaMonto').val();
        var Perid = $('#slcPeriodo').val();

        if (Perid === -1) {
            alertify.alert('Periodo Invalido');
            return true;
        }
        if (txtMonto > 0) {

            var nombre = $('#hCarga');
            nombre[0].innerText = "Guardando";
            $('#Load').modal('show');

            var an = $('#slcPeriodo').find(':selected').data("anio");
            var per = $('#slcPeriodo').find(':selected').data("periodoid");
            var Datos = "{";
            Datos += "AlumnoId:'" + Alumnoid + "',";
            Datos += "OfertaEducativaId:'" + $('#slcOfertas').val() + "',";
            Datos += "Anio:'" + an + "',";
            Datos += "PeriodoId:'" + per + "',";
            Datos += "Porcentaje:'" + txtMonto + "',";
            Datos += "UsuarioId:'" + $.cookie('userAdmin') + "'";
            Datos += "}";
            BecaDeportiva(Datos);
        }
        else {
            alertify.alert('El porcentaje de la Beca no puede ir en 0%');
            return false;
        }
    });

    function BecaDeportiva(Cadena) {
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Beca.asmx/BecaDeportiva",
            data: Cadena,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.d === 'Guardado') {
                    var obj = JSON.parse(JSON.stringify(eval('(' + Cadena + ')')));
                    GuardarDocumentos(obj.AlumnoId, obj.OfertaEducativaId, obj.Anio, obj.PeriodoId, false, obj.UsuarioId);
                } else {
                    alertify.alert('Error al cargar datos');
                    $('#Load').modal('hide');
                }
            }
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
        if (alumno.length > 0 && alumno > 0) {
            $('#Load').modal('show');
            $('#txtBecaMonto').val(0);
            if (tblBecas !== null) {
                tblBecas.fnClearTable();
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

        $.ajax({
            type: "POST",
            url: "../WebServices/WS/Beca.asmx/ObtenerAlumnoDeportiva",
            data: "{AlumnoId:" + AlumnoId + "}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var datos = data.d;
                AlumnoObject = datos;
                Alumnoid = AlumnoObject.AlumnoId;
                $('#lblNombre').text(datos.Nombre);

                if (datos.OfertasAlumnos.length > 1) {
                    $(datos.OfertasAlumnos).each(function (s, d) {
                        var option = $(document.createElement('option'));
                        option.text(d.Descripcion);
                        option.val(d.OfertaEducativaId);
                        $("#slcOfertas").append(option);
                    });

                }
                else if (datos.OfertasAlumnos.length === 1) {
                    var option = $(document.createElement('option'));
                    option.text(datos.OfertasAlumnos[0].Descripcion);
                    option.val(datos.OfertasAlumnos[0].OfertaEducativaId);
                    $("#slcOfertas").append(option);
                    $("#slcOfertas").val(datos.OfertasAlumnos[0].OfertaEducativaId);
                    $("#slcOfertas").change();
                } else {
                    $('#Load').modal('hide');
                    return false;
                }
                $('#Load').modal('hide');
            }
        });
    }

    $("#slcOfertas").change(function () {
        $("#btnGenerarCargos").attr("disabled", "disabled");
        var Oferta = $("#slcOfertas").val();

        if (Oferta === '-1') { return false; }

        $(AlumnoObject.PeriodosAlumno).each(function (s, d) {


            $("#slcPeriodo").empty();
            var option = $(document.createElement('option'));
            option.text(d.Descripcion);
            option.attr("data-Anio", this.Anio);
            option.attr("data-PeriodoId", this.PeriodoId);
            option.val(d.Anio + " " + d.PeriodoId);
            $("#slcPeriodo").append(option);
            $("#slcPeriodo").val(d.Anio + " " + d.PeriodoId);
            
        });

        $('#slcPeriodo').val(AlumnoObject.PeriodosAlumno[0].Anio +" "+ AlumnoObject.PeriodosAlumno[0].PeriodoId);
        $('#slcPeriodo').change();

        var desc = [];
        $(AlumnoObject.lstDescuentos).each(function () {
            var da = this;
            if (da.OfertaEducativaId.toString() === Oferta.toString()) {
                desc.push(da);
            }
            else {
                tblBecas.fnClearTable();
            }
        });
        //console.log(desc);
        if (desc.length > 0) { CargarDescuentos(desc); }
    });

    $('#slcPeriodo').change(function () {
        var Perid = $('#slcPeriodo').val();
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
                $('#txtBecaMonto').val(data.MontoDep);
                if (data.DocComiteRutaId != null && data.DocComiteRutaId > 0) {
                    var newLinkS = $(document.createElement("a"));
                    newLinkS[0].innerText = "Archivo...";
                    newLinkS.attr('data-file', data.DocComiteRutaId)

                    var colDeportiva = row.childNodes[2];
                    colDeportiva.innerHTML += "<br />" + newLinkS[0].outerHTML;
                }
            }
        });
    }

    $('#DeportivaArchivo').bind('change', function () {
        var file = $('#FileDeportiva');
        var tex = $('#TxtDeportivaArchivo').html();
        if (this.files.length > 0) {
            $('#TxtDeportivaArchivo').text(RecortarNombre(this.files[0].name));
            file.addClass('fileinput-exists').removeClass('fileinput-new');
            $('#FileDeportiva span span').text('Cambiar');
        }
        else {
            $('#TxtDeportivaArchivo').text('');
            file.removeClass('fileinput-exists').addClass('fileinput-new');
            $('#FileDeportiva span span').text('Seleccionar Archivo...');
        }
    });

    $('#FileDeportiva a').click(function () {
        var file = $('#FileDeportiva');
        $('#TxtDeportivaArchivo').text('');
        file.removeClass('fileinput-exists').addClass('fileinput-new');
        File[0] = null;
        $('#FileDeportiva span span').text('Seleccionar Archivo...');
    });

    $("#tblBecas").on('click', 'a', function () {
        var a = this;
        a = $(a).data("file");
        AbrirArchivo(a);
    });

    function AbrirArchivo(DocumentoId) {
        var url = "/../WebServices/WS/Beca.asmx/GenerarPDF2?DocumentoId=" + DocumentoId;
        var archiv = window;
        archiv.open("../Inscritos/Archivos/Archivo.html", "PDF");
        archiv.Ruta = url;
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

        filBeca = $('#DeportivaArchivo');
        filBeca = filBeca[0].files[0];
        data.append("DocumentoDeportiva", filBeca);

        data.append("AlumnoId", Alumnoid);
        data.append("OfertaEducativaId", OfertaEducativaId);
        data.append("Anio", Anio);
        data.append("Periodo", Periodo);
        data.append("EsComite", EsComite);
        data.append("UsuarioId", Usuario);

        var request = new XMLHttpRequest();
        request.open("POST", '../WebServices/WS/Beca.asmx/GuardarDocumentos', true);
        request.send(data);

        request.onreadystatechange = function () {
            if (request.readyState === 4) {
                if (request.readyState === XMLHttpRequest.DONE && request.status === 200) {
                    $('#Load').modal('show');
                    $('#slcOfertas').empty();
                    $('#txtBecaMonto').val(0);
                    if (tblBecas !== null) {
                        tblBecas.fnClearTable(); 
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