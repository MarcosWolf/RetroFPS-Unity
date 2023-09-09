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

    public float maxAccuracy = 0.7f;
    public float minAccuracy = 0.2f;

    //public bool hasMeleeAttack;

    public float meleeAttackDistance;
    public float rangedAttackDistance;
    public float chasePlayerDistance;
    public float nearbySfxDistance;
    public float attackDelay;

    public int rangedAttackDamage;
    public int meleeAttackDamage;

    public bool onChase;

    public bool isAlive;
    public bool isIdle;
    public bool isAttacking = false;
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
                // Calcule a porcentagem de visibilidade do jogador
                float visibilityPercentage = CalculateVisibilityPercentage(hit.collider);

                if (visibilityPercentage >= 0.5f) // Se a visibilidade for maior ou igual a 50%
                {                    
                    if (!sfxSightPlayed) {
                        if (enemySfx.enemySight.Length > 0)
                        {
                            int randomValue = Random.Range(0, enemySfx.enemySight.Length);
                            enemySfx.enemySight[randomValue].Play();
                            sfxSightPlayed = true;
                        }
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
        else
        {
            sfxSightPlayed = false;
            CancelInvoke(nameof(rangedAttackPlayer));
            return false;
        }
    }

    // Calcula precisão do tiro, para inimigos que não usam projeteis
    private bool calculateAccuracy()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerControl.instance.transform.position);
        float currentAccuracy = Mathf.Lerp(maxAccuracy, minAccuracy, distanceToPlayer / rangedAttackDistance);
        
        if (Random.value < currentAccuracy) {
            return true;
        } else {
            return false;
        }
    }

    float CalculateVisibilityPercentage(Collider2D playerCollider)
    {
        // Converta a posição do inimigo e o centro do collider do jogador para Vector2
        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerCenter = playerCollider.bounds.center;

        // Calcule o vetor do inimigo até o centro do collider do jogador
        Vector2 enemyToPlayer = playerCenter - enemyPosition;

        // Calcule o tamanho do raio de detecção do inimigo
        float detectionRadius = enemyToPlayer.magnitude;

        // Calcule a área de interseção entre o raio de detecção e o collider do jogador
        float overlapArea = Mathf.PI * detectionRadius * detectionRadius;

        // Calcule a área total do collider do jogador
        float playerArea = playerCollider.bounds.size.x * playerCollider.bounds.size.y;

        // Calcule a porcentagem de visibilidade
        float visibilityPercentage = overlapArea / playerArea;

        return visibilityPercentage;
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

                        if (isAttacking) {
                            rb.velocity = Vector2.zero;
                        } else {
                            rb.velocity = moveDirection * enemySpeed;
                            enemyAnimator.SetTrigger("EnemyWalk");
                        }
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

    private void resumeWalking()
    {
        isAttacking = false;
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
                    if (enemyName == "Imp" || enemyName == "Demon") {
                        meleeAttackPlayer();
                    }
                }

                
                if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < nearbySfxDistance)
                {
                    // Colocar delay
                        //enemySfx.enemyNearby.Play();
                        //Debug.Log("Nearby");
                }
                

                if (Vector3.Distance(transform.position, PlayerControl.instance.transform.position) < rangedAttackDistance )
                {
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
            Invoke("resumeWalking", 0.4f);

            if (useProjectile) {
                isAttacking = true;
                enemyAnimator.SetTrigger("EnemyAttack");
                Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation);
                enemySfx.enemyAttackRanged.Play();
                hasAttacked = true;

                Invoke(nameof(resetEnemyAttack), attackDelay);
            } else {
                isAttacking = true;
                enemyAnimator.SetTrigger("EnemyAttack");
                enemySfx.enemyAttackRanged.Play();

                if (calculateAccuracy())
                {
                    PlayerControl.instance.HitPlayer(rangedAttackDamage);
                }

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
                enemySfx.enemyHit.Play();
                enemyAnimator.SetTrigger("EnemyHit");
            }

            if (currentEnemyHP <= 0)
            {
                isAlive = false;
                onChase = false;
                canMove = false;

                if (enemySfx.enemyDeath.Length > 0)
                {
                    int randomValue = Random.Range(0, enemySfx.enemyDeath.Length);
                    enemySfx.enemyDeath[randomValue].Play();
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
