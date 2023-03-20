using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media.Imaging;
using WPFEncryptionTool2PROA04.Models;

namespace WPFEncryptionTool2PROA04
{
    /// <summary>
    /// Interaction logic for AES_Encryption.xaml
    /// </summary>
    public partial class WpfAesEncryption : Window
    {
        public WpfAesEncryption()
        {
            InitializeComponent();
            LoadCbo();
        }
        private void LoadCbo()
        {
            CboAESKeys.Items.Clear();

           var path = FileHelper.GetFolderPath(Folders.GeneratedAESKeys);

            if (!string.IsNullOrEmpty(path))
            {
                CboAESKeys.ItemsSource = FileHelper.GetDirectoryContent(path);
            }
            else
            {
                CboAESKeys.Items.Add("no keys generated");
                CboAESKeys.SelectedIndex= 0;
            }
        }

        List<string> keyList = new List<string>();

        string selectedFileName = "";  
        

       
        private string[] ReadLineArray(string line)
        {
            string[] arrayLine = line.Split(';');
            return arrayLine;
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\";
            if (ofd.ShowDialog() == true)
            {
                selectedFileName = ofd.FileName;
                Uri fileUri = new Uri(ofd.FileName);
                ImgToEncrypt.Source = new BitmapImage(fileUri);
            }
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboAESKeys.SelectedIndex != -1)
                {
                    if (ImgToEncrypt.Source != null)
                    {
                        if (TxtFileName.Text != "")
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(selectedFileName);
                            string base64Image = Convert.ToBase64String(imageArray);
                            string keyToFind = CboAESKeys.SelectedItem.ToString();
                            byte[] AesKey;
                            byte[] IV;

                            foreach (var key in keyList)
                            {
                                string[] arrayKeys = ReadLineArray(key);
                                if (arrayKeys[0] == keyToFind)
                                {
                                    AesKey = Convert.FromBase64String(arrayKeys[1]);
                                    IV = Convert.FromBase64String(arrayKeys[2]);
                                    var encryptedImage = EncryptStringToBytes_Aes(base64Image, AesKey, IV);
                                    string encryptedImageBase64 = Convert.ToBase64String(encryptedImage);
                                    using (StreamWriter sw = File.CreateText(@"C:\Github Repositories\WPFEncryptionTool2PROA04\AESImages\" + TxtFileName.Text + ".txt")) //dynamic path encrypted AES images
                                    {
                                        sw.WriteLine(encryptedImageBase64);
                                    }
                                }
                            }
                            MessageBox.Show("Your image has been encrypted succesfully.");
                        }
                        else
                        {
                            MessageBox.Show("Fill in file name.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an image to encrypt.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a key.");
                }
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
