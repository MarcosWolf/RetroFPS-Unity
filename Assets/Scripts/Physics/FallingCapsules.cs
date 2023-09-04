using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCapsules : MonoBehaviour
{
    /*
    public float fallSpeed = 2.0f;
    public float arcHeight = 50.0f;
    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private float timeElapsed = 0f;
    */
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
        /*
        timeElapsed += Time.deltaTime;

        float x = initialPosition.x;
        float y = initialPosition.y - fallSpeed * timeElapsed + (arcHeight / 100 ) * Mathf.Pow(timeElapsed, 2);

        rectTransform.anchoredPosition = new Vector2(x, y);
        */
        if (rectTransform.anchoredPosition.y < -Screen.height / 2)
        {
            Destroy(gameObject);
        }
        
        velocity.y += gravity * Time.deltaTime;
        rectTransform.anchoredPosition += velocity * Time.deltaTime;
    }
}
