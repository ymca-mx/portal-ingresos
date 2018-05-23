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
            var asf = Empresa;
            return Ok();
        }


    }
}
