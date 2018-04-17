using DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Docentes")]
    public class DocentesController : ApiController
    {
        [Route("TraerDocentes")]
        [HttpGet]
        public async System.Threading.Tasks.Task<IHttpActionResult> Docentes()
        {
            HttpClient client = new HttpClient();

           //List<DTO.DTODocenteActualizar> Docentes = null;
            try
            {
                //HttpResponseMessage response = await client.GetAsync("http://localhost:24959/api/Docente/GetAllDocentes");
                //if (response.IsSuccessStatusCode)
                //{
                //    Docentes = await response.Content.ReadAsAsync<List<DTO.DTODocenteActualizar>>();
                //}

                //return Ok(Docentes);
                return Ok(BLL.BLLDocente.ListaDocentesActualizar());
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }

        }

        [Route("GuardarCurso")]
        [HttpPost]
        public IHttpActionResult SaveCurso(DTODocenteCurso ObjCurso)
        {
            return Ok(BLL.BLLDocente.GuardarCurso(ObjCurso));
        }

        [Route("TraerPeriodos")]
        [HttpGet]
        public IHttpActionResult TraerPeriodos()
        {
            return
                Ok(BLL.BLLDocente.TraerPeriodoActSig());
        }

        [Route("TipoDocumentos")]
        [HttpGet]
        public IHttpActionResult TraerDocumentosTipo()
        {
            return Ok(BLL.BLLDocente.GetDocumentoTipo());
        }

        [Route("GuardarFormacion")]
        [HttpPost]
        public IHttpActionResult GuardarFormacion()
        {
            try
            {
                HttpFileCollection httpFileCollection = HttpContext.Current.Request.Files;
                System.Collections.Specialized.NameValueCollection Contenido = HttpContext.Current.Request.Form;

                DTODocenteEstudioPeriodo DocenteEstudio = JsonConvert.DeserializeObject<DTODocenteEstudioPeriodo>(Contenido["objDocente"].ToString());

                int EstudioId = -1;

                if (DocenteEstudio.EstudioId > 0)
                {
                    if (BLL.BLLDocente.ModificarFormacionAcademica(DocenteEstudio))
                    {
                        EstudioId = DocenteEstudio.EstudioId;
                    }
                }
                else
                {
                    EstudioId=
                    BLL.BLLDocente
                                .GuardarFormacionAcademica(
                                    DocenteEstudio.DocenteId,
                                    DocenteEstudio.EstudioDocente.Institucion,
                                    DocenteEstudio.EstudioDocente.OfertaEducativaTipoId ?? 0,
                                    DocenteEstudio.EstudioDocente.Carrera,
                                    DocenteEstudio.EstudioDocente.Documento.DocumentoTipoId,
                                    DocenteEstudio.EstudioDocente.UsuarioId ?? 0,
                                    DocenteEstudio.Anio,
                                    DocenteEstudio.PeriodoId);
                }
                if (EstudioId != -1)
                {

                    HttpPostedFile httpDocumento = httpFileCollection["DocumentoComprobante"];
                    if (httpDocumento != null)
                    {
                        Stream strDocumento = httpDocumento.InputStream;
                        byte[] DocumentoByte = Herramientas.ConvertidorT.ConvertirStream(strDocumento, httpDocumento.ContentLength);


                        string RutaServe =
                                     HttpContext.Current.Server.MapPath("/EgresosUniYMCA/Documentos/");
                        string rutas2 = "../EgresosUniYMCA/Documentos/";
                        if (BLL.BLLDocente.GuardarRelacionDocumento(EstudioId, DocenteEstudio.EstudioDocente.Documento.DocumentoTipoId, rutas2 + EstudioId + ".pdf"))
                        {
                            if (File.Exists(RutaServe + EstudioId + ".pdf"))
                            {
                                File.Delete(RutaServe + EstudioId + ".pdf");
                                File.WriteAllBytes(RutaServe + EstudioId + ".pdf", DocumentoByte);
                            }
                            else { File.WriteAllBytes(RutaServe + EstudioId + ".pdf", DocumentoByte); }
                        }
                        else
                        {
                            return Ok(new
                            {
                                EstudioId,
                                Messange = "Se guardo el estudio pero no el documento intente de nuevo."
                            });
                        }
                    }

                    return Ok(new
                    {
                        EstudioId,
                        Messange = "Se guardo todo correctamente"
                    });
                }
                else
                {
                   return BadRequest("Error al obtener el ID");
                }
            }
            catch (Exception Error)
            {
                return BadRequest(Error.Message);
            }
        }
    }
}
