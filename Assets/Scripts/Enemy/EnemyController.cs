using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator;

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
    public bool isIdle;
    public bool canMove;
    public bool hasAttacked;

    public int totalEnemyHP;
    public int currentEnemyHP;

    public float intervalBetweenPath;
    public float intervalCurrentTime;

    private Collider[] enemyCollidersChildren, enemyCollidersParent;

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        isIdle = false;
        canMove = true;
        onChase = false;

        enemyCollidersChildren = GetComponentsInChildren<Collider>();
        enemyCollidersParent = GetComponentsInParent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlayerAlive)
        { 
            checkDistance();
            moveEnemy();
        }
    }

    private void moveEnemy()
    {
        if (isAlive)
        {
            if (canMove) {
                if (onChase) {
                    transform.position = Vector2.MoveTowards(transform.position, PlayerControl.instance.transform.position, enemySpeed * Time.deltaTime);
                    enemyAnimator.SetTrigger("EnemyWalk");
                }
                else {
                    transform.position = Vector2.MoveTowards(transform.position, enemyPath[currentPath].position, enemySpeed * Time.deltaTime);

                    if (transform.position.y != enemyPath[currentPath].position.y )
                    {
                        enemyAnimator.SetTrigger("EnemyWalk");
                    }

                    if (transform.position.y == enemyPath[currentPath].position.y )
                    {
                        enemyAnimator.SetTrigger("EnemyIdle");
                        idleEnemy();
                    }

                    if (currentPath == enemyPath.Length)
                    {
                        currentPath = 0;
                    }
                }
            }
        }
    }

    private void idleEnemy()
    {
        //canMove = false;
        enemyAnimator.SetTrigger("EnemyIdle");

        intervalCurrentTime -= Time.deltaTime;

        if (intervalCurrentTime <= 0)
        {
            canMove = true;
            isIdle = false;
            currentPath++;
            intervalCurrentTime = intervalBetweenPath;
            enemyAnimator.SetTrigger("EnemyWalk");
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
            enemyAnimator.SetTrigger("EnemyAttack");
            Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
            SoundEffects.instance.sfxImpRanged();
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
            
            if (currentEnemyHP > 0 ) {
                SoundEffects.instance.sfxImpHit();
                enemyAnimator.SetTrigger("EnemyHit");
            }

            if (currentEnemyHP <= 0)
            {
                isAlive = false;
                onChase = false;
                canMove = false;
                SoundEffects.instance.sfxImpDeath1();

                enemyAnimator.SetTrigger("EnemyDead");
                foreach(Collider collider in enemyCollidersChildren)
                {
                    collider.gameObject.SetActive(false);
                }

                foreach (Collider colliderParent in enemyCollidersParent)
                {
                    Debug.Log("OIII");
                    colliderParent.gameObject.SetActive(false);
                }
            }
        }
    }

}
