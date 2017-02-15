using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BLLAlumnoInscritoDocumento
    {
        public static string GuardarDocumentos(List<DTOAlumnInscritoDocumento> LstDocumentos)
        {
            using(UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    LstDocumentos.ForEach(a =>
                    {
                        db.AlumnoInscritoDocumento.Add(
                            new AlumnoInscritoDocumento
                            {
                                AlumnoId = a.AlumnoId,
                                Anio = a.Anio,
                                OfertaEducativaId = a.OfertaEducativaId,
                                PeriodoId = a.PeriodoId,
                                TipoDocumento = a.TipoDocumento,
                                Archivo = a.Archivo,
                                UsuarioDocumento = a.UsuarioDocumento,
                                FechaDocumento = DateTime.Now
                            });
                    });

                    db.SaveChanges();

                    return "Guardado";
                }
                catch
                {
                    return "Fallo";
                }
            }
        }
    }
}