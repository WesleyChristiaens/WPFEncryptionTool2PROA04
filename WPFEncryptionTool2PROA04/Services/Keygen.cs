﻿using System;
using System.Security.Cryptography;

namespace WPFEncryptionTool2PROA04
{
    public class Keygen
    {
        public static void GenerateNewAESKey(string nameOfKey)
        {
            string key;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128;
                key = $"{Convert.ToBase64String(aes.Key)};{Convert.ToBase64String(aes.IV)}";
            }

            var fh = new FileHelper()
                .WithFolder(DefaultFolders.GeneratedAesKeys)
                .WithFileName(nameOfKey)
                .WithContent(key)
                .SaveToFile();
        }

        public static void GenerateNewRSaKeypair(string nameOfKey)
        {
            string publicKey;
            string privateKey;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }

            FileHelper publicKeyFile = new FileHelper()
                .WithFolder(DefaultFolders.RsaPublicKeys)
                .WithFileName(nameOfKey)
                .WithContent(publicKey)
                .IsXmlFile(true)
                .SaveToFile();

            FileHelper fh = new FileHelper()
                .WithFolder(DefaultFolders.RsaPrivateKeys)
                .WithFileName(nameOfKey)
                .WithContent(privateKey)
                .IsXmlFile(true)
                .SaveToFile();
        }
    }
}