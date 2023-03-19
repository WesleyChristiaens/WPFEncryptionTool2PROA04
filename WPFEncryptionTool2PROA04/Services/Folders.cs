using System;
using System.IO;

namespace WPFEncryptionTool2PROA04
{
    public class Folders
    {
        public static string Application = Environment.CurrentDirectory;
        public static string FolderIndex = Path.Combine(Folders.Application, "DefaultFolders.csv");
        public const string GeneratedAESKeys = "GeneratedAESKeys";
        public const string DecryptedAESKeys = "DecryptedAESKeys";
        public const string RSAEncryptedAESKeys = "RSAEncryptedAESKeys";
        public const string AESEncryptedImages = "AESEncryptedImages";
        public const string Images = "Images";
        public const string RSAPublicKeys = "RSAPublicKeys";
        public const string RSAPrivateKeys = "RSAPrivateKeys";
    }
}
