using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }
    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        Application.targetFrameRate = 30;

        ChangeState(GameState.Playing); // auto-start for now
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    // Clean, reliable restart for WebGL and deadlines: reload scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ChangeState(GameState newState)
    {
        State = newState;
        OnStateChanged?.Invoke(newState);
    }
}