using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.XR;

public class HeliController : MonoBehaviour
{
    public enum EnemyState {Patrol, Chase, Attack};

    public EnemyState currentState;
    public GameObject player;
    private Transform playerTransform;
    public Transform missileSpawn;
    public Transform missile;

    private NavMeshAgent agent;
    public float recoverTime = 1.5f;
    public float patrolDistance;
    public float chaseDistance;
    //Set high enough to attack from distance
    public float attackDistance;
    private int health = 300;
    private int damageAmount = 20;
    public float missileSpeed;

    private UnityAction<float> damageListener;

    private void OnEnable()
    {
        damageListener = new UnityAction<float>(ApplyDamage);
        EventManager.StartListening("EnemyDamager", damageListener);
    }
    private void OnDisable()
    {
        EventManager.StopListening("EnemyDamager", damageListener);

    }
    public float Health
    { 
        get { return health; }
        set
        {
            health -= (int)value;
            if (health < 0)
            {
                StopAllCoroutines();
            }
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrol;
        ChangeState(EnemyState.Patrol);

    }
    void ChangeState(EnemyState state)
    {
        currentState = state;
        StopAllCoroutines();
        switch (state)
        {
            case EnemyState.Patrol:
                StartCoroutine(AI_Patrol());
                break;
            case EnemyState.Chase:
                StartCoroutine(AI_Chase());
                break;
            case EnemyState.Attack:
                StartCoroutine(AI_Attack());
                break;
            default:
                StartCoroutine(AI_Patrol());
                break;
        }
    }

    IEnumerator AI_Patrol()
    {
        while (true)
        {
            NavMeshHit hit;
            Vector3 randomPosition = patrolDistance * Random.insideUnitSphere;
            randomPosition.y = gameObject.transform.position.y;

            //Finding the random position within Nav Mesh
            NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas);
            agent.SetDestination(hit.position);
            Debug.Log(hit.position);

            if(Vector3.Distance(playerTransform.position, transform.position) < chaseDistance)
            {
                ChangeState(EnemyState.Chase);
            }

            yield return new WaitForSeconds(3f);        
        }
    }
    IEnumerator AI_Chase()
    {

        while (true)
        {
            agent.SetDestination(playerTransform.position);
            if (Vector3.Distance(playerTransform.position, transform.position) < attackDistance)
            {
                ChangeState(EnemyState.Attack);
            }
            else if (Vector3.Distance(playerTransform.position, transform.position) > chaseDistance)
            {
                ChangeState(EnemyState.Patrol);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator AI_Attack()
    {
        agent.SetDestination(gameObject.transform.position);
        float elapsedTime = 0f;

        Debug.Log("Attacking");
        while (true)
        {

            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0f; // keep upright
            transform.rotation = Quaternion.LookRotation(direction);

            //attacks after a cooldown
            if (elapsedTime > recoverTime)
            {
                elapsedTime = 0f;
                //Spawn Missile
                Transform instance = Instantiate(missile, missileSpawn.position, missile.transform.rotation);
                //Move Missile
                instance.GetComponent<Rigidbody>().AddForce(direction * missileSpeed);
            }   

            elapsedTime++;

            if (Vector3.Distance(playerTransform.position, transform.position) > attackDistance)
            {
                ChangeState(EnemyState.Chase);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void ApplyDamage(float amt)
    {
        Health = amt;
        Debug.Log("Enemy damaged! Health: " + Health);
    }
}
