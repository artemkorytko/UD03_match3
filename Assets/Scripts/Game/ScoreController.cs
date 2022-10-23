using System;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ScoreController : MonoBehaviour
    {
        private SignalBus _signalBus;
        private TextMeshProUGUI _text;

        [Inject]
        public void Construct(SignalBus signal)
        {
            _signalBus = signal;
        }
        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            
        }

        private void Start()
        {
            _signalBus.Subscribe<OnScoreChangedSignal>(UpdateScore);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnScoreChangedSignal>(UpdateScore);
        }

        private void UpdateScore(OnScoreChangedSignal signal)
        {
            _text.text = signal.Value.ToString();
        }
    }
}