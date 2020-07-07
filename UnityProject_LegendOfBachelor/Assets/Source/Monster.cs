using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Monster : MonoBehaviour
{
    [Header("Monster Stats")]
    public Monstertype thisMonsterType;
    public List<Quiz.QuestionType> questionTypes = new List<Quiz.QuestionType>();
    public float maxHealth = 20f;
    public float currentHealth;

    public float attackPoints = 10f;
    public int monsterReward = 10;


    [Header("Behaviour Setttings")]
    public float spottingRadius = 10f;

    public float wanderingSpeed = 2.5f;
    public float attackingSpeed = 3.5f;
    public float fleeingSpeed = 7.5f;


    [Header("System Vars")]
    public AiState currentState = AiState.Wandering;
    private AiState lastState = AiState.Wandering;
    public bool playerInRange = false;
    public HealthBar monsterHealthBar;
    private GameObject player;
    private int layerMask = 1 << 8;
    private NavMeshAgent agent;
    private Collider[] inRange;
    public bool killswitch = false;


    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        agent = GetComponent<NavMeshAgent>();
        WanderingOrigin = transform.position;

        currentHealth = maxHealth;
        monsterHealthBar = LobUtilities.FindAllMonoBehaviourOfTypeExpensive<HealthBar>().First();//.Find(x => x.name == "MonsterHealthBar").GetComponent<HealthBar>(); -- Problems to be solved
        
        //Subscribtions
        LOBEvents.current.onPause += Pause;
        LOBEvents.current.onResume += Resume;


        // setup question Types
        switch (thisMonsterType)
        {
            case Monstertype.Apretice:
                questionTypes.Add(Quiz.QuestionType.Scrum);
                break;

            case Monstertype.Bug:
                questionTypes.Add(Quiz.QuestionType.WModell);
                break;

            case Monstertype.Duck:
                questionTypes.Add(Quiz.QuestionType.Allgemein);
                break;

            case Monstertype.Spaghetti:
                questionTypes.Add(Quiz.QuestionType.VModell);
                break;
            case Monstertype.Boss:
                questionTypes.Add(Quiz.QuestionType.WModell);
                questionTypes.Add(Quiz.QuestionType.Allgemein);
                questionTypes.Add(Quiz.QuestionType.VModell);
                questionTypes.Add(Quiz.QuestionType.Scrum);
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        monsterHealthBar.SetHealth(currentHealth);
    }

    private void Pause () 
    {
        lastState = currentState;
        currentState = AiState.Pause;
    }
    private void Resume()
    {
        currentState = lastState;
    }
    

    void FixedUpdate()
    {
        if(currentHealth <= 0 || killswitch)
        {
            player.GetComponent<Player>().attackingMonsters.Remove(this);
            player.GetComponent<Player>().inventory.money += monsterReward;

            LOBEvents.current.MonsterDeath(this);
            Destroy(gameObject);
        }
        //Debug.Log(paused + " :: " + (currentState == AiState.Fighting));
        switch (currentState)
        {
            case AiState.Wandering:
                agent.speed = wanderingSpeed;
                Wander();

                if (CheckForPlayer())
                {
                    currentState = AiState.Attacking;
                    LOBEvents.current.PlayerSpoted(this);
                }
                break;

            case AiState.Attacking:
                agent.speed = attackingSpeed;
                agent.SetDestination(player.transform.position);

                if (!CheckForPlayer())
                {
                    currentState = AiState.Wandering;
                    WanderingOrigin = transform.position;
                }
                break;

            case AiState.Fighting:
                monsterHealthBar.SetMaxHealth(maxHealth);
                monsterHealthBar.SetHealth(currentHealth);
                agent.SetDestination(transform.position);
                Fighting();
                break;

            case AiState.Fleeing:
                agent.speed = fleeingSpeed;
                agent.SetDestination(transform.position + (transform.position - player.transform.position));
                break;

            case AiState.Pause:
                break;
        }
    }


    private bool CheckForPlayer()
    {
        // Player in Range ?
        inRange = Physics.OverlapSphere(transform.position, spottingRadius, layerMask);
        return inRange.Length > 0;
    }


    [Header("Behaviour Atributes")]
    #region wandering
    public float timeToWander = 3f;
    public float timeToWanderVariance = 1f;
    public float wanderingRadius = 5f;
    private float timeLastWandered = 0f;
    private float thisWanderTime = 0f;
    private Vector3 WanderingOrigin;
    private void Wander()
    {
        if(Time.time > timeLastWandered + thisWanderTime)
        {
            timeLastWandered = Time.time;
            thisWanderTime = timeToWander + Random.Range(-timeToWanderVariance * 0.5f, timeToWanderVariance * 0.5f);

            Vector2 wanderPos = Random.insideUnitCircle* wanderingRadius;
            agent.SetDestination(new Vector3(WanderingOrigin.x + wanderPos.x, WanderingOrigin.y, WanderingOrigin.z + wanderPos.y));
            agent.speed = 2.5f;
        }
    }
    #endregion

    #region Flee From City

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag + " : " + "StadtWall");
        if (other.gameObject.tag == "StadtWall")
        {
            Debug.Log("Fleeing");
            currentState = AiState.Fleeing;

            StartCoroutine(returnToWandering());
        }
    }

    private IEnumerator returnToWandering()
    {
        yield return new WaitForSeconds(5);
        currentState = AiState.Wandering;
        WanderingOrigin = transform.position;
    }
    #endregion

    #region fighting
    public float timeToAttack = 2f;
    public float timeToAttackVariance = 1f;
    private float timeLastAttack = 0f;
    private float thisTimeToAttack = 0f;
    private void Fighting()
    {
        //if(Time.time > timeLastAttack + thisTimeToAttack)
        //{
        //    timeLastAttack = Time.time;
        //    thisTimeToAttack = timeToAttack + Random.Range(-timeToAttackVariance * 0.5f, timeToAttackVariance * 0.5f);

        //    player.GetComponent<Player>().TakeDamage((int)attackPoints);

        //}
    }
    #endregion
    public enum AiState
    {
        Wandering,
        Attacking,
        Fighting,
        Fleeing,
        Pause
    }
    public enum Monstertype
    {
        Spaghetti,
        Bug,
        Duck,
        Apretice,
        Boss
    }
}
