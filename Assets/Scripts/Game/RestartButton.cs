using Signals;

namespace Game
{
    public class RestartButton : BaseButton
    {
        protected override void OnClick()
        {
            _signalBus.Fire<RestartSignal>();
        }
    }
}