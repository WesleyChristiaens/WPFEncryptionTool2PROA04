using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFEncryptionTool2PROA04.Models;
using Image = System.Drawing.Image;

namespace WPFEncryptionTool2PROA04
{
    public partial class WpfAesDecryption : Window
    {
        public WpfAesDecryption()
        {
            InitializeComponent();
            LoadCbo();
            LoadImagesCbo();
            getImage();
        }

        private void LoadCbo()
        {
            CboAESKeys.Items.Clear();

            var path = FileHelper.GetFolderPath(Folders.GeneratedAESKeys);

            if (!string.IsNullOrEmpty(path))
            {
                CboAESKeys.ItemsSource = FileHelper.GetDirectoryContent(path);
            }
            else
            {
                CboAESKeys.Items.Add("no keys generated");
                CboAESKeys.SelectedIndex = 0;
            }
        }

        private void LoadImagesCbo()
        {
            CboImages.Items.Clear();

            var path = FileHelper.GetFolderPath(Folders.AESEncryptedImages);

            if (!string.IsNullOrEmpty(path))
            {
                CboImages.ItemsSource = FileHelper.GetDirectoryContent(path);
            }
            else
            {
                CboImages.Items.Add("no encrypted images found");
                CboImages.SelectedIndex = 0;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var encryptedImage = getImage();

            var aesKey = "";
            var decryptedImage = DecryptStringFromBytes_Aes(encryptedImage, aesKey);

            byte[] imageByteArray = Convert.FromBase64String(decryptedImage);
            SaveImage(imageByteArray);
        }

        private void SaveImage(byte[] imageByteArray)
        {
            using (Image image = Image.FromStream(new MemoryStream(imageByteArray)))
            {
                image.Save("output.jpg", ImageFormat.Jpeg);
            }
        }

        private string getImage()
        {
            string encryptedImage = "";
            var path = FileHelper.GetFolderPath(Folders.GeneratedAESKeys);
            var array = FileHelper.GetDirectoryContent(path).Where(x => x.Equals(CboImages.SelectedItem));

            
            return encryptedImage;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, AesKey aesKey)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (aesKey.Key == null || aesKey.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (aesKey.InitiationVector == null || aesKey.InitiationVector.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(aesKey.Key);
                aesAlg.IV = Encoding.ASCII.GetBytes(aesKey.InitiationVector);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
