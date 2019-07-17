using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Crypting : MonoBehaviour
{
    /*
    public static byte[] Encrypt(string value, RSAParameters rsaKeyInfo)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            // Récupère les infos de la clé publique
            rsa.ImportParameters(rsaKeyInfo);

            byte[] encodedData = Encoding.Default.GetBytes(value);

            // Chiffre les données.
            // Les données chiffrées sont retournées sous la forme d'un tableau de bytes
            byte[] encryptedData = rsa.Encrypt(encodedData, true);

            rsa.Clear();

            return encryptedData;
        }
    }
    

    public static string Decrypt(byte[] encryptedData, RSAParameters rsaKeyInfo)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            // Récupère les infos de la clé privée
            rsa.ImportParameters(rsaKeyInfo);

            // Déchiffre les données.
            // Les données déchiffrées sont retournées sous la forme d'un tableau de bytes
            byte[] decryptedData = rsa.Decrypt(encryptedData, true);

            string decryptedValue = Encoding.Default.GetString(decryptedData);

            rsa.Clear();

            return decryptedValue;
        }
    }
    */
    private const string mysecurityKey = "M@mltc21mr17st";
 
      public static string Encrypt(string TextToEncrypt)
      {
         byte[] MyEncryptedArray = UTF8Encoding.UTF8
            .GetBytes(TextToEncrypt);
 
         MD5CryptoServiceProvider MyMD5CryptoService = new
            MD5CryptoServiceProvider();
 
         byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash
            (UTF8Encoding.UTF8.GetBytes(mysecurityKey));
 
         MyMD5CryptoService.Clear();
 
         var MyTripleDESCryptoService = new
            TripleDESCryptoServiceProvider();
 
         MyTripleDESCryptoService.Key = MysecurityKeyArray;
 
         MyTripleDESCryptoService.Mode = CipherMode.ECB;
 
         MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
 
         var MyCrytpoTransform = MyTripleDESCryptoService
            .CreateEncryptor();
 
         byte[] MyresultArray = MyCrytpoTransform
            .TransformFinalBlock(MyEncryptedArray, 0,
            MyEncryptedArray.Length);
 
         MyTripleDESCryptoService.Clear();
 
         return Convert.ToBase64String(MyresultArray, 0,
            MyresultArray.Length);
      }
 

      public static string Decrypt(string TextToDecrypt)
      {
         byte[] MyDecryptArray = Convert.FromBase64String
            (TextToDecrypt);
 
         MD5CryptoServiceProvider MyMD5CryptoService = new
            MD5CryptoServiceProvider();
 
         byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(mysecurityKey));
 
         MyMD5CryptoService.Clear();
 
         var MyTripleDESCryptoService = new
            TripleDESCryptoServiceProvider();
 
         MyTripleDESCryptoService.Key = MysecurityKeyArray;
 
         MyTripleDESCryptoService.Mode = CipherMode.ECB;
 
         MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
 
         var MyCrytpoTransform = MyTripleDESCryptoService
            .CreateDecryptor();
 
         byte[] MyresultArray = MyCrytpoTransform
            .TransformFinalBlock(MyDecryptArray, 0,
            MyDecryptArray.Length);
 
         MyTripleDESCryptoService.Clear();
 
         return UTF8Encoding.UTF8.GetString(MyresultArray);
      }
}
