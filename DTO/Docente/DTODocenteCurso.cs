namespace DTO
{
   public class DTODocenteCurso
    {
        public int DocenteCursoId { get; set; }
        public string Institucion { get; set; }
        public int Duracion { get; set; }
        public string Descripcion { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public bool EsCursoYMCA { get; set; }
        public bool VoBo { get; set; }
    }
}
