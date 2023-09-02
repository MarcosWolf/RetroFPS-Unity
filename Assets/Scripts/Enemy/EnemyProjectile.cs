using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speedProjectile;
    public int damageProjectile;

    private Transform player;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        targetPosition = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemyProjectile();
    }

    private void MoveEnemyProjectile()
    {
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speedProjectile * Time.deltaTime;
        //transform.Translate(Vector3.forward * speedProjectile * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControl>().HitPlayer(damageProjectile);
            Debug.Log("Tomou dano de " + damageProjectile);
        }

    }
}
