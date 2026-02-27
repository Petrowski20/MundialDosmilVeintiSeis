using Microsoft.AspNetCore.Components.Authorization;
using MundialDosmilVeintiSeis.Models;
using Supabase;
using Supabase.Gotrue;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MundialDosmilVeintiSeis.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly AuthenticationStateProvider _authStateProvider;
        public AuthService(Supabase.Client supabase, AuthenticationStateProvider authStateProvider)
        {
            _supabase = supabase;
            _authStateProvider = authStateProvider;
        }
        public async Task<Session?> RegisterAsync(Dtos.RegisterDto dto)
        {
            var metadata = new Dictionary<string, object>
            {
            { "nickname", dto.Nickname },
            { "birth_date", dto.BirthDate?.ToString("yyyy-MM-dd") ?? "" },
            { "city", dto.City.ToUpper() }, // Reforzamos las mayúsculas aquí también
            { "favorite_club_id", dto.FavoriteClubId },
            { "invitation_id", dto.InvitationId }
          };
            var options = new SignUpOptions { Data = metadata };

            var response = await _supabase.Auth.SignUp(dto.Email, dto.Password, options);
            return response;
        }

        public async Task<Session?> LoginAsync(string identifier, string password)
        {
            string email = identifier;

            // Si el identificador no tiene '@', asumimos que es un Nickname
            if (!identifier.Contains("@"))
            {
                // Buscamos en la tabla profiles el email asociado a ese nickname
                var profile = await _supabase.From<Profile>()
                    .Filter("nickname", Supabase.Postgrest.Constants.Operator.Equals, identifier)
                    .Single();

                if (profile == null) throw new Exception("Usuario no encontrado.");

                // Necesitaremos que tu tabla Profiles tenga el campo email 
                // o usar una lógica de búsqueda en la tabla de Auth (si eres admin)
                // Nota: Como el email es sensible, lo ideal es que el perfil 
                // guarde el email o el usuario lo sepa.
            }

            // Login real en Supabase Auth
            var session = await _supabase.Auth.SignIn(email, password);
            ((SupabaseAuthStateProvider)_authStateProvider).NotifyAuthStateChanged();
            return session;
        }

        public async Task LogoutAsync()
        {
            await _supabase.Auth.SignOut();
            ((SupabaseAuthStateProvider)_authStateProvider).NotifyAuthStateChanged();
        }
    }
}
