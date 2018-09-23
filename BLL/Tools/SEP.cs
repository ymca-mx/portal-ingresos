using DTO.SEP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BLL.Tools 
{
    public class SEP 
    {
        internal static object CrearXMLTitulo(DAL.AlumnoTitulo alumno, DAL.Usuario userdb)
        {
            try
            {
                var Fiel= GetUsuarioFIEL(alumno.UsuarioId);
                dynamic objCertificado = new
                {
                    CertificadoBase64 = "",
                    NoCertificado = ""
                };

                if (Fiel.Count == 2)
                {
                    objCertificado = GetCertificado(Fiel.Where(a => a.Contains(".cer")).FirstOrDefault());
                }

                DateTime FechaPasada = new DateTime(1900, 1, 1, 23, 59, 59);
                string Folio = "A" + alumno.AlumnoTituloId + "-" + alumno.AlumnoId + "-" + alumno.AlumnoOfertaEducativaId;
                string Ruta = HttpContext.Current.Server.MapPath("~");
                Ruta += "//Documentos//SEP//Titulo//" + Folio + ".xml";

                if (!System.IO.File.Exists(Ruta))
                {

                    var Antecedente = new TituloElectronicoAntecedente
                    {
                        fechaInicio = alumno.AlumnoAntecedente1.FechaInicio <= FechaPasada ? "" : alumno.AlumnoAntecedente1.FechaInicio.ToString("yyyy-MM-dd"),
                        fechaTerminacion = alumno.AlumnoAntecedente1.FechaFin.ToString("yyyy-MM-dd"),
                        idEntidadFederativa = alumno.AlumnoAntecedente1.EntidadFederativa.Clave,
                        entidadFederativa = alumno.AlumnoAntecedente1.EntidadFederativa.Descripcion,
                        idTipoEstudioAntecedente = alumno.AlumnoAntecedente1.TipoEstudioAntecedenteId.ToString(),
                        tipoEstudioAntecedente = alumno.AlumnoAntecedente1.TipoEstudioAntecedente.TipoEstudio,
                        institucionProcedencia = alumno.AlumnoAntecedente1.Nombre,
                        noCedula=""
                    };
                    var Carrera = new TituloElectronicoCarrera
                    {
                        cveCarrera = alumno.AlumnoOfertaEducativa.OfertaEducativa.InstitucionOfertaEducativa.FirstOrDefault().ClaveOfertaEducativa,
                        nombreCarrera = alumno.AlumnoOfertaEducativa.OfertaEducativa.DescripcionSEP,
                        fechaInicio = alumno.AlumnoOfertaEducativa.FechaInicio <= FechaPasada ? "" : alumno.AlumnoOfertaEducativa.FechaInicio.ToString("yyyy-MM-dd"),
                        fechaTerminacion = alumno.AlumnoOfertaEducativa.FechaTermino.ToString("yyyy-MM-dd"),
                        idAutorizacionReconocimiento = alumno.AutorizacionReconocimientoId.ToString(),
                        autorizacionReconocimiento = alumno.AutorizacionReconocimiento.Descripcion,
                        numeroRvoe = alumno.AlumnoOfertaEducativa.RVOE
                    };
                    var Expedicion = new TituloElectronicoExpedicion
                    {
                        fechaExpedicion = DateTime.Now.ToString("yyyy-MM-dd"),
                        idModalidadTitulacion = alumno.ModalidadTitulacionId.ToString(),
                        modalidadTitulacion = alumno.ModalidadTitulacion.ModalidadTitulacion1,
                        fechaExamenProfesional = alumno.FechaExamenProfesional <= FechaPasada ? "" : alumno.FechaExamenProfesional.ToString("yyyy-MM-dd"),
                        cumplioServicioSocial = "1",
                        idFundamentoLegalServicioSocial = alumno.FundamentoLegalId.ToString(),
                        fundamentoLegalServicioSocial = alumno.FundamentoLegal.Descripcion,
                        idEntidadFederativa = alumno.EntidadFederativa.Clave,
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

                    TituloElectronico objTitulo = new TituloElectronico
                    {
                        Antecedente = Antecedente,
                        Carrera = Carrera,
                        Expedicion = Expedicion,
                        Institucion = Institucion,
                        Profesionista = Profesionista,
                        version = version,
                        folioControl = folioControl,
                        FirmaResponsables = new TituloElectronicoFirmaResponsable[]
                {
                    new TituloElectronicoFirmaResponsable
                    {
                        nombre=alumno.UsuarioResponsable.First().Usuario.Nombre,
                        primerApellido =alumno.UsuarioResponsable.First().Usuario.Paterno,
                        segundoApellido =alumno.UsuarioResponsable.First().Usuario.Materno,
                        curp=alumno.UsuarioResponsable.First().Usuario.UsuarioDetalle.CURP,
                        idCargo =alumno.UsuarioResponsable.First().Usuario.Cargo.First().CargoId.ToString(),
                        cargo =alumno.UsuarioResponsable.First().Usuario.Cargo.First().Descripcion,
                       abrTitulo=alumno.UsuarioResponsable.First().Usuario.UsuarioDetalle.TituloCorto,
                        sello ="",
                        certificadoResponsable=alumno.UsuarioResponsable.First().UsuarioId ==alumno.UsuarioId?objCertificado.CertificadoBase64 :"",
                        noCertificadoResponsable= alumno.UsuarioResponsable.First().UsuarioId ==alumno.UsuarioId?objCertificado.NoCertificado :""
                    },
                    new TituloElectronicoFirmaResponsable
                    {
                        nombre=alumno.UsuarioResponsable.Last().Usuario.Nombre,
                        primerApellido =alumno.UsuarioResponsable.Last().Usuario.Paterno,
                        segundoApellido =alumno.UsuarioResponsable.Last().Usuario.Materno,
                        curp=alumno.UsuarioResponsable.Last().Usuario.UsuarioDetalle.CURP,
                        idCargo =alumno.UsuarioResponsable.Last().Usuario.Cargo.First().CargoId.ToString(),
                        cargo =alumno.UsuarioResponsable.Last().Usuario.Cargo.First().Descripcion,
                        abrTitulo=alumno.UsuarioResponsable.Last().Usuario.UsuarioDetalle.TituloCorto,
                        sello ="",
                        certificadoResponsable=alumno.UsuarioResponsable.Last().UsuarioId ==alumno.UsuarioId?objCertificado.CertificadoBase64 :"",
                        noCertificadoResponsable= alumno.UsuarioResponsable.Last().UsuarioId ==alumno.UsuarioId?objCertificado.NoCertificado :""
                    }
                }
                    };

                    var objResp = objTitulo.FirmaResponsables.Where(responsable => userdb.Nombre == responsable.nombre
                                                   && userdb.Paterno == responsable.primerApellido
                                                   && userdb.Materno == responsable.segundoApellido)
                                          .FirstOrDefault();

                    objResp.sello = CreateSello(Fiel.Where(a => a.Contains(".key")).FirstOrDefault(),
                        Utilities.Seguridad.Desencripta(27, userdb.UsuarioDetalle.PassSat),
                        objTitulo);

                    var serialize = new XmlSerializer(typeof(TituloElectronico));


                    using (var stream = new StreamWriter(Ruta))
                    {
                        serialize.Serialize(stream, objTitulo);
                    }

                    Directory.Delete(HttpContext.Current.Server.MapPath("~") + "\\Documentos\\SEP\\FIEL\\" + alumno.UsuarioId, true);

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

                    foreach (TituloElectronicoFirmaResponsable responsable in result.FirmaResponsables)
                    {
                        if (responsable.sello.Length == 0)
                        {
                            var dbResponsable =
                               alumno.UsuarioResponsable
                                   .Where(a => a.Usuario.Nombre == responsable.nombre
                                                   && a.Usuario.Paterno == responsable.primerApellido
                                                   && a.Usuario.Materno == responsable.segundoApellido)
                                          .FirstOrDefault();

                            responsable.sello = CreateSello(Fiel.Where(a => a.Contains(".key")).FirstOrDefault(),
                        Utilities.Seguridad.Desencripta(27, userdb.UsuarioDetalle.PassSat),
                        result);

                            responsable.certificadoResponsable = dbResponsable.UsuarioId == alumno.UsuarioId ? objCertificado.CertificadoBase64 : "";
                            responsable.noCertificadoResponsable = dbResponsable.UsuarioId == alumno.UsuarioId ? objCertificado.NoCertificado : "";

                        }
                    }

                    using (var stream = new StreamWriter(Ruta))
                    {
                        serializer.Serialize(stream, result);
                    }

                    Directory.Delete(HttpContext.Current.Server.MapPath("~") + "\\Documentos\\SEP\\FIEL\\" + alumno.UsuarioId, true);

                    return null;
                }
            }
            catch (Exception error)
            {
                Directory.Delete(HttpContext.Current.Server.MapPath("~") + "\\Documentos\\SEP\\FIEL\\" + alumno.UsuarioId,true);
                return new
                {
                    error
                };
            }
        }

        internal static dynamic GetCertificado(string certificado)
        {
            try
            {
                X509Certificate2 certEmisor = new X509Certificate2();
                byte[] byteCertData = ReadFile(certificado);
                certEmisor.Import(byteCertData);

                string ncert = certEmisor.SerialNumber.ToString(),
                    tNumero = "",
                    rNumero = "",
                     tNumero2 = "";

                foreach (char c in ncert)
                {
                    switch (c)
                    {
                        case '0': tNumero += c; break;
                        case '1': tNumero += c; break;
                        case '2': tNumero += c; break;
                        case '3': tNumero += c; break;
                        case '4': tNumero += c; break;
                        case '5': tNumero += c; break;
                        case '6': tNumero += c; break;
                        case '7': tNumero += c; break;
                        case '8': tNumero += c; break;
                        case '9': tNumero += c; break;
                    }
                }
                for (int X = 1; X < tNumero.Length; X++)
                {
                    //wNewString = wNewString & Right$(Left$(wCadena, x), 1)
                    X += 1;
                    //rNumero = rNumero + 
                    tNumero2 = tNumero.Substring(0, X);
                    rNumero = rNumero + tNumero2.Substring(tNumero2.Length - 1, 1);// Right$(Left$(wCadena, x), 1)
                }

                return new
                {
                    CertificadoBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(certEmisor.GetRawCertData()))),
                    NoCertificado = rNumero
                };
            }
            catch
            {
                return null;
            }
        }

        internal static object SendArchivos(List<string> rutaFiles)
        {


            Titulos_SEP.cargaTituloElectronicoRequest objSEP = new Titulos_SEP.cargaTituloElectronicoRequest();

            string Ruta = HttpContext.Current.Server.MapPath("~");
            Ruta += "//Documentos//SEP//Titulo//";

            if (rutaFiles.Count > 1)
            {
                DateTime FHoy = DateTime.Now;                
                string NameZIP = FHoy.Day + "_" + FHoy.Month + "_" + FHoy.Year + "_" + FHoy.Hour + "_" + FHoy.Minute + ".Zip";

                objSEP.archivoBase64 = ReadFile(Ruta + NameZIP);
                objSEP.nombreArchivo = NameZIP;
            }
            else
            {
                objSEP.archivoBase64= ReadFile(Ruta+rutaFiles[0]);
                objSEP.nombreArchivo = rutaFiles[0];
            }

            objSEP.autenticacion = new Titulos_SEP.autenticacionType
            {
                usuario = "usuariomet.qa114",
                password = "jVJmXxGC"
            };
            try
            {
                //cargaTituloElectronicoResponse1 Result = titulosPort.cargaTituloElectronico(new cargaTituloElectronicoRequest1(objSEP));

                Titulos_SEP.TitulosPortTypeClient ClientSEP = new Titulos_SEP.TitulosPortTypeClient("TitulosPortTypeSoap11");
                ClientSEP.Open();

                var resul =
                ClientSEP.cargaTituloElectronico(objSEP);

                ClientSEP.Close();

                return new
                {
                    Status=true,
                    resul.mensaje,
                    resul.numeroLote
                };
            }catch(Exception error)
            {
                return new
                {
                    Status = false,
                    error.Message,
                    Inner = error?.InnerException?.Message ?? "",
                    Inner2 = error?.InnerException?.InnerException?.Message
                };
            }
        }

        private void ComprimirFiles(List<string> Files, string ruta, string Nombre)
        {
            using (FileStream zipToOpen = new FileStream(ruta + Nombre, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    Files.ForEach(archivo =>
                    {
                        archive.CreateEntryFromFile(ruta + archivo, archivo);
                    });
                }
            }
        }

        internal static List<dynamic> RevisarLotes(List<dynamic> lstLotes)
        {
            Titulos_SEP.consultaProcesoTituloElectronicoRequest objSEP = new Titulos_SEP.consultaProcesoTituloElectronicoRequest();
            Titulos_SEP.descargaTituloElectronicoRequest objSEPDow = new Titulos_SEP.descargaTituloElectronicoRequest();
            Titulos_SEP.TitulosPortTypeClient ClientSEP = new Titulos_SEP.TitulosPortTypeClient("TitulosPortTypeSoap11");
            List<dynamic> lstResult= new List<dynamic>();

            ClientSEP.Open();
            objSEP.autenticacion = new Titulos_SEP.autenticacionType
            {
                usuario = "usuariomet.qa114",
                password = "jVJmXxGC"
            };
            objSEPDow.autenticacion = objSEP.autenticacion;

            lstLotes.ForEach(objlote =>
            {
                objSEP.numeroLote = objlote.NumeroLote;

                var resul =
                ClientSEP.consultaProcesoTituloElectronico(objSEP);

                lstResult.Add(new
                {
                    objlote.AlumnoTituloId,
                    objlote.NumeroLote,
                    resul.mensaje,
                    resul.estatusLote
                });
            });

            lstResult.ForEach(obj =>
            {
                objSEPDow.numeroLote = obj.NumeroLote;

                var fileN = ClientSEP.descargaTituloElectronico(objSEPDow);
            });

            ClientSEP.Close();

            return lstResult;
        }

        internal static byte[] ReadFile(string strArchivo)
        {
            FileStream f = new FileStream(strArchivo, FileMode.Open, FileAccess.Read);
            int size = (int)f.Length;
            byte[] data = new byte[size];
            size = f.Read(data, 0, size);
            f.Close();
            return data;
        }

        internal static List<string> GetUsuarioFIEL(int UsuarioId)
        {
            try
            {
                string Ruta = HttpContext.Current.Server.MapPath("~");
                Ruta += "\\Documentos\\SEP\\FIEL\\" + UsuarioId + ".zip";

                string UsuarioSalida = HttpContext.Current.Server.MapPath("~"); 
                UsuarioSalida += "\\Documentos\\SEP\\FIEL\\" + UsuarioId + "\\";

                if (!Directory.Exists(UsuarioSalida))
                    Directory.CreateDirectory(UsuarioSalida);

                ZipFile.ExtractToDirectory(Ruta, UsuarioSalida);

                return Directory.GetFiles(UsuarioSalida).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        internal static string CreateSello( string ArchivoClavePrivada, string lPassword, TituloElectronico titulo )
        {
            try
            {
                #region Cadena Original
                string CadenaOriginal = "||";

                CadenaOriginal += titulo.version + "|";
                CadenaOriginal += titulo.folioControl + "|";

                CadenaOriginal += titulo.FirmaResponsables[0].curp + "|";
                CadenaOriginal += titulo.FirmaResponsables[0].idCargo + "|";
                CadenaOriginal += titulo.FirmaResponsables[0].cargo + "|";
                CadenaOriginal += titulo.FirmaResponsables[0].abrTitulo + "|";
                CadenaOriginal += titulo.FirmaResponsables[1].curp + "|";
                CadenaOriginal += titulo.FirmaResponsables[1].idCargo + "|";
                CadenaOriginal += titulo.FirmaResponsables[1].cargo + "|";
                CadenaOriginal += titulo.FirmaResponsables[1].abrTitulo + "|";

                CadenaOriginal += titulo.Institucion.cveInstitucion + "|";
                CadenaOriginal += titulo.Institucion.nombreInstitucion + "|";

                CadenaOriginal += titulo.Carrera.cveCarrera + "|";
                CadenaOriginal += titulo.Carrera.nombreCarrera + "|";
                CadenaOriginal += titulo.Carrera.fechaInicio + "|";
                CadenaOriginal += titulo.Carrera.fechaTerminacion + "|";
                CadenaOriginal += titulo.Carrera.idAutorizacionReconocimiento + "|";
                CadenaOriginal += titulo.Carrera.autorizacionReconocimiento + "|";
                CadenaOriginal += titulo.Carrera.numeroRvoe + "|";

                CadenaOriginal += titulo.Profesionista.curp + "|";
                CadenaOriginal += titulo.Profesionista.nombre + "|";
                CadenaOriginal += titulo.Profesionista.primerApellido + "|";
                CadenaOriginal += titulo.Profesionista.segundoApellido + "|";
                CadenaOriginal += titulo.Profesionista.correoElectronico + "|";

                CadenaOriginal += titulo.Expedicion.fechaExpedicion + "|";
                CadenaOriginal += titulo.Expedicion.idModalidadTitulacion + "|";
                CadenaOriginal += titulo.Expedicion.modalidadTitulacion + "|";
                CadenaOriginal += titulo.Expedicion.fechaExamenProfesional + "|";
                CadenaOriginal += titulo.Expedicion.fechaExencionExamenProfesional + "|";
                CadenaOriginal += titulo.Expedicion.cumplioServicioSocial + "|";
                CadenaOriginal += titulo.Expedicion.idFundamentoLegalServicioSocial + "|";
                CadenaOriginal += titulo.Expedicion.fundamentoLegalServicioSocial + "|";
                CadenaOriginal += titulo.Expedicion.idEntidadFederativa + "|";
                CadenaOriginal += titulo.Expedicion.entidadFederativa + "|";

                CadenaOriginal += titulo.Antecedente.institucionProcedencia + "|";
                CadenaOriginal += titulo.Antecedente.idTipoEstudioAntecedente + "|";
                CadenaOriginal += titulo.Antecedente.tipoEstudioAntecedente + "|";
                CadenaOriginal += titulo.Antecedente.idEntidadFederativa + "|";
                CadenaOriginal += titulo.Antecedente.entidadFederativa + "|";
                CadenaOriginal += titulo.Antecedente.fechaInicio + "|";
                CadenaOriginal += titulo.Antecedente.fechaTerminacion + "|";
                CadenaOriginal += titulo.Antecedente.noCedula + "|";

                CadenaOriginal += "||";

                #endregion

                byte[] ClavePrivada = File.ReadAllBytes(ArchivoClavePrivada);
                byte[] bytesFirmados = null;
                byte[] bCadenaOriginal = null;

                SecureString lSecStr = new SecureString();
                SHA256Managed sham = new SHA256Managed();
                // SHA1Managed sham = new SHA1Managed(); version 3.2
                lSecStr.Clear();

                foreach (char c in lPassword.ToCharArray())
                    lSecStr.AppendChar(c);

                RSACryptoServiceProvider lrsa = JavaScience.opensslkey.DecodeEncryptedPrivateKeyInfo(ClavePrivada, lSecStr);
                bCadenaOriginal = Encoding.UTF8.GetBytes(CadenaOriginal);
                try
                {
                    bytesFirmados = lrsa.SignData(Encoding.UTF8.GetBytes(CadenaOriginal), sham);

                }
                catch (NullReferenceException ex)
                {
                    throw new NullReferenceException("Clave privada incorrecta, revisa que la clave que escribes corresponde a los sellos digitales cargados");
                }
                string sellodigital = Convert.ToBase64String(bytesFirmados);
                return sellodigital;
            }
            catch
            {
                return "";
            }

        }
        
    }
}
