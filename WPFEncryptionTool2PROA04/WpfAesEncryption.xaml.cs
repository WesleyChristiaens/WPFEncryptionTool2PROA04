using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPFEncryptionTool2PROA04
{
    public partial class WpfAesEncryption : Window
    {
        private string imageAsPlaintext;
        

        public WpfAesEncryption()
        {
            InitializeComponent();
            LoadCbo();
        }
        private void LoadCbo()
        {
            CboAESKeys.Items.Clear();

            var folderContent = new FileHelper()
                .WithFolder(DefaultFolders.GeneratedAesKeys)
                .GetDirectoryContent().ToList();

            if (!folderContent.Any())
            {
                CboAESKeys.Items.Add("no keys generated");
                CboAESKeys.SelectedIndex = 0;
                return;
            }

            CboAESKeys.ItemsSource = folderContent;
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\";
            ofd.Filter = "Image Files(*.JPG)|*.JPG;|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                ImgToEncrypt.Source = new BitmapImage(new Uri(ofd.FileName));

                byte[] imageArray = System.IO.File.ReadAllBytes(ofd.FileName);

                imageAsPlaintext = Convert.ToBase64String(imageArray);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => this.Close();

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    string[] aesCredentials = new FileHelper()
                        .WithFolder(DefaultFolders.GeneratedAesKeys)
                        .WithFileName(CboAESKeys.SelectedItem.ToString())
                        .ReadFromFile().Split(';');

                    string aesKey = aesCredentials[0];
                    string aesIv = aesCredentials[1];

                    var encryptedImage = EncryptStringToBytes_Aes(imageAsPlaintext, aesKey, aesIv);

                    new FileHelper()
                        .WithFolder(DefaultFolders.AesEncryptedImages)
                        .WithFileName(TxtFileName.Text)
                        .WithContent(Convert.ToBase64String(encryptedImage))
                        .SaveToFile();
                }
                catch (CryptographicException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                MessageBox.Show(
                    $"Your image has been encrypted succesfully, and can be found at {DefaultFolders.AesEncryptedImages}");
                ((MainWindow)Application.Current.MainWindow).LblEncrypt.Content = "Encrypt: image is encrypted";

                this.Close();

            }
        }

        private bool ValidateInput()
        {
            if (CboAESKeys.SelectedIndex == -1 || CboAESKeys.SelectedIndex.ToString() == "no keys generated")
            {
                MessageBox.Show("Select an AES key to encrypt");
                return false;
            }
            if (String.IsNullOrEmpty(TxtFileName.Text))
            {
                MessageBox.Show("Please set a name for your encrypted image");
                return false;
            }
            if (ImgToEncrypt.Source == null)
            {
                MessageBox.Show("Please select an image that you want to encrypt");
                return false;
            }

            return true;
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, string aesKey, string aesIv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (aesKey == null || aesKey.Length <= 0)
                throw new ArgumentNullException("Key");
            if (aesIv == null || aesIv.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(aesKey);
                aesAlg.IV = Convert.FromBase64String(aesIv);


                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

               
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        

        
    }
}
