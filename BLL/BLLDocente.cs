using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BLLDocente
    {
        public static List<DTODocenteActualizar> ListaDocentesActualizar()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    return db.Docente
                                                            .Select(a =>
                                                                new DTODocenteActualizar
                                                                {
                                                                    DocenteId = a.DocenteId,
                                                                    Materno = a.Materno,
                                                                    Nombre = a.Nombre,
                                                                    Paterno = a.Paterno,
                                                                    Actualizaciones =
                                                                        a.DocenteActualizacion.Select(b =>
                                                                            new DTODocenteActualizacion
                                                                            {
                                                                                ActualizacionId = b.ActualizacionId,
                                                                                DocenteActualizacionId = b.DocenteActualizacionId,
                                                                                DocenteId = b.DocenteId,
                                                                                EsCurso = b.EsCurso,
                                                                                DocenteCurso = b.EsCurso ?
                                                                                    new DTODocenteCurso
                                                                                    {
                                                                                        Descripcion = b.DocenteCurso.Descripcion,
                                                                                        DocenteCursoId = b.DocenteCurso.DocenteCursoId,
                                                                                        Duracion = b.DocenteCurso.Duracion,
                                                                                        EsCursoYMCA = b.DocenteCurso.EsCursoYMCA,
                                                                                        FechaFinal = (b.DocenteCurso.FechaFinal.Day < 10 ?
                                                                                                    "0" + b.DocenteCurso.FechaFinal.Day : "" + b.DocenteCurso.FechaFinal.Day) + "/" +
                                                                                                    (b.DocenteCurso.FechaFinal.Month < 10 ?
                                                                                                    "0" + b.DocenteCurso.FechaFinal.Month : "" + b.DocenteCurso.FechaFinal.Month) + "/" + b.DocenteCurso.FechaFinal.Year,
                                                                                        FechaInicial = (b.DocenteCurso.FechaInicial.Day < 10 ?
                                                                                                    "0" + b.DocenteCurso.FechaInicial.Day : "" + b.DocenteCurso.FechaInicial.Day) + "/" +
                                                                                                    (b.DocenteCurso.FechaInicial.Month < 10 ?
                                                                                                    "0" + b.DocenteCurso.FechaInicial.Month : "" + b.DocenteCurso.FechaInicial.Month) + "/" + b.DocenteCurso.FechaInicial.Year,
                                                                                        Institucion = b.DocenteCurso.Institucion,
                                                                                        VoBo = b.DocenteCurso.VoBo
                                                                                    }
                                                                                    : null,
                                                                                DocenteEstudio = b.EsCurso ?
                                                                                    null
                                                                                    : new DTODocenteEstudio
                                                                                    {
                                                                                        Carrera = b.DocenteEstudio.Carrera,
                                                                                        Cedula = b.DocenteEstudio.Cedula,
                                                                                        DocenteId = b.DocenteId,
                                                                                        EstatusId = b.DocenteEstudio.EstatusId,
                                                                                        EstudioId = b.DocenteEstudio.EstudioId,
                                                                                        Fecha = (b.DocenteEstudio.Fecha.Value.Day < 10 ?
                                                                                                    "0" + b.DocenteEstudio.Fecha.Value.Day : "" + b.DocenteEstudio.Fecha.Value.Day) + "/" +
                                                                                                    (b.DocenteEstudio.Fecha.Value.Month < 10 ?
                                                                                                    "0" + b.DocenteEstudio.Fecha.Value.Month : "" + b.DocenteEstudio.Fecha.Value.Month) + "/" + b.DocenteEstudio.Fecha.Value.Year,
                                                                                        Hora = (b.DocenteEstudio.Hora.Value.Hours < 10 ? "0" + b.DocenteEstudio.Hora.Value.Hours : "" + b.DocenteEstudio.Hora.Value.Hours) + ":" +
                                                                                                (b.DocenteEstudio.Hora.Value.Minutes < 10 ? "0" + b.DocenteEstudio.Hora.Value.Minutes : "" + b.DocenteEstudio.Hora.Value.Minutes),
                                                                                        Institucion = b.DocenteEstudio.Institucion,
                                                                                        OfertaEducativaTipoId = b.DocenteEstudio.OfertaEducativaTipoId,
                                                                                        Titulo = b.DocenteEstudio.Titulo,
                                                                                        UsuarioId = b.DocenteEstudio.UsuarioId,
                                                                                    }
                                                                            }).ToList()
                                                                }).ToList();
                }
                catch { return null; }
            }
        }
    }
}
