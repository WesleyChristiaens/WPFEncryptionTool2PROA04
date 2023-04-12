using System;
using System.IO;
using System.Text;
using System.Windows;

namespace WPFEncryptionTool2PROA04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DefaultFolders.Load();
        }

        private void MnuOptions_Click(object sender, RoutedEventArgs e)
            => new WpfOptions().ShowDialog();

        private void MnuClose_Click(object sender, RoutedEventArgs e)
            => this.Close();

        private void BtnGenerateRSA_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateTextBoxinput(TxtName.Text))
            {
                return;
            }

            try
            {
                Keygen.GenerateNewRSaKeypair(TxtName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBox.Show(
                $"RsaKeypair successfully generated" +
                $"{Environment.NewLine}" +
                $" @{DefaultFolders.RsaPublicKeys} &" +
                $"{Environment.NewLine}" +
                $" {DefaultFolders.RsaPrivateKeys}");

            TxtName.Text = string.Empty;
        }

        private void BtnGenerateAES_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateTextBoxinput(TxtName.Text))
            {
                return;
            }

            try
            {
                Keygen.GenerateNewAESKey(TxtName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBox.Show(
                $"AesKey successfully generated" +
                $"{Environment.NewLine}" +
                $"@{DefaultFolders.GeneratedAesKeys}");

            
        }

        private void RSADecrypt_Click(object sender, RoutedEventArgs e)
            => new WpfRsaDecryption().ShowDialog();

        private void AESDecrypt_Click(object sender, RoutedEventArgs e)
            => new WpfAesDecryption().ShowDialog();


        private void AESEncrypt_Click(object sender, RoutedEventArgs e)
            => new WpfAesEncryption().ShowDialog();

        private void RSAEncrypt_Click(object sender, RoutedEventArgs e)
            => new RSA_Encryption().ShowDialog();

        private bool ValidateTextBoxinput(string input)
        {
            bool validation = true;

            if (input == string.Empty)
            {
                MessageBox.Show("Please enter a name for your new key");
                validation = false;
            }

            if (input.Contains("."))
            {
                MessageBox.Show("Character '.' is not allowed in the name ");
                validation = false;
            }

            return validation;
        }
    }
}