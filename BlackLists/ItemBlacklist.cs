using System.Xml.Serialization;

namespace ServerRestrictor
{
    public class ItemBlacklist
    {
        [XmlText]
        public ushort ItemId { get; set; }
        [XmlAttribute("IgnorePermission")]
        public string Ignore { get; set; }



        public ItemBlacklist() { }
 
    }
}