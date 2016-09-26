using SpaceInvaders.Character.Model;
using SpaceInvaders.Character.ViewPreseter;
using SpaceInvaders.Data;
using SpaceInvaders.Utilis;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.Controller
{
    public class EnemyController : BaseCharacterController
    {
        private List<IDisposable> _disposable = new List<IDisposable>();

        void OnDestroy()
        {
            foreach (var item in _disposable)
            {
                item.Dispose();
            }
        }

        public EnemyController(IMessageBroker messageBroker, 
            int id, 
            PlayerModel playerModel,
            EnemyModel enemyModel,
            Vector2 position,
            string characterName) : base(messageBroker, id, playerModel, enemyModel)
        {
            var enemy = CharacterFactory.BaseCharacterFactory.Create();
            var enemySprite = DataManager.Character.LoadSprite<EnemyModel>(characterName);
            enemy.name = id.ToString();

            enemy.transform.FindChild("Body").GetComponent<SpriteRenderer>().sprite = enemySprite;
            enemy.transform.position = position;
            enemy.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
            enemy.transform.SetParent(GameObject.Find("Enemys").transform);
            enemy.gameObject.AddComponent<EnemyViewPresenter>();
            enemy.gameObject.AddComponent<BoxCollider2D>();
            enemy.tag = "Enemy";

            _disposable.Add(
                _messageBroker.Receive<Events.RocketCollision>()
                    .Subscribe(val =>
                    {
                        if (val.CollisionObjectTag == "Enemy")
                        {
                            GameObject.Destroy(GameObject.Find(val.CollisionObjectId.ToString()));
                        }
                    })
            );
        }
    }
}
