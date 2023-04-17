using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WPFEncryptionTool2PROA04
{
    public partial class RSA_Encryption : Window
    {
        public RSA_Encryption()
        {
            InitializeComponent();
            LoadCbo();
        }

        private void LoadCbo()
        {
            CboAESKey.Items.Clear();
            CboRSAKey.Items.Clear();

            //opvullen combobox AES keys
            var folderContentAESKeys = new FileHelper()
                .WithFolder(DefaultFolders.GeneratedAesKeys)
                .GetDirectoryContent().ToList();

            if (!folderContentAESKeys.Any())
            {
                CboAESKey.Items.Add("no keys generated");
                CboAESKey.SelectedIndex = 0;
                return;
            }

            CboAESKey.ItemsSource = folderContentAESKeys;

            //opvullen combobox public RSA keys
            var folderContentRSAKeys = new FileHelper()
                .WithFolder(DefaultFolders.RsaPublicKeys)
                .GetDirectoryContent().ToList();

            if (!folderContentRSAKeys.Any())
            {
                CboRSAKey.Items.Add("no keys generated");
                CboRSAKey.SelectedIndex = 0;
                return;
            }

            CboRSAKey.ItemsSource = folderContentRSAKeys;
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboAESKey.SelectedIndex != -1 || CboRSAKey.SelectedIndex != -1)
                {
                    string aesCredentials = new FileHelper()
                        .WithFolder(DefaultFolders.GeneratedAesKeys)
                        .WithFileName(CboAESKey.SelectedItem.ToString() + ".txt")
                        .ReadFromFile();

                    byte[] dataToEncrypt = Encoding.Default.GetBytes(aesCredentials);
                    byte[] encryptedData;

                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {           
                        RSA.FromXmlString
                        (
                            new FileHelper()
                            .WithFolder(DefaultFolders.RsaPublicKeys)
                            .WithFileName(CboRSAKey.SelectedItem.ToString() + ".xml")
                            .IsXmlFile(true)
                            .ReadFromFile()
                        );
                        var param = RSA.ExportParameters(false);
                        encryptedData = RSA.Encrypt(dataToEncrypt, false);
                    }

                    new FileHelper()
                        .WithFolder(DefaultFolders.RsaEncryptedAesKeys)
                        .WithFileName(TxtSaveEncryptedAESKeyAs.Text)
                        .WithContent(Convert.ToBase64String(encryptedData))
                        .SaveToFile();

                }
                else
                {
                    MessageBox.Show("Please select an public RSA key and AES key to encrypt.");
                }
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show($"Your AES key has been encrypted succesfully, and can be found at {DefaultFolders.RsaEncryptedAesKeys}");
            ((MainWindow)Application.Current.MainWindow).LblEncrypt.Content = "Encrypt: AES key is encrypted";

            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
