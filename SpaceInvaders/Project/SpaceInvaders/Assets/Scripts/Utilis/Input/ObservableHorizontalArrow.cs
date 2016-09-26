using UniRx;
using UnityEngine;

namespace SpaceInvaders.Utilis.Input
{
    public enum Direction
    {
        Left,
        Right
    }

    public class ObservableHorizontalArrow : MonoBehaviour
    {
        public KeyCode _leftArrow;
        public KeyCode _rightArrow; 

        public IObservable<Direction> HorizontalArrowClicked
        {
            get { return _rxHorizontalArrowClicked.AsObservable(); }
        }

        public IObservable<Unit> HorizontalArrowUp
        {
            get { return _rxHorizontalArrowUp.AsObservable(); }
        }

        void Update()
        {
            if (UnityEngine.Input.GetKey(_leftArrow) && _rxHorizontalArrowClicked != null) { _rxHorizontalArrowClicked.OnNext(Direction.Left); }
            if (UnityEngine.Input.GetKey(_rightArrow) && _rxHorizontalArrowClicked != null) { _rxHorizontalArrowClicked.OnNext(Direction.Right); }
            if ((UnityEngine.Input.GetKeyUp(_leftArrow) || UnityEngine.Input.GetKeyUp(_rightArrow)) && _rxHorizontalArrowClicked != null) { _rxHorizontalArrowUp.OnNext(Unit.Default); }
        }

        void OnDestroy()
        {
            if (_rxHorizontalArrowClicked != null) _rxHorizontalArrowClicked.OnCompleted();
            if (_rxHorizontalArrowUp != null) _rxHorizontalArrowUp.OnCompleted();
        }

        private Subject<Direction> _rxHorizontalArrowClicked = new Subject<Direction>();
        private Subject<Unit> _rxHorizontalArrowUp = new Subject<Unit>();
    }
}
