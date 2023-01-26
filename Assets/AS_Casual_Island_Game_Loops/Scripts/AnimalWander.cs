using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Animal Wander will create a random point and move the object to it.
/// </summary>
public class AnimalWander : MonoBehaviour
{
    [Header("Animal Wander Settings")]
    [Space(5)]
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;
 
    private NavMeshAgent agent;
    private float timer;
    private Animator anim;
 
    private void OnEnable () {
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
        anim = GetComponent<Animator>();
    }
 
    private void Update () {
        timer += Time.deltaTime;
        if (timer >= wanderTimer) {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            anim.ResetTrigger("Turn Head");
            anim.SetTrigger("Walk");
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    anim.ResetTrigger("Walk");
                    anim.SetTrigger("Turn Head");
                }
            }
        }
    }
 
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

}
