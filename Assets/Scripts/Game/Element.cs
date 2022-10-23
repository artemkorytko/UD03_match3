using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Signals;
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

        private const float ANIMATION_TIME = 0.5f;

        [SerializeField] private SpriteRenderer bgSpriteRender;
        [SerializeField] private SpriteRenderer iconSpriteRender;

        private Vector2 _localPosition;
        private Vector2 _gridPosition;

        private ElementConfigItem _configItem;
        private SignalBus _signalBus;
        
        private Animation _animation;
        

        public string Key => _configItem.Key;
        public Vector2 GridPosition => _gridPosition;
        public ElementConfigItem ConfigItem => _configItem;
        public bool IsActive { get; private set; }
        public bool IsInitialized { get; private set; }

        private Vector3 _startScale;

        [Inject]
        public void Construct(ElementConfigItem configItem, ElementPosition elementPosition, SignalBus signalBus)
        {
            _configItem = configItem;
            _localPosition = elementPosition.LocalPosition;
            _gridPosition = elementPosition.GridPosition;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _startScale = transform.localScale;
            _animation = GetComponent<Animation>();
            SetConfig();
            SetLocalPosition();
            Enable().Forget();
        }

        private void Start()
        {
            _signalBus.Subscribe<OnDoStep>(StopShowSelf);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnDoStep>(StopShowSelf);
        }

        public void SetConfig(ElementConfigItem config)
        {
            _configItem = config;
            iconSpriteRender.sprite = _configItem.Sprite;
        }
        
        public void SetConfig()
        {
            iconSpriteRender.sprite = _configItem.Sprite;
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

        public async UniTask Enable()
        {
            IsActive = true;
            gameObject.SetActive(true);
            IsInitialized = true;
            SetSelected(false);
            transform.localScale = Vector3.zero;
            await transform.DOScale(_startScale, ANIMATION_TIME);
        }

        public async UniTask Disable()
        {
            IsActive = false;
            await transform.DOScale(Vector3.zero, ANIMATION_TIME);
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
            _signalBus.Fire(new OnElementClickSignal(this));
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void ElementShowSelf()
        {
            _animation.Play();
        }

        public void StopShowSelf()
        {
            _animation.Stop();
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}