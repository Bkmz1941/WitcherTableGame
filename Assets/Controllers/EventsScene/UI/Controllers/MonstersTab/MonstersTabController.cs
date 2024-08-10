using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.EventsScene.models.Monster;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Utils.JsonParser;

namespace Controllers.EventsScene.UI.Controllers
{
    public class MonstersTabController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDoc;
        [SerializeField] private EventSystem eventSystem;
        private MonsterAttackProcessController _monsterAttackProcessController;
        private VisualElement _rootElement;
        private List<Monster> _monsters = new List<Monster>();
        private Monster _currentMonster;


        private void OnEnable()
        {
            _rootElement = uiDoc.rootVisualElement;
            _monsterAttackProcessController = new MonsterAttackProcessController(_rootElement);

            FetchMonstersData();
            SetCurrentMonster(_monsters[0].Uuid);
            DrawMonster();
            InitMonsterStatsUI();
            RegisterEvents();
        }

        private void InitMonsterStatsUI()
        {
            var monsterAttackView = _rootElement.Q<VisualElement>("MonsterAttackView");
            monsterAttackView.style.display = DisplayStyle.None;
            
            var monsterActiveStateBlock = _rootElement.Q<VisualElement>("MonsterActiveStateBlock");
            monsterActiveStateBlock.style.display = DisplayStyle.None;
            var trailedToggleWrapper = _rootElement.Q<VisualElement>("MonsterWasTrailedToggleWrapper");
            var trailedToggle = _rootElement.Q<Toggle>("MonsterWasTrailedToggle");
            trailedToggleWrapper.RegisterCallback<MouseDownEvent>((evt) =>
            {
                trailedToggle.value = !trailedToggle.value;
                _currentMonster.WasTrailed = trailedToggle.value;
            });
        }

        public void RegisterEvents()
        {
            var monsterAttackProcessCreateBtn = _rootElement.Q<Button>(name: "MonsterAttackProcessCreateBtn");
            monsterAttackProcessCreateBtn.clicked += () =>
            {
                var monsterConfigBlock = _rootElement.Q<VisualElement>(name: "MonsterConfigBlock");
                monsterConfigBlock.SetEnabled(false);
                _monsterAttackProcessController.InitBattle(_currentMonster, _rootElement);
            };

            var openWeaknessesBtn = _rootElement.Q<Button>(name: "openWeaknessesBtn");
            var previousMonsterBtn = _rootElement.Q<Button>(name: "previousMonsterBtn");
            var nextMonsterBtn = _rootElement.Q<Button>(name: "nextMonsterBtn");
            var monsterWeaknessesClose = _rootElement.Q<VisualElement>(name: "monsterWeaknessesClose");
            openWeaknessesBtn.clicked += ShowMonsterWeaknessesContent;
            previousMonsterBtn.clicked += PreviousMonster;
            nextMonsterBtn.clicked += NextMonster;
            monsterWeaknessesClose.RegisterCallback<MouseDownEvent>((e) =>
            {
                var monsterWeaknessesContent = _rootElement.Q<VisualElement>(name: "monsterWeaknessesContent");
                var mainMonsterContent = _rootElement.Q<VisualElement>(name: "mainMonsterContent");
                monsterWeaknessesContent.style.display = DisplayStyle.None;
                mainMonsterContent.style.display = DisplayStyle.Flex;
            });
        }

        private void NextMonster()
        {
            int idx = _monsters.IndexOf(_currentMonster);
            idx = idx + 1;
            if (idx > _monsters.Count - 1) idx = 0;

            SetCurrentMonster(_monsters.ElementAt(idx).Uuid);
            DrawMonster();
        }

        private void PreviousMonster()
        {
            int idx = _monsters.IndexOf(_currentMonster);
            idx = idx - 1;
            if (idx < 0) idx = _monsters.Count - 1;

            SetCurrentMonster(_monsters.ElementAt(idx).Uuid);
            DrawMonster();
        }

        private void DrawMonster()
        {
            var monsterImage = _rootElement.Q<VisualElement>(name: "monsterImage");
            monsterImage.style.backgroundImage = UnityEngine.Resources.Load<Texture2D>(_currentMonster.Image);

            DrawMonsterWeaknessesContent();
        }

        private void DrawMonsterWeaknessesContent()
        {
            var monsterWeaknessesTitle = _rootElement.Q<Label>(name: "monsterWeaknessesTitle");
            var monsterWeaknessesDetails = _rootElement.Q<VisualElement>(name: "monsterWeaknessesDetails");
            var monsterWeaknessesLeftSide = _rootElement.Q<VisualElement>(name: "monsterWeaknessesLeftSide");
            monsterWeaknessesLeftSide.Clear();

            monsterWeaknessesTitle.text = _currentMonster.Name;
            monsterWeaknessesDetails.style.display = DisplayStyle.None;

            if (_currentMonster.Weaknesses != null && _currentMonster.Weaknesses.Length > 1)
            {
                foreach (var weakness in _currentMonster.Weaknesses)
                {
                    var weaknessElementButton = new Button();
                    var weaknessElementButtonLabel = new Label();
                    weaknessElementButtonLabel.text = "Слабость #" + weakness.Uuid;
                    weaknessElementButtonLabel.transform.position = new Vector3(0, -7, 0);
                    weaknessElementButton.AddToClassList("monster-weakness-effect_button");
                    weaknessElementButton.AddToClassList("monster-weakness-" + weakness.Uuid);
                    weaknessElementButton.Add(weaknessElementButtonLabel);

                    monsterWeaknessesLeftSide.Add(weaknessElementButton);
                    weaknessElementButton.clicked += () =>
                    {
                        monsterWeaknessesDetails.style.display = DisplayStyle.Flex;
                        var monsterWeaknessesText = _rootElement.Q<Label>(name: "monsterWeaknessesText");
                        var monsterWeaknessesEffect = _rootElement.Q<Label>(name: "monsterWeaknessesEffect");
                        monsterWeaknessesText.text = weakness.Text;
                        monsterWeaknessesEffect.text = weakness.Effect;
                    };
                }
            }
        }

        private void ShowMonsterWeaknessesContent()
        {
            var monsterWeaknessesContent = _rootElement.Q<VisualElement>(name: "monsterWeaknessesContent");
            var mainMonsterContent = _rootElement.Q<VisualElement>(name: "mainMonsterContent");
            monsterWeaknessesContent.style.display = DisplayStyle.Flex;
            mainMonsterContent.style.display = DisplayStyle.None;
        }

        private void SetCurrentMonster(int uuid)
        {
            _currentMonster = _monsters.Find(item => item.Uuid == uuid);
        }

        private void FetchMonstersData()
        {
            var monstersDataJson = UnityEngine.Resources.Load<TextAsset>("Monsters/MonstersData");
            var monsters = JsonUtility.FromJson<JsonWrapper<Monster>>(monstersDataJson.text);
            foreach (var monster in monsters.Items)
            {
                if (monster.isAvailable) _monsters.Add(monster);
            }
        }
    }
}