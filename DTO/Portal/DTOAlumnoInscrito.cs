using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTOAlumnoInscrito
    {
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public System.DateTime FechaInscripcion { get; set; }

        public System.TimeSpan HoraInscripcion { get; set; }
        public Nullable<int> PagoPlanId { get; set; }
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public bool EsEmpresa { get; set; }
        public bool EsEspecial { get; set; }
        public DTOOfertaEducativa OfertaEducativa { get; set; }
        public int EstatusId { get; set; }
        public int Cuatrimestre { get; set; }
        
    }
    public class DTOAlumnoInscrito2
    {
        public string Descripcion { get; set; }
        public List<string> Descripcion3
        {
            get { return new List<string> { Descripcion }; }
            set { Descripcion = Concatenado2(value); }
        }
        public DTOAlumnoInscrito2()
        {
            Descripcion = "";
        }
        private string Concatenado(List<string> lstDetalles)
        {
            lstDetalles.ForEach(Det =>
                {
                    string primera = lstDetalles[0];
                    lstDetalles[0] = "";
                    lstDetalles[0] = lstDetalles.Count > 1 ? primera == lstDetalles[0] ? primera + "/" : primera + Det : Det;
                });
            return lstDetalles.Count > 1 ? lstDetalles[0].Remove(lstDetalles.Count - 1, 1) : lstDetalles[0];
        }
        public string Concatenado2(List<string> lstDetalles)
        {
            if (lstDetalles.Count == 0) { return ""; }
            lstDetalles.ForEach(Det =>
            {
                string primera = lstDetalles[0];
                lstDetalles[0] = "";
                lstDetalles[0] = lstDetalles.Count > 1 ? primera == lstDetalles[0] ? primera + "/" : primera + Det : Det;
            });
            //Descripcion = lstDetalles[0];
            return lstDetalles.Count > 1 ? lstDetalles[0].Remove(lstDetalles.Count - 1, 1) : lstDetalles[0];
        }
    }
}
