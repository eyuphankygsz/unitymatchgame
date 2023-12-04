using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool canClick;
    public bool gameOver;

    public bool cantCount;
    public int counter;
    public float timer;

    SelectBlock sBlock;
    Blocks selected;
    Camera cam;

    public TextMeshProUGUI seconds;

    public Transform gameArea;

    BGM bgm;


    [SerializeField]
    GameObject deathPanel;

    public AudioClip gameoverSfx, bg;

    ADManager adm;

    [SerializeField]
    AudioSource buttonSfx;

    void Start()
    {
        adm = GetComponent<ADManager>();
        adm.LoadRewardedAd();
        bgm = FindObjectOfType<BGM>();
        timer = 1;
        counter = 5;
        seconds.text = counter.ToString();
        cam = Camera.main;
        canClick = true;
        sBlock = GetComponent<SelectBlock>();
        sBlock.Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        SetGameScale(Screen.height, Screen.width);
        if (Input.GetMouseButtonDown(0) && canClick && !gameOver)
        {

            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                selected = hit.collider.GetComponent<Blocks>();

                if (selected.cantClick) return;
                if (hit.transform.parent.parent.name == "TOP") return;

                sBlock.SelectItem(selected.i, selected.j, selected.name);
                //print(string.Format("Selected i: {0}     j: {1}", selected.i, selected.j));
                StartCoroutine(ClickCheck());
            }
        }

        if (cantCount) return;

        if (timer > 0 && !gameOver)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0 && !gameOver)
        {
            timer = 1;
            counter--;
            seconds.text = counter.ToString();
        }

        if (counter == 0 && !gameOver)
        {
            gameOver = true;
            bgm.GetComponent<AudioSource>().clip = gameoverSfx;
            bgm.GetComponent<AudioSource>().Play();
            deathPanel.SetActive(true);

        }
    }
    public void ResetTime()
    {
        timer = 1;
        counter = 5;
        seconds.text = counter.ToString();
    }
    public IEnumerator ClickCheck()
    {
        yield return new WaitForSeconds(0.5f);
        if (gameOver) yield return null;
        canClick = true;
    }

    void SetGameScale(int height, int width)
    {
        if ((width <= 1080 && height <= 1920) || (width <= 720 && height <= 1280))
        {
            gameArea.position = new Vector3(0, -0.26f, 0);
            gameArea.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (width <= 1080 && height <= 2160)
        {
            gameArea.position = new Vector3(0, -0.17f, 0);
            gameArea.localScale = new Vector3(0.96f, 0.96f, 0.96f);
        }
        else if (width <= 1080 && height <= 3000)
        {
            gameArea.position = new Vector3(0, 0.17f, 0);
            gameArea.localScale = new Vector3(0.86f, 0.86f, 1f);
        }
        else if (width <= 1440 && height <= 2560)
        {
            gameArea.position = new Vector3(0, -0.28f, 0);
            gameArea.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (width <= 1440 && height <= 3500)
        {
            gameArea.position = new Vector3(0, 0, 0);
            gameArea.localScale = new Vector3(0.93f, 0.93f, 0.93f);
        }
    }




    public GameObject loadingScreen;
    public Image loadingBar;

    public void LoadGame(int scene)
    {
        PlayButtonSound();
        if (scene == 1)
        {
            if (PlayerPrefs.GetInt("Health") == 0)
            {
                scene = 0;
            }
            else
            {
                PlayerPrefs.SetInt("Health", PlayerPrefs.GetInt("Health") - 1);
            }
        }

        PlayerPrefs.Save();
        bgm.GetComponent<AudioSource>().clip = bg;
        bgm.GetComponent<AudioSource>().Play();
        StartCoroutine(LoadSceneAsync(scene));
    }
    IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        loadingScreen.SetActive(true);
        while (!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void ShowAd()
    {
        PlayButtonSound();
        adm.ShowRewardedAd("Continue");
    }
    public void ContinueGame()
    {
        ResetTime();
        cantCount = gameOver = false;
        canClick = true;
        bgm.GetComponent<AudioSource>().clip = bg;
        bgm.GetComponent<AudioSource>().Play();
        deathPanel.SetActive(false);
        adm.LoadRewardedAd();
    }
    void PlayButtonSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            buttonSfx.Play();
        }
    }
}