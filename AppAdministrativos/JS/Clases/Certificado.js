var ClasesFn = {
    AlumnoCertificado: class {
        constructor(Alumno) {
            Alumno = Alumno || {};

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
    }
};