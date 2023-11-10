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
        private static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public AES(string keyFilePath, string ivFilePath)
        {
            this.keyFilePath = keyFilePath;
            this.ivFilePath = ivFilePath;
        }

        public void EncryptFile(string filePath, string resultFileName)
        {
            PerformAesFileOperation(filePath, resultFileName, aesAlg =>
            {
                aesAlg.GenerateIV();
                File.WriteAllBytes(ivFilePath, aesAlg.IV);
                return aesAlg.CreateEncryptor();
            });
        }

        public void DecryptFile(string filePath, string resultFileName)
        {
            PerformAesFileOperation(filePath, resultFileName, aesAlg =>
            {
                byte[] iv = File.ReadAllBytes(ivFilePath);
                aesAlg.IV = iv;
                return aesAlg.CreateDecryptor();
            });
        }

        private void PerformAesFileOperation(string filePath, string resultFileName, Func<Aes, ICryptoTransform> transformFunc)
        {
            try
            {
                if (File.Exists(keyFilePath))
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

                            MessageBox.Show($"File {(transformFunc.Method.Name.Contains("Encrypt") ? "encrypted" : "decrypted")} and saved to {resultFilePath} successfully.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No symmetric key found! File cannot be decrypted without the key with which it was encrypted.", "Key Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
