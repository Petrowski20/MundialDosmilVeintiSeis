using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using MundialDosmilVeintiSeis.Components;
using MundialDosmilVeintiSeis.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Registra tu proveedor personalizado que acabamos de crear
builder.Services.AddScoped<AuthenticationStateProvider, SupabaseAuthStateProvider>();

// Tu AuthService (ya lo tenías, asegúrate de que esté)
builder.Services.AddScoped<AuthService>();

// MudBlazor - UI moderna, responsive, dark mode, snackbars bonitos
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});

builder.Services.AddHttpClient();

// Supabase - Nuestro backend completo (DB + Auth + Realtime)
builder.Services.AddSingleton<SupabaseService>();

// Background Service - Actualiza resultados y estadísticas de equipos 1 vez al día
builder.Services.AddHostedService<MatchUpdateService>();

// Logging (veremos en consola cuando se ejecute el job diario)
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();