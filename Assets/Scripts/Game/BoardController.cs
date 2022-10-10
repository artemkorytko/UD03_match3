using UnityEngine;
using Zenject;

namespace Game
{
    public class BoardController : IInitializable
    {
        private readonly BoardConfig _boardConfig;
        private readonly ElementsConfig _elementsConfig;
        private readonly Element.Factory _factory;

        private Element[,] _elements;
        private DiContainer _container;

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
            var column = _boardConfig.SizeX;
            var row = _boardConfig.SizeY;
            var elementsOffset = _boardConfig.ElementOffset;
            _elements = new Element[column, row];

            var startPosition = new Vector2(-elementsOffset * column * 0.5f + elementsOffset * 0.5f, elementsOffset * row * 0.5f - elementsOffset * 0.5f);

            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    var position = startPosition + new Vector2(elementsOffset * x, -elementsOffset * y);
                    var element = _factory.Create(_elementsConfig.Items[Random.Range(0, _elementsConfig.Items.Length)],
                        new ElementPosition(position, new Vector2(x, y)));
                    element.Initialize();
                    _elements[x, y] = element;
                }
            }
        }
    }
}