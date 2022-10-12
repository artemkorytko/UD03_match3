using System;
using Signals;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>
        {
        }


        [SerializeField] private SpriteRenderer bgSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;

        private ElementConfigItem _configItem;
        private SignalBus _signalBus;

        public Vector2 GridPosition => _gridPosition;
        public ElementConfigItem ConfigItem => _configItem;
        public bool IsActive { get; private set; }
        public bool IsInitialized { get; private set; }

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition,
            SignalBus signalBus)
        {
            _configItem = configItem;
            _localPosition = elementPosition.LocalPosition;
            _gridPosition = elementPosition.GridPosition;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            SetConfig();
            SetLocalPosition();
            Enable();
        }

        public void SetConfig(ElementConfigItem configItem)
        {
            _configItem = configItem;
            iconSpriteRenderer.sprite = _configItem.Sprite;
        }
        
        public void SetConfig()
        {
            iconSpriteRenderer.sprite = _configItem.Sprite;
        }

        public void SetLocalPosition()
        {
            transform.localPosition = _localPosition;
        }
        
        public void SetLocalPosition(Vector2 localPosition, Vector2 gridPosition)
        {
            transform.localPosition = localPosition;
            _gridPosition = gridPosition;
        }

        public void Enable()
        {
            IsActive = true;
            gameObject.SetActive(true);
            IsInitialized = true;
            SetSelected(false);
        }

        public void Disable()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRenderer.enabled = isOn;
        }

        private void OnMouseUpAsButton()
        {
            OnClick();
        }

        private void OnClick()
        {
            _signalBus.Fire(new OnElementClickSignal(this));
        }
    }
}