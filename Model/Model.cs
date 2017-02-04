using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModelGenerator.Model
{
    public class Model
    {
        [XmlAttribute("create")]
        public bool GenerateCreateModel { get; set; }

        [XmlAttribute("detail")]
        public bool GenerateDetailModel { get; set; }

        [XmlAttribute("summary")]
        public bool GenerateSummaryModel { get; set; }

        [XmlAttribute("update")]
        public bool GenerateUpdateModel { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("property")]
        public List<Property> Properties { get; set; }
    }

    [XmlRoot("models")]
    public class Models
    {
        [XmlElement("model")]
        public List<Model> Items { get; set; }
    }

    public class Property
    {
        [XmlAttribute("display-name")]
        public bool DisplayName { get; set; }

        [XmlAttribute("list")]
        public bool GenerateAsList { get; set; }

        [XmlAttribute("create")]
        public bool IncludeInCreate { get; set; }

        [XmlAttribute("detail")]
        public bool IncludeInDetail { get; set; }

        [XmlAttribute("summary")]
        public bool IncludeInSummary { get; set; }

        [XmlAttribute("update")]
        public bool IncludeInUpdate { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("required")]
        public bool PropertyRequried { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("email")]
        public bool ValidateAsEmail { get; set; }
    }
}