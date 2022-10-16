using System;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class RestartButton : MonoBehaviour
    {
        private SignalBus _signalBus;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            _signalBus.Fire<OnRestartSignal>();
        }
    }
}