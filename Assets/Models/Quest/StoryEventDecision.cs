using Models.Location;
using UnityEngine.Serialization;

namespace Controllers.EventsScene.models
{
    [System.Serializable]
    public class StoryEventDecision
    {
        public string Text;
        public bool Questable = false;
        public StoryEventDecisionEffect[] Effects;

        public StoryEventDecision(
            string Text,
            bool Questable,
            StoryEventDecisionEffect[] Effects
        )
        {
            Text = Text;
            Effects = Effects;
            Questable = Questable;
        }
    }
}