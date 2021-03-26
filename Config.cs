using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ServerRestrictor
{

    public class Config : IRocketPluginConfiguration

    {
        [XmlArrayItem(ElementName = "word")]
        public List<WordBlacklist> RestrictedWords;

        [XmlArrayItem(ElementName = "player")]
        public List<NameBlacklist> RestrictedNames;

        [XmlArrayItem(ElementName = "Item")]
        public List<ItemBlacklist> RestrictedItems;

        [XmlArrayItem(ElementName = "Vehicle")]
        public List<VehicleBlacklist> RestrictedVehicles;

        public string MessageColor;
        public bool IgnoreAdmins;

        public void LoadDefaults()
        {
            RestrictedWords = new List<WordBlacklist>
            {
                new WordBlacklist { name = "ExampleWord" },
                new WordBlacklist { Ignore = "ignore.exampleword2", name = "ExampleWord2" }
            };

            RestrictedNames = new List<NameBlacklist>
            {
                new NameBlacklist { name = "ExampleName" },
                new NameBlacklist { Ignore = "ignore.examplenam2", name = "ExampleName2" }
            };  

            RestrictedItems = new List<ItemBlacklist>
            {
                new ItemBlacklist { ItemId = 000},
                new ItemBlacklist { Ignore = "ignore.111", ItemId = 111 },
            };

            RestrictedVehicles = new List<VehicleBlacklist>
            {
                new VehicleBlacklist { VehicleId = 222},
                new VehicleBlacklist { Ignore = "ignore.333", VehicleId = 333 }
            };

            IgnoreAdmins = true;
            MessageColor = "red";


        }
    }
}