using Microsoft.AspNetCore.Components;
using MudBlazor;
using MundialDosmilVeintiSeis.Models;
using MundialDosmilVeintiSeis.Services;
using static MundialDosmilVeintiSeis.Models.Dtos;

namespace MundialDosmilVeintiSeis.Components.Pages
{
    public partial class Register
    {
        [Inject]
        Supabase.Client Supabase { get; set; }

        [Inject]
        AuthService AuthService { get; set; }

        [Inject]
        NavigationManager NavManager { get; set; }

        [Inject]
        ISnackbar Snackbar { get; set; }

        [SupplyParameterFromQuery(Name = "token")]
        public Guid? Token { get; set; }

        private RegisterDto registerDto = new();
        private DateTime? _birthDate;
        private FavoriteClub? _selectedClub;
        private List<FavoriteClub> _allClubs = new();
        private string CityUppercase
        {
            get => registerDto.City;
            set => registerDto.City = value?.ToUpper() ?? string.Empty;
        }

        private bool _isLoading = true;
        private bool _isInvalidToken = false;
        private bool _isRegistering = false;

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine($"🔍 Token capturado de la URL: {Token}"); // AÑADE ESTO
            // 1. Validar que exista un token en la URL
            if (Token == null)
            {
                _isInvalidToken = true;
                _isLoading = false;
                return;
            }

            try
            {
                // 2. Comprobar en Supabase si la invitación es válida y no se ha usado
                var invitationResponse = await Supabase.From<Invitation>()
                    .Where(i => i.Id == Token && i.IsUsed == false)
                    .Get();

                var invitation = invitationResponse.Models.FirstOrDefault();

                if (invitation == null)
                {
                    _isInvalidToken = true;
                }
                else
                {
                    // Rellenamos el DTO con los datos de la invitación
                    registerDto.InvitationId = invitation.Id;
                    registerDto.Email = invitation.Email;
                    registerDto.Nickname = invitation.SuggestedUsername ?? "";
                    registerDto.PrivateLeagueId = invitation.PrivateLeagueId ?? 1;

                    // 3. Cargamos la lista de clubes para el Autocomplete
                    var clubsResponse = await Supabase.From<FavoriteClub>().Get();
                    _allClubs = clubsResponse.Models;
                }
            }
            catch (Exception ex)
            {
                _isInvalidToken = true;
                Console.WriteLine($"Error cargando invitación: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        // Función que usa MudAutocomplete para buscar mientras escribes
        private async Task<IEnumerable<FavoriteClub>> SearchClubs(string value, CancellationToken token)
        {
            await Task.Delay(5); // Pequeña pausa para evitar parpadeos visuales

            if (string.IsNullOrEmpty(value))
                return _allClubs.Take(10); // Si no escribe nada, mostramos los 10 primeros

            return _allClubs.Where(c => c.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        private async Task HandleRegistration()
        {
            if (_selectedClub == null || _selectedClub.Id == 0)
            {
                Snackbar.Add("Por favor, selecciona un club válido de la lista desplegable.", Severity.Warning);
                return;
            }

            // Pasamos los valores visuales al DTO antes de enviarlo
            registerDto.BirthDate = _birthDate;
            registerDto.FavoriteClubId = _selectedClub.Id;

            _isRegistering = true;

            try
            {
                // Llamamos al servicio que creamos
                var session = await AuthService.RegisterAsync(registerDto);

                if (session != null)
                {
                    Snackbar.Add("¡Registro completado! Bienvenido a la porra.", Severity.Success);
                    // Si el registro va bien, el usuario ya tiene sesión activa. Le mandamos a la portada.
                    NavManager.NavigateTo("/");
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al registrar: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isRegistering = false;
            }
        }
    }
}

