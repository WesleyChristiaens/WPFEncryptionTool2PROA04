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
    public partial class WpfRSA_Decryption : Window
    {
        public WpfRSA_Decryption()
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

                    using (var rsa = new RSACryptoServiceProvider(1024))
                    {
                        try
                        {
                            var base64Encrypted = File.ReadAllText(Path.Combine
                                (Folders.RSAEncryptedAESKeys, CboAESKeys.SelectedItem.ToString()));

                            // server decrypting data with private key                    
                            rsa.FromXmlString(File.ReadAllText(
                                Path.Combine(
                                    Folders.RSAPrivateKeys, CboprivateRSAKeys.SelectedItem.ToString())));

                            var resultBytes = Convert.FromBase64String(base64Encrypted);
                            var decryptedBytes = rsa.Decrypt(resultBytes, true);
                            var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                            decryptedData.ToString();
                        }
                        finally
                        {
                            rsa.PersistKeyInCsp = false;
                        }
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
                cbo.Items.Add("no keys available");
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
