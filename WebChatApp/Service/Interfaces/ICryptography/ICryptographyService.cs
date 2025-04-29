namespace WebChatApp.Service.Interfaces.NewFolder
{
    public interface ICryptographyService
    {
        Task<(string EncryptedMessage, string EncryptedKey, string Iv)> EncryptMessageHybridAsync(string plainText);
        Task<string> DecryptMessageWithRsaAsync(string encryptedMessage);
    }

}
