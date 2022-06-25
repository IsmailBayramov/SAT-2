using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarningScript : MonoBehaviour
{
    public void StartVisible()
    {
        StartCoroutine(Visibling());
    }

    private IEnumerator Visibling()
    {
        for(float i = 0; i <= 1; i += 0.1f)
        {
            GetComponent<Image>().color = new Color(255, 255, 225, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.0f);
        for (float i = 1; i > -0.1f; i -= 0.1f)
        {
            GetComponent<Image>().color = new Color(255, 255, 225, i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartVisible2()
    {
        StartCoroutine(Visibling2());
    }

    private IEnumerator Visibling2()
    {
        for (float i = 0; i <= 1; i += 0.1f)
        {
            GetComponent<Image>().color = new Color(255, 255, 225, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
