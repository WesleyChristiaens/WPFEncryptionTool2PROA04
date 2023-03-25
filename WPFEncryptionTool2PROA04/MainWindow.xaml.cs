using System;
using System.IO;
using System.Text;
using System.Windows;
using WPFEncryptionTool2PROA04.Models;

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
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(Folders.FolderIndex))
            {
                var file = File.Create(Folders.FolderIndex);
                file.Close();
            }
        }

        private void MnuOptions_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfOptions();
            wpf.ShowDialog();
        }

        private void MnuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnGenerateRSA_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTextBoxinput(TxtName.Text))
            {
                var rkp = Keygen.GenerateNewRSaKeypair(TxtName.Text);
                ShowOperationResult(FileHelper.StoreRSAKeyPair(rkp));
            }
        }

        private void BtnGenerateAES_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTextBoxinput(TxtName.Text))
            {
                var aesKey = Keygen.GenerateNewAESKey(TxtName.Text);
                ShowOperationResult(FileHelper.StoreAesKey(Folders.GeneratedAESKeys,aesKey));
            }          
        }

        private void RSADecrypt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AESDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfAesDecryption();
            wpf.ShowDialog();
        }

        private void AESEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfAesEncryption();
            wpf.ShowDialog();
        }

        private void RSAEncrypt_Click(object sender, RoutedEventArgs e)
        {

        }



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

        private void ShowOperationResult(SaveResult result)
        {
            if (result.Succeeded)
            {
                MessageBox.Show("Key succesfully generated", "Operation succesfull", MessageBoxButton.OK);
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    sb.AppendLine(item);
                    sb.Append(Environment.NewLine);
                }

                MessageBox.Show($"{sb.ToString()}", "Error", MessageBoxButton.OK);
            }
        }








    }
}
