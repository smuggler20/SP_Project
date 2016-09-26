using SpaceInvaders.Character.Controller;
using SpaceInvaders.Character.Model;
using SpaceInvaders.Data.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Character.Factory
{
    public class CharacterControllerFactroy
    {
        private DiContainer _container;
        private IMessageBroker _messageBroker;
        private PlayerModel _playerModel;
        private EnemyModel _enemyModel;
        private ObstacleModel _obstacleModel;

        public CharacterControllerFactroy(DiContainer container,
            IMessageBroker messageBroker,
            PlayerModel playerModel,
            EnemyModel enemyModel,
            ObstacleModel obstacleModel,
            SpacedeerModel spacedeerModel)
        {
            _container = container;
            _messageBroker = messageBroker;
            _playerModel = playerModel;
            _enemyModel = enemyModel;
            _obstacleModel = obstacleModel;
        }

        public BaseCharacterController Create(int id, CharacterType characterType, Vector2 position, string characterName = null)
        {
            switch (characterType)
            {
                case CharacterType.Player:
                    return _container.Instantiate<PlayerController>(new object[] {_messageBroker, id, _playerModel, position});
                case CharacterType.EnemyStandard:
                    return _container.Instantiate<EnemyController>(new object[] {_messageBroker, id, _enemyModel, position, characterName});
                case CharacterType.Obstacle:
                    return _container.Instantiate<ObstacleController>(new object[] { _messageBroker, id, _obstacleModel, position});
                default:
                    return null;
            }
        }

        public BaseCharacterController Create(int id, CharacterType characterType, Vector2 position, RocketDirection direction)
        {
            switch (characterType)
            {
                case CharacterType.Rocket:
                    return _container.Instantiate<RocketController>(new object[] { _messageBroker, id, position, direction});
                default:
                    return null;
            }
        }

        public BaseCharacterController Create(int id, CharacterType characterType, Vector2 position, SpacedeerDirection direction)
        {
            switch (characterType)
            {
                case CharacterType.Spacedeer:
                    return _container.Instantiate<SpacedeerController>(new object[] { _messageBroker, id, position, direction });
                default:
                    return null;
            }
        }
    }
}
