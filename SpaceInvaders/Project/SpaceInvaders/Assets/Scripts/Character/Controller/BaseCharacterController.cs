using SpaceInvaders.Character.Factory;
using SpaceInvaders.Character.Model;
using SpaceInvaders.Data.Controller;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.Controller
{
    public class BaseCharacterController : BaseController
    {
        protected readonly int _id;
        protected PlayerModel _playerModel;
        protected EnemyModel _enemyModel;
        protected ObstacleModel _obstacleModel;
        protected readonly CharacterFactory _characterFactory;

        public CharacterFactory CharacterFactory
        {
            get { return GameObject.FindObjectOfType<CharacterFactory>().GetComponent<CharacterFactory>(); }
        }

        public BaseCharacterController(IMessageBroker messageBroker,
            int id,
            PlayerModel playerModel,
            EnemyModel enemyModel) : base(messageBroker)
        {
            _id = id;
            _playerModel = playerModel;
            _characterFactory = CharacterFactory;
        }

        public BaseCharacterController(IMessageBroker messageBroker,
            int id) : base(messageBroker)
        {
            _id = id;
            _characterFactory = CharacterFactory;
        }

        public BaseCharacterController(IMessageBroker messageBroker,
            ObstacleModel obstacleModel,
            int id) : base(messageBroker)
        {
            _id = id;
            _characterFactory = CharacterFactory;
        }
    }
}
