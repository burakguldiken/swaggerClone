using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TestApi.Entity;
using TestApi.Interfaces;
using static TestApi.Entity.EApiXml;

namespace TestApi.Services
{
    public class SDocument : IDocument
    {
        IMethod _SMethod;
        public SDocument(IMethod _SMethod)
        {
            this._SMethod = _SMethod;
        }

        public List<IGrouping<string,EDocumentInfo>> Create_Document_Info()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            List<EDocumentInfo> documentInfo = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new EDocumentInfo()
                {
                    MethodName = x.Name.ToString(),
                    Controller = x.DeclaringType.Name.ToString(),
                    ReturnType = x.ReturnType.Name.ToString(),
                    ActionName = x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")).FirstOrDefault().ToString(),
                    ActionDescription = x.CustomAttributes.FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString(),
                    Summary = "",
                    ParametersType = x.GetParameters().Length > 0 ? x.GetParameters()?.ToList().Select(x => x.ParameterType.Name).ToList() : x.GetParameters()?.Select(x => x.ParameterType)?.ToList()?.Select(x => x.GetProperties())?.ToList()?.FirstOrDefault()?.Select(x => x.PropertyType.Name).ToList(),
                    ParametersName = x.GetParameters().Length > 0 ? x.GetParameters()?.ToList().Select(x => x.Name).ToList() : x.GetParameters()?.Select(x => x.ParameterType)?.ToList()?.Select(x => x.GetProperties())?.ToList()?.FirstOrDefault()?.Select(x => x.Name).ToList()
                }).ToList();

            string[] fileArr = Directory.GetFiles(Directory.GetCurrentDirectory()).ToArray();

            string currentPath = fileArr.Where(x => x.Contains(".xml")).FirstOrDefault();

            using (FileStream fs = System.IO.File.OpenRead(currentPath))
            {
                long fileSize = fs.Length;
                string result = "";
                while (fileSize > 0)
                {
                    fileSize -= 5000000;
                    byte[] bytes = new byte[5000000];
                    fs.Read(bytes, 0, bytes.Length);
                    result = System.Text.Encoding.UTF8.GetString(bytes);

                    byte[] arr = Encoding.UTF8.GetBytes(result);
                    arr = _SMethod.NullRemover(arr);
                    result = Encoding.UTF8.GetString(arr);

                    result = result.Replace(@"<?xml version=""1.0""?>", "");
                    var response = _SMethod.ParseXml<Doc>(result);

                    List<EMethodSummary> methodSummaryPair = response.Members?.Member.Select(x => new EMethodSummary()
                    {
                        Name = x.Name,
                        Summary = x.Summary.summary != null ? x.Summary.summary.Replace("\n", "").TrimStart() : ""
                    }).ToList();

                    foreach (var item in documentInfo)
                    {
                        EMethodSummary currentMethodSummaryPair = methodSummaryPair.Where(x => x.Name.Contains(item.MethodName)).FirstOrDefault();
                        item.Summary = currentMethodSummaryPair.Summary;
                    }
                }
            }
            var documentInfoGroupByControllerName = documentInfo.GroupBy(x => x.Controller).ToList();
            
            return documentInfoGroupByControllerName;
        }
    }
}
