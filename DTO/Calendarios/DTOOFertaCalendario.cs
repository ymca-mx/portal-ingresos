namespace DTO
{
    public class DTOOFertaCalendario
    {
        int OFertaCalendarioId { get; set; }
        int CalendarioEscolarId { get; set; }
        int OfertaEducativaId { get; set; }
        DTOOfertaEducativa OFertaEducativa { get; set; }
    }
}
