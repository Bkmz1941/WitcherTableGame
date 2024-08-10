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
    public class PlayersTabController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDoc;
        [SerializeField] private PlayerViewController playerViewController;
        private VisualElement _rootElement;

        private void OnEnable()
        {
            _rootElement = uiDoc.rootVisualElement;

            UsersService.of().FetchMonstersData();
            DrawPlayers();
        }

        public void DrawPlayers()
        {
            var playersRow = _rootElement.Q<VisualElement>(name: "PlayersRow");
            playersRow.Clear();

            foreach (var player in UsersService.of().players)
            {
                VisualTreeAsset uiAsset =
                    UnityEngine.Resources.Load<VisualTreeAsset>("UIComponents/Player/UIPlayer");
                VisualElement playerTemplate = uiAsset.Instantiate();
                playerTemplate.Q<Label>(name: "UIPlayerName").text = player.Name;
                playerTemplate.Q<VisualElement>(name: "UIPlayerAvatar").style.backgroundImage =
                    UnityEngine.Resources.Load<Texture2D>(player.Avatar);
                playerTemplate.RegisterCallback<MouseDownEvent>((evt) => { playerViewController.Show(player); });
                playersRow.Add(playerTemplate);
            }
        }
    }
}