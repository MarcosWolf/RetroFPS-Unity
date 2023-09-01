using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speedProjectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemyProjectile();
    }

    private void MoveEnemyProjectile()
    {
        transform.Translate(Vector3.forward * speedProjectile * Time.deltaTime);
    }
}
