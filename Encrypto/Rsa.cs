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
    public class Rsa
    {
        private string publicKeyFilePath;
        private string privateKeyFilePath;

        private string encryptedFilePath;
        private string decryptedFilePath;

        public Rsa(string publicKeyFilePath, string privateKeyFilePath, string encryptedFilePath, string decryptedFilePath)
        {
            this.publicKeyFilePath = publicKeyFilePath;
            this.privateKeyFilePath = privateKeyFilePath;

            this.encryptedFilePath = encryptedFilePath;
            this.decryptedFilePath = decryptedFilePath;
        }

        public void GenerateAndWriteKeys()
        {
            try
            {
                using (RSA rsaAlg = RSA.Create())
                {
                    string publicKey = Convert.ToBase64String(rsaAlg.ExportRSAPublicKey());
                    string privateKey = Convert.ToBase64String(rsaAlg.ExportRSAPrivateKey());

                    File.WriteAllText(publicKeyFilePath, publicKey);
                    File.WriteAllText(privateKeyFilePath, privateKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void EncryptFile(string filePath)
        {
            try
            {
                if (!File.Exists(publicKeyFilePath))
                {
                    MessageBox.Show("No public key file found! File cannot be encrypted without the key.", "Key Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string publicKeyString = File.ReadAllText(publicKeyFilePath);
                    using (RSA rsaAlg = RSA.Create())
                    {
                        rsaAlg.ImportRSAPublicKey(Convert.FromBase64String(publicKeyString), out _);

                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        byte[] encryptedBytes = rsaAlg.Encrypt(fileBytes, RSAEncryptionPadding.Pkcs1);

                        File.WriteAllBytes(encryptedFilePath, encryptedBytes);

                        MessageBox.Show($"File encrypted and saved to {encryptedFilePath} successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DecryptFile(string filePath)
        {
            try
            {
                if (!File.Exists(privateKeyFilePath))
                {
                    MessageBox.Show("No private key file found! File cannot be decrypted without the key.", "Key Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string privateKeyString = File.ReadAllText(privateKeyFilePath);
                    using (RSA rsaAlg = RSA.Create())
                    {
                        rsaAlg.ImportRSAPrivateKey(Convert.FromBase64String(privateKeyString), out _);

                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        byte[] decryptedBytes = rsaAlg.Decrypt(fileBytes, RSAEncryptionPadding.Pkcs1);

                        File.WriteAllBytes(decryptedFilePath, decryptedBytes);

                        MessageBox.Show($"File decrypted and saved to {decryptedFilePath} successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
