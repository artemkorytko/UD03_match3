using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class StartButton : BaseButton
    {
        protected override void OnClick()
        {
            _signalBus.Fire<OnStartSignal>();
        }
    }
}