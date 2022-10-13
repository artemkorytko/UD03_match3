using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ScoreController : MonoBehaviour
    {
        private GameManager _gameManager;
        private TextMeshProUGUI _text;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        private void Update()
        {
            _text.text = _gameManager.Score.ToString();
        }
    }
}