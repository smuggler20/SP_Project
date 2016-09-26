using SpaceInvaders.Character.Controller;
using SpaceInvaders.Utilis;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Events
{
    public class RocketCollision
    {
        public int RocketId;
        public int CollisionObjectId;
        public string CollisionObjectTag;
        public RocketDirection Direction;
        public Vector2 Position;
    }
}

namespace SpaceInvaders.Character.ViewPreseter
{
    public class RocketViewPresenter : BaseDisposableMonoBehaviour
    {
        float rocketSpeed = 1.25f;
        float rocketDirection = 0.05f;

        private IMessageBroker _messageBroker;

        [Inject]
        private void Construct(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        private void Start()
        {
            var eulerAnglesZ = gameObject.transform.eulerAngles.z;
            var directionVector3 = Vector3.zero;
            if (eulerAnglesZ != 0.0f)
            {
                rocketDirection = -0.05f;
                directionVector3 = Vector3.down;
            }
            else
            {
                directionVector3 = Vector3.up;
            }

            Disposable.Add(
                Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    SmoothMove(directionVector3, rocketSpeed);
                    rocketSpeed += rocketDirection;
                }) 
            );
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            RocketDirection direction;

            if (rocketDirection > 0) direction = RocketDirection.ToUp;
            else direction = RocketDirection.ToDown;

            switch(direction)
            {
                case RocketDirection.ToUp:
                    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Obstacle")
                    {
                        _messageBroker.Publish(new Events.RocketCollision()
                        {
                            RocketId = Convert.ToInt32(gameObject.name),
                            CollisionObjectId = Convert.ToInt32(collision.gameObject.name),
                            CollisionObjectTag = collision.gameObject.tag,
                            Direction = direction,
                            Position = gameObject.transform.position
                        });
                    }
                    break;
                case RocketDirection.ToDown:
                    if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Obstacle")
                    {
                        _messageBroker.Publish(new Events.RocketCollision()
                        {
                            RocketId = Convert.ToInt32(gameObject.name),
                            CollisionObjectId = Convert.ToInt32(collision.gameObject.name),
                            CollisionObjectTag = collision.gameObject.tag,
                            Direction = direction,
                            Position = gameObject.transform.position
                        });
                    }
                    break;
            }
        }

        private void SmoothMove(Vector3 vector3, float speed)
        {
            var startTime = Time.time;
            Disposable.Add(
                Observable.EveryUpdate()
                    .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(150)))
                    .Subscribe(c =>
                    {
                        var nextPosition = Vector3.Lerp(gameObject.transform.position, gameObject.transform.position += vector3, (Time.time - startTime) * speed / 5);
                        if (nextPosition.y > 10.0f || nextPosition.y < -10.0f)
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            gameObject.transform.position = nextPosition;
                        }
                    })
            );
        }

        public class Factory : Factory<RocketViewPresenter>
        {

        }
    }
}
