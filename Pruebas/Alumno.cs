using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
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
            BLL.BLLAlumno.BuscarAlumno(8182, 3);

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
    }
}
