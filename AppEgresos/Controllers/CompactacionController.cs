using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL;

namespace AppEgresos.Controllers
{
    public class CompactacionController : ApiController
    {

        [HttpGet]
        [ActionName("ofertaEducativa")]
        public IHttpActionResult ofertaEducativa()
        {
            using (UniversidadEntities db=new UniversidadEntities())
            {
                List<DTOOfertaEducativa> ofertas = db.OfertaEducativa.OrderBy(a => a.OfertaEducativaTipoId)
                                                                     .Select(b => new DTOOfertaEducativa
                                                                     {
                                                                         ofertaEducativaId = b.OfertaEducativaId,
                                                                         ofertaEducativaTipoId = b.OfertaEducativaTipoId,
                                                                         ofertaEducativa= b.Descripcion
                                                                     }).ToList();

            return Ok(ofertas);
            }
        }

        [HttpGet]
        [ActionName("catalogos")]
        public IHttpActionResult catalogos(int ofertaEducativaId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOGrupo> grupos = null;

                List<DTOMateria> materias = db.Materia.Where(a => a.OfertaEducativaId == ofertaEducativaId)
                                                      .Select(b => new DTOMateria
                                                      {
                                                          materiaId = b.MateriaId,
                                                          materia = b.Descripcion,
                                                          ofertaEducativaId = b.OfertaEducativaId,
                                                          clave = b.Clave,
                                                          creditos = b.Creditos.ToString()
                                                      }).ToList();

                DTOCatalogoCompactacion cat = new DTOCatalogoCompactacion();

                cat.grupos = grupos;
                cat.materias = materias;

                return Ok(cat);
            }
        }

    }
}
