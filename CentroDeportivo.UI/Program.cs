using CentroDeportivo.Infraestructura.Persistencia.Contexto;
using CentroDeportivo.UI.Components;
using Microsoft.EntityFrameworkCore; // Necesario para .UseSqlite
// Usings de las capas de Aplicación e Infraestructura
using CentroDeportivo.Aplicacion.Interfaces;
using CentroDeportivo.Aplicacion.Validadores;
using CentroDeportivo.Aplicacion.Casos_de_uso.UsuarioUseCase;
using CentroDeportivo.Aplicacion.Casos_de_uso.CanchaUseCase;
using CentroDeportivo.Aplicacion.Casos_de_uso.ProfesorUseCase;
using CentroDeportivo.Aplicacion.Casos_de_uso.ActividadUseCase;
using CentroDeportivo.Aplicacion.Casos_de_uso.DevolucionUseCase;
using CentroDeportivo.Aplicacion.Casos_de_uso.TurnoUseCase;
using CentroDeportivo.Aplicacion.Casos_de_uso.ReservaUseCase;
using CentroDeportivo.Infraestructura.Servicios;
using CentroDeportivo.Infraestructura.Persistencia.Repositorios;
using CentroDeportivo.UI.Servicios;


var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRAR EL CONTEXTO DE LA BASE DE DATOS
// Usamos SQLite y definimos el nombre del archivo .db
builder.Services.AddDbContext<CentroDeportivoContext>(options =>
    options.UseSqlite("Data Source=CentroDeportivo.db"));

// --- 2. REGISTRAR SERVICIOS DE INFRAESTRUCTURA ---
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IProfesorRepositorio, ProfesorRepositorio>();
builder.Services.AddScoped<ICanchaRepositorio, CanchaRepositorio>();
builder.Services.AddScoped<IReservaRepositorio, ReservaRepositorio>();
builder.Services.AddScoped<ITurnoRepositorio, TurnoRepositorio>();
builder.Services.AddScoped<IActividadRepositorio, ActividadRepositorio>();
builder.Services.AddScoped<IDevolucionRepositorio, DevolucionRepositorio>();
builder.Services.AddScoped<IHashServicio, ServicioHash>();
builder.Services.AddScoped<IEmailServicio, EmailServicio>();
builder.Services.AddHostedService<TurnosBackgroundServicio>();

// --- 3. REGISTRAR VALIDADORES (Lógica de Negocio) ---
builder.Services.AddScoped<UsuarioClienteValidador>();
builder.Services.AddScoped<UsuarioEmpleadoValidador>();
builder.Services.AddScoped<ActividadValidador>();
builder.Services.AddScoped<CanchaValidador>();
builder.Services.AddScoped<ReservaValidador>();
builder.Services.AddScoped<TurnoValidador>();
builder.Services.AddScoped<ProfesorValidador>();
builder.Services.AddScoped<LoginValidador>();
builder.Services.AddScoped<DevolucionValidador>();

// --- 4. REGISTRAR CASOS DE USO ---
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<CrearEmpleadoUseCase>();
builder.Services.AddScoped<IniciarSesionUseCase>();
builder.Services.AddScoped<CambiarContraseniaUseCase>();
builder.Services.AddScoped<EliminarEmpleadoUseCase>();
builder.Services.AddScoped<RecuperarContraseniaUseCase>();
builder.Services.AddScoped<ListarEmpleadosUseCase>();

builder.Services.AddScoped<CrearActividadUseCase>();
builder.Services.AddScoped<ListarActividadesUseCase>();
builder.Services.AddScoped<EliminarActividadUseCase>();

builder.Services.AddScoped<EliminarCanchaUseCase>();
builder.Services.AddScoped<CrearCanchaUseCase>();
builder.Services.AddScoped<ListarCanchasUseCase>();
builder.Services.AddScoped<EditarCanchaUseCase>();

builder.Services.AddScoped<CrearProfesorUseCase>();
builder.Services.AddScoped<EliminarProfesorUseCase>();
builder.Services.AddScoped<EditarProfesorUseCase>();
builder.Services.AddScoped<ListarProfesoresUseCase>();

builder.Services.AddScoped<CrearReservaUseCase>();
builder.Services.AddScoped<CancelarReservaUseCase>();
builder.Services.AddScoped<ListarInscriptosUseCase>();
builder.Services.AddScoped<ListarReservasUseCase>();
builder.Services.AddScoped<CancelarReservaUseCase>();
builder.Services.AddScoped<RegistrarAsistenciaQrUseCase>();
builder.Services.AddScoped<ConsultarDisponibilidadUseCase>();
builder.Services.AddScoped<CrearTurnoUseCase>();
builder.Services.AddScoped<EditarTurnoUseCase>();
builder.Services.AddScoped<EliminarTurnoUseCase>();
builder.Services.AddScoped<ListarTurnosUseCase>();
builder.Services.AddScoped<ListarTurnosCalendarioUseCase>();

builder.Services.AddScoped<Sesion>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

//  INICIALIZAR LA BASE DE DATOS Y EL ADMIN
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
