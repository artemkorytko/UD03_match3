using System;
using Signals;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Game
{
    public class GameManager: IInitializable,IDisposable
    {
        private const int SCORE_FOR_ELEMENTS = 10;
        private readonly SignalBus _signalBus;
        private readonly SaveSystem _saveSystem;

        private int _score;

        private int Score
        {
            get => _score;
            set
            {
                if (value == _score) return;
                _score += value;
                Debug.Log(_score);
            }
        }

        public GameManager(SignalBus signalBus, SaveSystem saveSystem)
        {
            _signalBus = signalBus;
            _saveSystem = saveSystem;
        }
        
        public void Initialize()
        {
            SubscribeSignals();
            _score = _saveSystem.Data.Score;
        }
        
        public void Dispose()
        {
            UnsubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
        }
        
        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
        }

        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            Score = SCORE_FOR_ELEMENTS * signal.Value;
            _saveSystem.Data.UpdateScore(Score);
        }
    }
}