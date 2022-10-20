using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public abstract class BaseButton : BaseUiElement
    {
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

        protected abstract void OnClick();
    }
}