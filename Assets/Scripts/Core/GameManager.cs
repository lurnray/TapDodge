using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State { get; private set; }
    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

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

    void ChangeState(GameState newState)
    {
        State = newState;
        OnStateChanged?.Invoke(newState);
    }
}