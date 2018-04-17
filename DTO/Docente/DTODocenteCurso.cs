namespace DTO
{
   public class DTODocenteCurso
    {
        public int DocenteCursoId { get; set; }
        public int DocenteId { get; set; }
        public string Institucion { get; set; }
        public int Duracion { get; set; }
        public string Descripcion { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public bool EsCursoYMCA { get; set; }
        public bool VoBo { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public DTOPeriodo Periodo { get; set; }
        public int UsuarioId { get; set; }
    }
}
