using Game;

namespace Signals
{
    public class OnElementClickSignal
    {
        public readonly Element Element;
        
        public OnElementClickSignal(Element element)
        {
            Element = element;
        }
        
    }
}