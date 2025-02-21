using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour, IScoreView
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void SetActive(bool active)
    {
        scoreText.gameObject.SetActive(active);
    }
    
    public void SetScore(int score)
    {
        scoreText.text = $"Score: {score.ToString()}";
    }
}
