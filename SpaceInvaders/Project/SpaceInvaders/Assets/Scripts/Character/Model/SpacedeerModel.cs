using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceInvaders.Data.Model;

namespace SpaceInvaders.Character.Model
{
    public class SpacedeerModel : BaseModel
    {
        public int Score
        {
            get
            {
                _score = UnityEngine.Random.Range(50, 300);
                return _score;
            }
        }

        private int _score;
    }
}
