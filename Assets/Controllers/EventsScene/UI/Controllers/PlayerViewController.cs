using System;
using System.Collections.Generic;
using Controllers.EventsScene.models.Players;
using Services;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.JsonParser;

namespace Controllers.EventsScene.UI.Controllers
{
    public class PlayerViewController : MonoBehaviour
    {
        private UsersService _usersService;
        [SerializeField] private UIDocument uiDoc;
        private Player player;
        private VisualElement _rootElement;
        [SerializeField] private QuestsController questsController;

        public PlayerViewController()
        {
            _usersService = new UsersService();
        }

        public void Show(Player payloadPlayer)
        {
            player = payloadPlayer;
            DrawMainPlayerData();
            DrawPlayersQuests();
            ToggleDisplayElement(true);
        }

        public void DrawMainPlayerData()
        {
            var playerBox = _rootElement.Q<VisualElement>(name: "PlayerBox");
            playerBox.Q<Label>(name: "UIPlayerName").text = player.Name;
            playerBox.Q<VisualElement>(name: "UIPlayerAvatar").style.backgroundImage =
                UnityEngine.Resources.Load<Texture2D>(player.Avatar);
        }

        public void DrawPlayersQuests()
        {
            var playerViewQuestScrollView = _rootElement.Q<ScrollView>(name: "PlayerViewQuestScrollView");
            playerViewQuestScrollView.Clear();

            foreach (var quest in player.Quests)
            {
                var questElement = new Button();
                var questElementLabel = new Label(quest.Story);
                questElement.AddToClassList("player-view_quest");
                questElementLabel.AddToClassList("player-view_quest-title");
                questElement.Add(questElementLabel);
                playerViewQuestScrollView.Add(questElement);

                questElement.clicked += () => { questsController.ShowSpecificQuest(quest, player); };
            }
        }

        public void ToggleDisplayElement(bool isShow)
        {
            var playerView = _rootElement.Q<VisualElement>(name: "PlayerView");
            playerView.style.display = isShow ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void RegisterEvents()
        {
            var alertQuestClose = _rootElement.Q<VisualElement>(name: "PlayerViewClose");
            alertQuestClose.RegisterCallback<MouseDownEvent>((evt) => { ToggleDisplayElement(false); });
        }

        private void OnEnable()
        {
            _rootElement = uiDoc.rootVisualElement;
            RegisterEvents();
        }
    }
}