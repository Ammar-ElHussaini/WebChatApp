using System.Security.Cryptography;

namespace WebChatApp.Service.Implementations.CryptographyService
{
    public static class DecryptWithAes
    {
        public static string DecryptHybrid(string encryptedMessage, byte[] decryptedKey, byte[] iv)
        {
            return DecryptWithAos(encryptedMessage, decryptedKey, iv);
        }

        private static string DecryptWithAos(string encryptedMessage, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedMessage)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
