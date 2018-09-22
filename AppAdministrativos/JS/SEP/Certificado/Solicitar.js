$(function () {
    var tblCertificados,
        tblAlumnos;

    var CertificadoFn = {
        init() {
            tblCertificados = $('#tblCertificados').DataTable();
            $('#btnBuscar').on('click', this.BtnBuscarClick);
            $('#txtClave').on('keydown', this.txtClaveKeydown);
            $('#slcSedePrev').on('change', this.SedeChangePrev);
            $('#tblAlumnos').on('click', 'a', this.TablaAlumnoClick);
        },
        AlumnoSelect: {},
        BuscarAlumno(idAlumno) {
            $('#modalAlumno input').val('');
            $('#modalAlumno select').empty();
            CertificadoFn.AlumnoSelect = {};

            IndexFn.Api("Certificado/Alumno/" + idAlumno, "GET", "")
                .done(function (data) {
                    IndexFn.Block(false);
                    if (data.Sede.length === 0) {
                        alertify.alert("Universidad YMCA", "El alumno no contiene una oferta valida o no tiene mas ofertas por titular.");
                        return false;
                    }
                    $('#txtAlumnoId').val(data.AlumnoId);
                    $('#txtNombre').val(data.Nombre);
                    $('#txtPaterno').val(data.Paterno);
                    $('#txtMaterno').val(data.Materno);
                    $('#txtCURP').val(data.CURP);
                    $('#txtEmail').val(data.Email);
                
                    CertificadoFn.SetSede(data.Sede, "slcSedePrev");
                    
                    $('#modalAlumno').data("Sede", JSON.stringify(data.Sede));

                    $('#btnAlumnoAdd').show();
                    $('#modalAlumno').modal('show');
                })
                .fail(function (data) {
                    console.log(data);
                    IndexFn.Block(false);
                });
        },
        BuscarNombre(Nombre) {

            IndexFn.Api("Alumno/BuscarAlumnoString/" + Nombre, "GET", "")
                .done(function (data) {
                    if (data !== null) {
                        $('#frmVarios').show();
                        tblAlumnos = $('#tblAlumnos').dataTable({
                            "aaData": data,
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
                })
                .fail(function (data) {
                    IndexFn.Block(false);
                    console.log(data);
                });
        },
        BtnBuscarClick() {

            var AlumnoId = $('#txtClave').val();

            if (AlumnoId.length === 0) { return false; }

            IndexFn.Block(true);

            if (!isNaN(AlumnoId)) { CertificadoFn.BuscarAlumno(AlumnoId); }
            else { CertificadoFn.BuscarNombre(AlumnoId); }
        },
        txtClaveKeydown(e) {
            if (e.which === 13) {
                CertificadoFn.BtnBuscarClick();
            }
        },
        TablaAlumnoClick() {
            IndexFn.Block(true);
            var rowadd = tblAlumnos.fnGetData($(this).closest('tr'));

            CertificadoFn.BuscarAlumno(rowadd.AlumnoId);
        },
        SetSede(lst, slcSede) {
            $("#" + slcSede).empty();

            $(lst).each(function () {
                var option = $(document.createElement('option'));

                option.val(this.SedeId);
                option.text(this.Nombre);
                option.data("Ofertas", JSON.stringify(this.Ofertas));
                option.data("Clave", this.Clave);

                $("#" + slcSede).append(option);
            });

            if (CertificadoFn.AlumnoSelect.AlumnoId !== undefined) {
                $('#' + slcSede).val(CertificadoFn.AlumnoSelect.Institucion.SedeId);
            }
            $('#' + slcSede).change();
        },
        SetOfertas(lst, slcoferta) {
            $('#' + slcoferta).empty();

            $(lst).each(function () {
                var option = $(document.createElement('option'));

                option.val(this.OfertaEducativaId);
                option.text(this.Descripcion);

                option.data("Clave", this.ClaveOfertaEducativa);
                option.data("RVOE", this.Rvoe);
                option.data("FechaFin", this.FechaFin);
                option.data("FechaInicio", this.FechaInicio);

                $("#" + slcoferta).append(option);
            });

            if (CertificadoFn.AlumnoSelect.AlumnoId !== undefined && CertificadoFn.AlumnoSelect.Carrera.OfertaEducativaId !== undefined) {
                $('#' + slcoferta).val(CertificadoFn.AlumnoSelect.Carrera.OfertaEducativaId);
            }
            $('#' + slcoferta).change();
        },
        SedeChangePrev() {
            CertificadoFn.SetOfertas(JSON.parse($('#slcSedePrev :selected').data("Ofertas")), "slcOfertaPrev");
        },
    };

    CertificadoFn.init();
});