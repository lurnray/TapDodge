using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnScoreChanged.AddListener(UpdateScore);
            UpdateScore(ScoreSystem.Instance.Score);
        }
    }

    void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    void OnDestroy()
    {
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnScoreChanged.RemoveListener(UpdateScore);
        }
    }
}