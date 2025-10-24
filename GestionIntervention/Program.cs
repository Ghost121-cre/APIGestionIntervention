using GestionIntervention.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders; // Ajoutez ce using

var builder = WebApplication.CreateBuilder(args);

// 🔹 Base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 CORS - pour React (port 5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Ajoutez cette ligne si vous utilisez l'authentification
    });
});

// 🔹 Configuration des contrôleurs avec gestion des cycles JSON
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

// 🔹 Swagger (exploration API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 ASSUREZ-VOUS QUE LE DOSSIER WWWROOT EXISTE
// Cette ligne est implicite avec WebApplication.CreateBuilder()

var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

// 🔹 AJOUTEZ CES LIGNES POUR LES FICHIERS STATIQUES
app.UseStaticFiles(); // Active la gestion des fichiers statiques depuis wwwroot

// 🔹 Configuration spécifique pour le dossier uploads
var uploadsPath = Path.Combine(app.Environment.WebRootPath ?? app.Environment.ContentRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
    Console.WriteLine($"📁 Dossier uploads créé: {uploadsPath}");
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads",
    ServeUnknownFileTypes = true // Permet de servir tous les types de fichiers
});

// Dans Program.cs, ajoutez cette configuration
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "interventions")),
    RequestPath = "/uploads/interventions"
});

app.UseAuthorization();

app.MapControllers();

app.Run();