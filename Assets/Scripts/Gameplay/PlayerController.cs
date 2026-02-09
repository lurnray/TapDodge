using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float laneOffset = 2f;
    private int currentLane = 0; // 0 = left, 1 = right

    [Header("Shield Feature")]
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private float shieldCooldown = 20f;

    private bool isShieldActive = false;
    private float shieldTimer = 0f;
    private float shieldCooldownTimer = 0f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

        // Lane switching on tap / click
        if (Input.GetMouseButtonDown(0))
        {
            SwitchLane();
        }

        HandleShield();
    }

    void SwitchLane()
    {
        currentLane = 1 - currentLane;
        float xPos = currentLane == 0 ? -laneOffset : laneOffset;
        transform.position = new Vector3(xPos, transform.position.y, 0f);
    }

    void HandleShield()
    {
        // Cooldown timer
        shieldCooldownTimer += Time.deltaTime;

        // Activate shield when cooldown completes
        if (!isShieldActive && shieldCooldownTimer >= shieldCooldown)
        {
            ActivateShield();
        }

        // Shield active duration
        if (isShieldActive)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= shieldDuration)
            {
                DeactivateShield();
            }
        }
    }

    void ActivateShield()
    {
        isShieldActive = true;
        shieldTimer = 0f;
        shieldCooldownTimer = 0f;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.cyan;
    }

    void DeactivateShield()
    {
        isShieldActive = false;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Obstacle"))
            return;

        if (isShieldActive)
        {
            Destroy(other.gameObject);
            return;
        }

        GameManager.Instance.GameOver();
    }
}