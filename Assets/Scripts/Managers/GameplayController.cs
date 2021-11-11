using Assets.UnityFoundation.Code.Common;
using System;

namespace Assets.Scripts
{
    public abstract class GameplayController : Singleton<GameplayController>
    {
        protected int score;

        public int Score => score;

        public event Action<int> OnIncreaseLevel;

        protected void InvokeOnIncreaseLevel() => OnIncreaseLevel?.Invoke(Score);

        public abstract void FlapTheBird();

        public abstract void SetScore();

        public abstract void PlayerDiedShowScore(BirdScript bird);

        public abstract void StartGame();
    }
}