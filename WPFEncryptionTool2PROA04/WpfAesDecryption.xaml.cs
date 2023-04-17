using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;

namespace WPFEncryptionTool2PROA04
{
    public partial class WpfAesDecryption : Window
    {
        public WpfAesDecryption()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            LoadCbo(CboAESKeys, DefaultFolders.GeneratedAesKeys);
            LoadCbo(CboImages, DefaultFolders.AesEncryptedImages);
        }

        private void LoadCbo(ComboBox cbo, string folderpath)
        {
            cbo.Items.Clear();

            var folderContent = new FileHelper()
                .WithFolder(folderpath)
                .GetDirectoryContent().ToList();

            if (!folderContent.Any())
            {
                cbo.Items.Add("no keys generated");
                cbo.SelectedIndex = 0;
                return;
            }

            cbo.ItemsSource = folderContent;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => this.Close();

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboAESKeys.SelectedIndex == -1 || CboAESKeys.SelectedIndex.ToString() == "no keys generated")
                {
                    MessageBox.Show("Select an AES key to decrypt");
                    return;
                }

                if (CboImages.SelectedIndex == -1 || CboImages.SelectedIndex.ToString() == "no encrypted images found")
                {
                    MessageBox.Show("Select an image to decrypt");
                    return;
                }

                if (String.IsNullOrEmpty(TxtFileName.Text))
                {
                    MessageBox.Show("Please specify file name");
                    return;
                }

                var encryptedImage = getImage();

                string[] aesCredentials = new FileHelper()
                    .WithFolder(DefaultFolders.GeneratedAesKeys)
                    .WithFileName(CboAESKeys.SelectedItem.ToString())
                    .ReadFromFile().Split(';');

                string aesKey = aesCredentials[0];
                string aesIv = aesCredentials[1];

                var decryptedImage = DecryptStringFromBytes_Aes(encryptedImage, aesKey, aesIv);

                SaveImage(decryptedImage);
               
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBox.Show($"Your image has been decrypted succesfully, and can be found at {DefaultFolders.Images}");
            this.Close();
        }

        private void SaveImage(byte[] imageByteArray)
        {
            if (string.IsNullOrEmpty(DefaultFolders.Images))
            {
                MessageBox.Show($"Please set standard folder for {nameof(DefaultFolders.Images)} first.");
                var wpf = new WpfOptions();
                wpf.ShowDialog();
            }

            string filePath = Path.Combine(DefaultFolders.Images, TxtFileName.Text);

            using (Image image = Image.FromStream(new MemoryStream(imageByteArray)))
            {
                image.Save(filePath + ".jpg", ImageFormat.Jpeg);
            }

            Uri fileUri = new Uri(filePath + ".jpg");
            ImgToEncrypt.Source = new BitmapImage(fileUri);
        }

        private byte[] getImage()
        {
            string encryptedImage = new FileHelper()
                .WithFolder(DefaultFolders.AesEncryptedImages)
                .WithFileName(CboImages.SelectedItem.ToString())
                .ReadFromFile();

            if (string.IsNullOrEmpty(encryptedImage))
            {
                MessageBox.Show("Image not found");
            }

            return Convert.FromBase64String(encryptedImage);
        }

        static byte[] DecryptStringFromBytes_Aes(byte[] cipherText, string aesKey, string aesIv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (aesKey == null || aesKey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (aesIv == null || aesIv.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(aesKey);
                aesAlg.IV = Convert.FromBase64String(aesIv);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
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