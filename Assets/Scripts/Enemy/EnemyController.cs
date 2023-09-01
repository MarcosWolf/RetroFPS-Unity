using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float enemySpeed;
    public Transform[] enemyPath; 
    public int currentPath;

    public Transform projectileOrigin;
    public GameObject projectile;

    public float meleeAttackDistance;
    public float rangedAttackDistance;
    public float chasePlayerDistance;
    public float attackDelay;

    public bool onChase;

    public bool isAlive;
    public bool canMove;
    public bool hasAttacked;

    public int totalEnemyHP;
    public int currentEnemyHP;

    public float intervalBetweenPath;
    public float intervalCurrentTime;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        canMove = true;
        onChase = false;
    }

    // Update is called once per frame
    void Update()
    { 
        checkDistance();
        moveEnemy();
    }

    private void moveEnemy()
    {
        if (isAlive)
        {
            if (canMove && onChase) {
                transform.position = Vector2.MoveTowards(transform.position, PlayerControl.instance.transform.position, enemySpeed * Time.deltaTime);
            }
            else {
                transform.position = Vector2.MoveTowards(transform.position, enemyPath[currentPath].position, enemySpeed * Time.deltaTime);

                if (transform.position.y == enemyPath[currentPath].position.y )
                {
                    idleEnemy();
                }

                if (currentPath == enemyPath.Length)
                {
                    currentPath = 0;
                }
            }
        }
    }

    private void idleEnemy()
    {
        canMove = false;

        intervalCurrentTime -= Time.deltaTime;

        if (intervalCurrentTime <= 0)
        {
            canMove = true;
            currentPath++;
            intervalCurrentTime = intervalBetweenPath;
        }
    }

    private void checkDistance()
    {
        if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < chasePlayerDistance )
        {
            onChase = true;
                        
            if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < meleeAttackDistance )
            {
                meleeAttackPlayer();
            }

            if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < rangedAttackDistance )
            {
                rangedAttackPlayer();
            }
              
        }
        else
        {
            onChase = false;
        }        
    }

    private void rangedAttackPlayer()
    {
        if (!hasAttacked && isAlive)
        {
            Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
            hasAttacked = true;

            Invoke(nameof(resetEnemyAttack), attackDelay);
        }
    }

    private void meleeAttackPlayer()
    {
        if (isAlive) {
            Debug.Log("Atacando perto");
        }
    }

    private void resetEnemyAttack()
    {
        hasAttacked = false;
    }

    public void HitEnemy(int enemyDamageTaken)
    {
        if (isAlive)
        {
            currentEnemyHP -= enemyDamageTaken;

            if (currentEnemyHP <= 0)
            {
                isAlive = false;
                onChase = false;
                canMove = false;
                Debug.Log("Imp morto");
                //Animação de morte
                DestroyEnemy();
            }
        }
    }

    public void DestroyEnemy()
    {
        //Destroy(this.gameObject);
    }

}
