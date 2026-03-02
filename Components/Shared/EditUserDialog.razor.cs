using Microsoft.AspNetCore.Components;
using MudBlazor;
using MundialDosmilVeintiSeis.Models;

namespace MundialDosmilVeintiSeis.Components.Shared
{
    public partial class EditUserDialog : ComponentBase
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] Supabase.Client Supabase { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = null!;

        // El usuario que nos pasa la página Admin al abrir la ventana
        [Parameter] public Profile UserToEdit { get; set; } = null!;

        protected Profile _editModel = new();
        protected Enums.UserRole _selectedRole;

        protected List<PrivateLeague> _allLeagues = new();
        protected IReadOnlyCollection<int> _selectedLeagueIds = new List<int>();
        protected bool _isLoadingLeagues = true;
        protected bool _isSubmitting = false;

        protected override async Task OnInitializedAsync()
        {
            // Hacemos una copia para no alterar la tabla de fondo antes de darle a Guardar
            _editModel.Id = UserToEdit.Id;
            _editModel.Nickname = UserToEdit.Nickname;
            _editModel.City = UserToEdit.City;
            _selectedRole = UserToEdit.Role;

            await CargarLigas();
        }

        private async Task CargarLigas()
        {
            try
            {
                // 1. Traemos todas las ligas para el desplegable
                var ligasResponse = await Supabase.From<PrivateLeague>().Get();
                _allLeagues = ligasResponse.Models;

                // 2. Traemos las ligas en las que ESTÁ este usuario actualmente
                var userLeaguesResponse = await Supabase.From<ProfileLeague>()
                    .Where(pl => pl.ProfileId == UserToEdit.Id)
                    .Get();

                // 3. Marcamos los checkboxes correspondientes
                _selectedLeagueIds = userLeaguesResponse.Models.Select(pl => pl.LeagueId).ToHashSet();
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

        protected void Cancel() => MudDialog.Cancel();

        protected async Task GuardarCambios()
        {
            if (string.IsNullOrWhiteSpace(_editModel.Nickname))
            {
                Snackbar.Add("El Nickname es obligatorio.", Severity.Warning);
                return;
            }

            _isSubmitting = true;
            try
            {
                // 1. Actualizamos el Perfil (Nickname, Ciudad, Rol)
                await Supabase.From<Profile>()
                    .Where(p => p.Id == _editModel.Id)
                    .Set(p => p.Nickname, _editModel.Nickname)
                    .Set(p => p.City, _editModel.City?.ToUpper())
                    .Set(p => p.Role, _selectedRole)
                    .Update();

                // 2. MAGIA RELACIONAL: Borramos sus ligas antiguas y metemos las nuevas
                await Supabase.From<ProfileLeague>().Where(pl => pl.ProfileId == _editModel.Id).Delete();

                if (_selectedLeagueIds.Any())
                {
                    var nuevasLigas = _selectedLeagueIds.Select(id => new ProfileLeague
                    {
                        ProfileId = _editModel.Id,
                        LeagueId = id,
                        JoinedAt = DateTime.UtcNow
                    }).ToList();

                    await Supabase.From<ProfileLeague>().Insert(nuevasLigas);
                }

                Snackbar.Add("Usuario actualizado correctamente.", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al guardar: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}
