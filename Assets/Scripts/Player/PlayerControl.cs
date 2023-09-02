using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;

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
            Debug.Log("Oi");
            playerCurrentHP -= playerDamageTaken;

            if (playerCurrentHP <= 0)
            {
                playerCurrentHP = 0;
                GameManager.instance.GameOver();
            }
        }
    }

}
