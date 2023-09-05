using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator;

    public ImpSfx impSfx;

    public float enemySpeed;
    public Transform[] enemyPath; 
    public int currentPath;

    public Transform projectileOrigin;
    public GameObject projectile;

    public float meleeAttackDistance;
    public float rangedAttackDistance;
    public float chasePlayerDistance;
    public float attackDelay;

    public int meleeAttackDamage;

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

    bool canSeePlayer()
    {
        Vector3 directionToPlayer = PlayerControl.instance.transform.position - transform.position;
        RaycastHit Hit;

        int obstacleLayerMask = LayerMask.GetMask("Obstacle");

        if (Physics.Raycast(transform.position, directionToPlayer, out Hit, Mathf.Infinity, obstacleLayerMask))
        {
            return false;
        }

        return true;
    }

    private void moveEnemy()
    {
        if (isAlive)
        {
            if (canMove)
            {
                if (onChase && canSeePlayer())
                {
                    Vector3 targetPosition = PlayerControl.instance.transform.position;
                    targetPosition.z = -0.5f;
                    
                    Vector3 directionToPlayer = targetPosition - transform.position;

                    float distanceToPlayer = directionToPlayer.magnitude;

                    float stopDistance = 6f;

                    if (distanceToPlayer > stopDistance)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);    
                        enemyAnimator.SetTrigger("EnemyWalk");
                    }
                    else
                    {
                        //enemyAnimator.SetTrigger("EnemyIdle");
                        idleEnemy();
                    }
                    
                }
                else
                {
                    Vector3 targetPosition = enemyPath[currentPath].position;
                    targetPosition.z = -0.5f;
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);

                    if (transform.position.y != enemyPath[currentPath].position.y )
                    {
                        enemyAnimator.SetTrigger("EnemyWalk");
                    }

                    if (transform.position.y == enemyPath[currentPath].position.y )
                    {
                        //enemyAnimator.SetTrigger("EnemyIdle");
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

        if (!onChase)
        {
            if (intervalCurrentTime <= 0)
            {
                canMove = true;
                isIdle = false;
                currentPath++;
                intervalCurrentTime = intervalBetweenPath;
                enemyAnimator.SetTrigger("EnemyWalk");
            }
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
        if (!hasAttacked && isAlive && canSeePlayer())
        {
            enemyAnimator.SetTrigger("EnemyAttack");
            Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
            impSfx.impRanged.Play();

            hasAttacked = true;

            Invoke(nameof(resetEnemyAttack), attackDelay);
        }
    }

    private void meleeAttackPlayer()
    {
        if (!hasAttacked && isAlive && canSeePlayer())
        {
            enemyAnimator.SetTrigger("EnemyAttack");
            impSfx.impMelee.Play();
            PlayerControl.instance.HitPlayer(meleeAttackDamage);

            hasAttacked = true;

            Invoke(nameof(resetEnemyAttack), attackDelay);
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
                impSfx.impHit.Play();
                enemyAnimator.SetTrigger("EnemyHit");
            }

            if (currentEnemyHP <= 0)
            {
                isAlive = false;
                onChase = false;
                canMove = false;

                if (Random.Range(0, 2) == 0)
                {
                    impSfx.impDeath1.Play();
                } else {
                    impSfx.impDeath2.Play();
                }

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
