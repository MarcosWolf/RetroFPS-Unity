using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator;

    public ImpSfx impSfx;

    public Rigidbody2D rb;
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

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private Collider[] enemyCollidersChildren;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        isAlive = true;
        isIdle = false;
        canMove = true;
        onChase = false;

        enemyCollidersChildren = GetComponentsInChildren<Collider>();
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
        Vector2 directionToPlayer = PlayerControl.instance.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, Mathf.Infinity, playerLayer | obstacleLayer);

        Debug.DrawRay(transform.position, directionToPlayer.normalized * (hit.collider != null ? hit.distance : 100f), Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                CancelInvoke(nameof(rangedAttackPlayer));
                return false;
            }
        }
        else
        {
            CancelInvoke(nameof(rangedAttackPlayer));
            return false;
        }
    }

    private void moveEnemy()
    {
        if (isAlive)
        {
            if (canSeePlayer())
            {
                if (onChase)
                {
                    Vector3 targetPosition = PlayerControl.instance.transform.position;
                    targetPosition.z = -0.7f;
                    
                    Vector3 directionToPlayer = targetPosition - transform.position;
                    targetPosition.z = transform.position.z;

                    float distanceToPlayer = directionToPlayer.magnitude;

                    float stopDistance = 6f;

                    rb.velocity = Vector2.zero;

                    if (distanceToPlayer > stopDistance)
                    {
                        Vector2 moveDirection = directionToPlayer.normalized;
                        rb.velocity = moveDirection * enemySpeed; // Aplique velocidade usando Rigidbody
                        //transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);
                        Debug.Log("Andando");    
                        enemyAnimator.SetTrigger("EnemyWalk");
                    }
                    else
                    {
                        idleEnemy();
                    }
                }
            }
            else
            {
                if (enemyPath.Length > 0)
                {
                    
                    Vector3 pathPosition = enemyPath[currentPath].position;

                    rb.velocity = Vector2.zero; // Pare o movimento atual
                    
                    //transform.position = Vector2.MoveTowards(transform.position, pathPosition, enemySpeed * Time.deltaTime);

                    Vector3 moveDirection = pathPosition - transform.position;
                    moveDirection.z = 0f;
                    
                    if (moveDirection.magnitude > 0.1f)
                    {
                        Vector2 moveVector = moveDirection.normalized * enemySpeed;
                        rb.velocity = moveVector; // Aplique velocidade usando Rigidbody
                        enemyAnimator.SetTrigger("EnemyWalk");
                    }
                    else {
                        idleEnemy();
                    }

                    if (currentPath == enemyPath.Length)
                    {
                        currentPath = 0;
                    }
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void idleEnemy()
    {
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
        if (canSeePlayer())
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
                    //rangedAttackPlayer();
                    Invoke(nameof(rangedAttackPlayer), 0.7f);
                }
                
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
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }
    }
