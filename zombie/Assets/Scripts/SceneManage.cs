using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public GameObject panel;

    public void LoadTo(int i)
    {
        SceneManager.LoadScene(i);
        Time.timeScale = 1;
    }

    public void Restart(int i)
    {
        SceneManager.LoadScene(i);
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }

    public void OpenLinkVk(string link)
    {
        Application.OpenURL(link);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
