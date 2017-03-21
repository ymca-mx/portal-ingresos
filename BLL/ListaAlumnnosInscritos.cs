using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ListaAlumnnosInscritos
    {
        public List<DTO.Alumno.Beca.DTOAlumnoBeca> lista = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();

        public ListaAlumnnosInscritos()
        {
            lista.Add(new
                 DTO.Alumno.Beca.DTOAlumnoBeca
            {

                alumnoId = 7572,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeBeca = decimal.Parse("	0.00"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new
                           DTO.Alumno.Beca.DTOAlumnoBeca
            {

                alumnoId = 7449,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	50.00"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new
                           DTO.Alumno.Beca.DTOAlumnoBeca
            {

                alumnoId = 5699,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 27,
                porcentajeBeca = decimal.Parse("	60.00"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new
                           DTO.Alumno.Beca.DTOAlumnoBeca
            {

                alumnoId = 7640,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	50.00"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new
                           DTO.Alumno.Beca.DTOAlumnoBeca
            {

                alumnoId = 5876,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 29,
                porcentajeBeca = decimal.Parse("	50.00"),
                esSEP = true,
                usuarioId = 100000
            });
            lista.Add(new
                           DTO.Alumno.Beca.DTOAlumnoBeca
            {

                alumnoId = 5964,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 29,
                porcentajeBeca = decimal.Parse("	50.00"),
                esSEP = false,
                usuarioId = 100000
            });
        }

        public static List<DTO.Alumno.Beca.DTOAlumnoBeca> ListaAlumnosEmpresa20163()
        {
            List<DTO.Alumno.Beca.DTOAlumnoBeca> lista = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();

            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 12,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 20,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 471,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 3228,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 3313,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 3914,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 3942,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 4037,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 4220,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 4788,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 13,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	61.49	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 5168,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6142,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6144,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6145,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6148,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6152,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6153,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6154,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6156,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6159,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6301,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6302,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6303,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6306,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6313,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6314,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6315,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6360,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6364,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6411,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6418,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6447,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6448,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6502,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6554,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6558,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6565,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6567,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6616,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6624,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6626,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6627,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6628,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6629,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6630,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6634,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6636,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6643,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6644,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6645,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6648,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6651,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6654,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6662,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6691,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	90.68	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6692,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	90.68	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6699,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	90.68	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6750,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6750,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6761,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	74.42	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6779,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6853,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6861,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6862,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6866,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6867,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6870,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6872,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6874,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6878,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6884,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6885,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6889,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6912,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6914,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6915,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6921,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6922,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6924,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6959,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6966,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6969,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6984,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6991,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6998,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeInscripcion = decimal.Parse("	10.95	"),
                porcentajeBeca = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7001,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7020,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7021,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7023,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7024,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7025,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7026,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7027,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7031,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7032,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7033,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7038,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7039,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7042,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7044,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7046,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7049,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7056,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7057,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7058,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7059,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7060,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7061,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7082,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7171,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7186,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7187,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7188,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7189,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7190,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7191,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7207,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7210,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7217,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7219,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7220,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7221,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7223,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7225,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7226,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7230,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7232,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	68.54	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7234,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7236,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7237,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7250,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7252,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7277,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7278,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7280,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7285,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7293,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7294,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7295,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7297,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7332,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7344,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7376,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7403,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7410,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7413,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7422,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7430,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7440,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7460,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7463,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7488,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7490,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7504,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7505,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7528,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7532,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7533,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7535,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7541,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7544,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7546,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7551,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7556,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7565,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	59.55	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7569,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7570,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7574,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	55.06	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7575,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7578,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7589,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7592,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7617,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7632,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7637,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7639,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7643,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7644,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7646,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7647,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeInscripcion = decimal.Parse("	0.00	"),
                porcentajeBeca = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7653,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7655,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 30,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	51.47	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7659,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7660,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7661,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7662,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7666,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7668,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7671,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7672,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7673,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7676,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeInscripcion = decimal.Parse("	100.00	"),
                porcentajeBeca = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });

            return lista;
        }
        public static List<DTO.Alumno.Beca.DTOAlumnoBeca> ListaAlumnosEmpresa20162()
        {
            List<DTO.Alumno.Beca.DTOAlumnoBeca> lista = new List<DTO.Alumno.Beca.DTOAlumnoBeca>();
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 3228,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 5168,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6142,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6144,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6145,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6148,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6152,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6153,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6154,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6156,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6159,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6301,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6302,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6303,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6306,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6313,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6314,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6315,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6360,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6364,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6411,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6418,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6447,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6448,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6502,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6554,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6565,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6616,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6634,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6636,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6643,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6644,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6645,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6648,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6651,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6654,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6662,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6761,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	74.42	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6779,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	64.05	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6853,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6861,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6862,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6866,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6867,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6870,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6872,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6874,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6878,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6884,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6885,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6889,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6912,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6914,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6915,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 3,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6921,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 2,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6922,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6924,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6959,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6966,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6969,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6984,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6991,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 6998,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 15,
                porcentajeBeca = decimal.Parse("	10.95	"),
                porcentajeInscripcion = decimal.Parse("	64.95	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7023,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7024,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7025,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7026,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7027,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7042,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7044,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7046,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 28,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.53	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7171,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 21,
                porcentajeBeca = decimal.Parse("	100.00	"),
                porcentajeInscripcion = decimal.Parse("	77.64	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7403,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7410,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7413,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7440,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7463,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7488,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7490,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7505,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7544,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7551,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7556,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 14,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7589,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7617,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7639,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7644,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7646,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 5,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });
            lista.Add(new DTO.Alumno.Beca.DTOAlumnoBeca
            {
                alumnoId = 7647,
                anio = 2016,
                periodoId = 3,
                ofertaEducativaId = 4,
                porcentajeBeca = decimal.Parse("	0.00	"),
                porcentajeInscripcion = decimal.Parse("	62.88	"),
                esSEP = false,
                usuarioId = 100000
            });

            return lista;
        }
    }
}
