using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Utilities;

namespace BLL
{
    public class BLLBiblioteca
    {
        public static JObject AddComunicado(string Asunto, int UsuarioId, string NombreDocumento)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    var Periodo = db.Periodo
                        .Where(per => DateTime.Now >= per.FechaInicial
                                    && DateTime.Now <= per.FechaFinal)
                        .Select(per => new
                        {
                            per.Anio,
                            per.PeriodoId
                        }).FirstOrDefault();


                    db.Comunicado.Add(new DAL.Comunicado
                    {
                        Anio = Periodo.Anio,
                        Asunto = Asunto,
                        Documento = NombreDocumento,
                        Fecha = DateTime.Now,
                        PeriodoId = Periodo.PeriodoId,
                        UsuarioId = UsuarioId
                    });

                    db.SaveChanges();

                    return db.Comunicado
                                .Local
                                .Select(a => JObject.FromObject(new
                                {
                                    a.Anio,
                                    a.Asunto,
                                    a.ComunicadoId,
                                    a.Documento,
                                    a.UsuarioId,
                                    a.PeriodoId
                                })).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
        }

        public static object SendComunicado(JObject jComunicado, string Documento)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var objComunicado = new
                {
                    Anio = (int)jComunicado["Anio"],
                    Asunto = (string)jComunicado["Asunto"],
                    ComunicadoId = (int)jComunicado["ComunicadoId"],
                    UsuarioId = (int)jComunicado["UsuarioId"],
                    PeriodoId = (int)jComunicado["PeriodoId"],
                    Documento = (string)jComunicado["Documento"],
                };
                DTO.DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();

                #region Load List
                var listAlumnos = db.Alumno
                                            .Where(x => x.AlumnoInscrito
                                                            .Where(al => al.OfertaEducativaId != 43
                                                                    && al.Anio == objComunicado.Anio
                                                                    && al.PeriodoId == objComunicado.PeriodoId)
                                                            .ToList()
                                                            .Count > 0
                                                         && x.EstatusId == 1
                                                         && x.AlumnoDetalle.Email.Length > 5)
                                             .Select(x => new
                                             {
                                                 x.AlumnoId,
                                                 x.Nombre,
                                                 x.Materno,
                                                 x.Paterno,
                                                 x.AlumnoDetalle.Email
                                             })
                                             .ToList();

                var listDocentes = db.Docente
                                        .Join(db.DocenteDetalle,
                                                Docente => Docente.DocenteId,
                                                DocDeta => DocDeta.DocenteId,
                                                (Docente, DocDeta) => new { Docente, DocenteDetalle = DocDeta })
                                        .Select(d => new
                                        {
                                            d.Docente.DocenteId,
                                            d.Docente.Nombre,
                                            d.Docente.Paterno,
                                            d.Docente.Materno,
                                            d.DocenteDetalle.Email
                                        }).ToList();

                var listUsuarios = db.Usuario
                                    .Where(us => us.UsuarioId == objComunicado.UsuarioId
                                                || us.UsuarioId==100000)
                                    .Select(us => new
                                    {
                                        us.UsuarioId,
                                        us.Nombre,
                                        us.Paterno,
                                        us.Materno,
                                        us.UsuarioDetalle.Email
                                    }).ToList();
                #endregion

                #region SendAlumnos
                listAlumnos.ForEach(alumno =>
                {
                    try
                    {
                        ProcessResult envio = Utilities.Email.Enviar(objCuenta.Email, objCuenta.Password, "Universidad YMCA", alumno.Email, ',', "", ',', objComunicado.Asunto, "", Documento, ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true);
                        if (envio.Estatus)
                        {
                            db.ComunicadoUsuario.Add(new ComunicadoUsuario
                            {
                                ComunicadoId = objComunicado.ComunicadoId,
                                UsuarioId = alumno.AlumnoId,
                                UsuarioTipoId = 2,
                                Fallido = false,
                                Mensaje = ""
                            });
                        }
                        else
                        {
                            db.ComunicadoUsuario.Add(new ComunicadoUsuario
                            {
                                ComunicadoId = objComunicado.ComunicadoId,
                                UsuarioId = alumno.AlumnoId,
                                UsuarioTipoId = 2,
                                Fallido = true,
                                Mensaje = envio.Mensaje
                            });
                        }
                    }
                    catch (Exception error) {
                        db.ComunicadoUsuario.Add(new ComunicadoUsuario
                        {
                            ComunicadoId = objComunicado.ComunicadoId,
                            UsuarioId = alumno.AlumnoId,
                            UsuarioTipoId = 2,
                            Fallido = true,
                            Mensaje = error.Message
                        });
                    }
                });
                #endregion

                #region Docentes
                listDocentes.ForEach(doce =>
                {
                    try
                    {
                        ProcessResult envio = Utilities.Email.Enviar(objCuenta.Email, objCuenta.Password, "Universidad YMCA", doce.Email, ',', "", ',', objComunicado.Asunto, "", Documento, ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true);
                        if (envio.Estatus)
                        {

                            db.ComunicadoUsuario.Add(new ComunicadoUsuario
                            {
                                ComunicadoId = objComunicado.ComunicadoId,
                                UsuarioId = doce.DocenteId,
                                UsuarioTipoId = 27,
                                Fallido = false,
                                Mensaje = ""
                            });
                        }
                        else
                        {
                            db.ComunicadoUsuario.Add(new ComunicadoUsuario
                            {
                                ComunicadoId = objComunicado.ComunicadoId,
                                UsuarioId = doce.DocenteId,
                                UsuarioTipoId = 27,
                                Fallido = true,
                                Mensaje = envio.Mensaje
                            });
                        }
                    }
                    catch (Exception error)
                    {
                        db.ComunicadoUsuario.Add(new ComunicadoUsuario
                        {
                            ComunicadoId = objComunicado.ComunicadoId,
                            UsuarioId = doce.DocenteId,
                            UsuarioTipoId = 27,
                            Fallido = true,
                            Mensaje = error.Message
                        });
                    }
                });
                #endregion

                #region Usuarios
                listUsuarios.ForEach(usu =>
                {
                    try
                    {
                        ProcessResult envio = Utilities.Email.Enviar(objCuenta.Email, objCuenta.Password, "Universidad YMCA", usu.Email, ',', "", ',', objComunicado.Asunto, "", Documento, ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true);
                        if (envio.Estatus)
                        {

                            db.ComunicadoUsuario.Add(new ComunicadoUsuario
                            {
                                ComunicadoId = objComunicado.ComunicadoId,
                                UsuarioId = usu.UsuarioId,
                                UsuarioTipoId = 1,
                                Fallido = false,
                                Mensaje = ""
                            });
                        }
                        else
                        {
                            db.ComunicadoUsuario.Add(new ComunicadoUsuario
                            {
                                ComunicadoId = objComunicado.ComunicadoId,
                                UsuarioId = usu.UsuarioId,
                                UsuarioTipoId = 1,
                                Fallido = true,
                                Mensaje = envio.Mensaje
                            });
                        }
                    }
                    catch (Exception error)
                    {
                        db.ComunicadoUsuario.Add(new ComunicadoUsuario
                        {
                            ComunicadoId = objComunicado.ComunicadoId,
                            UsuarioId = usu.UsuarioId,
                            UsuarioTipoId = 1,
                            Fallido = true,
                            Mensaje = error.Message
                        });
                    }
                });
                #endregion


                db.SaveChanges();

                return 
                db.ComunicadoUsuario
                    .Where(a => a.ComunicadoId==objComunicado.ComunicadoId
                                && a.Fallido)
                    .Select(a => new
                    {
                        a.UsuarioId,
                        TipoUsuario = a.UsuarioTipo.Descripcion,
                        Error=a.Mensaje
                    }).ToList();
            }

        }

        public static object ReEnviarComunicado(int comunicadoId, string Patch)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    DTO.DTOCuentaMail objCuenta = BLLCuentaMail.ConsultarCuentaMail();

                    var objComunicado = db.Comunicado
                                        .Where(co => co.ComunicadoId == comunicadoId)
                                        .FirstOrDefault();

                    objComunicado.ComunicadoUsuario
                            .Where(com => com.Fallido)
                            .ToList()
                            .ForEach(comu =>
                            {
                                string Email = "";

                                switch (comu.UsuarioTipoId)
                                {
                                    case 1:
                                        Email = db.Usuario
                                                    .Where(usu => usu.UsuarioId == comu.UsuarioId)
                                                    .Select(usu =>
                                                        usu.UsuarioDetalle.Email
                                                    ).FirstOrDefault();
                                        break;
                                    case 2:
                                        Email = db.AlumnoDetalle
                                                    .Where(usu => usu.AlumnoId == comu.UsuarioId)
                                                    .Select(usu =>
                                                        usu.Email
                                                   ).FirstOrDefault();
                                        break;
                                    case 27:
                                        Email = db.DocenteDetalle
                                                    .Where(usu => usu.DocenteId == comu.UsuarioId)
                                                    .Select(usu =>
                                                        usu.Email
                                                    ).FirstOrDefault();
                                        break;
                                }

                                string DocumentoRuta = Patch + objComunicado.ComunicadoId + "_" + objComunicado.Anio + "_" + objComunicado.PeriodoId + ".pdf";

                                try
                                {
                                    if (Email.Length > 0)
                                    {
                                        ProcessResult envio = Utilities.Email.Enviar(objCuenta.Email, objCuenta.Password, "Universidad YMCA", Email, ',', "", ',', objComunicado.Asunto, "", DocumentoRuta, ',', objCuenta.Smtp, objCuenta.Puerto, objCuenta.SSL, true);

                                        comu.Fallido = !envio.Estatus ? true : false;
                                        comu.Mensaje = !envio.Estatus ? envio.Mensaje : "";
                                    }
                                    else
                                    {
                                        comu.Fallido = true;
                                        comu.Mensaje = "No tiene Email en el sistema.";
                                    }
                                }
                                catch (Exception error)
                                {
                                    comu.Fallido = true;
                                    comu.Mensaje = error.Message;
                                }
                            });

                    db.SaveChanges();

                    return true;
                }
                catch(Exception Error) { return new { Error }; }
            }
        }

        public static object GetHistorial()
        {
            using(UniversidadEntities db = new UniversidadEntities())
            {
                return db.Comunicado
                        .Select(a => new
                        {
                            a.ComunicadoId,
                            a.Documento,
                            Fecha = (a.Fecha.Day < 10 ? "0" + a.Fecha.Day : "" + a.Fecha.Day) + "/" +
                                (a.Fecha.Month < 10 ? "0" + a.Fecha.Month : "" + a.Fecha.Month) + "/" + a.Fecha.Year,
                            Periodo = a.Anio + "-" + a.PeriodoId,
                            a.Asunto,
                            Usuario = a.Usuario.Nombre + " " + a.Usuario.Paterno + " " + a.Usuario.Materno,
                            Fallidos = a.ComunicadoUsuario
                                            .Where(b => b.Fallido)
                                            .Select(b => new
                                            {
                                                b.UsuarioId,
                                                b.Mensaje,
                                                Nombre = b.UsuarioTipoId == 2 ?
                                                            (db.Alumno.Where(al => al.AlumnoId == b.UsuarioId).Select(al => al.Nombre + " " + al.Paterno + " " + al.Materno).FirstOrDefault()) :
                                                         b.UsuarioTipoId == 27 ? (db.Docente.Where(al => al.DocenteId == b.UsuarioId).Select(al => al.Nombre + " " + al.Paterno + " " + al.Materno).FirstOrDefault()) :
                                                         (db.Usuario.Where(al => al.UsuarioId == b.UsuarioId).Select(al => al.Nombre + " " + al.Paterno + " " + al.Materno).FirstOrDefault()),
                                                TipoUsuario = b.UsuarioTipo.Descripcion
                                            })
                        })
                        .ToList();
            }
        }

        
    }
}
