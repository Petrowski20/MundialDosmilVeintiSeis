using Microsoft.AspNetCore.Components;
using MudBlazor;
using MundialDosmilVeintiSeis.Models;

namespace MundialDosmilVeintiSeis.Components.Shared
{
    public partial class InviteDialog : ComponentBase
    {
        [CascadingParameter] 
        IMudDialogInstance MudDialog { get; set; }

        [Inject] Supabase.Client Supabase { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        private string _email = string.Empty;
        private string? _suggestedUsername;
        private int _selectedLeagueId;

        private List<PrivateLeague> _leagues = new();
        private bool _isLoadingLeagues = true;
        private bool _isSubmitting = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Cargamos las ligas disponibles para el desplegable
                var response = await Supabase.From<PrivateLeague>().Get();
                _leagues = response.Models;

                if (_leagues.Any())
                {
                    _selectedLeagueId = _leagues.First().Id; // Selecciona la primera por defecto (ej. PETROWSKI)
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error cargando ligas: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoadingLeagues = false;
            }
        }

        private void Cancel() => MudDialog.Cancel();

        private async Task GenerarInvitacion()
        {
            if (string.IsNullOrWhiteSpace(_email))
            {
                Snackbar.Add("El correo es obligatorio.", Severity.Warning);
                return;
            }

            _isSubmitting = true;

            try
            {
                var session = Supabase.Auth.CurrentSession;
                if (session == null || session.User == null) throw new Exception("No hay sesión activa.");

                // Generamos el ID aquí en C# para poder mostrarlo luego si queremos
                var newInvitationId = Guid.NewGuid();

                var invitation = new Invitation
                {
                    Id = newInvitationId,
                    Email = _email.Trim(),
                    SuggestedUsername = string.IsNullOrWhiteSpace(_suggestedUsername) ? null : _suggestedUsername.Trim(),
                    PrivateLeagueId = _selectedLeagueId,
                    // Supabase mapeará automáticamente esto a la base de datos
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };

                await Supabase.From<Invitation>().Insert(invitation);

                Snackbar.Add("Invitación creada con éxito.", Severity.Success);

                // Cerramos la ventana y devolvemos la invitación creada para que la página Admin la use
                MudDialog.Close(DialogResult.Ok(invitation));
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al crear invitación: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}