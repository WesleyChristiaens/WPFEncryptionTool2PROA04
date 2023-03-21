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
using System.Windows.Media.Imaging;
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
        }

        private void LoadCbo()
        {
            CboAESKeys.Items.Clear();

            var path = FileHelper.GetFolderPath(Folders.GeneratedAESKeys);
            var folderContent = FileHelper.GetDirectoryContent(path);

            if (folderContent.Count() > 0)
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
            this.Close();
        }

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboAESKeys.SelectedIndex == -1)
                {
                    MessageBox.Show("Select an AES key to decrypt");
                    return;
                }
                if (CboImages.SelectedIndex == -1)
                {
                    MessageBox.Show("Select an image to decrypt");
                    return;
                }
                if (String.IsNullOrEmpty(FileHelper.GetFolderPath(Folders.Images)))
                {
                    MessageBox.Show("Please set standard folders first");
                    return;
                }
                if (String.IsNullOrEmpty(TxtFileName.Text))
                {
                    MessageBox.Show("Please specify file name");
                    return;
                }

                var encryptedImage = getImage();
                var selectedkey = CboAESKeys.SelectedItem.ToString();
                AesKey aesKey = FileHelper.GetAesKey(Folders.GeneratedAESKeys, selectedkey);
                var decryptedImage = DecryptStringFromBytes_Aes(encryptedImage, aesKey);
                SaveImage(decryptedImage);
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveImage(byte[] imageByteArray)
        {
            string folderPath = FileHelper.GetFolderPath(Folders.Images);
            string filePath = Path.Combine(folderPath, TxtFileName.Text);
            using (Image image = Image.FromStream(new MemoryStream(imageByteArray)))
            {
                image.Save(filePath + ".jpg", ImageFormat.Jpeg);
            }

            Uri fileUri = new Uri(filePath + ".jpg");
            ImgToEncrypt.Source = new BitmapImage(fileUri);
        }

        private byte[] getImage()
        {
            
            var path = Path.Combine(FileHelper.GetFolderPath(Folders.AESEncryptedImages), CboImages.SelectedItem.ToString());
            string encryptedImage = FileHelper.ReadStringFromCsv(path);

            return Convert.FromBase64String(encryptedImage);
        }

        static byte[] DecryptStringFromBytes_Aes(byte[] cipherText, AesKey aesKey)
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
                aesAlg.Key = Convert.FromBase64String(aesKey.Key);
                aesAlg.IV = Convert.FromBase64String(aesKey.InitiationVector);

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

            return Convert.FromBase64String(plaintext);
        }
    }
}
