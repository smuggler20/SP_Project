using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.ViewPreseter
{
    public class SpacedeerViewPresenter : BaseViewPresenter
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Rocekt")
            {
                gameObject.GetComponent<BaseViewPresenter>()._messageBroker.Publish(new Events.EnemyCollision()
                {
                    EnemyId = Convert.ToInt32(gameObject.name),
                    CollisionObjectTag = collision.gameObject.tag,
                });
            }
        }

        void Update()
        {
            SmoothMove(Vector3.right, 0.2f);
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
                        if(nextPosition.x < 15.0f)
                        {
                            gameObject.transform.position = nextPosition;
                        }
                    })
            );
        }
    }
}
