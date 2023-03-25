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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    //get encrypted AESkey from file
                    string aesKey = FileHelper.GetKey(Folders.RSAEncryptedAESKeys, CboAESKeys.SelectedItem.ToString());

                    //get private RSA key from file
                    string rsaKey = FileHelper.GetKey(Folders.RSAPrivateKeys, CboprivateRSAKeys.SelectedItem.ToString());

                    //decrypt AESkey using RSA private key   
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        // https://t-phitakgul.medium.com/c-rsa-encryption-decryption-with-my-own-key-dab2d1f4df1b
                    }

                    //write decrypted AES-key to file
                    AesKey decryptedkey = new AesKey();

                    StoreDecryptedKey(decryptedkey);

                }
                catch (CryptographicException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }

        private void StoreDecryptedKey(AesKey decryptedkey)
        {
            var saveresult = FileHelper.StoreAesKey(Folders.DecryptedAESKeys, decryptedkey);
            if (saveresult.Succeeded)
            {
                MessageBox.Show("Decryption was succesfull");
            }
            else
            {
                MessageBox.Show(saveresult.Errors.ToString());
            }
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

        private bool ValidateInput()
        {          
            if (CboprivateRSAKeys.SelectedIndex == -1 || CboprivateRSAKeys.SelectedIndex.ToString() == "no keys generated")
            {
                MessageBox.Show("Select a private RSA key to decrypt");
                return false;
                
            }
            if (CboAESKeys.SelectedIndex == -1 || CboAESKeys.SelectedIndex.ToString() == "no encrypted images found")
            {
                MessageBox.Show("Select an image to decrypt");
                return false;
            }
            if (String.IsNullOrEmpty(FileHelper.GetFolderPath(Folders.DecryptedAESKeys)))
            {
                MessageBox.Show($"No folder set for '{nameof(Folders.DecryptedAESKeys)}'");
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
