using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Income.Common;
using System.Diagnostics;

namespace Income.Services
{
    using System.Security.Cryptography;
    using System.Text;

    public class SecureAuditLoggingService
    {
        private readonly string _logDirectory;
        private byte[] _encryptionKey;
        private const string ENCRYPTION_PASSWORD = "YourSecurePasswordHere"; // TODO: Change this!

        public SecureAuditLoggingService()
        {
            // Derive encryption key from password (consistent across installs)
            _encryptionKey = DeriveKeyFromPassword(ENCRYPTION_PASSWORD);
        }

        private byte[] DeriveKeyFromPassword(string password)
        {
            // Use PBKDF2 to derive a consistent key from password
            using (var deriveBytes = new Rfc2898DeriveBytes(
                password,
                Encoding.UTF8.GetBytes("Income_Salt_2024"), // Static salt for consistency
                100000,
                HashAlgorithmName.SHA256))
            {
                return deriveBytes.GetBytes(32); // 256-bit key
            }
        }

        public async Task LogApiRequestAsync(string endpoint, string method, object requestData, string code = "")
        {
            try
            {
                // Sanitize sensitive data before logging
                var logEntry = new
                {
                    Timestamp = DateTime.UtcNow,
                    FsuId = SessionStorage.SelectedFSUId,
                    Endpoint = endpoint,
                    Method = method,
                    Code = code,
                    DeviceInfo = $"{DeviceInfo.Platform} {DeviceInfo.Version}",
                    // Only log sanitized/hashed data
                    RequestHash = ComputeHash(requestData),
                    Success = true // Set based on actual result
                };

                string json = System.Text.Json.JsonSerializer.Serialize(logEntry,
                    new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

                // Encrypt the data
                byte[] encryptedData = EncryptData(json);

                string fileName = $"{SessionStorage.SelectedFSUId}_{endpoint}_{code}_{DateTime.Now:yyyyMMdd_HHmmss}.enc";

                // Save to public Documents folder
                await SaveEncryptedLogAsync(fileName, encryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error logging API request: {ex.Message}");
            }
        }

        private async Task SaveEncryptedLogAsync(string fileName, byte[] encryptedData)
        {
#if WINDOWS
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string logPath = Path.Combine(documentsPath, "Income_Logs", "Encrypted");
        
        if (!Directory.Exists(logPath))
            Directory.CreateDirectory(logPath);
        
        string filePath = Path.Combine(logPath, fileName);
        await File.WriteAllBytesAsync(filePath, encryptedData);
        
#elif ANDROID
            // Use MediaStore to save to Documents/Income_Logs/Encrypted
            var context = Android.App.Application.Context;
            var contentResolver = context.ContentResolver;

            var contentValues = new Android.Content.ContentValues();
            contentValues.Put(Android.Provider.MediaStore.MediaColumns.DisplayName, fileName);
            contentValues.Put(Android.Provider.MediaStore.MediaColumns.MimeType, "application/octet-stream");
            contentValues.Put(Android.Provider.MediaStore.MediaColumns.RelativePath,
                $"{Android.OS.Environment.DirectoryDocuments}/Income_Logs/Encrypted");

            var uri = contentResolver.Insert(
                Android.Provider.MediaStore.Files.GetContentUri("external"),
                contentValues);

            if (uri != null)
            {
                using (var outputStream = contentResolver.OpenOutputStream(uri))
                {
                    await outputStream.WriteAsync(encryptedData, 0, encryptedData.Length);
                    await outputStream.FlushAsync();
                }
            }
#endif
        }

        private byte[] EncryptData(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    // Write IV first (needed for decryption)
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }

                    return ms.ToArray();
                }
            }
        }

        public async Task<string> DecryptLogAsync(string filePath)
        {
            byte[] encryptedData = await File.ReadAllBytesAsync(filePath);
            return DecryptData(encryptedData);
        }

        private string DecryptData(byte[] encryptedData)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;

                // Extract IV from beginning of data
                byte[] iv = new byte[16]; // AES IV is always 16 bytes
                Array.Copy(encryptedData, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string ComputeHash(object data)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(data);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Tool to decrypt logs for auditing
        public async Task<List<string>> DecryptAllLogsAsync()
        {
            var decryptedLogs = new List<string>();

#if WINDOWS
        string logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Income_Logs", "Encrypted");

        if (Directory.Exists(logPath))
        {
            var files = Directory.GetFiles(logPath, "*.enc");
            foreach (var file in files)
            {
                try
                {
                    var decrypted = await DecryptLogAsync(file);
                    decryptedLogs.Add(decrypted);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error decrypting {file}: {ex.Message}");
                }
            }
        }
#elif ANDROID
            // Read encrypted logs from MediaStore - CORRECTED VERSION
            var context = Android.App.Application.Context;
            var contentResolver = context.ContentResolver;

            var projection = new[]
            {
            Android.Provider.MediaStore.MediaColumns.Id,
            Android.Provider.MediaStore.MediaColumns.DisplayName,
            Android.Provider.MediaStore.MediaColumns.RelativePath
        };

            var selection = $"{Android.Provider.MediaStore.MediaColumns.RelativePath} LIKE ?";
            var selectionArgs = new[] { "%Income_Logs/Encrypted%" };

            using (var cursor = contentResolver.Query(
                Android.Provider.MediaStore.Files.GetContentUri("external"),
                projection,
                selection,
                selectionArgs,
                null))
            {
                if (cursor != null && cursor.MoveToFirst())
                {
                    var idColumn = cursor.GetColumnIndex(Android.Provider.MediaStore.MediaColumns.Id);

                    do
                    {
                        try
                        {
                            var id = cursor.GetLong(idColumn);
                            var uri = Android.Content.ContentUris.WithAppendedId(
                                Android.Provider.MediaStore.Files.GetContentUri("external"), id);

                            using (var inputStream = contentResolver.OpenInputStream(uri))
                            {
                                if (inputStream != null)
                                {
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        await inputStream.CopyToAsync(memoryStream);
                                        byte[] encryptedData = memoryStream.ToArray();
                                        string decrypted = DecryptData(encryptedData);
                                        decryptedLogs.Add(decrypted);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error decrypting log: {ex.Message}");
                        }
                    } while (cursor.MoveToNext());
                }
            }
#endif

            return decryptedLogs;
        }

        // Export decrypted logs (with user consent/authentication)
        public async Task<string> ExportDecryptedLogsAsync(string password)
        {
            // Verify password before allowing export
            if (password != ENCRYPTION_PASSWORD)
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            var logs = await DecryptAllLogsAsync();
            var exportJson = System.Text.Json.JsonSerializer.Serialize(logs,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

#if WINDOWS
        string exportPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            $"Income_Audit_Export_{DateTime.Now:yyyyMMdd_HHmmss}.json");
        await File.WriteAllTextAsync(exportPath, exportJson);
        return exportPath;
#elif ANDROID
            // Save decrypted export to Documents
            var context = Android.App.Application.Context;
            var contentResolver = context.ContentResolver;

            var contentValues = new Android.Content.ContentValues();
            contentValues.Put(Android.Provider.MediaStore.MediaColumns.DisplayName,
                $"Income_Audit_Export_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            contentValues.Put(Android.Provider.MediaStore.MediaColumns.MimeType, "application/json");
            contentValues.Put(Android.Provider.MediaStore.MediaColumns.RelativePath,
                Android.OS.Environment.DirectoryDocuments);

            var uri = contentResolver.Insert(
                Android.Provider.MediaStore.Files.GetContentUri("external"),
                contentValues);

            if (uri != null)
            {
                using (var outputStream = contentResolver.OpenOutputStream(uri))
                {
                    if (outputStream != null)
                    {
                        using (var writer = new StreamWriter(outputStream))
                        {
                            await writer.WriteAsync(exportJson);
                        }
                    }
                }
                return "Documents/Income_Audit_Export_*.json";
            }
#endif
            return null;
        }
    }
}
