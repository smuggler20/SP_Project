using UnityEngine;
using UniRx;

namespace SpaceInvaders.Utilis.Input
{
    public class ObservablePressed : MonoBehaviour
    {
        public KeyCode _pressed;

        public IObservable<KeyCode> InputPressed
        {
            get { return _rxInputPressed.AsObservable(); }
        }

        void Update()
        {
            if(UnityEngine.Input.GetKeyDown(_pressed) && _rxInputPressed != null) { _rxInputPressed.OnNext(_pressed); }
        }

        void OnDestroy()
        {
            if (_rxInputPressed != null) _rxInputPressed.OnCompleted();
        }

        private Subject<KeyCode> _rxInputPressed = new Subject<KeyCode>();
    }
}
