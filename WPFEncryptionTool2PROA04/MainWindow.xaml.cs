using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        }

        string generatedAESKeysFolder;


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {            

            if (!File.Exists(Folders.FolderIndex))
            {
                var file = File.Create(Folders.FolderIndex);                    
                file.Close();                
            }

            var content = File.ReadAllText(Folders.FolderIndex);           

            if (content.Contains(Folders.GeneratedAESKeys))
            {
                generatedAESKeysFolder = FileHelper.GetFolderPath(Folders.GeneratedAESKeys);
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

        }

        private void BtnGenerateAES_Click(object sender, RoutedEventArgs e)
        {
            CreateAesKey();
        }


        private void RSADecrypt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AESDecrypt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AESEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new AES_Encryption();
            wpf.ShowDialog();
        }

        private void RSAEncrypt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateAesKey()
        {
            using (Aes myAes = Aes.Create())
            {
                string key = Convert.ToBase64String(myAes.Key);
                string iv = Convert.ToBase64String(myAes.IV);
                string keyName = TxtName.Text; //input field from xaml
                StringBuilder sb = new StringBuilder();
                sb.Append($"{keyName};{key};{iv}");

                string fileName = @"C:\Github Repositories\WPFEncryptionTool2PROA04\test.txt"; //path name
                FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(sb);

                sw.Close();
                fs.Close();
            }
        }


    }
}
