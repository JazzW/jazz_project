using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace WebFrameWork.Helper
{
    public class SecurityHelper
    {
        public sealed class AesHelper
        {
            static Aes _key;

            public static Aes getKey()
            {
                return Aes.Create();
            }

            public static void SetKey(Aes key)
            {
                _key=key;
            }

            public static void SetKey(string key)
            {
                _key = Aes.Create();
                _key.Key = Encoding.UTF8.GetBytes(key);
                _key.IV = Encoding.UTF8.GetBytes(key);

            }

            public static string Encrypt(string plainText)
            {
                var bytes= EncryptStringToBytes_Aes(plainText, _key.Key, _key.IV);
                string publicStr = Convert.ToBase64String(bytes);//使用Base64将byte转换为string
                return publicStr;
            }
            
            public static string Decrypt(string cipherText)
            {
                byte[] privateValue = Convert.FromBase64String(cipherText);//使用Base64将string转换为byte
                return DecryptStringFromBytes_Aes(privateValue, _key.Key, _key.IV);
            }

            static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
            {
                // Check arguments.
                if (plainText == null || plainText.Length <= 0)
                    throw new ArgumentNullException("plainText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                byte[] encrypted;
                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
                return encrypted;

            }

            static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
            {
                if (cipherText == null || cipherText.Length <= 0)
                    throw new ArgumentNullException("cipherText");
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");
                string plaintext = null;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream 
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }

                return plaintext;

            }
        }

        public sealed class DesHelper
        {

            static DES _key;

            public static DES getKey()
            {
                return DES.Create();
            }

            public static void setKey(DES key)
            {
                _key = key;
            }

            public static string Encrypt(string plainText)
            {
                return DesEncrypt(plainText,_key.Key,_key.IV);
            }

            public static string Decrypt(string cipherText)
            {
                return DesDecrypt(cipherText, _key.Key, _key.IV);
            }

            /// <summary>
            /// DES加密算法
            /// sKey为8位或16位
            /// </summary>
            /// <param name="pToEncrypt">需要加密的字符串</param>
            /// <param name="sKey">密钥</param>
            /// <returns></returns>
            static string DesEncrypt(string pToEncrypt, byte[] Key, byte[] IV)
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = Key;
                des.IV = IV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                ret.ToString();
                return ret.ToString();
                //return a;
            }
            /// <summary>
            /// DES解密算法
            /// sKey为8位或16位
            /// </summary>
            /// <param name="pToDecrypt">需要解密的字符串</param>
            /// <param name="sKey">密钥</param>
            /// <returns></returns>
            static string DesDecrypt(string pToDecrypt, byte[] Key, byte[] IV)
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = Key;
                des.IV = IV;
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
        }

        public sealed class RsaHelper
        {
            static string _privateKey="";

            static string _publicKey="";

            public static void getKey(ref string privateKey, ref string publicKey)
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //生成RSA[公钥私钥]
                privateKey = rsa.ToXmlString(true);
                publicKey = rsa.ToXmlString(false);
            }

            public static void setKey(string privateKey, string publicKey)
            {
                _privateKey = privateKey;
                _publicKey = publicKey;
            }

            public static string Encrypt(string plainText)
            {
                return RSAEncrypt(plainText,_publicKey);
            }

            public static string Decrypt(string cipherText)
            {
                return RSADecrypt(cipherText, _privateKey);
            }

            /// <summary>
            /// 使用RSA实现加密
            /// </summary>
            /// <param name="data">加密数据</param>
            /// <returns></returns>
            static string RSAEncrypt(string data, string publicKey)
            {
                RSACryptoServiceProvider rsaPublic = new RSACryptoServiceProvider();
                rsaPublic.FromXmlString(publicKey);
                byte[] publicValue = rsaPublic.Encrypt(Encoding.UTF8.GetBytes(data), false);
                string publicStr = Convert.ToBase64String(publicValue);//使用Base64将byte转换为string
                return publicStr;
            }

            /// <summary>
            /// 使用RSA实现解密
            /// </summary>
            /// <param name="data">解密数据</param>
            /// <returns></returns>
            static string RSADecrypt(string data, string privateKey)
            {
                //C#默认只能使用[私钥]进行解密(想使用[私钥加密]可使用第三方组件BouncyCastle来实现)
                //创建RSA对象并载入[私钥]
                RSACryptoServiceProvider rsaPrivate = new RSACryptoServiceProvider();
                rsaPrivate.FromXmlString(privateKey);
                //对数据进行解密
                byte[] privateValue = rsaPrivate.Decrypt(Convert.FromBase64String(data), false);//使用Base64将string转换为byte
                string privateStr = Encoding.UTF8.GetString(privateValue);
                return privateStr;
            }
        }
    }
}
