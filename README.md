WPF C# desktop application created for Advanced Operating Systems course.

# Features

- Generate symmetric and asymmetric keys (AES, RSA)
- File encryption using symmetric key (AES)
- File encryption using asymmetric keys (RSA)
- File decryption
- Hash calculation for files (SHA256)
- Digitally sign files
- Check digital signature of files

Each option will create `.txt` file with the corresponding data, saved to Desktop. 

Before encryption/decryption, it is necessary to first choose the option Generate keys, which creates three `.txt` files with the keys used for encryption/decryption:
- `tajni_kljuc.txt` (contains secret symmetric key created with AES)
- `javni_kljuc.txt` (contains public asymmetric key created with RSA)
- `privatni_kljuc.txt` (contains private asymmetric key created with RSA)

When checking digital signature, first choose the original file which was previously digitally signed, then choose `digitalSignature.txt` file previously created and saved to Desktop. If the original file has not been changed, the signature will be valid.

# Instructions

Run the project from Visual Studio 2022. 
