using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public abstract class BaseButtonController : MonoBehaviour
    {
        protected SignalBus _signalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected Button _button;
        
        private  void Awake()
        {
            _button = GetComponent<Button>();
        }

        protected virtual void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
        
        protected abstract void OnClick();
    }
}