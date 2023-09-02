using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;

    public int playerTotalHP;
    public int playerCurrentHP;

    //public Animator cameraAnimator;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HitPlayer(int playerDamageTaken)
    {
        if (GameManager.instance.isPlayerAlive == true)
        {
            Debug.Log("Oi");
            playerCurrentHP -= playerDamageTaken;

            if (playerCurrentHP <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
    }

}
