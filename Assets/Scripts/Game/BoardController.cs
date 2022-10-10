using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace Game
{
    public class BoardController : IInitializable
    {
        private readonly BoardConfig _boardConfig;
        private readonly ElementsConfig _elementsConfig;

        private Element[,] _elements;
        private DiContainer _container;
        private Element.Factory _factory;

        public BoardController(BoardConfig boardConfig, ElementsConfig elementsConfig, Element.Factory factory)
        {
            _boardConfig = boardConfig;
            _elementsConfig = elementsConfig;
            _factory = factory;
        }
        
        public void Initialize()
        {
            GenerateElements();
        }

        private void GenerateElements()
        {
            var colums = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var elementsOffset = _boardConfig.ElementOffset;
            _elements = new Element[colums, row];

            var startPosition = new Vector2(-elementsOffset * colums * 0.5f + elementsOffset * 0.5f,
                elementsOffset * row * 0.5f - elementsOffset * 0.5f);

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < colums; x++)
                {
                    var position = startPosition + new Vector2(elementsOffset * x, -elementsOffset * y);
                    var element = _factory.Create(_elementsConfig.Items[UnityEngine.Random.Range(0, _elementsConfig.Items.Length)],
                        new ElementPosition(position, new Vector2(x, y)));
                    element.Initialize();
                }
            }
        }
    }
}