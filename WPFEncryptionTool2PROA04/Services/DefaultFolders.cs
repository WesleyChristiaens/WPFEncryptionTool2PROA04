using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;

namespace WPFEncryptionTool2PROA04
{
    public class DefaultFolders
    {
        public static string Application = Environment.CurrentDirectory;

        public static string FolderIndex = Path.Combine(Application, "DefaultFolders.csv");
        public static string GeneratedAesKeys { get; set; }
        public static string DecryptedAesKeys { get; set; }
        public static string RsaEncryptedAesKeys { get; set; }
        public static string AesEncryptedImages { get; set; }
        public static string Images { get; set; }
        public static string RsaPublicKeys { get; set; }
        public static string RsaPrivateKeys { get; set; }

        public static Dictionary<string, string> FolderList =>
            new Dictionary<string, string>()
            {
                { nameof(GeneratedAesKeys), GeneratedAesKeys},
                { nameof(DecryptedAesKeys), DecryptedAesKeys},
                { nameof(RsaEncryptedAesKeys), RsaEncryptedAesKeys},
                { nameof(AesEncryptedImages), AesEncryptedImages},
                { nameof(Images), Images},
                { nameof(RsaPublicKeys), RsaPublicKeys},
                { nameof(RsaPrivateKeys), RsaPrivateKeys}
            };

        public static void Load()
        {
            if (!File.Exists(FolderIndex))
            {
                var file = File.Create(FolderIndex);
                file.Close();

                SaveToFile();
            }

            using (FileStream fs = new FileStream(FolderIndex, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] record = sr.ReadLine()?.Split(';');
                        foreach (var prop in typeof(DefaultFolders).GetProperties())
                        {
                            if (record != null && prop.Name == record[0])
                            {
                                prop.SetValue(prop, record[1]);
                            }
                        }
                    }
                }
            }
        }

        public static bool SaveToFile()
        {
            bool succeeded = false;

            if (!File.Exists(FolderIndex))
            {
                File.Create(FolderIndex);
            }

            using (FileStream fs = new FileStream(FolderIndex, FileMode.Open, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.WriteLine($"{nameof(GeneratedAesKeys)};{GeneratedAesKeys}");
                    sr.WriteLine($"{nameof(DecryptedAesKeys)};{DecryptedAesKeys}");
                    sr.WriteLine($"{nameof(RsaEncryptedAesKeys)};{RsaEncryptedAesKeys}");
                    sr.WriteLine($"{nameof(AesEncryptedImages)};{AesEncryptedImages}");
                    sr.WriteLine($"{nameof(Images)};{Images}");
                    sr.WriteLine($"{nameof(RsaPublicKeys)};{RsaPublicKeys}");
                    sr.WriteLine($"{nameof(RsaPrivateKeys)};{RsaPrivateKeys}");
                }

                succeeded = true;
            }

            return succeeded;
        }

        
    }
}