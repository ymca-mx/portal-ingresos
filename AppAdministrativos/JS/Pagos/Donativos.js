$(document).ready(function ()
{
    var buscar = -1;
    var BuscarTexto, tblReferencia, tblAlumnos, tblAlumnos1, lst = [], lst2 = [], lstAlumnos = [], importe , saldoasignado;
    $("#slcBuscar").val(-1);

    var funciones =
        {
            buscar: function () {
                if (tblReferencia != undefined) { tblReferencia.fnClearTable(); }
                if (tblAlumnos != undefined) { tblAlumnos.fnClearTable(); }
                var dato = $("#txtDato").val();
                lst = [];
                if (buscar != -1) {
                    if (dato != "" && parseInt(dato)>0) {
                        IndexFn.Block(true);
                        $("#divAgregar").hide();
                        importe = 0;
                        saldoasignado = 0;
                        $.ajax({
                            type: "POST",
                            url: "WS/Alumno.asmx/BuscarReferencia",
                            data: "{Dato:'" + dato + "',Buscar:" + buscar + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            success: function (data) {
                                var datos = data.d;

                                if (datos == null) {
                                    $("#lbReferencia").text("");

                                    alertify.alert("El número de " + BuscarTexto + " no existe o no pertenece a un concepto de donativo");
                                }
                                else {
                                    $("#lbReferencia").text(datos.referenciaId);
                                    lst.push(datos);



                                    tblReferencia = $("#tblReferencia").dataTable({
                                        "aaData": lst,
                                        "aoColumns": [
                                            { "mDataProp": "fechaPago" },
                                            { "mDataProp": "importe" }
                                        ],
                                        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, 'Todos']],
                                        "searching": false,
                                        "ordering": false,
                                        "async": false,
                                        "bDestroy": true,
                                        "bPaginate": false,
                                        "bLengthChange": false,
                                        "bFilter": false,
                                        "bInfo": false,
                                        "bAutoWidth": false,
                                        "asStripClasses": null,
                                        "colReorder": false,
                                        "oSearch": { "bSmart": false }

                                    });

                                    importe = lst[0].importe;

                                    $("#divAgregar").show();
                                   
                                }
                                $("#lbSaldo").text("$" + importe)

                                IndexFn.Block(false);
                            }
                        });
                    } else {
                        alertify.alert("Favor de indicar el número  de " + BuscarTexto);
                        IndexFn.Block(false);
                    }
                   

                } else {
                    alertify.alert("Debe seleccionar una opción de búsqueda.");
                    IndexFn.Block(false);
                }
            },
            btnAgregar: function ()
            {
                $("#PopAlumno").modal('show');
                $("#txtClave").val("");
                if (tblAlumnos1 != undefined) { tblAlumnos1.fnClearTable(); }
            },
            buscarAlumno: function (Alumnoid) {
                IndexFn.Block(true);
                $.ajax({
                    type: "POST",
                    url: "WS/Alumno.asmx/ConsultarAlumno",
                    data: "{AlumnoId:'" + Alumnoid + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.d != null) {
                            lst2 = [];
                            lst2.push(data.d);
                            tblAlumnos1 = $("#tblAlumnos1").dataTable({
                                "aaData": lst2,
                                "aoColumns": [
                                    {
                                        "mDataProp": "AlumnoId",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            link = d.AlumnoId + " | " + d.Nombre + " " + d.Paterno + " " + d.Materno;

                                            return link;
                                        }
                                    },
                                    { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                    {
                                        "mDataProp": function (data) {

                                            return "<a class='btn blue' name ='btnAgregar'>Agregar</a>";

                                        }
                                    }
                                ],
                                "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
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
                                "colReorder": true,
                                "language": {
                                    "lengthMenu": "_MENU_ Registro",
                                    "paginate": {
                                        "previos": "<",
                                        "next": ">"
                                    },
                                    "search": "Buscar Alumno ",
                                },
                                "order": [[1, "desc"]],
                                "createdRow": function (row, data, dataIndex) {
                                    row.childNodes[0].style.textAlign = 'center';
                                    row.childNodes[1].style.textAlign = 'center';
                                    row.childNodes[2].style.textAlign = 'center';
                                }
                            });//$('#dtbecas').DataTable
                        }

                    }
                });

                IndexFn.Block(false);


            },
            buscarNombre: function (Nombre) {
                $.ajax({
                    url: 'WS/Alumno.asmx/BuscarAlumnoString',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: '{Filtro:"' + Nombre + '"}',
                    dataType: 'json',
                    success: function (data) {
                        if (data != null) {
                            tblAlumnos1 = $("#tblAlumnos1").dataTable({
                                "aaData": data.d,
                                "aoColumns": [
                                    {
                                        "mDataProp": "AlumnoId",
                                        "mRender": function (data, f, d) {
                                            var link;
                                            link = d.AlumnoId + " | " + d.Nombre;

                                            return link;
                                        }
                                    },
                                    { "mDataProp": "AlumnoInscrito.OfertaEducativa.Descripcion" },
                                    {
                                        "mDataProp": function (data) {

                                            return "<a class='btn blue' name ='btnAgregar'>Agregar</a>";

                                        }
                                    }
                                ],
                                "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
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
                                "colReorder": true,
                                "language": {
                                    "lengthMenu": "_MENU_ Registro",
                                    "paginate": {
                                        "previos": "<",
                                        "next": ">"
                                    },
                                    "search": "Buscar Alumno ",
                                },
                                "order": [[1, "desc"]],
                                "createdRow": function (row, data, dataIndex) {
                                    row.childNodes[0].style.textAlign = 'center';
                                    row.childNodes[1].style.textAlign = 'center';
                                    row.childNodes[2].style.textAlign = 'center';
                                }
                            });//$('#dtbecas').DataTable
                        }

                        IndexFn.Block(false);

                    }
                });
            },
            btnBuscarClick: function () {

                var alumnoId = $('#txtClave').val();

                if (alumnoId.length == 0) { return false; }

                IndexFn.Block(true);

                if (!isNaN(alumnoId)) { funciones.buscarAlumno(alumnoId); }
                else { funciones.buscarNombre(alumnoId); }
            },
            tablaAlumnoClick: function () {

                var rowadd = tblAlumnos1.fnGetData($(this).closest('tr'));
                
                if (lstAlumnos.filter(function (e) { return e.AlumnoId == rowadd.AlumnoId; }).length == 0) {
                    if (rowadd.Paterno == null) {
                        lstAlumnos.push({ AlumnoId: rowadd.AlumnoId, Nombre: rowadd.Nombre, Monto: 0 });
                    } else {
                        lstAlumnos.push({ AlumnoId: rowadd.AlumnoId, Nombre: rowadd.Nombre + " " + rowadd.Paterno + " " + rowadd.Materno, Monto: 0 });
                    }
                }

                tblAlumnos = $("#tblAlumnos").dataTable({
                    "aaData": lstAlumnos,
                    "aoColumns": [
                        {"mDataProp": "AlumnoId"},
                        { "mDataProp": "Nombre" },
                        {
                            "mDataProp": function (data) {

                                return '<input type="number" class="form-control-static font-md" tabindex="1" min="1" onPaste="return false;" value="' + data.Monto + '">';
                            }
                        },
                        {
                            "mDataProp": function (data) {
                                return '<a class="btn red" id="btnQuitar" tabindex="2">X</a>';
                            }
                        }

                    ],
                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
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
                    "colReorder": false,
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "order": [[1, "desc"]]
                });

                $("#PopAlumno").modal('hide');

            },
            tablaMonto: function ()
            {
                var monto = this.value;
                var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));
                alumnoid = rowadd.AlumnoId;
                var saldo = 0 ;
                lstAlumnos.forEach(function (data)
                {
                    if (data.AlumnoId == alumnoid)
                    {
                        data.Monto = monto;
                    }
                    saldo += data.Monto.length>0 ? parseInt(data.Monto): 0;
                });

                saldoasignado = saldo;
                
                $("#lbSaldo").text("$" + (importe - saldoasignado));
            },
            clickQuitar: function ()
            {
                var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));

                lstAlumnos = lstAlumnos.filter(function (e) { return e.AlumnoId !== rowadd.AlumnoId; });

                tblAlumnos = $("#tblAlumnos").dataTable({
                    "aaData": lstAlumnos,
                    "aoColumns": [
                        { "mDataProp": "AlumnoId" },
                        { "mDataProp": "Nombre" },
                        {
                            "mDataProp": function (data) {

                                return '<input type="number" class="form-control-static font-md" tabindex="1" min="1" onPaste="return false;" value="' + data.Monto + '">';
                            }
                        },
                        {
                            "mDataProp": function (data) {
                                return '<a class="btn red" id="btnQuitar" tabindex="2">X</a>';
                            }
                        }

                    ],
                    "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, 'Todos']],
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
                    "colReorder": false,
                    "language": {
                        "lengthMenu": "_MENU_ Registro",
                        "paginate": {
                            "previos": "<",
                            "next": ">"
                        },
                        "search": "Buscar Alumno ",
                    },
                    "order": [[1, "desc"]]
                });
            },
            AsignarDonativos: function ()
            {
                if (lst.length > 0 && lstAlumnos.length > 0) {

                    if (saldoasignado == importe) {
                        IndexFn.Block(true);

                        var obj = {
                            "AlumnoDonativo":
                            {
                                "Alumnos": lstAlumnos,
                                "ReferenciaId": lst[0].referenciaId,
                                "UsuarioId":  localStorage.getItem('userAdmin')
                            }
                        };

                        obj = JSON.stringify(obj);

                        $.ajax({
                            type: "POST",
                            url: "WS/Alumno.asmx/AplicarDonativo",
                            data: obj,
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            success: function (data) {
                                if (data.d != null) {
                                    lst = [];
                                    lstAlumnos = [];
                                    $("#slcBuscar").val(-1);
                                    $("#txtDato").val("");
                                    $("#lbSaldo").text("$0");
                                    $("#divAgregar").hide();
                                    $("#lbReferencia").text("");
                                    $("#lbBuscar").text("--Seleccionar--");
                                    if (tblReferencia != undefined) { tblReferencia.fnClearTable(); }
                                    if (tblAlumnos != undefined) { tblAlumnos.fnClearTable(); }
                                    alertify.alert("El donativo se aplicó correctamente.");
                                } else {
                                    alertify.alert("Ocurrió un problema al aplicar el donativo.");
                                }

                                IndexFn.Block(false);
                            }
                        });
                    } else if (saldoasignado < importe) {
                        alertify.alert("El saldo asignado es menor al importe de la referencia.");
                    } else
                    {
                        alertify.alert("El saldo asignado es mayor al importe de la referencia.");
                    }
                    

                } else
                {
                    alertify.alert("Debe agregar al menos un alumno para aplicar donativo");
                }
            }
        };
    
    $("#slcBuscar").change(function ()
    {
        buscar = this.value;
        BuscarTexto = $("#slcBuscar option:selected").text();
        $("#lbBuscar").text(BuscarTexto);
    });

    $("#btnBuscar").click(funciones.buscar);

    $('#txtDato').on('keydown', function (e) {
        if (e.which == 13) {
            funciones.buscar();
        }
    });

    $("#btnAgregar").click(funciones.btnAgregar);
    
    $("#btnBuscar1").click(funciones.btnBuscarClick);

    $('#txtClave').on('keydown', function (e) {
        if (e.which == 13) {
            funciones.btnBuscarClick();
        }
    });

    $('#tblAlumnos1').on('click', 'a', funciones.tablaAlumnoClick);

    $('#tblAlumnos').on('keyup mouseup', 'input', funciones.tablaMonto);
    $('#tblAlumnos').on('click', 'a', funciones.clickQuitar);

    $("#btnGuardar").click(funciones.AsignarDonativos);

});