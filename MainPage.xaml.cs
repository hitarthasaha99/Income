namespace Income
{
    public partial class MainPage : ContentPage
    {
        public static Func<bool>? OnBack;

#if ANDROID
    DateTime _lastBackPressed = DateTime.MinValue;
    const int EXIT_INTERVAL_SECONDS = 2;
#endif

        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            // 1️⃣ Let Blazor decide first (login, logout, etc.)
            if (OnBack?.Invoke() == true)
                return true;

#if ANDROID
        // 2️⃣ Double-tap to exit logic
        var now = DateTime.UtcNow;

        if ((now - _lastBackPressed).TotalSeconds < EXIT_INTERVAL_SECONDS)
        {
            // Exit app
            Microsoft.Maui.ApplicationModel.Platform
                .CurrentActivity?
                .FinishAffinity();

            return true;
        }

        _lastBackPressed = now;

        // Show hint
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Android.Widget.Toast.MakeText(
                Android.App.Application.Context,
                "Press back again to exit",
                Android.Widget.ToastLength.Short
            )?.Show();
        });

        return true; // consume back press
#else
            return base.OnBackButtonPressed();
#endif
        }
    }

}
