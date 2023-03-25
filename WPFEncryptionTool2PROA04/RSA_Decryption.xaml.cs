using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFEncryptionTool2PROA04.Models;

namespace WPFEncryptionTool2PROA04
{
    /// <summary>
    /// Interaction logic for RSA_Decryption.xaml
    /// </summary>
    public partial class RSA_Decryption : Window
    {
        public RSA_Decryption()
        {
            InitializeComponent();
            LoadComboboxes();
        }

        string folderRsaPrivateKeys;
        string folderEncryptedAESKeys;     
               

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
           
        }      



        private void LoadComboboxes()
        {
            LoadCbo(CboAESKeys, Folders.RSAEncryptedAESKeys);
            LoadCbo(CboprivateRSAKeys, Folders.RSAPrivateKeys);
        }

        private void LoadCbo(ComboBox cbo, string folder)
        {
            cbo.Items.Clear();

            var path = FileHelper.GetFolderPath(folder);
            var folderContent = FileHelper.GetDirectoryContent(path);

            if (!string.IsNullOrEmpty(path))
            {
                if (folderContent.Count() > 0)
                {
                    cbo.ItemsSource = FileHelper.GetDirectoryContent(path);
                }
                else
                {
                    cbo.Items.Add("no keys generated");
                    cbo.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBoxResult msg = MessageBox.Show
                    ($"Could not find predefined folder {nameof(folder)}", "Problem", MessageBoxButton.OK);
            }
        }
    }
}
