using DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult SaveCurso([FromBody] JObject ObjCurso)
        {
            return Ok(BLL.BLLDocente
                .GuardarCurso(
                    ObjCurso["NombreInstitucion"].ToString(),
                    ObjCurso["TituloCurso"].ToString(),
                    int.Parse(ObjCurso["Anio"].ToString()),
                    int.Parse(ObjCurso["PeriodoId"].ToString()),
                    int.Parse(ObjCurso["Duracion"].ToString()),
                    ObjCurso["FechaFinal"].ToString(),
                    ObjCurso["FechaInicial"].ToString(),
                    bool.Parse(ObjCurso["EsCursoYmca"].ToString()),
                    int.Parse(ObjCurso["DocenteId"].ToString()),
                    int.Parse(ObjCurso["UsuarioId"].ToString()))
                );
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
        public IHttpActionResult GuardarFormacion([FromBody] DTODocenteEstudioPeriodo DocenteEstudio)
        {
            return Ok(BLL.BLLDocente
                        .GuardarFormacionAcademica(
                            DocenteEstudio.DocenteId,
                            DocenteEstudio.EstudioDocente.Institucion,
                            DocenteEstudio.EstudioDocente.OfertaEducativaTipoId ?? 0,
                            DocenteEstudio.EstudioDocente.Carrera,
                            DocenteEstudio.EstudioDocente.Documento.DocumentoTipoId,
                            DocenteEstudio.EstudioDocente.UsuarioId ?? 0,
                            DocenteEstudio.Anio,
                            DocenteEstudio.PeriodoId)
                );
        }
    }
}
