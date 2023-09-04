using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject fallingObjectPrefab;
    public Transform spawnPoint;

    void Awake()
    {
        instance = this;
    }

    public void SpawnFallingObject()
    {
        GameObject newObject = Instantiate(fallingObjectPrefab, spawnPoint.position, Quaternion.identity);
        newObject.transform.SetParent(transform);
    }
    
}
