using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthByTime : MonoBehaviour
{
    int year, month, day, hour, min, sec;

    MainMenu menu;

    private void Awake()
    {
        if (FindObjectsOfType<HealthByTime>().Length > 1)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        DateTime now = DateTime.Now;

        if (!PlayerPrefs.HasKey("Year"))
            PlayerPrefs.SetInt("Year", now.Year);
        if (!PlayerPrefs.HasKey("Month"))
            PlayerPrefs.SetInt("Month", now.Month);
        if (!PlayerPrefs.HasKey("Day"))
            PlayerPrefs.SetInt("Day", now.Day);
        if (!PlayerPrefs.HasKey("Hour"))
            PlayerPrefs.SetInt("Hour", now.Hour);
        if (!PlayerPrefs.HasKey("Min"))
            PlayerPrefs.SetInt("Min", now.Minute);
        if (!PlayerPrefs.HasKey("Sec"))
            PlayerPrefs.SetInt("Sec", now.Second);
        GetDetails();
        StartCoroutine(HealthChecker());
    }

    void GetDetails()
    {
        year = PlayerPrefs.GetInt("Year");
        month = PlayerPrefs.GetInt("Month");
        day = PlayerPrefs.GetInt("Day");
        hour = PlayerPrefs.GetInt("Hour");
        min = PlayerPrefs.GetInt("Min");
        sec = PlayerPrefs.GetInt("Sec");

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (menu == null)
            {
                menu = FindObjectOfType<MainMenu>();
            }
            menu.GetHealth();
        }
    }

    IEnumerator HealthChecker()
    {
        CheckTime();
        yield return new WaitForSeconds(5);
        StartCoroutine(HealthChecker());
    }
    public void CheckTime()
    {
        DateTime old = new DateTime(year, month, day, hour, min, sec);
        DateTime now = DateTime.Now;

        int minPassed = (int)now.Subtract(old).TotalMinutes;

        if (PlayerPrefs.GetInt("Health") == 5)
        {
            PlayerPrefs.SetInt("Year", now.Year);
            PlayerPrefs.SetInt("Month", now.Month);
            PlayerPrefs.SetInt("Day", now.Day);
            PlayerPrefs.SetInt("Hour", now.Hour);
            PlayerPrefs.SetInt("Min", now.Minute);
            PlayerPrefs.SetInt("Sec", now.Second);
            GetDetails();
            return;
        }
        while (minPassed / 15 != 0 && PlayerPrefs.GetInt("Health") < 5)
        {
            Debug.Log(PlayerPrefs.GetInt("Health"));
            minPassed -= 15;
            PlayerPrefs.SetInt("Health", PlayerPrefs.GetInt("Health") + 1);
            old = old.AddMinutes(15);
        }
        PlayerPrefs.SetInt("Year", old.Year);
        PlayerPrefs.SetInt("Month", old.Month);
        PlayerPrefs.SetInt("Day", old.Day);
        PlayerPrefs.SetInt("Hour", old.Hour);
        PlayerPrefs.SetInt("Min", old.Minute);
        PlayerPrefs.SetInt("Sec", old.Second);
        GetDetails();
    }
    //private void OnApplicationQuit()
    //{
    //    PlayerPrefs.DeleteAll();
    //    Debug.Log("Quit");
    //}
}
