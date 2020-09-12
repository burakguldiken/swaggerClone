using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestApi.Entity
{
    public class EApiXml
    {
		[XmlRoot(ElementName = "assembly")]
		public class Assembly
		{
			[XmlElement(ElementName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "param")]
		public class Param
		{
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "member")]
		public class Member
		{
			[XmlElement(ElementName = "param")]
			public List<Param> Param { get; set; }
			[XmlElement(ElementName = "returns")]
			public string Returns { get; set; }
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
			[XmlElement(ElementName = "typeparam")]
			public Typeparam Typeparam { get; set; }
			[XmlElement(ElementName = "summary")]
			public Summary Summary { get; set; }
		}

		[XmlRoot(ElementName = "typeparam")]
		public class Typeparam
		{
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
		}

		[XmlRoot(ElementName = "summary")]
		public class Summary
		{
			[XmlText]
			public string summary { get; set; }
		}

		[XmlRoot(ElementName = "members")]
		public class Members
		{
			[XmlElement(ElementName = "member")]
			public List<Member> Member { get; set; }
		}

		[XmlRoot(ElementName = "doc")]
		public class Doc
		{
			[XmlElement(ElementName = "assembly")]
			public Assembly Assembly { get; set; }
			[XmlElement(ElementName = "members")]
			public Members Members { get; set; }
		}
	}
}
