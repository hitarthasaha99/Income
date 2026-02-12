using Android.App;
using Android.Content;
using Android.Net;
using Uri = Android.Net.Uri;

namespace Income.Platforms.Android
{
    public static class SAFService
    {
        public const int RequestCode = 9001;
        public static Uri? SelectedTreeUri;

        public static void LaunchFolderPicker(Activity activity)
        {
            var intent = new Intent(Intent.ActionOpenDocumentTree);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission |
                            ActivityFlags.GrantWriteUriPermission |
                            ActivityFlags.GrantPersistableUriPermission);

            activity.StartActivityForResult(intent, RequestCode);
        }

        public static void OnFolderPicked(Context context, Intent? data)
        {
            var uri = data?.Data;
            if (uri == null) return;

            context.ContentResolver.TakePersistableUriPermission(
                uri,
                ActivityFlags.GrantReadUriPermission |
                ActivityFlags.GrantWriteUriPermission);

            SelectedTreeUri = uri;

            Preferences.Set("SAF_TREE_URI", uri.ToString());
        }

        public static Uri? Restore()
        {
            var saved = Preferences.Get("SAF_TREE_URI", null);
            return saved == null ? null : Uri.Parse(saved);
        }
    }

}
