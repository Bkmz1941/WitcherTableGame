using System.Collections.Generic;
using System.Linq;
using Controllers.EventsScene.models;
using Controllers.EventsScene.models.Players;
using UnityEngine;
using Utils.JsonParser;

namespace Services
{
    public class UsersService
    {
        public List<Player> players = new List<Player>();
        public static UsersService instance;

        public static UsersService of()
        {
            if (UsersService.instance == null) UsersService.instance = new UsersService();
            return instance;
        }

        public void FetchMonstersData()
        {
            var playersDataJson = UnityEngine.Resources.Load<TextAsset>("Players/PlayersData");
            var playersData = JsonUtility.FromJson<JsonWrapper<Player>>(playersDataJson.text);
            foreach (var player in playersData.Items) players.Add(player);
        }

        public void AddQuestToPlayerByName(string name, StoryEvent quest)
        {
            int playerIdx = players.FindIndex((player) => player.Name.Equals(name));
            players.ElementAt(playerIdx).Quests.Add(quest);
        }
    }
}