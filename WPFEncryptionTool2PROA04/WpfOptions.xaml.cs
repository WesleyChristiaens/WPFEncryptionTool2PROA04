using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

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
            LoadTextBoxes();
        }

        private void LoadTextBoxes()
        {
            FolderStack.Children.Clear();

            foreach (var folder in DefaultFolders.FolderList)
            {
                FolderStack.Children.Add(CreateFolderPanel(folder.Key, folder.Value));
            }
        }

        private WrapPanel CreateFolderPanel(string name,string folderpath)
        {
            WrapPanel wp = new WrapPanel();
            wp.Margin= new Thickness(0,5,0,5);
            wp.HorizontalAlignment = HorizontalAlignment.Right;

            Label lbl = new Label();
            lbl.Content = name;

            wp.Children.Add(lbl);

            TextBox tb = new TextBox();
            tb.Name = $"Txt{name}";
            tb.Text = folderpath;
            tb.Width = 300;
            tb.Margin = new Thickness(5,0,5,0);
            tb.LostFocus += Tb_LostFocus;

            wp.Children.Add(tb);

            Button btn = new Button();
            btn.Name= $"Btn{name}";
            btn.Content = "...";
            btn.Width = 25;
            btn.Click += FolderSelectBtn_Click;

            wp.Children.Add(btn);

            return wp;

        }

        private void Tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            var prop = typeof(DefaultFolders).GetProperty(tb.Name.Remove(0, 3));
            prop.SetValue(prop,tb.Text);
        }       

        private void FolderSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();  
            WrapPanel panel = btn.Parent as WrapPanel;
            TextBox tb = panel.Children[1] as TextBox;

            fbd.ShowDialog();

            if (fbd.SelectedPath.Any())
            {
                tb.Text = fbd.SelectedPath;
                tb.Focus();
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DefaultFolders.SaveToFile())
            {
                MessageBox.Show("Folders succesfully saved");
                this.Close();
            }
            else
            {
                MessageBox.Show($"Something went wrong saving the folderpaths, please check {DefaultFolders.FolderIndex}.");
            }
           
        }             

    }
}
