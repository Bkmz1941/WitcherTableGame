using System.Collections.Generic;
using Controllers.EventsScene.models;
using Controllers.EventsScene.UI.Classes;
using Controllers.EventsScene.UI.Controllers;
using Controllers.EventsScene.UI.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Utils.JsonParser;

namespace Controllers.EventsScene.UI
{
    public class MainUIController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDoc;
        [SerializeField] public MonstersTabController monstersTabController;
        [FormerlySerializedAs("UsersTabController")] [SerializeField] public PlayersTabController playerTabController;

        [FormerlySerializedAs("alertQuestController")] [SerializeField]
        public QuestsController questsController;

        private VisualElement _rootElement;
        private bool isShowCard = true;
        private string _activeTab = "tab1";

        public void SetActiveTab(string tabName)
        {
            var tab1Content = _rootElement.Q<VisualElement>(name: "tab1Content");
            var tab2Content = _rootElement.Q<VisualElement>(name: "tab2Content");
            var tab3Content = _rootElement.Q<VisualElement>(name: "tab3Content");

            if (tabName.Equals("tab1"))
            {
                tab1Content.style.display = DisplayStyle.Flex;
                tab2Content.style.display = DisplayStyle.None;
                tab3Content.style.display = DisplayStyle.None;
            }

            if (tabName.Equals("tab2"))
            {
                tab1Content.style.display = DisplayStyle.None;
                tab2Content.style.display = DisplayStyle.Flex;
                tab3Content.style.display = DisplayStyle.None;
            }

            if (tabName.Equals("tab3"))
            {
                tab1Content.style.display = DisplayStyle.None;
                tab2Content.style.display = DisplayStyle.None;
                tab3Content.style.display = DisplayStyle.Flex;
            }
        }

        private void RegisterTabEvents()
        {
            var tab1Content = _rootElement.Q<VisualElement>(name: "tab1");
            var tab2Content = _rootElement.Q<VisualElement>(name: "tab2");
            var tab3Content = _rootElement.Q<VisualElement>(name: "tab3");

            tab1Content.RegisterCallback<MouseDownEvent>((evt) => { SetActiveTab("tab1"); });
            tab2Content.RegisterCallback<MouseDownEvent>((evt) => { SetActiveTab("tab2"); });
            tab3Content.RegisterCallback<MouseDownEvent>((evt) => { SetActiveTab("tab3"); });
        }

        public void OnEnable()
        {
            _rootElement = uiDoc.rootVisualElement;
            InitStartStylesElements();
            SetActiveTab("tab1");
            RegisterTabEvents();
            RegisterEvents();
        }

        public void InitStartStylesElements()
        {
            var selectCardContentElement = _rootElement.Q<VisualElement>(name: "selectCardContent");
            selectCardContentElement.style.display = DisplayStyle.Flex;
        }

        public void SetLocationAndShowCard(EventCardLocation eventCardLocation)
        {
            questsController.Show(eventCardLocation);
        }

        private void RegisterEvents()
        {
            // Alert
            var alertWrapper = _rootElement.Q<VisualElement>(name: "alertWrapper");
            alertWrapper.RegisterCallback<MouseDownEvent>((evt) => { alertWrapper.style.display = DisplayStyle.None; });
        }

        private void Reset()
        {
            var storyTextLabel = _rootElement.Q<Label>(name: "storyLabel");
            var variantATextLabel = _rootElement.Q<Label>(name: "variantALabel");
            var variantBTextLabel = _rootElement.Q<Label>(name: "variantBLabel");
            var decisionLabel = _rootElement.Q<Label>(name: "decisionLabel");
            storyTextLabel.text = null;
            variantATextLabel.text = null;
            variantATextLabel.SetEnabled(true);
            variantBTextLabel.text = null;
            variantBTextLabel.SetEnabled(true);
            decisionLabel.text = null;
        }

        private void HideCard()
        {
            var selectCardContentElement = _rootElement.Q<VisualElement>(name: "selectCardContent");
            var cardContentElement = _rootElement.Q<VisualElement>(name: "cardContent");
            cardContentElement.style.display = DisplayStyle.None;
            selectCardContentElement.style.display = DisplayStyle.Flex;
            Reset();
        }

        public void ShowAlert(string text)
        {
            var alertWrapper = _rootElement.Q<VisualElement>(name: "alertWrapper");
            alertWrapper.style.display = DisplayStyle.Flex;
            var label = alertWrapper.Q<Label>(name: "alertText");
            label.text = text;
        }
    }
}