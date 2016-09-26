using UniRx;

namespace SpaceInvaders.Data.Controller
{
    public abstract class BaseController
    {
        protected readonly IMessageBroker _messageBroker;

        public BaseController(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }
    }
}
