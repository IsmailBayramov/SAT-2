using UnityEngine;
using UnityEngine.UI;

public class ChangeToggle : MonoBehaviour
{
    public GameObject PostProcessing;
    public Toggle toggle;

    private void Start()
    {
        if (PlayerPrefs.GetInt("toggle") == 1)
        {
            toggle.isOn = true;
            PostProcessing.SetActive(true);
            print("yes");
        }
        else
        {
            toggle.isOn = false;
            PostProcessing.SetActive(false);
            print("no");
        }

        toggle.onValueChanged.AddListener(delegate { ChangeToggleValue(toggle); });
    }   

    private void ChangeToggleValue(Toggle toggleValue)
    {
        PostProcessing.SetActive(toggle.isOn);
        int i;
        if (toggle.isOn == true) i = 1;
        else i = 0;
        PlayerPrefs.SetInt("toggle", i);
    }
}
