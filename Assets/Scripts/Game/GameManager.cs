using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameManager : IInitializable, IDisposable
    {
        private const int SCORE_FOR_ELEMENTS = 10;
        private const float WAIT_TIME = 5f;
        
        private readonly SignalBus _signalBus;
        private readonly SaveDataSystem _saveDataSystem;
        private readonly BoardController _boardController;
        private readonly BoardConfig _boardConfig;
        
        private int _score =-1;
        private bool _isNeedToCancel;

        public int Score
        {
            get => _score;
            set
            {
                if (value==_score)
                    return;
                _score = value;
                _signalBus.Fire(new OnScoreChangedSignal(_score));
            }
        }

        public GameManager(SignalBus signalBus, SaveDataSystem saveDataSystem, BoardController boardController, BoardConfig boardConfig)
        {
            _signalBus = signalBus;
            _saveDataSystem = saveDataSystem;
            _boardController = boardController;
            _boardConfig = boardConfig;
        }
        
        public void Initialize()
        {
            _saveDataSystem.Initialize();
            SubscribeSignals();
            CreateBoard();
            Score = _saveDataSystem.Data.Score;
            StartTimer();
        }

        private void CreateBoard()
        {
            if (_saveDataSystem.Data.ElementKeys == null || _saveDataSystem.Data.ElementKeys.Length != _boardConfig.SizeY*_boardConfig.SizeX )
            {
                _boardController.Initialize();
            }
            else
            {
                _boardController.Initialize(_saveDataSystem.Data.ElementKeys);
            }
        }

        public void Dispose()
        {
            UnSubscribeSignals();
            SaveGame();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnBoardMatch>(Match);
            _signalBus.Subscribe<OnDoStep>(StartTimer);
        }

        private void UnSubscribeSignals()
        {
            _signalBus.Unsubscribe<OnBoardMatch>(Match);
            _signalBus.Unsubscribe<OnDoStep>(StartTimer);
        }

        private void Match(OnBoardMatch signal)
        {
            Score += SCORE_FOR_ELEMENTS * signal.Value;
        }

        private void SaveGame()
        {
            _saveDataSystem.Data.Score = Score;
            _saveDataSystem.Data.ElementKeys = _boardController.SaveElementKeys();
            _saveDataSystem.SaveData();
        }


        private async void StartTimer()
        {
            var cts = new CancellationTokenSource();
            var _cts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
            
            if (_isNeedToCancel)
                _cts.Cancel();
            
            try
            {
                Debug.Log("Timer was started");
                _isNeedToCancel = true;
                await UniTask.Delay(TimeSpan.FromSeconds(WAIT_TIME), cancellationToken: _cts.Token);
                _isNeedToCancel = false;
                _signalBus.Fire<OnElementForMatchShow>();
                Debug.Log("Show hint");
            }
            catch (Exception e)
            {
                if (!(e is OperationCanceledException))
                {
                    Debug.Log(e.Message);
                    throw;
                }
                Debug.Log("Hint was canceled");
                _cts.Dispose();
                _isNeedToCancel = false;
            }
        }
    }
}