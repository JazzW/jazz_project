using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ZZ.Excel.Helper.Class;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZZ.Excel.Helper.Common
{
    public class IOHelper
    {

        public static CFileInfo[] getInfos(String dirPath)
        {
            List<CFileInfo> output = new List<CFileInfo>();
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            foreach (var _info in dirInfo.GetDirectories())
            {
                output.Add(new CFileInfo() { IsFile = false, Info = _info });
            }
            foreach (var _info in dirInfo.GetFiles())
            {
                output.Add(new CFileInfo() { IsFile =true, Info = _info });
            }

            return output.ToArray();
        }

        public static bool pathIsExit(string Path)
        {
            bool exist = false;
            if (File.Exists(Path))
            {
                exist = true;
            }
            if (Directory.Exists(Path))
            {
                exist = true;
            }
            return exist;
        }

        public static bool pathIsExit(string Path, ref CFileInfo Info)
        {
            bool exist = false;
            if (File.Exists(Path))
            {
                exist = true;
                Info = new CFileInfo() { IsFile = true, Info = new FileInfo(Path) };
            }
            if (Directory.Exists(Path))
            {
                exist = true;
                Info = new CFileInfo() { IsFile = false, Info = new DirectoryInfo(Path) };
            }
            return exist;
        }

        /// <summary>
        /// 使用UTF8编码将byte数组转成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ConvertToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data , 0 , data.Length);
        }

        /// <summary>
        /// 使用指定字符编码将byte数组转成字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ConvertToString(byte[] data , Encoding encoding)
        {
            return encoding.GetString(data , 0 , data.Length);
        }

        /// <summary>
        /// 使用UTF8编码将字符串转成byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ConvertToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 使用指定字符编码将字符串转成byte数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ConvertToByte(string str , Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 将对象序列化为二进制数据 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBinary(object obj)
        {
            MemoryStream stream = new MemoryStream( );
            BinaryFormatter bf = new BinaryFormatter( );
            bf.Serialize(stream , obj);

            byte[] data = stream.ToArray( );
            stream.Close( );

            return data;
        }

        /// <summary>
        /// 将对象序列化为XML数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToXml(object obj)
        {
            MemoryStream stream = new MemoryStream( );
            XmlSerializer xs = new XmlSerializer(obj.GetType( ));
            xs.Serialize(stream , obj);

            byte[] data = stream.ToArray( );
            stream.Close( );

            return data;
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object DeserializeWithBinary(byte[] data)
        {
            MemoryStream stream = new MemoryStream( );
            stream.Write(data , 0 , data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter( );
            object obj = bf.Deserialize(stream);

            stream.Close( );

            return obj;
        }

        /// <summary>
        /// 将二进制数据反序列化为指定类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeWithBinary<T>(byte[] data)
        {
            return (T)DeserializeWithBinary(data);
        }

        /// <summary>
        /// 将XML数据反序列化为指定类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeWithXml<T>(byte[] data)
        {
            MemoryStream stream = new MemoryStream( );
            stream.Write(data , 0 , data.Length);
            stream.Position = 0;
            XmlSerializer xs = new XmlSerializer(typeof(T));
            object obj = xs.Deserialize(stream);

            stream.Close( );

            return (T)obj;
        }

        public static void WriteToFile(byte[] data,string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);

            //将byte数组写入文件中
            fs.Write(data, 0, data.Length);
            //所有流类型都要关闭流，否则会出现内存泄露问题
            fs.Close();
        }

        public static byte[] LoadFormFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            //获取文件大小
            long size = fs.Length;

            byte[] array = new byte[size];

            //将文件读到byte数组中
            fs.Read(array, 0, array.Length);

            fs.Close();

            return array;
        }
    }
}
