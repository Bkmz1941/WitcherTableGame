using System.Collections.Generic;
using Models.Location;
using UnityEngine;
using Utils.JsonParser;

namespace Services
{
    public class QuestsService
    {
        public static QuestsService instance;
        public List<Location> forestLocations = new List<Location>();
        public List<Location> waterLocations = new List<Location>();
        public List<Location> mountainLocations = new List<Location>();

        public static QuestsService of()
        {
            if (QuestsService.instance == null) QuestsService.instance = new QuestsService();
            return instance;
        }

        public int GetLocationCount(string type)
        {
            if (type == "Mountain") return mountainLocations.Count;
            if (type == "Forest") return forestLocations.Count;
            if (type == "Water") return waterLocations.Count;
            return -1;
        }

        public Location GetLocation(string type)
        {
            if (type == "Mountain")
            {
                var random = Random.Range(0, mountainLocations.Count - 1);
                if (mountainLocations.Count > 0)
                {
                    var location = mountainLocations[random];
                    mountainLocations.Remove(location);
                    return location;
                }

                return null;
            }

            if (type == "Forest")
            {
                var random = Random.Range(0, forestLocations.Count - 1);
                var location = forestLocations[random];
                forestLocations.Remove(location);
                return location;
            }

            if (type == "Water")
            {
                var random = Random.Range(0, waterLocations.Count - 1);
                var location = waterLocations[random];
                waterLocations.Remove(location);
                return location;
            }

            return null;
        }

        public void ReturnLocation(Location location)
        {
            if (location.Type == "Mountain") mountainLocations.Add(location);
            if (location.Type == "Forest") forestLocations.Add(location);
            if (location.Type == "Water") waterLocations.Add(location);
        }

        public void FetchLocationsData()
        {
            var forestLocationsJson = UnityEngine.Resources.Load<TextAsset>("Locations/ForestLocations");
            var forestLocationsData = JsonUtility.FromJson<JsonWrapper<Location>>(forestLocationsJson.text);
            foreach (var location in forestLocationsData.Items) forestLocations.Add(location);

            var waterLocationsJson = UnityEngine.Resources.Load<TextAsset>("Locations/WaterLocations");
            var waterLocationsData = JsonUtility.FromJson<JsonWrapper<Location>>(waterLocationsJson.text);
            foreach (var location in waterLocationsData.Items) waterLocations.Add(location);

            var mountainLocationsJson = UnityEngine.Resources.Load<TextAsset>("Locations/MountainLocations");
            var mountainLocationsData = JsonUtility.FromJson<JsonWrapper<Location>>(mountainLocationsJson.text);
            foreach (var location in mountainLocationsData.Items) mountainLocations.Add(location);
        }
    }
}