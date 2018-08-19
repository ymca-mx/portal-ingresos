$(document).ready(function () {
    var tblInscritos, anio, periodo,descripcion;
    
    CargarCuatrimestre();
    
    $("#slcCuatrimestre").change(function () {

        anio = $('#slcCuatrimestre').find(':selected').data("anio");
        periodo = $('#slcCuatrimestre').find(':selected').data("periodoid");
        descripcion = $('#slcCuatrimestre option:selected').text();
        CargarAlumnosInscritos(anio, periodo);
    
    });

    $("#slcOferta").change(function () {
        filtro();
    });
    
    function filtro() {
        
          var oferta = $("#slcOferta option:selected").html();
        
                if (oferta != "--Todas--") {
                    if (oferta.includes('Derecho')) {
                        tblInscritos.columns(2)
                            .search(oferta)
                            .draw();
                    } else if (oferta.includes('Maestría en Educación')) {
                        var selected = [];
                        selected.push("^" + oferta + "$");
                        selected.push("^" + oferta + " SAT$");
                        var regex = selected.join("|");
                        tblInscritos.columns(2)
                            .search(regex, true, false, true)
                            .draw();
                    } else {
                        tblInscritos.columns(2)
                            .search("^" + oferta + "$", true, false, true)
                            .draw();
                    }

                } else {
                    tblInscritos.columns(2)
                        .search("")
                        .draw();
                }
                tblInscritos.draw();
    }

    function CargarCuatrimestre() {
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargarCuatrimestreHistorico",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.d === null) {
                    return false;
                }
                var datos = data.d.periodos;
                var datos2 = data.d.ofertas;
                if (datos.length > 0) {
                    var n = 0;
                    $(datos).each(function () {
                        var option = $(document.createElement('option'));
                        option.text(this.descripcion);
                        option.attr("data-Anio", this.anio);
                        option.attr("data-PeriodoId", this.periodoId);
                        option.val(n);

                        $("#slcCuatrimestre").append(option);
                        n++;
                    });// $(datos).each(function ()
                    $("#slcCuatrimestre").val(n-1);
                    $("#slcCuatrimestre").change();
                }//if
                if (datos2.length > 0) {
                    $(datos2).each(function () {
                        var option1 = $(document.createElement('option'));
                        option1.val(this.ofertaEducativaId);
                        option1.text(this.descripcion);
                        $("#slcOferta").append(option1);
                    });// $(datos).each(function ()
                }//if
            }//success
        });// $.ajax
        
    }

 

    function CargarAlumnosInscritos(anio, periodo) {
        IndexFn.Block(true);
        $.ajax({
            type: 'POST',
            url: "WS/Reporte.asmx/CargaReporteInscrito",
            data: "{anio:" + anio + ",periodo:" + periodo + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {

                if (data.d === null) {
                    IndexFn.Block(false);
                    return false;
                }

                tblInscritos = $("#dtInscritos").DataTable({
                    "aaData": data.d,
                    "aoColumns": [
                        { "mDataProp": "alumnoId", "sWidth": "10%" },
                        { "mDataProp": "nombreAlumno", "sWidth": "20%" },
                        { "mDataProp": "especialidad", "sWidth": "20%" },
                        { "mDataProp": "fechaInscripcion", "sWidth": "10%" },
                        { "mDataProp": "porcentajeDescuento", "sWidth": "10%" },
                        { "mDataProp": "tipoAlumno", "sWidth": "10%" },
                        { "mDataProp": "esEmpresa", "sWidth": "10%" },
                        { "mDataProp": "usuarioAplico", "sWidth": "10%" }
                    ],
                    "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, 'Todos']],
                    "searching": true,
                    "ordering": true,
                    "async": false,
                    "bDestroy": true,
                    "bPaginate": true,
                    "bLengthChange": true,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "asStripClasses": null,
                    "colReorder": false,
                    "oSearch": { "bSmart": false },
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
                        row.childNodes[3].style.textAlign = 'center';
                        row.childNodes[4].style.textAlign = 'right';
                    }
                    , "fnDrawCallback": function (oSettings) {
                        var registros = oSettings.aiDisplay.length;
                        $('#lbInscritos').text(registros);
                    }
                });//$('#dtbecas').DataTable

                var fil = $('#dtInscritos_filter label input');
                fil.removeClass('input-small').addClass('input-large');
                
                filtro();
                IndexFn.Block(false);
            }//success

        });//$.ajax


    }

 
    $('#divContenido').submit(function () {
        //do your stuff
        return false;
    });

    function exportarexcel(Tabla,nombre) {

        var table1 = $('#' + Tabla).dataTable().api();
        var data1 = table1.rows({ filter: 'applied' }).data();
        var data2 = [];
        var hd;

        $(data1).each(function () {
            var ojb2 = {
                "Alumno Id": this.alumnoId,
                "Nombre Alumno": this.nombreAlumno,
                "Oferta Educativa": this.especialidad,
                "Fecha de Inscripción": this.fechaInscripcion,
                "Porcentaje beca | descuento": this.porcentajeDescuento,
                "Tipo de Alumno": this.tipoAlumno,
                "Es Empresa": this.esEmpresa,
                "Usuario Aplico": this.usuarioAplico
            };
            data2.push(ojb2);
        });
        hd = ["Alumno Id", "Nombre Alumno", "Oferta Educativa", "Fecha de Inscripción", "Porcentaje beca | descuento", "Tipo de Alumno", "Es Empresa", "Usuario Aplico"];


        var ws = XLSX.utils.json_to_sheet(data2, {
            header: hd
        });

        var ws_name = nombre;

        function Workbook() {
            if (!(this instanceof Workbook)) return new Workbook();
            this.SheetNames = [];
            this.Sheets = {};
        }

        var wb = new Workbook();

        /* add worksheet to workbook */
        wb.SheetNames.push(ws_name);

        wb.Sheets[ws_name] = ws;

        var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });


        function s2ab(s) {
            var buf = new ArrayBuffer(s.length);
            var view = new Uint8Array(buf);
            for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
            return buf;
        }

        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), nombre + " " + descripcion + ".xlsx");
    }

    $('#btnInscritos').on('click', function () {
        exportarexcel('dtInscritos',"Inscritos");
    });



   
});


