namespace Resources.MonsterAttacks
{
    [System.Serializable]
    public class MonsterAttackSkill
    {
        public int Level;
        public string Title;
        public string Text;

        public MonsterAttackSkill(int Level, string Title, string Text)
        {
            Level = Level;
            Title = Title;
            Text = Text;
        }
    }
}