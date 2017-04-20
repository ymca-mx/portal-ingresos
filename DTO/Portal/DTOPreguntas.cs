using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPreguntas
    {
        public int PreguntaId { get; set; }
        public string Descripcion { get; set; }
        public string SupPregunta { get; set; }
        public int PreguntaTipoId { get; set; }
        public List<DTOOpciones> Opciones { get; set; }
    }
    public class DTOOpciones
    {
        public int PreguntaTipoValoresId { get; set; }
        public int PreguntaTipoId { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
    }
    public class DTORespuestas
    {
        public List <DTORespuestas1> Respuestas { get; set; }
    }
    public class DTORespuestas1
    {
        public int AlumnoId { get; set; }
        public int Pregunta { get; set; }
        public int Respuesta { get; set; }
        public string Comentario { get; set; }
    }
}
