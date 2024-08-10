using JetBrains.Annotations;
using UnityEngine.Serialization;

namespace Models.Location
{
    [System.Serializable]
    public class LocationMeta
    {
        public bool Generate;
        public string LocationType;
        public Location Location = null;

        public LocationMeta(
            bool Generate,
            string LocationType,
            Location Location = null
        )
        {
            Generate = Generate;
            LocationType = LocationType;
            Location = Location;
        }
    }
}