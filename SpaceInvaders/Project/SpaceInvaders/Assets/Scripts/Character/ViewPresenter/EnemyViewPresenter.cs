using SpaceInvaders.Character.Controller;
using System;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Events
{
    public class EnemyCollision
    {
        public int EnemyId;
        public string CollisionObjectTag;
        public int Id;
    }

    public class EnemySpawnRocket
    {
        public int Id;
    }
}

namespace SpaceInvaders.Character.ViewPreseter
{
    public class EnemyViewPresenter : BaseViewPresenter
    {
        BaseViewPresenter basePresenter;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "MapEnd")
            {
                gameObject.GetComponent<BaseViewPresenter>()._messageBroker.Publish(new Events.EnemyCollision()
                {
                    EnemyId = Convert.ToInt32(gameObject.name),
                    CollisionObjectTag = collision.gameObject.tag,
                });
            }
            else if(collision.gameObject.tag == "Obstacle")
            {
                gameObject.GetComponent<BaseViewPresenter>()._messageBroker.Publish(new Events.EnemyCollision()
                {
                    EnemyId = Convert.ToInt32(gameObject.name),
                    CollisionObjectTag = collision.gameObject.tag,
                    Id = Convert.ToInt32(collision.gameObject.name)
                });
            }
        }

        void Start()
        {
            basePresenter = gameObject.GetComponent<BaseViewPresenter>();
            var minId = 1;

            Disposable.Add(
                basePresenter._messageBroker.Receive<Events.EnemySpawnRocket>()
                    .Subscribe(val =>
                    {
                        minId = val.Id;

                        if (minId == Convert.ToInt32(gameObject.name))
                        {
                            var position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.25f);
                            basePresenter._messageBroker.Publish(new Events.RocketSpawnEvent()
                            {
                                spawnPosition = position,
                                rocketDirection = RocketDirection.ToDown,
                                Id = Convert.ToInt32(gameObject.name)
                            });
                        }
                    })
            );
        }
    }
}
