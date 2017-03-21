using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

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

        public static string GuardarEmpresa(DTOEmpresa objEmpresa)
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                try
                {
                    db.Empresa.Add(new Empresa 
                    {
                        RFC= objEmpresa.RFC,
                        RazonSocial = objEmpresa.RazonSocial,
                        FechaAlta = DateTime.Now,
                        FechaVigencia = objEmpresa.FechaVigencia,
                        UsuarioId = objEmpresa.Usuarioid,

                        EmpresaDetalle = new EmpresaDetalle
                        {

                            Nombre = objEmpresa.EmpresaDetalle.Nombre,
                            Paterno = objEmpresa.EmpresaDetalle.Paterno,
                            Materno = objEmpresa.EmpresaDetalle.Materno,
                            EmailContacto = objEmpresa.EmpresaDetalle.EmailContacto,
                            Telefono = objEmpresa.EmpresaDetalle.Telefono,
                            Celular = objEmpresa.EmpresaDetalle.Celular,
                            PaisId = objEmpresa.EmpresaDetalle.PaisId,
                            EntidadFederativaId = objEmpresa.EmpresaDetalle.EntidadFederativaId,
                            MunicipioId = objEmpresa.EmpresaDetalle.MunicipioId,
                            CP = objEmpresa.EmpresaDetalle.CP,
                            Colonia = objEmpresa.EmpresaDetalle.Colonia,
                            Calle = objEmpresa.EmpresaDetalle.Calle,
                            NoExterior = objEmpresa.EmpresaDetalle.NoExterior,
                            NoInterior = objEmpresa.EmpresaDetalle.NoInterior,
                            Email = objEmpresa.EmpresaDetalle.Email,
                            Observacion = objEmpresa.EmpresaDetalle.Observacion,

                        },

                        DatosFiscales = new DatosFiscales
                        {
                            RFC = objEmpresa.EmpresaDetalle.DatosFiscales.RFC,
                            PaisId = objEmpresa.EmpresaDetalle.DatosFiscales.PaisId,
                            EntidadFederativaId = objEmpresa.EmpresaDetalle.DatosFiscales.EntidadFederativaId,
                            MunicipioId = objEmpresa.EmpresaDetalle.DatosFiscales.MunicipioId,
                            CP = objEmpresa.EmpresaDetalle.DatosFiscales.CP,
                            Colonia = objEmpresa.EmpresaDetalle.DatosFiscales.Colonia,
                            Calle = objEmpresa.EmpresaDetalle.DatosFiscales.Calle,
                            NoExterior = objEmpresa.EmpresaDetalle.DatosFiscales.NoExterior,
                            NoInterior = objEmpresa.EmpresaDetalle.DatosFiscales.NoInterior,
                            Observacion = objEmpresa.EmpresaDetalle.DatosFiscales.Observacion,
                            EsEmpresa = (bool)objEmpresa.EmpresaDetalle.DatosFiscales.EsEmpresa,
                        }

                    });

                    db.SaveChanges();
                    //return db.Empresa.Local[0].EmpresaId;
                    return true.ToString();
                }
                catch (Exception )
                {
                    //return e.Message;
                    return false.ToString();
                }
            }
        }
    }
}
