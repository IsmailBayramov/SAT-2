using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float range = 15;
    public bool isShoot = false, isShooting = true;
    public Transform bulletSpawn;
    public ParticleSystem muzzleFlash, Bullets;
    public Animator anim;
    public AudioSource AS;
    public AudioClip boom;
    public FixedJoystick MoveJoystick;
    public Camera cam;
    private Vector2 nullVector = new Vector2(0,0);

    private void Update()
    {
        if (!MyScript.aiming)
        {
            if (!isShoot)
            {
                if (MoveJoystick.Direction != nullVector)
                {
                    setAnim(run: true);
                }
                else
                    setAnim(idle: true);
            }
            else
            {
                setAnim(shoot: true);
                Invoke("Shooting", 0.13f);
            }

            if (!isShooting)
                Invoke("Shoot", 0.1f);
        }
    }

    bool Shooting()
    {
        isShoot = false;
        return isShoot;
    }

    private void setAnim(bool shoot = false, bool idle = false, bool run = false)
    {
        anim.SetBool("shoot", shoot);
        anim.SetBool("idle", idle);
        anim.SetBool("run", run);
    }

    public void Shoot()
    {
        isShoot = true;
        AS.Play();
        muzzleFlash.Play();
        Bullets.Play();
        RaycastHit hit;

        Vector3 pos = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);

        if (Physics.Raycast(pos, cam.transform.forward, out hit, range))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<ZombieScript>().AddDamage(damage);
            }

            if (hit.transform.gameObject.CompareTag("bucket"))
            {
                AS.PlayOneShot(boom);
                hit.transform.GetComponent<Explosion>().ExplodeWithDelay();
            }
        }    
    }

    public void Shot(bool sh)
    {
        isShooting = sh;
    }
}
