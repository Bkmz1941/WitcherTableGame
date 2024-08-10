namespace Controllers.EventsScene.models
{
    [System.Serializable]
    public class MonsterWeakness
    {
        public int Uuid;
        public string Text;
        public string Effect;

        public MonsterWeakness(
            int Uuid,
            string Text,
            string Effect
        )
        {
            Uuid = Uuid;
            Text = Text;
            Effect = Effect;
        }
    }
}