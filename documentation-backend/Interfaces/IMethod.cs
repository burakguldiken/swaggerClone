using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi.Interfaces
{
    public interface IMethod
    {
        /// <summary>
        /// Application xml file is parsed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        T ParseXml<T>(string xml) where T : class, new();

        /// <summary>
        /// Remove null portions
        /// </summary>
        /// <param name="DataStream"></param>
        /// <returns></returns>
        byte[] NullRemover(byte[] DataStream);
    }
}
