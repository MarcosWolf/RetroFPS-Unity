using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Adicione esta linha

public class DamageHud : MonoBehaviour
{
    private Image panelImage;
    private Color originalColor;
    public bool isFlashing = false;

    public float flashDuration = 0.1f;

    // Start is called before the first frame update
    private void Start()
    {
        panelImage = GetComponent<Image>();
        originalColor = panelImage.color;
    }

    public void Flash()
    {
            StartCoroutine(FlashEffect());

    }

    private IEnumerator FlashEffect()
    {
        //isFlashing = true;

        // Define a cor inicial como vermelho opaco
        panelImage.color = new Color(1f, 0f, 0f, 1f);

        yield return new WaitForSeconds(flashDuration);

        // Transição de volta para a cor inicial (totalmente transparente)
        while (panelImage.color.a > 0)
        {
            panelImage.color = Color.Lerp(panelImage.color, originalColor, Time.deltaTime * 5); // Ajuste a velocidade do fade conforme necessário
            yield return null;
        }

        // Garante que a cor final seja exatamente a cor inicial (totalmente transparente)
        panelImage.color = originalColor;
        //isFlashing = false; // Redefine a flag para permitir futuros flashes
    }
}
