using System.Xml.Serialization;

namespace ServerRestrictor
{
    public class NameBlacklist
    {
        [XmlAttribute("name")]
        public string name;
        [XmlAttribute("IgnorePermission")]
        public string Ignore { get; set; }

        public NameBlacklist()
        {
            name = "";
        }

        public NameBlacklist(string name)
        {
            this.name = name;
        }
    }
}