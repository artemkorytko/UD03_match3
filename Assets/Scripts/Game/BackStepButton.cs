using Signals;

namespace Game
{
    public class BackStepButton : BaseButtonController
    {
        protected override void Start()
        {
            base.Start();
            _button.interactable = false;
            _signalBus.Subscribe<OnDoStep>(OnShowSelf);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _signalBus.Unsubscribe<OnDoStep>(OnShowSelf);
        }

        protected override void OnClick()
        {
            _signalBus.Fire<OnBackStepSignal>();
            _button.interactable = false;
        }

        private void OnShowSelf()
        {
            _button.interactable = true;
        }
    }
}