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
    public class BLLEmpresa
    {
        public static List<DTOEmpresa> ListaEmpresas()
        {
            using(UniversidadEntities db= new UniversidadEntities())
            {
                return (from a in db.Empresa
                        select new DTOEmpresa
                        {
                            RazonSocial = a.RazonSocial,
                            EmpresaId = a.EmpresaId,
                            RFC = a.RFC,
                            FechaAltaS = (a.FechaAlta.Day.ToString().Length < 2 ? "0" + a.FechaAlta.Day.ToString() : a.FechaAlta.Day.ToString()) + "/" +
                                                      (a.FechaAlta.Month.ToString().Length < 2 ? "0" + a.FechaAlta.Month.ToString() : a.FechaAlta.Month.ToString()) + "/" +
                                                      a.FechaAlta.Year.ToString()
                        }).ToList();
            }
        }

        public static List<DTOEmpresaLigera> ListarEmpresaLigera()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                return (from a in db.Empresa
                        select new DTOEmpresaLigera
                        {
                            EmpresaId = a.EmpresaId,
                            RFC = a.RFC,
                            RazonSocial = a.RazonSocial,
                            Grupo = db.Grupo.Where(b => b.EmpresaId == a.EmpresaId)
                                            .Select(c => new DTOGrupoLigero 
                                            {
                                                GrupoId = c.GrupoId,
                                                Descripcion = c.Descripcion,
                                                NumeroDePagos = c.NumeroPagos
                                            }).ToList()
                        }).ToList();
            }
        }

        public static string GuardarEmpresa(DTOEmpresa empresa)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    db.Empresa.Add(new Empresa 
                    {
                        RFC= empresa.RFC,
                        RazonSocial = empresa.RazonSocial,
                        FechaAlta = DateTime.Now,
                        FechaVigencia = DateTime.ParseExact(empresa.FechaV, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        UsuarioId = empresa.Usuarioid,
                        EmpresaDetalle = new EmpresaDetalle
                        {
                            Nombre = empresa.EmpresaDetalle.Nombre,
                            Paterno = empresa.EmpresaDetalle.Paterno,
                            Materno = empresa.EmpresaDetalle.Materno,
                            EmailContacto = empresa.EmpresaDetalle.EmailContacto,
                            Telefono = empresa.EmpresaDetalle.Telefono,
                            Celular = empresa.EmpresaDetalle.Celular,
                            PaisId = empresa.EmpresaDetalle.PaisId,
                            EntidadFederativaId = empresa.EmpresaDetalle.EntidadFederativaId,
                            MunicipioId = empresa.EmpresaDetalle.MunicipioId,
                            CP = empresa.EmpresaDetalle.CP,
                            Colonia = empresa.EmpresaDetalle.Colonia,
                            Calle = empresa.EmpresaDetalle.Calle,
                            NoExterior = empresa.EmpresaDetalle.NoExterior,
                            NoInterior = empresa.EmpresaDetalle.NoInterior,
                            Email = empresa.EmpresaDetalle.Email,
                            Observacion = empresa.EmpresaDetalle.Observacion,
                        },
                        DatosFiscales = new DatosFiscales
                        {
                            RFC = empresa.EmpresaDetalle.DatosFiscales.RFC,
                            PaisId = empresa.EmpresaDetalle.DatosFiscales.PaisId,
                            EntidadFederativaId = empresa.EmpresaDetalle.DatosFiscales.EntidadFederativaId,
                            MunicipioId = empresa.EmpresaDetalle.DatosFiscales.MunicipioId,
                            CP = empresa.EmpresaDetalle.DatosFiscales.CP,
                            Colonia = empresa.EmpresaDetalle.DatosFiscales.Colonia,
                            Calle = empresa.EmpresaDetalle.DatosFiscales.Calle,
                            NoExterior = empresa.EmpresaDetalle.DatosFiscales.NoExterior,
                            NoInterior = empresa.EmpresaDetalle.DatosFiscales.NoInterior,
                            Observacion = empresa.EmpresaDetalle.DatosFiscales.Observacion,
                            EsEmpresa = true
                        }

                    });

                    db.SaveChanges();
                    return true.ToString();
                }
                catch (Exception Ex)
                {
                    return false.ToString();
                }
            }
        }
    }
}
