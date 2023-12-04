using UnityEngine;

public class BGM : MonoBehaviour
{
    AudioSource music;
    public int vol;
    void Awake()
    {
        if (FindObjectsOfType<BGM>().Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        music = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Music"))
            vol = PlayerPrefs.GetInt("Music");
        else
            PlayerPrefs.SetInt("Music", 1);


        GetVolume();
    }

    void GetVolume()
    {
        if (vol == 1)
            music.volume = 1;
        else
            music.volume = 0;
    }
    public void SetVolume()
    {
        vol = ((vol == 1)? 0 : 1);
        PlayerPrefs.SetInt("Music", vol);
        GetVolume();
    }

}
