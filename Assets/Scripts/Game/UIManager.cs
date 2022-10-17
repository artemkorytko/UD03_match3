namespace Game
{
    public class UIManager
    {
        private readonly ScoreCounter _scoreCounter;

        public UIManager(ScoreCounter scoreCounter)
        {
            _scoreCounter = scoreCounter;
        }

        public void Initialize(int startScore)
        {
            _scoreCounter.Initialize(startScore);
        }

        public void AddScore(int score)
        {
            _scoreCounter.AddScore(score);
        }
    }
}