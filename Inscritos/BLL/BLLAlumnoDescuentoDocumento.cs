using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{
    public class BLLAlumnoDescuentoDocumento
    {
        public static void AlumnoDocumentoGuardar(List<DTOAlumnoDescuentoDocumento> listAlumnosDescuentos)
        { 
            using(UniversidadEntities db = new UniversidadEntities() )
            {
                listAlumnosDescuentos.ForEach(delegate(DTOAlumnoDescuentoDocumento objDoc)
                {
                    db.AlumnoDescuentoDocumento.Add(new AlumnoDescuentoDocumento
                    {
                        AlumnoDescuentoId = objDoc.AlumnoDescuentoId,
                        AlumnoDescuentoDocumento1 = objDoc.AlumnoDescuentoDocumento1
                    });
                });                   
                
                db.SaveChanges();
            }
        }

        public static DTOAlumnoDescuentoDocumento AlumnoDocumentoConsultar()
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                DTOAlumnoDescuentoDocumento objAlumnoDocumento = (from a in db.AlumnoDescuentoDocumento
                                                                  where a.AlumnoDescuentoId == 1
                                                                  select new DTOAlumnoDescuentoDocumento
                                                                  {
                                                                      AlumnoDescuentoId = a.AlumnoDescuentoId,
                                                                      AlumnoDescuentoDocumento1 = a.AlumnoDescuentoDocumento1
                                                                  }).FirstOrDefault();
                return objAlumnoDocumento;
            }
        }
        public static string GuardarAlumnoInscritoBecaDocumentos(DTOAlumnoInscritoBecaDocumento objDoc)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    db.AlumnoInscritoBecaDocumento.Add(new AlumnoInscritoBecaDocumento
                    {
                        AlumnoId = objDoc.AlumnoId,
                        Anio = objDoc.Anio,
                        ArchivoBeca = objDoc.ArchivoBeca,
                        ArchivoComite = objDoc.ArchivoComite,
                        EsComite = objDoc.EsComite,
                        OfertaEducativaId = objDoc.OfertaEducativaId,
                        PeriodoId = objDoc.PeriodoId
                    });

                    db.SaveChanges();

                    return "Guardado";
                }
                catch (Exception a)
                {
                    return a.Message;
                }
            }
        }
        public static DTOAlumnoInscritoBecaDocumento BuscarAlumno(int AlumnoId, int OfertaEducativaId, int Anio, int PeriodoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoInscritoBecaDocumento
                        where a.AlumnoId == AlumnoId && a.Anio == Anio && a.PeriodoId == a.PeriodoId && a.OfertaEducativaId == OfertaEducativaId
                        select new DTOAlumnoInscritoBecaDocumento
                        {
                            AlumnoId = a.AlumnoId,
                            Anio = a.Anio,
                            ArchivoBeca = a.ArchivoBeca,
                            ArchivoComite = a.ArchivoComite,
                            EsComite = a.EsComite,
                            OfertaEducativaId = a.OfertaEducativaId,
                            PeriodoId = a.PeriodoId
                        }).FirstOrDefault();
            }
        }
    }
}
