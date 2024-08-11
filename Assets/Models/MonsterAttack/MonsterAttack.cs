using System.Collections.Generic;
using Resources.MonsterAttacks;

namespace Models.MonsterAttack
{
    [System.Serializable]
    public class MonsterAttack
    {
        public int Uuid;
        public string Type;
        public int Damage;
        public bool Special;
        public int SpecialLevel;
        public string Text;
        public string Title;

        public MonsterAttack(
            int Uuid,
            string Type,
            int Damage,
            bool Special,
            int SpecialLevel,
            string Text,
            string Title
        )
        {
            Uuid = Uuid;
            Type = Type;
            Damage = Damage;
            Special = Special;
            SpecialLevel = SpecialLevel;
            Text = Text;
            Title = Title;
        }
    }
}