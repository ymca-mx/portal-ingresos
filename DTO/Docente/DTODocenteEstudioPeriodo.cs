namespace DTO
{
   public class DTODocenteEstudioPeriodo
    {
        public int DocenteEstudioPeriodoId { get; set; }
        public int DocenteId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public int EstudioId { get; set; }
        public DTODocenteEstudio EstudioDocente { get; set; }
    }
}
