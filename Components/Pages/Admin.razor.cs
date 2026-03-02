using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MundialDosmilVeintiSeis.Components.Shared;
using MundialDosmilVeintiSeis.Models;
using System.Security.Claims; // Añadido para ClaimTypes

namespace MundialDosmilVeintiSeis.Components.Pages
{
    public partial class Admin
    {
        [Inject] Supabase.Client Supabase { get; set; } = null!;
        [Inject] NavigationManager NavManager { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

        [Inject] IDialogService DialogService { get; set; } = null!;

        private bool _isLoading = true;
        private List<Profile> _profiles = new();
        private List<PrivateLeague> _leagues = new();

        protected override async Task OnInitializedAsync()
        {
            await ComprobarSiEsAdmin();
            await CargarUsuarios();
        }

        private async Task ComprobarSiEsAdmin()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                NavManager.NavigateTo("/");
                return;
            }

            var userIdStr = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Parseamos el Guid fuera para que el compilador no se queje
            if (Guid.TryParse(userIdStr, out var userId))
            {
                var myProfile = await Supabase.From<Profile>()
                    .Where(p => p.Id == userId)
                    .Single();

                if (myProfile == null || myProfile.Role != Enums.UserRole.ADMIN)
                {
                    NavManager.NavigateTo("/");
                }
            }
            else
            {
                NavManager.NavigateTo("/");
            }
        }

        private async Task CargarUsuarios()
        {
            _isLoading = true;
            try
            {
                // Cargamos perfiles
                var response = await Supabase.From<Profile>().Get();
                _profiles = response.Models;

                // Cargamos ligas
                var ligasResponse = await Supabase.From<PrivateLeague>().Get();
                _leagues = ligasResponse.Models;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error cargando usuarios: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        // --------------------------------------------------------
        // ACCIONES DE BOTONES
        // --------------------------------------------------------
        private async Task AbrirDialogoInvitacion()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            // Aquí abrimos el componente InviteDialog que creamos antes
            var dialog = await DialogService.ShowAsync<InviteDialog>("Nueva Invitación", options);
            var result = await dialog.Result;

            if (result != null && !result.Canceled)
            {
                var invitacionCreada = (Invitation)result.Data;
                var urlRegistro = $"{NavManager.BaseUri}register?token={invitacionCreada.Id}";

                Snackbar.Add($"Link generado: {urlRegistro}", Severity.Info);
            }
        }
        private async Task AbrirDialogoLiga()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            // Llamamos al nuevo componente CreateLeagueDialog
            var dialog = await DialogService.ShowAsync<CreateLeagueDialog>("Crear Nueva Liga", options);
            var result = await dialog.Result;

            if (result != null && !result.Canceled)
            {
                await CargarUsuarios();
            }
        }

        private async Task EditarUsuario(Profile p)
        {
            // Le pasamos el objeto 'p' como parámetro al componente EditUserDialog
            var parameters = new DialogParameters { ["UserToEdit"] = p };
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            var dialog = await DialogService.ShowAsync<EditUserDialog>("Editar Usuario", parameters, options);
            var result = await dialog.Result;

            if (result != null && !result.Canceled)
            {
                // Si guardó cambios, recargamos la tabla
                await CargarUsuarios();
            }
        }

        private async Task BorrarUsuario(Profile p)
        {
            // Confirmación nativa de MudBlazor (¡Sin crear un .razor extra!)
            bool? isConfirmed = await DialogService.ShowMessageBoxAsync(
                "Confirmar Eliminación",
                $"¿Estás seguro de que quieres eliminar a {p.Nickname}? Esta acción borrará su cuenta y sus predicciones. NO se puede deshacer.",
                yesText: "Sí, Eliminar", cancelText: "Cancelar");

            if (isConfirmed == true)
            {
                try
                {
                    // Borramos el perfil de la base de datos
                    await Supabase.From<Profile>().Where(x => x.Id == p.Id).Delete();

                    Snackbar.Add($"Usuario {p.Nickname} eliminado correctamente.", Severity.Success);
                    await CargarUsuarios();
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error al eliminar: {ex.Message}", Severity.Error);
                }
            }
        }
    }
}