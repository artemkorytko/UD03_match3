using System;
using Signals;
using Zenject;

namespace Game
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int SCORE_FOR_ELEMENTS = 10;
        private readonly SignalBus _signalBus;
        private readonly SaveSystem _saveSystem;

        private int _score = -1;

        private int Score
        {
            get => _score;
            set
            {
                if (value == _score)
                    return;
                _score = value;
                _signalBus.Fire(new OnScoreChangedSignal(_score));
            }
        }

        public GameManager(SignalBus signalBus, SaveSystem saveSystem)
        {
            _signalBus = signalBus;
            _saveSystem = saveSystem;
        }

        public void Initialize()
        {
            Score = _saveSystem.Data.Store;
            SubscribeSignals();
        }

        public void Dispose()
        {
            UnsubscribeSignals();
            _saveSystem.Data.Store = Score;
            _saveSystem.SaveData();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Subscribe<CreateGameSignal>(OnCreateGame);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Unsubscribe<CreateGameSignal>(OnCreateGame);
        }

        private void OnCreateGame()
        {
        }

        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            Score += SCORE_FOR_ELEMENTS * signal.Value;
        }
    }
}