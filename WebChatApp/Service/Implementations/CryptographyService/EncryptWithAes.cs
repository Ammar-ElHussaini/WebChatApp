using System;
using System.IO;
using System.Security.Cryptography;

namespace WebChatApp.Service.Implementations.CryptographyService
{

    public class AesEncryptor
    {
        public static (string EncryptedMessage, string EncryptedKey, string Iv) EncryptHybrid(string plainText, RSA rsaPublicKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();

                byte[] encryptedMessage = AesEncryption(plainText, aes.Key, aes.IV);

                byte[] encryptedKey = rsaPublicKey.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);

                return (Convert.ToBase64String(encryptedMessage), Convert.ToBase64String(encryptedKey), Convert.ToBase64String(aes.IV));
            }
        }

        static private byte[] AesEncryption(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
    }
}
