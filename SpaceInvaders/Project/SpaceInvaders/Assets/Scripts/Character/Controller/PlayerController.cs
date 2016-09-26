using SpaceInvaders.Character.Factory;
using SpaceInvaders.Character.Model;
using SpaceInvaders.Character.ViewPreseter;
using SpaceInvaders.Data;
using SpaceInvaders.Utilis.Input;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.Controller
{
    public class PlayerController : BaseCharacterController
    {
        public PlayerController(IMessageBroker messageBroker, 
            int id, 
            PlayerModel playerModel,
            EnemyModel enemyModel,
            Vector2 position) : base(messageBroker, id, playerModel, enemyModel)
        {
            var player = CharacterFactory.BaseCharacterFactory.Create();
            var playerSprite = DataManager.Character.LoadSprite<PlayerModel>("Player");
            player.name = id.ToString();

            player.transform.FindChild("Body").GetComponent<SpriteRenderer>().sprite = playerSprite;
            player.transform.position = position;
            player.gameObject.AddComponent<PlayerViewPresenter>();
            player.gameObject.AddComponent<BoxCollider2D>();
            player.tag = "Player";

            player.gameObject.AddComponent<ObservableHorizontalArrow>();
            player.gameObject.GetComponent<ObservableHorizontalArrow>()._leftArrow = KeyCode.LeftArrow;
            player.gameObject.GetComponent<ObservableHorizontalArrow>()._rightArrow = KeyCode.RightArrow;

            player.gameObject.AddComponent<ObservablePressed>();
            player.gameObject.GetComponent<ObservablePressed>()._pressed = KeyCode.Space;
        }
    }
}
