using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.EventsScene.Dto;
using Controllers.EventsScene.models;
using Controllers.EventsScene.models.Players;
using Controllers.EventsScene.UI.Classes;
using Controllers.EventsScene.UI.Enums;
using Models.Location;
using Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utils.JsonParser;
using Button = UnityEngine.UIElements.Button;
using Random = UnityEngine.Random;

namespace Controllers.EventsScene.UI.Controllers
{
    public class QuestsController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDoc;
        private StoryEvent _currentStoryEvent;
        [SerializeField] private MainUIController _mainUIController;
        [SerializeField] private PlayerViewController playerViewController;
        private EventCardLocation _eventCardLocation = EventCardLocation.City;
        private VisualElement _rootElement;
        private Player player;
        private bool isQuestAddedToPlayer = false;
        private bool isNewQuest = true;

        public TextAsset cityEventsJson;
        public TextAsset villageEventsJson;
        private readonly List<StoryEvent> _cityStories = new List<StoryEvent>();
        private readonly List<StoryEvent> _villageStories = new List<StoryEvent>();

        public void Show(EventCardLocation location)
        {
            isNewQuest = true;
            isQuestAddedToPlayer = false;
            player = null;

            SetEventLocation(location);
            var alertQuest = _rootElement.Q<VisualElement>(name: "AlertQuest");
            alertQuest.style.display = DisplayStyle.Flex;
            SetRandomCurrentStory();
            UpdateStoryTextFields();
            RegisterVariansEvents();
            RemoveFinishBtn();
        }

        public void ShowSpecificQuest(StoryEvent quest, Player argPlayer)
        {
            isNewQuest = false;
            player = argPlayer;

            var alertQuest = _rootElement.Q<VisualElement>(name: "AlertQuest");
            alertQuest.style.display = DisplayStyle.Flex;
            alertQuest.BringToFront();
            _currentStoryEvent = quest;
            UpdateStoryTextFields();
            UnregisterEvents();
            UpdateDecisionTextFields(null, new UpdateDecisionFieldsDto(
                _currentStoryEvent.VariantASelected ? "A" : "B",
                false
            ));
            DrawFinishBtn();
        }

        void DrawFinishBtn()
        {
            var alertQuestPlayerActionsBlock = _rootElement.Q<VisualElement>(name: "AlertQuestPlayerActionsBlock");
            var btn = _rootElement.Q<Button>(name: "AlertQuestPlayerActionFinishQuest");
            if (btn != null) alertQuestPlayerActionsBlock.Remove(btn);

            alertQuestPlayerActionsBlock.AddToClassList("quest-player-actions");
            var selectPlayerSubmit = new Button();
            selectPlayerSubmit.name = "AlertQuestPlayerActionFinishQuest";
            var selectPlayerSubmitLabel = new Label("Завершить");
            selectPlayerSubmit.Add(selectPlayerSubmitLabel);
            selectPlayerSubmit.AddToClassList("quest-player-btn");
            alertQuestPlayerActionsBlock.Add(selectPlayerSubmit);

            selectPlayerSubmit.clicked += () =>
            {
                int idx = player.Quests.FindIndex((el) => el.Uuid == _currentStoryEvent.Uuid);
                StoryEvent quest = player.Quests.Find((el) => el.Uuid == _currentStoryEvent.Uuid);

                player.ReturnQuestLocationToPull(quest.Uuid);
                player.Quests.RemoveAt(idx);
                playerViewController.DrawPlayersQuests();
                CloseView();
            };
        }

        public void RemoveFinishBtn()
        {
            var alertQuestPlayerActionsBlock = _rootElement.Q<VisualElement>(name: "AlertQuestPlayerActionsBlock");
            var btn = _rootElement.Q<Button>(name: "AlertQuestPlayerActionFinishQuest");
            if (btn != null) alertQuestPlayerActionsBlock.Remove(btn);
        }

        private void UnregisterEvents()
        {
            var variantAElement = _rootElement.Q<VisualElement>(name: "variantA");
            variantAElement.UnregisterCallback<MouseDownEvent, UpdateDecisionFieldsDto>(UpdateDecisionTextFields);
            variantAElement.SetEnabled(_currentStoryEvent.VariantASelected);

            var variantBElement = _rootElement.Q<VisualElement>(name: "variantB");
            variantBElement.UnregisterCallback<MouseDownEvent, UpdateDecisionFieldsDto>(UpdateDecisionTextFields);
            variantBElement.SetEnabled(_currentStoryEvent.VariantBSelected);
        }

        private void AddQuestToPlayer(DropdownField dropdownField)
        {
            UsersService.of().AddQuestToPlayerByName(dropdownField.value, _currentStoryEvent);
            var replaceLocationBtn = _rootElement.Q<VisualElement>(name: "ReplaceLocationBtn");
            if (replaceLocationBtn != null) replaceLocationBtn.SetEnabled(false);
            isQuestAddedToPlayer = true;
        }

        public int GetCountOfCityStories()
        {
            return _cityStories.Count;
        }

        public int GetCountOfVillageStories()
        {
            return _villageStories.Count;
        }

        public void SetEventLocation(EventCardLocation location)
        {
            this._eventCardLocation = location;
        }

        private void OnEnable()
        {
            _rootElement = uiDoc.rootVisualElement;

            QuestsService.of().FetchLocationsData();

            RegisterEvents();
            InitEventStories();
            RegisterVariansEvents();
        }

        private void RegisterEvents()
        {
            var alertQuestClose = _rootElement.Q<VisualElement>(name: "AlertQuestClose");
            alertQuestClose.RegisterCallback<MouseDownEvent>((evt) =>
            {
                if (isQuestAddedToPlayer == false && isNewQuest)
                {
                    if (_currentStoryEvent.VariantASelected && _currentStoryEvent.DecisionA.Questable)
                    {
                        foreach (var effect in _currentStoryEvent.DecisionA.Effects)
                        {
                            if (effect.LocationMeta.Generate && effect.LocationMeta.Location.Uuid != null)
                            {
                                Debug.Log("Return");
                                QuestsService.of().ReturnLocation(effect.LocationMeta.Location);
                            }
                        }
                    }

                    if (_currentStoryEvent.VariantBSelected && _currentStoryEvent.DecisionB.Questable)
                    {
                        foreach (var effect in _currentStoryEvent.DecisionB.Effects)
                        {
                            if (effect.LocationMeta.Generate && effect.LocationMeta.Location.Uuid != null)
                            {
                                QuestsService.of().ReturnLocation(effect.LocationMeta.Location);
                            }
                        }
                    }
                }

                CloseView();
            });
        }

        private void CloseView()
        {
            var alertQuest = _rootElement.Q<VisualElement>(name: "AlertQuest");
            alertQuest.style.display = DisplayStyle.None;
        }

        private void RegisterVariansEvents()
        {
            // Labels
            var variantAElement = _rootElement.Q<VisualElement>(name: "variantA");
            variantAElement.RegisterCallback<MouseDownEvent, UpdateDecisionFieldsDto>(UpdateDecisionTextFields,
                new UpdateDecisionFieldsDto("A", true, true));
            variantAElement.SetEnabled(true);

            var variantBElement = _rootElement.Q<VisualElement>(name: "variantB");
            variantBElement.RegisterCallback<MouseDownEvent, UpdateDecisionFieldsDto>(UpdateDecisionTextFields,
                new UpdateDecisionFieldsDto("B", true, true));
            variantBElement.SetEnabled(true);
        }

        public void DrawSelectPlayer()
        {
            var alertQuestPlayer = _rootElement.Q<VisualElement>(name: "AlertQuestPlayer");
            alertQuestPlayer.style.display = DisplayStyle.Flex;
            alertQuestPlayer.Clear();

            var selectPlayerContainerRoot = new VisualElement();
            var selectPlayerContainer = new VisualElement();
            var selectPlayerLabel = new Label("Добавить квест игроку");
            selectPlayerContainerRoot.AddToClassList("select-player-container_root");
            selectPlayerContainerRoot.style.display = DisplayStyle.Flex;
            selectPlayerContainer.AddToClassList("select-player-container");
            List<string> options = new List<string>() { };
            foreach (var player in UsersService.of().players) options.Add(player.Name);

            var selectPlayerSubmit = new Button();
            var selectPlayerSubmitLabel = new Label("Добавить");
            selectPlayerSubmit.Add(selectPlayerSubmitLabel);
            selectPlayerSubmit.AddToClassList("select-player-container_submit");

            var selectPlayerDropdown = new DropdownField(null, options, 0);
            selectPlayerDropdown.AddToClassList("select-player-container_dropdown");

            selectPlayerContainerRoot.Add(selectPlayerLabel);
            selectPlayerContainerRoot.Add(selectPlayerContainer);
            selectPlayerContainer.Add(selectPlayerDropdown);
            selectPlayerContainer.Add(selectPlayerSubmit);
            alertQuestPlayer.Add(selectPlayerContainerRoot);

            selectPlayerSubmit.clicked += () =>
            {
                AddQuestToPlayer(selectPlayerDropdown);
                selectPlayerDropdown.SetEnabled(false);
                selectPlayerSubmit.SetEnabled(false);
            };
        }

        private void SetRandomCurrentStory()
        {
            if (_eventCardLocation == EventCardLocation.City)
            {
                var random = Random.Range(0, _cityStories.Count - 1);
                _currentStoryEvent = _cityStories[random];
                _cityStories.Remove(_currentStoryEvent);
            }

            if (_eventCardLocation == EventCardLocation.Village)
            {
                var random = Random.Range(0, _villageStories.Count - 1);
                _currentStoryEvent = _villageStories[random];
                _villageStories.Remove(_currentStoryEvent);
            }
        }

        private void UpdateStoryTextFields()
        {
            var storyTextLabel = _rootElement.Q<Label>(name: "storyLabel");
            var variantAElement = _rootElement.Q<VisualElement>(name: "variantA");
            var variantATextLabel = _rootElement.Q<Label>(name: "variantALabel");
            var variantBElement = _rootElement.Q<VisualElement>(name: "variantB");
            var variantBTextLabel = _rootElement.Q<Label>(name: "variantBLabel");
            var decisionLabel = _rootElement.Q<VisualElement>(name: "AlertQuestRightSide");
            storyTextLabel.text = _currentStoryEvent.Story;

            if (_currentStoryEvent.VariantA.Length != 0)
            {
                variantAElement.style.display = DisplayStyle.Flex;
                variantATextLabel.text = _currentStoryEvent.VariantA;
            }
            else
            {
                variantAElement.style.display = DisplayStyle.None;
            }

            if (_currentStoryEvent.VariantB.Length != 0)
            {
                variantBElement.style.display = DisplayStyle.Flex;
                variantBTextLabel.text = _currentStoryEvent.VariantB;
            }
            else
            {
                variantBElement.style.display = DisplayStyle.None;
            }

            decisionLabel.style.display = DisplayStyle.None;
        }

        private void UpdateDecisionTextFields(MouseDownEvent evt, UpdateDecisionFieldsDto dto)
        {
            var decisionElement = _rootElement.Q<VisualElement>(name: "AlertQuestRightSide");
            var decisionLabel = _rootElement.Q<Label>(name: "decisionLabel");

            var alertQuestPlayer = _rootElement.Q<VisualElement>(name: "AlertQuestPlayer");
            alertQuestPlayer.style.display = DisplayStyle.None;

            decisionElement.style.display = DisplayStyle.Flex;

            var variantATextLabel = _rootElement.Q<VisualElement>(name: "variantB");
            var variantBTextLabel = _rootElement.Q<VisualElement>(name: "variantB");

            if (dto.Variant.Equals("A"))
            {
                decisionLabel.text = _currentStoryEvent.DecisionA.Text;
                _currentStoryEvent.VariantASelected = true;

                variantBTextLabel.SetEnabled(false);
                ParseAndDrawDecisionEvents("A", dto);

                if (dto.ShowSelectPlayer && _currentStoryEvent.DecisionA.Questable) DrawSelectPlayer();
            }
            else
            {
                decisionLabel.text = _currentStoryEvent.DecisionB.Text;
                _currentStoryEvent.VariantBSelected = true;

                variantATextLabel.SetEnabled(false);
                ParseAndDrawDecisionEvents("B", dto);

                if (dto.ShowSelectPlayer && _currentStoryEvent.DecisionB.Questable) DrawSelectPlayer();
            }

            UnregisterEvents();
        }

        private void ParseAndDrawDecisionEvents(string variant, UpdateDecisionFieldsDto dto)
        {
            var decisionEffectsElement = _rootElement.Q<VisualElement>(name: "decisionEffects");
            decisionEffectsElement.Clear();

            StoryEventDecision decision = variant.Equals("A")
                ? _currentStoryEvent.DecisionA
                : _currentStoryEvent.DecisionB;
            dto.Decision = decision;
            StoryEventDecisionEffectController.of().ParseAndCreateEffects(dto, decisionEffectsElement);
        }

        private void InitEventStories()
        {
            var cityWrapper = JsonUtility.FromJson<JsonWrapper<StoryEvent>>(cityEventsJson.text);
            foreach (var element in cityWrapper.Items)
            {
                _cityStories.Add(element);
            }

            var villageWrapper = JsonUtility.FromJson<JsonWrapper<StoryEvent>>(villageEventsJson.text);
            foreach (var element in villageWrapper.Items)
            {
                _villageStories.Add(element);
            }
        }
    }
}