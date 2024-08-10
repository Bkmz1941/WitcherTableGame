namespace Controllers.EventsScene.models
{
    [System.Serializable]
    public class StoryEvent
    {
        public int Uuid;
        public string Story;
        public string VariantA;
        public string VariantB;
        public bool VariantASelected = false;
        public bool VariantBSelected = false;
        public StoryEventDecision DecisionA;
        public StoryEventDecision DecisionB;

        public StoryEvent(
            int uuid,
            string story,
            string variantA,
            string variantB,
            StoryEventDecision decisionA,
            StoryEventDecision decisionB
        )
        {
            Uuid = uuid;
            Story = story;
            VariantA = variantA;
            VariantB = variantB;
            DecisionA = decisionA;
            DecisionB = decisionB;
        }
    }
}