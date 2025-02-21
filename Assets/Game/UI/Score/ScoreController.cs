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

    public int Score => score;

    public void SetScore(int score)
    {
        this.score = score;
        scoreView.SetScore(score);
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