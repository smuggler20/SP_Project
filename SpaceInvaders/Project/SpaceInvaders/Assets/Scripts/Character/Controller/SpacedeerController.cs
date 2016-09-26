using SpaceInvaders.Character.Model;
using SpaceInvaders.Character.ViewPreseter;
using SpaceInvaders.Data;
using UniRx;
using UnityEngine;

namespace SpaceInvaders.Character.Controller
{
    public enum SpacedeerDirection
    {
        ToLeft,
        ToRight
    }

    public class SpacedeerController : BaseCharacterController
    {
        public SpacedeerController(IMessageBroker messageBroker, 
            int id, 
            Vector2 position,
            SpacedeerDirection direction) : base(messageBroker, id)
        {
            var spacedeer = CharacterFactory.BaseCharacterFactory.Create();
            var spacedeerSprite = DataManager.Character.LoadSprite<SpacedeerModel>("Spacedeer");
            spacedeer.name = id.ToString();

            spacedeer.transform.FindChild("Body").GetComponent<SpriteRenderer>().sprite = spacedeerSprite;
            spacedeer.transform.position = position;
            spacedeer.gameObject.AddComponent<SpacedeerViewPresenter>();
            spacedeer.gameObject.AddComponent<BoxCollider2D>();
            spacedeer.tag = "Enemy";
        }
    }
}
