using SpaceInvaders.Data.Model;
using UniRx;

namespace SpaceInvaders.Character.Model
{
    public class PlayerModel : BaseModel
    {
        public int Score
        {
            get { return _rxScore.Value; }
            set { _rxScore.Value = value; }
        }

        public IObservable<int> ObservableScore
        {
            get { return _rxScore.AsObservable(); }
        }

        private IntReactiveProperty _rxScore = new IntReactiveProperty();
    }
}
