using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
    private static readonly int Dying = Animator.StringToHash("Dying");
    private static readonly int GettingHit = Animator.StringToHash("GettingHit");
    private float health;
    public Animator ZombieAnimator;
    private NavMeshAgent agent;
    private Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkpoint;
    bool walkpointSet;
    private float walkpointRange = 15f;
    private bool patrolEligible = true;
    private float timeBetweenAttacks = 7f;
    private bool alreadyAttacked = false;
    
    public bool attackPlayerTag = false;
    public float level;
    
    private float sightRange = 15f, attackRage = 1.5f;
    private bool playerInSightRange, playerInAttackRange;
    private bool canScream = true;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private GameObject handBox;
    [SerializeField] private GameObject feetBox;

    public void ActivateHandBox()
    {
        handBox.SetActive(true);
    }

    public void DeactivateHandBox()
    {
        handBox.SetActive(false);    
    }
    public void ActivateFeetBox()
    {
        feetBox.SetActive(true);
    }

    public void DeactivateFeetBox()
    {
        feetBox.SetActive(false);
    }
    public void AttackingPlayerTag()
    {
        attackPlayerTag = true;
    }
    
    public void NotAttackingPlayerTag()
    {
        attackPlayerTag = false;
    }
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        whatIsGround = LayerMask.GetMask("whatIsGround");
        whatIsPlayer = LayerMask.GetMask("whatIsPlayer");
        ZombieAnimator = GetComponent<Animator>();
        agent.stoppingDistance = 1.5f;
        if (GameData.Instance.GameLevel.worldLevel == 1)
        {
            level = Mathf.Round(Random.Range(1f, 20f));
        } else if (GameData.Instance.GameLevel.worldLevel == 2)
        {
            level = Mathf.Round(Random.Range(21f, 45f));
        } else if (GameData.Instance.GameLevel.worldLevel == 3)
        {
            level = Mathf.Round(Random.Range(46f, 70f));
        } else if (GameData.Instance.GameLevel.worldLevel == 4)
        {
            level = Mathf.Round(Random.Range(71f, 90f));
        }
        
        // Health Zombie
        health = level * 200;
        healthSlider.maxValue = level * 200;
        healthSlider.value = level * 200;
        
        UpdateLevelColor(level);
    }
    
    public void UpdateLevelColor(float enemyLevel)
    {
        float playerLevel = (float) GameData.Instance.GamePlayerStats.level;
        float levelDifference = enemyLevel - playerLevel;

        if (levelDifference <= 0)
        {
            levelText.color = Color.green; 
        }
        else if (levelDifference <= 5)
        {
            levelText.color = new Color(1f, 0.65f, 0f); 
        }
        else if (levelDifference > 5)
        {
            levelText.color = Color.red; 
        }

        levelText.text = $"Lv. {enemyLevel}";
    }
    

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);
        Vector3 walkpointInitial = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkpointInitial, -transform.up, 2f, whatIsGround))
        {
            walkpointSet = true;
            walkpoint = walkpointInitial;
        }
    }


    private bool resetOnProgress = false;
    private float idleProgress = 0f;
    private void Patroling()
    {
        agent.isStopped = false;
        if (!walkpointSet && patrolEligible)
        {
            SearchWalkPoint();
        }
        
        if (walkpointSet && patrolEligible)
        {
            patrolEligible = false;
            ZombieAnimator.SetBool("Walk", true);
            ZombieAnimator.SetBool("Run", false);
            agent.SetDestination(walkpoint);
        }
        
        if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance + 0.1f))
        {
            walkpointSet = false;
            ZombieAnimator.SetBool("Walk", false);
            ZombieAnimator.SetBool("Run", false);
            if (resetOnProgress == false)
            {
                Invoke(nameof(ResetWalkEligibility), 45f);
                resetOnProgress = true;
            }

            idleProgress += Time.deltaTime; 
            if (canScream && idleProgress > 5f && Random.Range(0f, 1f) <= 0.5f)
            {
                idleProgress = 0f;
                canScream = false;
                agent.isStopped = true;
                ZombieAnimator.SetTrigger("Scream");
                Invoke(nameof(EnableAgentAfterDelay), 15f);
            }
        }
    }
    private void EnableAgentAfterDelay()
    {
        idleProgress = 0f;
        agent.isStopped = false;
        canScream = true;
    }

    private void ResetWalkEligibility()
    {
        resetOnProgress = false;
        patrolEligible = true;
        walkpointSet = false;
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        ZombieAnimator.SetBool("Run", true);
        ZombieAnimator.SetBool("Walk", false);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            Vector3 targetPosition = new Vector3(hit.position.x, transform.position.y, hit.position.z);

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPosition, path);
            
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(targetPosition);
            }
            else
            {
                Debug.Log("Zombie cant find path to player!!");
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * (agent.speed * Time.deltaTime);
                transform.LookAt(player.position);
            }
        }
        else
        {
            Debug.LogWarning("Player position not valid on NavMesh!");
        }
    }

    private void AttackPlayer()
    {
        agent.isStopped = true; 
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(targetPosition);
        transform.LookAt(targetPosition);
        ZombieAnimator.SetBool("Run", false);
        ZombieAnimator.SetBool("Walk", false);
        if (!alreadyAttacked)
        {
            if (Random.Range(0, 100) % 2 == 0)
            {
                ZombieAnimator.SetTrigger("Attack");
            }
            else
            {
                ZombieAnimator.SetTrigger("Kick");
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            agent.isStopped = false;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private bool isDead = false;
    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }
        health -= damage;
        ZombieAnimator.SetTrigger(GettingHit);
        DamagePopUpGenerator.instance.CreatePopUp(transform.position, damage.ToString(), Color.white);
        StartCoroutine(AnimateSlider(healthSlider, healthSlider.value, health, 0.5f));
        if (health <= 0)
        {
            isDead = true;
            ZombieAnimator.ResetTrigger("GettingHit");
            ZombieAnimator.SetTrigger("Dying");
            Invoke(nameof(DestroyEnemy), 5);
        }
    }
    private IEnumerator AnimateSlider(Slider slider, float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }

        slider.value = endValue; 
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
        InstantiateDrops.instance.InstantiateDrop(transform.position, "exp", (int) level);
    }

    private bool isChasing = false;
    void Update()
    {
        if (health <= 0)
        {
            return;
        }
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRage, whatIsPlayer);
        
        if (!playerInSightRange && !playerInAttackRange)
        {
            if (isChasing == true)
            {
                ZombieAnimator.SetBool("Walk", true);
                ZombieAnimator.SetBool("Run", false);
                isChasing = false;
            }
            agent.speed = 1f;
            agent.angularSpeed = 60f;
            Patroling();

        }
        if (playerInSightRange && !playerInAttackRange)
        {
            isChasing = true;
            agent.speed = 3f;
            agent.angularSpeed = 85f;
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }
}
