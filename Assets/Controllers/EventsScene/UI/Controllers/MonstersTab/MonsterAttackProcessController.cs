using System;
using Controllers.EventsScene.models.Monster;
using Models.MonsterAttack;
using Resources.MonsterAttacks;
using Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers.EventsScene.UI.Controllers.MonstersTab
{
    public class MonsterAttackProcessController
    {
        private Monster _monster;
        private VisualElement _rootElement;
        public MonstersTabController TabController;
        private MonsterAttackSkill _currentMonsterAttackSkill;
        private MonsterAttackStartViewController _monsterAttackStartViewController;
        private int round;

        public MonsterAttackProcessController(VisualElement root, MonstersTabController monstersTabController)
        {
            _rootElement = root;
            TabController = monstersTabController;
            _monsterAttackStartViewController = new MonsterAttackStartViewController(TabController);
            MonstersService.of().Init();
            _monsterAttackStartViewController.Init(_rootElement);
            RegisterEvents();
        }

        public void InitBattle(Monster monster, VisualElement root)
        {
            _monster = monster;
            DrawMonsterLives(monster.Lives);
            DrawMonsterActiveStateBlock();
            DrawMonsterAttackStatsBlock();
            MonstersService.of().GenerateMonsterAttackPool(monster.Lives);
            DrawConfigUI();
            ResetRound();

            if (!monster.WasTrailed && monster.StartSkill.PassFirstAttack == false) OffsetRound();
            else ResetRound();

            _monsterAttackStartViewController.Run(monster).SetAttackEvent(() =>
            {
                if (monster.StartSkill.PassFirstAttack == false)
                {
                    MonsterAttack(MonstersService.of().GetMonsterAttackFromPool());
                    MonsterMinusLife();
                }

                return null;
            });
        }

        public void IncreaseRound()
        {
            var el = _rootElement.Q<Label>("MonsterAttackStatsRoundNumber");
            el.text = (++round).ToString();
        }

        public void ResetRound()
        {
            round = 1;
            var el = _rootElement.Q<Label>("MonsterAttackStatsRoundNumber");
            el.text = "1";
        }

        public void OffsetRound()
        {
            round = 0;
            var el = _rootElement.Q<Label>("MonsterAttackStatsRoundNumber");
            el.text = "0";
        }

        private void DrawConfigUI()
        {
            var minusEl = _rootElement.Q<Button>("MonsterAttackRemoveLifeBtn");
            var monsterAttackBtn = _rootElement.Q<Button>("MonsterAttackBtn");
            minusEl.SetEnabled(true);
            monsterAttackBtn.SetEnabled(true);
        }

        private void DrawMonsterActiveStateBlock()
        {
            var monsterActiveStateBlock = _rootElement.Q<VisualElement>("MonsterActiveStateBlock");
            var monsterAttackEffectsBlock = _rootElement.Q<VisualElement>("MonsterAttackEffectsBlock");
            monsterActiveStateBlock.style.display = DisplayStyle.Flex;
            monsterAttackEffectsBlock.style.display = DisplayStyle.Flex;
        }

        public void DrawMonsterAttackStatsBlock()
        {
            var monsterAttackEffectsBlock = _rootElement.Q<VisualElement>("MonsterAttackEffectsBlock");
            var monsterActiveStateBlock = _rootElement.Q<VisualElement>("MonsterAttackStatsBlock");
            monsterActiveStateBlock.style.display = DisplayStyle.Flex;
            monsterAttackEffectsBlock.style.display = DisplayStyle.Flex;
        }

        private void DrawMonsterLives(int count)
        {
            var livesEl = _rootElement.Q<Label>("MonsterLive");
            livesEl.text = count.ToString();
        }

        private void MonsterAttack(MonsterAttack attack, bool contrAttack = false)
        {
            IncreaseRound();
            
            MonstersService.of().ReturnMonsterAttack(attack);

            var monsterAttackViewAddEffect = _rootElement.Q<Button>("MonsterAttackViewAddEffect");
            var monsterAttackView = _rootElement.Q<VisualElement>("MonsterAttackView");
            var monsterAttackViewTitle = monsterAttackView.Q<Label>("MonsterAttackViewTitle");
            var monsterAttackViewText = monsterAttackView.Q<Label>("MonsterAttackViewText");
            var monsterAttackType = monsterAttackView.Q<Label>("MonsterAttackType");
            monsterAttackView.style.display = DisplayStyle.Flex;
            monsterAttackViewAddEffect.SetEnabled(true);

            if (attack.Special)
            {
                monsterAttackType.AddToClassList("special-attack");
                if (contrAttack)
                {
                    monsterAttackType.text = "Особый эффект";
                    var el = _monster.SpecialSkill;
                    monsterAttackViewText.text = el.Text;
                    monsterAttackViewTitle.text = el.Title;
                    _currentMonsterAttackSkill = _monster.SpecialSkill;
                    
                    if (el.Time > 0) TabController.AddElementTempEffects(_currentMonsterAttackSkill);
                }
                else
                {
                    monsterAttackType.text = "Особая атака";
                    var el = _monster.AttackSkills.Find((el) => el.Level == attack.SpecialLevel);
                    monsterAttackViewText.text = el.Text;
                    monsterAttackViewTitle.text = el.Title;
                    _currentMonsterAttackSkill =  el;
                    
                    if (el.Time > 0) TabController.AddElementTempEffects(_currentMonsterAttackSkill);
                }

                TabController.UpdateListAttackEffects();
                return;
            }

            monsterAttackViewAddEffect.style.display = DisplayStyle.None;
            monsterAttackType.RemoveFromClassList("special-attack");
            monsterAttackType.text = "Обычная атака";
            monsterAttackViewTitle.text = attack.Title;
            monsterAttackViewText.text = attack.Text;
            TabController.UpdateListAttackEffects();
        }

        private void RegisterEvents()
        {
            var monsterAttackView = _rootElement.Q<VisualElement>("MonsterAttackView");
            var monsterAttackViewAddEffect = _rootElement.Q<Button>("MonsterAttackViewAddEffect");
            var monsterAttackViewClose = _rootElement.Q<Button>("MonsterAttackViewClose");
            var monsterAttackBtn = _rootElement.Q<Button>("MonsterAttackBtn");
            var monsterAttackReset = _rootElement.Q<Button>("MonsterAttackReset");
            var plusEl = _rootElement.Q<Button>("MonsterAttackAddLifeBtn");
            var minusEl = _rootElement.Q<Button>("MonsterAttackRemoveLifeBtn");
            var livesEl = _rootElement.Q<Label>("MonsterLive");

            monsterAttackViewAddEffect.clicked += () =>
            {
                monsterAttackViewAddEffect.SetEnabled(false);
                TabController.AddElementTempEffects(_currentMonsterAttackSkill);
            };

            monsterAttackViewClose.clicked += () =>
            {
                monsterAttackView.style.display = DisplayStyle.None;
                _currentMonsterAttackSkill = null;
            };

            monsterAttackBtn.clicked += () =>
            {
                MonsterAttack(MonstersService.of().GetMonsterAttackFromPool());
                MonsterMinusLife();
            };

            monsterAttackReset.clicked += () =>
            {
                var monsterConfigBlock = _rootElement.Q<VisualElement>(name: "MonsterConfigBlock");
                monsterConfigBlock.SetEnabled(true);

                var monsterActiveStateBlock = _rootElement.Q<VisualElement>("MonsterActiveStateBlock");
                monsterActiveStateBlock.style.display = DisplayStyle.None;

                var monsterActiveStatsBlock = _rootElement.Q<VisualElement>("MonsterAttackStatsBlock");
                monsterActiveStatsBlock.style.display = DisplayStyle.None;
                
                var monsterAttackEffectsBlock = _rootElement.Q<VisualElement>("MonsterAttackEffectsBlock");
                monsterAttackEffectsBlock.style.display = DisplayStyle.None;
                
                TabController.RemoveAndUpdateElementTempEffects();
            };

            plusEl.SetEnabled(true);
            plusEl.clicked += () =>
            {
                MonstersService.of().AddRandomMonsterCardToPool();
                int count = Convert.ToInt32(livesEl.text);
                count++;
                DrawMonsterLives(count);

                minusEl.SetEnabled(true);
                monsterAttackBtn.SetEnabled(true);
            };

            minusEl.SetEnabled(true);
            minusEl.clicked += () =>
            {
                var contrAttack = MonstersService.of().GetMonsterAttackFromPool();
                if (contrAttack.Special)
                {
                    MonsterAttack(contrAttack, true);
                }

                MonstersService.of().ReturnMonsterAttack(contrAttack);
                MonsterMinusLife();
            };
        }

        private void MonsterMinusLife()
        {
            var livesEl = _rootElement.Q<Label>("MonsterLive");
            var minusEl = _rootElement.Q<Button>("MonsterAttackRemoveLifeBtn");
            var monsterAttackBtn = _rootElement.Q<Button>("MonsterAttackBtn");
            int count = Convert.ToInt32(livesEl.text);
            count--;
            DrawMonsterLives(count);
            if (count <= 0)
            {
                minusEl.SetEnabled(false);
                monsterAttackBtn.SetEnabled(false);
            }
        }
    }
}