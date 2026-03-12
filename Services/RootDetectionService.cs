using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#if ANDROID
using Android.Content.PM;
using Android.OS;
using Income.Services;
using Java.Lang;
using Process = Java.Lang.Process;

#endif

namespace Income.Services
{
    public interface IRootDetectionService
    {
        Task<bool> IsDeviceRooted();
        Task<RootDetectionResult> PerformComprehensiveCheck();
    }

    public class RootDetectionResult
    {
        public bool IsRooted { get; set; }
        public List<string> DetectedIndicators { get; set; } = new List<string>();
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
    }

    public class RootDetectionService : IRootDetectionService
    {
#if ANDROID
        private static readonly string[] SU_BINARY_PATHS = new[]
        {
            "/system/app/Superuser.apk",
            "/sbin/su",
            "/system/bin/su",
            "/system/xbin/su",
            "/data/local/xbin/su",
            "/data/local/bin/su",
            "/system/sd/xbin/su",
            "/system/bin/failsafe/su",
            "/data/local/su",
            "/su/bin/su",
            "/su/bin",
            "/system/xbin/daemonsu"
        };

        private static readonly string[] KNOWN_ROOT_APPS_PACKAGES = new[]
        {
            "com.noshufou.android.su",
            "com.noshufou.android.su.elite",
            "eu.chainfire.supersu",
            "com.koushikdutta.superuser",
            "com.thirdparty.superuser",
            "com.yellowes.su",
            "com.topjohnwu.magisk",
            "com.kingroot.kinguser",
            "com.kingo.root",
            "com.smedialink.oneclickroot",
            "com.zhiqupk.root.global",
            "com.alephzain.framaroot",
            "com.koushikdutta.rommanager",
            "com.koushikdutta.rommanager.license",
            "com.dimonvideo.luckypatcher",
            "com.chelpus.lackypatch",
            "com.ramdroid.appquarantine",
            "com.ramdroid.appquarantinepro",
            "com.devadvance.rootcloak",
            "com.devadvance.rootcloakplus",
            "de.robv.android.xposed.installer",
            "com.saurik.substrate",
            "com.zachspong.temprootremovejb",
            "com.amphoras.hidemyroot",
            "com.amphoras.hidemyrootadfree",
            "com.formyhm.hiderootPremium",
            "com.formyhm.hideroot"
        };

        private static readonly string[] KNOWN_DANGEROUS_PROPS = new[]
        {
            "[ro.debuggable]:[1]",
            "[ro.secure]:[0]"
        };

        private static readonly string[] ROOT_CLOAKING_PACKAGES = new[]
        {
            "com.devadvance.rootcloak",
            "com.devadvance.rootcloakplus",
            "de.robv.android.xposed.installer",
            "com.saurik.substrate",
            "com.zachspong.temprootremovejb",
            "com.amphoras.hidemyroot",
            "com.amphoras.hidemyrootadfree",
            "com.formyhm.hiderootPremium",
            "com.formyhm.hideroot"
        };

        public async Task<bool> IsDeviceRooted()
        {
            var result = await PerformComprehensiveCheck();
            return result.IsRooted;
        }

        public async Task<RootDetectionResult> PerformComprehensiveCheck()
        {
            var result = new RootDetectionResult();

            await Task.Run(() =>
            {
                // Check 1: Test-Keys Build
                if (CheckTestKeys())
                {
                    result.DetectedIndicators.Add("Test-keys build detected");
                }

                // Check 2: SU Binary Detection
                if (CheckForSuBinary())
                {
                    result.DetectedIndicators.Add("SU binary found");
                }

                // Check 3: Known Root Apps
                var rootApps = CheckForRootApps();
                if (rootApps.Any())
                {
                    result.DetectedIndicators.Add($"Root apps detected: {string.Join(", ", rootApps)}");
                }

                // Check 4: Dangerous System Properties
                if (CheckForDangerousProps())
                {
                    result.DetectedIndicators.Add("Dangerous system properties detected");
                }

                // Check 5: RW System Partition
                if (CheckForRWPaths())
                {
                    result.DetectedIndicators.Add("Read-write system partition detected");
                }

                // Check 6: SU Command Execution
                if (CheckSuExists())
                {
                    result.DetectedIndicators.Add("SU command executable");
                }

                // Check 7: Root Cloaking Detection
                var cloakingApps = CheckForRootCloaking();
                if (cloakingApps.Any())
                {
                    result.DetectedIndicators.Add($"Root cloaking detected: {string.Join(", ", cloakingApps)}");
                }

                // Check 8: Busybox Detection
                if (CheckForBusyBox())
                {
                    result.DetectedIndicators.Add("BusyBox detected");
                }

                // Check 9: Magisk Detection
                if (CheckForMagisk())
                {
                    result.DetectedIndicators.Add("Magisk detected");
                }

                // Check 10: SELinux Status
                if (CheckSELinuxStatus())
                {
                    result.DetectedIndicators.Add("SELinux permissive mode detected");
                }
            });

            result.IsRooted = result.DetectedIndicators.Count > 0;
            return result;
        }

        private bool CheckTestKeys()
        {
            try
            {
                string buildTags = Build.Tags;
                return buildTags != null && buildTags.Contains("test-keys");
            }
            catch
            {
                return false;
            }
        }

        private bool CheckForSuBinary()
        {
            foreach (var path in SU_BINARY_PATHS)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        return true;
                    }
                }
                catch
                {
                    // Access denied might indicate protection
                }
            }
            return false;
        }

        private List<string> CheckForRootApps()
        {
            var detectedApps = new List<string>();
            var packageManager = Android.App.Application.Context.PackageManager;

            foreach (var packageName in KNOWN_ROOT_APPS_PACKAGES)
            {
                try
                {
                    packageManager?.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                    detectedApps.Add(packageName);
                }
                catch (PackageManager.NameNotFoundException)
                {
                    // Package not found - this is good
                }
                catch
                {
                    // Other errors
                }
            }

            return detectedApps;
        }

        private bool CheckForDangerousProps()
        {
            try
            {
                // Check ro.debuggable
                string debuggable = GetSystemProperty("ro.debuggable");
                if (debuggable == "1")
                {
                    return true;
                }

                // Check ro.secure
                string secure = GetSystemProperty("ro.secure");
                if (secure == "0")
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private string GetSystemProperty(string key)
        {
            try
            {
                var systemProperties = Class.ForName("android.os.SystemProperties");
                var getMethod = systemProperties.GetMethod("get", Class.FromType(typeof(string)));
                var value = getMethod.Invoke(systemProperties, key);
                return value?.ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool CheckForRWPaths()
        {
            try
            {
                var mountReader = File.ReadAllLines("/proc/mounts");
                foreach (var line in mountReader)
                {
                    string[] args = line.Split(' ');
                    if (args.Length < 4) continue;

                    string mountPoint = args[1];
                    string mountOptions = args[3];

                    // Check if system partition is mounted as read-write
                    if (mountPoint.Equals("/system") || 
                        mountPoint.Equals("/system/root") ||
                        mountPoint.Equals("/"))
                    {
                        var options = mountOptions.Split(',');
                        foreach (var option in options)
                        {
                            if (option.Equals("rw"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Unable to read mounts
            }

            return false;
        }

        private bool CheckSuExists()
        {
            Java.Lang.Process process = null;
            try
            {
                process = Runtime.GetRuntime()?.Exec(new[] { "which", "su" });
                var exitValue = process?.WaitFor() ?? -1;
                return exitValue == 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                process?.Destroy();
            }
        }

        private List<string> CheckForRootCloaking()
        {
            var detectedApps = new List<string>();
            var packageManager = Android.App.Application.Context.PackageManager;

            foreach (var packageName in ROOT_CLOAKING_PACKAGES)
            {
                try
                {
                    packageManager?.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                    detectedApps.Add(packageName);
                }
                catch (PackageManager.NameNotFoundException)
                {
                    // Package not found
                }
                catch
                {
                    // Other errors
                }
            }

            return detectedApps;
        }

        private bool CheckForBusyBox()
        {
            try
            {
                var paths = new[] 
                { 
                    "/system/xbin/busybox",
                    "/system/bin/busybox",
                    "/data/local/xbin/busybox",
                    "/data/local/bin/busybox",
                    "/sbin/busybox"
                };

                foreach (var path in paths)
                {
                    if (File.Exists(path))
                    {
                        return true;
                    }
                }

                // Try executing busybox command
                Process process = null;
                try
                {
                    process = Runtime.GetRuntime()?.Exec(new[] { "which", "busybox" });
                    var exitValue = process?.WaitFor() ?? -1;
                    return exitValue == 0;
                }
                finally
                {
                    process?.Destroy();
                }
            }
            catch
            {
                return false;
            }
        }

        private bool CheckForMagisk()
        {
            try
            {
                // Check for Magisk app
                var packageManager = Android.App.Application.Context.PackageManager;
                try
                {
                    packageManager?.GetPackageInfo("com.topjohnwu.magisk", PackageInfoFlags.Activities);
                    return true;
                }
                catch (PackageManager.NameNotFoundException)
                {
                    // Continue to other checks
                }

                // Check for Magisk mount points
                if (File.Exists("/sbin/.magisk") || 
                    Directory.Exists("/sbin/.magisk") ||
                    File.Exists("/system/xbin/su") && File.Exists("/sbin/.magisk"))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool CheckSELinuxStatus()
        {
            try
            {
                Process process = null;
                try
                {
                    process = Runtime.GetRuntime()?.Exec("getenforce");
                    using (var reader = new StreamReader(process?.InputStream))
                    {
                        string output = reader.ReadToEnd()?.Trim().ToLower();
                        return output == "permissive" || output == "disabled";
                    }
                }
                finally
                {
                    process?.Destroy();
                }
            }
            catch
            {
                return false;
            }
        }
#else
        public Task<bool> IsDeviceRooted()
        {
            return Task.FromResult(false);
        }

        public Task<RootDetectionResult> PerformComprehensiveCheck()
        {
            return Task.FromResult(new RootDetectionResult { IsRooted = false });
        }
#endif
    }
}
