using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEncryptionTool2PROA04
{
    public class Folders
    {
        public static string FolderIndex = Path.Combine(Environment.CurrentDirectory, "DefaultFolders.csv");
        public const string GeneratedAESKeys = "GeneratedAESKeys";
        public const string DecryptedAESKeys = "DecryptedAESKeys";
        public const string RSAEncryptedAESKeys = "RSAEncryptedAESKeys";
        public const string AESEncryptedImages = "AESEncryptedImages";
        public const string Images = "Images";
        public const string RSAPublicKeys = "RSAPublicKeys";
        public const string RSAPrivateKeys = "RSAPrivateKeys";
    }
}
