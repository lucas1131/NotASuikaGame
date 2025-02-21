public class ScoreController : IScoreController
{
    private IScoreView scoreView;
    private int score;
    
    public ScoreController(IScoreView scoreView)
    {
        this.scoreView = scoreView;
        this.scoreView.SetScore(0);
        this.score = 0;
        ShowScore();
    }

    public void IncrementScore(int amount)
    {
        score += amount;
        scoreView.SetScore(score);
    }

    public void ShowScore()
    {
        scoreView.SetActive(true);
    }
    
    public void HideScore()
    {
        scoreView.SetActive(false);
    }
}

public interface IScoreController
{
    void IncrementScore(int amount);
    void ShowScore();
    void HideScore();
}
