using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Obstacle : MonoBehaviour
{
    public float speed = 4f;

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
            Destroy(gameObject);
    }
}