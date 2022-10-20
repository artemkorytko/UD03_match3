using Signals;

namespace Game
{
    public class MenuButton : BaseButton
    {
        protected override void OnClick()
        {
            _signalBus.Fire<OnMenuSignal>();
        }
    }
}