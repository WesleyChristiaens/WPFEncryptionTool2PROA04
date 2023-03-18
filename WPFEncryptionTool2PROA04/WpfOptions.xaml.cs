using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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

        IEnumerable<Folder> folders;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            folders = ReadFolderIndex();
            LoadFoldersinTextBox();            
        }
        private void FolderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();

            if (fbd.SelectedPath.Any())
            {
                ShowSelectedFolder(btn.Name,fbd.SelectedPath);
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            SaveFolders();

        }

        private void SaveFolders()
        {
            var result = FileHelper.WriteCsv(GetContentOfTextBoxes(), Folders.FolderIndex);
            MessageBoxResult message;

            if (result.Succeeded)
            {
                message = MessageBox.Show("Folders stored succesfully", "Success", MessageBoxButton.OK);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    sb.AppendLine(item);
                }

                message = MessageBox.Show($"{sb.ToString()}", "Error", MessageBoxButton.OK);
            }

            if (message == MessageBoxResult.OK)
            { 
                this.Close(); 
            }
        }

        private IEnumerable<Folder> GetContentOfTextBoxes()
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

                            lst.Add(new Folder() { Name = textbox.Name.Substring(3), Path = textbox.Text });

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

                            foreach (var folder in folders)
                            {
                                if ($"Txt{folder.Name}" == textbox.Name)
                                {
                                    textbox.Text = folder.Path;
                                }
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<Folder> ReadFolderIndex()
        {
            IEnumerable<Folder> lst = new List<Folder>();
            
            if (File.ReadAllText(Folders.FolderIndex) != string.Empty)
            {
                lst = FileHelper.ReadCsv<Folder>(Folders.FolderIndex).records;
            }

            return lst.ToList();
        }

        private void ShowSelectedFolder(string btnName,string selectedPath)
        {
            switch (btnName)
            {
                case "BtnGeneratedAESKeys":
                    TxtGeneratedAESKeys.Text = selectedPath;
                    break;
                case "BtnDecryptedAESKeys":
                    TxtDecryptedAESKeys.Text = selectedPath;
                    break;
                case "BtnRSAEncryptedAESKeys":
                    TxtRSAEncryptedAESKeys.Text = selectedPath;
                    break;
                case "BtnAESEncryptedImages":
                    TxtAESEncryptedImages.Text = selectedPath;
                    break;
                case "BtnImages":
                    TxtImages.Text = selectedPath;
                    break;
                case "BtnRSAPublicKeys":
                    TxtRSAPublicKeys.Text = selectedPath;
                    break;
                case "BtnRSAPrivateKeys":
                    TxtRSAPrivateKeys.Text = selectedPath;
                    break;
                default:
                    break;
            }
        }


    }
}
