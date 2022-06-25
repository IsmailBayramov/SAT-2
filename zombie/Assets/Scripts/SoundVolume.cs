using UnityEngine;
using UnityEngine.UI;

public class SoundVolume : MonoBehaviour
{
    private AudioSource audioSource;
    public Slider slider;
    private float musicVolume;
    [SerializeField] private string Tag;

    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(Tag);
        if(obj == null)
        {
            gameObject.tag = Tag;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        //DontDestroyOnLoad(gameObject);
        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        slider.value = musicVolume;
    }

    void Update()
    {
        audioSource.volume = musicVolume;
    }

    public void SetVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
}
