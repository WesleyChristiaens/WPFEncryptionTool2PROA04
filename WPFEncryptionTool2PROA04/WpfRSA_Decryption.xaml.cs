using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;

namespace WPFEncryptionTool2PROA04
{
    /// <summary>
    /// Interaction logic for RSA_Decryption.xaml
    /// </summary>
    public partial class WpfRsaDecryption : Window
    {
        public IEnumerable AesKeys { get; set; }

        public WpfRsaDecryption()
        {
            InitializeComponent();
            LoadComboboxes();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => this.Close();


        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            string cipherText = 
            (
                new FileHelper()
                    .WithFolder(DefaultFolders.RsaEncryptedAesKeys)
                    .WithFileName(CboAesKeys.SelectedItem.ToString() + ".txt")
                    .ReadFromFile()
            );

            byte[] dataToDecrypt = Convert.FromBase64String(cipherText);

            try
            {
                byte[] decryptedData;

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString
                    (
                        new FileHelper()
                            .WithFolder(DefaultFolders.RsaPrivateKeys)
                            .WithFileName(CboPrivateRsaKeys.SelectedItem.ToString() + ".xml")
                            .IsXmlFile(true)
                            .ReadFromFile()
                    );

                    decryptedData = rsa.Decrypt(dataToDecrypt, false);
                }

                new FileHelper()
                    .WithFolder(DefaultFolders.DecryptedAesKeys)
                    .WithFileName(TxtFileName.Text)
                    .WithContent(Encoding.UTF8.GetString(decryptedData))
                    .SaveToFile();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show($"Your AES key has been decrypted succesfully, and can be found at {DefaultFolders.DecryptedAesKeys}");
            ((MainWindow)Application.Current.MainWindow).LblDecrypt.Content = "Decrypt: AES key is decrypted";

            this.Close();
        }

        private void LoadComboboxes()
        {
            LoadCbo(CboAesKeys, DefaultFolders.RsaEncryptedAesKeys);
            LoadCbo(CboPrivateRsaKeys, DefaultFolders.RsaPrivateKeys);
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

        private bool ValidateInput()
        {
            if (CboPrivateRsaKeys.SelectedIndex == -1 ||
                CboPrivateRsaKeys.SelectedIndex.ToString() == "no keys generated")
            {
                MessageBox.Show("Select a private RSA key to decrypt");
                return false;
            }

            if (CboAesKeys.SelectedIndex == -1 || CboAesKeys.SelectedIndex.ToString() == "no encrypted images found")
            {
                MessageBox.Show("Select an image to decrypt");
                return false;
            }

            if (String.IsNullOrEmpty(TxtFileName.Text))
            {
                MessageBox.Show("Please specify file name");
                return false;
            }

            return true;
        }
    }
}