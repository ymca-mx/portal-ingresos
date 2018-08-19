using System.Collections.Generic;

namespace DTO.SEP
{
    public class TituloGeneral
    {
        public int AlumnoId { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string CURP { get; set; }
        public string Email { get; set; }
        public int UsuarioId { get; set; }
        public Institucion Institucion { get; set; }
        public Titulo Titulo { get; set; }
        public Carrera Carrera { get; set; }
        public Antecedente Antecedente { get; set; }
        public List<Responsable> Responsables { get; set; }
        public TituloError Error { get; set; }
    }

    public class TituloError
    {
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public string InnerInnerMessage { get; set; }
    }

    public class Institucion
    {
        public string InstitucionId { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
    }
    public class Titulo
    {
        public int MedioTitulacionId { get; set; }
        public string MedioTitulacion { get; set; }
        public string FExamenProf { get; set; }
        public string FExencion { get; set; }
        public int FudamentoLegalId { get; set; }
        public int EntidadFederativaId { get; set; }
    }

    public class Carrera
    {
        public int OfertaEducativaId { get; set; }
        public string OfertaEducativa { get; set; }
        public string Clave { get; set; }
        public string FInicio { get; set; }
        public string FFin { get; set; }
        public int AutReconocimientoId { get; set; }
        public string RVOE { get; set; }
    }


    public class Antecedente
    {
        public string Institucion { get; set; }
        public int TipoAntecedenteId { get; set; }
        public string TipoAntecedente { get; set; }
        public int EntidadFederativaId { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }

    public class Responsable
    {
        public int UsuarioId { get; set; }
        public int Nombre { get; set; }
        public int Paterno { get; set; }
        public int Materno { get; set; }
        public int CargoId { get; set; }
    }
}