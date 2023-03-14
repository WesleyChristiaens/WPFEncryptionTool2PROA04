using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFEncryptionTool2PROA04
{
    /// <summary>
    /// Interaction logic for WpfOptions.xaml
    /// </summary>
    public partial class WpfOptions : Window
    {
        public WpfOptions()
        {
            InitializeComponent();
            /*GetDefaultFolder();*/
        }

        

        private void FolderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();

            if (fbd.SelectedPath.Any())
            {
                switch (btn.Name)
                {
                    case "BtnGeneratedAESKeys":
                        TxtGeneratedAESKeys.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    case "BtnDecryptedAESKeys":
                        TxtDecryptedAESKeys.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    case "BtnRSAEncryptedAESKeys":
                        TxtRSAEncryptedAESKeys.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    case "BtnAESEncryptedImages":
                        TxtAESEncryptedImages.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    case "BtnImages":
                        TxtImages.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    case "BtnRSAPublicKeys":
                        TxtRSAPublicKeys.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    case "BtnRSAPrivateKeys":
                        TxtRSAPrivateKeys.Text = fbd.SelectedPath;
                        Folders.Default.Add(btn.Name.Substring(3), fbd.SelectedPath);
                        break;
                    default:
                        break;
                }

            }

        }

        private void GetDefaultFolder()
        {
            if (Folders.Default != null)
            {
                
                TxtGeneratedAESKeys.Text = Folders.Default["GeneratedAESKeys"];
                TxtDecryptedAESKeys.Text = Folders.Default["DecryptedAESKeys"];
                TxtRSAEncryptedAESKeys.Text = Folders.Default["RSAEncryptedAESKeys"];
                TxtAESEncryptedImages.Text = Folders.Default["AESEncryptedImages"];
                TxtImages.Text = Folders.Default["Images"];
                TxtRSAPublicKeys.Text = Folders.Default["RSAPublicKeys"];
                TxtRSAPrivateKeys.Text = Folders.Default["RSAPrivateKeys"];
            }            
        }




        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var kvp in Folders.Default)
            {
                sb.AppendLine($"{kvp.Key};{kvp.Value}");
            }
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "DefaultFolders.csv"),sb.ToString());

            this.Close();

        }
    }
}
