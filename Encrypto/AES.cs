using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Encrypto
{
    public class AES
    {
        private string keyFilePath;
        private string ivFilePath;
        private string encryptedFilePath = "encryptedFileAES.txt";
        private string decryptedFilePath = "decryptedFileAES.txt";

        private static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public AES(string keyFilePath, string ivFilePath)
        {
            this.keyFilePath = keyFilePath;
            this.ivFilePath = ivFilePath;
        }

        public void EncryptFile(string filePath)
        {
            PerformAesFileOperation(filePath, aesAlg =>
            {
                aesAlg.GenerateIV();
                File.WriteAllBytes(ivFilePath, aesAlg.IV);
                return aesAlg.CreateEncryptor();
            });
        }

        public void DecryptFile(string filePath)
        {
            PerformAesFileOperation(filePath, aesAlg =>
            {
                byte[] iv = File.ReadAllBytes(ivFilePath);
                aesAlg.IV = iv;
                return aesAlg.CreateDecryptor();
            });
        }

        private void PerformAesFileOperation(string filePath, Func<Aes, ICryptoTransform> transformFunc)
        {
            try
            {
                bool decrypting = transformFunc.Method.Name.Contains("Decrypt");
                string resultFileName = transformFunc.Method.Name.Contains("Encrypt") ? encryptedFilePath : decryptedFilePath;

                if (!File.Exists(keyFilePath))
                {
                    MessageBox.Show("No symmetric key file found! File cannot be encrypted or decrypted without the key.", "Key Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (decrypting && !File.Exists(ivFilePath))
                {
                    MessageBox.Show("No IV file found! File cannot be decrypted without the initialization vector with which it was encrypted.", "IV Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string keyString = File.ReadAllText(keyFilePath);
                    byte[] key = Convert.FromBase64String(keyString);

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;

                        byte[] fileBytes = File.ReadAllBytes(filePath);

                        using (ICryptoTransform cryptoTransform = transformFunc(aesAlg))
                        {
                            byte[] resultBytes = cryptoTransform.TransformFinalBlock(fileBytes, 0, fileBytes.Length);

                            string resultFilePath = Path.Combine(desktopPath, resultFileName);
                            File.WriteAllBytes(resultFilePath, resultBytes);

                            MessageBox.Show($"File {(decrypting ? "decrypted" : "encrypted")} and saved to {resultFilePath} successfully.");
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.Message == "The input data is not a complete block.") message = "This file is not encrypted with AES algorithm.";

                MessageBox.Show($"An error occurred: {message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
