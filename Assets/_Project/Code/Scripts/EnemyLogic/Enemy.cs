using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public float speed = 2.5f;
    public float chaseRange = 10.0f;
    public float deathRange = 1.5f;
    public AudioClip chaseSound;

    private NavMeshAgent agent;
    private AudioSource audioSource;
    private Transform player;
    private PlayerRespawn playerResp;
    private float chaseTime = 10f;
    private float restTime = 15f;
    private float timer = 0f;
    private enum STATES:int{
        WALK = 1,
        CHASE = 2,
        REST = 3
    }
    private STATES currentState = STATES.WALK;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        playerResp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRespawn>();
    }

    private void Update() {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (currentState == STATES.REST) {
            timer += Time.deltaTime;
            if (timer > restTime) {
                currentState = STATES.WALK;
                timer = 0f;
            }
        } else {
            if (distanceToPlayer <= chaseRange) {
                if (distanceToPlayer <= deathRange) {
                    playerResp.Respawn();
                } else {
                    timer += Time.deltaTime;
                    if (timer > chaseTime) {
                        currentState = STATES.REST;
                        timer = 0f;
                    } else {
                        if (currentState != STATES.CHASE) {
                            audioSource.PlayOneShot(chaseSound);
                            currentState = STATES.CHASE;
                        }
                    }
                }
            } else {currentState = STATES.WALK;}
        }
        switch (currentState) {
            case STATES.CHASE:
                ChasePlayer();
                break;
            case STATES.WALK:
                Patrol();
                break;
            default:
                break;
        }
    }

    private void ChasePlayer() {
        agent.SetDestination(player.position);
        agent.speed = speed * 1.5f;
    }

    private void Patrol() {
        // Create a list of waypoints on the NavMesh
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, GetRandomWaypoint(), NavMesh.AllAreas, path);

        // Follow the path
        agent.SetPath(path);
        agent.speed = speed;
    }

    private Vector3 GetRandomWaypoint() {
        // Get a random point on the NavMesh
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
        int randomIndex = Random.Range(0, triangulation.indices.Length);
        Vector3 randomPoint = triangulation.vertices[triangulation.indices[randomIndex]];
        return randomPoint;
    }
}
