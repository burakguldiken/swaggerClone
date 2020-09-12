using System.Collections.Generic;
using System.Linq;
using TestApi.Entity;

namespace TestApi.Interfaces
{
    public interface IDocument
    {
        /// <summary>
        /// Create document infos
        /// </summary>
        /// <returns></returns>
        List<IGrouping<string,EDocumentInfo>> Create_Document_Info();
    }
}
