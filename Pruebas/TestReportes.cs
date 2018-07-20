using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pruebas
{
    [TestClass]
    public class TestReportes
    {
        [TestMethod]
        public void AntiguedadSaldos()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {

                List<SP_ReporteSaldoAlumno_Result> alumnoSaldo0 = db.SP_ReporteSaldoAlumno().ToList();

                var periodos0 = alumnoSaldo0.Select(a => a.Descripcion).Distinct().ToList();
                var ofertas = alumnoSaldo0.Select(a => a.OfertaEducativaId).Distinct().ToList();

                var TipoOfertas = db.OfertaEducativa
                                    .Where(a => ofertas.Contains(a.OfertaEducativaId))
                                    .Select(b => new
                                    {
                                        b.OfertaEducativaTipoId,
                                        b.OfertaEducativaTipo.Descripcion
                                    })
                                    .Distinct()
                                    .ToList()
                                    .Select(a => new
                                    {
                                        a.OfertaEducativaTipoId,
                                        a.Descripcion,
                                        OfertasEducativas = db.OfertaEducativa
                                                            .Where(b => ofertas.Contains(b.OfertaEducativaId) && b.OfertaEducativaTipoId== a.OfertaEducativaTipoId)
                                                            .Select(Ab => new
                                                            {
                                                                Ab.OfertaEducativaId,
                                                                Ab.Descripcion
                                                            })
                                                            .ToList()
                                    })
                                    .ToList();

            }
        }
    }
}
