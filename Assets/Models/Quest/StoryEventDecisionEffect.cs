using JetBrains.Annotations;
using Models.Location;
using UnityEngine.Serialization;

namespace Controllers.EventsScene.models
{
    [System.Serializable]
    public class StoryEventDecisionEffect
    {
        public string Type;
        public string Text;
        public string Value;
        public LocationMeta LocationMeta;

        public StoryEventDecisionEffect(
            string Type,
            string Text,
            string Value,
            LocationMeta LocationMeta
        )
        {
            Type = Type;
            Text = Text;
            Value = Value;
            LocationMeta = LocationMeta;
        }
    }
}