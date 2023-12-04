using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject[] panels;
    BGM bgm;
    [SerializeField]
    Sprite[] musicSp,soundSp;
    [SerializeField]
    Image music, sound;

    [SerializeField]
    AudioSource buttonSfx;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 1);
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music",1);
    }
    void Start()
    {
        bgm = FindObjectOfType<BGM>();
        GetButtons();
        if (!PlayerPrefs.HasKey("Health"))
            PlayerPrefs.SetInt("Health", 5);

        GetHealth();

        adm = GetComponent<ADManager>();
        adm.LoadRewardedAd();
    }


    public void PanelOpener(GameObject panel)
    {
        PlayButtonSound();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panel.SetActive(true);
    }

    void GetButtons()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            music.sprite = musicSp[1];
            bgm.GetComponent<AudioSource>().volume = 1;
        }
        else
        {
            music.sprite = musicSp[0];
            bgm.GetComponent<AudioSource>().volume = 0;
        }

        if (PlayerPrefs.GetInt("Sound") == 1)
            sound.sprite = soundSp[1];
        else
            sound.sprite = soundSp[0];
    }

    public void SetButtons(string which)
    {
        PlayButtonSound();
        if (which == "Music")
        {
            bgm.SetVolume();
        }
        else
            PlayerPrefs.SetInt(which, PlayerPrefs.GetInt(which) == 1 ? 0 : 1);

        GetButtons();
    }


    public GameObject loadingScreen;
    public Image loadingBar;

    public void LoadGame()
    {
        PlayButtonSound();
        if (PlayerPrefs.GetInt("Health") == 0)
        {
            healthPanel.SetActive(true);
            return;
        }
        PlayerPrefs.SetInt("Health", PlayerPrefs.GetInt("Health") - 1);
        PlayerPrefs.Save();
        StartCoroutine(LoadSceneAsync());
    }

    public GameObject healthPanel;
    IEnumerator LoadSceneAsync()
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        while (!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public TextMeshProUGUI healthTxt;
    public TextMeshProUGUI watchTxt;
    public void GetHealth()
    {
        healthTxt.text = PlayerPrefs.GetInt("Health").ToString();
        if (PlayerPrefs.GetInt("Health") == 5)
        {
            watchTxt.text = "MAX HEALTH";
        }
    }

    ADManager adm;

    public void HealthUpgrade()
    {
        PlayerPrefs.SetInt("Health", PlayerPrefs.GetInt("Health") + 1);
        GetHealth();
        adm.LoadRewardedAd();
    }

    public void HealthButton()
    {
        PlayButtonSound();
        if (PlayerPrefs.GetInt("Health") < 5)
        {
            adm.ShowRewardedAd("Health");
        }
    }
    void PlayButtonSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            buttonSfx.Play();
        }
    }
}
