using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WPFEncryptionTool2PROA04.Models;

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
        }

        Dictionary<string, string> folders;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(FileHelper.FolderIndex))
            {
                SaveFolders();
            }

            ReadFolderIndex();
            LoadFoldersinTextBox();
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
                        break;
                    case "BtnDecryptedAESKeys":
                        TxtDecryptedAESKeys.Text = fbd.SelectedPath;
                        break;
                    case "BtnRSAEncryptedAESKeys":
                        TxtRSAEncryptedAESKeys.Text = fbd.SelectedPath;
                        break;
                    case "BtnAESEncryptedImages":
                        TxtAESEncryptedImages.Text = fbd.SelectedPath;
                        break;
                    case "BtnImages":
                        TxtImages.Text = fbd.SelectedPath;
                        break;
                    case "BtnRSAPublicKeys":
                        TxtRSAPublicKeys.Text = fbd.SelectedPath;
                        break;
                    case "BtnRSAPrivateKeys":
                        TxtRSAPrivateKeys.Text = fbd.SelectedPath;
                        break;
                    default:
                        break;
                }

            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (SaveFolders())
            {
                MessageBoxResult message = MessageBox.Show("Folders stored succesfully", "Success", MessageBoxButton.OK);

                if (message == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }

        }

        private bool SaveFolders()
        {
            bool result = false;

            try
            {
                FileHelper.WriteToCsv(GetContentOfTextBoxes());
                result = true;
            }
            catch (Exception ex)
            {
                MessageBoxResult r = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);

                if (r == MessageBoxResult.OK)
                {
                    this.Close();
                }
            }

            return result;
        }

        private List<Folder> GetContentOfTextBoxes()
        {
            //Reads the content of all the textboxes to a List

            List<Folder> lst = new List<Folder>();

            foreach (var child in Stack.Children)
            {
                if (child.GetType().Equals(typeof(WrapPanel)))
                {
                    var panel = child as WrapPanel;

                    foreach (var element in panel.Children)
                    {
                        if (element.GetType().Equals(typeof(TextBox)))
                        {
                            var textbox = element as TextBox;
                            if (!string.IsNullOrEmpty(textbox.Text))
                            {
                                lst.Add(new Folder() { Name = textbox.Name.Substring(3), path = textbox.Text });
                            }
                            else
                            {
                                lst.Add(new Folder() { Name = textbox.Name.Substring(3) });
                            }
                        }
                    }
                }
            }

            return lst;
        }

        private void LoadFoldersinTextBox()
        {
            foreach (var child in Stack.Children)
            {
                if (child.GetType().Equals(typeof(WrapPanel)))
                {
                    var panel = child as WrapPanel;

                    foreach (var element in panel.Children)
                    {
                        if (element.GetType().Equals(typeof(TextBox)))
                        {
                            var textbox = element as TextBox;

                            if (folders.ContainsKey(textbox.Name))
                            {
                                textbox.Text = folders[textbox.Name];
                            }
                        }
                    }
                }
            }
        }

        private void ReadFolderIndex()
        {
            folders = new Dictionary<string, string>();

            var records = FileHelper.ReadCsv<Folder>(FileHelper.FolderIndex);

            foreach (var record in records)
            {
                if (folders.Keys.Contains($"Txt{record.Name}"))
                {
                    folders[record.Name] = record.path;
                }
                else
                {
                    folders.Add($"Txt{record.Name}", record.path);
                }
            }
        }
    }
}
