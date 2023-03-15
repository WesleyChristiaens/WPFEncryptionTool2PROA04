using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
