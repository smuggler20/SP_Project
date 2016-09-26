using SpaceInvaders.Character.Controller;
using SpaceInvaders.Utilis.Input;
using System;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Events
{
    public class RocketSpawnEvent
    {
        public Vector2 spawnPosition;
        public RocketDirection rocketDirection;
        public int Id;
    }
}

namespace SpaceInvaders.Character.ViewPreseter
{
    public class PlayerViewPresenter : BaseViewPresenter
    {
        BaseViewPresenter basePresenter;

        private void Start()
        {
            var move = gameObject.GetComponent<ObservableHorizontalArrow>();
            var fire = gameObject.GetComponent<ObservablePressed>();
            var engine = gameObject.transform.FindChild("Engine");
            basePresenter = gameObject.GetComponent<BaseViewPresenter>();

            move.HorizontalArrowClicked.Subscribe(val =>
            {
                if (gameObject.transform.position.x >= _leftBlocker && gameObject.transform.position.x <= _rightBlocker)
                {
                    switch (val)
                    {
                        case Direction.Left:
                            SmoothMove(Vector3.left, 0.35f);
                            break;
                        case Direction.Right:
                            SmoothMove(Vector3.right, 0.35f);
                            break;
                    }

                    engine.gameObject.SetActive(true);
                }
            });

            move.HorizontalArrowUp.Subscribe(_ =>
            {
                engine.gameObject.SetActive(false);
            });

            fire.InputPressed.Subscribe(val =>
            {
                var position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.25f);
                basePresenter._messageBroker.Publish(new Events.RocketSpawnEvent()
                {   
                    spawnPosition = position,
                    rocketDirection = RocketDirection.ToUp,
                    Id = Convert.ToInt32(gameObject.name)
                });
            });
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
                        if (nextPosition.x < _leftBlocker)
                        {
                            nextPosition = new Vector3(_leftBlocker, gameObject.transform.position.y, gameObject.transform.position.z);
                            gameObject.transform.position = nextPosition;
                        }
                        else if (nextPosition.x > _rightBlocker)
                        {
                            nextPosition = new Vector3(_rightBlocker, gameObject.transform.position.y, gameObject.transform.position.z);
                            gameObject.transform.position = nextPosition;
                        }
                        else
                        {
                            gameObject.transform.position = nextPosition;
                        }
                    })
            );
        }
    }
}
