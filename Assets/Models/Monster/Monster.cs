using System.Collections.Generic;
using Resources.MonsterAttacks;

namespace Controllers.EventsScene.models.Monster
{
    [System.Serializable]
    public class Monster
    {
        public int Uuid;
        public string Name;
        public string Image;
        public bool WasTrailed;
        public MonsterAttackSkill StartSkill;
        public MonsterAttackSkill SpecialSkill;
        public List<MonsterAttackSkill> AttackSkills;
        public int Lives;
        public int Level;
        public bool isAvailable;
        public MonsterWeakness[] Weaknesses;

        public Monster(
            int Uuid,
            string Name,
            string Image,
            int Lives,
            bool WasTrailed,
            int Level,
            MonsterAttackSkill StartSkill,
            MonsterAttackSkill SpecialSkill,
            List<MonsterAttackSkill> AttackSkills,
            bool isAvailable,
            MonsterWeakness[] Weaknesses
        )
        {
            Uuid = Uuid;
            Name = Name;
            Lives = Lives;
            StartSkill = StartSkill;
            SpecialSkill = SpecialSkill;
            AttackSkills = AttackSkills;
            WasTrailed = WasTrailed;
            Level = Level;
            StartSkill = StartSkill;
            Image = Image;
            isAvailable = isAvailable;
            Weaknesses = Weaknesses;
        }
    }
}