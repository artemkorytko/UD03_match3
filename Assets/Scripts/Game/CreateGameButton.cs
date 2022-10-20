using Signals;
using UnityEngine;

namespace Game
{
    public class CreateGameButton : BaseButton
    {
        protected override void OnClick()
        {
            _signalBus.Fire<CreateGameSignal>();
            gameObject.SetActive(false);
        }
    }
}