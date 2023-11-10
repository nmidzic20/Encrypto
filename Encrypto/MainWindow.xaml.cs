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

        private string secretKeyFilePath = Path.Combine(desktopPath, "tajni_kljuc.txt");
        private string initializationVectorFilePath = Path.Combine(desktopPath, "inicijalizacijski_vektor.txt");

        private string publicKeyFilePath = Path.Combine(desktopPath, "javni_kljuc.txt");
        private string privateKeyFilePath = Path.Combine(desktopPath, "privatni_kljuc.txt");

        private string encryptedFilePathStart = Path.Combine(desktopPath, "encryptedFile");
        private string decryptedFilePathStart = Path.Combine(desktopPath, "decryptedFile");
        private const string aesFilePathEnd = "AES.txt";
        private const string rsaFilePathEnd = "RSA.txt";

        private AES aes;
        private Rsa rsa;

        public MainWindow()
        {
            InitializeComponent();
            aes = new AES(secretKeyFilePath, initializationVectorFilePath, encryptedFilePathStart + aesFilePathEnd, decryptedFilePathStart + aesFilePathEnd);
            rsa = new Rsa(publicKeyFilePath, privateKeyFilePath, encryptedFilePathStart + rsaFilePathEnd, decryptedFilePathStart + rsaFilePathEnd);
        }

        private void Generate_Keys_Click(object sender, RoutedEventArgs e)
        {
           aes.GenerateAndWriteKey();
            rsa.GenerateAndWriteKeys();

            MessageBox.Show("AES and RSA keys successfully generated");
        }

        private void Encrypt_Symmetric_Click(object sender, RoutedEventArgs e)
        {
            ProcessFileWithAlgorithm(aes.EncryptFile);
        }

        private void Decrypt_Symmetric_Click(object sender, RoutedEventArgs e)
        {
            ProcessFileWithAlgorithm(aes.DecryptFile);
        }

        private void Encrypt_Asymmetric_Click(object sender, RoutedEventArgs e)
        {
            ProcessFileWithAlgorithm(rsa.EncryptFile);
        }

        private void Decrypt_Asymmetric_Click(object sender, RoutedEventArgs e)
        {
            ProcessFileWithAlgorithm(rsa.DecryptFile);
        }

        private void ProcessFileWithAlgorithm(Action<string> processFunction)
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
