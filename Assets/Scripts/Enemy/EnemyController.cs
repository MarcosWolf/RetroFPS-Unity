using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator;
    public string enemyName;

    public EnemySfx enemySfx;
  
    public Rigidbody2D rb;
    public float enemySpeed;
    public Transform[] enemyPath; 
    public int currentPath;

    public Transform projectileOrigin;
    public GameObject projectile;
    public bool useProjectile;

    public float meleeAttackDistance;
    public float rangedAttackDistance;
    public float chasePlayerDistance;
    public float nearbySfxDistance;
    public float attackDelay;

    public int meleeAttackDamage;

    public bool onChase;

    public bool isAlive;
    public bool isIdle;
    public bool canMove;
    public bool hasAttacked;

    private bool sfxSightPlayed = false;
    private bool sfxNearbyPlayed = false;

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

        if (isAlive && hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!sfxSightPlayed) {
                    Debug.Log("Tocar som");
                    int randomValue = Random.Range(0, 2);

                    if (randomValue == 0)
                    {
                        enemySfx.sfxEnemySight1();
                    } else {
                        enemySfx.sfxEnemySight2();
                    }
                    sfxSightPlayed = true;
                }
                
                return true;
            }
            else
            {
                sfxSightPlayed = false;
                CancelInvoke(nameof(rangedAttackPlayer));
                return false;
            }
        }
        else
        {
            sfxSightPlayed = false;
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
                    directionToPlayer.z = -0.7f;

                    float distanceToPlayer = directionToPlayer.magnitude;

                    float stopDistance = 6f;

                    rb.velocity = Vector2.zero;

                    if (distanceToPlayer > stopDistance)
                    {
                        Vector2 moveDirection = directionToPlayer.normalized;

                        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1f, obstacleLayer);

                        if (hit.collider != null)
                        {
                            Vector2 avoidanceDirection = Vector2.Perpendicular(moveDirection);
                            moveDirection += avoidanceDirection * 1f;

                            Vector2 newPosition = (Vector2)transform.position + moveDirection * Time.deltaTime * enemySpeed;

                            RaycastHit2D newHit = Physics2D.Raycast(newPosition, moveDirection, 1f, obstacleLayer);

                            if (newHit.collider == null)
                            {
                                rb.MovePosition(newPosition);
                            }
                        }

                        rb.velocity = moveDirection * enemySpeed;
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

                    rb.velocity = Vector2.zero;                

                    Vector3 moveDirection = pathPosition - transform.position;
                    moveDirection.z = 0f;
                    
                    if (moveDirection.magnitude > 0.1f)
                    {
                        Vector2 moveVector = moveDirection.normalized * enemySpeed;
                        rb.velocity = moveVector;
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
                    if (enemyName == "Imp") {
                        meleeAttackPlayer();
                    }
                }

                if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < nearbySfxDistance)
                {
                    //if (!sfxNearbyPlayed) {
                        Debug.Log("GOGORO");
                        enemySfx.sfxEnemyNearby();
                        sfxNearbyPlayed = true;
                    //}
                }

                if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < rangedAttackDistance )
                {
                    sfxNearbyPlayed = false;
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
            if (useProjectile) {
                enemyAnimator.SetTrigger("EnemyAttack");
                Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);

                enemySfx.enemyAttackRanged.Play();

                hasAttacked = true;

                Invoke(nameof(resetEnemyAttack), attackDelay);
            }
        }
    }

    private void meleeAttackPlayer()
    {
        if (!hasAttacked && isAlive && canSeePlayer())
        {
            enemyAnimator.SetTrigger("EnemyAttack");
            enemySfx.enemyAttackMelee.Play();
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
                enemySfx.sfxEnemyHit();
                enemyAnimator.SetTrigger("EnemyHit");
            }

            if (currentEnemyHP <= 0)
            {
                isAlive = false;
                onChase = false;
                canMove = false;

                if (enemyName == "Imp")
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        enemySfx.enemyDeath1.Play();
                    } else {
                        enemySfx.enemyDeath2.Play();
                    }
                } else if (enemyName == "Zombieman")
                {
                    int randomValue = Random.Range(0, 3);

                    if (randomValue == 0)
                    {
                        enemySfx.enemyDeath1.Play();
                    } else if (randomValue == 1) {
                        enemySfx.enemyDeath2.Play();
                    } else {
                        enemySfx.enemyDeath3.Play();
                    }
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

    private void callSfxEnemyNearby()
    {
        if (enemySfx != null)
        {
            enemySfx.sfxEnemyNearby();
        }
    }
}
