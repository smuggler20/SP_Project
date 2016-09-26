using SpaceInvaders.Character.ViewPreseter;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Character.Factory
{
    public class CharacterFactory : MonoBehaviour
    {
        private BaseViewPresenter.Factory _characterFactory;

        private RocketViewPresenter.Factory _rocketFactory;

        public BaseViewPresenter.Factory BaseCharacterFactory
        {
            get { return _characterFactory; }
        }

        public RocketViewPresenter.Factory RocketFactory
        {
            get { return _rocketFactory; }
        }

        [Inject]
        private void Construct(BaseViewPresenter.Factory characterFactory,
            RocketViewPresenter.Factory rocketFactory)
        {
            _characterFactory = characterFactory;
            _rocketFactory = rocketFactory;
        }
    }
}
