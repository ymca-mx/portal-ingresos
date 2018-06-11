using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppAdministrativos.Controllers
{
    [RoutePrefix("Api/Empresas")]
    public class EmpresaController : ApiController
    {
        [Route("ListarEmpresas")]
        [HttpGet]
        public IHttpActionResult GetEmpresas()
        {
            return Ok(BLLEmpresa.ListaEmpresas());
        }

        [Route("ListarGrupos/{EmpresaId:int}")]
        [HttpGet]
        public IHttpActionResult GetGrupos(int EmpresaId)
        {
            return Ok(BLLGrupo.ListaGrupo(EmpresaId));
        }

        [Route("DireccionEmpresa/{SucursalId:int}")]
        [HttpGet]
        public IHttpActionResult GetDireccion(int SucursalId)
        {
            DTOSucursal sucursal = BLLSucursal.TraerSucursal(SucursalId);
            if (sucursal.Detalle.Calle != null)
            {
                return Ok("Calle " + sucursal.Detalle.Calle + " No.Exterior " + sucursal.Detalle.NoExterior + " Colonia " + sucursal.Detalle.Colonia + " Delegación " + sucursal.Detalle.Delegacion);
            }
            else { return Ok(""); }
        }

        [Route("GuardarEmpresa")]
        [HttpPost]
        public IHttpActionResult SaveEmpresa(DTOEmpresa Empresa)
        {
            return Ok(BLLEmpresa.GuardarEmpresa(Empresa));
        }

        [Route("MovimientosAlumnoGrupo")]
        [HttpPost]
        public IHttpActionResult SaveAlumnoGrupo(DTOGrupoMovimiento grupoMovimiento)
        {
            return Ok(BLLGrupo.MovimientosAlumnoGrupo(grupoMovimiento));
        }

        [Route("GuardarGrupo")]
        [HttpPost]
        public IHttpActionResult SaveGrupo(DTOGrupo grupo)
        {
            return Ok(BLLGrupo.GuardarGrupo2(grupo));
        }

        [Route("ListarAlumnos/{grupoId:int}")]
        [HttpGet]
        public IHttpActionResult GetAlumnosGrupo(int grupoId)
        {
            return Ok(BLLGrupo.TraerAlumnosDeEpresa(grupoId));
        }

        [Route("ListarEmpresaLigera")]
        [HttpGet]
        public IHttpActionResult GetEmpresaLigera()
        {
            return Ok(BLLEmpresa.ListarEmpresaLigera());
        }

        [Route("GuardarConfiguracion")]
        [HttpPost]
        public IHttpActionResult SaveConfiguracion(DTOGrupoAlumnoCuotaString AlumnoConfig)
        {
            if (BLLGrupo.GuardarAlumnoConfiguracion(AlumnoConfig))
            {
                BLLGrupo.GenerarCuotas(AlumnoConfig);
                return Ok(true);
            }
            else { return Ok(false); }
        }

    }
}
