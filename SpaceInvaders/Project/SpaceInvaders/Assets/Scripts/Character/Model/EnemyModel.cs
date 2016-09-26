using SpaceInvaders.Data.Model;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Character.Model
{
    public class EnemyModel : BaseModel
    {
        public List<EnemyCoord> EnemyList
        {
            get { return _enemyList; }
            set { _enemyList = value; }
        }

        public List<EnemyCoord> _enemyList;
    }

    public class EnemyCoord
    {
        public CharacterType CharacterType;
        public int CharacterScore;
        public Vector2 position;
        public int Id;
    }
}
