using System.Security.Cryptography;
using System.Text;

namespace SaysanPwa.Application.Utilities.Cryptography;

public class Encrypt_CLS
{
    private Rijndael RJ = Rijndael.Create();

    private MD5CryptoServiceProvider MD = new MD5CryptoServiceProvider();

    private byte[] MD5Hash(string Value)
    {
        return MD.ComputeHash(Encoding.ASCII.GetBytes(Value));
    }

    public string Encrypt_Text(string Text_TO_Encrypt, string Key)
    {
        RJ.Key = MD5Hash(Key);
        RJ.Mode = CipherMode.ECB;
        byte[] bytes = Encoding.ASCII.GetBytes(Text_TO_Encrypt);
        return Convert.ToBase64String(RJ.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
    }

    public string Decrypt_Text(string Encrypted_Text, string Key)
    {
        //IL_005a: Unknown result type (might be due to invalid IL or missing references)
        try
        {
            RJ.Key = MD5Hash(Key);
            RJ.Mode = CipherMode.ECB;
            byte[] array = Convert.FromBase64String(Encrypted_Text);
            return Encoding.ASCII.GetString(RJ.CreateDecryptor().TransformFinalBlock(array, 0, array.Length));
        }
        catch (Exception)
        {
            return null!;
        }
    }
}
