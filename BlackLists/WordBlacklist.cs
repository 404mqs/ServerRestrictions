using System.Xml.Serialization;

namespace ServerRestrictor
{
    public class WordBlacklist
    {
        [XmlAttribute("name")]
        public string name;
        [XmlAttribute("IgnorePermission")]
        public string Ignore { get; set; }

        public WordBlacklist()
        {
            name = "";
        }

        public WordBlacklist(string name)
        {
            this.name = name;
        }
    }
}