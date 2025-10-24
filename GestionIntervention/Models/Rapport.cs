using System;

namespace GestionIntervention.Models
{
    public class Rapport
    {
        public int Id { get; set; }
        public int InterventionId { get; set; }
        public DateTime DateRapport { get; set; } = DateTime.Now;
        public string Client { get; set; } = null!;
        public string Intervenant { get; set; } = null!;
        public string TypeIntervention { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Observations { get; set; }
        public string? TravauxEffectues { get; set; }

        public Intervention? Intervention { get; set; }
    }
}
