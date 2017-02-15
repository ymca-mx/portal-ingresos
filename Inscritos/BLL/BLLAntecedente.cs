using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data.Entity;

namespace BLL
{
    public class BLLAntecedente
    {
        public static bool GuardarAntecendente(DTOAlumnoAntecendente objAntecedente)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    AlumnoAntecedente objVer = db.AlumnoAntecedente
                                                .Where(a => a.AlumnoId == objAntecedente.AlumnoId
                                                         && a.AntecedenteTipoId == objAntecedente.AntecedenteTipoId).FirstOrDefault();
                    if (objVer != null)
                    {
                        db.AlumnoAntecedenteBitacora.Add(new AlumnoAntecedenteBitacora
                        {
                            AlumnoId = objVer.AlumnoId,
                            AntecedenteTipoId = objVer.AntecedenteTipoId,
                            Anio = objVer.Anio,
                            AreaAcademicaId = objVer.AreaAcademicaId,
                            EntidadFederativaId = objVer.EntidadFederativaId,
                            EscuelaEquivalencia = objVer.EscuelaEquivalencia,
                            EsEquivalencia = objVer.EsEquivalencia,
                            EsTitulado = objVer.EsTitulado,
                            FechaBitacora = DateTime.Now,
                            FechaRegistro = objVer.FechaRegistro,
                            HoraBitacora = DateTime.Now.TimeOfDay,
                            MedioDifusionId = objVer.MedioDifusionId,
                            MesId = objVer.MesId,
                            PaisId = objVer.PaisId,
                            Procedencia = objVer.Procedencia,
                            Promedio = objVer.Promedio,
                            TitulacionMedio = objVer.TitulacionMedio,
                            UsuarioId = objVer.UsuarioId,
                            UsuarioIdBitacora = objAntecedente.UsuarioId
                        });

                        objVer.Procedencia = objAntecedente.Procedencia;
                        objVer.Promedio = objAntecedente.Promedio;
                        objVer.Anio = objAntecedente.Anio;
                        objVer.MesId = objAntecedente.MesId;
                        objVer.EsEquivalencia = objAntecedente.EsEquivalencia;
                        objVer.EscuelaEquivalencia = objAntecedente.EscuelaEquivalencia;
                        objVer.PaisId = objAntecedente.PaisId;
                        objVer.EntidadFederativaId = objAntecedente.PaisId == 146 ? objAntecedente.EntidadFederativaId : 33;
                        objVer.EsTitulado = objAntecedente.EsTitulado;
                        objVer.TitulacionMedio = objAntecedente.TitulacionMedio;
                        objVer.MedioDifusionId = objAntecedente.MedioDifusionId;
                        objVer.UsuarioId = objAntecedente.UsuarioId;

                    }
                    else
                    {
                        db.AlumnoAntecedente.Add(new AlumnoAntecedente
                        {
                            AlumnoId = objAntecedente.AlumnoId,
                            Anio = objAntecedente.Anio,
                            AntecedenteTipoId = objAntecedente.AntecedenteTipoId,
                            AreaAcademicaId = objAntecedente.AreaAcademicaId,
                            EntidadFederativaId = objAntecedente.EntidadFederativaId,
                            EscuelaEquivalencia = objAntecedente.EscuelaEquivalencia,
                            EsEquivalencia = objAntecedente.EsEquivalencia,
                            EsTitulado = objAntecedente.EsTitulado,
                            FechaRegistro = DateTime.Now,
                            MedioDifusionId = objAntecedente.MedioDifusionId,
                            MesId = objAntecedente.MesId,
                            PaisId = objAntecedente.PaisId,
                            Procedencia = objAntecedente.Procedencia,
                            Promedio = objAntecedente.Promedio,
                            TitulacionMedio = objAntecedente.TitulacionMedio,
                            UsuarioId = objAntecedente.UsuarioId
                        });
                    }

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
