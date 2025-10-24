using GestionIntervention.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionIntervention.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentImage> IncidentImages { get; set; }
        public DbSet<Intervention> Interventions { get; set; }
        public DbSet<InterventionImage> InterventionImages { get; set; }
        public DbSet<Rapport> Rapports { get; set; }
        public DbSet<StatutIntervention> StatutInterventions { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relations Incident
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Client)
                .WithMany(c => c.Incidents)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Produit)
                .WithMany(p => p.Incidents)
                .HasForeignKey(i => i.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IncidentImage>()
                .HasOne(ii => ii.Incident)
                .WithMany(i => i.Images)
                .HasForeignKey(ii => ii.IncidentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relations Intervention
            modelBuilder.Entity<Intervention>()
                .HasOne(iv => iv.Client)
                .WithMany(c => c.Interventions)
                .HasForeignKey(iv => iv.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Intervention>()
                .HasOne(iv => iv.Produit)
                .WithMany(p => p.Interventions)
                .HasForeignKey(iv => iv.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Intervention>()
                .HasOne(iv => iv.Incident)
                .WithMany(i => i.Interventions)
                .HasForeignKey(iv => iv.IncidentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Intervention>()
                .HasOne(iv => iv.Technicien)
                .WithMany(u => u.Interventions)
                .HasForeignKey(iv => iv.TechnicienId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InterventionImage>()
                .HasOne(ii => ii.Intervention)
                .WithMany(iv => iv.Images)
                .HasForeignKey(ii => ii.InterventionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Plusieurs rapports par intervention
            modelBuilder.Entity<Rapport>()
                .HasOne(r => r.Intervention)
                .WithMany(iv => iv.Rapports)
                .HasForeignKey(r => r.InterventionId)
                .OnDelete(DeleteBehavior.Cascade);

            // StatutIntervention
            modelBuilder.Entity<StatutIntervention>()
                .HasOne(si => si.Intervention)
                .WithMany(iv => iv.Statuts)
                .HasForeignKey(si => si.InterventionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StatutIntervention>()
                .HasOne(si => si.Utilisateur)
                .WithMany(u => u.StatutsInterventions)
                .HasForeignKey(si => si.UtilisateurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Utilisateur)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unicité email utilisateur
            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
