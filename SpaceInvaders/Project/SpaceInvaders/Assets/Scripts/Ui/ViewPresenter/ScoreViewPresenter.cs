using UnityEngine;
using SpaceInvaders.Character.Model;
using Zenject;
using UniRx;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpaceInvaders.Ui
{
    public class ScoreViewPresenter : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> score;

        private PlayerModel _playerModel;

        [Inject]
        private void Construct(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        void Start()
        {
            _playerModel.ObservableScore.Subscribe(val => score.ForEach(c => c.GetComponent<Text>().text = val.ToString()));
        }
    }
}
