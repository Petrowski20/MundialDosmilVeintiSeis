using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using MundialDosmilVeintiSeis.Components;
using MundialDosmilVeintiSeis.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Componentes Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. EL MOTOR DE SUPABASE (¡Esto es lo que faltaba!)
// Asegúrate de que los nombres coinciden con tu appsettings.json
var supabaseUrl = builder.Configuration["Supabase:Url"] ?? throw new Exception("Falta Supabase:Url");
var supabaseKey = builder.Configuration["Supabase:Key"] ?? throw new Exception("Falta Supabase:Key");

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(supabaseUrl, supabaseKey, new Supabase.SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true
    }));

// 3. Sistema de Autenticación de Blazor
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, SupabaseAuthStateProvider>();
builder.Services.AddScoped<AuthService>();

// 4. Tu servicio de base de datos
builder.Services.AddScoped<SupabaseService>();

// 5. MudBlazor - UI moderna
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.MaxDisplayedSnackbars = 5;
});

builder.Services.AddHttpClient();

// 6. Background Service 
// (LO COMENTAMOS TEMPORALMENTE HASTA QUE ARREGLEMOS SU INYECCIÓN DE DEPENDENCIAS)
// builder.Services.AddHostedService<MatchUpdateService>();

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