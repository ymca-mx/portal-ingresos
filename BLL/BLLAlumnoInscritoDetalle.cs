using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class BLLAlumnoInscritoDetalle2
    {
        //public static bool Insertar(DTOAlumnoInscritoDetalle objAlumno)
        //{
        //    using(UniversidadEntities db= new UniversidadEntities())
        //    {
        //        try
        //        {
        //            db.AlumnoInscritoDetalle.Add(new AlumnoInscritoDetalle
        //            {
        //                AlumnoId = objAlumno.AlumnoId,
        //                OfertaEducativaId = objAlumno.OfertaEducativaId,
        //                Anio = objAlumno.Anio,
        //                PeriodoId = objAlumno.PeriodoId,
        //                NuevoIngreso = objAlumno.NuevoIngreso,
        //                EstatusId = objAlumno.EstatusId,
        //                CargosIniciales = objAlumno.CargosIniciales,
        //                UsuarioCargosIniciales = objAlumno.UsuarioCargosIniciales,
        //                FechaCargosIniciales = objAlumno.FechaCargosIniciales,
        //                FechaAdeudoBiblioteca = objAlumno.FechaAdeudoBiblioteca,
        //                FechaBecaComite = objAlumno.FechaBecaComite,
        //                FechaBecaAcademica = objAlumno.FechaBecaAcademica,
        //                FechaBecaDeportiva = objAlumno.FechaBecaDeportiva,
        //                FechaBecaSEP = objAlumno.FechaBecaSEP,
        //                FechaFinanciamiento = objAlumno.FechaFinanciamiento,
        //                FechaInscripcion = objAlumno.FechaInscripcion,
        //                FechaPagare = objAlumno.FechaPagare,
        //            });

        //            db.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}
        //public static bool CargosIniciales(DTOAlumnoInscritoDetalle objAlumno)
        //{
        //    using(UniversidadEntities db= new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlumno.AlumnoId &&
        //            a.OfertaEducativaId == objAlumno.OfertaEducativaId
        //             && a.Anio == objAlumno.Anio && a.PeriodoId == objAlumno.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            objAlumnoD.CargosIniciales = objAlumno.CargosIniciales;
        //            objAlumnoD.UsuarioCargosIniciales= objAlumno.UsuarioCargosIniciales;
        //            objAlumnoD.FechaCargosIniciales = DateTime.Now;
        //            objAlumnoD.EstatusId = objAlumno.EstatusId;

        //            db.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}
        //public static bool Inscripcion(DTOAlumnoInscritoDetalle objAlumno)
        //{
        //    using(UniversidadEntities db= new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlumno.AlumnoId && 
        //            a.OfertaEducativaId == objAlumno.OfertaEducativaId
        //             && a.Anio == objAlumno.Anio && a.PeriodoId == objAlumno.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            objAlumnoD.Inscripcion = objAlumno.Inscripcion;
        //            objAlumnoD.UsuarioInscripcion = objAlumno.UsuarioInscripcion;
        //            objAlumnoD.FechaInscripcion = DateTime.Now;
        //            objAlumnoD.EstatusId = objAlumno.EstatusId;

        //            db.SaveChanges();
        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public static bool BecaAcademica(DTOAlumnoInscritoDetalle objAlumno)
        //{
        //    using(UniversidadEntities db= new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlumno.AlumnoId &&
        //           a.OfertaEducativaId == objAlumno.OfertaEducativaId
        //            && a.Anio == objAlumno.Anio && a.PeriodoId == objAlumno.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            objAlumnoD.BecaAcademica = objAlumno.BecaAcademica;
        //            objAlumnoD.Porcentaje = objAlumno.Porcentaje;
        //            objAlumnoD.UsuarioBecaAcademica = objAlumno.UsuarioBecaAcademica;
        //            objAlumnoD.FechaBecaAcademica = DateTime.Now;
        //            objAlumnoD.BecaSEP = objAlumno.BecaSEP;
        //            objAlumnoD.BecaComite = objAlumno.BecaComite;

        //            db.SaveChanges();

        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public static bool BecaSEP(DTOAlumnoInscritoDetalle objAlSEP)
        //{
        //    using(UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlSEP.AlumnoId &&
        //         a.OfertaEducativaId == objAlSEP.OfertaEducativaId
        //          && a.Anio == objAlSEP.Anio && a.PeriodoId == objAlSEP.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            objAlumnoD.Porcentaje = objAlSEP.Porcentaje;
        //            objAlumnoD.BecaSEP = objAlSEP.BecaSEP;
        //            objAlumnoD.UsuarioBecaSEP = objAlSEP.UsuarioBecaSEP;
        //            objAlumnoD.FechaBecaSEP = DateTime.Now;
        //            objAlumnoD.BecaAcademica = objAlSEP.BecaAcademica;
        //            objAlumnoD.BecaComite = objAlSEP.BecaComite;

        //            db.SaveChanges();

        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public static bool BecaComite(DTOAlumnoInscritoDetalle objAlComite)
        //{
        //    using(UniversidadEntities db= new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlComite.AlumnoId &&
        //        a.OfertaEducativaId == objAlComite.OfertaEducativaId
        //         && a.Anio == objAlComite.Anio && a.PeriodoId == objAlComite.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            objAlumnoD.Porcentaje = objAlComite.Porcentaje;
        //            objAlumnoD.BecaComite = objAlComite.BecaComite;
        //            objAlumnoD.UsuarioBecaComite = objAlComite.UsuarioBecaComite;
        //            objAlumnoD.FechaBecaComite = DateTime.Now;
        //            objAlumnoD.BecaSEP = objAlComite.BecaSEP;
        //            objAlumnoD.BecaAcademica = objAlComite.BecaAcademica;

        //            db.SaveChanges();

        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public static bool BecaDeportiva(DTOAlumnoInscritoDetalle objAlDep)
        //{
        //    using(UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlDep.AlumnoId &&
        //        a.OfertaEducativaId == objAlDep.OfertaEducativaId
        //         && a.Anio == objAlDep.Anio && a.PeriodoId == objAlDep.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            objAlumnoD.BecaDeportiva = objAlDep.BecaDeportiva;
        //            objAlumnoD.PorcentajeBecaDeportiva = objAlDep.PorcentajeBecaDeportiva;
        //            objAlumnoD.UsuarioBecaDeportiva = objAlDep.UsuarioBecaDeportiva;
        //            //objAlumnoD.FechaBecaDeportiva = DateTime.Now;

        //            db.SaveChanges();

        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public static DTOAlumnoInscritoDetalle TraerAlumnoInscritoDetalle(DTOAlumnoInscritoDetalle objBuscar)
        //{
        //    using(UniversidadEntities db= new UniversidadEntities())
        //    {
        //        return (from a in db.AlumnoInscritoDetalle
        //                                               where a.AlumnoId == objBuscar.AlumnoId &&
        //                                               a.OfertaEducativaId == objBuscar.OfertaEducativaId
        //                                               && a.Anio == objBuscar.Anio && a.PeriodoId == objBuscar.PeriodoId
        //                                               select new DTOAlumnoInscritoDetalle
        //                                               {
        //                                                   AdeudoBiblioteca = a.AdeudoBiblioteca,
        //                                                   AlumnoId = a.AlumnoId,
        //                                                   Anio = a.Anio,
        //                                                   BecaAcademica = a.BecaAcademica,
        //                                                   BecaComite = a.BecaComite,
        //                                                   BecaDeportiva = a.BecaDeportiva,
        //                                                   BecaSEP = a.BecaSEP,
        //                                                   CargosIniciales = a.CargosIniciales,
        //                                                   EstatusId = a.EstatusId,
        //                                                   FechaAdeudoBiblioteca = a.FechaAdeudoBiblioteca,
        //                                                   FechaBecaAcademica = a.FechaBecaAcademica,
        //                                                   FechaBecaComite = a.FechaBecaComite,
        //                                                   FechaBecaDeportiva = a.FechaBecaDeportiva,
        //                                                   FechaBecaSEP = a.FechaBecaSEP,
        //                                                   FechaCargosIniciales = a.FechaCargosIniciales,
        //                                                   FechaFinanciamiento = a.FechaFinanciamiento,
        //                                                   FechaInscripcion = a.FechaInscripcion,
        //                                                   FechaPagare = a.FechaPagare,
        //                                                   Financiamiento = a.Financiamiento,
        //                                                   Inscripcion = a.Inscripcion,
        //                                                   NuevoIngreso = a.NuevoIngreso,
        //                                                   OfertaEducativaId = a.OfertaEducativaId,
        //                                                   Pagare = a.Pagare,
        //                                                   PeriodoId = a.PeriodoId,
        //                                                   Porcentaje = a.Porcentaje,
        //                                                   PorcentajeBecaDeportiva = a.PorcentajeBecaDeportiva,
        //                                                   UsuarioAdeudoBiblioteca = a.UsuarioAdeudoBiblioteca,
        //                                                   UsuarioBecaAcademica = a.UsuarioBecaAcademica,
        //                                                   UsuarioBecaComite = a.UsuarioBecaComite,
        //                                                   UsuarioBecaDeportiva = a.UsuarioBecaDeportiva,
        //                                                   UsuarioBecaSEP = a.UsuarioBecaSEP,
        //                                                   UsuarioCargosIniciales = a.UsuarioCargosIniciales,
        //                                                   UsuarioFinanciamiento = a.UsuarioFinanciamiento,
        //                                                   UsuarioInscripcion = a.UsuarioInscripcion,
        //                                                   UsuarioPagare = a.UsuarioPagare
        //                                               }).FirstOrDefault();
                
        //    }
        //}

        //public static bool InsertarBitacora(DTOAlumnoInscritoDetalle objAlumnoB,int UsuarioId)
        //{
        //    using(UniversidadEntities db = new UniversidadEntities())
        //    {
        //        try
        //        {
        //            AlumnoInscritoDetalle objAlumnoD = db.AlumnoInscritoDetalle.Where(a => a.AlumnoId == objAlumnoB.AlumnoId &&
        //      a.OfertaEducativaId == objAlumnoB.OfertaEducativaId
        //       && a.Anio == objAlumnoB.Anio && a.PeriodoId == objAlumnoB.PeriodoId).FirstOrDefault();

        //            if (objAlumnoD == null) return false;

        //            db.AlumnoInscritoDetalleBitacora.Add(new AlumnoInscritoDetalleBitacora
        //            {
        //                AdeudoBiblioteca = objAlumnoD.AdeudoBiblioteca,
        //                AlumnoId = objAlumnoD.AlumnoId,
        //                Anio = objAlumnoD.Anio,
        //                BecaAcademica = objAlumnoD.BecaAcademica,
        //                BecaComite = objAlumnoD.BecaComite,
        //                BecaDeportiva = objAlumnoD.BecaDeportiva,
        //                BecaSEP = objAlumnoD.BecaSEP,
        //                CargosIniciales = objAlumnoD.CargosIniciales,
        //                EstatusId = objAlumnoD.EstatusId,
        //                FechaAdeudoBiblioteca = objAlumnoD.FechaAdeudoBiblioteca,
        //                FechaBecaAcademica = objAlumnoD.FechaBecaAcademica,
        //                FechaBecaComite = objAlumnoD.FechaBecaComite,
        //                FechaBecaDeportiva = objAlumnoD.FechaBecaDeportiva,
        //                FechaBecaSEP = objAlumnoD.FechaBecaSEP,
        //                FechaCargosIniciales = objAlumnoD.FechaCargosIniciales,
        //                FechaFinanciamiento = objAlumnoD.FechaFinanciamiento,
        //                FechaInscripcion = objAlumnoD.FechaInscripcion,
        //                FechaPagare = objAlumnoD.FechaPagare,
        //                Financiamiento = objAlumnoD.Financiamiento,
        //                Inscripcion = objAlumnoD.Inscripcion,
        //                NuevoIngreso = objAlumnoD.NuevoIngreso,
        //                OfertaEducativaId = objAlumnoD.OfertaEducativaId,
        //                Pagare = objAlumnoD.Pagare,
        //                PeriodoId = objAlumnoD.PeriodoId,
        //                Porcentaje = objAlumnoD.Porcentaje,
        //                PorcentajeBecaDeportiva = objAlumnoD.PorcentajeBecaDeportiva,
        //                UsuarioAdeudoBiblioteca = objAlumnoD.UsuarioAdeudoBiblioteca,
        //                UsuarioBecaAcademica = objAlumnoD.UsuarioBecaAcademica,
        //                UsuarioBecaComite = objAlumnoD.UsuarioBecaComite,
        //                UsuarioBecaDeportiva = objAlumnoD.UsuarioBecaDeportiva,
        //                UsuarioBecaSEP = objAlumnoD.UsuarioBecaSEP,
        //                UsuarioCargosIniciales = objAlumnoD.UsuarioCargosIniciales,
        //                UsuarioFinanciamiento = objAlumnoD.UsuarioFinanciamiento,
        //                UsuarioInscripcion = objAlumnoD.UsuarioInscripcion,
        //                UsuarioPagare = objAlumnoD.UsuarioPagare,
        //                FechaBitacota = DateTime.Now,
        //                UsuarioBitacora = UsuarioId
        //            });
        //            //db.AlumnoInscritoDetalle.Local.Clear();
        //            db.SaveChanges();

        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        
    }
}
