using System;
using Controllers.EventsScene.models.Monster;
using Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers.EventsScene.UI.Controllers
{
    public class MonsterAttackProcessController
    {
        private Monster _monster;
        private VisualElement _rootElement;

        public MonsterAttackProcessController(VisualElement root)
        {
            _rootElement = root;
            MonstersService.of().Init();
            RegisterEvents();
        }

        public void InitBattle(Monster monster, VisualElement root)
        {
            _monster = monster;
            DrawMonsterLives(monster.Lives);
            DrawMonsterActiveStateBlock();
            MonstersService.of().GenerateMonsterAttackPool(monster.Lives);
        }

        private void DrawMonsterActiveStateBlock()
        {
            var monsterActiveStateBlock = _rootElement.Q<VisualElement>("MonsterActiveStateBlock");
            monsterActiveStateBlock.style.display = DisplayStyle.Flex;
        }

        private void DrawMonsterLives(int count)
        {
            var livesEl = _rootElement.Q<Label>("MonsterLive");
            livesEl.text = count.ToString();
        }

        private void MonsterAttack(bool canUseSpecialCounterAttack = false)
        {
            var monsterAttackView = _rootElement.Q<VisualElement>("MonsterAttackView");
            var monsterAttackViewText = monsterAttackView.Q<Label>("MonsterAttackViewText");
            monsterAttackView.style.display = DisplayStyle.Flex;
            var attack = MonstersService.of().GetMonsterAttackFromPool();

            if (attack.Special)
            {
                if (canUseSpecialCounterAttack) monsterAttackViewText.text = "Special counter attack";
                else
                {
                    Debug.Log(_monster);
                    monsterAttackViewText.text = attack.SpecialLevel switch
                    {
                        1 => "Special 1",
                        2 => "Special 2",
                        3 => "Special 3",
                        4 => "Special 4",
                        _ => monsterAttackViewText.text
                    };
                }

                return;
            }

            monsterAttackViewText.text = attack.Text;
        }

        private void RegisterEvents()
        {
            var monsterAttackView = _rootElement.Q<VisualElement>("MonsterAttackView");
            var monsterAttackViewClose = _rootElement.Q<Button>("MonsterAttackViewClose");
            var monsterAttackBtn = _rootElement.Q<Button>("MonsterAttackBtn");
            var monsterAttackReset = _rootElement.Q<Button>("MonsterAttackReset");
            var plusEl = _rootElement.Q<Button>("MonsterAttackAddLifeBtn");
            var minusEl = _rootElement.Q<Button>("MonsterAttackRemoveLifeBtn");
            var livesEl = _rootElement.Q<Label>("MonsterLive");

            monsterAttackViewClose.clicked += () => { monsterAttackView.style.display = DisplayStyle.None; };

            monsterAttackBtn.clicked += () => { MonsterAttack(); };

            monsterAttackReset.clicked += () =>
            {
                var monsterConfigBlock = _rootElement.Q<VisualElement>(name: "MonsterConfigBlock");
                monsterConfigBlock.SetEnabled(true);

                var monsterActiveStateBlock = _rootElement.Q<VisualElement>("MonsterActiveStateBlock");
                monsterActiveStateBlock.style.display = DisplayStyle.None;
            };

            plusEl.SetEnabled(true);
            plusEl.clicked += () =>
            {
                int count = Convert.ToInt32(livesEl.text);
                count++;
                DrawMonsterLives(count);
            };

            minusEl.SetEnabled(true);
            minusEl.clicked += () =>
            {
                int count = Convert.ToInt32(livesEl.text);
                count--;
                DrawMonsterLives(count);
                if (count <= 0) minusEl.SetEnabled(false);
            };
        }
    }
}