namespace DTO
{
   public class DTODocenteActualizacion
    {
        public int DocenteActualizacionId { get; set; }
        public int DocenteId { get; set; }
        public int ActualizacionId { get; set; }
        public bool EsCurso { get; set; }
        public DTODocenteCurso DocenteCurso { get; set; }
        public DTODocenteEstudio DocenteEstudio { get; set; }
    }
}
