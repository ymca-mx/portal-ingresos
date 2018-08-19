using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SEP
{
    public class DTOTitulo
    {
        public int AlumnoTituloId { get; set; }
        public int AlumnoId { get; set; }
        public int InstitucionId { get; set; }
        public int OfertaEducativaId { get; set; }
        public string FechaInicio { get; set; }
        public string FechaTermino { get; set; }
        public int AutorizacionReconocimientoId { get; set; }
        public string RVOE { get; set; }
        public string FechaExpedicion { get; set; }
        public int ModalidadTitulacionId { get; set; }
        public string FechaExencionExamenProfecional { get; set; }
        public bool ServicioSocial { get; set; }
        public int FundamentoLegalId { get; set; }
        public int EntidadFederativaIdExpedicion { get; set; }
        public int TipoEstudioAntecedenteId { get; set; }
        public int EntidadFederativaIdAntecedente { get; set; }
        public string FechaInicioAntecedente { get; set; }
        public string FechaFinAntecedente { get; set; }
        public int UsuarioId { get; set; }
        public List<DTOUsuarioResponsable> Resposables { get; set; }
    }

    public class DTOCertificado
    {
        public string CertificadoBase64 { get; set; }
        public string NoCertificado { get; set; }
    }

    public class DTOUsuarioResponsable
    {
        public int UsuarioResponsableId { get; set; }
        public int UsuarioId { get; set; }
    }
}
