using GestionIntervention.Dtos.Client;

namespace GestionIntervention.Dtos.Intervention
{
    public class InterventionDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Statut { get; set; } = null!;
        public string? Priorite { get; set; }
        public DateTime? DatePlanifiee { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public ClientDto? Client { get; set; }
    }
}
