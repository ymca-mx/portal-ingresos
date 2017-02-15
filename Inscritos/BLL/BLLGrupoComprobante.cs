using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
namespace BLL
{
    public class BLLGrupoComprobante
    {
        public static void GuardarComprobante(List<DTOGrupoComprobante> lstGrupos)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                lstGrupos.ForEach(delegate(DTOGrupoComprobante obj)
                {
                    db.GrupoComprobante.Add(new GrupoComprobante
                    {
                        GrupoId=obj.GrupoId,
                        Observaciones=obj.Observaciones,                        
                        GrupoComprobanteDocumento = new GrupoComprobanteDocumento
                        {
                            Documento=obj.GrupoComprobanteDocumento.Documento
                        }
                    });
                });
                db.SaveChanges();
            }
        }
    }
}
