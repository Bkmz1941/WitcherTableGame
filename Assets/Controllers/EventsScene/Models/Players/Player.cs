using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Services;
using UnityEngine;

namespace Controllers.EventsScene.models.Players
{
    [System.Serializable]
    public class Player
    {
        public int Uuid;
        public string Name;
        public string Avatar;
        public List<StoryEvent> Quests;

        public Player(
            int uuid,
            string name,
            string avatar,
            List<StoryEvent> quests
        )
        {
            Uuid = uuid;
            Name = name;
            Avatar = avatar;
            Quests = quests;
        }

        public void ReturnQuestLocationToPull(int passedUuid)
        {
            var quest = Quests.Find((el) => el.Uuid == passedUuid);
            if (quest.VariantASelected)
            {
                foreach (var effect in quest.DecisionA.Effects)
                {
                    if (effect.LocationMeta.Generate && effect.LocationMeta.Location.Uuid != null)
                    {
                        Debug.Log("Return A");
                        QuestsService.of().ReturnLocation(effect.LocationMeta.Location);
                    }
                }
            }

            if (quest.VariantBSelected)
            {
                foreach (var effect in quest.DecisionB.Effects)
                {
                    if (effect.LocationMeta.Generate && effect.LocationMeta.Location.Uuid != null)
                    {
                        Debug.Log("Return B");
                        QuestsService.of().ReturnLocation(effect.LocationMeta.Location);
                    }
                }
            }
        }
    }
}