using SpaceInvaders.Character.Factory;
using SpaceInvaders.Character.Model;
using SpaceInvaders.Character.ViewPreseter;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Utilis.Installers
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [SerializeField]
        private GameObject _characterPrefab;

        [SerializeField]
        private GameObject _rocketPrefab;

        private IMessageBroker _messageBroker = new MessageBroker();

        public override void InstallBindings()
        {
            //MessageBroker bind
            Container.Bind<IMessageBroker>().FromInstance(_messageBroker);

            //Factory bind
            Container.Bind<CharacterFactory>().AsSingle();
            Container.Bind<CharacterControllerFactroy>().AsSingle();
            Container.BindFactory<BaseViewPresenter, BaseViewPresenter.Factory>().FromPrefab(_characterPrefab);
            Container.BindFactory<RocketViewPresenter, RocketViewPresenter.Factory>().FromPrefab(_rocketPrefab);

            //Manager bind
            Container.Bind<GameManager>().AsSingle();

            //Model bind
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<EnemyModel>().AsSingle();
            Container.Bind<ObstacleModel>().AsSingle();
            Container.Bind<SpacedeerModel>().AsSingle();
        }
    }
}
