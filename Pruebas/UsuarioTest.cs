using System;
using System.Linq;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pruebas
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void GetMenu()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                var SubMenus =
                    db.Usuario
                        .Where(a => a.UsuarioId == 100000)
                        .Select(a =>
                            a.UsuarioTipo
                            .TipoUsuarioSubmenu
                            .Select(b => new
                            {
                                b.SubMenu.Descripcion,
                                b.SubMenu.Direccion,
                                b.SubmenuId,
                                b.SubMenu.MenuId
                            }).ToList()
                            ).FirstOrDefault();

                var listMenuId = SubMenus.Select(a => a.MenuId).ToList();

                listMenuId = listMenuId.Distinct().ToList();

                var Menu =
                db.Menu
                .Where(a => listMenuId.Contains(a.MenuId))
                .ToList()
                .AsEnumerable()
                .Select(a => new
                {
                    a.MenuId,
                    a.Descripcion,
                    SubMenu = SubMenus
                            .Where(b => b.MenuId == a.MenuId)
                            .Select(b => new
                            {
                                b.Descripcion,
                                b.Direccion,
                                b.MenuId,
                                b.SubmenuId
                            })
                            .ToList()
                })
                .ToList()
                .OrderBy(a => a.MenuId);
            }
        }

        [TestMethod]
        public void GetReport()
        {
            using (UniversidadEntities db = new UniversidadEntities())
            {
                string Query = "";

                #region Query
                Query += "begin  " +
"begin try  " +
"DROP TABLE #Pagos20182;   " +
"end try  " +
"begin catch  " +
    "print 'table does not exist'  " +
"end catch  " +
"select  " +
    "a.AlumnoId,   " +
    "a.PagoId,   " +
    "a.OfertaEducativaId,   " +
    "a.Anio,   " +
    "a.PeriodoId,   " +
    "b.PagoConceptoId,   " +
    "a.Cuota,   " +
    "a.Promesa,  " +
    "a.EstatusId  " +
    "into #Pagos20182  " +
"from Pago a  " +
"inner  " +
"join Cuota b on a.cuotaid = b.cuotaId and a.anio = b.anio and a.periodoid = b.periodoid  " +
"inner join PagoConcepto c on c.pagoconceptoid = b.PagoConceptoId and c.ofertaeducativaid = b.ofertaEducativaid  " +
"inner join OfertaEducativa d on a.OfertaEducativaId = d.OfertaEducativaId  " +
"where a.anio = 2018 and a.periodoid = 2  " +
"and c.PagoConceptoId in (800, 802)  " +
"and d.OfertaEducativaTipoId in(1, 2, 3)  " +
"and a.EstatusId != 2  " +
"order by AlumnoId  " +
"end  " +


"begin  " +
"begin try  " +
  "DROP TABLE #Pagos20183;   " +
"end try  " +
"begin catch  " +
    "print 'table does not exist'  " +
"end catch  " +

"select  " +
    "a.AlumnoId,   " +
    "a.PagoId,   " +
    "a.OfertaEducativaId,   " +
    "a.Anio,   " +
    "a.PeriodoId,   " +
    "b.PagoConceptoId,   " +
    "a.Cuota,   " +
    "a.Promesa,  " +
    "a.EstatusId  " +
    "into #Pagos20183  " +
"from Pago a  " +
"inner join Cuota b on a.cuotaid = b.cuotaId and a.anio = b.anio and a.periodoid = b.periodoid  " +
"inner join PagoConcepto c on c.pagoconceptoid = b.PagoConceptoId and c.ofertaeducativaid = b.ofertaEducativaid  " +
"inner join OfertaEducativa d on a.OfertaEducativaId = d.OfertaEducativaId  " +
"where a.anio = 2018 and a.periodoid = 3  " +
"and c.PagoConceptoId in (800, 802)  " +
"and d.OfertaEducativaTipoId in(1, 2, 3)  " +
"and a.EstatusId != 2  " +
"order by AlumnoId  " +
"end  " +


"begin  " +



"declare @AlumnoAmbos table(Id int identity, AlumnoId int, OfertaEducativaId int)  " +

"insert into @AlumnoAmbos  " +
"Select a.AlumnoId, a.OfertaEducativaId from #Pagos20183 a  " +
"Inner join  " +
    "(Select AlumnoId, OfertaEducativaId from #Pagos20182  " +
        "where Cuota != Promesa  " +
        "group by AlumnoId, OfertaEducativaId) b  " +
        "on a.AlumnoId = b.AlumnoId and a.OfertaEducativaId = b.OfertaEducativaId  " +
"where Cuota != Promesa  " +
"group by a.AlumnoId, a.OfertaEducativaId  " +
"order by a.AlumnoId  " +

"declare @AlumnoDetalle  " +
"table(AlumnoId int,  " +
        "PagoConceptoId int,  " +
        "OfertaEducativaId int,  " +
        "PagoIdAnterior int,  " +
        "AnioAnterior int,  " +
        "PeriodoIdAnterior int,  " +
        "SubPeriodoIdAnterior int,  " +
        "PorcentajeAnterior decimal(18, 2),  " +
        "PagoIdActual int,  " +
        "AnioActual int,  " +
        "PeriodoIdActual int,  " +
        "SubPeriodoIdActual int,  " +
        "PorcentajeActual decimal(18, 2))  " +

"insert into @AlumnoDetalle  " +
"select AlumnoId, 802, OfertaEducativaId,null,2018,2,null,null,null,2018,3,null,null from @AlumnoAmbos  " +

"insert into @AlumnoDetalle  " +
"select AlumnoId, 800, OfertaEducativaId,null,2018,2,1,null,null,2018,3,1,null from @AlumnoAmbos  " +

"insert into @AlumnoDetalle  " +
"select AlumnoId, 800, OfertaEducativaId,null,2018,2,2,null,null,2018,3,2,null from @AlumnoAmbos  " +

"insert into @AlumnoDetalle  " +
"select AlumnoId, 800, OfertaEducativaId,null,2018,2,3,null,null,2018,3,3,null from @AlumnoAmbos  " +

"insert into @AlumnoDetalle  " +
"select AlumnoId, 800, OfertaEducativaId,null,2018,2,4,null,null,2018,3,4,null from @AlumnoAmbos  " +

"declare @ind int = 1, @max int = (select max(Id) from @AlumnoAmbos)  " +

"while @ind <= @max  " +
"begin  " +

    "declare @AlumnoId int, @OfertaEducativaId int  " +

    "select @AlumnoId = AlumnoId, @OfertaEducativaId = OfertaEducativaId from @AlumnoAmbos  " +


    "where Id = @ind  " +


    "Update a  " +
        "set a.PagoIdAnterior = p.PagoId,  " +
            "a.SubPeriodoIdAnterior = p.SubPeriodoId,  " +
            "a.PorcentajeAnterior = (100 - ((p.Promesa * 100) / p.Cuota))  " +
    "from @AlumnoDetalle a  " +
    "inner join Pago P on a.AlumnoId = P.AlumnoId and a.OfertaEducativaId = P.OfertaEducativaId  " +
    "inner join Cuota c on p.CuotaId = c.CuotaId and a.PagoConceptoId = c.PagoConceptoId  " +
    "where P.Anio = 2018 and p.PeriodoId = 2 and p.EstatusId != 2 and  " +
        "a.AlumnoId = @AlumnoId and a.OfertaEducativaId = @OfertaEducativaId and a.PagoConceptoId = 802  " +

    "Update a  " +
        "set a.PagoIdActual = p1.PagoId,  " +
            "a.SubPeriodoIdActual = p1.SubPeriodoId,  " +
            "a.PorcentajeActual = (100 - ((p1.Promesa * 100) / p1.Cuota))  " +
    "from @AlumnoDetalle a  " +
    "inner    join Pago P1 on a.AlumnoId = P1.AlumnoId and a.OfertaEducativaId = P1.OfertaEducativaId  " +
    "inner join Cuota c1 on p1.CuotaId = c1.CuotaId and a.PagoConceptoId = c1.PagoConceptoId  " +
    "where P1.Anio = 2018 and P1.PeriodoId = 3 and p1.EstatusId != 2 and  " +
        "a.AlumnoId = @AlumnoId and a.OfertaEducativaId = @OfertaEducativaId and a.PagoConceptoId = 802  " +


    "declare @subind int= 1  " +


    "while @subind <= 4  " +

    "begin  " +
        "Update a  " +
        "set a.PagoIdAnterior = p.PagoId,  " +
            "a.SubPeriodoIdAnterior = p.SubPeriodoId,  " +
            "a.PorcentajeAnterior = (100 - ((p.Promesa * 100) / p.Cuota))  " +
        "from @AlumnoDetalle a  " +

        "inner join Pago P on a.AlumnoId = P.AlumnoId and a.OfertaEducativaId = P.OfertaEducativaId  " +
        "inner join Cuota c on p.CuotaId = c.CuotaId and a.PagoConceptoId = c.PagoConceptoId  " +
        "where P.Anio = 2018 and p.PeriodoId = 2 and P.SubPeriodoId = @subind and p.EstatusId != 2 and  " +
            "a.AlumnoId = @AlumnoId and a.OfertaEducativaId = @OfertaEducativaId  " +
            "and a.PagoConceptoId = 800 and a.SubPeriodoIdAnterior = @subind  " +


        "Update a  " +
        "set a.PagoIdActual = p1.PagoId,  " +
            "a.SubPeriodoIdActual = p1.SubPeriodoId,  " +
            "a.PorcentajeActual = (100 - ((p1.Promesa * 100) / p1.Cuota))  " +
        "from @AlumnoDetalle a  " +
        "inner join Pago P1 on a.AlumnoId = P1.AlumnoId and a.OfertaEducativaId = P1.OfertaEducativaId  " +
        "inner join Cuota c1 on p1.CuotaId = c1.CuotaId and a.PagoConceptoId = c1.PagoConceptoId  " +
        "where P1.Anio = 2018 and P1.PeriodoId = 3 and P1.SubPeriodoId = @subind and p1.EstatusId != 2 and  " +
            "a.AlumnoId = @AlumnoId and a.OfertaEducativaId = @OfertaEducativaId  " +
            "and a.PagoConceptoId = 800 and a.SubPeriodoIdAnterior = @subind  " +


    "set @subind+= 1  " +

    "end  " +

"set @ind += 1  " +
"end  " +

"Select a.AlumnoId,   " +
    "p.Descripcion,  " +
    "b.Descripcion OfertaEducativa,  " +
    "'2018 - 2' Periodo,  " +
    "man.Descripcion Mes,  " +
    "a.PorcentajeAnterior Beca,  " +
    "'2018 - 3' Periodo,  " +
    "ma.Descripcion Mes,  " +
    "a.PorcentajeActual Beca  " +
"from @AlumnoDetalle a  " +
"inner join OfertaEducativa b on a.OfertaEducativaId = b.OfertaEducativaId  " +
"inner join SubPeriodo ca on a.PeriodoIdActual = ca.PeriodoId and a.SubPeriodoIdActual = ca.SubperiodoId  " +
"inner join SubPeriodo can on a.PeriodoIdAnterior = can.PeriodoId and a.SubPeriodoIdAnterior = can.SubperiodoId  " +
"inner join Mes ma on ca.MesId = ma.MesId  " +
"inner join Mes man on ca.MesId = man.MesId  " +
"inner join PagoConcepto p on a.PagoConceptoId = p.PagoConceptoId and a.OfertaEducativaId = p.OfertaEducativaId  " +
"order by AlumnoId , a.PagoConceptoId desc  " +

"end";
                #endregion

                var Result = db.Database.SqlQuery<object>(Query).ToList();
            }
        }
    }
}
