using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    public int health;
    public Slider healthText;
    public GameObject wd;
    private bool isWarning;

    private void Start() => healthText.value = health;

    private void OnCollisionEnter(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(ToDamage());
        }
        if(!enemy.gameObject.GetComponent<ZombieScript>())
            StopAllCoroutines();
    }

    private void OnCollisionExit(Collision enemy)
    {
        if (enemy.gameObject.CompareTag("Enemy"))
            StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieScript zs = other.gameObject.GetComponent<ZombieScript>();
        if (zs.health > 0)
        {
            zs.agent.enabled = false;
            zs.setAnim(attack: true);
            other.gameObject.isStatic = true;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        ZombieScript zs = other.gameObject.GetComponent<ZombieScript>();
        zs.agent.enabled = true;
        zs.agent.SetDestination(transform.position);
        zs.setAnim(run: true);
    }*/

    private IEnumerator ToDamage()
    {
        while (health > 0)
        {
            health -= 1;
            healthText.value = health;
            if (health < 4 && !isWarning)
            {
                isWarning = true;
                wd.GetComponent<WarningScript>().StartVisible2();
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
}
