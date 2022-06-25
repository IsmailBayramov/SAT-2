using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Printer_Text : MonoBehaviour
{
    private string text;
    public AudioClip impact;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //textPanel = GameObject.FindGameObjectWithTag("Panel");
        text = GetComponent<TextMeshProUGUI>().text;
        GetComponent<TextMeshProUGUI>().text = "";
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine()
    {
        foreach (char abc in text)
        {
            GetComponent<TextMeshProUGUI>().text += abc;
            yield return new WaitForSeconds(0.03f);
            audioSource.PlayOneShot(impact);
        }
        yield return new WaitForSeconds(8);
        if(SceneManager.GetActiveScene().name == "History")
            SceneManager.LoadScene(2);
    }
}
