using System.Collections.Generic;
using System.Xml.Serialization;

namespace ModelGenerator.Model
{
    public class Class
    {
        [XmlAttribute("clonableModel")]
        public bool ClonableModel { get; set; }

        [XmlAttribute("create")]
        public bool GenerateCreateModel { get; set; }

        [XmlAttribute("model")]
        public bool GenerateDatabaseModel { get; set; } = true;

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
    public class Classes
    {
        [XmlElement("enum")]
        public List<Enum> Enumerations { get; set; }

        [XmlElement("model")]
        public List<Class> Items { get; set; }

        [XmlAttribute("modelNamespace")]
        public string ModelNamespace { get; set; }

        [XmlAttribute("modelsFolder")]
        public string ModelsFolder { get; set; }

        [XmlAttribute("rootNamespace")]
        public string RootNamespace { get; set; }

        [XmlAttribute("typescriptFolder")]
        public string TypescriptFolder { get; set; }

        [XmlAttribute("viewModelNamespace")]
        public string ViewModelNamespace { get; set; }
    }

    public class Enum
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public class Property
    {
        [XmlAttribute("component")]
        public string Component { get; set; }

        [XmlAttribute("display-name")]
        public string DisplayName { get; set; }

        [XmlAttribute("list")]
        public bool GenerateAsList { get; set; }

        [XmlAttribute("create")]
        public bool IncludeInCreate { get; set; }

        [XmlAttribute("model")]
        public bool IncludeInDatabaseModel { get; set; } = true;

        [XmlAttribute("detail")]
        public bool IncludeInDetail { get; set; }

        [XmlAttribute("summary")]
        public bool IncludeInSummary { get; set; }

        [XmlAttribute("update")]
        public bool IncludeInUpdate { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("navigation-prop-id")]
        public string NavigationPropertyId { get; set; }

        [XmlAttribute("required")]
        public bool PropertyRequired { get; set; }

        [XmlAttribute("regex")]
        public string RegularExpression { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("email")]
        public bool ValidateAsEmail { get; set; }
    }
}