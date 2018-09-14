var ClasesFn = {
    AlumnoTitulo: class {
        constructor(Alumno, objins, objTit, objCa, objAnt, lstUsuarios, lstSedes) {
            Alumno = Alumno || {};
            objins = objins || {};
            objTit = objTit || {};
            objCa = objCa || {};
            objAnt = objAnt || {};
            lstUsuarios = lstUsuarios || {};
            lstSedes = lstSedes || {};
            var objRes = [];
            $(lstUsuarios).each(function () { objRes.push(new ClasesFn.Responsable(this)); });

            this.AlumnoId = Alumno.AlumnoId;
            this.Nombre = Alumno.Nombre;
            this.Paterno = Alumno.Paterno;
            this.Materno = Alumno.Materno;
            this.CURP = Alumno.CURP;
            this.Email = Alumno.Email;
            this.Institucion = new ClasesFn.Institucion(objins);
            this.Titulo = new ClasesFn.Titulo(objTit);
            this.Carrera = new ClasesFn.Carrera(objCa);
            this.Antecedente = new ClasesFn.Antecedente(objAnt);
            this.Responsables = objRes;
            this.Sede = lstSedes;
            this.UsuarioId = Alumno.UsuarioId;
            this.EstatusId = Alumno.EstatusId;
        }
    },

    Institucion: class {
        constructor(obj) {
            obj = obj || {};
            this.InstitucionId = obj.InstitucionId;
            this.SedeId = obj.SedeId;
            this.Nombre = obj.Nombre;
            this.Clave = obj.Clave;
        }
    },

    Titulo: class {
        constructor(obj) {
            obj = obj || {};
            this.MedioTitulacionId = obj.MedioTitulacionId;
            this.MedioTitulacion = obj.MedioTitulacion;
            this.FExamenProf = obj.FExamenProf;
            this.FExencion = obj.FExencion;
            this.FundamentoLegalId = obj.FundamentoLegalId;
            this.EntidadFederativaId = obj.EntidadFederativaId;
        }
    },

    Carrera: class {
        constructor(obj) {
            obj = obj || {};
            this.OfertaEducativaId = obj.OfertaEducativaId;
            this.OfertaEducativa = obj.OfertaEducativa;
            this.Clave = obj.Clave;
            this.FInicio = obj.FInicio;
            this.FFin = obj.FFin;
            this.AutReconocimientoId = obj.AutReconocimientoId;
            this.RVOE = obj.RVOE;
        }
    },

    Antecedente: class {
        constructor(obj) {
            obj = obj || {};
            this.Institucion = obj.Institucion;
            this.TipoAntecedenteId = obj.TipoAntecedenteId;
            this.TipoAntecedente = obj.TipoAntecedente;
            this.EntidadFederativaId = obj.EntidadFederativaId;
            this.FechaInicio = obj.FechaInicio;
            this.FechaFin = obj.FechaFin;
        }
    },

    Responsable: class {
        constructor(obj) {
            obj = obj || {};
            this.UsuarioId = obj.UsuarioId;
            this.Nombre = obj.Nombre;
            this.Paterno = obj.Paterno;
            this.Materno = obj.Materno;
            this.CargoId = obj.CargoId;
        }
    }
};