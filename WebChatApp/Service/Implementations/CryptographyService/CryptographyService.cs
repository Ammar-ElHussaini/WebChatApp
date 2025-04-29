using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;
using WebChatApp.Models;
using WebChatApp.Service.Interfaces.NewFolder;

namespace WebChatApp.Service.Implementations.CryptographyService
{
    public class CryptographyService : ICryptographyService
    {
        private readonly IConfiguration _configuration;

        public CryptographyService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(string EncryptedMessage, string EncryptedKey, string Iv)> EncryptMessageHybridAsync(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText)) ; //here hander

            string publicKeyBase64 = _configuration["AppSettings:EncryptionKeys:RsaPublicKey"];
            if (string.IsNullOrWhiteSpace(publicKeyBase64)) ;
            //here hander

            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyBase64);
            using RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            return AesEncryptor.EncryptHybrid(plainText, rsa);
        }

        public async Task<string> DecryptMessageWithRsaAsync(string encryptedMessage)
        {
            if (string.IsNullOrWhiteSpace(encryptedMessage))
            {
                throw new ArgumentException("الرسالة المشفرة فارغة");
            }

            string privateKeyBase64 = _configuration["AppSettings:EncryptionKeys:RsaPrivateKey"];
            if (string.IsNullOrWhiteSpace(privateKeyBase64))
            {
                throw new ArgumentException("المفتاح الخاص غير موجود في الإعدادات");
            }

            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
            using RSA rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            byte[] encryptedBytes = Convert.FromBase64String(encryptedMessage);
            byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);

            return Encoding.UTF8.GetString(decryptedBytes);  // إعادة النص المفكوك
        }

    }



}
