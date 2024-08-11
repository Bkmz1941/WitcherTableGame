namespace Resources.MonsterAttacks
{
    [System.Serializable]
    public class MonsterAttackSkill
    {
        public int Level;
        public string Title;
        public string Text;
        public bool PassFirstAttack;
        public int Time;

        public MonsterAttackSkill(int Level, string Title, string Text, bool PassFirstAttack, int Time)
        {
            Level = Level;
            Title = Title;
            Text = Text;
            PassFirstAttack = PassFirstAttack;
            Time = Time;
        }
    }
}