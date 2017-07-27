namespace DTO
{
   public class DTODocenteCurso
    {
        public int DocenteCursoId { get; set; }
        public string Institucion { get; set; }
        public int Duracion { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaInicial { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public bool EsCursoYMCA { get; set; }
        public bool VoBo { get; set; }
    }
}
