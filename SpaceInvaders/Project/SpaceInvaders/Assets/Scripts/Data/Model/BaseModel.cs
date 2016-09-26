using UniRx;

namespace SpaceInvaders.Data.Model
{
    public enum CharacterType
    {
        Obstacle,
        Empty,
        Player,
        EnemyStandard,
        EnemyBoss,
        Rocket,
        Spacedeer
    }

    public class BaseModel
    {
        public CharacterType CharacterType;

        public int Life
        {
            get { return _rxLife.Value; }
            set { _rxLife.Value = value; }
        }

        public IObservable<int> ObservableLife
        {
            get { return _rxLife.AsObservable(); }
        } 

        private IntReactiveProperty _rxLife = new IntReactiveProperty();
    }
}
