$(document).ready(function () {
    var tblEstadoDeCuenta;
    var tblEstadoDeCuentaT;
    var AlumnoId;
    var lstEstadoCuenta = [];
    var Totales;

    starDate();
    function starDate() {
        //console.log("Estoy dentro...");
        var formon = moment();
        formon.locale('es');
        formon.format('l');
        //        moment.locale('es');
        var fnow = new Date();
        var mes = fnow.getMonth() + 1;
        var fnowS = (fnow.getDate() > 9 ? fnow.getDate().toString() : "0" + fnow.getDate().toString()) + "/" +
                (mes > 9 ? mes.toString() : "0" + mes.toString()) + "/" + fnow.getFullYear().toString();

        fnow.setMonth(fnow.getMonth() - 2);
        mes = fnow.getMonth() + 1;

        var fAntS = '01/01/2010';
            //(fnow.getDate() > 9 ? fnow.getDate().toString() : "0" + fnow.getDate().toString()) + "/" +
              //  (mes > 9 ? mes.toString() : "0" + mes.toString()) + "/" + fnow.getFullYear().toString();
        $('#txtPeriodo').val(fAntS + ' - ' + fnowS);
        $('#txtPeriodo .ui-datepicker-calendar').css("display","none");
        $('#txtPeriodo').daterangepicker({
            language: 'es',
            autoApply: true,
            showDropdowns: true,
            alwaysShowCalendars: false,
            linkedCalendars: true,
            locale: {
                format: "DD/MM/YYYY",
                separator: " - ",
                applyLabel: "Aplicar",
                cancelLabel: "Cancelar",
                fromLabel: "del",
                toLabel: "al",
                "customRangeLabel": "Custom",
                daysOfWeek: [
                    "Do",
                    "Lu",
                    "Ma",
                    "Mi",
                    "Ju",
                    "Vi",
                    "Sa"
                ],
                monthNames: [
                    "Enero",
                    "Febrero",
                    "Marzo",
                    "Abril",
                    "Mayo",
                    "Junio",
                    "Julio",
                    "Agosto",
                    "Septiembre",
                    "Octubre",
                    "Noviembre",
                    "Diciembre"
                ],
                "firstDay": 1
            },
            startDate: fAntS,
            endDate: fnowS,
            disableEntry: true
        });

        var inps = $('.input-mini ');
        $(inps).each(function (k, a) {
            $(a).removeClass('input-mini').addClass('input');
            var icon = $(a).parent().find('i');
            $(icon).each(function (i, m) {
                $(m).hide();
            });
        });
    }

    $('#btnBuscar').click(function () {
        var lbl = $('#lblNombre');
        lbl[0].innerHTML = "";
        AlumnoId = $('#txtClave').val();
        if (AlumnoId.length == 0) {
            alertify.alert("No hay nada escrito.");
            return false;
        }
        if (tblEstadoDeCuenta != undefined) {
            $('#tblEstado').empty();
        }
        $('#Load').modal('show');
        BuscarAlumno(AlumnoId);
    });
    function BuscarAlumno(idAlumno) {
        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/ConsultarAlumno",
            data: "{AlumnoId:'" + idAlumno + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    alertify.alert("Error");
                    return false;
                }
                var lbl = $('#lblNombre');
                lbl[0].innerHTML = data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno;
                lbl[0].innerHTML += data.d.AlumnoInscrito.EsEmpresa == true ? " - Grupo Especial" : "";

                var Rango = $('#txtPeriodo').val();
                
                EstadoDeCuenta(data.d.AlumnoId,
                    Rango.substring(0, 10), Rango.substring(13, 23));
            }
        });
    }

    function EstadoDeCuenta(Alumnoid, FechaI, FechaF) {
        //console.log(Alumnoid);
        //console.log(FechaI._i);
        //console.log(FechaF._i);
        $.ajax({
            type: "POST",
            url: "/../WebServices/WS/Alumno.asmx/EstadoDeCuenta",
            data: "{AlumnoId:'" + Alumnoid + "',FechaI:'" + FechaI + "',FechaF:'" + FechaF + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (data) {
                if (data.d === null) {
                    $('#Load').modal('hide');
                    alertify.alert("Error");
                    return false;
                }
                CrearLista(data.d);
                $('#Load').modal('hide');
            }
        });
    }

    function CrearLista(datos) {
        lstEstadoCuenta = null;
        lstEstadoCuenta = [];
        Totales = null;

        var indiceFind = datos.findIndex(function (objfin) {
            return objfin.referenciaProcesadaId == 0;
        });
        Totales = datos[indiceFind];
        datos.splice(indiceFind, 1);

        $(datos).each(function (indice, objl) {
            //Objeto de lista 'Cabecero'
            var objEdoCuenta = {
                abono: "",
                anio: "",
                cargo: "",
                concepto: "",
                conceptodescriptivo: "",
                fechaPago: "",
                mesId: "",
                pagoId: "",
                reciboId: "",
                referenciaId: "",
                referenciaProcesadaId: "",
                restante: "",
                nPago: ""
            };

            objEdoCuenta.abono = objl.abono.length > 0 ? String(objl.abono) : "";
            objEdoCuenta.cargo = objl.cargo.length > 0 ? String(objl.cargo) : "";
            objEdoCuenta.concepto = String(objl.concepto);
            objEdoCuenta.fechaPago = String(objl.fechaPago);
            objEdoCuenta.referenciaId = objl.referenciaId.length > 0 ? String(objl.referenciaId) : "";
            objEdoCuenta.referenciaProcesadaId = objl.referenciaProcesadaId.length > 0 ? String(objl.referenciaProcesadaId) : "";
            objEdoCuenta.restante = objl.restante.length > 0 ? String(objl.restante) : "";
            objEdoCuenta.nPago = objl.referenciaProcesadaId;

            lstEstadoCuenta.push(objEdoCuenta);
            $(objl.Pagos).each(function (ind2, ObjP) {
                //Objeto de lista 'Detalle'
                var objPago = {
                    abono: "",
                    anio: "",
                    cargo: "",
                    concepto: "",
                    conceptodescriptivo: "",
                    fechaPago: "",
                    mesId: "",
                    pagoId: "",
                    reciboId: "",
                    referenciaId: "",
                    referenciaProcesadaId: "",
                    restante: "",
                    nPago: ""
                };
                objPago.abono = ObjP.abono.length > 0 ? String(ObjP.abono) : "";
                objPago.anio = ObjP.anio.length > 0 ? String(ObjP.anio) : "";
                objPago.cargo = ObjP.cargo.length > 0 ? String(ObjP.cargo) : "";
                objPago.concepto = String(ObjP.concepto);
                objPago.conceptodescriptivo = ObjP.conceptodescriptivo > 0 ? String(ObjP.conceptodescriptivo) : "";
                objPago.fechaPago = String(ObjP.fechaPago);
                objPago.mesId = ObjP.mesId.length > 0 ? String(ObjP.mesId) : "";
                objPago.pagoId = ObjP.pagoId.length > 0 ? String(ObjP.pagoId) : "";
                objPago.reciboId = ObjP.reciboId.length > 0 ? String(ObjP.reciboId) : "";
                objPago.referenciaId = ObjP.referenciaId.length > 0 ? String(ObjP.referenciaId) : "";
                objPago.referenciaProcesadaId = ObjP.referenciaProcesadaId.length > 0 ? String(ObjP.referenciaProcesadaId) : "";
                objPago.restante = ObjP.restante.length > 0 ? String(ObjP.restante) : "";
                objPago.nPago = objl.referenciaProcesadaId;
                lstEstadoCuenta.push(objPago);
            });
        });

        //console.log(lstEstadoCuenta);
        CrearTabla(lstEstadoCuenta);
    }

    function CrearTabla(ListaObjetos) {
        var ban = 0;
        Totales.fechaPago = "01/01/";
        Totales.referenciaId = "000";
        Totales.cargo = "000";
        Totales.abono = "000";
        tblEstadoDeCuenta = $('#tblEstado').dataTable({
            "aaData": ListaObjetos,
            "aoColumns": [
                {
                    "mDataProp": "fechaPago",
                    "mRender": function (columna, Data,d) {
                        
                        var fa = "";
                        var abon = '"' + d.abono + '"';
                        var currency = abon;
                        var number = Number(currency.replace(/[^0-9\.]+/g, ""));
                        //console.log(abon);
                        if (number==0) {
                            fa = "<span class='fa fa-file-text-o '> " + columna + "</span>";
                        }
                        else {                            
                            fa = "<i class='fa fa-plus-circle' > " + columna + "</span>";

                        }
                        return fa;
                    }
                },
                { "mDataProp": "concepto" },
                { "mDataProp": "referenciaId" },
                { "mDataProp": "cargo" },
                { "mDataProp": "abono" },
                { "mDataProp": "restante" }
            ],
            "lengthMenu": [[20, 60, 100, -1], [20, 60, 100, 'Todos']],
            "searching": false,
            "ordering": false,
            "async": true,
            "bDestroy": true,
            "bPaginate": true,
            "bLengthChange": true,
            "bFilter": false,
            "bInfo": false,
            "pageLength": 20,
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
            "createdRow": function (row, data, dataIndex) {
                row.childNodes[2].style.textAlign = 'right';
                row.childNodes[3].style.textAlign = 'right';
                row.childNodes[4].style.textAlign = 'right';
                row.childNodes[5].style.textAlign = 'right';
                
                //console.log(data.nPago);
                
                if (dataIndex === 0) {
                    ban = data.nPago;
                    row.style.borderTopStyle = 'Solid';
                    row.style.borderTopColor = 'black';
                    row.style.backgroundColor = "#5cb85c";
                    row.style.color = "#fff";
                }
                if (data.nPago != ban) {
                    ban = data.nPago;
                    row.style.borderTopStyle = 'Solid';
                    row.style.borderTopColor = '#3598dc';
                    row.style.backgroundColor = "#5cb85c";
                    row.style.color = "#fff";
                }
            },
            "fnFooterCallback": function (tfoot, data, start, end, display) {
                
                tfoot.style.backgroundColor = "#3598dc";
                tfoot.style.color = "white";
                var nCells = tfoot.getElementsByTagName('th');
                nCells[1].innerHTML = Totales.concepto;
                nCells[5].innerHTML = Totales.restante;
            }
        });      
    }
});