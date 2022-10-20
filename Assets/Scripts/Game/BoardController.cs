using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Signals;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class BoardController :IInitializable,IDisposable
    {
        private readonly BoardConfig _boardConfig;
        private readonly ElementsConfig _elementsConfig;
        private readonly Element.Factory _factory;
        private readonly SignalBus _signalBus;
        private readonly SaveSystem _saveSystem;

        private Element[,] _elements;
        private DiContainer _container;

        private Element _firstSelected;

        private bool _isBlocked;

        public BoardController(BoardConfig boardConfig, ElementsConfig elementsConfig,Element.Factory factory,SignalBus signalBus,SaveSystem saveSystem)
        {
            _boardConfig = boardConfig;
            _elementsConfig = elementsConfig;
            _factory = factory;
            _signalBus = signalBus;
            _saveSystem = saveSystem;
        } 
        public void Initialize()
        {
            SubscribeSignals();
        }
        public void Dispose()
        {
            UnsubscribeSignals();
            //_saveSystem.Data.BoardState = GetBoardState();
            //_saveSystem.SaveData(); 
        }

        private string[] GetBoardState()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var list = new List<string>();
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                   list.Add(_elements[x,y].ConfigItem.Key);
                }
            }

            return list.ToArray();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<OnElementClickSignal>(OnElementClick);
            _signalBus.Subscribe<OnRestartSignal>(OnRestart);
            //_signalBus.Subscribe<OnMenuSignal>(OnMenu);
            _signalBus.Subscribe<CreateGameSignal>(OnCreateGame);
        }


        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<OnElementClickSignal>(OnElementClick);
            _signalBus.Unsubscribe<OnRestartSignal>(OnRestart);
            //_signalBus.Unsubscribe<OnMenuSignal>(OnMenu);
            _signalBus.Unsubscribe<CreateGameSignal>(OnCreateGame);
        }

        private void OnCreateGame()
        {
            if (_saveSystem.Data.BoardState==null)
            {
                GenerateElements();
            }
            else
            {
                GenerateElementsWithData(_saveSystem.Data.BoardState);
            }
        }

        private void GenerateElementsWithData(string[] dataBoardState)
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var elementsoffset = _boardConfig.ElementOffset;
            _elements = new Element[column,row];

            var counter = 0;

            var startPosition = new Vector2(-elementsoffset * column * 0.5f + elementsoffset * 0.5f,
                elementsoffset * row * 0.5f - elementsoffset * 0.5f);
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    var position = startPosition + new Vector2(elementsoffset * x, -elementsoffset * y);
                    var element = _factory.Create(_elementsConfig.GetByKey(dataBoardState[counter++]),
                        new ElementPosition(position,new Vector2(x,y)));
                    element.Initialize();
                    _elements[x, y] = element;
                }
            }
        }

        // private void OnMenu()
        // {
        //     UiController.FindObjectOfType<UiController>().ShowMenuPanel();
        // }

        private async void OnRestart()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    _elements[x, y].DestroySelf();
                }
            }

            _elements = null;
            await UniTask.Yield();
            GenerateElements();
            
            // var tasks = new List<UniTask>();
            // foreach (var Element in _elements)
            // {
            //     tasks.Add(Element.DestroyElement());
            // }
            // await UniTask.WhenAll(tasks);
            //
            // GenerateElements();
        }

        private void OnElementClick(OnElementClickSignal signal)
        {
            if(_isBlocked)
                return;
            
            var element = signal.Element;
            if (_firstSelected == null)
            {
                _firstSelected = element;
                _firstSelected.SetSelected(true);
            }
            else
            {
                if (IsCanSwipe(_firstSelected,element))
                {
                    _firstSelected.SetSelected(false);
                    Swap(_firstSelected, element);
                    _firstSelected = null;
                    CheckBoard();
                }
                else
                {
                    if (_firstSelected==element)
                    {
                        _firstSelected.SetSelected(false);
                        _firstSelected = null;
                    }
                    else
                    {
                        _firstSelected.SetSelected(false);
                        _firstSelected = element;
                        _firstSelected.SetSelected(true);
                    }
                }
            }
        }

        private async void CheckBoard()
        {
            _isBlocked = true;
            bool isNeedReCheck;
            List<Element> elementsForCollecting = new List<Element>();

            do
            {
                isNeedReCheck = false;
                elementsForCollecting.Clear();
                elementsForCollecting = SearchLines();
                if (elementsForCollecting.Count > 0)
                {
                    await DisableElements(elementsForCollecting);
                    _signalBus.Fire(new OnBoardMatchSignal(elementsForCollecting.Count));
                    await NormalizeBoard();
                    isNeedReCheck = true;
                }
            } while (isNeedReCheck);
            _isBlocked = false; 
        }

        private List<Element> SearchLines()
        {
            List<Element> elementsForCollecting = new List<Element>();

            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    if (_elements[x,y].IsActive && !elementsForCollecting.Contains(_elements[x,y]))
                    {
                        List<Element> checkResult;
                        bool needAddFirst = false;
                        checkResult = CheckHorizontal(x,y);
                        if (checkResult !=null && checkResult.Count >=2)
                        {
                            needAddFirst = true;
                            elementsForCollecting.AddRange(checkResult);
                        }

                        checkResult = CheckVertical(x, y);
                        if (checkResult!=null && checkResult.Count>=2)
                        {
                            needAddFirst = true;
                            elementsForCollecting.AddRange(checkResult);
                        }

                        if (needAddFirst)
                        {
                            elementsForCollecting.Add(_elements[x,y]);
                        }
                    }
                }
            }

            return elementsForCollecting;
        }

        private List<Element> CheckHorizontal(int x, int y)
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;

            int nextColumn = x + 1;
            int nextRow = y;

            if (nextColumn >= column)
                return null;

            List<Element> elementsInLine = new List<Element>();
            var element = _elements[x, y];

            while (_elements[nextColumn,nextRow].IsActive && element.ConfigItem.Key==_elements[nextColumn,nextRow].ConfigItem.Key)
            {
                elementsInLine.Add(_elements[nextColumn,nextRow]);
                if (nextColumn+1 < column)
                {
                    nextColumn++;
                }
                else
                {
                    break;
                }
            }

            return elementsInLine;
        }

        private List<Element> CheckVertical(int x, int y)
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;

            int nextColumn = x;
            int nextRow = y + 1;

            if (nextRow >= row)
                return null;

            List<Element> elementsInLine = new List<Element>();
            var element = _elements[x, y];

            while (_elements[nextColumn,nextRow].IsActive && element.ConfigItem.Key==_elements[nextColumn,nextRow].ConfigItem.Key)
            {
                elementsInLine.Add(_elements[nextColumn,nextRow]);
                if (nextRow+1 < row)
                {
                    nextRow++;
                }
                else
                {
                    break;
                }
            }

            return elementsInLine;
        }

        private async UniTask DisableElements(List<Element> elementsForCollecting)
        {
            var tasks = new List<UniTask>();
            foreach (var element in elementsForCollecting)
            {
                tasks.Add(element.Disable());
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask NormalizeBoard()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            for (int x = column - 1; x >= 0; x--)
            {
                List<Element> freeElements = new List<Element>();
                for (int y = row - 1; y >= 0; y--)
                {
                    while (y>=0 && !_elements[x,y].IsActive)
                    {
                        freeElements.Add(_elements[x,y]);
                        y--;
                    }

                    if (y>=0 && freeElements.Count >0)
                    {
                        Swap(_elements[x,y],freeElements[0]);
                        freeElements.Add(freeElements[0]);
                        freeElements.RemoveAt(0);  
                    }
                }
            }

            var tasks = new List<UniTask>();
            for (int y = row - 1; y >= 0; y--)
            {
                for (int x = column - 1; x >= 0; x--)
                {
                    if (!_elements[x,y].IsActive)
                    {
                        GenerateRandomElement(_elements[x,y],column,row);
                        tasks.Add(_elements[x,y].Enable());
                    }
                }
            }

            await UniTask.WhenAll(tasks);
        }

        private void GenerateRandomElement(Element element, int column, int row)
        {
            Vector2 gridPosition = element.GridPosition;
            var elements = GetPossibleElement((int)gridPosition.x, (int)gridPosition.y, column, row);
            element.SetConfig(elements);
        }
        
        private bool IsCanSwipe(Element first,Element second)
        {
            var pos1 = first.GridPosition;
            var pos2 = second.GridPosition;

            Vector2 comparePosition = pos1;
            comparePosition.x += 1;
            if (comparePosition==pos2)
            {
                return true;
            }
            comparePosition = pos1;
            comparePosition.x -= 1;
            if (comparePosition==pos2)
            {
                return true;
            }
            comparePosition = pos1;
            comparePosition.y += 1;
            if (comparePosition==pos2)
            {
                return true;
            }
            comparePosition = pos1;
            comparePosition.y -= 1;
            if (comparePosition==pos2)
            {
                return true;
            }
            return false;
        }

        private void Swap(Element first,Element second)
        {
            _elements[(int)first.GridPosition.x, (int)first.GridPosition.y] = second;
            _elements[(int)second.GridPosition.x, (int)second.GridPosition.y] = first;

            Vector2 position = second.transform.localPosition;
            Vector2 gridPosition = second.GridPosition;
            
            second.SetLocalPosition(first.transform.localPosition,first.GridPosition);
            first.SetLocalPosition(position,gridPosition);
        }

        private void  GenerateElements()
        {
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var elementsoffset = _boardConfig.ElementOffset;
            _elements = new Element[column,row];

            var startPosition = new Vector2(-elementsoffset * column * 0.5f + elementsoffset * 0.5f,
                elementsoffset * row * 0.5f - elementsoffset * 0.5f);

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    var position = startPosition + new Vector2(elementsoffset * x, -elementsoffset * y);
                    var element = _factory.Create(GetPossibleElement(x,y,column,row),
                        new ElementPosition(position,new Vector2(x,y)));
                    element.Initialize();
                    _elements[x, y] = element;
                }
            }
        }

        private ElementConfigItem GetPossibleElement(int column,int row, int columnCount,int rowCount)
        {
            var tempList = new List<ElementConfigItem>(_elementsConfig.Items);
            int x = column;
            int y = row - 1;
            if (x >=0 && x<columnCount && y>=0 && y<rowCount)
            {
                if (_elements[x,y].IsInitialized)
                {
                    tempList.Remove(_elements[x,y].ConfigItem);
                }
            }

            x = column - 1;
            y = row;
            
            if (x >=0 && x<columnCount && y>=0 && y<rowCount)
            {
                if (_elements[x,y].IsInitialized)
                {
                    tempList.Remove(_elements[x,y].ConfigItem);
                }
            }

            return tempList[Random.Range(0, tempList.Count)];    

        }
    }
}