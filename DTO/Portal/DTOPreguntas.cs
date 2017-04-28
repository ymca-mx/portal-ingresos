using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOPreguntas2
    {
        public int PreguntaId { get; set; }
        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public int PreguntaTipoId1 { get; set; }
        public int PreguntaTipoId2 { get; set; }
        public List<DTOOpciones> Opciones1 { get; set; }
        public List<DTOOpciones> Opciones2 { get; set; }
    }

    public class DTOPreguntas
    {
        public int PreguntaId { get; set; }
        public List<DTOPregunta> Preguntas { get; set; }
    }
    public class DTOPregunta
    {
        public string Pregunta { get; set; }
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
        public int Respuesta1 { get; set; }
        public Nullable<int> Respuesta2 { get; set; }
        public string Comentario { get; set; }
    }
}
