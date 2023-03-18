using System;
using System.Security.Cryptography;
using WPFEncryptionTool2PROA04.Models;

namespace WPFEncryptionTool2PROA04
{
    public class Keygen
    {
        public static AesKey GenerateNewAESKey(string nameOfKey)
        {
            AesKey myAesKey = new AesKey();

            using (Aes keygen = Aes.Create())
            {
                myAesKey.Name = nameOfKey;
                myAesKey.Key = Convert.ToBase64String(keygen.Key);
                myAesKey.InitiationVector = Convert.ToBase64String(keygen.IV);
            }

            return myAesKey;
        }

        public static RsaKeyPair GenerateNewRSaKeypair(string nameOfKey)
        {
            var rkp = new RsaKeyPair();

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rkp.Name = nameOfKey;
                rkp.PublicKey = rsa.ToXmlString(false);
                rkp.PrivateKey = rsa.ToXmlString(true);
            }

            return rkp;

        }
    }
}
