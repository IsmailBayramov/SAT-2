using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieScript : MonoBehaviour
{
    private GameObject _Player;
    public GameObject Bottle;
    public ParticleSystem blood;
    public NavMeshAgent agent;
    private Animator anim;
    private Transform tranformZombie;
    private bool isDie;
    public int health = 5, price_for_kill;
    [HideInInspector] public static int money;

    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("rocket");
        money = PlayerPrefs.GetInt("moneys");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        tranformZombie = GetComponent<Transform>();
        agent.SetDestination(_Player.transform.position);
        setAnim(run: true);
    }

    public void setAnim(bool attack = false, bool run = false, bool idle = false)
    {
        anim.SetBool("attack", attack);
        anim.SetBool("run", run);
        anim.SetBool("idle", idle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            agent.enabled = true;

            agent.SetDestination(_Player.transform.position);
        }
    }

    private void SpawnBottle()
    {
        Instantiate(Bottle, new Vector3(tranformZombie.position.x + 1, tranformZombie.position.y - 1, tranformZombie.position.z + 0.7f), Quaternion.identity);
    }

    public void AddDamage(int damage)
    {
        health -= damage;
        blood.Play();
        if (health <= 0 && !isDie)
        {
            isDie = true;
            money+= price_for_kill;//изменить на ценность зомби
            PlayerPrefs.SetInt("moneys", money);
            setAnim(idle: true);
            Invoke("SpawnBottle", 1.9f);
            Destroy(gameObject, 2f);
        }
    }
}
