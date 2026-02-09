using UnityEngine;
using System.Collections;

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance;

    [Header("UI")]
    [SerializeField] private AchievementToast toast; // optional but recommended

    // Achievement IDs
    private const string FIRST_RUN = "ACH_FIRST_RUN";
    private const string SCORE_20 = "ACH_SCORE_20";
    private const string SCORE_50 = "ACH_SCORE_50";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        // Subscribe to score changes
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnScoreChanged.AddListener(OnScoreChanged);
        }

        // Unlock First Run once, when the game begins playing
        if (GameManager.Instance != null && GameManager.Instance.State == GameState.Playing)
        {
            TryUnlock(FIRST_RUN, "First Run", "Started your first run");
        }
    }

    void OnDestroy()
    {
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnScoreChanged.RemoveListener(OnScoreChanged);
        }
    }

    private void OnScoreChanged(int score)
    {
        if (score >= 20) TryUnlock(SCORE_20, "Score 20", "Reached score 20");
        if (score >= 50) TryUnlock(SCORE_50, "Score 50", "Reached score 50");
    }

    private void TryUnlock(string id, string title, string description)
    {
        if (PlayerPrefs.GetInt(id, 0) == 1) return;

        PlayerPrefs.SetInt(id, 1);
        PlayerPrefs.Save();

        Debug.Log($"Achievement Unlocked: {title}");

        if (toast != null)
        {
            toast.Show(title, description);
        }
    }
}