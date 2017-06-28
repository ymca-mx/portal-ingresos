using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using BLL;
using System.Globalization;

namespace Pruebas
{
    [TestClass]
    public class Alumno
    {
        static CultureInfo Cultura = CultureInfo.CreateSpecificCulture("es-MX");

        [TestMethod]
        public void BuscarAlumnoBeca()
        {
            var obj=
            BLL.BLLAlumnoPortal.BuscarAlumno(8182, 3);

            Console.WriteLine(obj.AlumnoId + " " + obj.Nombre + " ");
        }

        [TestMethod]
        public void DescuentosBeca()
        {
            BLL.BLLDescuentos.TraerDescuentos(5669, 29);
        }

        [TestMethod]
        public void PruebaDatosAlumo()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var AlumnoId = 5669;
                

                var datos = db.Alumno.Where(a => a.AlumnoId == AlumnoId)
                                    .Select(b => new DTOAlumnoDatos
                                    {
                                        AlumnoId = b.AlumnoId,
                                        Nombre = b.Nombre,
                                        Paterno = b.Paterno,
                                        Materno = b.Materno,
                                        FechaNacimiento = b.AlumnoDetalle.FechaNacimiento,
                                        GeneroId = b.AlumnoDetalle.GeneroId,
                                        CURP = b.AlumnoDetalle.CURP,
                                        PaisId = b.AlumnoDetalle.PaisId,
                                        EntidadNacimientoId = b.AlumnoDetalle.EntidadNacimientoId,
                                    }).FirstOrDefault();

                datos.FechaNacimientoC = datos.FechaNacimiento.ToString("dd/MM/yyyy", Cultura);

                datos.DatosContacto = new List<DTOAlumnoDatos2>();

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Estado Civil",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EstadoCivilId.ToString() ?? "",
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Correo Electrónico",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Email.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Teléfono Celular",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Celular.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Teléfono Casa",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.TelefonoCasa.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Calle",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Calle.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Número Exterior",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoExterior.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Numero Interior",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.NoInterior.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Código Postal",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.CP.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Colonia",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.Colonia.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Estado",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.EntidadFederativaId.ToString() ?? ""
                }
                );

                datos.DatosContacto.Add(new DTOAlumnoDatos2
                {
                    Dato = "Delegación | Municipio",
                    Alumno = db.AlumnoDetalleAlumno.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? "",
                    Coordinador = db.AlumnoDetalleCoordinador.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? "",
                    ServiciosEscolares = db.AlumnoDetalle.Where(a => a.AlumnoId == AlumnoId).FirstOrDefault()?.MunicipioId.ToString() ?? ""
                }
                );

            }
        }

        [TestMethod]
        public void CambioCarrera()
        {
            DTOAlumnoCambioCarrera Cambio = new DTOAlumnoCambioCarrera
            {
                AlumnoId = 8053,
                OfertaEducativaIdActual = 12,
                OfertaEducativaIdNueva= 11,
                Anio=2017,
                PeriodoId = 3,
                Observaciones = "Por error relaciones publicas",
                UsuarioId =  8289
            };
           

            BLLAlumnoPortal.AplicarCambioCarrera(Cambio);

        }

        [TestMethod]
        public void GenerarMatricula()
        {
            Console.WriteLine("LA matricula del alumno 7584 es : ------- ");
            Console.Write(
            Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
            {
                Anio = 2016,
                PeriodoId = 1,
                TurnoId = 1
            },
            new DTOOfertaEducativa
            {
                Rvoe = "20100446"
            },
            7584));
        }

        [TestMethod]
        public void MatriculaMasiva()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                List<DAL.Alumno> lstAlum = db.Alumno.
                                        Where(a => a.MatriculaId.Contains("0000"))
                                        //Where(a=> a.AlumnoId== 732)
                                        .ToList();

                List<string> MatriculasN = new List<string>();
                lstAlum.ForEach(k =>
                {
                    var ali = k.AlumnoInscrito
                                        .Where(o => o.OfertaEducativa.OfertaEducativaTipoId != 4 && o.OfertaEducativaId != 43)
                                        .ToList()
                                        .OrderBy(i => i.Anio)
                                            .ThenBy(i => i.PeriodoId)
                                            .ThenBy(i => i.FechaInscripcion)
                                            .ThenBy(i => i.HoraInscripcion)
                                        .ToList();
                    if (ali.Count>0)
                    {
                        var ult = ali.LastOrDefault();
                        string MAtric =
                        Herramientas.Matricula.ObtenerMatricula(new DTOAlumnoInscrito
                        {
                            Anio = k.Anio,
                            PeriodoId = k.PeriodoId,
                            TurnoId = ult.TurnoId
                        },
                       new DTOOfertaEducativa
                       {
                           Rvoe = ult.OfertaEducativa.Rvoe
                       },
                       k.AlumnoId);

                        MatriculasN.Add("AlumnoId: " + k.AlumnoId + "- Matricula: " + MAtric);
                        k.MatriculaId = MAtric;
                    }
                });

                //MatriculasN.ForEach(l => Console.WriteLine(l));
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void BuscarFiltro()
        {

            using (UniversidadEntities db = new UniversidadEntities())
            {
                string Cadena = "Lopez GARCÍA JULIO";
                string[] varios = Cadena.Split(' ');
                varios = varios.Where(c => c != " ")
               .ToArray();

                List<DAL.Alumno> alumnos = (from a in db.Alumno
                                            where varios.Contains(a.Nombre) && varios.Contains(a.Paterno) && varios.Contains(a.Materno)
                                            select a)
                                              .ToList();

            }
        }

    }
}
