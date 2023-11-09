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

namespace Encrypto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Generate_Keys_Click(object sender, RoutedEventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string secretKeyPath = Path.Combine(desktopPath, "tajni_kljuc.txt");
            string publicKeyPath = Path.Combine(desktopPath, "javni_kljuc.txt");
            string privateKeyPath = Path.Combine(desktopPath, "privatni_kljuc.txt");

            GenerateAndWriteAesKey(secretKeyPath);
            GenerateAndWriteRSAKeys(publicKeyPath, privateKeyPath);


        }

        public void GenerateAndWriteAesKey(string filePath)
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

        private void Upload_Files_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Encrypt_Symmetric_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Decrypt_Symmetric_Click(object sender, RoutedEventArgs e)
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
