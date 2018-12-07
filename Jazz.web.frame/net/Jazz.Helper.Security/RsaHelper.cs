using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Jazz.Helper.Security
{
    public sealed class RsaHelper
    {
        static string _privateKey = "";

        static string _publicKey = "";

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
            return RSAEncrypt(plainText, _publicKey);
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
