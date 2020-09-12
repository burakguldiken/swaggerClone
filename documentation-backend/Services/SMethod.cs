using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using TestApi.Interfaces;

namespace TestApi.Services
{
    public class SMethod : IMethod
    {
        public T ParseXml<T>(string xml) where T : class, new()
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            try
            {
                XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
                xmlRootAttribute.ElementName = "doc";
                xmlRootAttribute.IsNullable = true;
                XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRootAttribute);
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    result = (T)serializer.Deserialize(memoryStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("XML Parse Error:" + ex.Message);
                Console.WriteLine("\n" + xml);
            }
            return result;
        }

        public byte[] NullRemover(byte[] DataStream)
        {
            int i;
            byte[] temp = new byte[DataStream.Length];
            for (i = 0; i < DataStream.Length - 1; i++)
            {
                if (DataStream[i] == 0x00) break;
                temp[i] = DataStream[i];
            }
            byte[] NullLessDataStream = new byte[i];
            for (i = 0; i < NullLessDataStream.Length; i++)
            {
                NullLessDataStream[i] = temp[i];
            }
            return NullLessDataStream;
        }
    }
}
