using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Activity;
using Income.Services;
using Microsoft.Maui;

namespace Income
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (e.KeyCode == Keycode.Back && e.Action == KeyEventActions.Down && e.RepeatCount == 0)
            {
                var services = IPlatformApplication.Current?.Services;
                var backService = services?.GetService<BackButtonService>();

                if (backService?.OnBackPressed() == true)
                {
                    // Block handled by Blazor
                    return true;
                }

                // Otherwise let Android handle normally (exit if no pages left)
                return base.DispatchKeyEvent(e);
            }

            return base.DispatchKeyEvent(e);
        }
    }
}
