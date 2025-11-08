using GestionIntervention.Dtos.Intervention;

namespace GestionIntervention.Dtos.Rapport
{
    public class RapportDto
    {
        public int Id { get; set; }
        public int InterventionId { get; set; }
        public DateTime DateRapport { get; set; }
        public string Client { get; set; } = null!;
        public List<string> Intervenant { get; set; } = new List<string>();
        public string TypeIntervention { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Observations { get; set; }
        public string? TravauxEffectues { get; set; }
        public string? HeureDebut { get; set; }
        public string? HeureFin { get; set; }

        // Relations
        public InterventionDto? Intervention { get; set; }
    }
}
