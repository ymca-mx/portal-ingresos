using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using System.Data.Entity;

namespace BLL.Tools
{
    public class ProcesosMasivos
    {
        //public List<AlumnosNoInscritos> lstAlumnosNo { get; set; }
        //public ProcesosMasivos()
        //{
        //    lstAlumnosNo = new List<AlumnosNoInscritos>();
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 922,
        //        Anio = 2017,
        //        OfertaEducativaId = 25,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 922,
        //        Anio = 2017,
        //        OfertaEducativaId = 25,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 943,
        //        Anio = 2017,
        //        OfertaEducativaId = 25,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 2810,
        //        Anio = 2017,
        //        OfertaEducativaId = 10,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 3218,
        //        Anio = 2017,
        //        OfertaEducativaId = 27,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 4226,
        //        Anio = 2017,
        //        OfertaEducativaId = 16,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 4704,
        //        Anio = 2017,
        //        OfertaEducativaId = 8,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5319,
        //        Anio = 2017,
        //        OfertaEducativaId = 9,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5375,
        //        Anio = 2017,
        //        OfertaEducativaId = 13,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5376,
        //        Anio = 2017,
        //        OfertaEducativaId = 6,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5429,
        //        Anio = 2017,
        //        OfertaEducativaId = 27,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5442,
        //        Anio = 2017,
        //        OfertaEducativaId = 18,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5699,
        //        Anio = 2017,
        //        OfertaEducativaId = 27,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5757,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5805,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5831,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5861,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 5924,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6029,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6032,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6057,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6122,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6172,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6179,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6187,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6188,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6192,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6194,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6202,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6209,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6247,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6276,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6414,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6443,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6488,
        //        Anio = 2017,
        //        OfertaEducativaId = 17,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6496,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6512,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6557,
        //        Anio = 2017,
        //        OfertaEducativaId = 14,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6661,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6673,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6683,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6685,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6712,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6753,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6768,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6770,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6775,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6800,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6801,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6802,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6805,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6806,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6812,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6818,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6827,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6829,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6830,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6835,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6842,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6847,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6860,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6860,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6873,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6876,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6894,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6897,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6906,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6908,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6913,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6924,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6928,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6932,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6938,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6940,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6941,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6947,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6968,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6978,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6982,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6983,
        //        Anio = 2017,
        //        OfertaEducativaId = 3,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6989,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6993,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 6994,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7081,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7083,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7084,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7101,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7102,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7105,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7116,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7125,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7142,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7152,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7203,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7230,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7240,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7258,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7267,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7271,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7275,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7276,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7298,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7301,
        //        Anio = 2017,
        //        OfertaEducativaId = 14,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7305,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7306,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7308,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7310,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7312,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7313,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7316,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7318,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7321,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7335,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7337,
        //        Anio = 2017,
        //        OfertaEducativaId = 7,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7343,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7366,
        //        Anio = 2017,
        //        OfertaEducativaId = 4,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7368,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7370,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7385,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7401,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7402,
        //        Anio = 2017,
        //        OfertaEducativaId = 4,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7405,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7417,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7418,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7424,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7435,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7447,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7458,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7468,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7475,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7476,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7478,
        //        Anio = 2017,
        //        OfertaEducativaId = 3,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7496,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7500,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7508,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7513,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7516,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7517,
        //        Anio = 2017,
        //        OfertaEducativaId = 14,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7518,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7520,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7521,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7537,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7548,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7576,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7578,
        //        Anio = 2017,
        //        OfertaEducativaId = 30,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7582,
        //        Anio = 2017,
        //        OfertaEducativaId = 3,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7584,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7586,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7587,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7589,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7601,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7608,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7609,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7610,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7622,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7623,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7628,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7629,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7646,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7657,
        //        Anio = 2017,
        //        OfertaEducativaId = 26,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7665,
        //        Anio = 2017,
        //        OfertaEducativaId = 5,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7689,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7690,
        //        Anio = 2017,
        //        OfertaEducativaId = 4,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7692,
        //        Anio = 2017,
        //        OfertaEducativaId = 26,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7693,
        //        Anio = 2017,
        //        OfertaEducativaId = 14,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7695,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7697,
        //        Anio = 2017,
        //        OfertaEducativaId = 28,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7699,
        //        Anio = 2017,
        //        OfertaEducativaId = 1,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7702,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7705,
        //        Anio = 2017,
        //        OfertaEducativaId = 30,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7708,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7715,
        //        Anio = 2017,
        //        OfertaEducativaId = 2,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7724,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7725,
        //        Anio = 2017,
        //        OfertaEducativaId = 30,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7727,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7730,
        //        Anio = 2017,
        //        OfertaEducativaId = 29,
        //        PeriodoId = 1
        //    });
        //    lstAlumnosNo.Add(new AlumnosNoInscritos
        //    {
        //        AlumnoId = 7746,
        //        Anio = 2017,
        //        OfertaEducativaId = 6,
        //        PeriodoId = 1
        //    });
        //}
        //public List<string> ProcesarAlumnos()
        //{
        //    using (UniversidadEntities db = new UniversidadEntities())
        //    {
        //        List<string> lstAlumnosError = new List<string>();
        //        lstAlumnosNo.ForEach(a =>
        //        {
        //            try
        //            {
        //                #region AlumnoInscritoDetalle
        //                DTOAlumnoInscritoDetalle objAlumno = db.AlumnoInscritoDetalle.Where(s => s.AlumnoId == a.AlumnoId &&
        //                      s.OfertaEducativaId == a.OfertaEducativaId && s.Anio == a.Anio && s.PeriodoId == a.PeriodoId).Select(s1 => new DTOAlumnoInscritoDetalle
        //                      {
        //                          AlumnoId = s1.AlumnoId,
        //                          Anio = s1.Anio,
        //                          BecaAcademica = s1.BecaAcademica,
        //                          BecaComite = s1.BecaComite,
        //                          BecaSEP = s1.BecaSEP,
        //                          CargosIniciales = s1.CargosIniciales,
        //                          EstatusId = s1.EstatusId,
        //                          FechaBecaAcademica = s1.FechaBecaAcademica,
        //                          FechaBecaComite = s1.FechaBecaComite,
        //                          FechaBecaSEP = s1.FechaBecaSEP,
        //                          FechaCargosIniciales = s1.FechaCargosIniciales,
        //                          FechaInscripcion = s1.FechaInscripcion,
        //                          Inscripcion = s1.Inscripcion,
        //                          NuevoIngreso = s1.NuevoIngreso,
        //                          OfertaEducativaId = s1.OfertaEducativaId,
        //                          PeriodoId = s1.PeriodoId,
        //                          Porcentaje = s1.Porcentaje,
        //                          UsuarioBecaAcademica = s1.UsuarioBecaAcademica,
        //                          UsuarioBecaComite = s1.UsuarioBecaComite,
        //                          UsuarioBecaDeportiva = s1.UsuarioBecaDeportiva,
        //                          UsuarioBecaSEP = s1.UsuarioBecaSEP,
        //                          UsuarioCargosIniciales = s1.UsuarioCargosIniciales,
        //                          UsuarioInscripcion = s1.UsuarioInscripcion,
        //                          BecaDeportiva = s1.BecaDeportiva,
        //                          FechaBecaDeportiva = s1.FechaBecaDeportiva,
        //                          PorcentajeBecaDeportiva = s1.PorcentajeBecaDeportiva
        //                      }).FirstOrDefault();
        //                if (objAlumno == null)
        //                {
        //                    BLLAlumnoInscritoDetalle.Insertar(new DTOAlumnoInscritoDetalle
        //                    {
        //                        AlumnoId = a.AlumnoId,
        //                        OfertaEducativaId = a.OfertaEducativaId,
        //                        Anio = a.Anio,
        //                        PeriodoId = a.PeriodoId,
        //                        NuevoIngreso = false,
        //                        EstatusId = 1,
        //                        CargosIniciales = true,
        //                        UsuarioCargosIniciales = a.AlumnoId
        //                    });
        //                    objAlumno = db.AlumnoInscritoDetalle.Where(s => s.AlumnoId == a.AlumnoId &&
        //                      s.OfertaEducativaId == a.OfertaEducativaId && s.Anio == a.Anio && s.PeriodoId == a.PeriodoId).Select(s1 => new DTOAlumnoInscritoDetalle
        //                      {
        //                          AlumnoId = s1.AlumnoId,
        //                          Anio = s1.Anio,
        //                          BecaAcademica = s1.BecaAcademica,
        //                          BecaComite = s1.BecaComite,
        //                          BecaSEP = s1.BecaSEP,
        //                          CargosIniciales = s1.CargosIniciales,
        //                          EstatusId = s1.EstatusId,
        //                          FechaBecaAcademica = s1.FechaBecaAcademica,
        //                          FechaBecaComite = s1.FechaBecaComite,
        //                          FechaBecaSEP = s1.FechaBecaSEP,
        //                          FechaCargosIniciales = s1.FechaCargosIniciales,
        //                          FechaInscripcion = s1.FechaInscripcion,
        //                          Inscripcion = s1.Inscripcion,
        //                          NuevoIngreso = s1.NuevoIngreso,
        //                          OfertaEducativaId = s1.OfertaEducativaId,
        //                          PeriodoId = s1.PeriodoId,
        //                          Porcentaje = s1.Porcentaje,
        //                          UsuarioBecaAcademica = s1.UsuarioBecaAcademica,
        //                          UsuarioBecaComite = s1.UsuarioBecaComite,
        //                          UsuarioBecaDeportiva = s1.UsuarioBecaDeportiva,
        //                          UsuarioBecaSEP = s1.UsuarioBecaSEP,
        //                          UsuarioCargosIniciales = s1.UsuarioCargosIniciales,
        //                          UsuarioInscripcion = s1.UsuarioInscripcion,
        //                          BecaDeportiva = s1.BecaDeportiva,
        //                          FechaBecaDeportiva = s1.FechaBecaDeportiva,
        //                          PorcentajeBecaDeportiva = s1.PorcentajeBecaDeportiva
        //                      }).FirstOrDefault();
        //                }
        //                #endregion
        //                AlumnoInscrito objAlumnoIn = db.AlumnoInscrito.Where(A => A.AlumnoId == a.AlumnoId && A.OfertaEducativaId == a.OfertaEducativaId).FirstOrDefault();

        //                if (db.AlumnoInscrito.Where(ai => ai.AlumnoId == a.AlumnoId && ai.OfertaEducativaId == a.OfertaEducativaId
        //                 && ai.Anio == a.Anio && ai.PeriodoId == a.PeriodoId).ToList().Count == 0)
        //                {
        //                    #region Mover a Bitacora & Añadir nuevo
        //                    db.AlumnoInscrito.Add(new AlumnoInscrito
        //                    {
        //                        AlumnoId = a.AlumnoId,
        //                        OfertaEducativaId = a.OfertaEducativaId,
        //                        Anio = a.Anio,
        //                        PeriodoId = a.PeriodoId,
        //                        FechaInscripcion = objAlumno.FechaInscripcion,
        //                        PagoPlanId = objAlumnoIn.PagoPlanId,
        //                        TurnoId = objAlumnoIn.TurnoId,
        //                        EsEmpresa = objAlumnoIn.EsEmpresa,
        //                        UsuarioId = objAlumno.UsuarioInscripcion == a.AlumnoId ? objAlumnoIn.UsuarioId : objAlumno.UsuarioInscripcion,
        //                        EstatusId = objAlumnoIn.EstatusId
        //                    });
        //                    db.AlumnoInscritoBitacora.Add(new AlumnoInscritoBitacora
        //                    {
        //                        AlumnoId = objAlumnoIn.AlumnoId,
        //                        Anio = objAlumnoIn.Anio,
        //                        EsEmpresa = objAlumnoIn.EsEmpresa,
        //                        FechaInscripcion = objAlumnoIn.FechaInscripcion,
        //                        OfertaEducativaId = objAlumnoIn.OfertaEducativaId,
        //                        PagoPlanId = (int)objAlumnoIn.PagoPlanId,
        //                        PeriodoId = objAlumnoIn.PeriodoId,
        //                        TurnoId = objAlumnoIn.TurnoId,
        //                        UsuarioId = objAlumnoIn.UsuarioId
        //                    });
        //                    db.AlumnoInscrito.Remove(objAlumnoIn);
        //                    db.SaveChanges();
        //                    #endregion
        //                }
        //                if (objAlumno.BecaAcademica && objAlumno.EstatusId == 3)
        //                {
        //                    #region Becas
        //                    DTO.DTOAlumnoBeca objBeca = new DTO.DTOAlumnoBeca
        //                    {
        //                        alumnoId = objAlumno.AlumnoId,
        //                        anio = objAlumno.Anio,
        //                        esSEP = false,
        //                        ofertaEducativaId = objAlumno.OfertaEducativaId,
        //                        periodoId = objAlumno.PeriodoId,
        //                        porcentajeBeca = objAlumno.Porcentaje,
        //                        usuarioId = objAlumno.UsuarioBecaAcademica,
        //                        estatusid = 1//<<<<<------Falta
        //                    };
        //                    BLLAlumnoInscritoDetalle.InsertarBitacora(new DTOAlumnoInscritoDetalle
        //                    {
        //                        AlumnoId = objAlumno.AlumnoId,
        //                        OfertaEducativaId = objAlumno.OfertaEducativaId,
        //                        Anio = objAlumno.Anio,
        //                        PeriodoId = objAlumno.PeriodoId,
        //                    }, 100000);

        //                    AlumnoInscritoDetalle objDetalle = db.AlumnoInscritoDetalle.Where(q => q.AlumnoId == a.AlumnoId && q.OfertaEducativaId == a.OfertaEducativaId
        //                          && q.Anio == a.Anio && q.PeriodoId == a.PeriodoId).FirstOrDefault();

        //                    objDetalle.EstatusId = 2;
        //                    objDetalle.CargosIniciales = true;
        //                    objDetalle.FechaCargosIniciales = objDetalle.UsuarioCargosIniciales == 0 ? objDetalle.FechaInscripcion : objDetalle.FechaCargosIniciales;
        //                    objDetalle.UsuarioCargosIniciales = objDetalle.UsuarioCargosIniciales == 0 ? a.AlumnoId : objDetalle.UsuarioCargosIniciales;
        //                    db.SaveChanges();

        //                    bool resp = BLL.BLLAlumno.AplicaBecaAlumno(objBeca);
        //                    if (resp)
        //                    {
        //                        BLL.BLLSaldoAFavor.AplicacionSaldoAlumno(objBeca.alumnoId, false, false);
        //                    }
        //                    #endregion
        //                }

        //                lstAlumnosError.Add("Alumnoid:   " + objAlumno.AlumnoId + "   -----   Correcto");
        //            }
        //            catch
        //            {
        //                lstAlumnosError.Add("Alumnoid:   " + a.AlumnoId + "   -----   Error");
        //            }

        //        });

        //        return lstAlumnosError;
        //    }
        //}
    }

    public class AlumnosNoInscritos
    {
        public int AlumnoId { get; set; }
        public int OfertaEducativaId { get; set; }
        public int Anio { get; set; }
        public int PeriodoId { get; set; }
    }
}
