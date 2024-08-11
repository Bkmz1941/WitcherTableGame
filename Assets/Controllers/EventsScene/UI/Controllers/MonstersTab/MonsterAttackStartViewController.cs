using System;
using Controllers.EventsScene.models.Monster;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers.EventsScene.UI.Controllers.MonstersTab
{
    public class MonsterAttackStartViewController
    {
        private VisualElement _rootElement;
        private int state = 1;
        private Func<Null> _funcAttack;
        public MonstersTabController TabController;

        public MonsterAttackStartViewController(MonstersTabController tabController)
        {
            TabController = tabController;
        }

        public MonsterAttackStartViewController Init(VisualElement root)
        {
            _rootElement = root;
            RegisterEvents();

            return this;
        }

        public MonsterAttackStartViewController Run(Monster monster)
        {
            state = 1;
            DrawUI(monster);
            Show();
            return this;
        }

        private void DrawUI(Monster monster)
        {
            var trailedLabel = _rootElement.Q<Label>("MonsterAttackStartViewTrailBlockText");
            if (monster.WasTrailed) trailedLabel.text = "Вы выследили монстра, поэтому сможете напасть первым";
            else trailedLabel.text = "Вы не смогли выследить монстра, он первым вас видит и нападает";

            // var monsterAttackStartViewSpecialTitle = _rootElement.Q<Label>("MonsterAttackStartViewSpecialTitle");
            var monsterAttackStartViewSpecialText = _rootElement.Q<Label>("MonsterAttackStartViewSpecialText");
            // monsterAttackStartViewSpecialTitle.text = monster.StartSkill.Title;
            monsterAttackStartViewSpecialText.text = monster.StartSkill.Text;

            if (monster.StartSkill.Time > 0)
            {
                TabController.AddElementTempEffects(monster.StartSkill);
                TabController.UpdateListAttackEffects();
            }

            UpdateContent();
        }

        private void UpdateContent()
        {
            var viewState1 = _rootElement.Q<VisualElement>("MonsterAttackStartViewHelpContent");
            var viewState2 = _rootElement.Q<VisualElement>("MonsterAttackStartViewMonsterStartSkill");
            if (state == 1)
            {
                state = 2;
                viewState1.style.display = DisplayStyle.Flex;
                viewState2.style.display = DisplayStyle.None;
                return;
            }

            if (state == 2)
            {
                state = 3;
                viewState1.style.display = DisplayStyle.None;
                viewState2.style.display = DisplayStyle.Flex;
                return;
            }
        }

        private void RegisterEvents()
        {
            // var closeBtn = _rootElement.Q<Button>("MonsterAttackStartViewClose");
            // closeBtn.clicked += Hide;

            var nextBtn = _rootElement.Q<Button>("MonsterAttackStartViewNext");
            nextBtn.clicked += () =>
            {
                if (state == 1)
                {
                    UpdateContent();
                    return;
                }

                if (state == 2)
                {
                    UpdateContent();
                    return;
                }

                if (state == 3)
                {
                    Hide();
                    _funcAttack();
                }
            };
        }

        public void SetAttackEvent(Func<Null> func)
        {
            _funcAttack = func;
        }

        private void Show()
        {
            var el = _rootElement.Q<VisualElement>("MonsterAttackStartView");
            el.style.display = DisplayStyle.Flex;
            el.BringToFront();
        }

        private void Hide()
        {
            _rootElement.Q<VisualElement>("MonsterAttackStartView").style.display = DisplayStyle.None;
        }
    }
}