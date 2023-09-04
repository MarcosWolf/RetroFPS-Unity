using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCapsules : MonoBehaviour
{
    public float initialHorizontalSpeed = 2.0f;
    public float gravity = -9.8f;
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private Vector2 velocity;

    void Start()
    {
        
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;

        velocity = new Vector2(initialHorizontalSpeed, 5f);
    }

    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        rectTransform.anchoredPosition += velocity * Time.deltaTime;
    }
}
