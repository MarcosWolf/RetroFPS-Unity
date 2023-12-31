using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;

    public DamageHud damageHud;

    public int playerTotalHP;
    public int playerCurrentHP;

    // Hud
    public TMP_Text hpText;

    //public Animator cameraAnimator;

    void Awake()
    {
        instance = this;
    }

    public void UpdateHud()
    {
        hpText.text = "+ " + playerCurrentHP + " +";
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHud();
    }

    public void HitPlayer(int playerDamageTaken)
    {
        if (GameManager.instance.isPlayerAlive == true)
        {
            playerCurrentHP -= playerDamageTaken;
            
            if (playerCurrentHP > 0)
            {
                SoundEffects.instance.sfxPlayerHit();
                damageHud.Flash();
            }

            if (playerCurrentHP <= 0)
            {
                playerCurrentHP = 0;
                damageHud.Flash();
                SoundEffects.instance.sfxPlayerDeath();
                GameManager.instance.GameOver();
            }
        }
    }

    public bool HealPlayer(int healAmount)
    {
        if (GameManager.instance.isPlayerAlive == true)
        {
            if (playerCurrentHP < playerTotalHP)
            {
                int healToAdd = Mathf.Min(healAmount, playerTotalHP - playerCurrentHP);
                playerCurrentHP += healToAdd;
                return true;
            }
            else
            {
                return false;
            }
        }
        else {
            return false;
        }
    }

    public void DeadCamera()
    {
        // Crie uma nova posição com a posição X e Y atuais e defina o eixo Z como 1.45f
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -0.06f);
    
        // Atribua a nova posição à câmera
        transform.position = newPosition;
    }

}
