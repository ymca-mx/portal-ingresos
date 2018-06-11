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
        public string Nombre { get; set; }

        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
        public string PeriodoDescripcion { get; set; }

        public string _FechaInscripcion { get; set; }
        private DateTime FechaInscripcionP {get;set;}
        public System.DateTime FechaInscripcion {
            get
            {
                return FechaInscripcionP;
            }
            set
            {
                FechaInscripcionP = value;

                _FechaInscripcion = (value.Day < 10 ? "0" + value.Day : "" + value.Day) + "/" +
                                    (value.Month < 10 ? "0" + value.Month : "" + value.Month) + "/" +
                                    value.Year;
            }
        }

        public string _HoraInscripcion { get; set; }
        private TimeSpan HoraInscripcionP { get; set; }
        public System.TimeSpan HoraInscripcion
        {
            get
            {
                return HoraInscripcionP;
            }
            set
            {
                HoraInscripcionP = value;
                _HoraInscripcion = (value.Hours < 10 ? "0" + value.Hours : "" + value.Hours) + ":" +
                                    (value.Minutes < 10 ? "0" + value.Minutes : "" + value.Minutes);
            }
        }

        public Nullable<int> PagoPlanId { get; set; }
        public int TurnoId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public bool EsEmpresa { get; set; }
        public bool EsEspecial { get; set; }
        public DTOOfertaEducativa OfertaEducativa { get; set; }
        public int EstatusId { get; set; }
        public int Cuatrimestre { get; set; }
        public AlumnoAutorizacion AlumnoAutorizacion { get; set; }
        public bool Material { get; set; }
    }

    public class AlumnoAutorizacion
    {
        public int AlumnoAutorizacionId { get; set; }
        public int AlumnoId { get; set; }

        private DateTime Fecha_ { get; set; }
        public DateTime Fecha { get { return Fecha_; }
            set
            {
                Fecha_ = value;
                _Fecha= (value.Day < 10 ? "0" + value.Day : "" + value.Day) + "/" +
                                    (value.Month < 10 ? "0" + value.Month : "" + value.Month) + "/" +
                                    value.Year;
            }
        }
        public string _Fecha { get; set; }

        private TimeSpan Hora_ { get; set; }
        public TimeSpan Hora { get { return Hora_; }
            set
            {
                Hora_ = value;
                _Hora= (value.Hours < 10 ? "0" + value.Hours : "" + value.Hours) + ":" +
                                    (value.Minutes < 10 ? "0" + value.Minutes : "" + value.Minutes);
            }
        }
        public string _Hora { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
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
