using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApi
{
    public class BaseResult<T>
    {
        public T data { get; set; }
        public System.Net.HttpStatusCode statusCode { get; set; }
        public string message { get; set; }

        public BaseResult()
        {
            message = "Başarılı";
            data = (T)Activator.CreateInstance(typeof(T));
            statusCode = System.Net.HttpStatusCode.OK;
        }
    }
}
