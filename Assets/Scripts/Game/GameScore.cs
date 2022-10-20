using System;
using Signals;
using TMPro;

namespace Game
{
    public class GameScore : BaseUiElement
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _signalBus.Subscribe<OnScoreChangedSignal>(OnScoreChanged);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnScoreChangedSignal>(OnScoreChanged);
        }

        private void OnScoreChanged(OnScoreChangedSignal signal)
        {
            _text.text = signal.Value.ToString(); 
        }
    }
}