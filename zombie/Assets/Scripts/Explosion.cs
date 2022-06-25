using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Radius, Force;
    public GameObject ExplosionEffect;
    private bool _explosionDone;

    public void ExplodeWithDelay()
    {
        if (_explosionDone) return;
        _explosionDone = true;
        Invoke("Explode", 0.2f);

    }

    private void Explode()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, Radius);

        for(int i = 0; i < overlappedColliders.Length; i++)
        {
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;

            if(rigidbody)
            {
                rigidbody.AddExplosionForce(Force, transform.position, Radius);

                Explosion explosion = rigidbody.GetComponent<Explosion>();
                ZombieScript zombieScript = rigidbody.GetComponent<ZombieScript>();

                if (zombieScript)
                    zombieScript.AddDamage(10);

                if(explosion)
                {
                    if(Vector3.Distance(transform.position, rigidbody.position) < Radius / 2f)
                    {
                        explosion.ExplodeWithDelay();
                    }
                } 
            }
        }

        Destroy(gameObject);
        Instantiate(ExplosionEffect, new Vector3(transform.position.x - 1.44f, 0, transform.position.z + 1.2f), Quaternion.identity);     
    }
}
