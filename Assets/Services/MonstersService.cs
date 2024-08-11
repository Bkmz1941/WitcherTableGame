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
            monsterAttacksPool.Clear();
            var copyArray = new List<MonsterAttack>();
            foreach (var el in monsterAttacks) copyArray.Add(el);

            int i = 0;
            while (i != size)
            {
                var random = Random.Range(0, copyArray.Count - 1);
                var attack = copyArray.ElementAt(random);
                copyArray.Remove(attack);
                monsterAttacksPool.Add(attack);
                i++;
            }
        }

        public MonsterAttack GetMonsterAttackFromPool()
        {
            return monsterAttacksPool.ElementAt(monsterAttacksPool.Count - 1);
        }

        public void AddRandomMonsterCardToPool()
        {
            var random = Random.Range(0, monsterAttacks.Count - 1);
            monsterAttacksPool.Add(monsterAttacks.ElementAt(random));
        }

        public void ReturnMonsterAttack(MonsterAttack attack)
        {
            monsterAttacksPool.Remove(attack);
        }

        private void FetchMonsterAttackData()
        {
            var forestLocationsJson = UnityEngine.Resources.Load<TextAsset>("MonsterAttacks/MonsterAttacks");
            var forestLocationsData = JsonUtility.FromJson<JsonWrapper<MonsterAttack>>(forestLocationsJson.text);
            foreach (var location in forestLocationsData.Items) monsterAttacks.Add(location);
        }
    }
}