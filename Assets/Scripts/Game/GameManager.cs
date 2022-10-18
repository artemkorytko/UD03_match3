using System;
using Signals;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameManager:IInitializable,IDisposable
    {
        private const int SCORE_FOR_ELEMENTS = 10;
        private readonly SignalBus _signalBus;
        private int _score;

        private int Score
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
            UnsubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Subscribe<OnStartSignal>(OnStart);
        }
        
        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<OnBoardMatchSignal>(OnBoardMatch);
            _signalBus.Unsubscribe<OnStartSignal>(OnStart);
        }

        private void OnStart()
        {
            UiController.FindObjectOfType<UiController>().ShowGamePanel();
        }

        private void OnBoardMatch(OnBoardMatchSignal signal)
        {
            Score = SCORE_FOR_ELEMENTS * signal.Value;
        }
    }
}