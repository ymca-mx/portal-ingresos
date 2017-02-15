$(document).ready(function () {
    Cargar();
    //$.cookie('userAdmin', 6070, { expires: 1 });
    var AlumnoId;
    var MItable;
    var Usuario;
    $('#tbAlumnos').on('click', 'a', function () {
        var rEstatus = this.parentNode.parentNode;
        rEstatus = MItable.fnGetData(rEstatus, 3);
        if (rEstatus == "Ya Capturado") {
            alertify.alert("Información ya capturada", function () {
                return false;
            });
            return false;
        }
        Usuario = $.cookie('userAdmin');
        if (Usuario == 6070) {
            $('#Inscripcion').hide();
            $('#Financiamiento').hide();
            $('#BDeportiva').hide();
        } else if (Usuario == 6645) {
            $('#Beca').hide();
            $('#BDeportiva').hide();
            $('#SEP').hide();
        }
        else if (Usuario == 8138) {
            $('#SEP').hide();
            $('#Beca').hide();
            $('#Inscripcion').hide();
            $('#Financiamiento').hide();
        }
        else {
            $('#Inscripcion').hide();
            $('#Financiamiento').hide();
            $('#BDeportiva').hide();
        }
        $('#txtBeca').val('');
        $('#txtInscripcion').val('');
        $('#txtFinanciamiento').val('');
        $('#chkSEP').prop("checked", false);
        var spam = $('#chkSEP');
        spam = spam[0].parentNode;
        spam.className='';
        $('#txtBecaDeportiva').val('');
        $('#tblHistorico tbody').remove();
        var rFila = this.parentNode.parentNode;
        rFila = MItable.fnGetData(rFila, 0);
        AlumnoId = rFila;
        OfertasAlumno(rFila);
    });
    function OfertasAlumno(alumnoid) {
        $("#slcOferta").empty();
        var option = $(document.createElement('option'));

        option.text("--Seleccionar--");
        option.val(-1);
        $("#slcOferta").append(option);
        $.ajax({
            type: "POST",
            url: "../WebServices/WS/General.asmx/ConsultarOfertaEducativaAlumno",
            data: "{AlumnoId:'" + alumnoid + "'}", // the data in form-encoded format, ie as it would appear on a querystring
            //contentType: "application/x-www-form-urlencoded; charset=UTF-8", // if you are using form encoding, this is default so you don't need to supply it
            contentType: "application/json; charset=utf-8", // the data type we want back, so text.  The data will come wrapped in xml
            success: function (data) {
                var datos = data.d;
                $(datos).each(function () {
                    var option = $(document.createElement('option'));

                    option.text(this.Descripcion);
                    option.val(this.OfertaEducativaId);

                    $("#slcOferta").append(option);
                });
            }
        });
    }
    $("#slcOferta").change(function () {
        var opt = $("#slcOferta").val();
        if (opt == -1) {
            $('#txtBeca').val('');
            $('#txtInscripcion').val('');
            $('#txtFinanciamiento').val('');
            $('#chkSEP').prop("checked", false);
            var spam = $('#chkSEP');
            spam = spam[0].parentNode;
            spam.className = '';
            $('#txtBecaDeportiva').val('');
            $('#tblHistorico tbody').remove();
        } else {
            ConsultarAlumno(AlumnoId, opt);
        }
    });
    function ConsultarAlumno(alumnoid, Ofertaeducativaid)
    {
        var data1;
        AlumnoId = alumnoid;
        $('#tblHistorico tbody').remove();
        $.ajax({
            url: '../WebServices/WS/Descuentos.asmx/ConsultarDescuentos',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{AlumnoId:' + alumnoid + ',OfertaEducativaId:' + Ofertaeducativaid + '}',
            dataType: 'json',
            success: function (Respuesta) {
                
                $.each(Respuesta.d, function (index, obj) {
                    if (obj.Descuento.Descripcion == 'Beca Académica' && Usuario==6070) {
                        $('#txtBeca').val(obj.Monto);
                        data1 = $('#txtBeca').data();
                        data1.DescuentoId = obj.DescuentoId == 'undefined' ? 'null' : obj.DescuentoId;
                        var row = "<tr><td>" + obj.Anio + "</td><td>" + obj.PeriodoId + "</td><td>" + obj.Descuento.Descripcion + "</td><td>" + obj.Monto + "</td></tr>";
                        $('#tblHistorico').append(row);
                        
                    }
                    else if (obj.Descuento.Descripcion == 'Descuento en inscripción' && Usuario == 6645) {
                        $('#txtInscripcion').val(obj.Monto);
                        data1 = $('#txtInscripcion').data();
                        data1.DescuentoId = obj.DescuentoId == 'undefined' ? 'null' : obj.DescuentoId;
                        var row = "<tr><td>" + obj.Anio + "</td><td>" + obj.PeriodoId + "</td><td>" + obj.Descuento.Descripcion + "</td><td>" + obj.Monto + "</td></tr>";
                        $('#tblHistorico').append(row);
                    }
                    else if (obj.Descuento.Descripcion == 'Financiamiento' && Usuario == 6645) {
                        $('#txtFinanciamiento').val(obj.Monto);
                        data1 = $('#txtFinanciamiento').data();
                        data1.DescuentoId = obj.DescuentoId == 'undefined' ? 'null' : obj.DescuentoId;
                        var row = "<tr><td>" + obj.Anio + "</td><td>" + obj.PeriodoId + "</td><td>" + obj.Descuento.Descripcion + "</td><td>" + obj.Monto + "</td></tr>";
                        $('#tblHistorico').append(row);
                    }
                    else if (obj.Descuento.Descripcion == 'Beca SEP' && Usuario == 6070) {
                        $('#chkSEP').prop('checked', true);
                        var spam = $('#chkSEP');
                        $('#txtBeca').val(obj.Monto);
                        spam = spam[0].parentNode;
                        spam.className = 'checked';
                        data1 = $('#chkSEP').data();
                        data1.DescuentoId = obj.DescuentoId == 'undefined' ? 'null' : obj.DescuentoId;
                        var row = "<tr><td>" + obj.Anio + "</td><td>" + obj.PeriodoId + "</td><td>" + obj.Descuento.Descripcion + "</td><td>" + obj.Monto + "</td></tr>";
                        $('#tblHistorico').append(row);
                    }
                    else if (obj.Descuento.Descripcion == 'Beca deportiva' && Usuario == 6138) {
                        $('#txtBecaDeportiva').val(obj.Monto);
                        data1 = $('#txtBecaDeportiva').data();
                        data1.DescuentoId = obj.DescuentoId == 'undefined' ? 'null' : obj.DescuentoId;
                        var row = "<tr><td>" + obj.Anio + "</td><td>" + obj.PeriodoId + "</td><td>" + obj.Descuento.Descripcion + "</td><td>" + obj.Monto + "</td></tr>";
                        $('#tblHistorico').append(row);
                    }
                });
            }
        });
    }
    $('#btnguardar').on('click', function () {
        var Daos;
        if (Usuario == 6070) {
            var des, dsep;
            var SEP = $('#chkSEP').attr("checked") ? 'true' : 'false';
            des = $('#txtBeca').data().DescuentoId == undefined ? 'null' : $('#txtBeca').data().DescuentoId;
            dsep = $('#chkSEP').data().DescuentoId == undefined ? 'null' : $('#chkSEP').data().DescuentoId;
            Daos = '{AlumnoId:' + AlumnoId + ',UsuarioId:' + Usuario + ',Beca:' + $('#txtBeca').val() + ',SEP:' + SEP;
            Daos += ',DescuentoBeca:' + des + ',DescuentoSEP:' + dsep + ',OfertaEducativaId:' + $("#slcOferta").val(); +'';
            Daos += '}';
            $.ajax({
                url: '../WebServices/WS/Descuentos.asmx/GuardarDescuento',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: Daos, 
                dataType: 'json',
                success: function (Respuesta) {
                    alertify.alert('Procesando');
                }
            });
        } else if (Usuario == 6645) {
            var IdInsc = $('#txtInscripcion').data().DescuentoId == undefined ? 'null' : $('#txtInscripcion').data().DescuentoId;
            var IdFinan = $('#txtFinanciamiento').data().DescuentoId == undefined ? 'null' : $('#txtFinanciamiento').data().DescuentoId;

            Daos = '{AlumnoId:' + AlumnoId + ',UsuarioId:' + Usuario + ',Inscripcion:' + $('#txtInscripcion').val() + ',Financiamiento:' + $('#txtFinanciamiento').val();
            Daos += ',DescuentoInsc:' + IdInsc + ',DescuentoFinan:' + IdFinan;
            Daos += '}';
            $.ajax({
                url: '../WebServices/WS/Descuentos.asmx/GuardarDescuento2',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: Daos,
                dataType: 'json',
                success: function (Respuesta) {
                    alertify.alert('Procesando');
                }
            });
        } else if (Usuario == 8138) {
        }
    });
    function Cargar() {
        Usuario = $.cookie('userAdmin');
        var Ingles = Usuario == 6070 || 6645 || 8138 ? false : true;
        $.ajax({
            url: '../WebServices/WS/Alumno.asmx/ConsultarAlumnosBeca',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: '{Ingles:' + Ingles + '}',
            dataType: 'json',
            success: function (Respuesta) {
                MItable = $('#tbAlumnos').dataTable({
                    "aaData": Respuesta.d,
                    "aoColumns": [
                        {
                            "mDataProp": "AlumnoId", "Nombre": "AlumnoId",
                            "mRender": function (data) {
                                return "<a href='#Descuentos' data-toggle='modal'onclick='return false;'>" + data + " </a> ";
                            }
                        },
                        { "mDataProp": "Nombre", },
                        { "mDataProp": "AlumnoInscrito2.Descripcion" },
                        { "mDataProp": "EstatusDescuento" },
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
                var fil = $('#tbAlumnos_filter label input');
                fil.removeClass('input-small').addClass('input-xlarge');
            },
            error: function (Respuesta) {
                alertify.alert('Error al cargar datos');
            }
        });
    }
});