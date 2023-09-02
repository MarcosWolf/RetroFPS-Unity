using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPlayerAlive;

    public bool hasBlueKey;
    public bool hasYellowKey;
    public bool hasRedKey;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isPlayerAlive = true;

        hasBlueKey = false;
        hasYellowKey = false;
        hasRedKey = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool getKey(int KeyID)
    {
        if (KeyID == 0 && !hasBlueKey) {
            hasBlueKey = true;
            return true;
        }

        if (KeyID == 1 && !hasYellowKey) {
            hasYellowKey = true;
            return true;
        }

        if (KeyID == 2 && !hasRedKey) {
            hasRedKey = true;
            return true;
        }

        return false;
    }

    public void GameOver()
    {
        isPlayerAlive = false;
        Debug.Log("Game Over! Você morreu.");
    }
}
