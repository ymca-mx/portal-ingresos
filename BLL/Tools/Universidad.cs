using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace BLL
{

    public class Convertidor
    {
        public static DTOAlumno ToDTOAlumno(Alumno objAlumno)
        {
            return new DTOAlumno
            {
                AlumnoId = objAlumno.AlumnoId,
                Materno = objAlumno.Materno,
                Nombre = objAlumno.Nombre,
                Paterno = objAlumno.Paterno,
                UsuarioId = objAlumno.UsuarioId
            };
        }
        public static DTOAlumnoDescuento ToDTOAlumnoDescuento(AlumnoDescuento objAlumnoDescuento)
        {
            return new DTOAlumnoDescuento
            {
                AlumnoDescuentoId = objAlumnoDescuento.AlumnoDescuentoId,
                AlumnoId = objAlumnoDescuento.AlumnoId,
                Anio = objAlumnoDescuento.Anio,
                ConceptoId = objAlumnoDescuento.PagoConceptoId,
                DescuentoId = objAlumnoDescuento.DescuentoId,
                EstatusId = objAlumnoDescuento.EstatusId,
                Monto = objAlumnoDescuento.Monto,
                OfertaEducativaId = objAlumnoDescuento.OfertaEducativaId,
                PeriodoId = objAlumnoDescuento.PeriodoId
            };
        }
        public static Alumno ToAlumno(DTOAlumno objAlumno)
        {
            return new Alumno
            {
                AlumnoId = objAlumno.AlumnoId,
                Materno = objAlumno.Materno,
                Nombre = objAlumno.Nombre,
                Paterno = objAlumno.Paterno,
                UsuarioId = objAlumno.UsuarioId
            };
        }
        public static DTOAlumnoInscrito ToDTOAlumnoInscrito(AlumnoInscrito objAlumno)
        {
            return new DTOAlumnoInscrito
            {
                OfertaEducativaId = objAlumno.OfertaEducativaId,
                OfertaEducativa = new DTOOfertaEducativa
                {
                    OfertaEducativaId = objAlumno.OfertaEducativa.OfertaEducativaId,
                    OfertaEducativaTipoId = objAlumno.OfertaEducativa.OfertaEducativaTipoId,
                    Descripcion = objAlumno.OfertaEducativa.Descripcion,
                    Rvoe = objAlumno.OfertaEducativa.Rvoe,
                    FechaRvoe = objAlumno.OfertaEducativa.FechaRvoe.ToString(),
                    EstatusId = objAlumno.OfertaEducativa.EstatusId,
                    SucursalId = objAlumno.OfertaEducativa.SucursalId
                },
                Anio = objAlumno.Anio,
                PeriodoId = objAlumno.PeriodoId,
                AlumnoId = objAlumno.AlumnoId,
                PagoPlanId = objAlumno.PagoPlanId,
                FechaInscripcion = objAlumno.FechaInscripcion,
                TurnoId = objAlumno.TurnoId,
                UsuarioId = objAlumno.UsuarioId
            };
        }
        public static DTOOfertaEducativa ToDTOOfertaEducativa(OfertaEducativa objOFerta)
        {
            return new DTOOfertaEducativa
            {
                OfertaEducativaId = objOFerta.OfertaEducativaId,
                OfertaEducativaTipoId = objOFerta.OfertaEducativaTipoId,
                Descripcion = objOFerta.Descripcion,
                Rvoe = objOFerta.Rvoe,
                FechaRvoe = objOFerta.FechaRvoe.ToString(),
                EstatusId = objOFerta.EstatusId
            };
        }
        public static DTOPeriodo ToDTOPeriodo(Periodo objPeriodo)
        {
            return new DTOPeriodo
            {
                PeriodoId = objPeriodo.PeriodoId,
                Anio = objPeriodo.Anio,
                Descripcion = objPeriodo.Descripcion,
                FechaInicial = objPeriodo.FechaInicial,
                FechaFinal = objPeriodo.FechaFinal,
                Meses = objPeriodo.Meses,
                _FechaFinal = (objPeriodo.FechaFinal.Day.ToString().Length < 2 ? "0" + objPeriodo.FechaFinal.Day.ToString() : objPeriodo.FechaFinal.Day.ToString()) + "/" +
                (objPeriodo.FechaFinal.Month.ToString().Length < 2 ? "0" + objPeriodo.FechaFinal.Month.ToString() : objPeriodo.FechaFinal.Month.ToString()) + "/" + objPeriodo.FechaFinal.Year,

                _FechaInicial = (objPeriodo.FechaInicial.Day.ToString().Length < 2 ? "0" + objPeriodo.FechaInicial.Day.ToString() : objPeriodo.FechaInicial.Day.ToString()) + "/" +
                (objPeriodo.FechaInicial.Month.ToString().Length < 2 ? "0" + objPeriodo.FechaInicial.Month.ToString() : objPeriodo.FechaInicial.Month.ToString()) + "/" + objPeriodo.FechaInicial.Year
            };
        }
        public static DTOCuota ToDTOCuota(Cuota objCuota)
        {
            return new DTOCuota
            {
                CuotaId = objCuota.CuotaId,
                Anio = objCuota.Anio,
                PeriodoId = objCuota.PeriodoId,
                OfertaEducativaId = objCuota.OfertaEducativaId,
                PagoConceptoId = objCuota.PagoConceptoId,
                Monto = objCuota.Monto
            };
        }
        public static DTODescuentos ToDTODescuentos(Descuento objDescuento)
        {
            return new DTODescuentos
            {
                DescuentoId = objDescuento.DescuentoId,
                PagoConceptoId = objDescuento.PagoConceptoId,
                DescuentoTipoId = objDescuento.DescuentoTipoId,
                OfertaEducativaId = objDescuento.OfertaEducativaId,
                MontoMaximo = objDescuento.MontoMaximo,
                Descripcion = objDescuento.Descripcion
            };
        }
    }

    public class BLLVarios
    {
        public static List<DTO.Varios.DTOEstatus> EstatusActivos()
        {
            return new List<DTO.Varios.DTOEstatus> {
                new DTO.Varios.DTOEstatus { estatusId = 1, descripcion = "Activo"},
                new DTO.Varios.DTOEstatus { estatusId = 4, descripcion = "Pagado"},
                new DTO.Varios.DTOEstatus { estatusId = 13, descripcion = "Pago Parcial"},
                new DTO.Varios.DTOEstatus { estatusId = 14, descripcion = "Pagado Parcial"}
            };
        }
    }
}