using System;
using System.IO;
using NETCore.Encrypt;

namespace RSAKeyGenerator
{
    class Program
    {
        private static bool _shouldContinue = true;
        private static bool _areKeysAvailable = false;
        private static string _privateKey = "--- No Keys Available ---";
        private static string _publicKey = "--- No Keys Available ---";

        static void Main(string[] args)
        {
            ExportKeys();
            Listener();
        }

        private static void Listener()
        {
            ShowHelp();
            Console.WriteLine("Listening..");
            var key = Console.ReadLine();
            Console.Clear();
            switch (key.ToUpper())
            {
                case "X": _shouldContinue = false; break;
                case "KEYGEN": GenerateRSAKeys(); break;
                case "SPBK": ShowPublicKey(); break;
                case "SPRK": ShowPrivateKey(); break;
                case "EXPORT": ExportKeys(); break;
                default: break;
            }

            if (!_shouldContinue)
            {
                return;
            }
            Listener();
        }

        private static void GenerateRSAKeys()
        {
            var rsa = EncryptProvider.CreateRsaKey();
            _publicKey = rsa.PublicKey;
            _privateKey = rsa.PrivateKey;
            Console.WriteLine("--- New RSA Keys Generated ----");
            Console.WriteLine("--- ---- 2048 BitSize ----- ---");
            ShowPublicKey();
            ShowPrivateKey();
            _areKeysAvailable = true;
        }

        private static void ShowPublicKey()
        {
            Console.WriteLine();
            Console.WriteLine("Public RSA Key:- ");
            Console.WriteLine(_publicKey);
            Console.WriteLine();
        }

        private static void ShowPrivateKey()
        {
            Console.WriteLine();
            Console.WriteLine("Private RSA Key:-");
            Console.WriteLine(_privateKey);
            Console.WriteLine();
        }

        private static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("=== Help With Commands ===");
            Console.WriteLine(" keygen/KEYGEN ---> Generate New RSA Keys with 2048 Bit Size");
            Console.WriteLine(" export/EXPORT ---> Export Keys to 'keys' folder. This will overwrite previous files.");
            Console.WriteLine(" spbk/SPBK -------> Show Public Key (that is recently generated)  ");
            Console.WriteLine(" sprk/SPRK -------> Show Private Key (that is recently generated)  ");
            Console.WriteLine(" X/x: ------------> Exit");
            Console.WriteLine();
        }

        private static void ExportKeys()
        {
            if (!_areKeysAvailable)
            {
                Console.WriteLine(" -- No Keys Available --");
                return;
            }
            Console.WriteLine();

            //Check Directories
            var basePath = $"{System.Environment.CurrentDirectory}\\keys";
            var privateKeyPath = $"{basePath}\\RSAPrivateKey.txt";
            var publicKeyPath = $"{basePath}\\RSAPublicKey.txt";

            Console.WriteLine(basePath);
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            using (var file = new StreamWriter(privateKeyPath))
            {
                file.WriteLine(_privateKey);
            }
            using (var file = new StreamWriter(publicKeyPath))
            {
                file.WriteLine(_publicKey);
            }

            Console.WriteLine($" Private key is available at {privateKeyPath}");
            Console.WriteLine($" Public key is available at {publicKeyPath}");
        }
    }
}
