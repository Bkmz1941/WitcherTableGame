using Controllers.EventsScene.UI.Controllers;
using Controllers.EventsScene.UI.Enums;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers.EventsScene.UI
{
    public class UIContentController : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDoc;
        private VisualElement _rootElement;
        [SerializeField] private MainUIController UIController;
        [SerializeField] private QuestsController questsController;

        public void OnEnable()
        {
            _rootElement = uiDoc.rootVisualElement;
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            var cityCardElement = _rootElement.Q<VisualElement>(name: "cityCard");
            var villageCardElement = _rootElement.Q<VisualElement>(name: "villageCard");
            cityCardElement.RegisterCallback<MouseDownEvent, EventCardLocation>(SelectCard, EventCardLocation.City);
            villageCardElement.RegisterCallback<MouseDownEvent, EventCardLocation>(SelectCard,
                EventCardLocation.Village);
        }

        private void SelectCard(MouseDownEvent evt, EventCardLocation eventCardLocation)
        {
            if (
                eventCardLocation == EventCardLocation.City && questsController.GetCountOfCityStories() == 0 ||
                eventCardLocation == EventCardLocation.Village && questsController.GetCountOfVillageStories() == 0
            )
            {
                UIController.ShowAlert("Историй больше нет");
                return;
            }

            UIController.SetLocationAndShowCard(eventCardLocation);
        }
    }
}