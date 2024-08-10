using UnityEngine.Serialization;

namespace Models.Location
{
    [System.Serializable]
    public class Location
    {
        public string Uuid;
        public string Name;
        public string Image;
        public int Number;
        public string Type;

        public Location(
            string Uuid,
            string Name,
            string Image,
            int Number,
            string Type
        )
        {
            this.Uuid = Uuid;
            this.Name = Name;
            this.Image = Image;
            this.Number = Number;
            this.Type = Type;
        }
    }
}