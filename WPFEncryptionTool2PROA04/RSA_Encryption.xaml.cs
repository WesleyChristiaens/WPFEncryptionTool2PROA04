using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WPFEncryptionTool2PROA04
{
    /// <summary>
    /// Interaction logic for RSA_Encryption.xaml
    /// </summary>
    public partial class RSA_Encryption : Window
    {

        public static string rsaKey = "MIIBOgIBAAJBAKj34GkxFhD90vcNLYLInFEX6Ppy1tPf9Cnzj4p4WGeKLs1Pt8Qu\r\nKUpRKfFLfRYC9AIKjbJTWit+CqvjWYzvQwECAwEAAQJAIJLixBy2qpFoS4DSmoEm\r\no3qGy0t6z09AIJtH+5OeRV1be+N4cDYJKffGzDa88vQENZiRm0GRq6a+HPGQMd2k\r\nTQIhAKMSvzIBnni7ot/OSie2TmJLY4SwTQAevXysE2RbFDYdAiEBCUEaRQnMnbp7\r\n9mxDXDf6AU0cN/RPBjb9qSHDcWZHGzUCIG2Es59z8ugGrDY+pxLQnwfotadxd+Uy\r\nv/Ow5T0q5gIJAiEAyS4RaI9YG8EWx/2w0T67ZUVAw8eOMB6BIUg0Xcu+3okCIBOs\r\n/5OiPgoTdSy7bcF9IGpSE8ZgGKzgYQVZeN97YE00";
        public static string aesKey = "taDyfoLsljNYkUdlj44wcw==";

        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        byte[] plaintext;
        byte[] encryptedtext;

        public RSA_Encryption()
        {
            InitializeComponent();
        }

        #region --- HELPERS ---

        static public string EncodeToBase64(string toEncode)

        {

            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
        #endregion


        // function for Encryption.
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CboAESKey.SelectedIndex != -1 || CboRSAKey.SelectedIndex != -1)
                {

                    if (TxtSaveEncryptedAESKeyAs.Text != "")
                    {
                        //byte[] Rsakey = Convert.FromBase64String(CboRSAKey.SelectedValue.ToString());
                        byte[] base64String = ByteConverter.GetBytes(CboAESKey.SelectedValue.ToString());
                        byte[] encryptedAesKey = Encryption(base64String, RSA.ExportParameters(false), false);
                        using (StreamWriter sw = File.CreateText(@"C:\Github Repositories\WPFEncryptionTool2PROA04\AESImages\" + TxtSaveEncryptedAESKeyAs.Text + ".txt")) //dynamic path encrypted RSA
                        {
                            sw.WriteLine(encryptedAesKey.ToString());
                        }

                        MessageBox.Show("Your AES key has been encrypted succesfully.");
                    }
                    else
                    {
                        MessageBox.Show("Fill in file name.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select an public RSA key and AES key to encrypt.");
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
