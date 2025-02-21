public interface IScoreController
{
    void SetScore(int score);
    void IncrementScore(int amount);
    void ShowScore();
    void HideScore();
    int Score { get; }
}