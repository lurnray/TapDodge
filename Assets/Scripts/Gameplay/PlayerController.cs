using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float laneOffset = 2f;

    private int currentLane = 0; // 0 = left, 1 = right

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            SwitchLane();
        }
    }

    void SwitchLane()
    {
        currentLane = 1 - currentLane;
        float xPos = currentLane == 0 ? -laneOffset : laneOffset;
        transform.position = new Vector3(xPos, transform.position.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Obstacle")) return;

        GameManager.Instance.GameOver();
    }
}