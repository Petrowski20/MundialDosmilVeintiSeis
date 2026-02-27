using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Supabase;

namespace MundialDosmilVeintiSeis.Services
{
    public class SupabaseAuthStateProvider : AuthenticationStateProvider
    {
        private readonly Client _supabase;

        public SupabaseAuthStateProvider(Client supabase)
        {
            _supabase = supabase;
        }

        // Este método se ejecuta cada vez que Blazor necesita saber si estás logueado
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var session = _supabase.Auth.CurrentSession;
            var user = _supabase.Auth.CurrentUser;

            // Si no hay sesión, devolvemos un usuario "Anónimo" (vacío)
            if (session == null || user == null)
            {
                var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                return Task.FromResult(new AuthenticationState(anonymous));
            }

            // Si hay sesión, creamos un "Carnet de identidad" con su ID y Email
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                // Aquí en el futuro podríamos añadir un Claim de "Role" si es Admin
            };

            var identity = new ClaimsIdentity(claims, "SupabaseAuth");
            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(principal));
        }

        // Llamaremos a este método desde el AuthService cuando alguien haga Login o Logout
        public void NotifyAuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}