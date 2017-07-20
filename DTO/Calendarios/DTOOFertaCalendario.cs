namespace DTO
{
    public class DTOOFertaCalendario
    {
        public int OFertaCalendarioId { get; set; }
        public int CalendarioEscolarId { get; set; }
        public int OfertaEducativaId { get; set; }
        public DTOOfertaEducativa OFertaEducativa { get; set; }
    }
}
