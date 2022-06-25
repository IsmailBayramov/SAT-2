using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class MyScript : MonoBehaviour
{
    public FixedJoystick MoveJoystick;
    public FixedButton JumpButton;
    public FixedTouchField TouchField, TouchField2, TouchField3, TouchField4;
    public int O2;
    public Button shootButton;
    public Slider O2Text, sensitivitySlider;
    public GameObject wd, deagle, revolver, m4a1, ak47, scar, sniper, p90, hands, shootButtonForP90, aim;
    private bool onPointerDown = true, onPointerDownJumpButton = true;
    public static bool aiming;
    private float sensitivityVolume = 0.2f;

    Collider objCollider;
    Camera cam;
    GameObject obj;
    Plane[] planes;

    RigidbodyFirstPersonController rb;

    private void Start()
    {
        O2Text.value = O2;
        StartCoroutine(O2Damage());
        sensitivityVolume = PlayerPrefs.GetFloat("sensitivity");
        sensitivitySlider.value = sensitivityVolume;
        rb = GetComponent<RigidbodyFirstPersonController>();
        ChangeGun();
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        objCollider = GetComponent<Collider>();
    }

    private void ChangeGun()
    {   
        if (PlayerPrefs.GetInt("P90") == 2000)
        {
            SetGun(p90);
            Destroy(m4a1);
            Destroy(ak47);
            Destroy(deagle);
            Destroy(scar);
            Destroy(sniper);
            Destroy(revolver);
            shootButtonForP90.SetActive(true);
            shootButton.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Sniper") == 1500)
        {
            SetGun(sniper);
            Destroy(m4a1);
            Destroy(ak47);
            Destroy(deagle);
            Destroy(p90);
            Destroy(scar);
            Destroy(revolver);
            aim.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Scar") == 1000)
        {
            SetGun(scar);
            Destroy(m4a1);
            Destroy(ak47);
            Destroy(deagle);
            Destroy(p90);
            Destroy(sniper);
            Destroy(revolver);
        }
        else if (PlayerPrefs.GetInt("AK-47") == 500)
        {
            SetGun(ak47);
            Destroy(m4a1);
            Destroy(scar);
            Destroy(deagle);
            Destroy(p90);
            Destroy(sniper);
            Destroy(revolver);
        }
        else if (PlayerPrefs.GetInt("M4A1") == 200)
        {
            SetGun(m4a1);
            Destroy(scar);
            Destroy(ak47);
            Destroy(deagle);
            Destroy(p90);
            Destroy(sniper);
            Destroy(revolver);
        }
        else if (PlayerPrefs.GetInt("Revolver") == 50)
        {
            SetGun(revolver);
            Destroy(m4a1);
            Destroy(ak47);
            Destroy(p90);
            Destroy(sniper);
            Destroy(deagle);
            Destroy(scar);
        }
        else
        {
            Destroy(m4a1);
            Destroy(ak47);
            Destroy(p90);
            Destroy(sniper);
            Destroy(revolver);
            Destroy(scar);
        }
    }

    private void Update()
    {
        var fps = rb;
        if (aiming) MouseLook.XSensitivity = sensitivityVolume / 5;
        else MouseLook.XSensitivity = sensitivityVolume;
        fps.RunAxis = MoveJoystick.Direction;
        fps.JumpAxis = JumpButton.Pressed;

        if(onPointerDownJumpButton)
        {
            if(onPointerDown)
            {
                fps.mouseLook.LookAxis = TouchField.TouchDist;
            }

            else
            {
                if(p90)
                {
                    rb.mouseLook.LookAxis = TouchField4.TouchDist;
                }
                else
                {
                    rb.mouseLook.LookAxis = TouchField2.TouchDist;
                }
            }
        }
        else if (onPointerDown && !onPointerDownJumpButton) rb.mouseLook.LookAxis = TouchField3.TouchDist;

        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Debug.Log(obj.name + " has been detected!");
        }
    }

    private void OnCollisionEnter(Collision enemy)
    {   
        if (enemy.gameObject.CompareTag("Food"))
        {
            Destroy(enemy.gameObject);
            if(O2 < O2Text.maxValue)
            {
                O2 += 1; //поменять 1 на ценность капсулы
                O2Text.value = O2;
            }  
        }      
    }

    public void SetVolume(float vol)
    {
        sensitivityVolume = vol;
        PlayerPrefs.SetFloat("sensitivity", vol);
    }

    public void OnPointer(bool pointerDown) => onPointerDown = pointerDown;

    public void OnPointerJumpButton(bool jumpPointerDown) => onPointerDownJumpButton = jumpPointerDown;

    public void Aiming()
    {
        aiming = !aiming;
        Animator anim = GetComponent<Animator>();
        if (aiming)
        {
            anim.enabled = false;
            Camera.main.fieldOfView = 20;
            if (m4a1) m4a1.transform.localPosition = new Vector3(0.052f, -0.14f, 1.167f);
            else if (sniper)
            {
                sniper.transform.localPosition = new Vector3(0.04f, -0.12f, 1.02f);
                aim.SetActive(true);
            }
            else if (p90) p90.transform.localPosition = new Vector3(0.049f, -0.09f, 1.12f);
            else if (scar) scar.transform.localPosition = new Vector3(-0.222f, -0.06f, 1.18f);
            else if (ak47) ak47.transform.localPosition = new Vector3(0.058f, -0.18f, 0.35f);
            hands.transform.localPosition = new Vector3(-0.09f, -0.29f, -0.5f);
            hands.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0)); ;
        }
        else
        {
            anim.enabled = true;
            Camera.main.fieldOfView = 60;
            if (m4a1) m4a1.transform.localPosition = new Vector3(0.052f, -0.09f, 1.167f);
            else if (sniper)
            { 
                sniper.transform.localPosition = new Vector3(0.036f, -0.056f, 1.24f);
                aim.SetActive(false);
            }
            else if (p90) p90.transform.localPosition = new Vector3(0.051f, -0.052f, 0.941f);
            else if (scar) scar.transform.localPosition = new Vector3(-0.222f, -0.024f, 1.18f);
            else if (ak47) ak47.transform.localPosition = new Vector3(0.058f, -0.14f, 0.35f);
            hands.transform.localPosition = new Vector3(0.092f, -0.366f, 0.04f);
        }
    }

    private void SetGun(GameObject name)
    {
        deagle.SetActive(false);
        revolver.SetActive(false);
        scar.SetActive(false);
        m4a1.SetActive(false);
        ak47.SetActive(false);
        name.SetActive(true);
        shootButton.onClick.AddListener(name.GetComponent<Bullet>().Shoot);
    }

    private IEnumerator O2Damage()
    {
        while (O2 > 0)
        {
            if(O2 < 4)
                wd.GetComponent<WarningScript>().StartVisible();
            O2 -= 1;
            O2Text.value = O2;
            yield return new WaitForSeconds(10.0f);
        }
    }
}
