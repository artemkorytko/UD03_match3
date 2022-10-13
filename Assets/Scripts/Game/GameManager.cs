using System;
using Signals;
using Zenject;

namespace Game
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int SCORE_FOR_ELEMENTS = 10;
        
        private readonly SignalBus _signalBus;
        
        private int _score;

        public int Score
        {
            get => _score;
            set
            {
                if (value==_score)
                    return;
                _score = value;
                _signalBus.Fire(new OnScoreChangedSignal(_score));
            }
        }

        public GameManager(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            SubscribeSignals();
        }

        public void Dispose()
        {
            UnSubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnBoardMatch>(Match);
        }

        private void UnSubscribeSignals()
        {
            _signalBus.Unsubscribe<OnBoardMatch>(Match);
        }

        private void Match(OnBoardMatch signal)
        {
            Score = SCORE_FOR_ELEMENTS * signal.Value;
        }
    }
}