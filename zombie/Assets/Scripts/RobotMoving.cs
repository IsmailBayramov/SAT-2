using UnityEngine.AI;
using UnityEngine;

public class RobotMoving : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public AnimationClip move;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distance = agent.remainingDistance;
        Animator anim = GetComponent<Animator>();
        if (distance > agent.stoppingDistance)
        {
            agent.SetDestination(player.transform.position);
            anim.SetBool("isMove", true);
        }
        else
        {
            agent.ResetPath();
            anim.SetBool("isMove", true);
        }
    }
}
