namespace GestionIntervention.Dtos.Client
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
    }
}
