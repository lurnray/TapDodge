using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnInterval = 1.2f;
    public float obstacleSpeed = 4f;

    private float timer;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
{
    if (obstaclePrefab == null)
    {
        Debug.LogError("Obstacle Prefab not assigned!");
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
        obstacle.speed = obstacleSpeed;
}
}