using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace DosingApp.Models.WebSocket
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class MixerControl
    {
        public bool? ShowSettings { get; set; }
        public virtual Common Common { get; set; }
        public virtual List<Collector> Collectors { get; set; }

        //public ObservableCollection<DispenserCollector> DispenserCollectors { get; set; }
        //public ObservableCollection<DispenserSingle> DispenserSingles { get; set; }
        //public ObservableCollection<DispenserThreeWay> DispenserThreeWays { get; set; }

    }
}
