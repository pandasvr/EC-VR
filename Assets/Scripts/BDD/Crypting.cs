using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Crypting : MonoBehaviour
{
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

    static string Decrypt(byte[] encryptedData, RSAParameters rsaKeyInfo)
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
    
}
