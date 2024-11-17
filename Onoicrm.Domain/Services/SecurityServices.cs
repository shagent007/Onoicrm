using System.Security.Cryptography;
using System.Text;

namespace Onoicrm.Domain.Services;

public class SecurityServices
{
    private readonly string _securityKey;
    public SecurityServices(string securityKey)
    {
        _securityKey = securityKey;
    }

    public string EncryptPlainTextToCipherText(string plainText)
    {
        var toEncryptedArray = Encoding.UTF8.GetBytes(plainText);
        var objMd5CryptoService = new MD5CryptoServiceProvider();
        var securityKeyArray = objMd5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(_securityKey));
        objMd5CryptoService.Clear();
        var objTripleDesCryptoService = new TripleDESCryptoServiceProvider();
        objTripleDesCryptoService.Key = securityKeyArray;
        objTripleDesCryptoService.Mode = CipherMode.ECB;
        objTripleDesCryptoService.Padding = PaddingMode.PKCS7;
        var objCryptoTransform = objTripleDesCryptoService.CreateEncryptor();
        var resultArray = objCryptoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
        objTripleDesCryptoService.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public string DecryptCipherTextToPlainText(string cipherText)
    {
        var toEncryptArray = Convert.FromBase64String(cipherText);
        var objMd5CryptoService = new MD5CryptoServiceProvider();
        var securityKeyArray = objMd5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(_securityKey));
        objMd5CryptoService.Clear();
        var objTripleDesCryptoService = new TripleDESCryptoServiceProvider();
        objTripleDesCryptoService.Key = securityKeyArray;
        objTripleDesCryptoService.Mode = CipherMode.ECB;
        objTripleDesCryptoService.Padding = PaddingMode.PKCS7;
        var objCryptoTransform = objTripleDesCryptoService.CreateDecryptor();
        var resultArray = objCryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        objTripleDesCryptoService.Clear();
        return Encoding.UTF8.GetString(resultArray);
    }
}