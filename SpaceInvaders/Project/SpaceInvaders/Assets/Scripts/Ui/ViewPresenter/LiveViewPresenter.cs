using SpaceInvaders.Character.Model;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

namespace SpaceInvaders.Ui
{
    public class LiveViewPresenter : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> lives;

        private PlayerModel _playerModel;
        private IMessageBroker _messageBroker;

        [Inject]
        private void Construct(PlayerModel playerModel, IMessageBroker messageBroker)
        {
            _playerModel = playerModel;
            _messageBroker = messageBroker;
        }

        void Start()
        {
            _playerModel.ObservableLife.Subscribe(val => 
            {
                val += 1;
                if(val <= 2)
                {
                    lives[val].SetActive(false);
                }
            });

            _messageBroker.Receive<Events.PlayGame>().
                Subscribe(_ =>
                {
                    _playerModel.Life = 2;
                    lives.ForEach(c => c.SetActive(true));
                });

            lives.ForEach(c => c.SetActive(true));
        }
    }
}
