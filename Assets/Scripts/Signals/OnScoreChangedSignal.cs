namespace Signals
{
    public class OnScoreChangedSignal
    {
        public readonly int Value;

        public OnScoreChangedSignal(int value)
        {
            Value = value;
        }
    }
}