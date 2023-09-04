using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    public float initialHorizontalSpeed;
    public float gravity;
    private RectTransform rectTransform;
    private Vector2 initialPosition;

    private Vector2 velocity;
    private float elapsedTime;
    private bool isRising = true;

    // O tempo que o objeto vai subir antes de começar a cair.
    public float timeToRise = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;

        velocity = new Vector2(initialHorizontalSpeed, 0f);
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRising)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToRise)
            {
                isRising = false;
                velocity.y = 0;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
                rectTransform.anchoredPosition += velocity * Time.deltaTime;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime; // Usamos "-" para aplicar a gravidade na direção descendente.
            rectTransform.anchoredPosition += velocity * Time.deltaTime;
        }
    }
}