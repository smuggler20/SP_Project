using SpaceInvaders.Character.Model;
using SpaceInvaders.Character.ViewPreseter;
using SpaceInvaders.Data;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.Controller
{
    public class ObstacleController : BaseCharacterController
    {
        private List<IDisposable> _disposable = new List<IDisposable>();

        void OnDestroy()
        {
            foreach (var item in _disposable)
            {
                item.Dispose();
            }
        }

        public ObstacleController(IMessageBroker messageBroker,
            int id,
            ObstacleModel obstacleModel,
            Vector2 position) : base(messageBroker, obstacleModel, id)
        {
            var obstacle = CharacterFactory.BaseCharacterFactory.Create();
            var obstacleSprite = DataManager.Character.LoadSprite<ObstacleModel>("Obstacle");
            obstacle.name = id.ToString();
            var obstacleLife = obstacleModel.Life;
            obstacleLife = 5;

            obstacle.transform.FindChild("Body").GetComponent<SpriteRenderer>().sprite = obstacleSprite;
            obstacle.transform.position = position;
            obstacle.gameObject.AddComponent<ObstacleViewPresenter>();
            obstacle.gameObject.AddComponent<BoxCollider2D>();
            obstacle.tag = "Obstacle";

            obstacle.gameObject.AddComponent<Rigidbody2D>();
            obstacle.GetComponent<Rigidbody2D>().gravityScale = 0;
            obstacle.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            _disposable.Add(
                _messageBroker.Receive<Events.RocketCollision>()
                    .Subscribe(val =>
                    {
                        if (val.CollisionObjectTag == "Obstacle" && val.CollisionObjectId == id)
                        {
                            obstacleLife--;
                            if (obstacleLife == 0)
                                GameObject.Destroy(GameObject.Find(val.CollisionObjectId.ToString()));
                        }
                    })
            );
        }
    }
}
