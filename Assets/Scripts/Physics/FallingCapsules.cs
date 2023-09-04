using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCapsules : MonoBehaviour
{
    public float initialHorizontalSpeed = 6000;
    public float gravity = -8000;
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
