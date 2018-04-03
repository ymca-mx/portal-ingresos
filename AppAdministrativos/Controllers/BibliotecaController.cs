using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Biblioteca")]
    public class BibliotecaController : ApiController
    {
        [Route("Comunicados")]
        [HttpGet]
        public IHttpActionResult TraerComunicados()
        {
            return Ok(BLL.BLLBiblioteca.GetHistorial());
        }

        [Route("SendComunicado")]
        [HttpPost]
        public IHttpActionResult EnviarComunicado()
        {
            HttpFileCollection Archivos = HttpContext.Current.Request.Files;
            System.Collections.Specialized.NameValueCollection Contenido = HttpContext.Current.Request.Form;

            try
            {
                string Asunto = Contenido["Asunto"].ToString();
                int UsuarioId = int.Parse(Contenido["UsuarioId"].ToString());
                var Comunicadohttp = Archivos["Documento"];
                string ruta = HttpContext.Current.Server.MapPath("../../Documentos/Comunicados/");

                JObject objComunicado =
                BLL.BLLBiblioteca.AddComunicado(Asunto, UsuarioId, Comunicadohttp.FileName);

                if (objComunicado["ComunicadoId"] != null)
                {
                    string nombre = objComunicado["ComunicadoId"] + "_" + objComunicado["Anio"] + "_" + objComunicado["PeriodoId"] + ".pdf";
                    saveFileAsync(ruta, nombre, Comunicadohttp.InputStream);

                    nombre = ruta + nombre;
                    return Ok(BLL.BLLBiblioteca.SendComunicado(objComunicado, nombre));
                }
                else { return BadRequest("fallo"); }
                
            }
            catch
            {
                return BadRequest("fallo");
            }            
        }

        void saveFileAsync(string directory, string name, Stream file)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string patch = directory + name;

            using (var stream = new FileStream(patch, FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(stream);
            }
        }

        [Route("Renviar/{ComunicadoId:int}")]
        [HttpPost]
        public IHttpActionResult ReEnviar(int ComunicadoId)
        {
            string ruta = HttpContext.Current.Server.MapPath("../../../Documentos/Comunicados/");

            return Ok(
            BLL.BLLBiblioteca.ReEnviarComunicado(ComunicadoId,ruta));
        }
    }
}
