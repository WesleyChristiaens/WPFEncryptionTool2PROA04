using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using WPFEncryptionTool2PROA04.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WPFEncryptionTool2PROA04
{
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
            var folderContent = FileHelper.GetDirectoryContent(path);

            if (folderContent.Count() > 0)
            {
                CboAESKeys.ItemsSource = FileHelper.GetDirectoryContent(path);
            }
            else
            {
                CboAESKeys.Items.Add("no keys generated");
                CboAESKeys.SelectedIndex= 0;
            }
        }

        string selectedFileName = "";         

        static byte[] EncryptStringToBytes_Aes(string plainText, AesKey aesKey)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (aesKey.Key == null || aesKey.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (aesKey.InitiationVector == null || aesKey.InitiationVector.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(aesKey.Key);
                aesAlg.IV = Convert.FromBase64String(aesKey.InitiationVector);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        //moet deze uit de map images komen of mag dat random op je pc zijn?
        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\";
            ofd.Filter = "Image Files(*.JPG)|*.JPG;|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                selectedFileName = ofd.FileName;
                Uri fileUri = new Uri(ofd.FileName);
                ImgToEncrypt.Source = new BitmapImage(fileUri);
            }
        }

        private string GetBase64Image()
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(selectedFileName);
            string base64String = Convert.ToBase64String(imageArray);
            return base64String;
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboAESKeys.SelectedIndex == -1 || CboAESKeys.SelectedIndex.ToString() == "no keys generated")
                {
                    MessageBox.Show("Select an AES key to encrypt");
                    return;
                }
                if (String.IsNullOrEmpty(TxtFileName.Text))
                {
                    MessageBox.Show("Please set a name for your encrypted image");
                    return;
                }
                if (ImgToEncrypt.Source == null)
                {
                    MessageBox.Show("Please select an image that you want to encrypt");
                    return;
                }
                if (String.IsNullOrEmpty(FileHelper.GetFolderPath(Folders.AESEncryptedImages)))
                {
                    MessageBox.Show("Please set standard folders first");
                    return;
                }

                string base64Image = GetBase64Image();
                var selectedkey = CboAESKeys.SelectedItem.ToString();
                AesKey aesKey = FileHelper.GetAesKey(Folders.GeneratedAESKeys, selectedkey);
                var encryptedImage = EncryptStringToBytes_Aes(base64Image, aesKey);
                SaveEncryptedImage(encryptedImage);
                MessageBox.Show("Your image has been encrypted succesfully");
            }
            catch (CryptographicException ex)
            {
                MessageBox.Show(ex.Message);
            }       
        }

        private void SaveEncryptedImage(byte[] imageByteArray)
        {
            string folderPath = FileHelper.GetFolderPath(Folders.AESEncryptedImages);
            string filePath = Path.Combine(folderPath, TxtFileName.Text);
            FileHelper.WriteStringToCsv(Convert.ToBase64String(imageByteArray), filePath);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
