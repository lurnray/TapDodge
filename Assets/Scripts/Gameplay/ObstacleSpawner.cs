using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.2f;
    public float obstacleSpeed = 4f;

    private float timer;
    private int lastDifficultyScore = 0;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        // Difficulty scaling based on score
        if (ScoreSystem.Instance != null &&
            ScoreSystem.Instance.Score >= lastDifficultyScore + 10)
        {
            lastDifficultyScore = ScoreSystem.Instance.Score;
            IncreaseDifficulty();
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("Obstacle Prefab not assigned in ObstacleSpawner.");
            return;
        }

        float x = Random.value > 0.5f ? -2f : 2f;
        GameObject obs = Instantiate(
            obstaclePrefab,
            new Vector3(x, transform.position.y, 0),
            Quaternion.identity
        );

        Obstacle obstacle = obs.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            obstacle.speed = obstacleSpeed;
        }
    }

    void IncreaseDifficulty()
    {
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.1f);
        obstacleSpeed += 0.5f;
    }
}