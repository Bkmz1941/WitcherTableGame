using System;
using Controllers.EventsScene.Dto;
using Controllers.EventsScene.models;
using Models.Location;
using Services;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers.EventsScene.UI.Classes
{
    public class StoryEventDecisionEffectController
    {
        private static StoryEventDecisionEffectController _instance;

        public static StoryEventDecisionEffectController of()
        {
            if (_instance == null) _instance = new StoryEventDecisionEffectController();
            return _instance;
        }

        public void ParseAndCreateEffects(UpdateDecisionFieldsDto dto, VisualElement root)
        {
            foreach (var effect in dto.Decision.Effects)
            {
                if (effect.Type.Equals("Quest")) HandleQuestableEffect(effect, root, dto.IsReplaceLocationBtn);
                else HandleSimpleEffect(effect, root);
            }
        }

        private void HandleQuestableEffect(StoryEventDecisionEffect effect, VisualElement root,
            bool isDrawReplaceQuestLocationBtn)
        {
            var effectElementWrapper = new VisualElement();
            effectElementWrapper.AddToClassList("decision-effect-wrapper");
            var effectElement = new VisualElement();
            effectElement.AddToClassList("decision-effect");
            var effectElementPicture = new VisualElement();
            effectElementPicture.AddToClassList("decision-effect-image");
            var effectElementLabel = new Label();
            effectElementPicture.style.backgroundImage = GetEffectImage(effect.Type);

            effectElementLabel.text = effect.Text;
            if (effect.Value.Length > 0)
            {
                effectElementLabel.text += " (" + effect.Value + ")";
            }

            effectElementWrapper.Add(effectElementPicture);
            effectElement.Add(effectElementLabel);

            effectElementWrapper.Add(effectElement);

            if (effect.LocationMeta.Location.Uuid == null && effect.LocationMeta.Generate)
            {
                var location = QuestsService.of().GetLocation(effect.LocationMeta.LocationType);
                if (location != null) effect.LocationMeta.Location = location;
            }

            if (effect.LocationMeta.Location.Uuid == null && effect.LocationMeta.Generate)
            {
                effect.LocationMeta.Location = new Location(
                    null,
                    "Доступных локаций нет",
                    "",
                    -1,
                    null
                );
            }

            if (effect.LocationMeta.Location.Number != 0)
            {
                var container =
                    DrawQuestLocation(effect.LocationMeta.Location, effectElement, isDrawReplaceQuestLocationBtn);

                // if (isDrawReplaceQuestLocationBtn && effect.LocationMeta.Location.Number != -1)
                // {
                //     var replaceLocationBtn = container.Q<Button>(name: "ReplaceLocationBtn");
                //     if (replaceLocationBtn != null)
                //     {
                //         replaceLocationBtn.clicked += () =>
                //         {
                //             UpdateLocationEvent(effect);
                //             RedrawQuestLocation(effect.LocationMeta.Location, container);
                //         };
                //     }
                // }
            }

            root.Add(effectElementWrapper);
        }

        private void UpdateLocationEvent(StoryEventDecisionEffect effect)
        {
            QuestsService.of().ReturnLocation(effect.LocationMeta!.Location);
            var newLocation = QuestsService.of().GetLocation(effect.LocationMeta.LocationType);
            effect.LocationMeta.Location = newLocation;
        }

        private void RedrawQuestLocation(Location location, VisualElement locationVisualElement)
        {
            var label = locationVisualElement.Q<Label>("LocationLabel");
            var image = locationVisualElement.Q<Label>("LocationImage");
            label.text = "Локация: " + location.Name;
        }

        private VisualElement DrawQuestLocation(Location location, VisualElement root,
            bool isDrawReplaceQuestLocationBtn)
        {
            var container = new VisualElement();
            container.AddToClassList("quest-location");
            var image = new VisualElement();
            image.name = "LocationImage";
            image.AddToClassList("quest-location_image");
            // image.style.backgroundImage = Resources.Load<Texture2D>(location.Image);
            var label = new Label();
            label.name = "LocationLabel";
            label.text = "Локация: " + location.Name;
            // if (location.Number != -1)
            // {
            //     label.text += " (" + location.Number + ")";
            // }

            container.AddToClassList("quest-location_text");
            // container.Add(image);
            container.Add(label);

            // if (isDrawReplaceQuestLocationBtn && location.Number != -1)
            // {
            //     var replaceLocationBtn = new Button();
            //     var replaceLocationBtnLabel = new Label("Заменить");
            //     replaceLocationBtn.name = "ReplaceLocationBtn";
            //     replaceLocationBtn.Add(replaceLocationBtnLabel);
            //     replaceLocationBtn.AddToClassList("simple_button");
            //     replaceLocationBtn.AddToClassList("quest-location-replace-btn");
            //     container.Add(replaceLocationBtn);
            // }

            root.Add(container);
            return container;
        }

        private void HandleSimpleEffect(StoryEventDecisionEffect effect, VisualElement root)
        {
            var effectElementWrapper = new VisualElement();
            effectElementWrapper.AddToClassList("decision-effect-wrapper");
            var effectElement = new VisualElement();
            effectElement.AddToClassList("decision-effect");
            var effectElementPicture = new VisualElement();
            effectElementPicture.AddToClassList("decision-effect-image");
            var effectElementLabel = new Label();
            effectElementPicture.style.backgroundImage = GetEffectImage(effect.Type);

            effectElementLabel.text = effect.Text;
            if (effect.Value.Length > 0)
            {
                effectElementLabel.text += " (" + effect.Value + ")";
            }

            effectElementWrapper.Add(effectElementPicture);
            effectElement.Add(effectElementLabel);

            effectElementWrapper.Add(effectElement);
            root.Add(effectElementWrapper);
        }

        private Texture2D GetEffectImage(string effect)
        {
            switch (effect)
            {
                case "LowerSkill": return UnityEngine.Resources.Load<Texture2D>("book_brown");
                case "AttackSkill": return UnityEngine.Resources.Load<Texture2D>("sword");
                case "SpecialSkill": return UnityEngine.Resources.Load<Texture2D>("book");
                case "AlchemySkill": return UnityEngine.Resources.Load<Texture2D>("potion_red");
                case "DefenceSkill": return UnityEngine.Resources.Load<Texture2D>("shield");
                case "AnySkill": return UnityEngine.Resources.Load<Texture2D>("book");
                case "Elixir": return UnityEngine.Resources.Load<Texture2D>("potion_blue");
                case "TrailTag": return UnityEngine.Resources.Load<Texture2D>("map");
                case "Gold": return UnityEngine.Resources.Load<Texture2D>("coins");
                case "Shield": return UnityEngine.Resources.Load<Texture2D>("shield");
                case "Quest": return UnityEngine.Resources.Load<Texture2D>("map");
                case "Move": return UnityEngine.Resources.Load<Texture2D>("map");
                case "TakeCardProcess": return UnityEngine.Resources.Load<Texture2D>("book");
                case "RemoveCard": return UnityEngine.Resources.Load<Texture2D>("book");
                default: return UnityEngine.Resources.Load<Texture2D>("coins");
            }
        }
    }
}