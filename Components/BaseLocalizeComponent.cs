using Income.Services;
using Microsoft.AspNetCore.Components;

namespace Income.Components
{
    public abstract class BaseLocalizedComponent : ComponentBase, IDisposable
    {
        [Inject] protected LocalizationService Localization { get; set; } = default!;
        [Inject] protected LanguageState LanguageState { get; set; } = default!;

        protected override void OnInitialized()
        {
            LanguageState.OnChange += HandleLanguageChanged;
            base.OnInitialized();
        }

        private void HandleLanguageChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            LanguageState.OnChange -= HandleLanguageChanged;
        }
    }
}
