using System.Collections.Generic;
namespace TestApi.Entity
{
    public class EDocumentInfo
    {
        public string MethodName { get; set; }
        public string Controller { get; set; }
        public string ReturnType { get; set; }
        public List<string> ParametersName { get; set; }
        public List<string> ParametersType { get; set; }
        public string ActionDescription { get; set; }
        public string ActionName { get; set; }
        public string Summary { get; set; }
        public string actionColor { get; set; }
        public string methodBackgroundColor { get; set; }
    }

    public class DocumentKeyValue
    {
        public DocumentKeyValue()
        {
            value = new List<EDocumentInfo>();
        }

        public string key { get; set; }
        public List<EDocumentInfo> value { get; set; }
    }
}
