﻿$(function init() {
    var AlumnoId;
    var PeriodoId;
    var Anio;
    var tblReferencias;
    var PeriodoAlcorriente = null;
    var Periodo = null;
    var Tipo;
   
    var Funciones = {
        TablaMaster: {},
        BuscarAlumno: function (idAlumno) {
            $.ajax({
                type: "POST",
                url: "WS/Alumno.asmx/ConsultarAlumno",
                data: "{AlumnoId:'" + idAlumno + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    var lbl = $('#lblNombre');
                    lbl[0].innerHTML = data.d.Nombre + " " + data.d.Paterno + " " + data.d.Materno;
                    lbl[0].innerHTML += data.d.AlumnoInscrito.EsEmpresa == true ? (data.d.AlumnoInscrito.EsEspecial == true ? " - Alumno Especial  " : " - Grupo  Empresarial") + " - " + data.d.Grupo.Descripcion : "";
                    Funciones.CargarPagos();                    
                }
            });
        },
        BuscarNombre: function (Nombre) {
            $.ajax({
                url: 'WS/Alumno.asmx/BuscarAlumnoString',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{Filtro:"' + Nombre + '"}',
                dataType: 'json',
                success: function (data) {
                    if (data != null) {
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
                    $('#Load').modal('hide');

                }
            });
        },
        PintarTabla: function (dat1a) {
            var data = dat1a.d.Pagos;
            var dk = dat1a.d.Estatus;
            var Especial = dat1a.d.Pagos[0].EsEspecial;
            if (data[0].esEmpresa) {
                $('#tblReferencias2').hide();
                $('#tblReferencias').hide();
                $('#tblReferencias3').show();
                tblReferencias = $('#tblReferencias3').dataTable({
                    "aaData": data,
                    "bSort": false,
                    "aoColumns": [
                        { "mDataProp": "Concepto" },
                        { "mDataProp": "ReferenciaId" },
                        { "mDataProp": "CargoFechaLimite" },
                        { "mDataProp": "TotalMDescuentoMBecas" },
                        { "mDataProp": "OtroDescuento" },
                        { "mDataProp": "SaldoPagado" }
                    ],
                    "columnDefs": [
                        {
                            "targets": [4],
                            "visible": dk,
                            "searchable": false
                        },
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
                        "search": "Buscar Alumno "
                    },
                    "createdRow": function (row, data, dataIndex) {
                        row.childNodes[0].style.textAlign = 'left';
                        row.childNodes[1].style.textAlign = 'center';
                        row.childNodes[2].style.textAlign = 'center';
                        row.childNodes[3].style.textAlign = 'right';
                        row.childNodes[4].style.textAlign = 'right';
                        if (dk) {
                            row.childNodes[4].style.textAlign = 'right';
                        }
                        if (data.Pagoid == 0) {
                            row.childNodes[0].style.fontWeight = 'bold';
                            row.childNodes[0].style.fontSize = '12px';
                        }
                        if (data.Adeudo == true) {
                            row.style.color = "#FFFFFF";
                            row.style.backgroundColor = '#e35b5a';
                        }
                        if (data.Titulo === true) {
                            row.style.color = "#FFFFFF";
                            row.style.backgroundColor = '#217ebd';
                        }
                    }
                });

                var tr
                if (dk) {
                    tr = '<tr>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th style="text-align:right">' + data[0].TotalPagado + '</th></tr>';
                } else {
                    tr = '<tr>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th></th>' +
                        '<th style="text-align:right">' + data[0].TotalPagado + '</th></tr>';
                }
                //var tabla = document.getElementById("tblReferencias3");
                document.getElementById("tblReferencias3").insertRow(-1).innerHTML = tr;

            } else {
                if (data[0].BecaSEP != null) {

                    $('#tblReferencias2').hide();
                    $('#tblReferencias3').hide();
                    $('#tblReferencias').show();

                    tblReferencias = $('#tblReferencias').dataTable({
                        "aaData": data,
                        "bSort": false,
                        "aoColumns": [
                            { "mDataProp": "Concepto" },
                            { "mDataProp": "ReferenciaId" },
                            { "mDataProp": "CargoMonto" },
                            { "mDataProp": "CargoFechaLimite", },
                            { "mDataProp": "DescuentoXAnticipado" },
                            { "mDataProp": "Cargo_Descuento" },
                            { "mDataProp": "BecaAcademica_Pcj" },
                            { "mDataProp": "BecaAcademica_Monto" },
                            { "mDataProp": "BecaOpcional_Pcj" },
                            { "mDataProp": "BecaOpcional_Monto" },
                            { "mDataProp": "OtroDescuento" },
                            { "mDataProp": "TotalMDescuentoMBecas" },
                            { "mDataProp": "SaldoPagado" },
                        ],
                        "columnDefs": [
                            {
                                "targets": [10],
                                "visible": dk,
                                "searchable": false
                            },],
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
                            "search": "Buscar Alumno "
                        },
                        "createdRow": function (row, data, dataIndex) {
                            try {
                                row.childNodes[0].style.textAlign = 'left';
                                row.childNodes[1].style.textAlign = 'center';
                                row.childNodes[2].style.textAlign = 'right';
                                row.childNodes[3].style.textAlign = 'center';
                                row.childNodes[4].style.textAlign = 'right';
                                row.childNodes[5].style.textAlign = 'right';
                                row.childNodes[6].style.textAlign = 'center';
                                row.childNodes[7].style.textAlign = 'right';
                                row.childNodes[8].style.textAlign = 'right';
                                row.childNodes[9].style.textAlign = 'right';
                                if (dk) {
                                    var child = row.childNodes[10].style.textAlign = 'right';
                                }
                                if (data.Pagoid == 0) {
                                    row.childNodes[0].style.fontWeight = 'bold';
                                    row.childNodes[0].style.fontSize = '12px';
                                } if (data.Adeudo == true) {
                                    row.style.color = "#FFFFFF";
                                    row.style.backgroundColor = '#e35b5a';
                                }
                                if (data.Titulo === true) {
                                    row.style.color = "#FFFFFF";
                                    row.style.backgroundColor = '#217ebd';
                                }
                            } catch (err) {
                                console.log(err.message);
                            }
                        }
                    });

                    var tr;
                    if (dk) { tr = '<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th style="text-align:right">' + data[0].TotalPagado + '</th></tr>'; }
                    else { tr = '<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th style="text-align:right">' + data[0].TotalPagado + '</th></tr>'; }

                    document.getElementById("tblReferencias").insertRow(-1).innerHTML = tr;
                    var th = $('#thBeca');
                    th[0].innerHTML = data[0].BecaSEP;
                    var titulo = $('#tblReferencias');
                    titulo = titulo[0];
                    var DescripcipT = data[0].EsSep == 1 ? "BECA SEP" : data[0].EsSep == 2 ? "BECA Académica" : data[0].EsSep == 3 ? "BECA Comite" : 0;
                    titulo.childNodes[1].childNodes[1].childNodes[3].textContent = DescripcipT;
                    //$('#thBeca').innerHTML = data.d[0].BecaSEP;

                } else if (data[0].BecaSEP == null) {
                    $('#tblReferencias').hide();
                    $('#tblReferencias3').hide();
                    $('#tblReferencias2').show();
                    tblReferencias = $('#tblReferencias2').dataTable({
                        "aaData": data,
                        "bSort": false,
                        "aoColumns": [
                            { "mDataProp": "Concepto" },
                            { "mDataProp": "ReferenciaId" },
                            { "mDataProp": "CargoMonto" },
                            { "mDataProp": "CargoFechaLimite", },
                            { "mDataProp": "DescuentoXAnticipado" },
                            { "mDataProp": "Cargo_Descuento" },
                            { "mDataProp": "BecaAcademica_Pcj" },
                            { "mDataProp": "BecaAcademica_Monto" },
                            { "mDataProp": "OtroDescuento" },
                            { "mDataProp": "TotalMDescuentoMBecas" },
                            { "mDataProp": "SaldoPagado" },
                        ],
                        "columnDefs": [
                            {
                                "targets": [8],
                                "visible": dk,
                                "searchable": false
                            },],
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
                            "search": "Buscar Alumno "
                        },
                        "createdRow": function (row, data, dataIndex) {
                            try {
                                row.childNodes[0].style.textAlign = 'left';
                                row.childNodes[1].style.textAlign = 'center';
                                row.childNodes[2].style.textAlign = 'right';
                                row.childNodes[3].style.textAlign = 'center';
                                row.childNodes[4].style.textAlign = 'right';
                                row.childNodes[5].style.textAlign = 'right';
                                row.childNodes[6].style.textAlign = 'center';
                                row.childNodes[7].style.textAlign = 'right';
                                row.childNodes[8].style.textAlign = 'right';
                                row.childNodes[9].style.textAlign = 'right';
                                if (dk) {
                                    var child = row.childNodes[10].style.textAlign = 'right';
                                }
                                if (data.Pagoid == 0) {
                                    row.childNodes[0].style.fontWeight = 'bold';
                                    row.childNodes[0].style.fontSize = '12px';
                                }
                                if (data.Adeudo == true) {
                                    row.style.color = "#FFFFFF";
                                    row.style.backgroundColor = '#e35b5a';
                                }
                                if (data.Titulo === true) {
                                    row.style.color = "#FFFFFF";
                                    row.style.backgroundColor = '#217ebd';
                                }
                            } catch (err) {
                                console.log(err.message);
                            }
                        }
                    });

                    var tr;
                    if (dk) {
                        tr = '<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th style="text-align:right">' + data[0].TotalPagado + '</th></tr>';
                    }
                    else { tr = '</th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th style="text-align:right">' + data[0].TotalPagado + '</th></tr>'; }
                    document.getElementById("tblReferencias2").insertRow(-1).innerHTML = tr;

                    var titulo = $('#tblReferencias2');
                    titulo = titulo[0];
                    var DescripcipT1 = data[0].EsSep == 1 ? "BECA SEP" : data[0].EsSep == 2 ? "BECA Académica" : "BECA Comite";
                    titulo.childNodes[1].childNodes[1].childNodes[3].textContent = DescripcipT1;


                }
            }
            if (parseInt($('#sclPeriodo').val()) != -1) {
                Funciones.Anticipado();
            } else { $('#Load').modal('hide');}
            
        },
        CargarPagos: function () {
            $.ajax({
                type: "POST",
                url: "WS/Alumno.asmx/ConsultaPagosDetalle",
                data: "{AlumnoId:'" + AlumnoId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (dat1a) {
                    if (dat1a.d != null) {
                        Funciones.TablaMaster = dat1a;
                        Funciones.CrearCombo();
                    } else { $('#Load').modal('hide');}
                }
            });
        },
        CrearCombo: function () {
            if (Funciones.TablaMaster.d.Periodos.length > 1) {
                var option = $(document.createElement('option'));
                option.text("TODOS");
                option.val(-1);
                option.attr("data-Anio", 0);
                option.attr("data-PeriodoId", 0);

                $('#sclPeriodo').append(option);
            }

            $(Funciones.TablaMaster.d.Periodos).each(function () {
                var option2 = $(document.createElement('option'));
                option2.val(this.Anio + '' + this.PeriodoId);
                option2.text(this.Descripcion);
                option2.attr("data-Anio", this.Anio);
                option2.attr("data-PeriodoId", this.PeriodoId);
                option2.attr("data-total", this.Total);
                $('#sclPeriodo').append(option2);
            });
            if (Funciones.TablaMaster.d.Periodos.length > 1) { $('#sclPeriodo').val(-1); }
            else { $('#sclPeriodo').val(Funciones.TablaMaster.d.Periodos[0].Anio + '' + Funciones.TablaMaster.d.Periodos[0].PeriodoId); }
            Funciones.slcPeriodoChange();
        },
        Anticipado: function () {
            $.ajax({
                type: "POST",
                url: "WS/General.asmx/Ofertas_costos_Alumno",
                data: "{AlumnoId:'" + AlumnoId + "',Anio:'" + Anio + "',PeriodoId:'" + PeriodoId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    $('#Load').modal('hide');
                    var Descripcion = "";
                    if (data.d == null) { return null; }
                    var Genera = 0;
                    $('#divtablas').empty();

                    //Descripcion = " Si vas hacer tu pago anticipado tienes hasta el "+1+" para pagar los siguientes montos con las referencias indicadas:";

                    $(data.d).each(function () {
                        var tabla1 = '<br />' +
                            '<table>' +
                            '<thead>' +
                            '<tr style="position:center">' +
                            '<th>' +
                            'Inscripción' +
                            '</th>' +
                            '<th>&emsp;&emsp;&emsp;</th>' +
                            '<th>' +
                            'Referencia' +
                            '</th>' +
                            '</tr>' +
                            '<tr>' +
                            '<th style="text-align:center" id="thInscripcion">' +
                            this.MontoReins +
                            '</th>' +
                            '<th></th>' +
                            '<th style="text-align:center" id="thRefIns">' +
                            this.ReferenciaInsc +
                            '</th>' +
                            '</tr>' +
                            '</thead>' +
                            '</table>';
                        var tabla2 = '<br />' +
                            '<table>' +
                            '<thead>' +
                            '<tr style="position:center">' +
                            '<th>' +
                            'Colegiatura' +
                            '</th>' +
                            '<th>&emsp;&emsp;&emsp;</th>' +
                            '<th>' +
                            'Referencia' +
                            '</th>' +
                            '</tr>' +
                            '<tr>' +
                            '<th style="text-align:center" id="thInscripcion">' +
                            this.MontoColeg +
                            '</th>' +
                            '<th></th>' +
                            '<th style="text-align:center" id="thRefIns">' +
                            this.ReferenciaColg +
                            '</th>' +
                            '</tr>' +
                            '</thead>' +
                            '</table>';
                        if (this.MontoReins != null) {
                            $('#divtablas').append(tabla1);
                            Genera = 1;
                        }
                        if (this.MontoColeg != null) {
                            $('#divtablas').append(tabla2);
                            Genera = 1;
                        }
                    });

                    ///Se tiene que regresar a valor anterior
                    if (Genera == 1) {
                        $('#divAnticipado').show();
                        //$('#pdescription').text(Descripcion);
                    } else { $('#divAnticipado').hide(); }
                    //AlumnoId = "";
                }
            });
        },
        BtnBuscarClick: function () {
            $('#frmVarios').hide();
            var lbl = $('#lblNombre');
            lbl[0].innerHTML = "";
            AlumnoId = $('#txtClave').val();

            if (AlumnoId.length == 0) { return false; }
            if (tblReferencias != undefined) { tblReferencias.fnClearTable(); }

            $('#Load').modal('show');
            $('#sclPeriodo').empty();

            if (!isNaN(AlumnoId)) { Funciones.BuscarAlumno(AlumnoId); }
            else { Funciones.BuscarNombre(AlumnoId); }
        },
        txtClaveKeydown: function (e) {
            if (e.which == 13) {
                Funciones.BtnBuscarClick();
            }
        },
        TablaAlumnoClick: function () {
            $('#frmVarios').hide();
            $('#Load').modal('show');
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
            AlumnoId = rowadd.AlumnoId;
            Funciones.BuscarAlumno(rowadd.AlumnoId);
        },
        slcPeriodoChange: function () {
            var val = $('#sclPeriodo').val();
            Anio = $('#sclPeriodo').find(':selected').data("anio");
            PeriodoId = $('#sclPeriodo').find(':selected').data("periodoid");
            var Total = $('#sclPeriodo').find(':selected').data("total");

            if (val === "-1") {
                Funciones.PintarTabla(Funciones.TablaMaster);
            } else {
                var objNuevo = {
                    d: {
                        Estatus: Funciones.TablaMaster.d.Estatus,
                        Pagos: Funciones.TraerPagosPeriodo(Anio, PeriodoId, Total)
                    }
                };

                Funciones.PintarTabla(objNuevo);
            }
        },
        TraerPagosPeriodo: function (Anio, PeriodoId, Total) {
            var lstp = [], EsSep, BecaSEP, EsEmpresa;

            $(Funciones.TablaMaster.d.Pagos).each(function () {
                if (this.Anio === Anio && this.PeriodoId === PeriodoId && this.Titulo === false) {
                    lstp.push(this);
                }
            });

            $(Funciones.TablaMaster.d.Periodos).each(function () {
                if (this.Anio === Anio && this.PeriodoId === PeriodoId) {
                    EsSep = this.EsSep;
                    BecaSEP = this.BecaSEP;
                    EsEmpresa = this.EsEmpresa;
                }
            });

            lstp[0].TotalPagado = "$" + parseFloat(Total).toFixed(2).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
            lstp[0].EsSep = EsSep;
            lstp[0].BecaSEP = BecaSEP;
            lstp[0].esEmpresa = EsEmpresa;
            return lstp;
        }
    };

    $('#btnBuscar').on('click', Funciones.BtnBuscarClick);
    $('#txtClave').on('keydown', Funciones.txtClaveKeydown);
    $("#sclPeriodo").on('change', Funciones.slcPeriodoChange);
    $('#tblAlumnos').on('click', 'a', Funciones.TablaAlumnoClick);
});