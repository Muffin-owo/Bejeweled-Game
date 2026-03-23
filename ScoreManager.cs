public class ScoreManager
{
    public int Score { get; private set; }

    public void ResetScore()
    {
        Score = 0;
    }

    public void AddPoints(int gemsCleared, int comboMultiplier)
    {
        Score += gemsCleared * 10 * comboMultiplier;
    }
}