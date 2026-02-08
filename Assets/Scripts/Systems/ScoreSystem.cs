using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;

    public int Score { get; private set; }
    public UnityEvent<int> OnScoreChanged;

    private float timer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            AddScore(1);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
    Debug.Log("Score: " + Score);
    OnScoreChanged?.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }
}