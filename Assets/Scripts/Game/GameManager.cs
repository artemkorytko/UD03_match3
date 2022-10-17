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
        private readonly UIManager _uiManager;

        private int _score;

        private int Score
        {
            get => _score;
            set
            {
                if (value == _score) return;
                _score += value;
                _uiManager.AddScore(value);
            }
        }

        public GameManager(SignalBus signalBus, SaveSystem saveSystem,UIManager uiManager)
        {
            _signalBus = signalBus;
            _saveSystem = saveSystem;
            _uiManager = uiManager;
        }
        
        public void Initialize()
        {
            SubscribeSignals();
            _score = _saveSystem.Data.Score;
            _uiManager.Initialize(_score);
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