# Keep MAUI
-keep class Microsoft.Maui.** { *; }
-keep class Microsoft.AspNetCore.Components.** { *; }

# Keep Blazor WebView JS bridge
-keepclassmembers class * {
    @android.webkit.JavascriptInterface <methods>;
}

# Keep JSON serialization
-keep class Newtonsoft.Json.** { *; }

# SQLite / SQLCipher
-keep class net.sqlcipher.** { *; }
-keep class SQLitePCL.** { *; }

# Suppress warnings
-dontwarn Microsoft.Maui.**
-dontwarn androidx.**
