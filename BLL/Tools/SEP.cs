using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using DTO.SEP;

namespace BLL.Tools
{
    public class SEP
    {
        public static object CrearXMLTitulo(DAL.AlumnoTitulo alumno)
        {
            try
            {

                DateTime FechaPasada = new DateTime(1900, 1, 1, 23, 59, 59);
                string Folio = "A" + alumno.AlumnoTituloId + "-" + alumno.AlumnoId + "-" + alumno.AlumnoOfertaEducativaId;
                string Ruta = HttpContext.Current.Server.MapPath("~");
                Ruta += "//Documentos//SEP//Titulo//" + Folio + ".xml";

                if (!System.IO.File.Exists(Ruta))
                {

                    var Antecedente = new TituloElectronicoAntecedente
                    {
                        fechaInicio = alumno.AlumnoAntecedente1.FechaInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                        fechaTerminacion = alumno.AlumnoAntecedente1.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss"),
                        idEntidadFederativa = alumno.AlumnoAntecedente1.EntidadFederativaId.ToString(),
                        entidadFederativa = alumno.AlumnoAntecedente1.EntidadFederativa.Descripcion,
                        idTipoEstudioAntecedente = alumno.AlumnoAntecedente1.TipoEstudioAntecedenteId.ToString(),
                        tipoEstudioAntecedente = alumno.AlumnoAntecedente1.TipoEstudioAntecedente.Descripcion,
                        institucionProcedencia = alumno.AlumnoAntecedente1.Nombre
                    };
                    var Carrera = new TituloElectronicoCarrera
                    {
                        cveCarrera = alumno.AlumnoOfertaEducativa.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().ClaveOfertaEducativa,
                        nombreCarrera = alumno.AlumnoOfertaEducativa.OfertaEducativa.Descripcion,
                        fechaInicio = alumno.AlumnoOfertaEducativa.FechaInicio <= FechaPasada ? "" : alumno.AlumnoOfertaEducativa.FechaInicio.ToString("yyyy-MM-ddTHH"),
                        fechaTerminacion = alumno.AlumnoOfertaEducativa.FechaTermino.ToString("yyyy-MM-ddTHH:mm:ss"),
                        idAutorizacionReconocimiento = alumno.AutorizacionReconocimientoId.ToString(),
                        autorizacionReconocimiento = alumno.AutorizacionReconocimiento.Descripcion,
                        numeroRvoe = alumno.AlumnoOfertaEducativa.RVOE
                    };
                    var Expedicion = new TituloElectronicoExpedicion
                    {
                        fechaExpedicion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                        idModalidadTitulacion = alumno.ModalidadTitulacionId.ToString(),
                        modalidadTitulacion = alumno.ModalidadTitulacion.ModalidadTitulacion1,
                        fechaExamenProfesional = alumno.FechaExamenProfesional <= FechaPasada ? "" : alumno.FechaExamenProfesional.ToString("yyyy-MM-ddTHH:mm:ss"),
                        cumplioServicioSocial = "1",
                        idFundamentoLegalServicioSocial = alumno.FundamentoLegalId.ToString(),
                        fundamentoLegalServicioSocial = alumno.FundamentoLegal.Descripcion,
                        idEntidadFederativa = alumno.EntidadFederativaIdExpedicion < 10 ? "0" + alumno.EntidadFederativaIdExpedicion : alumno.EntidadFederativaIdExpedicion.ToString(),
                        entidadFederativa = alumno.EntidadFederativa.Descripcion
                    };
                    var Institucion = new TituloElectronicoInstitucion
                    {
                        cveInstitucion = alumno.AlumnoOfertaEducativa.InstitucionId,
                        nombreInstitucion = alumno.AlumnoOfertaEducativa.Institucion.Nombre
                    };
                    var Profesionista = new TituloElectronicoProfesionista
                    {
                        curp = alumno.Alumno.AlumnoDetalle.CURP,
                        nombre = alumno.Alumno.Nombre,
                        primerApellido = alumno.Alumno.Paterno,
                        segundoApellido = alumno.Alumno.Materno,
                        correoElectronico = alumno.Alumno.AlumnoDetalle.Email
                    };
                    var version = "1.0";
                    var folioControl = Folio;
                    var FirmaResponsables = new TituloElectronicoFirmaResponsable[]
                {
                    new TituloElectronicoFirmaResponsable
                    {
                        nombre=alumno.UsuarioResponsable.First().Usuario.Nombre,
                        primerApellido =alumno.UsuarioResponsable.First().Usuario.Paterno,
                        segundoApellido =alumno.UsuarioResponsable.First().Usuario.Materno,
                        curp="", //curp =alumno.UsuarioResponsable.First().Usuario.UsuarioDetalle.curp,
                        idCargo =alumno.UsuarioResponsable.First().Usuario.Cargo.First().CargoId.ToString(),
                        cargo =alumno.UsuarioResponsable.First().Usuario.Cargo.First().Descripcion,
                       abrTitulo="", //abrTitulo =alumno.UsuarioResponsable.First().Usuario.Titulo
                        sello ="",
                        certificadoResponsable="",//certificadoResponsable =GetCer(),
                        noCertificadoResponsable=""//noCertificadoResponsable ="00001000000400032474"
                    },
                    new TituloElectronicoFirmaResponsable
                    {
                        nombre=alumno.UsuarioResponsable.Last().Usuario.Nombre,
                        primerApellido =alumno.UsuarioResponsable.Last().Usuario.Paterno,
                        segundoApellido =alumno.UsuarioResponsable.Last().Usuario.Materno,
                        curp="",//curp =alumno.UsuarioResponsable.First().Usuario.UsuarioDetalle.curp,
                        idCargo =alumno.UsuarioResponsable.Last().Usuario.Cargo.First().CargoId.ToString(),
                        cargo =alumno.UsuarioResponsable.Last().Usuario.Cargo.First().Descripcion,
                        abrTitulo="",//abrTitulo =alumno.UsuarioResponsable.First().Usuario.Titulo
                        sello ="",
                        certificadoResponsable="",//certificadoResponsable =GetCer(),
                        noCertificadoResponsable=""//noCertificadoResponsable ="00001000000400032474"
                    }
                };

                    TituloElectronico objTitulo = new TituloElectronico
                    {
                        Antecedente = Antecedente,
                        Carrera = Carrera,
                        Expedicion = Expedicion,
                        Institucion = Institucion,
                        Profesionista = Profesionista,
                        version = version,
                        folioControl = folioControl,
                        FirmaResponsables = FirmaResponsables
                    };


                    var serialize = new XmlSerializer(typeof(TituloElectronico));


                    using (var stream = new StreamWriter(Ruta))
                    {
                        serialize.Serialize(stream, objTitulo);
                    }

                    return null;
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TituloElectronico));
                    TituloElectronico result;

                    using (FileStream fileStream = new FileStream(Ruta, FileMode.Open))
                    {
                         result = (TituloElectronico)serializer.Deserialize(fileStream);
                    }

                    File.Delete(Ruta);

                    foreach(TituloElectronicoFirmaResponsable responsable in result.FirmaResponsables)
                    {
                        if (responsable.sello.Length < 0)
                        {
                         var   dbResponsable=
                            alumno.UsuarioResponsable
                                .Where(a => a.Usuario.Nombre == responsable.nombre
                                                && a.Usuario.Paterno == responsable.primerApellido
                                                && a.Usuario.Materno == responsable.segundoApellido)
                                       .FirstOrDefault();

                            responsable.sello = "";
                            responsable.certificadoResponsable = "";
                            responsable.noCertificadoResponsable = "";

                        }
                    }

                    using (var stream = new StreamWriter(Ruta))
                    {
                        serializer.Serialize(stream, result);
                    }

                    return null;
                }
            }
            catch (Exception error)
            {
                return new
                {
                    error
                };
            }
        }
    }
}
