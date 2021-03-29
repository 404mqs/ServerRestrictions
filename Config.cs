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
                new WordBlacklist { Ignore = "ignore.exampleword2", name = "ExampleWord2" },
                new WordBlacklist { Ignore = "ignore.exampleword3", Message = "Example Message", name = "ExampleWord3"}
            };

            RestrictedNames = new List<NameBlacklist>
            {
                new NameBlacklist { name = "ExampleName" },
                new NameBlacklist { Ignore = "ignore.examplename2", name = "ExampleName2" },
                new NameBlacklist { Ignore = "ignore.examplename3", Message = "Example Message", name = "ExampleName3"}
            };  

            RestrictedItems = new List<ItemBlacklist>
            {
                new ItemBlacklist { ItemId = 000},
                new ItemBlacklist { Ignore = "ignore.111", ItemId = 111 },
                new ItemBlacklist { Ignore = "ignore.222", Message = "Example Message", ItemId = 222 }

            };

            RestrictedVehicles = new List<VehicleBlacklist>
            {
                new VehicleBlacklist { VehicleId = 333},
                new VehicleBlacklist { Ignore = "ignore.444", VehicleId = 444 },
                new VehicleBlacklist { Ignore = "ignore.555", Message = "Example Message", VehicleId = 555 }
            };

            IgnoreAdmins = true;
            MessageColor = "red";


        }
    }
}