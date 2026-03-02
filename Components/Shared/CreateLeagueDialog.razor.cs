using Microsoft.AspNetCore.Components;
using MudBlazor;
using MundialDosmilVeintiSeis.Models;

namespace MundialDosmilVeintiSeis.Components.Shared
{
    public partial class CreateLeagueDialog : ComponentBase
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;
        [Inject] Supabase.Client Supabase { get; set; } = null!;
        [Inject] ISnackbar Snackbar { get; set; } = null!;

        protected PrivateLeague _league = new();
        protected bool _isSubmitting = false;

        protected void Cancel() => MudDialog.Cancel();

        protected async Task CrearLiga()
        {
            if (string.IsNullOrWhiteSpace(_league.Name))
            {
                Snackbar.Add("El nombre de la liga es obligatorio.", Severity.Warning);
                return;
            }

            _isSubmitting = true;

            try
            {
                // Limpiamos espacios y lo ponemos en mayúsculas para mantener un estándar visual
                _league.Name = _league.Name.Trim().ToUpper();

                // Insertamos en la base de datos
                await Supabase.From<PrivateLeague>().Insert(_league);

                Snackbar.Add($"Liga '{_league.Name}' creada con éxito.", Severity.Success);

                // Cerramos la ventana y pasamos el objeto creado por si la página padre lo necesita
                MudDialog.Close(DialogResult.Ok(_league));
            }
            catch (Exception ex)
            {
                // Supabase lanzará un error aquí si intentas crear una liga con un nombre que ya existe (gracias al UNIQUE de SQL)
                Snackbar.Add($"Error al crear liga: {ex.Message}", Severity.Error);
            }
            finally
            {
                _isSubmitting = false;
            }
        }
    }
}