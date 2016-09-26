using SpaceInvaders.Character.Model;
using SpaceInvaders.Utilis;
using UniRx;
using Zenject;

namespace SpaceInvaders.Character.ViewPreseter
{
    public class BaseViewPresenter : BaseDisposableMonoBehaviour
    {
        public IMessageBroker _messageBroker;
        public EnemyModel _enemyModel;
        public PlayerModel _playerModel;

        protected float _leftBlocker = -9.0f;
        protected float _rightBlocker = 9.0f;

        [Inject]
        private void Construct(IMessageBroker messageBroker, EnemyModel enemyModel, PlayerModel playerModel)
        {
            _messageBroker = messageBroker;
            _enemyModel = enemyModel;
            _playerModel = playerModel;
        }

        public class Factory : Factory<BaseViewPresenter>
        {

        }
    }
}
