using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Events
{
    public class PlayGame
    {

    }
}

namespace SpaceInvaders.Ui
{
    public class MenuViewPresenter : MonoBehaviour
    {
        private IMessageBroker _messageBroker;
        private List<IDisposable> _disposable = new List<IDisposable>();

        [Inject]
        private void Construct(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public void PlayGame()
        {
            _messageBroker.Publish(new Events.PlayGame());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
