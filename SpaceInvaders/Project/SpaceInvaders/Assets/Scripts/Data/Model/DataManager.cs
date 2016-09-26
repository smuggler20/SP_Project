using SpaceInvaders.Character.Model;
using SpaceInvaders.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceInvaders.Data
{
    public static class DataManager
    {
        public static class Character
        {
            public static Sprite LoadSprite<T>(string name) where T : BaseModel
            {
                return Resources.LoadAll<Sprite>(_pathDictionary[typeof (T)]).Single(c => c.name == name);
            }

            public static IEnumerable<Sprite> LoadAllSprite<T>() where T : BaseModel
            {
                return Resources.LoadAll<Sprite>(_pathDictionary[typeof(T)]).ToList();
            }
        }

        public static class Data
        {
            public static Sprite LoadSprite(string name, string path)
            {
                return Resources.LoadAll<Sprite>(path).Single(c => c.name == name);
            }
        }

        private const string _dataPath = "Data/";
        private const string _characterPath = _dataPath + "Character/";
        private const string _rocketPath = _dataPath + "Rocket/";

        private static Dictionary<Type, string> _pathDictionary = new Dictionary<Type, string>()
        {
            {typeof(PlayerModel), _characterPath + "Player/" },
            {typeof(EnemyModel), _characterPath + "Enemy/"},
            {typeof(ObstacleModel), _characterPath + "Obstacle/"},
            {typeof(SpacedeerModel), _characterPath + "Spacedeer/"}
        };
    }
}
