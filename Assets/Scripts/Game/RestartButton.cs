using Signals;

namespace Game
{
    public class RestartButton : BaseButtonController
    {
        protected override void OnClick()
        {
            _signalBus.Fire<RestartSignal>();
        }
    }
}