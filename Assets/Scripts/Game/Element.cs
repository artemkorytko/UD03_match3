using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Element : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<ElementConfigItem, ElementPosition, Element>
        {
        }


        [SerializeField] private SpriteRenderer bgSpriteRender;
        [SerializeField] private SpriteRenderer iconSpriteRender;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;

        private ElementConfigItem _configItem;

        public Vector2 GridPosition => _gridPosition;
        public ElementConfigItem ConfigItem => _configItem;
        public bool IsActive { get; private set; }
        public bool IsInitialized { get; private set; }

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition)
        {
            _configItem = configItem;
            _localPosition = elementPosition.LocalPosition;
            _gridPosition = elementPosition.GridPosition;
        }

        public void Initialize()
        {
            SetConfig();
            SetLocalPosition();
            Enable();
        }

        public void SetConfig()
        {
            iconSpriteRender.sprite = _configItem.Sprite;
        }

        public void SetLocalPosition()
        {
            transform.localPosition = _localPosition;
        }

        public void Enable()
        {
            IsActive = true;
            //Dotween scale 0-1
            gameObject.SetActive(true);
            IsInitialized = true;
            SetSelected(false);
        }

        public void Disable()
        {
            IsActive = false;
            //Dotween scale 1-0
            gameObject.SetActive(false);
        }

        public void SetSelected(bool isOn)
        {
            bgSpriteRender.enabled = isOn;
        }
        
        private void OnMouseUpAsButton()
        {
            OnClick();
        }

        private void OnClick()
        {
            
        }
    }
}