using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace LY.Helper
{
    public class GlobalFunction
    {
        /// <summary>
        /// 深度复制
        /// 利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制,此种方法最可靠
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T CloneByRuntime<T>(T RealObject)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        /// <summary>
        /// 深度复制
        /// 利用System.Xml.Serialization来实现序列化与反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public static T CloneByXml<T>(T RealObject)
        {
            using (Stream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, RealObject);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 获取Guid值
        /// </summary>
        /// <returns></returns>
        public static string GetNewGuid()
        {
            return System.Guid.NewGuid().ToString();
        }


        public static object GetAttribute(System.Type type, System.Type attribute)
        {
            object[] objs = type.GetCustomAttributes(attribute, true);
            if (objs.Length > 0)
                return objs[0];
            return null;
        }

        public static T CreateInstance<T>()
        {
            object obj = GetAttribute(typeof(T), typeof(DLLInfoAttribute));
            if (obj == null)
            {
                throw new ArgumentNullException(string.Format("{0} 接口没有定义 DllInfoAttribute 属性信息，不能反射调用。", typeof(T).Name));
            }

            DLLInfoAttribute dllInfo = obj as DLLInfoAttribute;

            obj = CreateInstance(dllInfo.DllName, dllInfo.FullClassName);
            if (obj != null)
            {
                return (T)obj;
            }
            return default(T);
        }

        public static object CreateInstance(string dllName, string fullClassName, bool isStaticClass = false)
        {
            //使用绝对路径(XP,WIN7等系统使用openfiledialog后导致程序目录更改)
            if (dllName.IndexOf(":") < 0)
            {
                dllName =AppDomain.CurrentDomain.BaseDirectory+"\\" + dllName;
            }
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(dllName);
            if (assembly == null)
            {
                return null;
            }

            if (isStaticClass)
            {
                System.Type type = assembly.GetType(fullClassName);

                return type;
            }
            else
            {
                return assembly.CreateInstance(fullClassName);
            }
        }

    }
}
