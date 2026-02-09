using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem Instance;

    [Header("UI")]
    [SerializeField] private AchievementToast toast;

    // IDs saved in PlayerPrefs (persistent)
    private const string FIRST_RUN = "ACH_FIRST_RUN";
    private const string SCORE_20  = "ACH_SCORE_20";
    private const string SCORE_50  = "ACH_SCORE_50";

    // Per-run flags (repeatable toasts every run)
    private bool shownFirstRunThisRun = false;
    private bool shownScore20ThisRun  = false;
    private bool shownScore50ThisRun  = false;

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

        // If you have GameManager events, reset flags at the start of a run
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += OnGameStateChanged;

            // If the scene starts already in Playing, count it as a new run:
            if (GameManager.Instance.State == GameState.Playing)
            {
                BeginRun();
            }
        }
        else
        {
            // Fallback: treat Start as run start
            BeginRun();
        }
    }

    void OnDestroy()
    {
        if (ScoreSystem.Instance != null)
        {
            ScoreSystem.Instance.OnScoreChanged.RemoveListener(OnScoreChanged);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            BeginRun();
        }
    }

    // Called at the start of every run (repeatable behavior)
    private void BeginRun()
    {
        // Reset per-run toast flags
        shownFirstRunThisRun = false;
        shownScore20ThisRun = false;
        shownScore50ThisRun = false;

        // Show First Run toast once per run
        if (!shownFirstRunThisRun)
        {
            shownFirstRunThisRun = true;
            TryUnlockOrToast(FIRST_RUN, "First Run", "Started your run");
        }
    }

    private void OnScoreChanged(int score)
    {
        // Show each toast at most once per run
        if (score >= 20 && !shownScore20ThisRun)
        {
            shownScore20ThisRun = true;
            TryUnlockOrToast(SCORE_20, "Score 20", "Reached score 20");
        }

        if (score >= 50 && !shownScore50ThisRun)
        {
            shownScore50ThisRun = true;
            TryUnlockOrToast(SCORE_50, "Score 50", "Reached score 50");
        }
    }

    // Persist unlock once, but allow repeatable toasts each run
    private void TryUnlockOrToast(string id, string title, string description)
    {
        bool alreadyUnlocked = PlayerPrefs.GetInt(id, 0) == 1;

        // Always show toast when condition is reached in this run
        if (toast != null)
        {
            toast.Show(title, description);
        }

        // Save unlock only the first time ever
        if (!alreadyUnlocked)
        {
            PlayerPrefs.SetInt(id, 1);
            PlayerPrefs.Save();
            Debug.Log($"Achievement Unlocked (saved): {title}");
        }
        else
        {
            Debug.Log($"Achievement Reached (repeatable toast): {title}");
        }
    }
}