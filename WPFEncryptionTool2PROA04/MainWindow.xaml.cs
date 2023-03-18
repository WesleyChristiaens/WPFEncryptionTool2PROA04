using System.IO;
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
             
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {  
            if (!File.Exists(Folders.FolderIndex))
            {
                var file = File.Create(Folders.FolderIndex);                    
                file.Close();                
            }

            FileHelper.GetKeys(Folders.FolderIndex);
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
            Encryptor.GenerateNewRSaKeypair(TxtName.Text);


        }

        private void BtnGenerateAES_Click(object sender, RoutedEventArgs e)
        {            
            Encryptor.GenerateNewAESKey(TxtName.Text);
        }


        private void RSADecrypt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AESDecrypt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AESEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfAesEncryption();
            wpf.ShowDialog();
        }

        private void RSAEncrypt_Click(object sender, RoutedEventArgs e)
        {
            
        }

       

        

        


    }
}
