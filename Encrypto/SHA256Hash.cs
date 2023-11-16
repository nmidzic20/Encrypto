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
    public class SHA256Hash
    {
        public string CalculateFileHash(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void CalculateDigitalSignature(string filePath, string privateKeyFilePath)
        {
            try
            {
                string data = File.ReadAllText(filePath);

                string privateKeyString = File.ReadAllText(privateKeyFilePath);

                using (RSA rsaAlg = RSA.Create())
                {
                    rsaAlg.ImportRSAPrivateKey(Convert.FromBase64String(privateKeyString), out _);

                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] signatureBytes = rsaAlg.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                    string digitalSignatureFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "digitalSignature.txt");
                    File.WriteAllBytes(digitalSignatureFilePath, signatureBytes);

                    MessageBox.Show($"Digital signature calculated and saved to {digitalSignatureFilePath} successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CheckDigitalSignature(string filePath, string digitalSignatureFilePath, string publicKeyFilePath)
        {
            try
            {
                byte[] signatureBytes = File.ReadAllBytes(digitalSignatureFilePath);
                string data = File.ReadAllText(filePath);
                string publicKeyString = File.ReadAllText(publicKeyFilePath);

                using (RSA rsaAlg = RSA.Create())
                {
                    rsaAlg.ImportRSAPublicKey(Convert.FromBase64String(publicKeyString), out _);

                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    bool isSignatureValid = rsaAlg.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                    if (isSignatureValid)
                    {
                        MessageBox.Show("Digital signature is valid. File integrity is intact.");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Digital signature is invalid. File integrity may be compromised.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
