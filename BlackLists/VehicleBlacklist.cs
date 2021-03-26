using System.Xml.Serialization;

namespace ServerRestrictor
{
    public class VehicleBlacklist       
    {
        [XmlText]
        public ushort VehicleId { get; set; }
        [XmlAttribute("IgnorePermission")]
        public string Ignore { get; set; }


        public VehicleBlacklist() { }

    }
}