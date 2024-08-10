using System.Collections.Generic;
using System.Linq;
using Models.MonsterAttack;
using UnityEngine;
using Utils.JsonParser;

namespace Services
{
    public class MonstersService
    {
        private static MonstersService instance;
        public List<MonsterAttack> monsterAttacks = new List<MonsterAttack>();
        public List<MonsterAttack> monsterAttacksPool = new List<MonsterAttack>();

        public static MonstersService of()
        {
            if (MonstersService.instance == null) MonstersService.instance = new MonstersService();
            return instance;
        }

        public void Init()
        {
            FetchMonsterAttackData();
        }

        public void GenerateMonsterAttackPool(int size)
        {
            int i = 0;
            while (i != size)
            {
                var random = Random.Range(0, monsterAttacks.Count - 1);
                var attack = monsterAttacks.ElementAt(random);
                monsterAttacks.Remove(attack);
                monsterAttacksPool.Add(attack);
                i++;
            }
        }

        public MonsterAttack GetMonsterAttackFromPool()
        {
            var attack = monsterAttacks.ElementAt(monsterAttacks.Count - 1);
            monsterAttacks.Remove(attack);
            return attack;
        }

        public void ReturnMonsterAttackToPool()
        {
        }

        private void FetchMonsterAttackData()
        {
            var forestLocationsJson = UnityEngine.Resources.Load<TextAsset>("MonsterAttacks/MonsterAttacks");
            var forestLocationsData = JsonUtility.FromJson<JsonWrapper<MonsterAttack>>(forestLocationsJson.text);
            foreach (var location in forestLocationsData.Items) monsterAttacks.Add(location);
        }
    }
}