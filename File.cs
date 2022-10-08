using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Zwarp
{
    public static class File
    {
        private const string Key = "miknoN029nd92dq2ec30d9";
        public static void Init()
        {
            JObject jobj = JObject.Parse(System.IO.File.ReadAllText("00inilock").Decrypt());
            Data.Password = (jobj["Password"] ?? "").ToString();
            foreach (JProperty jprop in (jobj.GetValue("Services") ?? new JArray("")).Children())
            {
                Data.Services.Add(jprop.Name, jprop.Value.ToString());
            }
        }
        public static void OnExit(object? sender, EventArgs args)
        {
            StringBuilder builder = new StringBuilder($"{{\"Password\" : \"{Data.Password}\", \"Services\" : {{");
            for (int i = 0; i < Data.Services.Count; i++)
            {
                if (i == Data.Services.Count - 1)
                {
                    builder.Append($"\"{Data.Services.Keys.ElementAt(i)}\" : \"{Data.Services.Values.ElementAt(i)}\"");
                }
                else
                {
                    builder.Append($"\"{Data.Services.Keys.ElementAt(i)}\" : \"{Data.Services.Values.ElementAt(i)}\", ");
                }
            }
            System.IO.File.WriteAllText("00inilock", builder.Append("}}").ToString().Encrypt());
        }
        public static string Encrypt(this string text)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    text = Convert.ToBase64String(ms.ToArray());
                }
                return text;
            }
        }
        public static string Decrypt(this string text)
        {
            text = text.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return text;
        }
    }
}