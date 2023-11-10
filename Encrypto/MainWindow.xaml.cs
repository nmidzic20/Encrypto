using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Win32;

namespace Encrypto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private AES aes;

        public MainWindow()
        {
            InitializeComponent();
            aes = new AES(Path.Combine(desktopPath, "tajni_kljuc.txt"), Path.Combine(desktopPath, "inicijalizacijski_vektor.txt"));
        }

        private void Generate_Keys_Click(object sender, RoutedEventArgs e)
        {
            string secretKeyPath = Path.Combine(desktopPath, "tajni_kljuc.txt");
            string publicKeyPath = Path.Combine(desktopPath, "javni_kljuc.txt");
            string privateKeyPath = Path.Combine(desktopPath, "privatni_kljuc.txt");

            GenerateAndWriteAesKey(secretKeyPath);
            GenerateAndWriteRSAKeys(publicKeyPath, privateKeyPath);

            MessageBox.Show("AES and RSA keys successfully generated");
        }

        public static void GenerateAndWriteAesKey(string filePath)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.GenerateKey();

                    string keyString = Convert.ToBase64String(aesAlg.Key);

                    File.WriteAllText(filePath, keyString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static void GenerateAndWriteRSAKeys(string publicKeyPath, string privateKeyPath)
        {
            try
            {
                using (RSA rsaAlg = RSA.Create())
                {
                    string publicKey = Convert.ToBase64String(rsaAlg.ExportRSAPublicKey());
                    string privateKey = Convert.ToBase64String(rsaAlg.ExportRSAPrivateKey());

                    File.WriteAllText(publicKeyPath, publicKey);
                    File.WriteAllText(privateKeyPath, privateKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void Encrypt_Symmetric_Click(object sender, RoutedEventArgs e)
        {
            ProcessFileWithAes(aes.EncryptFile);
        }

        private void Decrypt_Symmetric_Click(object sender, RoutedEventArgs e)
        {
            ProcessFileWithAes(aes.DecryptFile);
        }

        private void ProcessFileWithAes(Action<string> processFunction)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = $"Select a file to {processFunction.Method.Name.Substring(0, 7)}";
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    processFunction(filePath);
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Upload_Files_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Encrypt_Asymmetric_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Decrypt_Asymmetric_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Calculate_Hash_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Make_Digital_Signature_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Check_Digital_Signature_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
