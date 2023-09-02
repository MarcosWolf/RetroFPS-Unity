using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isPlayerAlive;
    
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        isPlayerAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GameOver()
    {
        isPlayerAlive = false;
        Debug.Log("Game Over! Você morreu.");
    }
}
