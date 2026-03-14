using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Activity;
using Income.Platforms.Android;
using Income.Services;
using Microsoft.Maui;

namespace Income
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private IRootDetectionService _rootDetectionService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);

            // Initialize root detection service
            _rootDetectionService = new RootDetectionService();

            // Perform root check on startup
            Task.Run(async () =>
            {
                await CheckDeviceRoot();
            });
        }

        private async Task CheckDeviceRoot()
        {
            try
            {
                var rootResult = await _rootDetectionService.PerformComprehensiveCheck();

                if (rootResult.IsRooted)
                {
                    // Run on UI thread
                    RunOnUiThread(() =>
                    {
                        ShowRootDetectedDialog(rootResult);
                    });
                }
            }
            catch (System.Exception ex)
            {
                // Log the error but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Root detection error: {ex.Message}");
            }
        }

        private void ShowRootDetectedDialog(RootDetectionResult rootResult)
        {
            var builder = new AndroidX.AppCompat.App.AlertDialog.Builder(this);
            builder.SetTitle("Security Alert");
            builder.SetMessage("This device appears to be rooted. For security reasons, this application cannot run on rooted devices.");
            builder.SetCancelable(false);
            builder.SetPositiveButton("OK", (sender, args) =>
            {
                // Exit the application
                FinishAndRemoveTask();
                Process.KillProcess(Process.MyPid());
            });

            var dialog = builder.Create();
            dialog.Show();
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Optional: Re-check on resume to detect runtime rooting
            Task.Run(async () =>
            {
                var isRooted = await _rootDetectionService.IsDeviceRooted();
                if (isRooted)
                {
                    RunOnUiThread(() =>
                    {
                        FinishAndRemoveTask();
                        Process.KillProcess(Process.MyPid());
                    });
                }
            });
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == SAFService.RequestCode && resultCode == Result.Ok)
            {
                SAFService.OnFolderPicked(this, data);
            }
        }
    }
}
