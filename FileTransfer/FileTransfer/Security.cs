using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer
{
    class Security
    {

        //AES

        /*
    public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] encryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        //byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                AES.Key = passwordBytes;
                AES.GenerateIV();

                AES.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }
        }

        return encryptedBytes;
    }
    
        
            public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] IVBytes)
            {
                byte[] decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                //byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        AES.Key = passwordBytes;
                        AES.IV = IVBytes;

                        AES.Mode = CipherMode.CBC;

                        FileStream fsInput = new FileStream(file, FileMode.Open, FileAccess.Read);

                        FileStream fsEncrypted = new FileStream(encrypted, FileMode.Create, FileAccess.Write);

                        ICryptoTransform encryptor = AES.CreateEncryptor();

                        CryptoStream cryptostream = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write);
                        long length = fsInput.Length / 10;
                        byte[] bytearrayinput = new byte[length];
                        int count = 1;
                        while (count != 0)
                        {
                            count = fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                        }


                        cryptostream.Close();
                        fsInput.Close();
                        fsEncrypted.Close();
                    }
                }

                return decryptedBytes;
            }
            */
        public static string EncryptFile(string username, string file, string encrypted,byte[] IV)
        {
            string key = SQL.getKey(username);
            string pwd = DecryptKey(key);

            byte[] passwordBytes = Convert.FromBase64String(pwd);

            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                AES.Key = passwordBytes;
                AES.IV = IV;

                AES.Mode = CipherMode.CBC;

                FileStream fsInput = new FileStream(file, FileMode.Open, FileAccess.Read);

                FileStream fsEncrypted = new FileStream(encrypted, FileMode.Create, FileAccess.Write);

                ICryptoTransform encryptor = AES.CreateEncryptor();

                CryptoStream cryptostream = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write);

                int bytesread;
                byte[] buffer = new byte[16384];
                while (true)
                {
                    bytesread = fsInput.Read(buffer, 0, 16384);
                    if (bytesread == 0)
                        break;
                    cryptostream.Write(buffer, 0, bytesread);
                }


                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
                return Convert.ToBase64String(AES.IV);

            }

        }

        public static string EncryptFile(string username, string file, string encrypted)
        {
            string key = SQL.getKey(username);
            string pwd = DecryptKey(key);

            byte[] passwordBytes = Convert.FromBase64String(pwd);

            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                AES.Key = passwordBytes;
                AES.GenerateIV();

                AES.Mode = CipherMode.CBC;

                FileStream fsInput = new FileStream(file, FileMode.Open, FileAccess.Read);

                FileStream fsEncrypted = new FileStream(encrypted, FileMode.Create, FileAccess.Write);

                ICryptoTransform encryptor = AES.CreateEncryptor();

                CryptoStream cryptostream = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write);
                
                int bytesread;
                byte[] buffer = new byte[16384];
                while (true)
                {
                    bytesread = fsInput.Read(buffer, 0, 16384);
                    if (bytesread == 0)
                        break;
                    cryptostream.Write(buffer, 0, bytesread);
                }
                
                
                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
                return Convert.ToBase64String(AES.IV);

            }

        }


        public static void DecryptFile(string username, string temppath, string dest, string IV)
        {
            byte[] IVBytes = Convert.FromBase64String(IV);
            string key = SQL.getKey(username);
            string pwd = DecryptKey(key);
            byte[] passwordBytes = Convert.FromBase64String(pwd);

            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.KeySize = 256;
                AES.BlockSize = 128;

                AES.Key = passwordBytes;
                AES.IV = IVBytes;

                AES.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = AES.CreateDecryptor();
                FileStream fsInput = new FileStream(temppath, FileMode.Open, FileAccess.Read);
                FileStream fsOutput = new FileStream(dest, FileMode.Create, FileAccess.Write);
                CryptoStream cryptostream = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read);
                /*
                int data;
                while ((data = cryptostream.ReadByte()) != -1)
                    fsOutput.WriteByte((byte)data);
                */

                
                
                int read;
                byte[] buffer = new byte[16384];
                while (true)
                {
                    read = cryptostream.Read(buffer, 0, 16384);
                    if (read == 0)
                        break;
                    fsOutput.Write(buffer, 0, read);
                }
                
                cryptostream.Close();
                fsInput.Close();
                fsOutput.Close();
            }

        }



        //RSA

        public static string EncryptKey(string key)
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = "RSAKeys";

            byte[] keybyte = Convert.FromBase64String(key);
            string encryptedkey;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(cp))
            {
                encryptedkey = Convert.ToBase64String(RSA.Encrypt(keybyte, false));
            }

            return encryptedkey;
        }


        public static string DecryptKey(string key)
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = "RSAKeys";

            byte[] keybyte = Convert.FromBase64String(key);
            string decryptedkey;

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(cp))
            {
                decryptedkey = Convert.ToBase64String(RSA.Decrypt(keybyte, false));
            }
            return decryptedkey;
        }

    }
}




/*
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel;
using System.IO;

namespace AsymmetricTest
{
class Program
{
static void Main(string[] args)
{
    // creates the CspParameters object and sets the key container name used to store the RSA key pair
    CspParameters cp = new CspParameters();
    cp.KeyContainerName = "RSAKeys";

    UnicodeEncoding ByteConverter = new UnicodeEncoding();

    //Create byte arrays to hold original, encrypted, and decrypted data.
    byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
    byte[] encryptedData;
    byte[] decryptedData;


    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(cp))
    {

        //Pass the data to ENCRYPT, the public key information 
        //(using RSACryptoServiceProvider.ExportParameters(false),
        //and a boolean flag specifying no OAEP padding.
        encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

        Console.WriteLine("Encrypted plaintext: {0}", ByteConverter.GetString(encryptedData));
        Console.WriteLine("\n\n");
        //Pass the data to DECRYPT, the private key information 
        //(using RSACryptoServiceProvider.ExportParameters(true),
        //and a boolean flag specifying no OAEP padding.
        decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);
        Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));


    }
}

static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
{
    try
    {
        byte[] encryptedData;
        //Create a new instance of RSACryptoServiceProvider.
        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        {

            //Import the RSA Key information. This only needs
            //toinclude the public key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Encrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }
        return encryptedData;
    }
    //Catch and display a CryptographicException  
    //to the console.
    catch (CryptographicException e)
    {
        Console.WriteLine(e.Message);

        return null;
    }

}

static public byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
{
    try
    {
        byte[] decryptedData;
        //Create a new instance of RSACryptoServiceProvider.
        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
        {
            //Import the RSA Key information. This needs
            //to include the private key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Decrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }
        return decryptedData;
    }
    //Catch and display a CryptographicException  
    //to the console.
    catch (CryptographicException e)
    {
        Console.WriteLine(e.ToString());

        return null;
    }

}
}
}

 */

