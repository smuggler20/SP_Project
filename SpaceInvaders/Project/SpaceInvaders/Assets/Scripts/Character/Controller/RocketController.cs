using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.Controller
{
    public enum RocketDirection
    {
        ToUp,
        ToDown
    }

    public class RocketController : BaseCharacterController
    {
        public RocketController(IMessageBroker messageBroker, 
            int id, 
            Vector2 position,
            RocketDirection direction) : base(messageBroker, id)
        {
            var rocket = CharacterFactory.RocketFactory.Create();
            rocket.transform.position = position;
            switch(direction)
            {
                case RocketDirection.ToUp:
                    rocket.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                case RocketDirection.ToDown:
                    rocket.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
                    break;
            }
            rocket.name = id.ToString();
            rocket.tag = "Rocket";
        }
    }
}
