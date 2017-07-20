using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Globalization;

namespace BLL
{
    public class BLLCalendarioEscolar
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        public static List<DTOCalendarioEscolar> TraerCalendarios()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                List<DTOCalendarioEscolar> ListaCalendarios = db.CalendarioEscolar
                                                                .Select(c => new DTOCalendarioEscolar
                                                                {
                                                                    CalendarioEscolarId = c.CalendarioEscolarId,
                                                                    Direccion = c.Direccion,
                                                                    FechaAlta = (c.FechaAlta.Day < 10 ? "0" + c.FechaAlta.Day : "" + c.FechaAlta.Day) + "/" +
                                                                                (c.FechaAlta.Month < 10 ? "0" + c.FechaAlta.Month : "" + c.FechaAlta.Month) + "/" + c.FechaAlta.Year,
                                                                    HoraAlta = c.HoraAlta.Hours + ":" + c.HoraAlta.Minutes,
                                                                    EstatusId = c.EstatusId,
                                                                    Nombre = c.Nombre,
                                                                    UsuarioId = c.UsuarioId,
                                                                    UsuarioNombre = c.Usuario.Nombre,
                                                                    OfertasCalendario = c.OfertaCalendario.Select(of =>
                                                                     new DTOOFertaCalendario
                                                                     {
                                                                         CalendarioEscolarId = of.CalendarioEscolarId,
                                                                         OFertaCalendarioId = of.OfertaCalendarioId,
                                                                         OfertaEducativaId = of.OfertaEducativaId,
                                                                         OFertaEducativa = new DTOOfertaEducativa
                                                                         {
                                                                             OfertaEducativaId = of.OfertaEducativa.OfertaEducativaId,
                                                                             Descripcion = of.OfertaEducativa.Descripcion
                                                                         }
                                                                     }).ToList()
                                                                }).ToList();
                return ListaCalendarios;
            }
        }

        public static int NuevoCalendario(DTOCalendarioEscolar Calendario)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    db.CalendarioEscolar.Add(
                            new CalendarioEscolar
                            {
                                Direccion = Calendario.Direccion,
                                EstatusId = Calendario.EstatusId,
                                FechaAlta = DateTime.Now,
                                HoraAlta = DateTime.Now.TimeOfDay,
                                Nombre = Calendario.Nombre,
                                UsuarioId = Calendario.UsuarioId
                            });
                    db.SaveChanges();
                    return db.CalendarioEscolar.Local.FirstOrDefault()?.CalendarioEscolarId ?? 0;
                }
                catch { return 0; }
            }
        }

        public static bool ModificarCalendario(DTOCalendarioEscolar Calendario)
        {
            using(UniversidadEntities db=new UniversidadEntities())
            {
                try
                {
                    CalendarioEscolar Calendariodb = db.CalendarioEscolar.Where(a =>
                                                  a.CalendarioEscolarId == Calendario.CalendarioEscolarId)
                                                .FirstOrDefault();

                    if ((Calendariodb?.CalendarioEscolarId ?? 0) == 0) { return false; }

                    Calendariodb.Direccion = Calendario.Direccion;
                    Calendariodb.Nombre = Calendario.Nombre;
                    Calendariodb.UsuarioId = Calendario.UsuarioId;
                    Calendariodb.EstatusId = Calendario.EstatusId;

                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }
    }
}
