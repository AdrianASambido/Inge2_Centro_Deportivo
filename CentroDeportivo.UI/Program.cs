using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using CentroDeportivo.UI.Components;
using Microsoft.EntityFrameworkCore; // Necesario para .UseSqlite

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRAR EL CONTEXTO DE LA BASE DE DATOS
// Usamos SQLite y definimos el nombre del archivo .db
builder.Services.AddDbContext<CentroDeportivoContext>(options =>
    options.UseSqlite("Data Source=CentroDeportivo.db"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 2. INICIALIZAR LA BASE DE DATOS Y EL ADMIN
// Este bloque se ejecuta antes de que la app empiece a recibir usuarios
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CentroDeportivoContext>();
        // Llamamos a la clase dee infraestructura para crear tablas y el Admin
        DbInicializador.Initialize(context);
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Error al inicializar la BD: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
