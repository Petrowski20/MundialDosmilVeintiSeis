using Microsoft.AspNetCore.Components;

namespace MundialDosmilVeintiSeis.Components.Shared
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager Nav { get; set; } = default!;
        protected override void OnInitialized()
        {
            Nav.NavigateTo("/login",forceLoad: false);
        }
    }
}
