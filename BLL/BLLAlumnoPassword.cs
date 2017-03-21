using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;
using System.Data.Entity;
namespace BLL
{
    public class BLLAlumnoPassword
    {
        public static DTOAlumnoPassword  GuardarPassword(int AlumnoId, string Pass){
            using (UniversidadEntities db = new UniversidadEntities())
            {
                db.AlumnoPassword.Add(new AlumnoPassword
                {
                    AlumnoId=AlumnoId,
                    Password=Pass
                });
                db.SaveChanges();
                return new DTOAlumnoPassword
                {
                    AlumnoId = db.AlumnoPassword.Local[0].AlumnoId,
                    Password = db.AlumnoPassword.Local[0].Password
                };
            }
        }
        public static DTOAlumnoPassword ConsultarAlumnoPassword(int AlumnoId)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.AlumnoPassword
                        where a.AlumnoId == AlumnoId
                        select new DTOAlumnoPassword
                        {
                            AlumnoId = a.AlumnoId,
                            Password = a.Password
                        }).AsNoTracking().FirstOrDefault();
            }
        }
    }
}
